using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Data.OleDb;

public class DbServerInfo
{
    // �����ݿ�IP��PlayerDB����Ҳ��Ϊ�ؼ���
    public string m_serverIp = "";
    public int m_serverId;
    public string m_serverName = "";

    // ��־���ݿ�����IP
    public string m_logDbIp = "";
}

public class PlatformInfo
{
    public string m_engName = "";
    public string m_chaName = "";
    public string m_tableName = "";
}

public class OpRightInfo
{
    // ְԱ����
    public string m_staffType = "";
    // ���Ž������Ƽ�ֵ
    public int m_sendRewardLimit;
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

    private Dictionary<string, DbServerInfo> m_dbServer = new Dictionary<string, DbServerInfo>();
    private Dictionary<int, DbServerInfo> m_dbServerById = new Dictionary<int, DbServerInfo>();

    // ƽ̨�����Ϣ
    public Dictionary<string, PlatformInfo> m_plat = new Dictionary<string, PlatformInfo>();
    private Dictionary<int, PlatformInfo> m_platId = new Dictionary<int, PlatformInfo>();

    // ������Ա���Ž���ʱ������
    private Dictionary<string, OpRightInfo> m_opRight = new Dictionary<string, OpRightInfo>();

    // ����Ȩ����Ϣ
    private Dictionary<string, RightBaseInfo> m_baseRight = new Dictionary<string, RightBaseInfo>();

    public static ResMgr getInstance()
    {
        if (s_obj == null)
        {
            s_obj = new ResMgr();
            s_obj.init();
        }
        return s_obj;
    }

    public ResMgr()
    {
        m_path = HttpRuntime.BinDirectory + "..\\" + "data";
    }

    // ���ñ������·��
    public void setPath(string path)
    {
        m_path = path;
    }

    private void init()  //�Զ����ļ���
    {
        try
        {
            XmlConfigMaker c = new XmlConfigMaker();
            loadXmlConfig("money_reason.xml", c);
            loadXmlConfig("dbserver.xml", c);
            loadXmlConfig("cows_card.xml", c);
            loadTable("map_reduce.csv", new MapReduceTable(), '$');
            loadXmlConfig("M_RechangeNew.xml", c);

            setUpDbServerInfo();
            setUpPlatformInfo(c);
        }
        catch (System.Exception ex)
        {
            CLOG.Info(ex.ToString());
        }
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

    // ����ƽ̨����
    public PlatformInfo getPlatformInfo(int index)
    {
        if (m_platId.ContainsKey(index))
        {
            return m_platId[index];
        }
        return null;
    }

    // ����Ӣ�������õ�����ƽ̨��
    public PlatformInfo getPlatformInfoByName(string name)
    {
        if (m_plat.ContainsKey(name))
        {
            return m_plat[name];
        }
        return null;
    }

    // ����ƽ̨����
    public string getPlatformName(int index, bool eng = true)
    {
        PlatformInfo data = getPlatformInfo(index);
        if (data == null)
        {
            return "none";
        }
        if (eng)
            return data.m_engName;
        
        return data.m_chaName;
    }

    public Dictionary<int, PlatformInfo> getAllPlatId()
    {
        return m_platId;
    }

    public Dictionary<string, DbServerInfo> getAllDb()
    {
        return m_dbServer;
    }

    public Dictionary<string, RightBaseInfo> getBaseRight()
    {
        return m_baseRight;
    }

    public RightBaseInfo getBaseRightInfo(string rightId)
    {
        if (m_baseRight.ContainsKey(rightId))
            return m_baseRight[rightId];
        return null;
    }

    public DbServerInfo getDbInfo(string ip)
    {
        if (m_dbServer.ContainsKey(ip))
            return m_dbServer[ip];
        
        return null;
    }

    public DbServerInfo getDbInfoById(int serverId)
    {
        if (m_dbServerById.ContainsKey(serverId))
            return m_dbServerById[serverId];

        return null;
    }

    public OpRightInfo getOpRightInfo(string staffType)
    {
        if (m_opRight.ContainsKey(staffType))
            return m_opRight[staffType];

        return null;
    }

    private XmlConfig loadXmlConfig(string file, XmlConfigMaker c, bool add = true)
    {
        string fullfile = Path.Combine(m_path, file);
        XmlConfig xml = c.loadFromFile(fullfile);
        if (xml != null && add)
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
           // LOGW.Info("��ȡ�ļ�[{0}]ʧ��!", file);
        }
        else
        {
            if (!m_allTable.ContainsKey(file))
            {
                m_allTable.Add(file, table);
            }
        }
    }

