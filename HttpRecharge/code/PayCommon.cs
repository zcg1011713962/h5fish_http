using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;

public class PayCommon
{
    public static void savePayinfo(string info, int amount)
    {
        if (string.IsNullOrEmpty(info) || amount <= 0)
            return;

        DateTime dt = DateTime.Now.Date;
        Dictionary<string, object> data = new Dictionary<string, object>();
        data[info] = (long)amount;
        data["total_rmb"] = (long)amount;

        MongodbPayment.Instance.ExecuteIncByQuery("pay_infos", Query.EQ("date", BsonValue.Create(dt)), data);
    }
}

////////////////////////////////////////////////////////////////////
public class PayCallbackBase
{
    public static string[] SPLIT_STR = new string[] { "?-?" };

    public static string[] S_TOTAL_PAY_FIELDS = { "PlayerId", "PayCode", "Account", "channel_number", "RMB" };

    static string CHECK_ORDER = ConfigurationManager.AppSettings["checkOrder"];

    // 支付回调，返回操作结果串
    public virtual string notifyPay(object param) { throw new Exception("not implement"); }

    /*
     *      插入支付数据到表内
     *      tableName   支付表名
     *      allData     所有要插入到表内的数据
     *      baseData    基础数据，要插入到充值总表里面
     */
    public bool insertPayData(string tableName, Dictionary<string, object> allData, PayInfoBase baseData)
    {
        bool res = false;
        if (!MongodbPayment.Instance.KeyExistsBykey(tableName, "OrderID", allData["OrderID"]))
        {
            if (baseData != null)
            {
                PayBase.updateDataToPaymentTotal(baseData, MongodbPayment.Instance);
            }
            if (MongodbPayment.Instance.ExecuteInsert(tableName, allData))
            {
                // 游戏支付log
                /*if (allData.ContainsKey("shoppage"))
                {
                    PayCommon.savePayinfo(Convert.ToString(allData["shoppage"]), Convert.ToInt32(allData["RMB"]));
                }
                else
                {
                    PayCommon.savePayinfo("lobby", Convert.ToInt32(allData["RMB"]));
                }*/
                
                // 渠道统计log
                Dictionary<string, object> savelog = new Dictionary<string, object>();
                savelog["acc"] = allData["Account"];
                if (allData.ContainsKey("real_Account"))
                {
                    savelog["real_acc"] = allData["real_Account"];
                }
                else
                {
                    savelog["real_acc"] = allData["Account"];
                }
                //记录设备号
                if (allData.ContainsKey("acc_dev"))
                {
                    savelog["acc_dev"] = allData["acc_dev"];
                }

                savelog["time"] = DateTime.Now;
                savelog["channel"] = allData["channel_number"];
                savelog["rmb"] = allData["RMB"];
                MongodbPayment.Instance.ExecuteInsert("PayLog", savelog);
                res = true;
                notify(baseData, tableName);
            }
        }
        else
        {
            res = true; // 订单已经存在了
        }
       
        return res;
    }

    /**
     *  插入临时表用于 confirm_delivery参数
     */
    public bool insertPayConfirmData(string tableName, Dictionary<string, object> inData)
    {
        bool res = false;
        if (!MongodbPayment.Instance.KeyExistsBykey(tableName, "orderid", inData["orderid"]))
        {
            if (MongodbPayment.Instance.ExecuteInsert(tableName, inData))
            {
                res = true;
            }
            else {
                res = false;
            }
        }
        else
        {
            res = true; // 订单已经存在了
        }
        return res;
    }


    void notify(PayInfoBase baseData, string payType)
    {
        try
        {
            if (!string.IsNullOrEmpty(CHECK_ORDER))
            {
                string url = string.Format(CHECK_ORDER, baseData.m_playerId, baseData.m_orderId, payType);
                var ret = HttpPost.Get(new Uri(url), req =>
                {
                    req.Timeout = 1200;
                });
            }
        }
        catch (System.Exception ex)
        {
            CLOG.Info(ex.ToString());
        }
    }

    // 总表中是否存在某订单
    public static bool existOrderInPaymentTotal(PayInfoBase baseData)
    {
        return PayBase.existOrderInPaymentTotal(baseData, MongodbPayment.Instance);
    }

    // 更新付费总表中的数据
    public static void updateDataToPaymentTotal(PayInfoBase baseData)
    {
        PayBase.updateDataToPaymentTotal(baseData, MongodbPayment.Instance);
    }

    public string[] splitInfo(string info)
    {
        string[] arr = info.Split(SPLIT_STR, StringSplitOptions.RemoveEmptyEntries);
        return arr;
    }

    // 返回待验签串，按照key升序排列，并且组成key=value&key=value..的形式
    // excludeKey内的key不验签
    public static string getWaitSignStrByAsc(Dictionary<string, object> data, List<string> excludeKey = null)
    {
        var descData = from s in data
                       orderby s.Key ascending
                       select s;

        string value = "";
        StringBuilder sbuilder = new StringBuilder();
        bool first = true;
        foreach (var d in descData)
        {
            if (excludeKey != null && excludeKey.Contains(d.Key))
                continue;

            if (d.Value == null)
            {
                value = "";
            }
            else
            {
                value = Convert.ToString(d.Value);
            }

            if (first)
            {
                first = false;
                sbuilder.AppendFormat("{0}={1}", d.Key, value);
            }
            else
            {
                sbuilder.AppendFormat("&{0}={1}", d.Key, value);
            }
        }

        return sbuilder.ToString();
    }

