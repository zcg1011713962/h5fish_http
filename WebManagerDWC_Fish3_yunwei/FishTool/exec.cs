using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

// exchange表，增加status字段
public class ExchangeTrans
{
    public void exec()
    {
        int skip = 0;
        int count = 1000;
        Dictionary<string, object> updata = new Dictionary<string, object>();

        do
        {
            IMongoQuery imq = Query.Null;
            List<Dictionary<string, object>> dataList = QueryTool.nextData<MongodbPlayer>(MongodbPlayer.Instance, TableName.EXCHANGE, imq, ref skip, count);
            if(dataList==null)
                break;

            for (int i = 0; i < dataList.Count; ++i)
            {
                Dictionary<string, object> data = dataList[i];
                if (!data.ContainsKey("isReceive") || !data.ContainsKey("playerId") || !data.ContainsKey("exchangeId"))
                {
                    LogMgr.log.InfoFormat("record {0} error", Convert.ToString(data["exchangeId"]));
                    continue;
                }

                updata.Clear();
                int playerId = Convert.ToInt32(data["playerId"]);
                bool isReceive = Convert.ToBoolean(data["isReceive"]);
                string exchangeId = Convert.ToString(data["exchangeId"]);

                if (isReceive)
                {
                    updata.Add("status", 4);
                }
                else
                {
                    updata.Add("status", 3);
                }

                string str = MongodbPlayer.Instance.ExecuteUpdate(TableName.EXCHANGE, "exchangeId", exchangeId, updata, UpdateFlags.None);
                if (!string.IsNullOrEmpty(str))
                {
                    LogMgr.log.ErrorFormat("record {0} set status error", Convert.ToString(data["exchangeId"]));
                }
            }

        } while (true);

        LogMgr.log.Info("complete-------------------");
    }
}

// 玩家的 TurretLevel 等级变换
public class TurretLevelTrans
{
    static string[] PLAYER_FIELDS = { "TurretLevel", "player_id", "_FishLevelChangeCount_" };
    const string MAIL_TITLE = "炮倍调整变动";
    const string MAIL_CONTENT = "由于本次更新把500炮倍至1000炮倍中间的50炮倍去除，由于您刚好在本次更新的50炮倍之间，我们把您的炮倍往上调整了一级，对应该等级的奖励请查收，谢谢您的支持，愿幸运常伴您左右。";

    public void exec()
    {
        int skip = 0;
        int count = 1000;
        Dictionary<string, object> updata = new Dictionary<string, object>();
        MailSys mailSys = new MailSys();

        do
        {
            IMongoQuery imq = Query.Null;
            List<Dictionary<string, object>> dataList = QueryTool.nextData<MongodbPlayer>(MongodbPlayer.Instance, TableName.PLAYER_INFO, imq, ref skip, count, PLAYER_FIELDS);
            if (dataList == null)
                break;

            for (int i = 0; i < dataList.Count; ++i)
            {
                Dictionary<string, object> data = dataList[i];
                if (!data.ContainsKey("player_id") || !data.ContainsKey("TurretLevel"))
                {
                   // LogMgr.log.InfoFormat("player {0} error", Convert.ToString(data["exchangeId"]));
                    continue;
                }

                int changeCount = 0;
                if (data.ContainsKey("_FishLevelChangeCount_"))
                {
                    changeCount = Convert.ToInt32(data["_FishLevelChangeCount_"]);
                }
                if (changeCount > 0)
                    continue;

                updata.Clear();
                int playerId = Convert.ToInt32(data["player_id"]);
                int turretLevel = Convert.ToInt32(data["TurretLevel"]);

                FishLevelUpdateInfo info = FishLevelUpdate.getInstance().getValue(turretLevel);
                if (info == null)
                    continue;

                int dstLevel = info.m_newLevel;
                updata.Add("TurretLevel", dstLevel);
                updata.Add("_FishLevelChangeCount_", 1);

                string str = MongodbPlayer.Instance.ExecuteUpdate(TableName.PLAYER_INFO, "player_id", playerId, updata, UpdateFlags.None);
                if (!string.IsNullOrEmpty(str))
                {
                    LogMgr.log.ErrorFormat("player {0} set trans error", playerId);
                }
                else
                {
                    sendMailReward(mailSys, playerId, info);
                    LogMgr.log.InfoFormat("player {0}, old level {1}, new level {2},  trans success", playerId, turretLevel, dstLevel);
                }
            }

        } while (true);

        LogMgr.log.Info("complete-------------------");
    }