    private void setUpDbServerInfo()
    {
        XmlConfig cfg = getRes("dbserver.xml");
        List<Dictionary<string, object>> t = cfg.getTable("server");
        for (int i = 0; i < t.Count; i++)
        {
            DbServerInfo info = new DbServerInfo();
            info.m_serverIp = Convert.ToString(t[i]["serverIp"]);
            info.m_serverId = Convert.ToInt32(t[i]["serverId"]);
            info.m_serverName = Convert.ToString(t[i]["serverName"]);
            info.m_logDbIp = Convert.ToString(t[i]["logDbIp"]);
            m_dbServer.Add(info.m_serverIp, info);
            m_dbServerById.Add(info.m_serverId, info);
        }
    }

    private void setUpPlatformInfo(XmlConfigMaker c)
    {
        XmlConfig cfg = loadXmlConfig("platform.xml", c, false);
        int count = cfg.getCount();
        for (int i = 0; i < count; i++)
        {
            List<Dictionary<string, object>> data = cfg.getTable(i.ToString());

            PlatformInfo info = new PlatformInfo();
            info.m_engName = Convert.ToString(data[0]["eng"]);
            info.m_chaName = Convert.ToString(data[0]["cha"]);
            info.m_tableName = Convert.ToString(data[0]["table"]);
            m_plat.Add(info.m_engName, info);
            m_platId.Add(i, info);
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
        item.m_map = table.fetch("map").toStr();
        item.m_reduce = table.fetch("reduce").toStr();
        m_items.Add(key, item);
    }

    public void endRead()
    {
    }

    public MapReduceItem getItem(string key)
    {
        if(m_items.ContainsKey(key))
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
public class DialLotteryItemCFG : XmlDataTable<DialLotteryItemCFG, int, ItemCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_DialLotteryCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            ItemCFGData tmp = new ItemCFGData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_itemId = Convert.ToInt32(sid);
            tmp.m_itemName = node.Attributes["RewardType"].Value;
            tmp.m_itemCount = Convert.ToInt32(node.Attributes["RewardCoin"].Value);
            m_data.Add(tmp.m_itemId, tmp);
        }
    }
}
///////////////////////////////////////////////////////////////////////////
public class ItemCFG : XmlDataTable<ItemCFG, int, ItemCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_ItemCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            ItemCFGData tmp = new ItemCFGData();
            string sid = node.Attributes["ItemId"].Value;
            tmp.m_itemId = Convert.ToInt32(sid);
            tmp.m_itemName = node.Attributes["ItemName"].Value;
            m_data.Add(tmp.m_itemId, tmp);
        }
    }
}
//////////////////////////////////////////////////////////////////////////

public class QuestCFG : XmlDataTable<QuestCFG, int, QusetCFGData>, IXmlData
{
    // ÿ������
    private List<QusetCFGData> m_dailyTask = new List<QusetCFGData>();
    // �ɾ�
    private List<QusetCFGData> m_achiveTask = new List<QusetCFGData>();

    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "QuestCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            QusetCFGData tmp = new QusetCFGData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_questId = Convert.ToInt32(sid);
            tmp.m_questType = Convert.ToInt32(node.Attributes["Type"].Value);
            tmp.m_questName = node.Attributes["Name"].Value;
            m_data.Add(tmp.m_questId, tmp);

            if (tmp.m_questType == 1) // ÿ��
            {
                m_dailyTask.Add(tmp);
            }
            else
            {
                m_achiveTask.Add(tmp);
            }
        }
    }

    // �����б�
    public List<QusetCFGData> getTaskList(TaskType taskType) 
    { 
        if(taskType == TaskType.taskTypeDaily)
            return m_dailyTask;
        return m_achiveTask;
    }
}

//////////////////////////////////////////////////////////////////////////

