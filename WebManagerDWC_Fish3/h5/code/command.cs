using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Web.Configuration;
using System.Text;

//////////////////////////////////////////////////////////////////////////
public class CommandBase
{
    protected int m_opRes;

    public virtual object getResult(object param)
    {
        return null;
    }

    public virtual string execute(CMemoryBuffer buf)
    {
        return "error";
    }

    public int getOpRes()
    {
        return m_opRes;
    }

    public void setOpRes(int res)
    {
        m_opRes = res;
    }

    public static CMemoryBuffer createBuf()
    {
        CMemoryBuffer buf = new CMemoryBuffer();
        buf.begin();
        return buf;
    }
}

//////////////////////////////////////////////////////////////////////////
public class CommandRecvBenefit : CommandBase
{
    Dictionary<string, object> m_ret = new Dictionary<string, object>();

    public override object getResult(object param)
    {
        return null;
    }

    public override string execute(CMemoryBuffer buf)
    {
        m_ret.Clear();

        buf.begin();

        string op = buf.Reader.ReadString();
        string idstr = buf.Reader.ReadString();
        int playerId = 0;

        try
        {
            do
            {
                if (!int.TryParse(idstr, out playerId))
                {
                    m_ret.Add(RetResult.KEY_RESULT, RetResult.RET_PARAM_ERROR);
                    break;
                }

                PlayerActBenefit pdata = new PlayerActBenefit();
                pdata.Load(playerId);

                if (!pdata.IsLoad)
                {
                    m_ret.Add(RetResult.KEY_RESULT, RetResult.RET_NOT_FIND);
                    break;
                }

                if (op == "0") // 获取状态
                {
                    if (canRecv(pdata))
                    {
                        m_ret.Add(RetResult.KEY_RESULT, RetResult.RET_SUCCESS);
                    }
                    break;
                }
                else
                {
                    if (!canRecv(pdata))
                    {
                        break;
                    }
                }
                
                ParamSendMail param = new ParamSendMail();
                param.m_toPlayer = playerId.ToString();
                param.m_itemList = string.Format("1 {0}", 10000);
                param.m_sender = "系统";
                param.m_title = "周末爽翻天";
                param.m_content = "恭喜您成功领取周末福利：10000金币！";

                DyOpSendMail dy = new DyOpSendMail();
                int code = dy.doDyop(param);
                if (code == RetResult.RET_SUCCESS)
                {
                    addRecvData(playerId);
                }

                m_ret.Add(RetResult.KEY_RESULT, code);
            } while (false);
        }
        catch (System.Exception ex)
        {
        }

        return JsonHelper.genJson(m_ret);
    }

    bool canRecv(PlayerActBenefit pdata)
    {
        DayOfWeek week = DateTime.Now.DayOfWeek;
        if (!(week == DayOfWeek.Friday || week == DayOfWeek.Saturday || week == DayOfWeek.Sunday))
        {
            m_ret.Add(RetResult.KEY_RESULT, RetResult.RET_ACT_NOT_START);
            return false;
        }

        if (pdata.IsRecv)
        {
            m_ret.Add(RetResult.KEY_RESULT, RetResult.RET_HAS_RECEIVE);
            return false;
        }

        if (pdata.Gold > 0)
        {
            m_ret.Add(RetResult.KEY_RESULT, RetResult.RET_NOT_SATISFY_COND);
            return false;
        }

        if (pdata.RecvCount < CC.BENEFIT_RECV_COUNT)
        {
            m_ret.Add(RetResult.KEY_RESULT, RetResult.RET_NOT_SATISFY_COND);
            return false;
        }

        if (pdata.LastRecvDateTime != DateTime.Now.Date)
        {
            m_ret.Add(RetResult.KEY_RESULT, RetResult.RET_NOT_SATISFY_COND);
            return false;
        }

        return true;
    }

    void addRecvData(int playerId)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("recvDateTime", DateTime.Now.Date);
        data.Add("playerId", playerId);

        MongodbGame.Instance.ExecuteInsert(TableName.FISHLORD_ACT_BENEFIT_RECV, data);
    }
}

//////////////////////////////////////////////////////////////////////////
public class ParamSendMail
{
    public string m_title = "";
    public string m_sender = "";
    public string m_content = "";
    public string m_toPlayer = "";
    public string m_itemList = "";
    public string m_validDay = "";
    public int m_target;
    public bool m_isCheck = false;

    // 条件，下线时间
    public string m_condLogoutTime = "";
    // 条件，vip等级区间
    public string m_condVipLevel = "";

    public string m_comment = "";
    public string m_result = "";
}

public class ParamCheckMail : ParamSendMail
{
    public string m_id = "";
}

