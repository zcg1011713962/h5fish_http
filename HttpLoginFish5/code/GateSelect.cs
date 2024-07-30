using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Text;

// 选择gate服务器
public class GateSelector
{
    const string GATE_SELECT = "GateSelect";

    static int s_gateMaxPlayerCount = 0;

  //  StringBuilder m_builder = new StringBuilder();

    private Dictionary<string, object> m_dic = new Dictionary<string, object>();
    private List<Dictionary<string, object>> m_list = new List<Dictionary<string, object>>();

    static GateSelector()
    {
        string count = ConfigurationManager.AppSettings["gateMaxPlayerCount"];
        if (!string.IsNullOrEmpty(count))
        {
            s_gateMaxPlayerCount = Convert.ToInt32(count);
        }
    }

    public static bool isAutoSelGate()
    {
        return s_gateMaxPlayerCount > 0;
    }

    // 返回0成功， 1没有查到结果
    public int selectLoginGate(string channel)
    {
        int channelType = getChannelType(channel);
        IMongoQuery imq1 = Query.GTE("UpdateTime", DateTime.Now.AddSeconds(-120));
        IMongoQuery imq2 = Query.EQ("channel", channelType);
        IMongoQuery imq = Query.And(imq1, imq2);

        List<Dictionary<string, object>> dataList = MongodbConfig.Instance.ExecuteGetListByQuery(GATE_SELECT, imq, null, "GateServerId", true);
        if (dataList.Count == 0)
        {
            CLOG.Info("selectLoginGate not find");
            return 1;
        }

        //m_builder.AppendFormat("serverlist = {{}};");
        bool isFind = false;

        foreach (var data in dataList)
        {
            if (data.ContainsKey("CurPlayerCount"))
            {
                int cnt = Convert.ToInt32(data["CurPlayerCount"]);
                if (cnt < s_gateMaxPlayerCount)
                {
                    addGate(Convert.ToString(data["ServerIp"]), Convert.ToInt32(data["GateServerId"]));
                    isFind = true;
                    break;
                }
            }
        }

        if (!isFind)
        {
            Dictionary<string, object> d = getMinCount(dataList);
            if (d == null)
            {
                return 1;
            }

            addGate(Convert.ToString(d["ServerIp"]), Convert.ToInt32(d["GateServerId"]));
        }

       // m_builder.Append("return serverlist;");
        return 0;
    }

    public string getGate(HttpLogin.ServerList3 obj)
    {
//         if (m_list.Count <= 0)
//         {
//             m_dic.Add("sname", "");
//             m_dic.Add("sip", "");
//         }

        m_dic.Add("slist", m_list);
        string str = JsonHelper.genJson(m_dic);
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));

        //string str = JsonHelper.genJson(m_dic);
        //return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
    }

    void addGate(string ip, int gateId)
    {
        Dictionary<string, object> tmp = new Dictionary<string, object>();
        tmp.Add("sname", "g" + gateId.ToString());
        tmp.Add("sip", ip);
        m_list.Add(tmp);
        //m_builder.AppendFormat("serverlist[{0}] = {{ID={1}, serverName=\"gate-{2}\", serverIP=\"{3}\"}}", 1, 1, gateId, ip);
    }

    Dictionary<string, object> getMinCount(List<Dictionary<string, object>> dataList)
    {
        int minCount = 99999999;
        Dictionary<string, object> result = null;

        foreach (var data in dataList)
        {
            int cnt = Convert.ToInt32(data["CurPlayerCount"]);
            if (cnt < minCount)
            {
                minCount = cnt;
                result = data;
            }
        }

        return result;
    }

    int getChannelType(string channel)
    {
        return isIOS(channel) ? 1 : 0;
    }

    bool isIOS(string channel)
    {
        return false;
        return channel == "800149" || channel == "800152" || channel == "800159" ||
            channel == "800161" || channel == "800182";
    }
}

public class ServerListMgr
{
    private Dictionary<string, object> m_dic = new Dictionary<string, object>();

    private List<Dictionary<string, object>> m_list = new List<Dictionary<string, object>>();

    public void fetchList()
    {
        var ret = MongodbConfig.Instance.ExecuteGetAll("ServerList");
        if (ret != null)
        {
            foreach (var info in ret)
            {
                if (info.ContainsKey("name") && info.ContainsKey("ip"))
                {
                    Dictionary<string, object> tmp = new Dictionary<string, object>();
                    tmp.Add("sname", Convert.ToString(info["name"]));
                    tmp.Add("sip", Convert.ToString(info["ip"]));
                    m_list.Add(tmp);
                }
            }
        }
    }

    public string get()
    {
        m_dic.Add("slist", m_list);
        string str = JsonHelper.genJson(m_dic);
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
    }
}

    



