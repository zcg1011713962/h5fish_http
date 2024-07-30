using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

public struct JsonCfg
{
    public const string JSON_CONFIG = "config.json";
}

public class ResMgr
{
    static ResMgr s_obj = new ResMgr();

    protected Dictionary<string, object> m_dic = new Dictionary<string, object>();
    protected string m_workPath;

    public static ResMgr getInstance()
    {
        return s_obj;
    }

    public void init()
    {
        m_workPath = "..\\data\\";

        string json = File.ReadAllText(m_workPath + JsonCfg.JSON_CONFIG);
        var jsonObj = JsonHelper.ParseFromStr<Dictionary<string, object>>(json);
        m_dic.Add(JsonCfg.JSON_CONFIG, jsonObj);
    }

    public Dictionary<string, object> getJson(string cfg)
    {
        if (m_dic.ContainsKey(cfg))
        {
            return (Dictionary<string, object>)m_dic[cfg];
        }

        return null;
    }

    public string getString(string cfgFile, string key)
    {
        Dictionary<string, object> d = getJson(cfgFile);
        if (d == null)
            return "";

        if (!d.ContainsKey(key))
            return "";

        return Convert.ToString(d[key]);
    }

    public bool getBool(string cfgFile, string key)
    {
        Dictionary<string, object> d = getJson(cfgFile);
        if (d == null)
            return false;

        if (!d.ContainsKey(key))
            return false;

        return Convert.ToBoolean(d[key]);
    }
}
