using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NS_OpenApiV3;
using NS_SnsNetWork;
using NS_SnsSigCheck;
using NS_SnsStat;
using System.Diagnostics;

public struct DEF
{
    public const int FLAG_CONFIRM = 0;
    public const int FLAG_TRY = 1;

    // 最多重试次数
    public const int MAX_TRY_COUNT = 3;

    // 确认成功
    public const int CONFIRM_RES_SUCCESS = 0;
    // 确认需要重试
    public const int CONFIRM_RES_TRY = 1;

    //结束
    public const int CONFIRM_RES_END = 2;

    public const string NEED_CONFIRM_QQ = "qqgame";
}

public class ConfirmData
{
    // db key
    public string m_key;

    // 重试次数
    public int m_tryCount;

    // 标志
    public int m_flag;

    // 上次重试的时间
    public DateTime m_lastTryTime;
}

/////////////////////////////////////////////////////////////////
public abstract class IConfirm
{
    // 从db加载数据
    public abstract void loadFromDb();

    // 每帧更新
    public abstract void update();

    // 由外部通知加载新数据
    public abstract void notifyLoad(string key);
}

public abstract class ConfirmBase<T> : IConfirm where T : ConfirmData, new()
{
    protected Dictionary<string, T> m_data = new Dictionary<string, T>();
    
    protected List<string> m_delKeys = new List<string>();

    public override void loadFromDb()
    {
        m_data.Clear();
        load(m_data);
    }

    // 加载数据，并且转换成T存储
    public abstract void load(Dictionary<string, T> data);

    // 可否确认
    public abstract bool canConfirm(T data);

    // 开始确认
    public abstract int startConfirm(T data);

    // 从db移除数据
    public abstract void removeFromDb(T data);

    // 加载新的数据
    public abstract T loadNewData(string key);

    public override void update()
    {
        m_delKeys.Clear();

        foreach (var d in m_data)
        {
            var data = d.Value;
            if (canConfirm(data))
            {
                int v = startConfirm(data);
                if (v == DEF.CONFIRM_RES_SUCCESS)
                {
                    removeFromDb(data);
                    m_delKeys.Add(d.Key);
                }
                else if (v == DEF.CONFIRM_RES_TRY)
                {
                    data.m_flag = DEF.CONFIRM_RES_TRY;
                    data.m_lastTryTime = DateTime.Now;
                    data.m_tryCount++;
                    if (data.m_tryCount > DEF.MAX_TRY_COUNT) // 重试次数到了，不再重试，直接失败
                    {
                        removeFromDb(data);
                        m_delKeys.Add(d.Key);
                    }
                }
                else if (v == DEF.CONFIRM_RES_END) // 不需要重试了
                {
                    removeFromDb(data);
                    m_delKeys.Add(d.Key);
                }
                else
                {
                    LogMgr.error("ConfirmBase, unknown confirm type {0}", v);
                }
            }
        }

        if (m_delKeys.Count > 0)
        {
            foreach (var k in m_delKeys)
            {
                m_data.Remove(k);
            }
            m_delKeys.Clear();
        }
    }

    public override void notifyLoad(string key)
    {
        T d = loadNewData(key);
        if (d != null)
        {
            m_data.Add(key, d);
        }
    }
}

/////////////////////////////////////////////////////////////////
public class QQGameData : ConfirmData
{
    public DateTime m_time;

    public string m_openid;
    public string m_openkey;
    public string m_appid;
    public string m_pf;

    public string m_ts;
    public string m_payitem;
    public string m_tokenId;
    public string m_billno;
    public string m_zoneid;
    public string m_provide_errno;
    public string m_amt;
    public string m_payment_coins;
}

public class QQGameConfirm : ConfirmBase<QQGameData>
{
    // 一次性从数据表加载全部数据，并且存储于data中
    public override void load(Dictionary<string, QQGameData> data)
    {
        List<Dictionary<string, object>> dataList = MongodbPayment.Instance.ExecuteGetAll(QQgameCFG.QQ_GAME_CONFIRM_TABLE);
        if (dataList != null && dataList.Count > 0) 
        {
            for (int i=0;  i < dataList.Count; i++) 
            {
                Dictionary<string, object> dataInfo = dataList[i];
                QQGameData da = new QQGameData();
                _load(dataInfo, da);
                data.Add(da.m_key, da);
            }
        }
    }

    public override bool canConfirm(QQGameData data)
    {
        if (data.m_flag == DEF.FLAG_CONFIRM) // 首次确认
        {
            TimeSpan span = DateTime.Now - data.m_time;
            if (span.TotalSeconds > 9.0)
            {
                return true;
            }
        }
        else if (data.m_flag == DEF.FLAG_TRY) // 重试
        {
            TimeSpan span = DateTime.Now - data.m_lastTryTime;
            if (span.TotalSeconds > 2.0)
            {
                return true;
            }
        }

        return false;
    }

