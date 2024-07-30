using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public struct RRDbNam
{
    // 总在线
    public const string RRD_DB_TOTAL = "dbTotal.rrd";

    // 捕鱼总在线
    public const string RRD_DB_FISH_TOTAL = "dbFishTotal.rrd";

    public const string RRD_DB_FISH_ROOM1 = "dbFishRoom1.rrd";

    public const string RRD_DB_FISH_ROOM2 = "dbFishRoom2.rrd";

    public const string RRD_DB_FISH_ROOM3 = "dbFishRoom3.rrd";

    public const string RRD_DB_FISH_ROOM4 = "dbFishRoom4.rrd";

    public const string RRD_DB_FISH_ROOM5 = "dbFishRoom5.rrd";

    public const string RRD_DB_FISH_ROOM6 = "dbFishRoom6.rrd";

    public const string RRD_DB_FISH_ROOM20 = "dbFishRoom20.rrd";

    public const string RRD_DB_CROCODILE = "dbCrocodile.rrd";

    public const string RRD_DB_COW = "dbCow.rrd";

    public const string RRD_DB_SHCD = "dbShcd.rrd";

    public const string RRD_DB_SHUIHZ = "dbShuihz.rrd";

    public const string RRD_DB_BZ = "dbbz.rrd";

    public const string RRD_DB_FRUIT = "dbFruit.rrd";

    public const string RRD_DB_JEWEL = "dbJewel.rrd";
}

public class OnlineMgr
{
    static int[] FISH_ROOMS = { 1, 2, 3, 4, 5, 6 };

    Dictionary<int, RRDOnlinePerson> m_dic = new Dictionary<int, RRDOnlinePerson>();

    Dictionary<int, string> m_dbName = new Dictionary<int, string>();

    public DateTime StartTime { set; get; }

    public string RRDToolPath { set; get; }

    public OnlineMgr()
    {
        StartTime = DateTime.Now.Date;
    }

    public void init()
    {
        initDbName();

        for (int i = 0; i < StrName.s_onlineGameIdList.Length; i++)
        {
            if (StrName.s_onlineGameIdList[i] == (int)GameId.fishlord)
            {
                for (int k = 0; k < FISH_ROOMS.Length; k++)
                {
                    add((int)StrName.s_onlineGameIdList[i], FISH_ROOMS[k]);
                }
            }
            else
            {
                add((int)StrName.s_onlineGameIdList[i]);
            }
        }
    }

    public string getDbName(int gameId, int roomId)
    {
        int id = genId(gameId, roomId);
        if (m_dbName.ContainsKey(id))
        {
            return m_dbName[id];
        }
        return "";
    }

    public string getDbName(int id)
    {
        if (m_dbName.ContainsKey(id))
        {
            return m_dbName[id];
        }
        return "";
    }

    public int genId(int gameId, int roomId = 0)
    {
        return gameId * 100 + roomId;
    }

    public RRDOnlinePerson getRRd(int gameId, int roomId)
    {
        RRDOnlinePerson p = null;
        int id = genId(gameId, roomId);
        if (m_dic.ContainsKey(id))
        {
            p = m_dic[id];
        }

        return p;
    }

    void add(int gameId, int roomId = 0)
    {
        int id = genId(gameId, roomId);
        string dbName = getDbName(id);
        if (string.IsNullOrEmpty(dbName))
            return;

        var r = new RRDOnlinePerson();
        r.DbFileName = Path.Combine(RRDToolPath, dbName);
        r.StartTime = StartTime;
        r.RRDToolPath = RRDToolPath;
        r.createDbFile();
        m_dic.Add(id, r);
    }

    void initDbName()
    {
        addName(0, 0, RRDbNam.RRD_DB_TOTAL);

        addName((int)(GameId.fishlord), 0, RRDbNam.RRD_DB_FISH_TOTAL);
        addName((int)(GameId.fishlord), 1, RRDbNam.RRD_DB_FISH_ROOM1);
        addName((int)(GameId.fishlord), 2, RRDbNam.RRD_DB_FISH_ROOM2);
        addName((int)(GameId.fishlord), 3, RRDbNam.RRD_DB_FISH_ROOM3);
        addName((int)(GameId.fishlord), 4, RRDbNam.RRD_DB_FISH_ROOM4);
        addName((int)(GameId.fishlord), 5, RRDbNam.RRD_DB_FISH_ROOM5);
        addName((int)(GameId.fishlord), 6, RRDbNam.RRD_DB_FISH_ROOM6);
       // addName((int)(GameId.fishlord),20,  RRDbNam.RRD_DB_FISH_ROOM20);

        /*addName((int)(GameId.crocodile), 0, RRDbNam.RRD_DB_CROCODILE);

        addName((int)(GameId.cows), 0, RRDbNam.RRD_DB_COW);

        addName((int)(GameId.shcd), 0, RRDbNam.RRD_DB_SHCD);

        addName((int)(GameId.shuihz), 0, RRDbNam.RRD_DB_SHUIHZ);

        addName((int)(GameId.bz), 0, RRDbNam.RRD_DB_BZ);

        addName((int)(GameId.fruit), 0, RRDbNam.RRD_DB_FRUIT);

        addName((int)(GameId.jewel), 0, RRDbNam.RRD_DB_JEWEL);*/
    }

    void addName(int gameId, int roomId, string dbname)
    {
        int id = genId(gameId, roomId);
        m_dbName.Add(id, dbname);
    }
}


