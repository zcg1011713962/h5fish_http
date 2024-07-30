using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Diagnostics;
using System.Xml;

// 渠道信息
public class ChannelInfo
{
    // 渠道编号
    public string m_channelNum;

    // 渠道名称
    public string m_channelName;

    // 该渠道账号注册表
    public string m_regeditTable;

    // 该渠道账号登录表
    public string m_loginTable;

    // 该渠道账号充值表
    public string m_paymentTable;

    // 设备激活表
    public string m_deviceActivationTable;

    //////////////////////////////////////////////////////////////////////////

    // 统计日
    public DateTime m_statDay;
}

public class ResMgr
{
    private static ResMgr s_obj = null;
    // 表格所在路径
    private string m_path;
    // 存储表格容器
    private Dictionary<string, XmlConfig> m_allRes = new Dictionary<string, XmlConfig>();
    // 存储表格容器
    private Dictionary<string, IUserTabe> m_allTable = new Dictionary<string, IUserTabe>();

    private List<ChannelInfo> m_channelList = new List<ChannelInfo>();
    private List<WeekChannelInfo> m_channelWeekList = new List<WeekChannelInfo>();

    public static ResMgr getInstance()
    {
        if (s_obj == null)
        {
            s_obj = new ResMgr();
        }
        return s_obj;
    }

    public ResMgr()
    {
        m_path = @"..\data\";
    }

    public void reload()
    {
        m_allRes.Clear();

        XmlConfig cfg = loadXmlConfig("dbserver.xml");

        setUpChannelList();

        loadTable("map_reduce.csv", new MapReduceTable(), '$');
    }

    // 设置表格所在路径
    public void setPath(string path)
    {
        m_path = path;
    }

    // 取得某个表格
    public XmlConfig getRes(string name)
    {
        if (m_allRes.ContainsKey(name))
        {
            return m_allRes[name];
        }
        return null;
    }

    // 取得某个表格
    public T getTable<T>(string name) where T : IUserTabe
    {
        if (m_allTable.ContainsKey(name))
        {
            return (T)m_allTable[name];
        }
        return default(T);
    }

    public List<ChannelInfo> getChannelList()
    {
        return m_channelList;
    }

    public List<WeekChannelInfo> getChannelWeekList()
    {
        return m_channelWeekList;
    }

    private XmlConfig loadXmlConfig(string file, bool save = true)
    {
        XmlConfigMaker c = new XmlConfigMaker();
        string fullfile = Path.Combine(m_path, file);
        XmlConfig xml = c.loadFromFile(fullfile);
        if (xml != null && save)
        {
            m_allRes.Add(file, xml);
        }
        return xml;
    }

    private void loadTable(string file, IUserTabe table, char end_flag = ' ')
    {
        string fullfile = Path.Combine(m_path, file);
        if (!Csv.load(fullfile, table, end_flag))
        {
            //LOGW.Info("读取文件[{0}]失败!", file);
        }
        else
        {
            if (!m_allTable.ContainsKey(file))
            {
                m_allTable.Add(file, table);
            }
        }
    }

    private void setUpChannelList()
    {
        //List<Dictionary<string, object>> clist = cfg.getTable("channel");
        var allData = TdChannel.getInstance().getAllData();
        //for (int i = 0; i < clist.Count; i++)
        foreach(var d in allData)
        {
            //Dictionary<string, object> data = clist[i];
            ChannelInfo info = new ChannelInfo();
            m_channelList.Add(info);

            info.m_channelNum = d.Key;//Convert.ToString(data["channelNum"]);
            info.m_channelName = d.Value.m_channelName;//Convert.ToString(data["channelName"]);
            info.m_regeditTable = "RegisterLog";
            info.m_paymentTable = "PayLog";

            info.m_loginTable = TableName.PLAYER_LOGIN;
            info.m_deviceActivationTable = "link_phone";

            WeekChannelInfo wi = new WeekChannelInfo();
            wi.m_channelInfo = info;
            m_channelWeekList.Add(wi);
        }
    }
}

//////////////////////////////////////////////////////////////////////////

public class MapReduceItem
{
    public string m_map = "";
    public string m_reduce = "";
}

public class MapReduceTable : IUserTabe
{
    private Dictionary<string, MapReduceItem> m_items = new Dictionary<string, MapReduceItem>();

    public void beginRead()
    {
        m_items.Clear();
    }

