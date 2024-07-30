using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Diagnostics;
using System.Xml;

// ������Ϣ
public class ChannelInfo
{
    // �������
    public string m_channelNum;

    // ��������
    public string m_channelName;

    // �������˺�ע���
    public string m_regeditTable;

    // �������˺ŵ�¼��
    public string m_loginTable;

    // �������˺ų�ֵ��
    public string m_paymentTable;

    // �豸�����
    public string m_deviceActivationTable;

    //////////////////////////////////////////////////////////////////////////

    // ͳ����
    public DateTime m_statDay;
}

public class ResMgr
{
    private static ResMgr s_obj = null;
    // �������·��
    private string m_path;
    // �洢�������
    private Dictionary<string, XmlConfig> m_allRes = new Dictionary<string, XmlConfig>();
    // �洢�������
    private Dictionary<string, IUserTabe> m_allTable = new Dictionary<string, IUserTabe>();

    private List<ChannelInfo> m_channelList = new List<ChannelInfo>();

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
    }

    // ���ñ������·��
    public void setPath(string path)
    {
        m_path = path;
    }

    // ȡ��ĳ�����
    public XmlConfig getRes(string name)
    {
        if (m_allRes.ContainsKey(name))
        {
            return m_allRes[name];
        }
        return null;
    }

    // ȡ��ĳ�����
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
            //LOGW.Info("��ȡ�ļ�[{0}]ʧ��!", file);
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
    // ��Ծ���
    public const int STAT_FLAG_ACTIVE = 1;

    // ���ѱ��
    public const int STAT_FLAG_RECHARGE = 2;

    // ������
    public const int STAT_FLAG_REMAIN = 4;

    // ������ͳ��
    public const int STAT_FLAG_COUNT = 8;

    // ������Ҽ�ֵ
    public const int STAT_LTV = 9;
}

// ͳ�ƽ����ÿͳ��ģ��������еĲ�������
public class StatResult : GameStatData
{
    public int m_statFlag;

    // ����������������ʱ����
    public int m_2DayRemainCountTmp;

    // 3����������
    public int m_3DayRemainCountTmp;

    // 7����������
    public int m_7DayRemainCountTmp;

    // 30����������
    public int m_30DayRemainCountTmp;

    //////////////////////////////////////////////////////////////////////////
    // �����豸������������ʱ����
    public int m_2DayDevRemainCountTmp;

    // 3���豸��������
    public int m_3DayDevRemainCountTmp;

    // 7���豸��������
    public int m_7DayDevRemainCountTmp;

    // 30���豸��������
    public int m_30DayDevRemainCountTmp;
   
    //////////////////////////////////////////////////////////////////////////

    // 1���ܳ�ֵ,��ʱ����
    public int m_1DayTotalRechargeTmp;
    // 3���ܳ�ֵ
    public int m_3DayTotalRechargeTmp;
    // 7���ܳ�ֵ
    public int m_7DayTotalRechargeTmp;
    // 14���ܳ�ֵ
    public int m_14DayTotalRechargeTmp;
    // 30���ܳ�ֵ
    public int m_30DayTotalRechargeTmp;
    // 60���ܳ�ֵ
    public int m_60DayTotalRechargeTmp;
    // 90���ܳ�ֵ
    public int m_90DayTotalRechargeTmp;

    // �Ƿ�ͳ����ĳ������
    public bool containsStat(int flag)
    {
        return (flag & m_statFlag) > 0;
    }

    public StatResult()
    {
        // -1��ʾ������ݻ�û�г�������Ҫ�ٵȼ��죬��ʾ �ޡ���������ע�ᣬ��ʾΪ0
        m_2DayRemainCount = -1;
        m_3DayRemainCount = -1;
        m_7DayRemainCount = -1;
        m_30DayRemainCount = -1;
    }
}

public class ParamStat
{
    // ������Ϣ
    public ChannelInfo m_channel;
}

//////////////////////////////////////////////////////////////////////////
public struct StatKey
{
    public const string KEY_LOSE = "v4Lose";

    public const string KEY_DRAGON = "playerDragonBall";

    public const string KEY_DRAGON_DAILY = "dragonBallDaily";

    public const string KEY_ONLINE_GAME_TIME = "onlineGameTime";

    // ����֧��
    public const string KEY_INCOME_EXPENSES = "incomeExpenses";
    // ����֧��
    public const string KEY_INCOME_EXPENSES_NEW = "incomeExpensesNew";

    // ÿСʱ��ֵ
    public const string KEY_RECHARGE_HOUR = "rechargePerHour";
    // ÿСʱ��������
    public const string KEY_ONLINE_HOUR = "onlinePerHour";

    public const string KEY_ONLINE_HOUR_RRD = "onlinePerHourRRd";

    // �û�����Ϸ����ʱ��
    public const string KEY_GAME_TIME_FOR_PLAYER_ACTIVE = "gameTimeForPlayerActive";
    // �׸���Ϊ
    public const string KEY_FIRST_RECHARGE_DISTRIBUTION = "firstRechargeDistribution";
    // �û���ע���ͳ��
    public const string KEY_PLAYER_GAME_BET = "playerGameBet";
    // ���������û������ע�ֲ�
    public const string KEY_NEW_PLAYER_OUTLAY_DISTRIBUTION = "newPlayerOutlayDistribution";

    // �������������ɳ��������Ծ�ֲ�
    public const string KEY_NEW_PLAYER_LEVEL_FISH_ACTIVITY = "newPlayerLevelFishActivity";

    public const string KEY_PERSON_TIME_GLOBAL_DAY = "personTimeGlobalDay";

    // ���ȯͳ��
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

    public const string KEY_NY_GIFT = "nyGift";

    public const string KEY_WUYI_RECHARGE = "wuyiRechargeLottery";
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
        string str = string.Format("{0}Сʱ{1}��{2}��{3}����", span.Hours, span.Minutes, span.Seconds, span.Milliseconds);
        LogMgr.log.InfoFormat("{0},������ʱ��:{1}", msg, str);
    }
}

//////////////////////////////////////////////////////////////////////////
public class TdChannelInfo
{
    public string m_channelNo;

    public string m_channelName;
};

// ������ص�����ͳ��
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
    public const int DAY_2_REMAIN = 1;   // ������������

    public const int DAY_3_REMAIN = 2;   // 3����������

    public const int DAY_7_REMAIN = 6;   // 7����������

    public const int DAY_30_REMAIN = 29;   // 30����������
}
