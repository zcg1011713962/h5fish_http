using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class DataInfo
{
    public DateTime m_endTime;
    public int m_playerId;
    public long m_winGold;
}

public class CacheData
{
    Dictionary<int, DataInfo> m_datas = new Dictionary<int, DataInfo>();
    static object s_obj = new object();

    public bool getWinGold(int playerId, ref long outGold)
    {
        DataInfo info = null;
        if (m_datas.ContainsKey(playerId))
        {
            info = m_datas[playerId];

            DateTime now = DateTime.Now;
            if (now > info.m_endTime)
            {
                info = null;
                lock (s_obj)
                {
                    m_datas.Remove(playerId);
                }
            }
        }

        if (info != null)
        {
            outGold = info.m_winGold;
            return true;
        }

        return false;
    }

    public void addWinGold(int playerId, long gold)
    {
        lock (s_obj)
        {
            DataInfo info = new DataInfo();
            info.m_playerId = playerId;
            info.m_winGold = gold;
            int dv = new Random().Next(3, 5);
            info.m_endTime = DateTime.Now.AddMinutes(dv);
            m_datas[playerId] = info;
        }
    }
}

//////////////////////////////////////////////////////////
public class TDataInfo
{
    public DateTime m_endTime;
    public int m_playerId;
    public object m_data;    // 具体数据
}

public class TPlayerInfo
{
    public long m_winGold;
    public long m_recharge;
}

public class TCacheData
{
    Dictionary<int, TDataInfo> m_datas = new Dictionary<int, TDataInfo>();
    static object s_obj = new object();

    public bool getWinGold(int playerId, ref TDataInfo outGold)
    {
        TDataInfo info = null;
        if (m_datas.ContainsKey(playerId))
        {
            info = m_datas[playerId];

            DateTime now = DateTime.Now;
            if (now > info.m_endTime)
            {
                info = null;
                lock (s_obj)
                {
                    m_datas.Remove(playerId);
                }
            }
        }

        if (info != null)
        {
            outGold = info;
            return true;
        }

        return false;
    }

    public void addWinGold(int playerId, object gold)
    {
        lock (s_obj)
        {
            TDataInfo info = new TDataInfo();
            info.m_playerId = playerId;
            info.m_data = gold;
            int dv = new Random().Next(3, 5);
            info.m_endTime = DateTime.Now.AddMinutes(dv);
            m_datas[playerId] = info;
        }
    }
}