    public void readLine(ITable table)
    {
        MapReduceItem item = new MapReduceItem();
        string key = table.fetch("fun").toStr();

        int from = table.fetch("isLoadFromFile").toInt();
        if (from == 0)
        {
            item.m_map = table.fetch("map").toStr();
            item.m_reduce = table.fetch("reduce").toStr();
        }
        else
        {
            string js = table.fetch("map").toStr();
            ReadMapReduce r = new ReadMapReduce();
            bool res = r.load("..\\data\\stat\\" + js);
            if (res)
            {
                item.m_map = r.m_map;
                item.m_reduce = r.m_reduce;
            }
            else
            {
                return;
            }
        }
       
        m_items.Add(key, item);
    }

    public void endRead()
    {
    }

    public MapReduceItem getItem(string key)
    {
        if (m_items.ContainsKey(key))
        {
            return m_items[key];
        }
        return null;
    }

    public static string getMap(string key)
    {
        MapReduceTable t = ResMgr.getInstance().getTable<MapReduceTable>("map_reduce.csv");
        if (t != null)
        {
            MapReduceItem item = t.getItem(key);
            if (item != null)
            {
                return item.m_map;
            }
        }
        return "";
    }

    public static string getReduce(string key)
    {
        MapReduceTable t = ResMgr.getInstance().getTable<MapReduceTable>("map_reduce.csv");
        if (t != null)
        {
            MapReduceItem item = t.getItem(key);
            if (item != null)
            {
                return item.m_reduce;
            }
        }
        return "";
    }
}

//////////////////////////////////////////////////////////////////////////
public struct StatFlag
{
    // 活跃标记
    public const int STAT_FLAG_ACTIVE = 1;

    // 付费标记
    public const int STAT_FLAG_RECHARGE = 2;

    // 留存标记
    public const int STAT_FLAG_REMAIN = 4;

    // 纯数量统计
    public const int STAT_FLAG_COUNT = 8;

    // 新增玩家价值
    public const int STAT_LTV = 9;
}

// 统计结果，每统计模块填充其中的部分数据
public class StatResult : GameStatData
{
    public int m_statFlag;

    // 次日留存人数，临时数据
    public int m_2DayRemainCountTmp;

    // 3日留存人数
    public int m_3DayRemainCountTmp;

    // 7日留存人数
    public int m_7DayRemainCountTmp;

    // 30日留存人数
    public int m_30DayRemainCountTmp;

    //////////////////////////////////////////////////////////////////////////
    // 次日设备留存人数，临时数据
    public int m_2DayDevRemainCountTmp;

    // 3日设备留存人数
    public int m_3DayDevRemainCountTmp;

    // 7日设备留存人数
    public int m_7DayDevRemainCountTmp;

    // 30日设备留存人数
    public int m_30DayDevRemainCountTmp;
   
    //////////////////////////////////////////////////////////////////////////

    // 1日总充值,临时数据
    public int m_1DayTotalRechargeTmp;
    // 3日总充值
    public int m_3DayTotalRechargeTmp;
    // 7日总充值
    public int m_7DayTotalRechargeTmp;
    // 14日总充值
    public int m_14DayTotalRechargeTmp;
    // 30日总充值
    public int m_30DayTotalRechargeTmp;
    // 60日总充值
    public int m_60DayTotalRechargeTmp;
    // 90日总充值
    public int m_90DayTotalRechargeTmp;

    //////////////////////////////////////////////////////////////////////////
    // 次日付费人数 临时数据
    public int m_2DayRechargePersonNumTmp = 0;

    //////////////////////////////////////////////////////////////////////////
    // 次日留存人数(付费)
    public int m_2DayRemainCountRechargeTmp = 0;

    // 3日留存人数(付费)
    public int m_3DayRemainCountRechargeTmp = 0;

    // 7日留存人数(付费)
    public int m_7DayRemainCountRechargeTmp = 0;

    // 是否统计了某块数据
    public bool containsStat(int flag)
    {
        return (flag & m_statFlag) > 0;
    }

    public StatResult()
    {
        // -1表示这个数据还没有出来，需要再等几天，显示 无。若当天无注册，显示为0
        m_2DayRemainCount = -1;
        m_3DayRemainCount = -1;
        m_7DayRemainCount = -1;
        m_30DayRemainCount = -1;
    }
}

public class ParamStat
{
    // 渠道信息
    public ChannelInfo m_channel;
}

//////////////////////////////////////////////////////////////////////////
public struct StatKey
{
    public const string KEY_LOSE = "v4Lose";

    public const string KEY_DRAGON = "playerDragonBall";

    public const string KEY_DRAGON_DAILY = "dragonBallDaily";

    public const string KEY_ONLINE_GAME_TIME = "onlineGameTime";

