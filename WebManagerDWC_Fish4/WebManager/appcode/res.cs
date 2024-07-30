using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Data.OleDb;

/*public class DbServerInfo
{
    // 主数据库IP，PlayerDB，它也作为关键字
    public string m_serverIp = "";
    public int m_serverId;
    public string m_serverName = "";

    // 日志数据库所在IP
    public string m_logDbIp = "";

    // playerdb备
    public string m_playerDbSlave;
    // logdb备
    public string m_logDbSlave;
}*/

public class PlatformInfo
{
    public string m_engName = "";
    public string m_chaName = "";
    public string m_tableName = "";
}

public class OpRightInfo
{
    // 职员类型
    public string m_staffType = "";
    // 发放奖励限制价值
    public int m_sendRewardLimit;
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

    private Dictionary<string, DbServerInfo> m_dbServer = new Dictionary<string, DbServerInfo>();
    private Dictionary<int, DbServerInfo> m_dbServerById = new Dictionary<int, DbServerInfo>();

    // 平台相关信息
    public Dictionary<string, PlatformInfo> m_plat = new Dictionary<string, PlatformInfo>();
    private Dictionary<int, PlatformInfo> m_platId = new Dictionary<int, PlatformInfo>();

    // 各类人员发放奖励时的限制
    private Dictionary<string, OpRightInfo> m_opRight = new Dictionary<string, OpRightInfo>();

    // 基础权限信息
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

    // 设置表格所在路径
    public void setPath(string path)
    {
        m_path = path;
    }