public class FishCFG : XmlDataTable<FishCFG, int, FishCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fish_FishCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            FishCFGData tmp = new FishCFGData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_fishId = Convert.ToInt32(sid);
            tmp.m_fishName = node.Attributes["FishName"].Value;
            m_data.Add(tmp.m_fishId, tmp);
        }
    }
}

// ���㹫԰�������
public class FishParkCFG : XmlDataTable<FishParkCFG, int, FishCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data\\fishpark";
        string file = Path.Combine(path, "Fish_FishCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            FishCFGData tmp = new FishCFGData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_fishId = Convert.ToInt32(sid);
            tmp.m_fishName = node.Attributes["FishName"].Value;
            m_data.Add(tmp.m_fishId, tmp);
        }
    }
}

//////////////////////////////////////////////////////////////////////////

public class Crocodile_RateCFG : XmlDataTable<Crocodile_RateCFG, int, Crocodile_RateCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Crocodile_RateCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            Crocodile_RateCFGData tmp = new Crocodile_RateCFGData();
            string sid = node.Attributes["Key"].Value;
            tmp.m_areaId = Convert.ToInt32(sid);
            tmp.m_name = node.Attributes["Name"].Value;
            tmp.m_icon = Path.GetFileName(node.Attributes["Icon"].Value);
            tmp.m_color = node.Attributes["Color"].Value;
            m_data.Add(tmp.m_areaId, tmp);
        }
    }
}

//////////////////////////////////////////////////////////////////////////
public class Dice_BetAreaCFG : XmlDataTable<Dice_BetAreaCFG, int, Dice_BetAreaCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Dice_BetAreaCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            Dice_BetAreaCFGData tmp = new Dice_BetAreaCFGData();
            string sid = node.Attributes["Key"].Value;
            tmp.m_areaId = Convert.ToInt32(sid);
            tmp.m_name = node.Attributes["AreaName"].Value;

            if (node.Attributes["span"] != null)
            {
                tmp.m_span = Convert.ToInt32(node.Attributes["span"].Value);
            }
            if (node.Attributes["odds"] != null)
            {
                tmp.m_desc = node.Attributes["odds"].Value;
            }
            m_data.Add(tmp.m_areaId, tmp);
        }
    }
}

//////////////////////////////////////////////////////////////////////////

public class RechargeCFGData
{
    public int m_payId;
    public int m_price;
    public string m_name = "";
}

public class RechargeCFG : XmlDataTable<RechargeCFG, int, RechargeCFGData>, IXmlData
{
    private List<RechargeCFGData> m_rechargeList = new List<RechargeCFGData>();

    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_RechangeCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            RechargeCFGData tmp = new RechargeCFGData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_payId = Convert.ToInt32(sid);
            tmp.m_price = Convert.ToInt32(node.Attributes["Price"].Value);
            tmp.m_name = node.Attributes["Name"].Value;
            m_rechargeList.Add(tmp);
            m_data.Add(tmp.m_payId, tmp);
        }
    }

    public List<RechargeCFGData> getRechargeList()
    {
        return m_rechargeList;
    }
}

public class Channel
{
    private static Channel s_obj = null;
    public List<ChannelInfo> m_cList = new List<ChannelInfo>();
    private Dictionary<string, ChannelInfo> m_channels = new Dictionary<string, ChannelInfo>();

    public static Channel getInstance()
    {
        if (s_obj == null)
        {
            s_obj = new Channel();
            s_obj.init();
        }
        return s_obj;
    }

    public ChannelInfo getChannel(string channelNo)
    {
        if (m_channels.ContainsKey(channelNo))
            return m_channels[channelNo];

        return null;
    }