    // 总收支表
    public const string KEY_INCOME_EXPENSES = "incomeExpenses";
    // 总收支表
    public const string KEY_INCOME_EXPENSES_NEW = "incomeExpensesNew";

    // 每小时充值
    public const string KEY_RECHARGE_HOUR = "rechargePerHour";
    // 每小时在线人数
    public const string KEY_ONLINE_HOUR = "onlinePerHour";

    public const string KEY_ONLINE_HOUR_RRD = "onlinePerHourRRd";

    // 用户各游戏在线时长
    public const string KEY_GAME_TIME_FOR_PLAYER_ACTIVE = "gameTimeForPlayerActive";

    public const string KEY_FISH_GAME_TIME_FOR_ROOM = "fishGameTimeForRoom";

    // 首付行为
    public const string KEY_FIRST_RECHARGE_DISTRIBUTION = "firstRechargeDistribution";
    // 用户下注情况统计
    public const string KEY_PLAYER_GAME_BET = "playerGameBet";
    // 当日新增用户金币下注分布
    public const string KEY_NEW_PLAYER_OUTLAY_DISTRIBUTION = "newPlayerOutlayDistribution";

    // 当日新增炮数成长，捕鱼活跃分布
    public const string KEY_NEW_PLAYER_LEVEL_FISH_ACTIVITY = "newPlayerLevelFishActivity";

    public const string KEY_PERSON_TIME_GLOBAL_DAY = "personTimeGlobalDay";

    // 抽彩券统计
    public const string KEY_STAR_LOTTERY = "starLottery2";

    public const string KEY_STAR_LOTTERY_DETAIL = "starLotteryDetail";

    public const string KEY_LABA_LOTTERY = "labaLottery";

    public const string KEY_BAOJIN = "fishBaoJin";

    public const string KEY_PUPPET = "actPuppet";

    public const string KEY_RECHARGE_POINT = "statRechargePoint";

    public const string KEY_OLD_PLAYER_LOGIN = "statOldPlayerLogin";

    public const string KEY_SIGN = "statSign";

    public const string KEY_AIBEI = "statRechargeAibei";
    public const string KEY_BAOJIN_JOIN_DISTRIBUTE = "statFishBaojinJoinDistribute";

    public const string KEY_DRAGON_PALACE_JOIN_DISTRIBUTE = "statDragonPalaceDistribute";

    public const string KEY_NY_GIFT = "nyGift";

    public const string KEY_WUYI_RECHARGE = "wuyiRechargeLottery";

    public const string KEY_PANIC_BOX = "panicBox";

    public const string KEY_NATIONAL_DAY2018 = "nationalDay2018";

    public const string KEY_PLAYER_MONEY_RANK = "playerMoneyRank";

    public const string KEY_MINI_GAME_DAU = "miniGameDau";

    public const string KEY_PLAYER_MONEY_REP = "playerMoneyRep";

    public const string KEY_ITEM_BUY = "itemBuy";

    public const string KEY_NEW_PLAYER_TURRET_TIME = "newPlayerTurretTime";

    // vip特权
    public const string KEY_VIP_RECORD = "vipRecord";

    public const string KEY_TURRET_ITEMS = "turretItems";

    // 携带金币
    public const string KEY_PLAYER_WITH_GOLD = "playerWithGold";

    public const string KEY_PLAYER_EXCHANGE = "playerExchange";
    public const string KEY_TOUCH_FISH_PLAYER_DISTRI = "touchFishPlayerDistri";

    public const string KEY_CHANNEL_100003 = "statChannelGold100003";
    public const string KEY_CHANNEL_100009 = "statChannelGold100009";
    public const string KEY_CHANNEL_100010 = "statChannelGold100010";
    public const string KEY_CHANNEL_100011 = "statChannelGold100011";
    public const string KEY_CHANNEL_100012 = "statChannelGold100012";
    public const string KEY_CHANNEL_100013 = "statChannelGold100013";
    public const string KEY_CHANNEL_100014 = "statChannelGold100014";
    public const string KEY_CHANNEL_100015 = "statChannelGold100015";
    public const string KEY_CHANNEL_100016 = "statChannelGold100016";
}

//////////////////////////////////////////////////////////////////////////

public class QueryTool
{
    // 返回下一条数据
    public static List<Dictionary<string, object>> nextData<T>(MongodbHelper<T> db,
                                                                string table,
                                                                IMongoQuery imq,
                                                                ref int skip,
                                                                int count,
                                                                string[] fields = null,
                                                                string sort = "",
                                                                bool asc = true) where T : new()
    {
        List<Dictionary<string, object>> data =
            db.ExecuteGetListByQuery(table, imq, fields, sort, asc, skip, count);
        if (data == null || data.Count == 0)
            return null;
        skip += count;
        return data;
    }