    private void init()  //自定义文件读
    {
        try
        {
            XmlConfigMaker c = new XmlConfigMaker();
            loadXmlConfig("money_reason.xml", c);
            loadXmlConfig("dbserver.xml", c);
            //  loadXmlConfig("fish_consume.xml", c);
            //loadXmlConfig("cows_card.xml", c);
            //loadXmlConfig("RightList.xml", c);
            loadTable("map_reduce.csv", new MapReduceTable(), '$');
            loadXmlConfig("M_RechangeNew.xml", c);
            //loadXmlConfig("M_ReloadServiceCFG.xml",c);

            setUpDbServerInfo();
            //setUpPlatformInfo(c);
            setUpOpRight(c);
            setUpBaseRightInfo(c);
        }
        catch (System.Exception ex)
        {
            CLOG.Info(ex.ToString());
        }
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

    // 返回平台名称
    public PlatformInfo getPlatformInfo(int index)
    {
        if (m_platId.ContainsKey(index))
        {
            return m_platId[index];
        }
        return null;
    }

    // 根据英文名，得到中文平台名
    public PlatformInfo getPlatformInfoByName(string name)
    {
        if (m_plat.ContainsKey(name))
        {
            return m_plat[name];
        }
        return null;
    }

    // 返回平台名称
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
            LOGW.Info("读取文件[{0}]失败!", file);
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

            info.m_playerDbSlave = Convert.ToString(t[i]["playerDbSlave"]);
            info.m_logDbSlave = Convert.ToString(t[i]["logDbSlave"]);

            if (t[i].ContainsKey("monitor"))
            {
                info.m_monitor = Convert.ToString(t[i]["monitor"]);
            }else 
            {
                info.m_monitor = "";
            }

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

    private void setUpOpRight(XmlConfigMaker c)
    {
        string[] arr = new string[] { "service", "operation", "opDirector", "ceo", "admin" };
        XmlConfig cfg = loadXmlConfig("OpRight.xml", c, false);
        int count = cfg.getCount();
        for (int i = 0; i < arr.Length; i++)
        {
            List<Dictionary<string, object>> data = cfg.getTable(arr[i]);
            if (data != null)
            {
                OpRightInfo info = new OpRightInfo();
                info.m_staffType = arr[i];
                info.m_sendRewardLimit = Convert.ToInt32(data[0]["sendRewardLimit"]);
                m_opRight.Add(arr[i], info);
            }
            else
            {
                LOGW.Info("OpRight.xml找不到关键字[{0}]}", arr[i]);
            }
        }
    }

    private void setUpBaseRightInfo(XmlConfigMaker c)
    {
        XmlConfig cfg = loadXmlConfig("RightList.xml", c, false);
        int count = cfg.getCount();
        //string[] arr = new string[] { "op", "svr", "td", "data", "fish", "crod", "dice", "bacc", "cow", "d5", "shcd", "calf", "other", "shuihz" };
        foreach(var rname in cfg.getData())
        {
            List<Dictionary<string, object>> data = cfg.getTable(rname.Key);
            if (data != null)
            {
                for (int k = 0; k < data.Count; k++)
                {
                    RightBaseInfo info = new RightBaseInfo();
                    info.m_category = rname.Key; // arr[i];
                    info.m_rightId = Convert.ToString(data[k]["rightId"]);
                    info.m_rightName = Convert.ToString(data[k]["name"]);
                    m_baseRight.Add(info.m_rightId, info);
                }
            }
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
//巨鲨场
public class FishGiantSharkIntegralCFG : XmlDataTable<FishGiantSharkIntegralCFG, int, ItemCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fish_GiantSharkIntegral.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            ItemCFGData tmp = new ItemCFGData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_itemId = Convert.ToInt32(sid);
            tmp.m_itemName = node.Attributes["Name"].Value;
            m_data.Add(tmp.m_itemId, tmp);
        }
    }
}
///////////////////////////////////////////////////////////////////////////
//巨鲨场抽奖奖励
public class SharkRoomTurntableData 
{
    public int m_index;
    public string m_rewardList;
    public string m_rewardCount;
}
public class FishGiantSharkTurntableCFG : XmlDataTable<FishGiantSharkTurntableCFG, int, SharkRoomTurntableData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fish_GiantSharkTurntable.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            SharkRoomTurntableData tmp = new SharkRoomTurntableData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_index = Convert.ToInt32(sid);
            tmp.m_rewardList = node.Attributes["RewardList"].Value;
            tmp.m_rewardCount = node.Attributes["RewardCount"].Value;
            m_data.Add(tmp.m_index, tmp);
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
            tmp.m_gameItem = node.Attributes["GameItem"].Value;
            tmp.m_goldValue = node.Attributes["GoldValue"].Value;
            m_data.Add(tmp.m_itemId, tmp);
        }
    }
}
///////////////////////////////////////////////////////////////////////////
//新手任务
public class NewPlayerQuestCFG : XmlDataTable<NewPlayerQuestCFG, int, ItemCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_NewPlayerQuestCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            ItemCFGData tmp = new ItemCFGData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_itemId = Convert.ToInt32(sid);
            tmp.m_itemName = node.Attributes["Desc"].Value;
            m_data.Add(tmp.m_itemId, tmp);
        }
    }
}
/////////////////////////////////////////////////////////////////////////////////
//十一活动
public class ActNationalDayMissionData
{
    public int m_indexId;
    public int m_branchId;
    public int m_stage;
}
public class ActivityNationalDayMissionCFG : XmlDataTable<ActivityNationalDayMissionCFG, int, ActNationalDayMissionData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_ActivityNationalDayMission.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            ActNationalDayMissionData tmp = new ActNationalDayMissionData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_indexId = Convert.ToInt32(sid);
            tmp.m_branchId = Convert.ToInt32(node.Attributes["TheBranchLine"].Value);
            tmp.m_stage = Convert.ToInt32(node.Attributes["Stage"].Value);
            m_data.Add(tmp.m_indexId, tmp);
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////
public class LotteryCFGData
{
    public int m_indexId;
    public int m_itemId;
    public int m_itemCount;
}
public class ActivityDrawLotteryCFG : XmlDataTable<ActivityDrawLotteryCFG, int, LotteryCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_ActivityDrawLottery.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            LotteryCFGData tmp = new LotteryCFGData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_indexId = Convert.ToInt32(sid);
            tmp.m_itemId = Convert.ToInt32(node.Attributes["ItemId"].Value);
            tmp.m_itemCount = Convert.ToInt32(node.Attributes["Number"].Value);
            m_data.Add(tmp.m_indexId, tmp);
        }
    }
}
///////////////////////////////////////////////////////////////////////////////
//经典捕鱼基本信息表
public class FishBaseInfoData
{
    public string m_index;
    public int m_value;
}
public class FishBaseInfoCFG : XmlDataTable<FishBaseInfoCFG, string, FishBaseInfoData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fish_BaseInfo.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            FishBaseInfoData tmp = new FishBaseInfoData();
            string sid = node.Attributes["Key"].Value;
            tmp.m_index = sid;
            tmp.m_value = Convert.ToInt32(node.Attributes["Value"].Value);

            m_data.Add(tmp.m_index, tmp);
        }
    }
}