    private void init()
    {
        XmlDocument doc = new XmlDocument();
        string fileName = HttpRuntime.BinDirectory + "..\\" + "data" + "\\channel.xml";
        /*  XmlNode root = doc.CreateElement("Root");
          doc.AppendChild(root);

          AccessDb.getAccDb().setConnDb("channel.mdb");

          string sql = "select* from channel where enable=true order by ID;";
          OleDbDataReader r = AccessDb.getAccDb().startQuery(sql);
          if (r != null)
          {
              while (r.Read())
              {
                  XmlElement node = doc.CreateElement("add");
                  node.SetAttribute("id", r["ID"].ToString());
                  node.SetAttribute("channelNo", r["channelNo"].ToString());
                  node.SetAttribute("channelName", r["channelName"].ToString());
                  root.AppendChild(node);
              }
          }
          AccessDb.getAccDb().end();
          doc.Save(fileName);*/

        doc.Load(fileName);
        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            ChannelInfo tmp = new ChannelInfo();
            tmp.ID = Convert.ToInt32(node.Attributes["id"].Value);
            tmp.channelName = node.Attributes["channelName"].Value;
            tmp.channelNo = node.Attributes["channelNo"].Value;
            m_cList.Add(tmp);

            m_channels.Add(tmp.channelNo, tmp);
        }
    }
}

public class ChannelInfo
{
    public int ID { set; get; }
    public string channelName { set; get; }
    public string channelNo { set; get; }
}

/////////////////////////////////////////////////////////////////////////

//���ջ
public class ActivityCFGData
{
    public int m_activityId;
    public string m_activityName="";
    public string m_activityStartTime = "";
    public string m_activityEndTime="";
}

public class ActivityCFG : XmlDataTable<ActivityCFG, int, ActivityCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_ActivityCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            ActivityCFGData tmp = new ActivityCFGData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_activityId = Convert.ToInt32(sid);
            tmp.m_activityName = Convert.ToString(node.Attributes["ActivityName"].Value);
            tmp.m_activityStartTime = node.Attributes["StartTime"].Value;
            tmp.m_activityEndTime=node.Attributes["EndTime"].Value;
            m_data.Add(tmp.m_activityId, tmp);
        }
    }
}
///////////////////////////////////////////////////////////////////////////
//���ջ
public class SevenDayActivityData
{
    public string m_activityName = "";
}

public class SevenDayActivityCFG : XmlDataTable<SevenDayActivityCFG, int, SevenDayActivityData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_SevenDayActivityCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            SevenDayActivityData tmp = new SevenDayActivityData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_activityName = Convert.ToString(node.Attributes["Name"].Value);
            m_data.Add(Convert.ToInt32(sid), tmp);
        }
    }
}
///////////////////////////////////////////////////////////////////////////
//���Գ齱��λ
public class ActivityLabaLotteryProbItem 
{
    public int m_labaProbId;
    public string m_rewardList;
    public string m_rewardCount;
}
public class ActivityLabaCFG : XmlDataTable<ActivityLabaCFG, int, ActivityLabaLotteryProbItem>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_ActivityLabaCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);
        XmlNode node = doc.SelectSingleNode("/Root");
        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            ActivityLabaLotteryProbItem tmp = new ActivityLabaLotteryProbItem();
            int sid = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_labaProbId = sid;
            tmp.m_rewardList = Convert.ToString(node.Attributes["RewardList"].Value);
            tmp.m_rewardCount = Convert.ToString(node.Attributes["RewardCount"].Value);
            m_data.Add(sid, tmp);
        }
    }
}
///////////////////////////////////////////////////////////////////////////
//����ż
public class ActivityPuppetItem 
{
    public int m_actId;
    public string m_actName;
}
public class ActivityPuppetCFG : XmlDataTable<ActivityPuppetCFG, int, ActivityPuppetItem>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_ActivityPuppetCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);
        XmlNode node = doc.SelectSingleNode("/Root");
        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            ActivityPuppetItem tmp = new ActivityPuppetItem();
            int sid = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_actId = sid;
            tmp.m_actName = Convert.ToString(node.Attributes["Name"].Value);
            m_data.Add(sid, tmp);
        }
    }
}
//����ż����
public class ActivityPuppetDonateRewardItem 
{
    public int m_itemId;
    public int m_awardType;
    public int m_donateCount;
    public string m_awardItemIds;
    public string m_awardItemCounts;
}
public class ActivityPuppetDonateRewardCFG : XmlDataTable<ActivityPuppetDonateRewardCFG, int, ActivityPuppetDonateRewardItem>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_ActivityPuppetDonateRewardCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);
        XmlNode node = doc.SelectSingleNode("/Root");
        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            ActivityPuppetDonateRewardItem tmp = new ActivityPuppetDonateRewardItem();
            int sid = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_itemId = sid;
            tmp.m_awardType = Convert.ToInt32(node.Attributes["AwardType"].Value);
            tmp.m_donateCount = Convert.ToInt32(node.Attributes["DonateCount"].Value);
            tmp.m_awardItemIds = Convert.ToString(node.Attributes["AwardItemIDs"].Value);
            tmp.m_awardItemCounts = Convert.ToString(node.Attributes["AwardItemCounts"].Value);
            m_data.Add(sid, tmp);
        }
    }
}
///////////////////////////////////////////////////////////////////////////
//��ʱ�
public class ActivityPanicBuyingItem
{
    public int m_activityId;
    public string m_activityName = "";
    public int m_maxCount;
}
public class ActivityPanicBuyingCFG : XmlDataTable<ActivityPanicBuyingCFG, int, ActivityPanicBuyingItem>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_ActivityPanicBuyingCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);
        XmlNode node = doc.SelectSingleNode("/Root");
        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            ActivityPanicBuyingItem tmp = new ActivityPanicBuyingItem();
            int sid = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_activityId = sid;
            tmp.m_activityName = Convert.ToString(node.Attributes["ActivityName"].Value);
            tmp.m_maxCount = Convert.ToInt32(node.Attributes["MaxCount"].Value);
            m_data.Add(sid, tmp);
        }
    }
}
//////////////////////////////////////////////////////////////////////////
//��ұ仯��ϸ
//M_QuestCFG.xml
public class MQuestCFGData
{
    public int m_questId;
    public string m_questDesc = "";
    public string m_awardItemIDs;
    public string m_awardItemCounts = "";
    public int m_type;
}

