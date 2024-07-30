using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

public class ExchangeActItem
{
    public int m_id;     // 兑换id
    public int m_count;  // 个数
}

public class ExchangeActInfo
{
    public DateTime m_time;

    // id-> actItem 对应
    public Dictionary<int, ExchangeActItem> m_data = new Dictionary<int, ExchangeActItem>();

    public void addItem(string key, object count)
    {
        if (key.StartsWith("compose_"))
        {
            ExchangeActItem item = null;
            int id = Convert.ToInt32(key.Substring("compose_".Length));
            if (m_data.ContainsKey(id))
            {
                item = m_data[id];
            }
            else
            {
                item = new ExchangeActItem();
                item.m_id = id;
                item.m_count = 0;
                m_data.Add(id, item);
            }
            if (item != null)
            {
                item.m_count += Convert.ToInt32(count);
            }
        }
    }
}

// 兑换活动查询
public class QueryExchangeActivity : QueryBase
{
    private List<ExchangeActInfo> m_result = new List<ExchangeActInfo>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, imq);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq)
    {
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_EXCHANGE_ACTIVITY, user.getDbServerID(),
             DbName.DB_PUMP, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            ExchangeActInfo tmp = new ExchangeActInfo();

            if (data.ContainsKey("genTime"))
            {
                tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            }
            else
            {
                continue;
            }
            foreach(var info in data)
            {
                tmp.addItem(info.Key, info.Value);
            }

            m_result.Add(tmp);
        }
        return OpRes.opres_success;
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (string.IsNullOrEmpty(p.m_time))
        {
            return OpRes.op_res_time_format_error;
        }

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        imq = Query.And(imq1, imq2);

        return OpRes.opres_success;
    }
}

////////////////////////////////////////////////////////////////////////////
public class TurretLotteryActItem
{
    public int m_id;     // 兑换id
    public int m_count;  // 个数
}

public class TurretLotteryActInfo
{
    public DateTime m_time;
    // 单次兑换
    public int m_lotteryOnce;
    // 10次兑换
    public int m_lotteryTen;

    // id-> actItem 对应
    public Dictionary<int, TurretLotteryActItem> m_data = new Dictionary<int, TurretLotteryActItem>();

    public void addItem(string key, object count)
    {
        if (key.StartsWith("reward_"))
        {
            TurretLotteryActItem item = null;
            string str = key.Substring("reward_".Length);
            if (string.IsNullOrEmpty(str))
                return;

            int id = Convert.ToInt32(str);
            if (m_data.ContainsKey(id))
            {
                item = m_data[id];
            }
            else
            {
                item = new TurretLotteryActItem();
                item.m_id = id;
                item.m_count = 0;
                m_data.Add(id, item);
            }
            if (item != null)
            {
                item.m_count += Convert.ToInt32(count);
            }
        }
    }
}

// 炮台抽奖活动查询
public class QueryTuretLotteryActivity : QueryBase
{
    private List<TurretLotteryActInfo> m_result = new List<TurretLotteryActInfo>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, imq);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq)
    {
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_TURRET_LOTTERY_ACTIVITY, user.getDbServerID(),
             DbName.DB_PUMP, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            TurretLotteryActInfo tmp = new TurretLotteryActInfo();

            if (data.ContainsKey("genTime"))
            {
                tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            }
            else
            {
                continue;
            }
            foreach (var info in data)
            {
                if (info.Key == "lotteryOnce")
                {
                    tmp.m_lotteryOnce = Convert.ToInt32(data["lotteryOnce"]);
                }
                else if (info.Key == "lotteryTenTimes")
                {
                    tmp.m_lotteryTen = Convert.ToInt32(data["lotteryTenTimes"]);
                }
                else if (info.Key != "genTime")
                {
                    tmp.addItem(info.Key, info.Value);
                }
            }

            m_result.Add(tmp);
        }
        return OpRes.opres_success;
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (string.IsNullOrEmpty(p.m_time))
        {
            return OpRes.op_res_time_format_error;
        }

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        imq = Query.And(imq1, imq2);

        return OpRes.opres_success;
    }
}

////////////////////////////////////////////////////////////////////////////
// 限时购活动查询
public class QueryLimitTimeBuyActivity : QueryBase
{
    private List<TurretLotteryActInfo> m_result = new List<TurretLotteryActInfo>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, imq);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq)
    {
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_LIMIT_TIME_ACTIVITY, user.getDbServerID(),
             DbName.DB_PUMP, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            TurretLotteryActInfo tmp = new TurretLotteryActInfo();

            if (data.ContainsKey("genTime"))
            {
                tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            }
            else
            {
                continue;
            }
            foreach (var info in data)
            {
                if (info.Key == "lotteryOnce")
                {
                    tmp.m_lotteryOnce = Convert.ToInt32(data["lotteryOnce"]);
                }
                else if (info.Key == "lotteryTenTimes")
                {
                    tmp.m_lotteryTen = Convert.ToInt32(data["lotteryTenTimes"]);
                }
                else if (info.Key != "genTime")
                {
                    tmp.addItem(info.Key, info.Value);
                }
            }

            m_result.Add(tmp);
        }
        return OpRes.opres_success;
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (string.IsNullOrEmpty(p.m_time))
        {
            return OpRes.op_res_time_format_error;
        }

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        imq = Query.And(imq1, imq2);

        return OpRes.opres_success;
    }
}

////////////////////////////////////////////////////////////////////////////
public class IntScoreRankInfo
{
    public string m_nickName;  // 玩家名称
    public int m_playerId;     // 玩家ID
    public long m_socre;        // 积分
    public int m_rank;
}

public class IntScoreRankList
{
    public List<IntScoreRankInfo> m_rank = new List<IntScoreRankInfo>();

    public DateTime m_time;
}

public class IntScoreWrap
{
    // 历史榜
    public Dictionary<DateTime, IntScoreRankList> m_dataDic = new Dictionary<DateTime, IntScoreRankList>();

    // 抽奖查询
    public List<TurretLotteryActInfo> m_lotteryList = new List<TurretLotteryActInfo>();

    // 当前榜
    public IntScoreRankList m_dataLine = new IntScoreRankList();

    public void addRankCur(IntScoreRankInfo info)
    {
        if (info != null)
        {
            m_dataLine.m_rank.Add(info);
        }
    }

    public void addRankHistory(DateTime time, IntScoreRankInfo info)
    {
        IntScoreRankList obj = null;
        if (m_dataDic.ContainsKey(time))
        {
            obj = m_dataDic[time];
        }
        else
        {
            obj = new IntScoreRankList();
            obj.m_time = time;
            m_dataDic.Add(time, obj);
        }
        if (obj != null)
        {
            obj.m_rank.Add(info);
        }
    }

    public void resetData()
    {
        m_dataDic.Clear();
        m_lotteryList.Clear();
        m_dataLine.m_rank.Clear();
    }
}

// 积分送大奖查询
public class QueryIntegrationScoreSendBigAward : QueryBase
{
    // 当前排行
    public const int RANK_CUR = 1;
    // 历史排行
    public const int RANK_HISTORY = 2;
    // 抽奖
    public const int LOTTERY = 3;

    IntScoreWrap m_result = new IntScoreWrap();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.resetData();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        switch (p.m_op)
        {
            case RANK_CUR: // 当前榜
                {
                    return queryRankCur(user, p, imq);
                }
                break;
            case RANK_HISTORY: // 历史榜
                {
                    return queryRankHistory(user, p, imq);
                }
                break;
            case LOTTERY: // 抽奖
                {
                    return queryLottery(user, p, imq);
                }
                break;
            default:
                return OpRes.op_res_failed;
                break;
        }

        return OpRes.opres_success;
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (p.m_op != RANK_CUR)
        {
            if (string.IsNullOrEmpty(p.m_time))
            {
                return OpRes.op_res_time_format_error;
            }

            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            imq = Query.And(imq1, imq2);
        }

        return OpRes.opres_success;
    }

    OpRes queryRankCur(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        IMongoQuery timq = Query.GT("rankPoints", 0);
        dataList = DBMgr.getInstance().executeQuery(TableName.ACTIVITY_INT_SCORE_RANK_CUR, dip, timq, 0, 100, null, "rankPoints", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();
            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if (data.ContainsKey("nickname"))
            {
                tmp.m_nickName = Convert.ToString(data["nickname"]);
            }
            if (data.ContainsKey("rankPoints"))
            {
                tmp.m_socre = Convert.ToInt32(data["rankPoints"]);
            }
            tmp.m_rank = i + 1;

            m_result.addRankCur(tmp);
        }

        return OpRes.opres_success;
    }

    OpRes queryRankHistory(GMUser user, ParamQuery param, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_INT_SCORE_RANK_HISTORY, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("rank");

        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery2(TableName.PUMP_INT_SCORE_RANK_HISTORY, dip, imq,
            (param.m_curPage - 1) * param.m_countEachPage,
            param.m_countEachPage, null, sort);

        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();
            if (data.ContainsKey("player_id"))
            {
                tmp.m_playerId = Convert.ToInt32(data["player_id"]);
            }
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if (data.ContainsKey("points"))
            {
                tmp.m_socre = Convert.ToInt32(data["points"]);
            }
            if (data.ContainsKey("rank"))
            {
                tmp.m_rank = Convert.ToInt32(data["rank"]);
            }

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            m_result.addRankHistory(time, tmp);
        }

        return OpRes.opres_success;
    }

    // 积分送大奖抽奖
    OpRes queryLottery(GMUser user, ParamQuery param, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        //user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.AREA_RANK_DAY_HISTORY, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime");

        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery2(TableName.PUMP_INT_SCORE_SEND_AWARD_LOTTERY, dip, imq,
            0,
            0, null, sort);

        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            TurretLotteryActInfo tmp = new TurretLotteryActInfo();
            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
                }
                else if (d.Key == "lotteryOnce")
                {
                    tmp.m_lotteryOnce = Convert.ToInt32(data["lotteryOnce"]);
                }
                else if (d.Key == "lotteryTenTimes")
                {
                    tmp.m_lotteryTen = Convert.ToInt32(data["lotteryTenTimes"]);
                }
                else
                {
                    tmp.addItem(d.Key, d.Value);
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }
}

