using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

public class StatByDayBase : SysBase
{
    public const string TABLE_CLEAR = "_DATA_CLEAR_";

    protected DateTime m_statDay;

    public override void init()
    {
        Dictionary<string, object> data =
                MongodbPlayer.Instance.ExecuteGetOneBykey(TABLE_CLEAR, "key",
                getStatKey());
        if (data != null)
        {
            m_statDay = Convert.ToDateTime(data["statDay"]).ToLocalTime();
        }
        else
        {
            DateTime now = DateTime.Now.Date;
            m_statDay = now.AddDays(1);
            resetStatDay(m_statDay);
        }
    }

    protected void resetStatDay(DateTime statDay)
    {
        Dictionary<string, object> upData = new Dictionary<string, object>();
        upData.Add("statDay", statDay);
        MongodbPlayer.Instance.ExecuteStoreBykey(TABLE_CLEAR,
            "key", getStatKey(), upData);
    }

    public virtual string getStatKey()
    {
        throw new Exception();
    }

    protected void addStatDay()
    {
        m_statDay = m_statDay.AddDays(1);
        resetStatDay(m_statDay);
    }
}

// 用户邮件清理
public class CleanPlayerMail : StatByDayBase
{
    public const int STATE_IDLE = 0;
    public const int STATE_WORK = 1;

    private int m_state = STATE_IDLE;
    private DateTime m_lastTime;
    private int m_day = 180;

    public override string getStatKey()
    {
        return "playerMailClean";
    }

    public override void update(double delta)
    {
        switch (m_state)
        {
            case STATE_IDLE:
                {
                    if (DateTime.Now > m_statDay.AddHours(3))
                    {
                        m_state = STATE_WORK;
                        m_lastTime = DateTime.Now;
                        m_day = 0;
                    }
                }
                break;
            case STATE_WORK:
                {
                    bool res = clear();
                    if (res)
                    {
                        m_state = STATE_IDLE;
                        addStatDay();
                    }
                }
                break;
        }
    }

    bool clear()
    {
        TimeSpan span = DateTime.Now - m_lastTime;
        if (span.TotalSeconds > 120)
        {
            IMongoQuery imq = Query.LT("time", BsonValue.Create(DateTime.Now.AddDays(-31 - m_day)));

            object result = null;
            MongodbPlayer.Instance.removeByQuery(TableName.PLAYER_MAIL, imq, ref result);
            //             if (result != null)
            //             {
            //                 WriteConcernResult wcr = (WriteConcernResult)result;
            //                 if (wcr.DocumentsAffected <= 0)
            //                     return true;
            //             }
            m_day--;
            if (m_day < 0)
            {
                return true;
            }

            m_lastTime = DateTime.Now;
        }
        return false;
    }
}

//////////////////////////////////////////////////////////////////////////
public delegate int DelGetValue();

public class DataClearInfo
{
    // 表名
    public string m_tableName;
    // 时间字段名称
    public string m_timeFieldName; 
    // 数据库名称
    public int m_dbName;
    // 数据保存天数
    public int m_reserveDataDays;

    public DelGetValue m_fun = null;
}

public class CleanDataMgr : SysBase
{
    private List<DataClearInfo> m_list = new List<DataClearInfo>();
    private List<DataClearInfo> m_weekList = new List<DataClearInfo>();

    private DateTime m_clearTime;
    private DateTime m_weekTime;

    public override void init()
    {
        m_clearTime = DateTime.Now.Date.AddDays(1);

        int week = (int)DateTime.Now.DayOfWeek;
        week = (week == 0) ? 7 : week;
        m_weekTime = DateTime.Now.Date.AddDays(8 - week);

        addData(TableName.PUMP_PLAYER_MONEY, "genTime", DbName.DB_PUMP, 60);  // 玩家金币变化，保留60天
        addData(TableName.PLAYER_MAIL, "time", DbName.DB_PLAYER, 60);
        addData(TableName.PUMP_PLAYER_ITEM, "genTime", DbName.DB_PUMP, 60);
        addData(TableName.STAT_FISHLORD_MIDDLE_ROOM_RANK, "genTime", DbName.DB_PUMP, 8);
        addData(TableName.FISHLORD_ADVANCED_ROOM_ACT_RANK_HIS, "genTime", DbName.DB_PUMP, 8);
        addData(TableName.STAT_PUMP_LEGENDARY_FISH_ROOM_RANK, "genTime", DbName.DB_PUMP, 8);
        addData(TableName.STAT_FISHLORD_SHARK_RANK, "genTime", DbName.DB_PUMP, 8);
        addData(TableName.STAT_PUMP_MYTHICAL_ROOM_RANK, "genTime", DbName.DB_PUMP, 8);
        addWeekData("pumpDragonRoomRank", "weekCount", DbName.DB_PUMP, 8, getCurWeekCount);
    }

    public override void update(double delta)
    {
        if (DateTime.Now >= m_clearTime.AddHours(4))
        {
            foreach (var d in m_list)
            {
                clearData(d);
            }
            addClearTime();
        }

        if (DateTime.Now >= m_weekTime.AddHours(4.3))
        {
            foreach (var d in m_weekList)
            {
                clearData(d);
            }
            addWeekClearTime();
        }
    }

    protected void addClearTime()
    {
        m_clearTime = m_clearTime.AddDays(1);
    }

    protected void addWeekClearTime()
    {
        m_weekTime = m_weekTime.AddDays(7);
    }

    void addData(string tableName, string fieldName, int dbName, int days, DelGetValue fun = null)
    {
        DataClearInfo info = new DataClearInfo();
        info.m_tableName = tableName;
        info.m_timeFieldName = fieldName;
        info.m_dbName = dbName;
        info.m_reserveDataDays = days;
        info.m_fun = fun;
        m_list.Add(info);
    }

    void addWeekData(string tableName, string fieldName, int dbName, int days, DelGetValue fun = null)
    {
        DataClearInfo info = new DataClearInfo();
        info.m_tableName = tableName;
        info.m_timeFieldName = fieldName;
        info.m_dbName = dbName;
        info.m_reserveDataDays = days;
        info.m_fun = fun;
        m_weekList.Add(info);
    }
    void clearData(DataClearInfo info)
    {
        object result = null;
        IMongoQuery imq = null;
        if (info.m_fun == null)
        {
            imq = Query.LT(info.m_timeFieldName, BsonValue.Create(m_clearTime.AddDays(-info.m_reserveDataDays)));
        }
        else
        {
            imq = Query.LT(info.m_timeFieldName, BsonValue.Create(info.m_fun() - info.m_reserveDataDays));
        }

        if (info.m_dbName == DbName.DB_PUMP)
        {
            MongodbLog.Instance.removeByQuery(info.m_tableName, imq, ref result);
        }
        else if (info.m_dbName == DbName.DB_PLAYER)
        {
            MongodbPlayer.Instance.removeByQuery(info.m_tableName, imq, ref result);
        }
    }

    int getCurWeekCount()
    {
        // 604800 一周的秒数
        // 316800 距离1970.1.1周数补齐
        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return (int)(((long)ts.TotalSeconds - 316800) / 604800);
    }
}