public class DyOpSendMail
{
    public int doDyop(object param)
    {
        ParamSendMail p = (ParamSendMail)param;

        int days = 7;
        List<int> playerList = new List<int>();
        List<ParamItem> tmpItem = new List<ParamItem>();
        int code = checkValid(p, ref days, tmpItem, playerList);
        if (code != RetResult.RET_SUCCESS)
            return code;

        if (p.m_target == 0) // 给指定玩家发送
        {
            BsonDocument mailItem = null;

            if (p.m_itemList != "")
            {
                Dictionary<string, object> dd = new Dictionary<string, object>();
                for (int i = 0; i < tmpItem.Count; i++)
                {
                    Dictionary<string, object> tmpd = new Dictionary<string, object>();
                    tmpd.Add("giftId", tmpItem[i].m_itemId);
                    tmpd.Add("count", tmpItem[i].m_itemCount);
                    tmpd.Add("receive", false);
                    dd.Add(i.ToString(), tmpd.ToBsonDocument());
                }
                mailItem = dd.ToBsonDocument();
            }

            return specialSend(p, days, mailItem, playerList[0]);
        }

        return RetResult.RET_SUCCESS;
    }

    private int specialSend(ParamSendMail p, int days, BsonDocument mailItem, int playerId)
    {
        bool res = false;
        DateTime now = DateTime.Now;
        DateTime nt = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

        List<Dictionary<string, object>> docList = new List<Dictionary<string, object>>();

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("title", p.m_title);
        data.Add("sender", p.m_sender);
        data.Add("content", p.m_content);

        data.Add("time", nt);
        data.Add("deadTime", nt.AddDays(days));
        data.Add("isReceive", false);
        data.Add("playerId", playerId);

        // 标识是系统发送的邮件
        data.Add("senderId", 0);
        data.Add("mainReason", (int)PropertyReasonType.type_reason_receive_gm_mail);

        if (mailItem != null)
        {
            data.Add("gifts", mailItem);
        }

        res = MongodbPlayer.Instance.ExecuteInsert(TableName.PLAYER_MAIL, data);

        return res ? RetResult.RET_SUCCESS : RetResult.RET_UNKONWN;
    }

    // 邮件的合法性检验
    private int checkValid(ParamSendMail p, ref int days, List<ParamItem> itemList, List<int> playerList)
    {
        if (!string.IsNullOrEmpty(p.m_validDay))
        {
            if (!int.TryParse(p.m_validDay, out days))
            {
                return RetResult.RET_PARAM_ERROR;
            }
        }

        if (p.m_itemList != "")
        {
            if (itemList != null)
            {
                bool res = Tool.parseItemList(p.m_itemList, itemList);
                if (!res)
                {
                    return RetResult.RET_PARAM_ERROR;
                }

                for (int i = 0; i < itemList.Count; i++)
                {
                    var t = ItemCFG.getInstance().getValue(itemList[i].m_itemId);
                    if (t == null)
                    {
                        p.m_result += itemList[i].m_itemId + " ";
                    }
                }

                if (p.m_result != "")
                    return RetResult.RET_NOT_FIND;
            }
            else
            {
                if (!Tool.isItemListValid(p.m_itemList, true))
                    return RetResult.RET_PARAM_ERROR;
            }
        }

        if (p.m_target == 0) // 给指定玩家
        {
            bool res = Tool.parseNumList(p.m_toPlayer, playerList);
            if (!res)
                return RetResult.RET_PARAM_ERROR;

            /*for (int i = 0; i < playerList.Count; i++)
            {
                res = DBMgr.getInstance().keyExists(TableName.PLAYER_INFO, "player_id", playerList[i], user.getDbServerID(), DbName.DB_PLAYER);
                if (!res)
                {
                    p.m_result += playerList[i] + " ";
                }
            }

            if (p.m_result != "")
                return OpRes.op_res_player_not_exist;

            if (p.m_condVipLevel != "")
                return OpRes.op_res_param_not_valid;

            if (p.m_condLogoutTime != "")
                return OpRes.op_res_time_format_error;
            */
        }
        else // 全服发放
        {
        
        }

        return RetResult.RET_SUCCESS;
    }
}

/////////////////////////////////////////////////////////////////////////
public class CommandNoticeList : CommandBase 
{
    Dictionary<string, object> m_ret = new Dictionary<string, object>();

    public override object getResult(object param)
    {
        return null;
    }

    public override string execute(CMemoryBuffer buf)
    {
        m_ret.Clear();
        buf.begin();
        try
        {
            do
            {
                List<noticeList> data = NoticeCache.load();
                if (data == null || data.Count == 0)
                {
                    m_ret.Add(RetResult.KEY_RESULT, RetResult.RET_NOT_FIND);
                }
                else 
                {
                    m_ret.Add(RetResult.KEY_RESULT, RetResult.RET_SUCCESS);

                    m_ret.Add("resList", data.ToJson());
                }
            } while (false);
        }
        catch (System.Exception ex)
        {
        }
        return JsonHelper.genJson(m_ret);
    }
}