////////////////////////////////////////////////////////////////////////////
// id到数量的关系
public class MapIdCount
{
    public string m_prefix = "";

    // id-> actItem 对应
    public Dictionary<int, TurretLotteryActItem> m_data = new Dictionary<int, TurretLotteryActItem>();

    public void addItem(string key, object count)
    {
        if (string.IsNullOrEmpty(m_prefix)) return;

        if (key.StartsWith(m_prefix))
        {
            TurretLotteryActItem item = null;
            string str = key.Substring(m_prefix.Length);
            if (string.IsNullOrEmpty(str))
                return;

            int id = Convert.ToInt32(str);
            if (m_data.ContainsKey(id))
            {
                item = m_data[id];
            }
            else
            {
                item = new TurretLotteryActItem();
                item.m_id = id;
                item.m_count = 0;
                m_data.Add(id, item);
            }
            if (item != null)
            {
                item.m_count += Convert.ToInt32(count);
            }
        }
    }

    public void addItem(int key, int count)
    {
        TurretLotteryActItem da = null;
        if (m_data.ContainsKey(key))
        {
            da = m_data[key];
        }
        else
        {
            da = new TurretLotteryActItem();
            da.m_id = key;
            m_data.Add(key, da);
        }
        da.m_count += count;
    }

    public int getCount(int key)
    {
        if (m_data.ContainsKey(key))
        {
            return m_data[key].m_count;
        }

        return 0;
    }

    public void clearData()
    {
        m_data.Clear();
    }
}

public class AttributeSetArray<T> where T :struct
{
    public T[] m_data = null;

    public void init(int count)
    {
        if (m_data == null)
        {
            m_data = new T[count];
        }
    }

    public void setValue(int index, T val)
    {
        m_data[index] = val;
    }

    public T getValue(int index)
    {
        return m_data[index];
    }
}

public class QrResultItem
{
    public DateTime m_time;

    public AttributeSetArray<long> m_data = new AttributeSetArray<long>();

    public MapIdCount m_mapCount = new MapIdCount();

    public void init(int attrCount, string prefix)
    {
        m_mapCount.m_prefix = prefix;
        m_data.init(attrCount);
    }

    public MapIdCount getMapCount() { return m_mapCount; }
}

////////////////////////////////////////////////////////////////////////////
public class ScoreWrap
{
    // 历史榜
    public Dictionary<DateTime, IntScoreRankList> m_dataDic = new Dictionary<DateTime, IntScoreRankList>();

    public List<QrResultItem> m_lotteryList = new List<QrResultItem>();

    // 当前榜
    public IntScoreRankList m_dataLine = new IntScoreRankList();

    public void addRankCur(IntScoreRankInfo info)
    {
        if (info != null)
        {
            m_dataLine.m_rank.Add(info);
        }
    }

    public void addRankHistory(DateTime time, IntScoreRankInfo info)
    {
        IntScoreRankList obj = null;
        if (m_dataDic.ContainsKey(time))
        {
            obj = m_dataDic[time];
        }
        else
        {
            obj = new IntScoreRankList();
            obj.m_time = time;
            m_dataDic.Add(time, obj);
        }
        if (obj != null)
        {
            obj.m_rank.Add(info);
        }
    }

    public void resetData()
    {
        m_dataDic.Clear();
        m_lotteryList.Clear();
        m_dataLine.m_rank.Clear();
    }
}

////////////////////////////////////////////////////////////////////////////
// 砸蛋统计查询
public class QueryBreakEgg : QueryBase
{
    ScoreWrap m_result = new ScoreWrap();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.resetData();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        switch (p.m_op)
        {
            case DefCC.QRY_RANK_CUR: // 当前榜
                {
                    return queryRankCur(user, p, imq);
                }
                break;
            case DefCC.QRY_RANK_HISTORY: // 历史榜
                {
                    return queryRankHistory(user, p, imq);
                }
                break;
            case DefCC.QRY_LOTTERY: // 砸蛋统计
                {
                    return breakLottery(user, p, imq);
                }
                break;
            default:
                return OpRes.op_res_failed;
                break;
        }

        return OpRes.opres_success;
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (p.m_op != DefCC.QRY_RANK_CUR)
        {
            if (string.IsNullOrEmpty(p.m_time))
            {
                return OpRes.op_res_time_format_error;
            }

            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            imq = Query.And(imq1, imq2);
        }

        return OpRes.opres_success;
    }

    OpRes queryRankCur(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        IMongoQuery timq = Query.GT("score", 0);
        dataList = DBMgr.getInstance().executeQuery(TableName.ACTIVITY_BREAK_EGG, dip, timq, 0, 100, null, "score", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();
            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if (data.ContainsKey("nickname"))
            {
                tmp.m_nickName = Convert.ToString(data["nickname"]);
            }
            if (data.ContainsKey("score"))
            {
                tmp.m_socre = Convert.ToInt32(data["score"]);
            }
            tmp.m_rank = i + 1;

            m_result.addRankCur(tmp);
        }

        return OpRes.opres_success;
    }

    OpRes queryRankHistory(GMUser user, ParamQuery param, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_BREAK_EGG_RANK, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("rank");

        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery2(TableName.PUMP_BREAK_EGG_RANK, dip, imq,
            (param.m_curPage - 1) * param.m_countEachPage,
            param.m_countEachPage, null, sort);

        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();
            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if (data.ContainsKey("score"))
            {
                tmp.m_socre = Convert.ToInt32(data["score"]);
            }
            if (data.ContainsKey("rank"))
            {
                tmp.m_rank = Convert.ToInt32(data["rank"]);
            }

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            m_result.addRankHistory(time, tmp);
        }

        return OpRes.opres_success;
    }

    OpRes breakLottery(GMUser user, ParamQuery param, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        //user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.AREA_RANK_DAY_HISTORY, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime");

        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery2(TableName.PUMP_BREAK_EGG_LOTTERY, dip, imq,
            0,
            0, null, sort);

        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(3, "reward_");
            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
                }
                else if (d.Key == "type_1")
                {
                    int v = Convert.ToInt32(data["type_1"]);
                    tmp.m_data.setValue(0, v);
                }
                else if (d.Key == "type_2")
                {
                    int v = Convert.ToInt32(data["type_2"]);
                    tmp.m_data.setValue(1, v);
                }
                else if (d.Key == "type_3")
                {
                    int v = Convert.ToInt32(data["type_3"]);
                    tmp.m_data.setValue(2, v);
                }
                else
                {
                    tmp.m_mapCount.addItem(d.Key, d.Value);
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }
}

////////////////////////////////////////////////////////////////////////////
// 夏日狂欢查询
public class QuerySummerdayCarnival : QueryBase
{
    ScoreWrap m_result = new ScoreWrap();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.resetData();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        switch (p.m_op)
        {
            case DefCC.QRY_RANK_CUR: // 当前榜
                {
                    return queryRankCur(user, p, imq);
                }
                break;
            case DefCC.QRY_RANK_HISTORY: // 历史榜
                {
                    return queryRankHistory(user, p, imq);
                }
                break;
            case DefCC.QRY_LOTTERY: 
                {
                    return summerLottery(user, p, imq);
                }
                break;
            default:
                return OpRes.op_res_failed;
                break;
        }

        return OpRes.opres_success;
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (p.m_op != DefCC.QRY_RANK_CUR)
        {
            if (string.IsNullOrEmpty(p.m_time))
            {
                return OpRes.op_res_time_format_error;
            }

            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            imq = Query.And(imq1, imq2);
        }

        return OpRes.opres_success;
    }

    OpRes queryRankCur(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        IMongoQuery timq = Query.GT("score", 0);
        dataList = DBMgr.getInstance().executeQuery(TableName.ACTIVITY_SUMMERDAY, dip, timq, 0, 100, null, "score", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();
            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if (data.ContainsKey("nickname"))
            {
                tmp.m_nickName = Convert.ToString(data["nickname"]);
            }
            if (data.ContainsKey("score"))
            {
                tmp.m_socre = Convert.ToInt32(data["score"]);
            }
            tmp.m_rank = i + 1;

            m_result.addRankCur(tmp);
        }

        return OpRes.opres_success;
    }

    OpRes queryRankHistory(GMUser user, ParamQuery param, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_SUMMERDAY_RANK, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("rank");

        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery2(TableName.PUMP_SUMMERDAY_RANK, dip, imq,
            (param.m_curPage - 1) * param.m_countEachPage,
            param.m_countEachPage, null, sort);

        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();
            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if (data.ContainsKey("score"))
            {
                tmp.m_socre = Convert.ToInt32(data["score"]);
            }
            if (data.ContainsKey("rank"))
            {
                tmp.m_rank = Convert.ToInt32(data["rank"]);
            }

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            m_result.addRankHistory(time, tmp);
        }

        return OpRes.opres_success;
    }

    OpRes summerLottery(GMUser user, ParamQuery param, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        //user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.AREA_RANK_DAY_HISTORY, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime");

        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery2(TableName.PUMP_SUMMERDAY_LOTTERY, dip, imq,
            0,
            0, null, sort);

        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(1, "reward_");
            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
                }
                else
                {
                    tmp.m_mapCount.addItem(d.Key, d.Value);
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }
}

////////////////////////////////////////////////////////////////////////////
// 段位赛查询
public class QueryDanGrade : QueryBase
{
    public const int DAN_COMPLETE = 3;
    public const int DAN_BUY = 4;