    // 账号是否登录过
    public static bool isLogin(string acc, DateTime mint, DateTime maxt)
    {
        IMongoQuery imq1 = Query.LT("time", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("time", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.EQ(StatUnitRemain.ACC_KEY, BsonValue.Create(acc));
        IMongoQuery imq = Query.And(imq1, imq2, imq3);

        bool res = MongodbAccount.Instance.KeyExistsByQuery(TableName.PLAYER_LOGIN, imq);
        return res;
    }
}

//////////////////////////////////////////////////////////////////////////
public class WatchTime
{
    private Stopwatch m_watch = new Stopwatch();

    public void start(string info, params object[] args)
    {
        LogMgr.log.InfoFormat(info, args);
        m_watch.Start();
    }

    public void end(string info, params object[] args)
    {
        m_watch.Stop();
        TimeSpan span = m_watch.Elapsed;
        string msg = string.Format(info, args);
        string str = string.Format("{0}小时{1}分{2}秒{3}毫秒", span.Hours, span.Minutes, span.Seconds, span.Milliseconds);
        LogMgr.log.InfoFormat("{0},总运行时间:{1}", msg, str);
    }
}

//////////////////////////////////////////////////////////////////////////
public class TdChannelInfo
{
    public string m_channelNo;

    public string m_channelName;
};

// 渠道相关的数据统计
public class TdChannel : XmlDataTable<TdChannel, string, TdChannelInfo>, IXmlData
{
    public void init()
    {
        string path = "..\\" + "data";
        string file = Path.Combine(path, "td_channel.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            TdChannelInfo tmp = new TdChannelInfo();
            tmp.m_channelNo = node.Attributes["channelNo"].Value;
            tmp.m_channelName = node.Attributes["channelName"].Value;
            m_data.Add(tmp.m_channelNo, tmp);
        }
    }
}

public class WorldCupScheduleData
{
    public int m_matchId;
    public DateTime m_matchTime;
}

public class WorldCupSchedule : XmlDataTable<WorldCupSchedule, int, WorldCupScheduleData>, IXmlData
{
    public void init()
    {
        string path = "..\\" + "data";
        string file = Path.Combine(path, "M_WorldCupSchedule.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            WorldCupScheduleData tmp = new WorldCupScheduleData();
            tmp.m_matchId = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_matchTime = Convert.ToDateTime(node.Attributes["RaceTime"].Value);
            m_data.Add(tmp.m_matchId, tmp);
        }
    }
}

//////////////////////////////////////////////////////////////////////////
public struct CRemainConst
{
    public const int DAY_2_REMAIN = 1;   // 次日留存天数

    public const int DAY_3_REMAIN = 2;   // 3日留存天数

    public const int DAY_7_REMAIN = 6;   // 7日留存天数

    public const int DAY_30_REMAIN = 29;   // 30日留存天数

    public const int DAY_14_REMAIN = 13;

    public const int DAY_60_REMAIN = 59;

    public const int DAY_90_REMAIN = 89;  
}

public class ToolHelper
{
    public static string channelToString(string channel)
    {
        if (string.IsNullOrEmpty(channel))
            return "000000";

        return channel.PadLeft(6, '0');
    }
}

//////////////////////////////////////////////////////////////////////////
public class StatChannel100003_CFGData
{
    public int m_id;

    public List<long> m_dataList = new List<long>();
}

public class StatChannel100003_CFG : XmlDataTable<StatChannel100003_CFG, int, StatChannel100003_CFGData>, IXmlData
{
    public const int ID_GOLD = 1;
    public const int ID_MONEY = 2;

    public string XmlName { set; get; }

    public StatChannel100003_CFG()
    {
        XmlName = "StatChannel100003_CFG.xml";
    }

    public void init()
    {
        string path = "..\\" + "data";
        string file = Path.Combine(path, XmlName);
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            if(node.NodeType == XmlNodeType.Comment)
                continue;

            StatChannel100003_CFGData tmp = new StatChannel100003_CFGData();
            tmp.m_id = Convert.ToInt32(node.Attributes["ID"].Value);
            string stype = node.Attributes["Type"].Value;

            string range = node.Attributes["Range"].Value;
            string[] arr = range.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arr.Length; ++i)
            {
                if (stype == "gold")
                {
                    long a = Convert.ToInt64(arr[i]) * 10000;
                    tmp.m_dataList.Add(a);
                }
                else
                {
                    long a = Convert.ToInt64(arr[i]);
                    tmp.m_dataList.Add(a);
                }
            }
            m_data.Add(tmp.m_id, tmp);
        }
    }