////////////////////////////////////////////////////////////////////////////////
//捕鱼弹头
public class FishBulletHeadData
{
    public int m_id;
    public int m_goldBase;
    public int m_randRange;
}
public class FishBulletHeadCFG : XmlDataTable<FishBulletHeadCFG, int, FishBulletHeadData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fish_BulletHeadCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            FishBulletHeadData tmp = new FishBulletHeadData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_id = Convert.ToInt32(sid);
            tmp.m_goldBase = Convert.ToInt32(node.Attributes["GoldBase"].Value);
            tmp.m_randRange = Convert.ToInt32(node.Attributes["RandRange"].Value);
            m_data.Add(tmp.m_id, tmp);
        }
    }
}


//刮刮乐兑奖
public class ScratchCFGData
{
    public int m_id;
    public int m_itemId;
    public string m_itemCount;
    public int m_randType;
}
public class ScratchTicketCFG : XmlDataTable<ScratchTicketCFG, int, ScratchCFGData>, IXmlData 
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_ActivityScratchTicketCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            ScratchCFGData tmp = new ScratchCFGData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_id = Convert.ToInt32(sid);
            tmp.m_itemId = Convert.ToInt32(node.Attributes["ItemId"].Value);
            tmp.m_itemCount = Convert.ToString(node.Attributes["ItemCount"].Value);
            tmp.m_randType = Convert.ToInt32(node.Attributes["RandType"].Value);
            m_data.Add(tmp.m_id, tmp);
        }
    }
}
//////////////////////////////////////////////////////////////////////////

public class QuestCFG : XmlDataTable<QuestCFG, int, QusetCFGData>, IXmlData
{
    // 每日任务
    private List<QusetCFGData> m_dailyTask = new List<QusetCFGData>();
    // 成就
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

            if (tmp.m_questType == 1) // 每日
            {
                m_dailyTask.Add(tmp);
            }
            else
            {
                m_achiveTask.Add(tmp);
            }
        }
    }

    // 任务列表
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

/*
// 鳄鱼公园鱼表配置
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
}*/

//////////////////////////////////////////////////////////////////////////
//鳄鱼大亨
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
/////////////////////////////////////////////////////////////////////////////////
//水果机
public class Fruit_RateCFG : XmlDataTable<Fruit_RateCFG, int, Crocodile_RateCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fruit_ProbCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            Crocodile_RateCFGData tmp = new Crocodile_RateCFGData();
            string sid = node.Attributes["ProbID"].Value;
            tmp.m_areaId = Convert.ToInt32(sid);
            tmp.m_name = node.Attributes["Description"].Value;
            tmp.m_icon = Path.GetFileName(node.Attributes["Icon"].Value);
            m_data.Add(tmp.m_areaId, tmp);
        }
    }
}
//通过POSID获取probID和icon
public class Fruit_PosCFG : XmlDataTable<Fruit_PosCFG, int, Fruit_PosData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fruit_ProbCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            string sid = node.Attributes["PosID"].Value;
            if (string.IsNullOrEmpty(sid))
                continue;

            string[] sid_arr = Tool.split(sid, ',');
            foreach (var id in sid_arr)
            {
                int posId = Convert.ToInt32(id);
                if (posId == -1 || posId == 11 || posId == 23)
                    continue;

                Fruit_PosData tmp = new Fruit_PosData();
                tmp.m_posId = Convert.ToInt32(posId);
                tmp.m_icon = Path.GetFileName(node.Attributes["Icon"].Value);
                m_data.Add(tmp.m_posId, tmp);
            }
        }
    }
}
/////////////////////////////////////////////////////////////////////////////
//奔驰宝马
public class Bz_RateCFG : XmlDataTable<Bz_RateCFG, int, Crocodile_RateCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Bz_RateCFG.xml");
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
//补单补贴/客服回访福利邮件
public class RepublicLanguageData 
{
    public int m_mailId;
    public string m_mailName;
    public string m_mailOperator;
    public string m_mailText;
}
public class RepublicLanguagerMail : XmlDataTable<RepublicLanguagerMail, int, RepublicLanguageData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_RepublicLanguage.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            RepublicLanguageData tmp = new RepublicLanguageData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_mailId = Convert.ToInt32(sid);
            tmp.m_mailName = node.Attributes["Name"].Value;
            tmp.m_mailOperator = node.Attributes["Operator"].Value;
            tmp.m_mailText = node.Attributes["Text"].Value;
            m_data.Add(tmp.m_mailId, tmp);
        }
    }
}

