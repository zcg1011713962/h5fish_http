using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using StackExchange.Redis;
using WebManager;

public struct RedisCC
{
    public const string REDIS_GRAND_PRIX = "grand_prix";
    public const string REDIS_QUALIFY_PRIX = "qualify_prix";

    public static string[] REDIS_ALL = { REDIS_GRAND_PRIX, REDIS_QUALIFY_PRIX };

    public const string GLOBAL_REDIS = "global";
}

public class RValue
{
    public object m_key;
    public object m_value;
}

public class RedisMgr
{
    protected Dictionary<string, RedisServerGroup> m_servers = new Dictionary<string, RedisServerGroup>();
    static object s_lock = new object();

    public static RedisMgr getInstance()
    {
        return Global.m_redisMgr;
    }

    // ip端口形式
    public RedisServerGroup getRedisServer(string ip)
    {
        RedisServerGroup svr = null;

        Dictionary<string, RedisInfo> allInfo = null;
        if (ip == RedisCC.GLOBAL_REDIS) // 全局的
        {
            allInfo = ResMgr.getInstance().m_redisInfo;
        }
        else
        {
            allInfo = parse(ip);
        }

        if (m_servers.ContainsKey(ip))
        {
            svr = m_servers[ip];
            if (svr == null)
            {
                svr = new RedisServerGroup();
                svr.initGroup(allInfo);
                lock (s_lock)
                {
                    m_servers[ip] = svr;
                }
            }
        }
        else
        {
            svr = new RedisServerGroup();
            svr.initGroup(allInfo);
            lock (s_lock)
            {
                m_servers[ip] = svr;
            }
        }

        return svr;
    }

    Dictionary<string, RedisInfo> parse(string ip)
    {
        Dictionary<string, RedisInfo> ret = new Dictionary<string, RedisInfo>();

        string[] arr = ip.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < arr.Length; i++)
        {
            string[] tmp = arr[i].Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
            RedisInfo info = new RedisInfo();
            info.m_redisName = tmp[0];
            info.m_redisIP = tmp[1];
            ret.Add(info.m_redisName, info);
        }

        return ret;
    }
}

///////////////////////////////////////////////////////////
public class RedisServerGroup
{
    // redis name -> server服务器对应
    protected Dictionary<string, RedisServer> m_svr = new Dictionary<string, RedisServer>();

    public void initGroup(Dictionary<string, RedisInfo> allRedis)
    {
        foreach (var d in allRedis)
        {
            RedisServer svr = new RedisServer();
            if (!string.IsNullOrEmpty(d.Value.m_redisIP))
            {
                svr.connect(d.Value.m_redisIP);
                m_svr.Add(d.Value.m_redisName, svr);
            }
        }
    }

    public RedisServer findRedisServer(string redisName)
    {
        if (m_svr.ContainsKey(redisName))
        {
            return m_svr[redisName];
        }

        return null;
    }

    // redisName redis服务器名称
    public void zadd(string redisName, string ztableName, string key, double score)
    {
        RedisServer svr = findRedisServer(redisName);
        if (svr != null)
        {
            svr.zadd(ztableName, key, score);
        }
    }
}

///////////////////////////////////////////////////////////
// 服务器
public class RedisServer
{
    protected ConnectionMultiplexer m_conn;
    protected IDatabase m_db;

    public IDatabase CurDb
    {
        get { return m_db; }
    }

    public static RedisMgr getInstance()
    {
        return WebManager.Global.m_redisMgr;
    }

    public bool connect(string ip)
    {
        try
        {
            var options = ConfigurationOptions.Parse(ip);
            m_conn = ConnectionMultiplexer.Connect(options);
            m_db = m_conn.GetDatabase(0);

            //zadd("Data", "1", 1);
            //zadd("Data", "2", 2);
            //zadd("Data", "3", 3);
            //zadd("Data", "4", 4);
            //
            //zrevrange("Data", 0, -1, true);

            return true;
        }
        catch (Exception ex)
        {
            CLOG.Info(ex.ToString());
        }

        return false;
    }

    // 向有序集中增加数据
    public void zadd(string ztableName, string key, double score)
    {
        try
        {
            m_db.SortedSetAdd(ztableName, new RedisValue(key), score);
        }
        catch(Exception ex)
        {
            CLOG.Info(ex.ToString());
        }
    }

    // ZREVRANGE salary 0 -1 WITHSCORES 
    public List<RValue> zrevrange(string ztableName, int start, int top, bool withScore = false)
    {
        List<RValue> result = new List<RValue>();

        try
        {
            if (withScore)
            {
                SortedSetEntry[] vals = m_db.SortedSetRangeByRankWithScores(ztableName, start, top, Order.Descending);
                for (int i = 0; i < vals.Length; i++)
                {
                    string str = vals[i].Element.ToString();
                    RValue v = new RValue();
                    v.m_key = str;
                    v.m_value = vals[i].Score;
                    result.Add(v);
                }
            }
            else
            {
                RedisValue[] vals = m_db.SortedSetRangeByRank(ztableName, start, top, Order.Descending);
                for (int i = 0; i < vals.Length; i++)
                {
                    string str = vals[i].ToString();
                    RValue v = new RValue();
                    v.m_key = str;
                    v.m_value = null;
                    result.Add(v);
                }
            }
        }
        catch(Exception ex)
        {
        }
       
        return result;
    }
}