using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Xml;

/*public class DbServerInfo
{
    // �����ݿ�IP��PlayerDB����Ҳ��Ϊ�ؼ���
    public string m_serverIp = "";
    public int m_serverId;
    public string m_serverName = "";

    // ��־���ݿ�����IP
    public string m_logDbIp = "";
}*/

public class ResMgr
{
    private static ResMgr s_obj = null;
    // �������·��
    private string m_path;
    // �洢�������
    private Dictionary<string, XmlConfig> m_allRes = new Dictionary<string, XmlConfig>();
    private Dictionary<string, DbServerInfo> m_dbServer = new Dictionary<string, DbServerInfo>();
    private Dictionary<int, DbServerInfo> m_dbServerById = new Dictionary<int, DbServerInfo>();
    // �洢�������
    private Dictionary<string, IUserTabe> m_allTable = new Dictionary<string, IUserTabe>();

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
        m_path = @"..\data\";
    }

    // ���ñ������·��
    public void setPath(string path)
    {
        m_path = path;
    }

    private void init()
    {
        loadXmlConfig("dbserver.xml");
        loadXmlConfig("opres.xml");
        loadXmlConfig("money_reason.xml");

        setUpDbServerInfo();

        loadTable("map_reduce.csv", new MapReduceTable(), '$');
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

    public Dictionary<string, DbServerInfo> getAllDb()
    {
        return m_dbServer;
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

    public string getChannel()
    {
        XmlConfig xml = ResMgr.getInstance().getRes("dbserver.xml");
        string channel = xml.getString("channelNO", "channelName");
        return channel;
    }

    private void loadXmlConfig(string file)
    {
        XmlConfigMaker c = new XmlConfigMaker();
        string fullfile = Path.Combine(m_path, file);
        XmlConfig xml = c.loadFromFile(fullfile);
        if (xml != null)
        {
            m_allRes.Add(file, xml);
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
}

//////////////////////////////////////////////////////////////////////////

public class ItemCFG : XmlDataTable<ItemCFG, int, ItemCFGData>, IXmlData
{
    public void init()
    {
        string path = @"..\data\";
        string file = Path.Combine(path, "ItemCFG.xml");
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

///////////////////////////////////////////////////////////////////////////
//�ڱ��ȼ�
public class Fish_LevelCFGData
{
    public int m_level;
    public int m_openRate;
};
public class Fish_TurretLevelCFG : XmlDataTable<Fish_TurretLevelCFG, int, Fish_LevelCFGData>, IXmlData
{
    public void init()
    {
        string path = @"..\data\";
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

//�ڱ��ȼ��������ڱ���ȡ�ȼ�
public class Fish_TurretOpenRateCFG : XmlDataTable<Fish_TurretOpenRateCFG, int, Fish_LevelCFGData>, IXmlData
{
    public void init()
    {
        string path = @"..\data\";
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
//////////////////////////////////////////////////////////////////////////

public class QuestCFG : XmlDataTable<QuestCFG, int, QusetCFGData>, IXmlData
{
    // ÿ������
    private List<QusetCFGData> m_dailyTask = new List<QusetCFGData>();
    // �ɾ�
    private List<QusetCFGData> m_achiveTask = new List<QusetCFGData>();

    public void init()
    {
        string path = @"..\data\";
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
        if (taskType == TaskType.taskTypeDaily)
            return m_dailyTask;
        return m_achiveTask;
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
//��������/�ͷ��طø���
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
        string path = "..\\" + "data";
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
        string path = "..\\" + "data";
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
        string path ="..\\" + "data";
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