    // 开始确认
    public override int startConfirm(QQGameData data)
    {
        try
        {
            Dictionary<string, string> reqData = new Dictionary<string, string>();
            reqData.Add("openid", data.m_openid);
            reqData.Add("openkey", data.m_openkey);
            reqData.Add("appid", data.m_appid);
            reqData.Add("pf", data.m_pf);

            reqData.Add("ts", data.m_ts);
            reqData.Add("payitem", data.m_payitem);
            reqData.Add("token_id", data.m_tokenId);
            reqData.Add("billno", data.m_billno);
            reqData.Add("zoneid", data.m_zoneid);
            reqData.Add("provide_errno", data.m_provide_errno);
            reqData.Add("amt", data.m_amt);
            reqData.Add("payamt_coins", data.m_payment_coins);

            // Q点直购   //本接口需要使用https协议访问
            OpenApiV3 sdk = new OpenApiV3(QQgameCFG.appid, QQgameCFG.appkey);
            sdk.SetServerName(QQgameCFG.SERVER_NAME_TEST);

            RstArray confirm_res = sdk.Api("/v3/pay/confirm_delivery", reqData, "https");
            Dictionary<string, object> tmpRet = JsonHelper.ParseFromStr<Dictionary<string, object>>(confirm_res.Msg);
            int ret = Convert.ToInt32(tmpRet["ret"]);
            switch (ret)
            {
                case 0: //成功
                    return DEF.CONFIRM_RES_SUCCESS;
                case 1062: //重新确认
                case 1099:
                    return DEF.CONFIRM_RES_TRY;
            }

            return DEF.CONFIRM_RES_END;
        }
        catch(Exception ex)
        {
            LogMgr.error(ex.ToString());
        }

        return DEF.CONFIRM_RES_TRY;
    }

    public override void removeFromDb(QQGameData data)
    {
        string orderId = data.m_key;
        MongodbPayment.Instance.ExecuteRemoveBykey(QQgameCFG.QQ_GAME_CONFIRM_TABLE, "orderid", orderId);
    }

    public override QQGameData loadNewData(string key)
    {
        if (m_data.ContainsKey(key))
            return null;

        Dictionary<string, object> data = MongodbPayment.Instance.ExecuteGetBykey(QQgameCFG.QQ_GAME_CONFIRM_TABLE, "orderid", key);
        if(data!=null)
        {
            QQGameData da = new QQGameData();
            _load(data, da);
        }

        return null;
    }

    void _load(Dictionary<string, object> dataInfo, QQGameData da)
    {
        if (dataInfo == null)
            return;

        if (dataInfo.ContainsKey("create_time"))
            da.m_time = Convert.ToDateTime(dataInfo["create_time"]).ToLocalTime();

        da.m_key = Convert.ToString(dataInfo["orderid"]);

        if (dataInfo.ContainsKey("openid"))
            da.m_openid = Convert.ToString(dataInfo["openid"]);

        if (dataInfo.ContainsKey("openkey"))
            da.m_openkey = Convert.ToString(dataInfo["openkey"]);

        if (dataInfo.ContainsKey("appid"))
            da.m_appid = Convert.ToString(dataInfo["appid"]);

        if (dataInfo.ContainsKey("pf"))
            da.m_pf = Convert.ToString(dataInfo["pf"]);

        if (dataInfo.ContainsKey("ts"))
            da.m_ts = Convert.ToString(dataInfo["ts"]);

        if (dataInfo.ContainsKey("payitem"))
            da.m_payitem = Convert.ToString(dataInfo["payitem"]);

        if (dataInfo.ContainsKey("token_id"))
            da.m_tokenId = Convert.ToString(dataInfo["token_id"]);

        if (dataInfo.ContainsKey("billno"))
            da.m_billno = Convert.ToString(dataInfo["billno"]);

        if (dataInfo.ContainsKey("zoneid"))
            da.m_zoneid = Convert.ToString(dataInfo["zoneid"]);

        if (dataInfo.ContainsKey("provide_errno"))
            da.m_provide_errno = Convert.ToString(dataInfo["provide_errno"]);

        if (dataInfo.ContainsKey("amt"))
            da.m_amt = Convert.ToString(dataInfo["amt"]);

        if (dataInfo.ContainsKey("payamt_coins"))
            da.m_payment_coins = Convert.ToString(dataInfo["payamt_coins"]);
    }
}

