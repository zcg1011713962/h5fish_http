using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

public class PlayerActBenefit
{
    static string[] s_fieldGame = { "hasReceiveAlmsCount", "LastRecvBenefitDateTime" };

    static string[] s_fieldPlayer = { "gold" };

    public bool IsLoad { set; get; }

    public DateTime LastRecvDateTime { set; get; }

    public int RecvCount { set; get; }

    public bool IsRecv { set; get; }

    public long Gold { set; get; }

    public void Load(int playerId)
    {
        initDef();

        Dictionary<string, object> data = MongodbGame.Instance.ExecuteGetOneBykey(TableName.FISHLORD_PLAYER, "player_id", playerId, s_fieldGame);
        if (data == null)
        {
            return;
        }

        IsLoad = true;

        if (data.ContainsKey("hasReceiveAlmsCount"))
        {
            RecvCount = Convert.ToInt32(data["hasReceiveAlmsCount"]);
        }
        if (data.ContainsKey("LastRecvBenefitDateTime"))
        {
            LastRecvDateTime = Convert.ToDateTime(data["LastRecvBenefitDateTime"]).ToLocalTime().Date;
        }

        if (RecvCount < CC.BENEFIT_RECV_COUNT)
            return;

        IMongoQuery imq1 = Query.EQ("recvDateTime", DateTime.Now.Date);
        IMongoQuery imq2 = Query.EQ("playerId", playerId);
        IMongoQuery imq = Query.And(imq1, imq2);

        Dictionary<string, object> data2 = MongodbGame.Instance.ExecuteGetByQuery(TableName.FISHLORD_ACT_BENEFIT_RECV, imq);

        if (data2 != null && data2.ContainsKey("recvDateTime") && data2.ContainsKey("playerId"))
        {
            IsRecv = true;
        }

        if (IsRecv)
            return;

        Dictionary<string, object> data3 = MongodbPlayer.Instance.ExecuteGetOneBykey(TableName.PLAYER_INFO, "player_id", playerId, s_fieldPlayer);
        if (data3 != null && data3.ContainsKey("gold"))
        {
            Gold = Convert.ToInt64(data3["gold"]);
        }
    }

    void initDef()
    {
        IsLoad = false;
        LastRecvDateTime = DateTime.MinValue;
        RecvCount = 0;
        IsRecv = false;
        Gold = 10;
    }
}

//////////////////////////////////////////////////////////////////////
public class noticeList
{
    public string m_title = "";
    public string m_content = "";
}
public class ActNoticeList 
{
    List<noticeList> m_result = new List<noticeList>();

    public bool empty()
    {
        return m_result.Count == 0;
    }

    public void clearList()
    {
        m_result.Clear();
    }

    public List<noticeList> Load() 
    {
        m_result.Clear();
        DateTime time = DateTime.Now;
        IMongoQuery imq = Query.And(Query.LT("startTime",BsonValue.Create(time)),Query.GTE("deadTime",BsonValue.Create(time)));
        List<Dictionary<string, object>> dataList = MongodbPlayer.Instance.ExecuteGetListByQuery(TableName.OPERATION_NOTIFY,imq,null,"order");
        if (dataList != null && dataList.Count > 0)
        {
            List<Dictionary<string, object>> lists = dataList.OrderBy(a => a["genTime"]).OrderBy(a=>a["order"]).ToList();
            foreach (var data in lists)
            {
                noticeList tmp = new noticeList();
                m_result.Add(tmp);
                if (data.ContainsKey("title"))
                {
                    tmp.m_title = Convert.ToString(data["title"]);
                }

                if (data.ContainsKey("content"))
                {
                    tmp.m_content = Convert.ToString(data["content"]);
                }
            }
        }
        else 
        {
            return null;
        }
        return m_result;
    }

    public List<noticeList> getResult()
    {
        return m_result;
    }
}

public class NoticeCache
{
    static DateTime m_lastTime = DateTime.MinValue;
    static object s_obj = new object();
    static ActNoticeList s_list = new ActNoticeList();

    public static List<noticeList> load()
    {
        DateTime now = DateTime.Now;
        TimeSpan span = now - m_lastTime;
        if (span.TotalSeconds >= 60)
        {
            lock (s_obj)
            {
                span = now - m_lastTime;
                if (span.TotalSeconds >= 60)
                {
                    s_list.Load();
                    m_lastTime = now;
                }
            }
        }

        return s_list.getResult();
    }

}