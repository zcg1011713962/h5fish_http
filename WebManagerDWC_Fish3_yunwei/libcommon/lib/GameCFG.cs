using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

public interface IXmlData
{
    void init();
}

public class XmlDataTable<T, KEY, VALUE> where T : IXmlData, new()
{
    public static T s_obj = default(T);

    protected Dictionary<KEY, VALUE> m_data = new Dictionary<KEY, VALUE>();

    public static T getInstance()
    {
        if (s_obj == null)
        {
            s_obj = new T();
            s_obj.init();
        }
        return s_obj;
    }

    public VALUE getValue(KEY k)
    {
        if (m_data.ContainsKey(k))
            return m_data[k];

        return default(VALUE);
    }

    public Dictionary<KEY, VALUE> getAllData()
    {
        return m_data;
    }
}

public class ItemCFGData
{
    public int m_itemId;
    public string m_itemName = "";
    public int m_itemCount;
    public string m_gameItem;
    public string m_goldValue;
}

// 日常或成就相关
public class QusetCFGData
{
    // 任务ID
    public int m_questId;
    // 任务类型
    public int m_questType;
    // 任务名称
    public string m_questName = "";
}

public enum TaskType
{
    taskTypeDaily = 1,
    taskTypeAchieve = 2,
}

// 鱼表
public class FishCFGData
{
    // 鱼ID
    public int m_fishId;

    // 鱼名称
    public string m_fishName = "";
}

// 鳄鱼数据
public class Crocodile_RateCFGData
{
    // 区域ID
    public int m_areaId;

    // 名称
    public string m_name = "";

    public string m_icon;
    public string m_color;
}
//通过POSID获取probID和icon
public class Fruit_PosData
{
    public int m_posId;
    // 名称
    public string m_icon;
}
public class Dice_BetAreaCFGData
{
    // 区域ID
    public int m_areaId;

    // 名称
    public string m_name;

    public int m_span = 0;

    public string m_desc;
}

public class ReadMapReduce
{
    public string m_map = "";
    public string m_reduce = "";

    public bool load(string file)
    {
        if (!File.Exists(file))
        {
            LOG.Info("文件[{0}]不存在!", file);
            return false;
        }

        int state = 0;
        
        StreamReader sr = new StreamReader(file, Encoding.UTF8);
        while (true)
        {
            string line = sr.ReadLine();
            if(line == null)
                break;

            switch (state)
            {
                case 0: // 找map start，或reduce start
                    {
                        if (line == "/*map start*/")
                        {
                            state = 1;
                        }
                        else if (line == "/*reduce start*/")
                        {
                            state = 2;
                        }
                    }
                    break;
                case 1: // 读map
                    {
                        if (line == "/*map end*/")
                        {
                            state = 0;
                        }
                        else
                        {
                            m_map += line + "\n";
                        }
                    }
                    break;
                case 2:
                    {
                        if (line == "/*reduce end*/")
                        {
                            state = 0;
                        }
                        else
                        {
                            m_reduce += line + "\n";
                        }
                    }
                    break;
            }
        }
        
        sr.Close();
        return true;
    }
}

//////////////////////////////////////////////////////////////////////////
public class CMemoryBuffer
{
    MemoryStream m_ms = new MemoryStream();
    BinaryWriter m_writer;
    BinaryReader m_reader;
    object m_param1;
    object m_param2;

    public CMemoryBuffer()
    {
        m_writer = new BinaryWriter(m_ms);
        m_reader = new BinaryReader(m_ms);
    }

    public BinaryWriter Writer { get { return m_writer; } }

    public BinaryReader Reader { get { return m_reader; } }

    public object Param1
    {
        get { return m_param1; }
        set { m_param1 = value; }
    }

    public object Param2
    {
        get { return m_param2; }
        set { m_param2 = value; }
    }

    public void begin()
    {
        m_ms.Seek(0, SeekOrigin.Begin);
    }
}