//////////////////////////////////////////////////////////////////////////
//补单补贴/客服回访福利
public class RepairOrderData 
{
    public int m_itemId;
    public string m_itemName;
    public string m_itemCusSerGiftbag;
    public string m_itemCusSerGiftbagItems;
    public string m_itemCusSerGiftbagNum;
   
}
public class RepairOrderItem : XmlDataTable<RepairOrderItem, int, RepairOrderData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_CustomerServiceGiftbag.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            RepairOrderData tmp = new RepairOrderData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_itemId = Convert.ToInt32(sid);
            tmp.m_itemName = node.Attributes["Name"].Value;
            tmp.m_itemCusSerGiftbag = node.Attributes["CusSerGiftbag"].Value;
            tmp.m_itemCusSerGiftbagItems = node.Attributes["CusSerGiftbagItems"].Value;
            tmp.m_itemCusSerGiftbagNum = node.Attributes["CusSerGiftbagNum"].Value;
            m_data.Add(tmp.m_itemId, tmp);
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

//节日活动
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
//七日活动
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
//拉霸抽奖档位
public class ActivityLabaLotteryProbItem 
{
    public int m_labaProbId;
    public int m_group;
    public int m_lotteryType;
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
            if(sid<=0)
            {
                continue;
            }
            tmp.m_labaProbId = sid;
            tmp.m_rewardList = Convert.ToString(node.Attributes["RewardList"].Value);
            tmp.m_rewardCount = Convert.ToString(node.Attributes["RewardCount"].Value);
            m_data.Add(sid, tmp);
        }
    }
}
////////////////////////////////////////////////////////////////////////////
//金秋
public class JinQiuGiftItem 
{
    public int m_giftId;
    public int m_giftPrice;
}
public class JinQiuActCFG : XmlDataTable<JinQiuActCFG, int, JinQiuGiftItem>, IXmlData 
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_JinQiu_RechargeLotteryCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);
        XmlNode node = doc.SelectSingleNode("/Root");
        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            JinQiuGiftItem tmp = new JinQiuGiftItem();
            int sid = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_giftId = sid;
            if (!string.IsNullOrEmpty(node.Attributes["MinGold"].Value))
            {
                tmp.m_giftPrice = Convert.ToInt32(node.Attributes["MinGold"].Value) / 10000;
            }

            m_data.Add(sid, tmp);
        }
    }
}
///////////////////////////////////////////////////////////////////////////
//圣诞节/元旦活动
public class ChristmasActItem 
{
    public int m_actId;
    public int m_actType;
    public string m_actName;
    public int m_chgItemId;
    public int m_chgItemCount;
}
public class ActivityChristmasCFG : XmlDataTable<ActivityChristmasCFG, int, ChristmasActItem>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fish_ChristmasCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);
        XmlNode node = doc.SelectSingleNode("/Root");
        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            ChristmasActItem tmp = new ChristmasActItem();
            int sid = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_actId = sid;
            tmp.m_actType = Convert.ToInt32(node.Attributes["ActType"].Value);
            tmp.m_actName = Convert.ToString(node.Attributes["Name"].Value);
            tmp.m_chgItemId = Convert.ToInt32(node.Attributes["ChgItemId"].Value);
            tmp.m_chgItemCount = Convert.ToInt32(node.Attributes["ChgItemCount"].Value);
            m_data.Add(sid, tmp);
        }
    }
}
///////////////////////////////////////////////////////////////////////////
//世界杯大竞猜赛事
public class WorldCupNationItem 
{
    public int m_nationId;
    public string m_nationName;
}
public class WorldCupNationCFG : XmlDataTable<WorldCupNationCFG, int, WorldCupNationItem>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_Nation.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);
        XmlNode node = doc.SelectSingleNode("/Root");
        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            WorldCupNationItem tmp = new WorldCupNationItem();
            int sid = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_nationId = sid;
            tmp.m_nationName = Convert.ToString(node.Attributes["Nation"].Value);
            m_data.Add(sid, tmp);
        }
    }
}
/// //////////////////////////////////////////////////////////////////////////////////////
//世界杯比赛组别
public class WorldCupGroup
{
    public int m_groupId;
    public string m_groupName;
}
public class WorldCupGroupCFG : XmlDataTable<WorldCupGroupCFG, int, WorldCupGroup>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_Group.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);
        XmlNode node = doc.SelectSingleNode("/Root");
        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            WorldCupGroup tmp = new WorldCupGroup();
            int sid = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_groupId = sid;
            tmp.m_groupName = Convert.ToString(node.Attributes["Name"].Value);
            m_data.Add(sid, tmp);
        }
    }
}
//////////////////////////////////////////////////////////////////////////////
//集玩偶
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
//集玩偶捐赠
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
//限时活动
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
//金币变化详细
//M_QuestCFG.xml
public class MQuestCFGData
{
    public int m_questId;
    public string m_questDesc = "";
    public string m_awardItemIDs;
    public string m_awardItemCounts = "";
    public int m_type;
    public int m_group;
    public string m_name;
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
//金币变化详细
//M_DailyQuestCFG.xml
public class MDailyQuestCFG : XmlDataTable<MDailyQuestCFG, int, MQuestCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_DailyQuestCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            MQuestCFGData tmp = new MQuestCFGData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_questId = Convert.ToInt32(sid);
            tmp.m_group = Convert.ToInt32(node.Attributes["Group"].Value);
            tmp.m_name = Convert.ToString(node.Attributes["Name"].Value);
            tmp.m_awardItemIDs = Convert.ToString(node.Attributes["AwardItemIDs"].Value);
            tmp.m_awardItemCounts = Convert.ToString(node.Attributes["AwardItemCounts"].Value);
            m_data.Add(tmp.m_questId, tmp);
        }
    }
}
//////////////////////////////////////////////////////////////////////////////////////////////////
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