    // 除去&符号
    public static string getWaitSignStrByAsc2ExceptAnd(Dictionary<string, object> data, List<string> excludeKey = null)
    {
        var descData = from s in data
                       orderby s.Key ascending
                       select s;

        string value = "";
        StringBuilder sbuilder = new StringBuilder();
        foreach (var d in descData)
        {
            if (excludeKey != null && excludeKey.Contains(d.Key))
                continue;

            if (d.Value == null)
            {
                value = "";
            }
            else
            {
                value = Convert.ToString(d.Value);
            }

            sbuilder.AppendFormat("{0}={1}", d.Key, value);
        }

        return sbuilder.ToString();
    }

    // 通过rsa publickey进行验签
    public static bool checkSignByRSAPublicKey(string waitStr, string sign, string publicKey)
    {
        bool res = false;
        try
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);

                byte[] waitArr = Encoding.UTF8.GetBytes(waitStr);
                byte[] signArr = Convert.FromBase64String(sign);
                using (SHA1CryptoServiceProvider alg = new SHA1CryptoServiceProvider())
                {
                    res = rsa.VerifyData(waitArr, alg, signArr);
                }
            }
        }
        catch (System.Exception ex)
        {
            CLOG.Info("PayCallbackBase.checkSignByRSAPublicKey:{0}", ex.ToString());
        }

        return res;
    }

    public static bool checkSignByRSA256PublicKey(string waitStr, string sign, string publicKey)
    {
        bool res = false;
        try
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);

                byte[] waitArr = Encoding.UTF8.GetBytes(waitStr);
                byte[] signArr = Convert.FromBase64String(sign);
                using (SHA256Managed alg = new SHA256Managed())
                {
                    res = rsa.VerifyData(waitArr, alg, signArr);
                }
            }
        }
        catch (System.Exception ex)
        {
        }

        return res;
    }

    public static bool checkSignByRSA256PublicKey2(string waitStr, string sign, string publicKey)
    {
        bool res = false;
        try
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);

                byte[] waitArr = Encoding.UTF8.GetBytes(waitStr);
                byte[] signArr = Convert.FromBase64String(sign);
                using (SHA256CryptoServiceProvider alg = new SHA256CryptoServiceProvider())
                {
                    res = rsa.VerifyData(waitArr, alg, signArr);
                }
            }
        }
        catch (System.Exception ex)
        {
        }

        return res;
    }

    public static bool checkSignByRSAMd5PublicKey(string waitStr, string sign, string publicKey)
    {
        bool res = false;
        try
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);

                byte[] waitArr = Encoding.UTF8.GetBytes(waitStr);
                byte[] signArr = Convert.FromBase64String(sign);
                using (MD5CryptoServiceProvider alg = new MD5CryptoServiceProvider())
                {
                    res = rsa.VerifyData(waitArr, alg, signArr);
                }
            }
        }
        catch (System.Exception ex)
        {
        }

        return res;
    }

    public enum CheckRet
    {
        checkRetSuccess,
        checkRetNoOrder,
        checkRetRmbError,
        checkRetException,
    }

    public CheckRet checkOrder(string selfOrderId, int factRMB, PayInfoBase baseData = null)
    {
        try
        {
            Dictionary<string, object> qd = PayBase.queryBaseData(selfOrderId, MongodbPayment.Instance, PayCallbackBase.S_TOTAL_PAY_FIELDS);
            if (qd == null)
            {
                return CheckRet.checkRetNoOrder;
            }
            
            int srcRMB = Convert.ToInt32(qd["RMB"]);

            if (baseData != null)
            {
                baseData.m_payTime = DateTime.Now;
                baseData.m_orderId = selfOrderId;
                baseData.m_payCode = Convert.ToString(qd["PayCode"]);
                baseData.m_playerId = Convert.ToInt32(qd["PlayerId"]);
                baseData.m_account = Convert.ToString(qd["Account"]);
                baseData.m_rmb = srcRMB;
                baseData.m_channelNumber = Convert.ToString(qd["channel_number"]);
            }

            if (srcRMB != factRMB)
            {
                return CheckRet.checkRetRmbError;
            }

            return CheckRet.checkRetSuccess;
        }
        catch (System.Exception ex)
        {
            CLOG.Info("[{0}]", ex.ToString());
            return CheckRet.checkRetException;
        }
    }

    public CheckRet checkOrder(string selfOrderId, PayInfoBase baseData = null)
    {
        try
        {
            Dictionary<string, object> qd = PayBase.queryBaseData(selfOrderId, MongodbPayment.Instance, PayCallbackBase.S_TOTAL_PAY_FIELDS);
            if (qd == null)
            {
                return CheckRet.checkRetNoOrder;
            }

            int srcRMB = Convert.ToInt32(qd["RMB"]);

            if (baseData != null)
            {
                baseData.m_payTime = DateTime.Now;
                baseData.m_orderId = selfOrderId;
                baseData.m_payCode = Convert.ToString(qd["PayCode"]);
                baseData.m_playerId = Convert.ToInt32(qd["PlayerId"]);
                baseData.m_account = Convert.ToString(qd["Account"]);
                baseData.m_rmb = srcRMB;
                baseData.m_channelNumber = Convert.ToString(qd["channel_number"]);
            }
            return CheckRet.checkRetSuccess;
        }
        catch (System.Exception ex)
        {
            CLOG.Info("[{0}]", ex.ToString());
            return CheckRet.checkRetException;
        }
    }
}