public class MQuestCFG : XmlDataTable<MQuestCFG, int, MQuestCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_QuestCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            MQuestCFGData tmp = new MQuestCFGData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_questId = Convert.ToInt32(sid);
            tmp.m_type = Convert.ToInt32(node.Attributes["Type"].Value);
            tmp.m_questDesc = Convert.ToString(node.Attributes["Desc"].Value);
            tmp.m_awardItemIDs = Convert.ToString(node.Attributes["AwardItemIDs"].Value);
            tmp.m_awardItemCounts = Convert.ToString(node.Attributes["AwardItemCounts"].Value);
            m_data.Add(tmp.m_questId, tmp);
        }
    }
}

//M_ActivityReward.xml
public class ActivityRewardCFGData
{
    public int m_activityRewardId;
    public string m_activityRewardName = "";
    public int m_activityRewardItemId;
    public string m_activityRewardCount = "";
}

public class ActivityRewardCFG : XmlDataTable<ActivityRewardCFG, int, ActivityRewardCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_ActiveReward.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            ActivityRewardCFGData tmp = new ActivityRewardCFGData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_activityRewardId = Convert.ToInt32(sid);
            tmp.m_activityRewardName = Convert.ToString(node.Attributes["Name"].Value);
            tmp.m_activityRewardItemId = Convert.ToInt32(node.Attributes["ItemId"].Value);
            tmp.m_activityRewardCount = Convert.ToString(node.Attributes["Count"].Value);
            m_data.Add(tmp.m_activityRewardId, tmp);
        }
    }
}
//////////////////////////////////////////////////////////////////////////

public class Fish_ItemCFGData
{
	public int m_itemId;

    public string m_itemName;
};

public class Fish_ItemCFG : XmlDataTable<Fish_ItemCFG, int, Fish_ItemCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fish_ItemCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            Fish_ItemCFGData tmp = new Fish_ItemCFGData();
            string sid = node.Attributes["ItemID"].Value;
            tmp.m_itemId = Convert.ToInt32(sid);
            tmp.m_itemName = node.Attributes["ItemName"].Value;
            m_data.Add(tmp.m_itemId, tmp);
        }
    }
}

//////////////////////////////////////////////////////////////////////////

public class Fish_BuffCFGData
{
    public int m_id;

    public string m_buffName;
};