////////////////////////////////////////////////////////////////////////////////////////
public class Pay_SdkItem
{
    public string m_key;
    public string m_value;
    public string m_fieldName;
};

public class Pay_SdkCFG : XmlDataTable<Pay_SdkCFG, string, Pay_SdkItem>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "pay_sdk.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            Pay_SdkItem tmp = new Pay_SdkItem();
            tmp.m_key = node.Attributes["key"].Value;
            tmp.m_value = node.Attributes["value"].Value;
            tmp.m_fieldName=node.Attributes["fieldName"].Value;
            m_data.Add(tmp.m_key, tmp);
        }
    }
}

//新春充返配置表
public class Act_NyAccCFGItem 
{
    public int m_itemId;
    public int m_rechargeCount;
};
public class Act_NyAccRechargeCFG : XmlDataTable<Act_NyAccRechargeCFG, int, Act_NyAccCFGItem>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_ActivityNYAccRechargeCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            Act_NyAccCFGItem tmp = new Act_NyAccCFGItem();
            tmp.m_itemId = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_rechargeCount = Convert.ToInt32(node.Attributes["RechargeRMB"].Value);
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

// 渠道相关的数据统计
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

// 全渠道中去除的渠道
public class TdChannelExcept : XmlDataTable<TdChannelExcept, string, TdChannelInfo>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "td_channel_except.xml");
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

//活动幸运宝箱
public class PanicBoxInfo 
{
    public int m_boxId;
    public string m_awardsItemIds;
    public string m_awardsItemCounts;
}

public class PanicBoxCFG : XmlDataTable<PanicBoxCFG, int, PanicBoxInfo>, IXmlData 
{
    public void init() 
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path,"M_ActivityPanicBuyingBoxCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");
        for (node = node.FirstChild; node !=null; node = node.NextSibling ) 
        {
            PanicBoxInfo tmp = new PanicBoxInfo();
            tmp.m_boxId = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_awardsItemIds = node.Attributes["AwardItemIDs"].Value;
            tmp.m_awardsItemCounts = node.Attributes["AwardItemCounts"].Value;
            m_data.Add(tmp.m_boxId,tmp);
        }
    }
}

