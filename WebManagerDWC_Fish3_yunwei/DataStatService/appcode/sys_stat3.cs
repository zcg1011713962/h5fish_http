using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

// 每天统计一次玩家账富榜
class StatPlayerMoneyRank : StatByDayBase
{
    string[] S_FIELDS_GOLD = { "player_id", "nickname", "gold" };
    string[] S_FIELDS_DB = { "player_id", "nickname", "dragonBall" };

    public override string getStatKey()
    {
        return StatKey.KEY_PLAYER_MONEY_RANK;
    }

    public override void update(double delta)
    {
        if (DateTime.Now < m_statDay.AddMinutes(1))
            return;

        beginStat("StatPlayerMoneyRank开始统计");
        stat();
        addStatDay();
        endStat("StatPlayerMoneyRank结束统计");
    }

    void stat()
    {
        statRank("playerMoneyRank_Gold",S_FIELDS_GOLD, "gold");
        statRank("playerMoneyRank_DragonBall",S_FIELDS_DB, "dragonBall");
    }

    void statRank(string saveToTable, string[] retFields, string sortFieldName)
    {
        IMongoQuery imq = Query.EQ("is_robot", false);

        List<BsonDocument> rankList = new List<BsonDocument>();
        List<Dictionary<string, object>> dic = MongodbPlayerSlave.Instance.ExecuteGetListByQuery(TableName.PLAYER_INFO, imq, retFields, sortFieldName, false, 0, 100);
        if (dic != null)
        {
            DateTime genTime = m_statDay.AddDays(-1);
            for (int i = 0; i < dic.Count; i++)
            {
                Dictionary<string, object> cur = dic[i];
                Dictionary<string, object> d = new Dictionary<string, object>();
                d.Add("genTime", genTime);
                d.Add("playerId", Convert.ToInt32(cur["player_id"]));
                if (cur.ContainsKey("nickname"))
                {
                    d.Add("nickname", Convert.ToString(cur["nickname"]));
                }
                else
                {
                    d.Add("nickname", "");
                }
                if (cur.ContainsKey(sortFieldName))
                {
                    d.Add("totalValue", Convert.ToInt32(cur[sortFieldName]));
                }
                else
                {
                    d.Add("totalValue", 0);
                }
                
                d.Add("rank", i + 1);
                rankList.Add(d.ToBsonDocument());
            }

            MongodbLog.Instance.ExecuteInsterList(saveToTable, rankList);
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 统计小游戏DAU人数
class StatMiniGameDau : StatByDayBase
{
    public override string getStatKey()
    {
        return StatKey.KEY_MINI_GAME_DAU;
    }

    public override void update(double delta)
    {
        if (DateTime.Now < m_statDay.AddMinutes(20))
            return;

        beginStat("StatMiniGameDau开始统计");
        stat();
        addStatDay();
        endStat("StatMiniGameDau结束统计");
    }

    void stat()
    {
        DateTime genTime = m_statDay.AddDays(-1);
        IMongoQuery imq = Query.EQ("genTime", genTime);
        int count = MongodbLogSlave.Instance.ExecuteDistinct("pumpMiniGameDau", "playerId", imq);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("Date", genTime);
        data.Add("dau", count);
        MongodbLog.Instance.ExecuteInsert("statMiniGameDau", data);
        MongodbLog.Instance.ExecuteRemoveByQuery("pumpMiniGameDau", imq);
    }
}

//////////////////////////////////////////////////////////////////////////
class StatMoneyRep : StatByDayBase
{
    public override string getStatKey()
    {
        return StatKey.KEY_PLAYER_MONEY_REP;
    }

    public override void update(double delta)
    {
        if (DateTime.Now < m_statDay.AddMinutes(1))
            return;

        beginStat("StatMoneyRep开始统计");
        stat();
        addStatDay();
        endStat("StatMoneyRep结束统计");
    }

    void stat()
    {
        MapReduceResult mapResult = MongodbPlayerSlave.Instance.executeMapReduce(TableName.PLAYER_INFO,
                                                                    null,
                                                                    MapReduceTable.getMap("StatMoneyRep"),
                                                                    MapReduceTable.getReduce("StatMoneyRep"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();

            foreach (BsonDocument d in bson)
            {
                BsonValue resValue = d["value"];

                addData(0, resValue["all"]);
                addData(1, resValue["fact"]);
                addData(2, resValue["big"]);
            }
        }
    }

    // 0:all, 1:fact, 2:big
    public void addData(int repType, BsonValue resValue)
    {
        long gold = resValue["gold"].ToInt64();
        long dragonBall = resValue["dragon"].ToInt64();
        DateTime genTime = m_statDay.AddDays(-1);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("genTime", genTime);
        data.Add("gold", gold);
        data.Add("dragonBall", dragonBall);
        data.Add("repType", repType);
        MongodbLog.Instance.ExecuteInsert("statPlayerMoneyRep", data);
    }
}