    void sendMailReward(MailSys mailSys, int playerId, FishLevelUpdateInfo info)
    {
        if (info.m_reward.Count <= 0)
            return;

        List<ParamItem> itemList = new List<ParamItem>();

        for (int i = 0; i < info.m_reward.Count; i += 2)
        {
            ParamItem item = new ParamItem();
            itemList.Add(item);

            item.m_itemId = info.m_reward[i];
            item.m_itemCount = info.m_reward[i + 1];
        }
        mailSys.sendMail(playerId, "游戏运营团队", MAIL_TITLE, MAIL_CONTENT, itemList);
    }
}

//////////////////////////////////////////////////////////////////
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
    public bool m_isAdmin = false;

    // 条件，下线时间
    public string m_condLogoutTime = "";
    // 条件，vip等级区间
    public string m_condVipLevel = "";

    public string m_comment = "";
    public string m_result = "";
}

public class MailSys
{
    public const int RET_SUCCESS = 0;
    public const int RET_FAIL = 1;

    public int sendMail(int toPlayerId, string sender, string title, string content, List<ParamItem> itemList)
    {
        ParamSendMail paramMail = new ParamSendMail();
        paramMail.m_title = title;
        paramMail.m_sender = sender;
        paramMail.m_content = content;

        BsonDocument mailItem = null;

        Dictionary<string, object> dd = new Dictionary<string, object>();
        for (int i = 0; i < itemList.Count; i++)
        {
            Dictionary<string, object> tmpd = new Dictionary<string, object>();
            tmpd.Add("giftId", itemList[i].m_itemId);
            tmpd.Add("count", itemList[i].m_itemCount);
            tmpd.Add("receive", false);
            dd.Add(i.ToString(), tmpd.ToBsonDocument());
        }
        mailItem = dd.ToBsonDocument();
        
        return specialSend(paramMail, 31, mailItem, toPlayerId);
    }

    private int specialSend(ParamSendMail p, int days, BsonDocument mailItem, int playerId)
    {
        DateTime now = DateTime.Now;
        DateTime nt = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

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

        bool res = MongodbPlayer.Instance.ExecuteInsert(TableName.PLAYER_MAIL, data);
        return res ? RET_SUCCESS : RET_FAIL;
    }
}

//////////////////////////////////////////////////////////////////////////
// 并行任务字段删除  player_quest表 parallelQuest 字段
public class ParallelQuestTrans
{
    static string[] FIELDS = { "player_id", "parallelQuest" };

    public void exec()
    {
        int skip = 0;
        int count = 1000;
        Dictionary<string, object> updata = new Dictionary<string, object>();

        do
        {
            IMongoQuery imq = Query.Null;
            List<Dictionary<string, object>> dataList = QueryTool.nextData<MongodbPlayer>(MongodbPlayer.Instance, "player_quest", imq, ref skip, count, FIELDS);
            if (dataList == null)
                break;

            for (int i = 0; i < dataList.Count; ++i)
            {
                Dictionary<string, object> data = dataList[i];
                if (!data.ContainsKey("parallelQuest") || !data.ContainsKey("player_id") )
                {
                    //LogMgr.log.InfoFormat("record {0} error", Convert.ToString(data["exchangeId"]));
                    continue;
                }

                object[] arr = (object[])data["parallelQuest"];
                if (arr.Length <= 0)
                    continue;

                var obj = new BsonArray();
                updata.Clear();
                int playerId = Convert.ToInt32(data["player_id"]);
                updata.Add("parallelQuest", obj);

                string str = MongodbPlayer.Instance.ExecuteUpdate("player_quest", "player_id", playerId, updata, UpdateFlags.None);
                if (string.IsNullOrEmpty(str))
                {
                    LogMgr.log.InfoFormat("player {0} 并行任务删除成功", playerId);
                }
                else
                {
                    LogMgr.log.ErrorFormat("player {0} 并行任务删除失败", playerId);
                }
            }

        } while (true);

        LogMgr.log.Info("complete-------------------");
    }
}