///////////////////////////////////////////
//追击蟹将活动统计抽奖
public class FishActKillCrabLotteryCFG : XmlDataTable<FishActKillCrabLotteryCFG, int, PanicBoxInfo>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fish_ActivityKillCrabLotteryCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");
        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            PanicBoxInfo tmp = new PanicBoxInfo();
            tmp.m_boxId = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_awardsItemIds = node.Attributes["AwardItemIDs"].Value;
            tmp.m_awardsItemCounts = node.Attributes["AwardItemCounts"].Value;
            m_data.Add(tmp.m_boxId, tmp);
        }
    }
}

//追击蟹将活动任务统计
public class FishActKillCrabTaskCFG : XmlDataTable<FishActKillCrabTaskCFG, int, PanicBoxInfo>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fish_ActivityKillCrabLifeValueCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");
        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            PanicBoxInfo tmp = new PanicBoxInfo();
            tmp.m_boxId = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_awardsItemIds = node.Attributes["RewardItem"].Value;
            tmp.m_awardsItemCounts = node.Attributes["RewardCount"].Value;
            m_data.Add(tmp.m_boxId, tmp);
        }
    }
}
/////////////////////////////////////////////

//幸运抽奖奖励
public class LuckyDrawInfo 
{
    public int m_indexId;
    public int m_itemId;
    public int m_itemCount;
}
public class LuckyDrawCFG : XmlDataTable<LuckyDrawCFG, int, LuckyDrawInfo>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fish_LuckyDrawCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");
        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            LuckyDrawInfo tmp = new LuckyDrawInfo();
            tmp.m_indexId = Convert.ToInt32(node.Attributes["Index"].Value);
            tmp.m_itemId = Convert.ToInt32(node.Attributes["ItemType"].Value);
            tmp.m_itemCount = Convert.ToInt32(node.Attributes["ItemCount"].Value);
            m_data.Add(tmp.m_indexId, tmp);
        }
    }
}

//国庆中秋快乐抽奖
public class FishNationDay2018LotteryCFG : XmlDataTable<FishNationDay2018LotteryCFG, int, PanicBoxInfo>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fish_NationDay2018LotteryCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");
        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            PanicBoxInfo tmp = new PanicBoxInfo();
            tmp.m_boxId = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_awardsItemIds = node.Attributes["AwardItemIDs"].Value;
            tmp.m_awardsItemCounts = node.Attributes["AwardItemCounts"].Value;
            m_data.Add(tmp.m_boxId, tmp);
        }
    }
}

//捕鱼达人2渠道相关数据统计
public class TdChannelBuyu2 : XmlDataTable<TdChannelBuyu2, string, TdChannelInfo>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "td_channel_buyuDr2.xml");
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


//捕鱼达人IOS渠道相关数据统计
public class TdChannelBuyuIOS : XmlDataTable<TdChannelBuyuIOS, string, TdChannelInfo>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "td_channel_buyuDrIOS.xml");
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

//捕鱼达人Android渠道相关数据统计
public class TdChannelBuyuAndroid : XmlDataTable<TdChannelBuyuAndroid, string, TdChannelInfo>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "td_channel_buyuDrAndroid.xml");
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

//自由渠道相关数据统计
public class TdChannelZiyou : XmlDataTable<TdChannelZiyou, string, TdChannelInfo>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "td_channel_ziyou.xml");
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
    public int m_itemCount;
};

public class ExchangeCfg : XmlDataTable<ExchangeCfg, int, ExchangeData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_ExchangeCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            ExchangeData tmp = new ExchangeData();
            string sid = node.Attributes["ChangeId"].Value;
            tmp.m_id = Convert.ToInt32(sid);
            tmp.m_name = node.Attributes["ItemName"].Value;
            tmp.m_itemCount = Convert.ToInt32(node.Attributes["ItemCount"].Value);
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

//炮倍等级
public class Fish_TurretLevelCFG : XmlDataTable<Fish_TurretLevelCFG, int, Fish_LevelCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fish_TurretLevelCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            Fish_LevelCFGData tmp = new Fish_LevelCFGData();
            string sid = node.Attributes["ID"].Value;
            tmp.m_level = Convert.ToInt32(sid);
            tmp.m_openRate = Convert.ToInt32(node.Attributes["OpenRate"].Value);
            m_data.Add(tmp.m_level, tmp);
        }
    }
}