    public int getIndex(int key, long d)
    {
        var info = getValue(key);
        for (int i = info.m_dataList.Count - 1; i >= 0; --i)
        {
            if (d >= info.m_dataList[i])
                return i;
        }

        return -1;
    }
}

//////////////////////////////////////////////////////////////////////////
public class WeekInfo
{
    public int m_weekIndex;
    public int m_startDay;
    public int m_endDay;

    public DateTime m_startTime;
    public DateTime m_endTime;
}

public class WStatParam
{
    public int m_week;
    public DateTime m_startTime;
    public DateTime m_endTime;
}

public class DateWeekCal
{
    protected Dictionary<int, WeekInfo> m_weekList = new Dictionary<int, WeekInfo>();

    protected int m_year;
    protected int m_month;

    public void init(int year, int month)
    {
        m_year = year;
        m_month = month;

        m_weekList.Clear();

        DateTime d = new DateTime(year, month, 1, 0, 0, 0);
        int totalDays = DateTime.DaysInMonth(year, month);
//         int week = (int)d.DayOfWeek;
//         if (week == 0)
//         {
//             week = 7;
//         }

        int weekIndex = 1;

        for (int day = 1; day <= totalDays; day += 7)
        {
            WeekInfo info = new WeekInfo();
            m_weekList.Add(weekIndex, info);

            info.m_weekIndex = weekIndex;
            info.m_startDay = day;
            info.m_startTime = d.AddDays(info.m_startDay - 1);
            if (weekIndex == 4)
            {
                info.m_endDay = day + 6; //totalDays;
                day = 32; // 退出循环
            }
            else
            {
                info.m_endDay = day + 6;
            }
            info.m_endTime = d.AddDays(info.m_endDay);
            weekIndex++;
        }
    }

    // day 1-31之间，输入当月的第几天，返回是当月的第几周
    public WeekInfo getWeekInfoByDay(int day)
    {
        foreach (var v in m_weekList)
        {
            WeekInfo info = v.Value;
            if (day >= info.m_startDay && day <= info.m_endDay)
                return info;
        }

        return null;
    }

    public WeekInfo getWeekInfoByWeek(int week)
    {
        if (m_weekList.ContainsKey(week))
        {
            return m_weekList[week];
        }

        return null;
    }

    public DateTime getMonthEndTime()
    {
        DateTime t = new DateTime(m_year, m_month, 1, 0, 0, 0);
        DateTime r = t.AddMonths(1);
        return r;
    }

    public DateTime getNextMonthEndTime()
    {
        DateTime end = getMonthEndTime();
        DateTime t = new DateTime(end.Year, end.Month, 1, 0, 0, 0);
        DateTime r = t.AddMonths(1);
        return r;
    }

    // week 是当前周
    public DateTime getNextWeekEndTime(int week)
    {
        var wi = getWeekInfoByWeek(week + 1);
        if (wi != null)
            return wi.m_endTime;

        var cur = getWeekInfoByWeek(week);
        return cur.m_endTime.AddDays(7);
    }

    public WStatParam getMonthStatParam()
    {
        WStatParam r = new WStatParam();
        r.m_startTime = new DateTime(m_year, m_month, 1);
        r.m_endTime = getMonthEndTime();
        r.m_week = 0;
        return r;
    }

    public WStatParam getWeekStatParam(DateTime weekEndTime)
    {
        DateTime L = weekEndTime.AddDays(-1);
        var wi = getWeekInfoByDay(L.Day);

        WStatParam r = new WStatParam();
        r.m_startTime = wi.m_startTime;
        r.m_endTime = wi.m_endTime;
        r.m_week = wi.m_weekIndex;
        return r;
    }

    public DateWeekCal getNextMonthCal()
    {
        DateTime t = getMonthEndTime();
        DateWeekCal r = new DateWeekCal();
        r.init(t.Year, t.Month);
        return r;
    }
}

////////////////////////////////////////////////////////////////////////
public class WeekChannelInfo
{
    public ChannelInfo m_channelInfo;

    public DateTime m_weekStatTime;
    public DateTime m_monthStatTime;
}

public class StatWeekResult
{
    // 新增用户
    public long m_regeditCount;

    // 活跃人数
    public long m_activeCount;

    // 总收入
    public long m_totalIncome;

    // 付费人数
    public long m_rechargePersonNum;

    // 付费次数
    public long m_rechargeCount;

}