    ScoreWrap m_result = new ScoreWrap();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.resetData();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        switch(p.m_op)
        {
            case DAN_COMPLETE:
                return queryDanComplte(user, p, imq);
                break;
            case DAN_BUY:
                return queryBuy(user, p, imq);
                break;
            case DefCC.QRY_RANK_CUR:
                return queryRankCur(user, p, imq);
                break;
            case DefCC.QRY_RANK_HISTORY:
                return queryRankHistory(user, p, imq);
                break;
        }
        return OpRes.op_res_failed;
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;
        if (p.m_op != DefCC.QRY_RANK_CUR)
        {
            if (string.IsNullOrEmpty(p.m_time))
            {
                return OpRes.op_res_time_format_error;
            }

            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            imq = Query.And(imq1, imq2);
        }

        return OpRes.opres_success;
    }

    // 段位完成人数
    OpRes queryDanComplte(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_DAN_GRADING, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(1, "nor_division_");
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            foreach (var d in data)
            {
                if (d.Key.StartsWith("nor_division_"))
                {
                    tmp.m_mapCount.addItem(d.Key, Convert.ToInt32(d.Value));
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    OpRes queryBuy(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_DAN_GRADING, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(1, "sen_division_");
            if(data.ContainsKey("genTime"))
            {
                tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            }
            if(data.ContainsKey("payCount"))
            {
                int t = Convert.ToInt32(data["payCount"]);
                tmp.m_data.setValue(0, t);
            }
            foreach (var d in data)
            {
                if (d.Key.StartsWith("sen_division_"))
                {
                    tmp.m_mapCount.addItem(d.Key, Convert.ToInt32(d.Value));
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    OpRes queryRankCur(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        IMongoQuery timq = Query.GTE("score", 100000000);
        dataList = DBMgr.getInstance().executeQuery(TableName.ACTIVITY_DAN_GRADING, dip, timq, 0, 100, null, "score", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();
            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if (data.ContainsKey("score"))
            {
                tmp.m_socre = Convert.ToInt64(data["score"]);
            }
            tmp.m_rank = i + 1;

            m_result.addRankCur(tmp);
        }

        return OpRes.opres_success;
    }

    OpRes queryRankHistory(GMUser user, ParamQuery param, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.ACTIVITY_DAN_GRADING_RANK, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("rank");

        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery2(TableName.ACTIVITY_DAN_GRADING_RANK, dip, imq,
            /*(param.m_curPage - 1) * param.m_countEachPage*/0,
            param.m_countEachPage, null, sort);

        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();
            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if (data.ContainsKey("score"))
            {
                tmp.m_socre = Convert.ToInt64(data["score"]);
            }
            if (data.ContainsKey("rank"))
            {
                tmp.m_rank = Convert.ToInt32(data["rank"]);
            }

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            m_result.addRankHistory(time, tmp);
        }

        return OpRes.opres_success;
    }
}

////////////////////////////////////////////////////////////////////////////
// 自由赛查询
public class QueryFishArenaFree : QueryBase
{
    FishArenaRankWrap m_result = new FishArenaRankWrap();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.resetData();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        switch (p.m_op)
        {
            case DefCC.QRY_RANK_CUR: // 当前日榜
                {
                    queryFreeRankDayCur(user, p, imq);
                }
                break;
            case DefCC.QRY_RANK_HISTORY: // 历史日榜
                {
                    queryFreeRankHistory(user, p, imq);
                }
                break;
            case DefCC.QRY_LOTTERY:
                {
                    queryArenaFreeStat(user, p, imq);
                }
                break;
            default:
                return OpRes.op_res_failed;
                break;
        }

        return OpRes.opres_success;
    }

    // 自由赛统计
    OpRes queryArenaFreeStat(GMUser user, ParamQuery param, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        //user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_ARENA_STAT, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime");
        IMongoQuery imq1 = Query.EQ("roomId", param.m_type);
        imq = Query.And(imq1, imq);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery2(TableName.PUMP_ARENA_FREE_STAT, dip, imq,
            0,
            0, null, sort);

        if (dataList == null) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            FishArenaStat tmp = new FishArenaStat();
            if (data.ContainsKey("genTime"))
            {
                tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            }
            if (data.ContainsKey("cost"))
            {
                tmp.m_gemIncome = Convert.ToInt32(data["cost"]);
            }
            if (data.ContainsKey("count"))
            {
                tmp.m_joinPersonCount = Convert.ToInt32(data["count"]);
            }

            IMongoQuery timq = Query.And(imq1, Query.EQ("genTime", tmp.m_time));
            tmp.m_joinPerson = (int)DBMgr.getInstance().getRecordCount(TableName.PUMP_ARENA_FREE_STAT_DETAIL, timq, dip);
            m_result.m_stat.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 自由赛当前时段的榜
    OpRes queryFreeRankDayCur(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        IMongoQuery imq1 = Query.EQ("roomId", p.m_type);
        IMongoQuery timq = Query.And(Query.GT("rankScore", 0), imq1);

        dataList = DBMgr.getInstance().executeQuery(TableName.ARENA_FREE_MATCH_PLAYER, dip, timq, 0, 100, null, "rankScore", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            FishArenaRankInfo tmp = new FishArenaRankInfo();
            if (data.ContainsKey("player_id"))
            {
                tmp.m_playerId = Convert.ToInt32(data["player_id"]);
            }
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if (data.ContainsKey("rankScore"))
            {
                tmp.m_socre = Convert.ToInt32(data["rankScore"]);
            }
            tmp.m_rank = i + 1;

            m_result.addRankCur(tmp);
        }

        return OpRes.opres_success;
    }

    // 自由赛历史时段
    OpRes queryFreeRankHistory(GMUser user, ParamQuery param, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.ARENA_FREE_RANK_HISTORY, imq, dip);

        IMongoQuery imq2 = Query.EQ("state", Convert.ToInt32(param.m_playerId));
        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("rank");
        IMongoQuery imq1 = Query.EQ("roomId", param.m_type);
        imq = Query.And(imq1, imq2, imq);

        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery2(TableName.ARENA_FREE_RANK_HISTORY, dip, imq,
            (param.m_curPage - 1) * param.m_countEachPage,
            0, null, sort);

        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            FishArenaRankInfo tmp = new FishArenaRankInfo();
            if (data.ContainsKey("player_id"))
            {
                tmp.m_playerId = Convert.ToInt32(data["player_id"]);
            }
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if (data.ContainsKey("score"))
            {
                tmp.m_socre = Convert.ToInt32(data["score"]);
            }
            if (data.ContainsKey("rank"))
            {
                tmp.m_rank = Convert.ToInt32(data["rank"]);
            }

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            m_result.addRankHistory(time, tmp);
        }

        return OpRes.opres_success;
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (p.m_op != DefCC.QRY_RANK_CUR)
        {
            if (string.IsNullOrEmpty(p.m_time))
            {
                return OpRes.op_res_time_format_error;
            }

            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            imq = Query.And(imq1, imq2);
        }

        return OpRes.opres_success;
    }
}

////////////////////////////////////////////////////////////////////////////
public class ResultKillDemons : ScoreWrap
{
}

// 猎妖
public class QueryKillDemons : QueryBase
{
    ResultKillDemons m_result = new ResultKillDemons();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.resetData();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        switch (p.m_op)
        {
            case DefCC.QRY_RANK_CUR: // 查询猎妖任务
                {
                    return queryQuest(user, p, imq);
                }
                break;
            case DefCC.QRY_RANK_HISTORY: // 神兽统计
                {
                    return queryMonster(user, p, imq);
                }
                break;
            case DefCC.QRY_LOTTERY:
                {
                    return queryWeapon(user, p, imq);
                }
                break;
            case 4: // 斩妖剑
                {
                    return querySword(user, p, imq);
                }
                break;
            case 5: // 当前排行
                {
                    return queryRankCur(user, p, imq);
                }
                break;
            case 6: // 历史榜
                {
                    return queryRankHistory(user, p, imq);
                }
                break;
            default:
                return OpRes.op_res_failed;
                break;
        }

        return OpRes.opres_success;
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (p.m_op != 5)
        {
            if (string.IsNullOrEmpty(p.m_time))
            {
                return OpRes.op_res_time_format_error;
            }

            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            imq = Query.And(imq1, imq2);
        }

        return OpRes.opres_success;
    }

    OpRes queryQuest(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_KILL_MONSTER_QUEST, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(4, "quest_");

            foreach(var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else if (d.Key == "SignQuestOutlay")
                {
                    long v = Convert.ToInt64(d.Value);
                    tmp.m_data.setValue(0, v);
                }
                else if (d.Key == "ActivityQuestOutlay")
                {
                    long v = Convert.ToInt64(d.Value);
                    tmp.m_data.setValue(1, v);
                }
                else if (d.Key == "ChallengeQuestOutlay")
                {
                    long v = Convert.ToInt64(d.Value);
                    tmp.m_data.setValue(2, v);
                }
                else
                {
                    tmp.m_mapCount.addItem(d.Key, d.Value);
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    OpRes queryMonster(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_KILL_MONSTER_MONSTER, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(8, "Floor_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else if (d.Key == "5StarCount")
                {
                    long v = Convert.ToInt64(d.Value);
                    tmp.m_data.setValue(0, v);
                }
                else if (d.Key == "10StarCount")
                {
                    long v = Convert.ToInt64(d.Value);
                    tmp.m_data.setValue(1, v);
                }
                else if (d.Key == "15StarCount")
                {
                    long v = Convert.ToInt64(d.Value);
                    tmp.m_data.setValue(2, v);
                }
                else if (d.Key == "20StarCount")
                {
                    long v = Convert.ToInt64(d.Value);
                    tmp.m_data.setValue(3, v);
                }
                else if (d.Key == "25StarCount")
                {
                    long v = Convert.ToInt64(d.Value);
                    tmp.m_data.setValue(4, v);
                }
                else if (d.Key == "30StarCount")
                {
                    long v = Convert.ToInt64(d.Value);
                    tmp.m_data.setValue(5, v);
                }
                else if (d.Key == "playerCount")
                {
                    long v = Convert.ToInt64(d.Value);
                    tmp.m_data.setValue(6, v);
                }
                else
                {
                    tmp.m_mapCount.addItem(d.Key, d.Value);
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    OpRes queryWeapon(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_KILL_MONSTER_WEAPON, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(2, "item_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else if (d.Key == "huntDemonScoreOutlay")
                {
                    long v = Convert.ToInt64(d.Value);
                    tmp.m_data.setValue(0, v);
                }
                else if (d.Key == "huntDemonScoreIncome")
                {
                    long v = Convert.ToInt64(d.Value);
                    tmp.m_data.setValue(1, v);
                }
                else
                {
                    tmp.m_mapCount.addItem(d.Key, d.Value);
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    OpRes querySword(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_KILL_MONSTER_SWORD, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(1, "quest_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else if (d.Key == "swordEnergyOutlay")
                {
                    long v = Convert.ToInt64(d.Value);
                    tmp.m_data.setValue(0, v);
                }
                else
                {
                    tmp.m_mapCount.addItem(d.Key, d.Value);
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 当前排行
    OpRes queryRankCur(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        IMongoQuery timq = Query.GT("harmValue", 0);
        dataList = DBMgr.getInstance().executeQuery(TableName.KILL_MONSTER_ACTIVITY, dip, timq, 0, 100, null, "harmValue", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();

            if(data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if (data.ContainsKey("nickname"))
            {
                tmp.m_nickName = Convert.ToString(data["nickname"]);
            }
            if (data.ContainsKey("harmValue"))
            {
                tmp.m_socre = Convert.ToInt64(data["harmValue"]);
            }
            tmp.m_rank = i + 1;

            m_result.m_dataLine.m_rank.Add(tmp);
        }

        return OpRes.opres_success;
    }

    OpRes queryRankHistory(GMUser user, ParamQuery param, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
       // user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_SUMMERDAY_RANK, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("rank");

        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery2(TableName.PUMP_KILL_MONSTER_RANK, dip, imq,
            /*(param.m_curPage - 1) * param.m_countEachPage*/0,
            /*param.m_countEachPage*/0, null, sort);

        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();
            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if (data.ContainsKey("harmValue"))
            {
                tmp.m_socre = Convert.ToInt32(data["harmValue"]);
            }
            if (data.ContainsKey("rank"))
            {
                tmp.m_rank = Convert.ToInt32(data["rank"]);
            }

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            m_result.addRankHistory(time, tmp);
        }

        return OpRes.opres_success;
    }

}

////////////////////////////////////////////////////////////////////////////
public class DaShengRoomItem
{
    public DateTime m_time;
    public long m_fengDropTimes; // 风产出
    public long m_yuDropTimes;   // 雨产出
    public long m_leiDropTimes;   // 雷产出
    public long m_dianDropTimes;   // 电产出
    public long m_monkeyFishIncome;   // 玩法鱼收入
    public long m_monkeyFishOutlay;   // 玩法鱼支出
    public long m_monkeyFishJackpotIncome;   // 玩法鱼奖池收入
    public long m_monkeyFishJackpotOutlay;   // 玩法鱼奖池支出
    public long m_monkeyFishJackpotTimes;   // 奖池支出次数
    public long m_monkeyIncome;   // 大圣收入
    public long m_monkeyOutlay;   // 大圣支出
    public long m_monkeyJackpotIncome;   // 大圣奖池收入
    public long m_monkeyJackpotOutlay;   // 大圣奖池支出
    public long m_monkeyJackpotTimes;   // 奖池支出次数
}

public class ResultDaShengRoom : ScoreWrap
{
    public List<DaShengRoomItem> m_items = new List<DaShengRoomItem>();

    public void reset()
    {
        m_items.Clear();
        resetData();
    }
}

// 大圣场查询，高级场
public class QueryDaShengRoom : QueryBase
{
    ResultDaShengRoom m_result = new ResultDaShengRoom();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.reset();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;
         
        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        int op = 0, type = 0;
        ItemHelp.parseOpType(p.m_op, ref op, ref type);
        switch (op)
        {
            case DefCC.QRY_RANK_CUR: // 大圣场当日排行
                {
                    if (type == 0) // 当日
                    {
                        return queryRankDayCur(user, p, imq);
                    }
                    else // 当周
                    {
                        return queryRankWeekCur(user, p, imq);
                    }
                }
                break;
            case DefCC.QRY_RANK_HISTORY: // 大圣场历史日排行
                {
                    if (type == 0) // 当日
                    {
                        return queryRankDayHistory(user, p, imq);
                    }
                    else // 当周
                    {
                        return queryRankWeekHistory(user, p, imq);
                    }
                }
                break;
            case DefCC.QRY_LOTTERY: // 奖池
                {
                    return queryRewardPool(user, p, imq);
                }
                break;
            default:
                return OpRes.op_res_failed;
                break;
        }

        return OpRes.opres_success;
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;
        int op = 0, type = 0;
        ItemHelp.parseOpType(p.m_op, ref op, ref type);

        if (op != DefCC.QRY_RANK_CUR)
        {
            if (string.IsNullOrEmpty(p.m_time))
            {
                return OpRes.op_res_time_format_error;
            }

            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;
            if(op == DefCC.QRY_RANK_HISTORY && type == 1)
            {
                int minw = ItemHelp.getCurWeekCount(mint);
                int maxw = ItemHelp.getCurWeekCount(maxt);
                imq1 = Query.LT("week", BsonValue.Create(maxw));
                imq2 = Query.GTE("week", BsonValue.Create(minw));
                imq = Query.And(imq1, imq2);
            }
            else
            {
                imq1 = Query.LT("genTime", BsonValue.Create(maxt));
                imq2 = Query.GTE("genTime", BsonValue.Create(mint));
                imq = Query.And(imq1, imq2);
            }
        }

        return OpRes.opres_success;
    }

    OpRes queryRewardPool(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.FISHLORD_ADVANCED_ROOM_ACT, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DaShengRoomItem tmp = new DaShengRoomItem();

            if(data.ContainsKey("genTime"))
            {
                tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            }
            if (data.ContainsKey("fengDropTimes"))
            {
                tmp.m_fengDropTimes = Convert.ToInt64(data["fengDropTimes"]);
            }
            if (data.ContainsKey("yuDropTimes"))
            {
                tmp.m_yuDropTimes = Convert.ToInt64(data["yuDropTimes"]);
            }
            if (data.ContainsKey("leiDropTimes"))
            {
                tmp.m_leiDropTimes = Convert.ToInt64(data["leiDropTimes"]);
            }
            if (data.ContainsKey("dianDropTimes"))
            {
                tmp.m_dianDropTimes = Convert.ToInt64(data["dianDropTimes"]);
            }
            if (data.ContainsKey("monkeyFishIncome"))
            {
                tmp.m_monkeyFishIncome = Convert.ToInt64(data["monkeyFishIncome"]);
            }
            if (data.ContainsKey("monkeyFishOutlay"))
            {
                tmp.m_monkeyFishOutlay = Convert.ToInt64(data["monkeyFishOutlay"]);
            }
            if (data.ContainsKey("monkeyFishJackpotIncome"))
            {
                tmp.m_monkeyFishJackpotIncome = Convert.ToInt64(data["monkeyFishJackpotIncome"]);
            }
            if (data.ContainsKey("monkeyFishJackpotOutlay"))
            {
                tmp.m_monkeyFishJackpotOutlay = Convert.ToInt64(data["monkeyFishJackpotOutlay"]);
            }
            if (data.ContainsKey("monkeyFishJackpotTimes"))
            {
                tmp.m_monkeyFishJackpotTimes = Convert.ToInt64(data["monkeyFishJackpotTimes"]);
            }
            if (data.ContainsKey("monkeyIncome"))
            {
                tmp.m_monkeyIncome = Convert.ToInt64(data["monkeyIncome"]);
            }
            if (data.ContainsKey("monkeyOutlay"))
            {
                tmp.m_monkeyOutlay = Convert.ToInt64(data["monkeyOutlay"]);
            }
            if (data.ContainsKey("monkeyJackpotIncome"))
            {
                tmp.m_monkeyJackpotIncome = Convert.ToInt64(data["monkeyJackpotIncome"]);
            }
            if (data.ContainsKey("monkeyJackpotOutlay"))
            {
                tmp.m_monkeyJackpotOutlay = Convert.ToInt64(data["monkeyJackpotOutlay"]);
            }
            if (data.ContainsKey("monkeyJackpotTimes"))
            {
                tmp.m_monkeyJackpotTimes = Convert.ToInt64(data["monkeyJackpotTimes"]);
            }
            m_result.m_items.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 当日排行
    OpRes queryRankDayCur(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        IMongoQuery timq = Query.GT("rankScore", 0);
        dataList = DBMgr.getInstance().executeQuery(TableName.FISHLORD_ADVANCED_DAILY_PLAYER, dip, timq, 0, 100, null, "rankScore", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();

            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if (data.ContainsKey("rankScore"))
            {
                tmp.m_socre = Convert.ToInt64(data["rankScore"]);
            }
            tmp.m_rank = i + 1;

            m_result.m_dataLine.m_rank.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 当周排行
    OpRes queryRankWeekCur(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        IMongoQuery timq = Query.GT("rankScore", 0);
        dataList = DBMgr.getInstance().executeQuery(TableName.FISHLORD_ADVANCED_WEEKLY_PLAYER, dip, timq, 0, 100, null, "rankScore", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();

            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if (data.ContainsKey("rankScore"))
            {
                tmp.m_socre = Convert.ToInt64(data["rankScore"]);
            }
            tmp.m_rank = i + 1;

            m_result.m_dataLine.m_rank.Add(tmp);
        }

        return OpRes.opres_success;
    }

    OpRes queryRankDayHistory(GMUser user, ParamQuery param, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        // user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_SUMMERDAY_RANK, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("rank");

        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery2(TableName.FISHLORD_ADVANCED_DAILY_PLAYER_RANK, dip, imq,
            /*(param.m_curPage - 1) * param.m_countEachPage*/0,
            /*param.m_countEachPage*/0, null, sort);

        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();
            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if (data.ContainsKey("rankScore"))
            {
                tmp.m_socre = Convert.ToInt64(data["rankScore"]);
            }
            if (data.ContainsKey("rank"))
            {
                tmp.m_rank = Convert.ToInt32(data["rank"]);
            }

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            m_result.addRankHistory(time, tmp);
        }

        return OpRes.opres_success;
    }

    OpRes queryRankWeekHistory(GMUser user, ParamQuery param, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        // user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_SUMMERDAY_RANK, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("week").Ascending("rank");

        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery2(TableName.FISHLORD_ADVANCED_WEEK_PLAYER_RANK, dip, imq,
            /*(param.m_curPage - 1) * param.m_countEachPage*/0,
            /*param.m_countEachPage*/0, null, sort);

        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();
            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if (data.ContainsKey("rankScore"))
            {
                tmp.m_socre = Convert.ToInt64(data["rankScore"]);
            }
            if (data.ContainsKey("rank"))
            {
                tmp.m_rank = Convert.ToInt32(data["rank"]);
            }

            int week = Convert.ToInt32(data["week"]);
            DateTime time = ItemHelp.getWeekendTime(week);

            m_result.addRankHistory(time, tmp);
        }

        return OpRes.opres_success;
    }

}

///////////////////////////////////////////////////////////////
public class QrResultSF : QrResultItem
{
    public MapIdCount m_mapCount2 = new MapIdCount();

    public void init2(int attrCount, string prefix, string prefix2)
    {
        init(attrCount, prefix);
        m_mapCount2.m_prefix = prefix2;
    }

    public MapIdCount getMapIdCount(string key)
    {
        if (key.StartsWith(m_mapCount.m_prefix))
            return m_mapCount;

        if (key.StartsWith(m_mapCount2.m_prefix))
            return m_mapCount2;

        return null;
    }
}

public class ResultSailFestival
{
    public List<QrResultSF> m_lotteryList = new List<QrResultSF>();

    public void resetData()
    {
        m_lotteryList.Clear();
    }
}

// 启航盛典
public class QuerySailingFestival : QueryBase
{
    ResultSailFestival m_result = new ResultSailFestival();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.resetData();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        switch (p.m_op)
        {
            case 1: // 打卡情况
                {
                    return querySign(user, p, imq);
                }
                break;
            case 2: // 招财进宝
                {
                    return queryVip(user, p, imq);
                }
                break;
            case 3: // 成就
                {
                    return queryAchive(user, p, imq);
                }
                break;
            case 4: // 成就商店
                {
                    return queryAchiveShop(user, p, imq);
                }
                break;
            case 5: // 成就等级
                {
                    return queryAchiveLevel(user, p, imq);
                }
                break;
            case 6: // 海盗宝藏
                {
                    return queryTreasure(user, p, imq);
                }
                break;
            case 7:
                { 
                    return queryAccOpen(user, p, imq);
                }
                break;
            default:
                return OpRes.op_res_failed;
                break;
        }

        return OpRes.op_res_failed;
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (string.IsNullOrEmpty(p.m_time))
        {
            return OpRes.op_res_time_format_error;
        }

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        imq = Query.And(imq1, imq2);

        return OpRes.opres_success;
    }

    // 打卡情况
    OpRes querySign(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_SAIL_FESTIVAL, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultSF tmp = new QrResultSF();
            tmp.init2(2, "sign_", "resign_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                    setVal(user, tmp.m_data, 1, tmp.m_time, 0);
                }
                else if (d.Key == "signGold")
                {
                    long v = Convert.ToInt64(d.Value);
                    tmp.m_data.setValue(0, v);
                }
                else
                {
                    var mapd = tmp.getMapIdCount(d.Key);
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 招财进宝
    OpRes queryVip(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_SAIL_FESTIVAL, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultSF tmp = new QrResultSF();
            tmp.init2(2, "piggy_", "piggy_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                    setVal(user, tmp.m_data, 1, tmp.m_time, 1);
                }
                else if (d.Key == "piggyGold")
                {
                    long v = Convert.ToInt64(d.Value);
                    tmp.m_data.setValue(0, v);
                }
                else
                {
                    var mapd = tmp.getMapIdCount(d.Key);
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }
            for (int k = 4; k <= 10; k++)
            {
                tmp.m_mapCount.addItem(3, tmp.m_mapCount.getCount(k));
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 成就
    OpRes queryAchive(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_SAIL_FESTIVAL, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultSF tmp = new QrResultSF();
            tmp.init2(1, "quest_", "quest_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else if (d.Key == "questGold")
                {
                    long v = Convert.ToInt64(d.Value);
                    tmp.m_data.setValue(0, v);
                }
                else
                {
                    var mapd = tmp.getMapIdCount(d.Key);
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 成就商店
    OpRes queryAchiveShop(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_SAIL_FESTIVAL, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultSF tmp = new QrResultSF();
            tmp.init2(0, "shop_", "shop_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else
                {
                    var mapd = tmp.getMapIdCount(d.Key);
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 成就等级
    OpRes queryAchiveLevel(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_SAIL_FESTIVAL, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultSF tmp = new QrResultSF();
            tmp.init2(1, "achieveLv_", "achieveLv_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else if (d.Key == "achieveGold")
                {
                    long v = Convert.ToInt64(d.Value);
                    tmp.m_data.setValue(0, v);
                }
                else
                {
                    var mapd = tmp.getMapIdCount(d.Key);
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 海盗宝藏
    OpRes queryTreasure(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_SAIL_FESTIVAL, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultSF tmp = new QrResultSF();
            tmp.init2(0, "vip_", "vip_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else
                {
                    var mapd = tmp.getMapIdCount(d.Key);
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 累计打开
    OpRes queryAccOpen(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_SAIL_FESTIVAL, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultSF tmp = new QrResultSF();
            tmp.init2(0, "signcount_", "signcount_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else
                {
                    var mapd = tmp.getMapIdCount(d.Key);
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    void setVal(GMUser user, AttributeSetArray<long> data, int index, DateTime time, int field)
    {
        ParamQuery qparam = new ParamQuery();
        qparam.m_time = time.ToShortDateString();
        OpRes res = user.doQuery(qparam, QueryType.queryTypeStatTodayInfo);
        List<TodayInfoItem> qresult = (List<TodayInfoItem>)user.getQueryResult(QueryType.queryTypeStatTodayInfo);
        if (qresult.Count > 0)
        {
            if (field == 0) // 注册
            {
                int v = qresult[0].m_rechargeCount;
                data.setValue(index, v);
            }
            else if (field == 1) // 活跃人数
            {
                int v = qresult[0].m_activeCount;
                data.setValue(index, v);
            }
        }
    }
}

///////////////////////////////////////////////////////////////////////
// 双11狂欢查询
public class QueryD11Activity : QueryBase
{
    ScoreWrap m_result = new ScoreWrap();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.resetData();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        switch (p.m_op)
        {
            case 1: // 抽奖
                {
                    return queryLottery(user, p, imq);
                }
                break;
            case 2: // 任务
                {
                    return queryTask(user, p, imq);
                }
                break;
            case 3: // 累充
                {
                    return queryAccRecharge(user, p, imq);
                }
                break;
            case 4:
                {
                    return queryPanic(user, p, imq);
                }
                break;
            case 5: // 当前排行
                {
                    return queryRankDayCur(user, p, imq);
                }
                break;
            case 6: // 历史排行
                {
                    return queryRankHistory(user, p, imq);
                }
                break;
            default:
                return OpRes.op_res_failed;
                break;
        }

        return OpRes.op_res_failed;
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (p.m_op != 5)
        {
            if (string.IsNullOrEmpty(p.m_time))
            {
                return OpRes.op_res_time_format_error;
            }

            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            imq = Query.And(imq1, imq2);
        }

        return OpRes.opres_success;
    }

    // 抽奖
    OpRes queryLottery(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_D11_TASK, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(0, "lottery_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else
                {
                    var mapd = tmp.getMapCount();
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    OpRes queryTask(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_D11_TASK, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(0, "quest_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else
                {
                    var mapd = tmp.getMapCount();
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    OpRes queryAccRecharge(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_D11_TASK, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(0, "recharge_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else
                {
                    var mapd = tmp.getMapCount();
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    OpRes queryPanic(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_D11_TASK, dip, imq, 0, 100);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            
            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else if (d.Key.StartsWith("diamondGift_"))
                {
                    string[] arr = d.Key.Split('_');
                    if (arr.Length == 4)
                    {
                        int giftId = Convert.ToInt32(arr[1]);
                        int count = Convert.ToInt32(d.Value);
                        int state = Convert.ToInt32(arr[3]);
                        tmp.getMapCount().addItem(state * 10 + giftId, count);
                    }

                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 当日排行
    OpRes queryRankDayCur(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        IMongoQuery timq = Query.GT("lotteryCount", 0);
        dataList = DBMgr.getInstance().executeQuery(TableName.D11_ACT, dip, timq, 0, 100, null, "lotteryCount", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();

            if (data.ContainsKey("player_id"))
            {
                tmp.m_playerId = Convert.ToInt32(data["player_id"]);
            }
            if (data.ContainsKey("nickname"))
            {
                tmp.m_nickName = Convert.ToString(data["nickname"]);
            }
            if (data.ContainsKey("lotteryCount"))
            {
                tmp.m_socre = Convert.ToInt64(data["lotteryCount"]);
            }
            tmp.m_rank = i + 1;

            m_result.m_dataLine.m_rank.Add(tmp);
        }

        return OpRes.opres_success;
    }

    OpRes queryRankHistory(GMUser user, ParamQuery param, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        // user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_SUMMERDAY_RANK, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("rank");

        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery2(TableName.PUMP_D11_RANK, dip, imq,
            /*(param.m_curPage - 1) * param.m_countEachPage*/0,
            /*param.m_countEachPage*/0, null, sort);

        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();
            if (data.ContainsKey("player_id"))
            {
                tmp.m_playerId = Convert.ToInt32(data["player_id"]);
            }
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if (data.ContainsKey("points"))
            {
                tmp.m_socre = Convert.ToInt64(data["points"]);
            }
            if (data.ContainsKey("rank"))
            {
                tmp.m_rank = Convert.ToInt32(data["rank"]);
            }

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            m_result.addRankHistory(time, tmp);
        }

        return OpRes.opres_success;
    }

}

///////////////////////////////////////////////////////////////////////
// 王者争霸
public class QueryKingCraft : QueryBase
{
    ScoreWrap m_result = new ScoreWrap();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.resetData();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        switch (p.m_op)
        {
            case 1: // 签到
                {
                    return querySignIn(user, p, imq);
                }
                break;
            case 2: // 抽奖
                {
                    return queryLottery(user, p, imq);
                }
                break;
            case 3: // 累充
                {
                    return queryAccRecharge(user, p, imq);
                }
                break;
            case 4: // 兑换
                {
                    return queryExchange(user, p, imq);
                }
                break;
            case 5: // 当前排行
                {
                    return queryRankDayCur(user, p, imq);
                }
                break;
            case 6: // 历史排行
                {
                    return queryRankHistory(user, p, imq);
                }
                break;
            case 7: // 礼包
                {
                    return queryGiftBuy(user, p, imq);
                }
                break;
            default:
                return OpRes.op_res_failed;
                break;
        }

        return OpRes.op_res_failed;
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (p.m_op != 5)
        {
            if (string.IsNullOrEmpty(p.m_time))
            {
                return OpRes.op_res_time_format_error;
            }

            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            imq = Query.And(imq1, imq2);
        }

        return OpRes.opres_success;
    }

    // 签到
    OpRes querySignIn(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_KING_CRAFT, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(0, "sign_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else
                {
                    var mapd = tmp.getMapCount();
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 抽奖
    OpRes queryLottery(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_KING_CRAFT, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(0, "lottery_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else
                {
                    var mapd = tmp.getMapCount();
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 累充
    OpRes queryAccRecharge(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_KING_CRAFT, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(0, "recharge_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else
                {
                    var mapd = tmp.getMapCount();
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 兑换
    OpRes queryExchange(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_KING_CRAFT, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(0, "exchange_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else
                {
                    var mapd = tmp.getMapCount();
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 礼包购买
    OpRes queryGiftBuy(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_KING_CRAFT, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(0, "gift_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else
                {
                    var mapd = tmp.getMapCount();
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 当日排行
    OpRes queryRankDayCur(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        IMongoQuery timq = Query.GT("lotteryCount", 0);
        dataList = DBMgr.getInstance().executeQuery(TableName.ACT_KING_CRAFT, dip, timq, 0, 100, null, "lotteryCount", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();

            if (data.ContainsKey("player_id"))
            {
                tmp.m_playerId = Convert.ToInt32(data["player_id"]);
            }
            if (data.ContainsKey("nickname"))
            {
                tmp.m_nickName = Convert.ToString(data["nickname"]);
            }
            if (data.ContainsKey("lotteryCount"))
            {
                tmp.m_socre = Convert.ToInt64(data["lotteryCount"]);
            }
            tmp.m_rank = i + 1;

            m_result.m_dataLine.m_rank.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 历史排行
    OpRes queryRankHistory(GMUser user, ParamQuery param, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        // user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_SUMMERDAY_RANK, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("rank");

        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery2(TableName.PUMP_KING_CRAFT_HISTORY_RANK, dip, imq,
            /*(param.m_curPage - 1) * param.m_countEachPage*/0,
            /*param.m_countEachPage*/0, null, sort);

        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();
            if (data.ContainsKey("player_id"))
            {
                tmp.m_playerId = Convert.ToInt32(data["player_id"]);
            }
            if (data.ContainsKey("nickname"))
            {
                tmp.m_nickName = Convert.ToString(data["nickname"]);
            }
            if (data.ContainsKey("points"))
            {
                tmp.m_socre = Convert.ToInt64(data["points"]);
            }
            if (data.ContainsKey("rank"))
            {
                tmp.m_rank = Convert.ToInt32(data["rank"]);
            }

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            m_result.addRankHistory(time, tmp);
        }

        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////
// 一起来摸鱼
public class QueryTouchFish : QueryBase
{
    ScoreWrap m_result = new ScoreWrap();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.resetData();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        switch (p.m_op)
        {
            case 1: // 当前赛季等级
                {
                    getSeasonImq("", ref imq);
                    return queryLevelDistr(user, p, imq);
                }
                break;
            case 2: // 历史赛季等级
                {
                    OpRes c = getSeasonImq(p.m_param, ref imq);
                    if (c != OpRes.opres_success)
                        return c;

                    return queryLevelDistr(user, p, imq);
                }
                break;
            case 3: // 当前赛季任务
                {
                    getSeasonImq("", ref imq);
                    return queryTask(user, p, imq);
                }
                break;
            case 4: // 历史赛季任务
                {
                    OpRes c = getSeasonImq(p.m_param, ref imq);
                    if (c != OpRes.opres_success)
                        return c;

                    return queryTask(user, p, imq);
                }
                break;
            case 5: // 礼包
                {
                    return queryGiftBuy(user, p, imq);
                }
                break;
            case 6: // 当前排行
                {
                    getSeasonImq("", ref imq);
                    return queryRankDayCur(user, p, imq);
                }
                break;
            case 7: // 历史排行
                {
                    OpRes c = getSeasonImq(p.m_param, ref imq);
                    if (c != OpRes.opres_success)
                        return c;

                    return queryRankHistory(user, p, imq);
                }
                break;
            default:
                return OpRes.op_res_failed;
                break;
        }

        return OpRes.op_res_failed;
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (p.m_op == 5)
        {
            if (string.IsNullOrEmpty(p.m_time))
            {
                return OpRes.op_res_time_format_error;
            }

            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            imq = Query.And(imq1, imq2);
        }

        return OpRes.opres_success;
    }

    // 礼包购买
    OpRes queryGiftBuy(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_TOUCH_FISH_GIFT, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(0, "gift_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else
                {
                    var mapd = tmp.getMapCount();
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 当日排行
    OpRes queryRankDayCur(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("level").Ascending("updateTime");

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        IMongoQuery timq = Query.GT("level", 0);
        dataList = DBMgr.getInstance().executeQuery2(TableName.ACTIVITY_TOUCH_FISH, dip, timq, 0, 100, null, sort);

        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();

            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if (data.ContainsKey("nickname"))
            {
                tmp.m_nickName = Convert.ToString(data["nickname"]);
            }
            if (data.ContainsKey("level"))
            {
                tmp.m_socre = Convert.ToInt64(data["level"]);
            }
            tmp.m_rank = i + 1;

            m_result.m_dataLine.m_rank.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 历史排行
    OpRes queryRankHistory(GMUser user, ParamQuery param, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);    
        List<Dictionary<string, object>> dataList = null;
        SortByBuilder sort = new SortByBuilder();
        sort = sort.Ascending("rank");

        dataList = DBMgr.getInstance().executeQuery2(TableName.PUMP_TOUCH_FISH, dip, imq,
            /*(param.m_curPage - 1) * param.m_countEachPage*/0,
            /*param.m_countEachPage*/0, null, sort);

        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();
            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if (data.ContainsKey("nickname"))
            {
                tmp.m_nickName = Convert.ToString(data["nickname"]);
            }
            if (data.ContainsKey("level"))
            {
                tmp.m_socre = Convert.ToInt64(data["level"]);
            }
            if (data.ContainsKey("rank"))
            {
                tmp.m_rank = Convert.ToInt32(data["rank"]);
            }

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            m_result.addRankHistory(time, tmp);
        }

        return OpRes.opres_success;
    }

    // 当前，历史赛季任务
    OpRes queryTask(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_TOUCH_FISH_PLAYER_TASK, dip, imq, 0, 0, null);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(1, "task_");

            foreach (var d in data)
            {
                if (d.Key == "adv")
                {
                    tmp.m_data.setValue(0, Convert.ToBoolean(data["adv"]) ? 1 : 0);
                }
                else
                {
                    var mapd = tmp.getMapCount();
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 当前，历史赛季等级分布
    OpRes queryLevelDistr(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.STAT_TOUCH_FISH_PLAYER_DISTRI, dip, imq, 0, 0, null);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(1, "index");

            foreach (var d in data)
            {
                if (d.Key == "adv")
                {
                    tmp.m_data.setValue(0, Convert.ToInt32(data["adv"]));
                }
                else
                {
                    var mapd = tmp.getMapCount();
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    OpRes getSeasonImq(string seasonStr, ref IMongoQuery imq)
    {
        if (!string.IsNullOrEmpty(seasonStr))
        {
            int season = 0;
            if (!int.TryParse(seasonStr, out season)) return OpRes.op_res_param_not_valid;
            imq = Query.EQ("season", season);
            return OpRes.opres_success;
        }

        DateTime now = DateTime.Now;
        int year = now.Year;
        int month = now.Month;
        int d = year * 100 + month;
        imq = Query.EQ("season", d);
        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////
// 庆典
public class QueryFishingCelebration : QueryBase
{
    ScoreWrap m_result = new ScoreWrap();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.resetData();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        switch (p.m_op)
        {
            case 1: // 抽奖
                {
                    return queryLottery(user, p, imq);
                }
                break;
            case 2: // 庆典券兑换
                {
                    return queryExchange(user, p, imq);
                }
                break;
            case 3: // 泡泡积分统计
                {
                    return queryPaoPaoScore(user, p, imq);
                }
                break;
            case 4: // 幸运转盘统计
                {
                    return queryDialPan(user, p, imq);
                }
                break;
            case 5: // 礼包统计
                {
                    return queryGift(user, p, imq);
                }
                break;
            case 6: // 排行榜
                {
                    return queryRankDayCur(user, p, imq);
                }
                break;
        }

        return OpRes.op_res_failed;
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (p.m_op != 6)
        {
            if (string.IsNullOrEmpty(p.m_time))
            {
                return OpRes.op_res_time_format_error;
            }

            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            imq = Query.And(imq1, imq2);
        }

        return OpRes.opres_success;
    }

    // 抽奖
    OpRes queryLottery(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_FISHING_CELE_STATE, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(2, "lottery_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else if (d.Key == "active")
                {
                    tmp.m_data.setValue(0, Convert.ToInt32(d.Value));
                }
                else if (d.Key == "state")
                {
                    tmp.m_data.setValue(1, Convert.ToInt32(d.Value));
                }
                else
                {
                    var mapd = tmp.getMapCount();
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 庆典券兑换
    OpRes queryExchange(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_FISHING_CELE_STATE, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(1, "exchange_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else if (d.Key == "state")
                {
                    tmp.m_data.setValue(0, Convert.ToInt32(d.Value));
                }
                else
                {
                    var mapd = tmp.getMapCount();
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 泡泡积分统计
    OpRes queryPaoPaoScore(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_FISHING_CELE, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(1, "exchange_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else if (d.Key == "bubble_outlay")
                {
                    tmp.m_data.setValue(0, Convert.ToInt32(d.Value));
                }
                else
                {
                    var mapd = tmp.getMapCount();
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 幸运转盘
    OpRes queryDialPan(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_FISHING_CELE, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(0, "lottery_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else
                {
                    var mapd = tmp.getMapCount();
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 礼包
    OpRes queryGift(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_FISHING_CELE, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            QrResultItem tmp = new QrResultItem();
            tmp.init(0, "gift_");

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else
                {
                    var mapd = tmp.getMapCount();
                    if (mapd != null)
                    {
                        mapd.addItem(d.Key, d.Value);
                    }
                }
            }

            m_result.m_lotteryList.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 当日排行
    OpRes queryRankDayCur(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("lotteryCount");

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        IMongoQuery timq = Query.GT("lotteryCount", 0);
        dataList = DBMgr.getInstance().executeQuery2(TableName.PUMP_FISHING_CELE_RANK, dip, timq, 0, 100, null, sort);

        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();

            if (data.ContainsKey("player_id"))
            {
                tmp.m_playerId = Convert.ToInt32(data["player_id"]);
            }
            if (data.ContainsKey("nickname"))
            {
                tmp.m_nickName = Convert.ToString(data["nickname"]);
            }
            if (data.ContainsKey("lotteryCount"))
            {
                tmp.m_socre = Convert.ToInt64(data["lotteryCount"]);
            }
            tmp.m_rank = i + 1;

            m_result.m_dataLine.m_rank.Add(tmp);
        }

        return OpRes.opres_success;
    }

    // 历史排行
    OpRes queryRankHistory(GMUser user, ParamQuery param, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        SortByBuilder sort = new SortByBuilder();
        sort = sort.Ascending("rank");

        dataList = DBMgr.getInstance().executeQuery2(TableName.PUMP_TOUCH_FISH, dip, imq,
            /*(param.m_curPage - 1) * param.m_countEachPage*/0,
            /*param.m_countEachPage*/0, null, sort);

        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            IntScoreRankInfo tmp = new IntScoreRankInfo();
            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if (data.ContainsKey("nickname"))
            {
                tmp.m_nickName = Convert.ToString(data["nickname"]);
            }
            if (data.ContainsKey("level"))
            {
                tmp.m_socre = Convert.ToInt64(data["level"]);
            }
            if (data.ContainsKey("rank"))
            {
                tmp.m_rank = Convert.ToInt32(data["rank"]);
            }

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            m_result.addRankHistory(time, tmp);
        }

        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////
public class NiuYearFeedBackInfo
{
    public string m_genTime;
    public int m_level;
    public int m_rewardId;
    public int m_playerId;
    public string m_rewardCode;
}

// 牛年大回馈活动
public class QueryNiuYearFeedBack : QueryBase
{
    List<NiuYearFeedBackInfo> m_result = new List<NiuYearFeedBackInfo>();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.Clear();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        IMongoQuery iq = Query.EQ("gear", p.m_op);
        imq = Query.And(imq, iq);
        return queryLottery(user, p, imq);
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (p.m_op != 6)
        {
            if (string.IsNullOrEmpty(p.m_time))
            {
                return OpRes.op_res_time_format_error;
            }

            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            imq = Query.And(imq1, imq2);
        }

        return OpRes.opres_success;
    }

    // 抽奖
    OpRes queryLottery(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_FEED_BACK_PRIZE, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            NiuYearFeedBackInfo item = new NiuYearFeedBackInfo();

            if (data.ContainsKey("genTime"))
            {
                item.m_genTime = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            }
            if (data.ContainsKey("gear"))
            {
                item.m_level = Convert.ToInt32(data["gear"]);
            }
            if (data.ContainsKey("rank"))
            {
                item.m_rewardId = Convert.ToInt32(data["rank"]);
            }
            if (data.ContainsKey("playerId"))
            {
                item.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if (data.ContainsKey("key"))
            {
                item.m_rewardCode = Convert.ToString(data["key"]);
            }

            m_result.Add(item);
        }

        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////
// 夏日计划， 计划激活存储
public class SummerPlanInfo
{
    public DateTime m_time;

    // 累充激活
    public int m_activeRecharge;
    // 礼包激活
    public int m_activeGift;

    // 红包
    public MapIdCount m_redPacket = new MapIdCount();
    // 累充
    public MapIdCount m_recharge = new MapIdCount();

    public SummerPlanInfo()
    {
        m_redPacket.m_prefix = "redpacket_";
        m_recharge.m_prefix = "recharged_";
    }
}

// 夏日计划查询
public class QuerySummerPlan : QueryBase
{
    List<SummerPlanInfo> m_result = new List<SummerPlanInfo>();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.Clear();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        return queryLottery(user, p, imq);
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (string.IsNullOrEmpty(p.m_time))
        {
            return OpRes.op_res_time_format_error;
        }

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        imq = Query.And(imq1, imq2);

        return OpRes.opres_success;
    }

    OpRes queryLottery(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_SUMMER_PLAN, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            SummerPlanInfo tmp = new SummerPlanInfo();

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else if (d.Key == "giftactive")
                {
                    tmp.m_activeGift = Convert.ToInt32(d.Value);
                }
                else if (d.Key == "active")
                {
                    tmp.m_activeRecharge = Convert.ToInt32(d.Value);
                }
                else if(d.Key.StartsWith(tmp.m_recharge.m_prefix))
                {
                    tmp.m_recharge.addItem(d.Key, d.Value);
                }
                else if (d.Key.StartsWith(tmp.m_redPacket.m_prefix))
                {
                    tmp.m_redPacket.addItem(d.Key, d.Value);
                }
            }

            m_result.Add(tmp);
        }

        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////
// 点石成金
public class GoldTouchInfo
{
    public DateTime m_time;

    // 点石成功
    public MapIdCount m_touchSuccess = new MapIdCount();
    // 点石失败
    public MapIdCount m_touchFail = new MapIdCount();

    public GoldTouchInfo()
    {
        m_touchSuccess.m_prefix = "touchGold_S_";
        m_touchFail.m_prefix = "touchGold_F_";
    }

    // type 1:画龙点睛  2:点石成金
    // id 1--6
    public int getSuccessCount(int type, int id)
    {
        if (type == 1)
        {
            return m_touchSuccess.getCount(id);
        }
        else if (type == 2)
        {
            return m_touchSuccess.getCount(id + 6);
        }
        return 0;
    }

    public int getFailCount(int type, int id)
    {
        if (type == 1)
        {
            return m_touchFail.getCount(id);
        }
        else if (type == 2)
        {
            return m_touchFail.getCount(id + 6);
        }
        return 0;
    }
}

// 点石成金查询
public class QueryGoldTouch : QueryBase
{
    List<GoldTouchInfo> m_result = new List<GoldTouchInfo>();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.Clear();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        return queryLottery(user, p, imq);
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (string.IsNullOrEmpty(p.m_time))
        {
            return OpRes.op_res_time_format_error;
        }

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        imq = Query.And(imq1, imq2);

        return OpRes.opres_success;
    }

    OpRes queryLottery(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_SUMMER_PLAN, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            GoldTouchInfo tmp = new GoldTouchInfo();

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else if (d.Key.StartsWith(tmp.m_touchSuccess.m_prefix))
                {
                    tmp.m_touchSuccess.addItem(d.Key, d.Value);
                }
                else if (d.Key.StartsWith(tmp.m_touchFail.m_prefix))
                {
                    tmp.m_touchFail.addItem(d.Key, d.Value);
                }
            }

            m_result.Add(tmp);
        }

        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////
public class TreasureWinInfo
{
    public DateTime m_time;

    // 玩家ID
    public int m_playerId;
    // 类型 0-海王福袋  1-海王礼盒 2-海王宝箱  3-海王宝库
    public int m_type;
    // 第几轮
    public int m_round;
}

public class SeaKingSendTreasureResult
{
    public List<TreasureWinInfo> m_info = new List<TreasureWinInfo>();

    public void addData(DateTime time, int playerId, int type, int round)
    {
        TreasureWinInfo info = new TreasureWinInfo();
        info.m_time = time;
        info.m_playerId = playerId;
        info.m_type = type;
        info.m_round = round;
        m_info.Add(info);
    }

    public void resetData()
    {
        m_info.Clear();
    }
}

// 查询海王送宝
public class QuerySeaKingSendTreasure : QueryBase
{
    SeaKingSendTreasureResult m_result = new SeaKingSendTreasureResult();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.resetData();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        return queryLottery(user, p, imq);
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (string.IsNullOrEmpty(p.m_time))
        {
            return OpRes.op_res_time_format_error;
        }

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        imq = Query.And(imq1, imq2);

        return OpRes.opres_success;
    }

    OpRes queryLottery(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("round");

        dataList = DBMgr.getInstance().executeQuery2(TableName.PUMP_SUMMER_PLAN_BALANCE, dip, imq, 0, 0, null, sort);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            if (!data.ContainsKey("genTime"))
                continue;
            if (!data.ContainsKey("playerId"))
                continue;
            if (!data.ContainsKey("type"))
                continue;
            if (!data.ContainsKey("round"))
                continue;

            DateTime genTime = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            int playerId = Convert.ToInt32(data["playerId"]);
            int type = Convert.ToInt32(data["type"]);
            int round = Convert.ToInt32(data["round"]);
            m_result.addData(genTime, playerId, type, round);
        }

        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////
// 蛟龙腾飞结果
public class DragonFlyInfo
{
    public DateTime m_time;

    public MapIdCount m_info = new MapIdCount();
    
    public DragonFlyInfo()
    {
        m_info.m_prefix = "rocket_";
    }

    public void resetData()
    {
        m_info.clearData();
    }
}

// 蛟龙腾飞查询
public class QueryDragonFly : QueryBase
{
    List<DragonFlyInfo> m_result = new List<DragonFlyInfo>();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.Clear();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        return queryLottery(user, p, imq);
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (string.IsNullOrEmpty(p.m_time))
        {
            return OpRes.op_res_time_format_error;
        }

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        imq = Query.And(imq1, imq2);

        return OpRes.opres_success;
    }

    OpRes queryLottery(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_SUMMER_PLAN, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DragonFlyInfo tmp = new DragonFlyInfo();

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else if (d.Key.StartsWith(tmp.m_info.m_prefix))
                {
                    tmp.m_info.addItem(d.Key, d.Value);
                }
            }
            m_result.Add(tmp);
        }

        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////
// 惊喜盒结果
public class ResultSurpriseBox
{
    public DateTime m_time;

    public MapIdCount m_info = new MapIdCount();

    public ResultSurpriseBox()
    {
    }

    public void setQueryType(int t)
    {
        if (t == 1) // 惊喜盒子
        {
            m_info.m_prefix = "box_";
        }
        else if (t == 2)
        {
            m_info.m_prefix = "lottery_";
        }
    }

    public void resetData()
    {
        m_info.clearData();
    }
}

// 惊喜盒中奖情况查询
public class QuerySurpriseBoxWinPrize : QueryBase
{
    List<ResultSurpriseBox> m_result = new List<ResultSurpriseBox>();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.Clear();

        IMongoQuery imq = null;
        OpRes c = constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        return queryLottery(user, p, imq);
    }

    OpRes constructCond(ParamQuery p, ref IMongoQuery imq)
    {
        IMongoQuery imq1 = null;
        IMongoQuery imq2 = null;

        if (string.IsNullOrEmpty(p.m_time))
        {
            return OpRes.op_res_time_format_error;
        }

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        imq = Query.And(imq1, imq2);

        return OpRes.opres_success;
    }

    OpRes queryLottery(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_SURPRISE_BOX, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            ResultSurpriseBox tmp = new ResultSurpriseBox();
            tmp.setQueryType(p.m_op);

            foreach (var d in data)
            {
                if (d.Key == "genTime")
                {
                    tmp.m_time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else if (d.Key.StartsWith(tmp.m_info.m_prefix))
                {
                    tmp.m_info.addItem(d.Key, d.Value);
                }
            }
            m_result.Add(tmp);
        }

        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////
public class SurpriseBoxRankItem
{
    public int m_rank;
    public int m_playerId;
    public int m_score;
}

// 惊喜盒当前榜
public class QuerySurpriseBoxRankCur : QueryBase
{
    List<SurpriseBoxRankItem> m_result = new List<SurpriseBoxRankItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        m_result.Clear();
        IMongoQuery imq = Query.GT("score", 0);
        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        return queryLottery(user, p, imq);
    }

    OpRes queryLottery(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.ACTIVITY_SURPRISE_BOX, dip, imq, 0, 100, null, "score", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            SurpriseBoxRankItem tmp = new SurpriseBoxRankItem();

            if(data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if(data.ContainsKey("score"))
            {
                tmp.m_score = Convert.ToInt32(data["score"]);
            }
            tmp.m_rank = i + 1;
            m_result.Add(tmp);
        }

        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////
public class ResultSurpriseBoxJD
{
    public int m_playerId;
    // 历史充值金额
    public int m_historyRecharge;
    // 活动充值金额
    public int m_activityRecharge;
    // 历史京东卡碎片
    public int m_historyChip;
    // 现有京东卡碎片
    public int m_curChip;
}

public class QuerySurpriseBoxJD : QueryBase
{
    ResultSurpriseBoxJD m_result = new ResultSurpriseBoxJD();
    static string[] PLAYER_FIELDS = { "player_id", "JDChip", "historyJDChip", "recharged" };

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        int playerId = 0;
        if (!int.TryParse(p.m_playerId, out playerId))
        {
            return OpRes.op_res_param_not_valid;
        }

        IMongoQuery imq = Query.EQ("player_id", playerId);
        return query(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        return queryLottery(user, p, imq);
    }

    OpRes queryLottery(GMUser user, ParamQuery p, IMongoQuery imq)
    {
       // DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        Dictionary<string, object> data = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, user.getDbServerID(), DbName.DB_PLAYER, imq, PLAYER_FIELDS);
        if (data == null)
            return OpRes.op_res_player_not_exist;

        if (data.ContainsKey("player_id"))
        {
            m_result.m_playerId = Convert.ToInt32(data["player_id"]);
        }
        if (data.ContainsKey("JDChip"))
        {
            m_result.m_curChip = Convert.ToInt32(data["JDChip"]);
        }
        if (data.ContainsKey("historyJDChip"))
        {
            m_result.m_historyChip = Convert.ToInt32(data["historyJDChip"]);
        }
        if (data.ContainsKey("recharged"))
        {
            m_result.m_historyRecharge = Convert.ToInt32(data["recharged"]);
        }

        IMongoQuery imq2 = Query.EQ("playerId", Convert.ToInt32(p.m_playerId));
        Dictionary<string, object> data2 =
            DBMgr.getInstance().getTableData(TableName.ACTIVITY_SURPRISE_BOX, user.getDbServerID(), DbName.DB_PLAYER, imq2);
        if (data2 != null)
        {
            if (data2.ContainsKey("recharged"))
            {
                m_result.m_activityRecharge = Convert.ToInt32(data2["recharged"]);
            }
        }

        return OpRes.opres_success;
    }
}


///////////////////////////////////////////////////////////////////////
public class ResultLingBao
{
    public DateTime m_time;

    public int m_playerId;

    public long[] m_lottery;

    public void init(int size)
    {
        m_lottery = new long[size];
    }

    public void setValue(int id, int value)
    {
        if(id >= 1 && id <= m_lottery.Length)
        {
            m_lottery[id - 1] = value;
        }
    }
}

// 灵宝抽奖统计
public class QueryLingBao : QueryBase
{
    List<ResultLingBao> m_result = new List<ResultLingBao>();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        IMongoQuery imq = null;
        OpRes c = ItemHelp.constructCond(p, ref imq);
        if (c != OpRes.opres_success)
            return c;

        m_result.Clear();
        return querySwitch(user, p, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    OpRes querySwitch(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        switch (p.m_type)
        {
            case 1:
                {
                    return query(user, p, imq);
                }
                break;
            case 2:
                {
                    return queryExchangeStat(user, p, imq);
                }
                break;
        }

        return OpRes.op_res_failed;
    }

    OpRes query(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_LINGBAO, imq, dip);
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_LINGBAO, dip, imq, 
            (p.m_curPage - 1) * p.m_countEachPage,
            p.m_countEachPage, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        foreach (var data in dataList)
        {
            ResultLingBao tmp = new ResultLingBao();
            tmp.init(14);
            m_result.Add(tmp);

            if (data.ContainsKey("genTime"))
            {
                tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            }
            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            for (int n = 1; n <= 14; n++)
            {
                string s = n.ToString();
                if (data.ContainsKey(s))
                {
                    int m = Convert.ToInt32(data[s]);
                    tmp.setValue(n, m);
                }
            }

        }

        return OpRes.opres_success;
    }

    // 宝莲兑换统计
    OpRes queryExchangeStat(GMUser user, ParamQuery p, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = null;
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_LINGBAO, dip, imq, 
            (p.m_curPage - 1) * p.m_countEachPage,
             p.m_countEachPage, null, "genTime", false);
        if (dataList == null || dataList.Count == 0) return OpRes.op_res_not_found_data;

        foreach (var data in dataList)
        {
            ResultLingBao tmp = new ResultLingBao();
            m_result.Add(tmp);

            if (data.ContainsKey("genTime"))
            {
                tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            }
            if (data.ContainsKey("PlayerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["PlayerId"]);
            }
            for (int n = 1; n <= 4; n++)
            {
                string s = n.ToString();
                if (data.ContainsKey(s))
                {
                    int m = Convert.ToInt32(data[s]);
                    tmp.setValue(n, m);
                }
            }
        }

        return OpRes.opres_success;
    }
}







