public class Fish_BuffCFG : XmlDataTable<Fish_BuffCFG, int, Fish_BuffCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fish_BuffCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            Fish_BuffCFGData tmp = new Fish_BuffCFGData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_id = Convert.ToInt32(sid);
            tmp.m_buffName = node.Attributes["BuffName"].Value;
            m_data.Add(tmp.m_id, tmp);
        }
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
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
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

//////////////////////////////////////////////////////////////////////////
public class CalfRoping_CalfCFGData
{
    public int m_id;
    public string m_calfName;
}

public class CalfRoping_CalfCFG : XmlDataTable<CalfRoping_CalfCFG, int, CalfRoping_CalfCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "CalfRoping_CalfCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            CalfRoping_CalfCFGData tmp = new CalfRoping_CalfCFGData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_id = Convert.ToInt32(sid);
            tmp.m_calfName = node.Attributes["Name"].Value;
            m_data.Add(tmp.m_id, tmp);
        }
    }
}

//////////////////////////////////////////////////////////////////////////
public class Cows_CardsCFGData
{
    public int m_id;

    public string m_cardName;
};

public class Cows_CardsCFG : XmlDataTable<Cows_CardsCFG, int, Cows_CardsCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Cows_CardsCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            Cows_CardsCFGData tmp = new Cows_CardsCFGData();
            string sid = node.Attributes["CardsID"].Value;
            tmp.m_id = Convert.ToInt32(sid);
            tmp.m_cardName = node.Attributes["CardsName"].Value;
            m_data.Add(tmp.m_id, tmp);
        }
    }
}

//////////////////////////////////////////////////////////////////////////
public class ExchangeData
{
    public int m_id;

    public string m_name;
};

public class ExchangeCfg : XmlDataTable<ExchangeCfg, int, ExchangeData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        //string file = Path.Combine(path, "Exchange.xml");
        string file = Path.Combine(path, "M_ExchangeCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            ExchangeData tmp = new ExchangeData();
            //string sid = node.Attributes["Key"].Value;
            string sid = node.Attributes["ChangeId"].Value;
            tmp.m_id = Convert.ToInt32(sid);
            tmp.m_name = node.Attributes["ItemName"].Value;
            m_data.Add(tmp.m_id, tmp);
        }
    }
}

//////////////////////////////////////////////////////////////////////////
public class Fish_LevelCFGData
{
    public int m_level;

    public int m_openRate;
};

public class Fish_LevelCFG : XmlDataTable<Fish_LevelCFG, int, Fish_LevelCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fish_LevelCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            Fish_LevelCFGData tmp = new Fish_LevelCFGData();
            string sid = node.Attributes["Level"].Value;
            tmp.m_level = Convert.ToInt32(sid);
            tmp.m_openRate = Convert.ToInt32(node.Attributes["OpenRate"].Value);
            m_data.Add(tmp.m_level, tmp);
        }
    }
}

public class Fish_OpenRate_LevelCFG : XmlDataTable<Fish_OpenRate_LevelCFG, int, Fish_LevelCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fish_LevelCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            Fish_LevelCFGData tmp = new Fish_LevelCFGData();
            string sid = node.Attributes["Level"].Value;
            tmp.m_level = Convert.ToInt32(sid);
            tmp.m_openRate = Convert.ToInt32(node.Attributes["OpenRate"].Value);
            m_data.Add(tmp.m_openRate, tmp);
        }
    }
}

//////////////////////////////////////////////////////////////////////////
public class M_CDKEY_GiftCFGData
{
    public int m_giftId;
    public string m_name;
};

public class M_CDKEY_GiftCFG : XmlDataTable<M_CDKEY_GiftCFG, int, M_CDKEY_GiftCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_CDKEY_GiftCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            M_CDKEY_GiftCFGData tmp = new M_CDKEY_GiftCFGData();
            string sid = node.Attributes["GiftId"].Value;
            tmp.m_giftId = Convert.ToInt32(sid);
            tmp.m_name = node.Attributes["GiftName"].Value;
            m_data.Add(tmp.m_giftId, tmp);
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// Ȩ�޻�����Ϣ
public class RightBaseInfo
{
    // Ȩ��id
    public string m_rightId;
    // ����
    public string m_rightName;
    // �������
    public string m_category;
}