//炮倍等级，根据炮倍获取等级
public class Fish_TurretOpenRateCFG : XmlDataTable<Fish_TurretOpenRateCFG, int, Fish_LevelCFGData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fish_TurretLevelCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            Fish_LevelCFGData tmp = new Fish_LevelCFGData();
            string openRate = node.Attributes["OpenRate"].Value;
            tmp.m_openRate = Convert.ToInt32(openRate);
            tmp.m_level = Convert.ToInt32(node.Attributes["ID"].Value);
            m_data.Add(tmp.m_openRate, tmp);
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
//弹头排行
public class F_BulletHeadRewardData 
{
    public string m_RewardList;
    public string m_RewardCount;
    public int m_RankType;
    public int m_StartRank;
    public int m_EndRank;
    public int m_type;
}
public class F_TORPEDO_RANK_REWARDCFG : XmlDataTable<F_TORPEDO_RANK_REWARDCFG, int, F_BulletHeadRewardData>, IXmlData
{
    public void init() 
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path,"Fish_TorpedoRankRewardCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);
        XmlNode node = doc.SelectSingleNode("/Root");
        for (node = node.FirstChild; node!=null; node=node.NextSibling) 
        {
            F_BulletHeadRewardData tmp = new F_BulletHeadRewardData();
            int sid = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_RewardList = node.Attributes["RewardList"].Value;
            tmp.m_RewardCount = node.Attributes["RewardCount"].Value;
            tmp.m_RankType = Convert.ToInt32(node.Attributes["RankType"].Value);
            tmp.m_StartRank = Convert.ToInt32(node.Attributes["StartRank"].Value);
            tmp.m_EndRank = Convert.ToInt32(node.Attributes["EndRank"].Value);
            tmp.m_type = Convert.ToInt32(node.Attributes["Type"].Value);
            m_data.Add(sid,tmp);
        }
    }
}

//龙鳞排行
public class F_DRAGON_SCALE_RANK_REWARDCFG : XmlDataTable<F_DRAGON_SCALE_RANK_REWARDCFG, int, F_BulletHeadRewardData>, IXmlData
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "Fish_SquamaRankRewardCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);
        XmlNode node = doc.SelectSingleNode("/Root");
        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            F_BulletHeadRewardData tmp = new F_BulletHeadRewardData();
            int sid = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_RewardList = node.Attributes["RewardList"].Value;
            tmp.m_RewardCount = node.Attributes["RewardCount"].Value;
            tmp.m_StartRank = Convert.ToInt32(node.Attributes["StartRank"].Value);
            tmp.m_EndRank = Convert.ToInt32(node.Attributes["EndRank"].Value);
            m_data.Add(sid, tmp);
        }
    }
}
///////////////////////////////////////////////////////////////////////////
//五一充返活动
public class WuyiRewardData 
{
    public int m_itemId;
    public int m_itemCount;
}
public class M_WUYI_RECHARGECFG : XmlDataTable<M_WUYI_RECHARGECFG, int, WuyiRewardData>, IXmlData 
{
    public void init()
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path,"M_ActivityWuyiRechargeCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);
        XmlNode node = doc.SelectSingleNode("/Root");
        for (node = node.FirstChild; node!=null; node=node.NextSibling) 
        {
            WuyiRewardData tmp = new WuyiRewardData();
            int sid = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_itemId = Convert.ToInt32(node.Attributes["ItemId"].Value);
            tmp.m_itemCount = Convert.ToInt32(node.Attributes["ItemCount"].Value);
            m_data.Add(sid,tmp);
        }
    }
}
//////////////////////////////////////////////////////////////////////////
//新手引导埋点
public class PumpNewGuideData 
{
    public int m_itemId;
    public string m_itemName;
}
public class M_GUIDECFG : XmlDataTable<M_GUIDECFG, int, PumpNewGuideData>, IXmlData 
{
    public void init() 
    {
        string path = HttpRuntime.BinDirectory + "..\\" + "data";
        string file = Path.Combine(path, "M_GuideCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);
        XmlNode node = doc.SelectSingleNode("/Root");
        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            PumpNewGuideData tmp = new PumpNewGuideData();
            int sid = Convert.ToInt32(node.Attributes["ID"].Value);
            tmp.m_itemId = sid;
            tmp.m_itemName = Convert.ToString(node.Attributes["Name"].Value);
            m_data.Add(sid, tmp);
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 权限基本信息
public class RightBaseInfo
{
    // 权限id
    public string m_rightId;
    // 名称
    public string m_rightName;
    // 所属类别
    public string m_category;
}
