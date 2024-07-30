using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.IO;

class SysStatRemain : SysBase
{
    // 统计模块
    private Dictionary<int, StatBase> m_statModule = new Dictionary<int, StatBase>();
    static int[] s_remain = { CRemainConst.DAY_2_REMAIN, CRemainConst.DAY_3_REMAIN, CRemainConst.DAY_7_REMAIN, CRemainConst.DAY_30_REMAIN };
    static int[] s_totalRecharge = { 0, CRemainConst.DAY_2_REMAIN/*-1*/, CRemainConst.DAY_3_REMAIN, CRemainConst.DAY_7_REMAIN, 
                                       CRemainConst.DAY_14_REMAIN, CRemainConst.DAY_30_REMAIN, CRemainConst.DAY_60_REMAIN, CRemainConst.DAY_90_REMAIN };
    DateTime m_lastTime = DateTime.Now;

    public override void init() 
    {
        initStatModule();
        initChannel();
    }

    public override void update(double delta)
    {
        DateTime now = DateTime.Now;
        TimeSpan span = now - m_lastTime;
        double totalMin = span.TotalMinutes;
        bool isStat = false;

        List<ChannelInfo> channelList = ResMgr.getInstance().getChannelList();
        foreach (var info in channelList)
        {
            bool res = statChannel(info);
            if (res)
            {
                resetChannelStatDay(info);
            }
            else
            {
                if (totalMin > 30)
                {
                    isStat = true;
                    statChannelByInterval(info);
                }
            }
        }

        if (isStat)
        {
            m_lastTime = now;
        }
    }

    private void initStatModule()
    {
        m_statModule.Add(StatFlag.STAT_FLAG_ACTIVE, new StatUnitActive());
        m_statModule.Add(StatFlag.STAT_FLAG_RECHARGE, new StatUnitRecharge());
        m_statModule.Add(StatFlag.STAT_FLAG_REMAIN, new StatUnitRemain());
        m_statModule.Add(StatFlag.STAT_FLAG_COUNT, new StatUnitCount());
        m_statModule.Add(StatFlag.STAT_LTV, new StatLTV());
    }

    private void initChannel()
    {
        List<ChannelInfo> channelList = ResMgr.getInstance().getChannelList();
        foreach (var info in channelList)
        {
            Dictionary<string, object> data =
                MongodbAccount.Instance.ExecuteGetOneBykey(TableName.CHANNEL_STAT_DAY, "channel", info.m_channelNum);
            if (data != null)
            {
                info.m_statDay = Convert.ToDateTime(data["statDay"]).ToLocalTime();
            }
            else
            {
                DateTime now = DateTime.Now.Date;
                info.m_statDay = now.AddDays(1);

                Dictionary<string, object> newData = new Dictionary<string, object>();
                newData.Add("statDay", info.m_statDay);
                newData.Add("channel", info.m_channelNum);
                MongodbAccount.Instance.ExecuteStoreBykey(TableName.CHANNEL_STAT_DAY, "channel", info.m_channelNum, newData);
            }
        }
    }

    private bool statChannel(ChannelInfo info)
    {
        if (!StatBase.canStat(info))
            return false;

        beginStat("渠道[{0}]开始统计", info.m_channelName);

        ParamStat param = new ParamStat();
        param.m_channel = info;

        StatResult result = new StatResult();

        foreach (var stat in m_statModule.Values)
        {
            stat.doStat(param, result);
        }

        Dictionary<string, object> newData = getData(result, info);

        IMongoQuery imq1 = Query.EQ("genTime", BsonValue.Create(info.m_statDay.Date.AddDays(-1)));
        IMongoQuery imq2 = Query.EQ("channel", BsonValue.Create(info.m_channelNum));
        IMongoQuery imq = Query.And(imq1, imq2);

        foreach (var d in s_remain)
        {
            updateRemain(info, d, result);
            updateDevRemain(info, d, result);
        }

        string str = MongodbAccount.Instance.ExecuteStoreByQuery(TableName.CHANNEL_TD, imq, newData);

        foreach (var d in s_totalRecharge)
        {
            updateTotalRecharge(info, d, result);
        }

        endStat("渠道[{0}]结束统计", info.m_channelName);
        return str == string.Empty;
    }

    // 按照一定的时间间隔统计
    private bool statChannelByInterval(ChannelInfo info)
    {
        beginStat("渠道[{0}]开始统计，按时间间隔", info.m_channelName);

        ParamStat param = new ParamStat();
        param.m_channel = info;

        StatResult result = new StatResult();

        foreach (var stat in m_statModule)
        {
            if (stat.Key == StatFlag.STAT_FLAG_ACTIVE ||
                stat.Key == StatFlag.STAT_FLAG_RECHARGE ||
                stat.Key == StatFlag.STAT_FLAG_COUNT)
            {
                stat.Value.doStat(param, result);
            }
        }

        Dictionary<string, object> newData = getData(result, info);

        IMongoQuery imq1 = Query.EQ("genTime", BsonValue.Create(info.m_statDay.Date.AddDays(-1)));
        IMongoQuery imq2 = Query.EQ("channel", BsonValue.Create(info.m_channelNum));
        IMongoQuery imq = Query.And(imq1, imq2);

        string str = MongodbAccount.Instance.ExecuteStoreByQuery(TableName.CHANNEL_TD, imq, newData);

        endStat("渠道[{0}]结束统计，按时间间隔", info.m_channelName);
        return str == string.Empty;
    }

    private void resetChannelStatDay(ChannelInfo info)
    {
        Dictionary<string, object> upData = new Dictionary<string, object>();
        upData.Add("statDay", info.m_statDay.Date.AddDays(1));
        MongodbAccount.Instance.ExecuteStoreBykey(TableName.CHANNEL_STAT_DAY, "channel", info.m_channelNum, upData);
        info.m_statDay = info.m_statDay.Date.AddDays(1);
    }

    // 更新往日留存
    private void updateRemain(ChannelInfo info, int days, StatResult result)
    {
        IMongoQuery imq1 = Query.EQ("genTime", StatBase.getRemainRegTime(info, days));
        IMongoQuery imq2 = Query.EQ("channel", BsonValue.Create(info.m_channelNum));
        IMongoQuery imq = Query.And(imq1, imq2);

        bool res = MongodbAccount.Instance.KeyExistsByQuery(TableName.CHANNEL_TD, imq);
        if (res)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            switch (days)
            {
                case CRemainConst.DAY_2_REMAIN:
                    {
                        data.Add("2DayRemainCount", result.m_2DayRemainCountTmp);
                        data.Add("2DayRemainCountRecharge", result.m_2DayRemainCountRechargeTmp);
                    }
                    break;
                case CRemainConst.DAY_3_REMAIN:
                    {
                        data.Add("3DayRemainCount", result.m_3DayRemainCountTmp);
                        data.Add("3DayRemainCountRecharge", result.m_3DayRemainCountRechargeTmp);
                    }
                    break;
                case CRemainConst.DAY_7_REMAIN:
                    {
                        data.Add("7DayRemainCount", result.m_7DayRemainCountTmp);
                        data.Add("7DayRemainCountRecharge", result.m_7DayRemainCountRechargeTmp);
                    }
                    break;
                case CRemainConst.DAY_30_REMAIN:
                    {
                        data.Add("30DayRemainCount", result.m_30DayRemainCountTmp);
                    }
                    break;
            }

            MongodbAccount.Instance.ExecuteUpdateByQuery(TableName.CHANNEL_TD, imq, data);
        }
    }

    // 更新往日设备留存
    private void updateDevRemain(ChannelInfo info, int days, StatResult result)
    {
        IMongoQuery imq1 = Query.EQ("genTime", StatBase.getRemainRegTime(info, days));
        IMongoQuery imq2 = Query.EQ("channel", BsonValue.Create(info.m_channelNum));
        IMongoQuery imq = Query.And(imq1, imq2);

        bool res = MongodbAccount.Instance.KeyExistsByQuery(TableName.CHANNEL_TD, imq);
        if (res)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            switch (days)
            {
                case CRemainConst.DAY_2_REMAIN:
                    {
                        data.Add("Day2DevRemainCount", result.m_2DayDevRemainCountTmp);
                    }
                    break;
                case CRemainConst.DAY_3_REMAIN:
                    {
                        data.Add("Day3DevRemainCount", result.m_3DayDevRemainCountTmp);
                    }
                    break;
                case CRemainConst.DAY_7_REMAIN:
                    {
                        data.Add("Day7DevRemainCount", result.m_7DayDevRemainCountTmp);
                    }
                    break;
                case CRemainConst.DAY_30_REMAIN:
                    {
                        data.Add("Day30DevRemainCount", result.m_30DayDevRemainCountTmp);
                    }
                    break;
            }

            MongodbAccount.Instance.ExecuteUpdateByQuery(TableName.CHANNEL_TD, imq, data);
        }
    }

    // 更新往日LTV价值
    private void updateTotalRecharge(ChannelInfo info, int days, StatResult result)
    {
        IMongoQuery imq1 = Query.EQ("genTime", StatBase.getRemainRegTime(info, days));
        IMongoQuery imq2 = Query.EQ("channel", BsonValue.Create(info.m_channelNum));
        IMongoQuery imq = Query.And(imq1, imq2);

        bool res = MongodbAccount.Instance.KeyExistsByQuery(TableName.CHANNEL_TD, imq);
        if (res)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            switch (days)
            {
                case 0:
                    {
                        // ltv 1日总充值，当日总充值
                        data.Add("Day1TotalRecharge", result.m_1DayTotalRechargeTmp);
                    }
                    break;
                case CRemainConst.DAY_2_REMAIN/*-1*/:
                    {
                        //data.Add("Day1TotalRecharge", result.m_1DayTotalRechargeTmp);

                        // 次日付费人数
                        data.Add("Day1RechargePersonNum", result.m_2DayRechargePersonNumTmp);
                    }
                    break;
                case CRemainConst.DAY_3_REMAIN:
                    {
                        data.Add("Day3TotalRecharge", result.m_3DayTotalRechargeTmp);
                    }
                    break;
                case CRemainConst.DAY_7_REMAIN:
                    {
                        data.Add("Day7TotalRecharge", result.m_7DayTotalRechargeTmp);
                    }
                    break;
                case CRemainConst.DAY_14_REMAIN:
                    {
                        data.Add("Day14TotalRecharge", result.m_14DayTotalRechargeTmp);
                    }
                    break;
                case CRemainConst.DAY_30_REMAIN:
                    {
                        data.Add("Day30TotalRecharge", result.m_30DayTotalRechargeTmp);
                    }
                    break;
                case CRemainConst.DAY_60_REMAIN:
                    {
                        data.Add("Day60TotalRecharge", result.m_60DayTotalRechargeTmp);
                    }
                    break;
                case CRemainConst.DAY_90_REMAIN:
                    {
                        data.Add("Day90TotalRecharge", result.m_90DayTotalRechargeTmp);
                    }
                    break;
            }

            MongodbAccount.Instance.ExecuteUpdateByQuery(TableName.CHANNEL_TD, imq, data);
        }
    }

    Dictionary<string, object> getData(StatResult result, ChannelInfo info)
    {
        Dictionary<string, object> newData = new Dictionary<string, object>();
        newData.Add("genTime", info.m_statDay.Date.AddDays(-1));
        newData.Add("channel", info.m_channelNum);

        newData.Add("regeditCount", result.m_regeditCount);
        newData.Add("deviceActivationCount", result.m_deviceActivationCount);
        newData.Add("activeCount", result.m_activeCount);
        newData.Add("deviceLoginCount", result.m_deviceLoginCount);

        newData.Add("totalIncome", result.m_totalIncome);
        newData.Add("rechargePersonNum", result.m_rechargePersonNum);
        newData.Add("rechargeCount", result.m_rechargeCount);

        newData.Add("newAccIncome", result.m_newAccIncome);
        newData.Add("newAccRechargePersonNum", result.m_newAccRechargePersonNum);

        newData.Add("2DayRemainCount", result.m_2DayRemainCount);

        newData.Add("3DayRemainCount", result.m_3DayRemainCount);

        newData.Add("7DayRemainCount", result.m_7DayRemainCount);

        newData.Add("30DayRemainCount", result.m_30DayRemainCount);

        newData.Add("enterFishRoomCount", result.m_enterFishRoomCount);

        //////////////////////////////////////////////////////////////////////////
        newData.Add("Day2DevRemainCount", result.m_2DayDevRemainCount);
        newData.Add("Day3DevRemainCount", result.m_3DayDevRemainCount);
        newData.Add("Day7DevRemainCount", result.m_7DayDevRemainCount);
        newData.Add("Day30DevRemainCount", result.m_30DayDevRemainCount);

        newData.Add("Day1TotalRecharge", result.m_1DayTotalRecharge);
        newData.Add("Day3TotalRecharge", result.m_3DayTotalRecharge);
        newData.Add("Day7TotalRecharge", result.m_7DayTotalRecharge);
        newData.Add("Day14TotalRecharge", result.m_14DayTotalRecharge);
        newData.Add("Day30TotalRecharge", result.m_30DayTotalRecharge);
        newData.Add("Day60TotalRecharge", result.m_60DayTotalRecharge);
        newData.Add("Day90TotalRecharge", result.m_90DayTotalRecharge);

        return newData;
    }
}

//////////////////////////////////////////////////////////////////////////
// 玩家拥有的金币总量
class SysStatPlayerTotalMoney : SysBase
{
    DateTime m_statDay;
    long m_moneyTotal; // 玩家金币
    long m_safeBox;    // 保险箱内金币

    public override void init()
    {
        Dictionary<string, object> data =
                MongodbPlayer.Instance.ExecuteGetOneBykey(TableName.TOTAL_MONEY_STAT_DAY, "key", "playerTotalMoney");
        if (data != null)
        {
            m_statDay = Convert.ToDateTime(data["statDay"]).ToLocalTime();
        }
        else
        {
            DateTime now = DateTime.Now.Date;
            m_statDay = now.AddDays(1);
            resetStatDay(m_statDay);
        }
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddHours(1);

        if (DateTime.Now < st)
            return;
        
        beginStat("SysStatPlayerTotalMoney开始统计");

        m_moneyTotal = -1;
        m_safeBox = -1;
        stat();
        if (m_moneyTotal >= 0)
        {
            Dictionary<string, object> newData = new Dictionary<string, object>();
            newData.Add("genTime", DateTime.Now.Date);
            newData.Add("money", m_moneyTotal);
            newData.Add("safeBox", m_safeBox);
            MongodbLog.Instance.ExecuteInsert(TableName.PUMP_PLAYER_TOTAL_MONEY, newData);
        }

        m_statDay = m_statDay.AddDays(1);
        resetStatDay(m_statDay);

        endStat("SysStatPlayerTotalMoney结束统计");
    }

    void stat()
    {
        DateTime now = DateTime.Now;
        //IMongoQuery imq1 = Query.GTE("logout_time", now.AddDays(-7));
        IMongoQuery imq = Query.EQ("is_robot", false);
        //IMongoQuery imq = Query.And(imq1, imq2);
        MapReduceResult mapResult = MongodbPlayer.Instance.executeMapReduce(TableName.PLAYER_INFO,
                                                                             imq,
                                                                             MapReduceTable.getMap("totalMoney"),
                                                                             MapReduceTable.getReduce("totalMoney"));

        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                BsonValue resValue = d["value"];
                m_moneyTotal = resValue["total"].ToInt64();
                m_safeBox = resValue["box"].ToInt64();
            }
        }
    }

    void resetStatDay(DateTime statDay)
    {
        Dictionary<string, object> upData = new Dictionary<string, object>();
        upData.Add("statDay", statDay);
        MongodbPlayer.Instance.ExecuteStoreBykey(TableName.TOTAL_MONEY_STAT_DAY,
            "key", "playerTotalMoney", upData);
    }
}

//////////////////////////////////////////////////////////////////////////

class StatByDayBase : SysBase
{
    protected DateTime m_statDay;

    public override void init()
    {
        Dictionary<string, object> data =
                MongodbPlayer.Instance.ExecuteGetOneBykey(TableName.TOTAL_MONEY_STAT_DAY, "key", 
                getStatKey());
        if (data != null)
        {
            m_statDay = Convert.ToDateTime(data["statDay"]).ToLocalTime();
        }
        else
        {
            DateTime now = DateTime.Now.Date;
            m_statDay = now.AddDays(1);
            resetStatDay(m_statDay);
        }
    }

    protected void resetStatDay(DateTime statDay)
    {
        Dictionary<string, object> upData = new Dictionary<string, object>();
        upData.Add("statDay", statDay);
        MongodbPlayer.Instance.ExecuteStoreBykey(TableName.TOTAL_MONEY_STAT_DAY,
            "key", getStatKey(), upData);
    }

    public virtual string getStatKey()
    {
        throw new Exception();
    }

    protected void addStatDay()
    {
        m_statDay = m_statDay.AddDays(1);
        resetStatDay(m_statDay);
    }
}

class StatByMonthBase : SysBase
{
    protected DateTime m_statDay;

    public override void init()
    {
        Dictionary<string, object> data =
                MongodbPlayer.Instance.ExecuteGetOneBykey(TableName.TOTAL_MONEY_STAT_DAY, "key",
                getStatKey());
        if (data != null)
        {
            m_statDay = Convert.ToDateTime(data["statDay"]).ToLocalTime();
        }
        else
        {
            DateTime now = DateTime.Now.Date;
            now = new DateTime(now.Year, now.Month, 1);
            m_statDay = now.AddMonths(1);
            resetStatDay(m_statDay);
        }
    }

    protected void resetStatDay(DateTime statDay)
    {
        Dictionary<string, object> upData = new Dictionary<string, object>();
        upData.Add("statDay", statDay);
        MongodbPlayer.Instance.ExecuteStoreBykey(TableName.TOTAL_MONEY_STAT_DAY,
            "key", getStatKey(), upData);
    }

    public virtual string getStatKey()
    {
        throw new Exception();
    }

    protected void addStatDay()
    {
        m_statDay = m_statDay.AddMonths(1);
        resetStatDay(m_statDay);
    }
}

//////////////////////////////////////////////////////////////////////////
// 大R流失
class SysStatLose : StatByDayBase
{
    static string[] PLAYER_FIELDS = { "player_id", "nickname", "VipLevel", "gold", "ticket", "dragonBall" };
    static string[] PLAYER_FIELDS_1 = { "account" };
    IMongoQuery m_queryCond = null;

    public override void init()
    {
        base.init();

        IMongoQuery imq1 = Query.GTE("VipLevel", BsonValue.Create(5));
        IMongoQuery imq2 = Query.EQ("is_robot", BsonValue.Create(false));
        m_queryCond = Query.And(imq1, imq2);
    }

    public override string getStatKey()
    {
        return StatKey.KEY_LOSE;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddHours(1.5);

        if (DateTime.Now < st)
            return;

        beginStat("SysStatLose开始统计");

        MongodbPlayer.Instance.ExecuteRemoveAll(TableName.RLOSE);
        DateTime mint = m_statDay.Date.AddDays(-1), maxt = m_statDay.Date;

        int skip = 0;
        List<Dictionary<string, object>> dataList = null;
        while (true)
        {
            dataList = QueryTool.nextData(MongodbPlayer.Instance,
                                      TableName.PLAYER_INFO,
                                      m_queryCond,
                                      ref skip,
                                      1000,
                                      PLAYER_FIELDS_1);
            if (dataList == null)
                break;

            for (int i = 0; i < dataList.Count; i++)
            {
                Dictionary<string, object> data = dataList[i];
                if (data.ContainsKey("account"))
                {
                    bool code = QueryTool.isLogin(Convert.ToString(data["account"]), mint, maxt);
                    if (!code)
                    {
                        addLosePlayer(Convert.ToString(data["account"]));
                    }
                }
            }
        }

        m_statDay = m_statDay.AddDays(1);
        resetStatDay(m_statDay);

        endStat("SysStatLose结束统计");
    }

    void addLosePlayer(string acc)
    {
        Dictionary<string, object> data = MongodbPlayer.Instance.ExecuteGetBykey(TableName.PLAYER_INFO, "account", acc, PLAYER_FIELDS);
        if (data == null)
            return;

        Dictionary<string, object> newData = new Dictionary<string, object>();
        newData.Add("playerId", Convert.ToInt32(data["player_id"]));
        newData.Add("nickName", Convert.ToString(data["nickname"]));
        newData.Add("vipLevel", Convert.ToInt32(data["VipLevel"]));
        newData.Add("gold", Convert.ToInt64(data["gold"]));
        newData.Add("gem", Convert.ToInt32(data["ticket"]));

        if (data.ContainsKey("dragonBall"))
        {
            newData.Add("dragonBall", Convert.ToInt32(data["dragonBall"]));
        }
        else
        {
            newData.Add("dragonBall", 0);
        }

        MongodbPlayer.Instance.ExecuteInsert(TableName.RLOSE, newData);
    }
}

//////////////////////////////////////////////////////////////////////////
// 根据一定时间间隔统计
class StatByIntervalBase : SysBase
{
    protected DateTime m_statTime;

    public override void init()
    {
        Dictionary<string, object> data =
                MongodbPlayer.Instance.ExecuteGetOneBykey(TableName.TOTAL_MONEY_STAT_DAY, "key",
                getStatKey());
        if (data != null)
        {
            m_statTime = Convert.ToDateTime(data["statDay"]).ToLocalTime();
        }
        else
        {
            DateTime now = DateTime.Now.Date.AddMinutes(getStatInterval());
            m_statTime = now;
            resetStatDay(m_statTime);
        }
    }

    protected void resetStatDay(DateTime statDay)
    {
        Dictionary<string, object> upData = new Dictionary<string, object>();
        upData.Add("statDay", statDay);
        MongodbPlayer.Instance.ExecuteStoreBykey(TableName.TOTAL_MONEY_STAT_DAY,
            "key", getStatKey(), upData);
    }

    protected void addStatTime()
    {
        m_statTime = m_statTime.AddMinutes(getStatInterval());
        resetStatDay(m_statTime);
    }

    public virtual string getStatKey()
    {
        throw new Exception();
    }

    // 返回统计间隔(分钟)
    public virtual int getStatInterval()
    {
        throw new Exception();
    }
}

//////////////////////////////////////////////////////////////////////////
// 根据时间间隔统计玩家龙珠
class StatPlayerDragonBall : StatByDayBase
{
    static string[] FIELDS_NEW_VALUE = { "newValue" };
    static string[] FIELDS_GOLD_REMAIN = { "goldRemain" };
    static string[] FIELDS_GEM_REMAIN = { "gemRemain" };
    static string[] FIELDS_DRAGON_REMAIN = { "dbRemain" };

    static string[] FIELDS_OLD_VALUE = { "oldValue" };

    public override string getStatKey()
    {
        return StatKey.KEY_DRAGON;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddHours(2);

        if (DateTime.Now < st)
            return;

        beginStat("StatPlayerDragonBall开始统计");

        stat();

        addStatDay();

        endStat("StatPlayerDragonBall结束统计");
    }

    void stat()
    {
        DateTime startTime = m_statDay.AddDays(-1);
        DateTime endTime = m_statDay;
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(endTime));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(startTime));
        IMongoQuery imq = Query.And(imq1, imq2);

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_PLAYER_MONEY,
                                                                             imq,
                                                                             MapReduceTable.getMap("dragonBallPlayer"),
                                                                             MapReduceTable.getReduce("dragonBallPlayer"));

        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            StatDragonItem item = new StatDragonItem();
            foreach (BsonDocument d in bson)
            {
                item.m_playerId = Convert.ToInt32(d["_id"]);
                BsonValue resValue = d["value"];

                item.m_dbgain = resValue["dbgain"].ToInt64();
                item.m_dbsend = resValue["dbsend"].ToInt64();
                item.m_dbaccept = resValue["dbaccept"].ToInt64();
                item.m_dbexchange = resValue["dbexchange"].ToInt64();
                item.m_dbRemain = getLastVal(endTime, 14, item.m_playerId, FIELDS_DRAGON_REMAIN);
                item.m_dbStart = getStartVal(startTime, 14, item.m_playerId, FIELDS_DRAGON_REMAIN);

                item.m_goldByRecharge = resValue["goldByRecharge"].ToInt64();
                item.m_goldByOther = resValue["goldByOther"].ToInt64();
                item.m_goldConsume = resValue["goldConsume"].ToInt64();
                item.m_goldRemain = getLastVal(endTime, 1, item.m_playerId, FIELDS_GOLD_REMAIN);
                item.m_goldStart = getStartVal(startTime, 1, item.m_playerId, FIELDS_GOLD_REMAIN);

                item.m_gemByRecharge = resValue["gemByRecharge"].ToInt64();
                item.m_gemByOther = resValue["gemByOther"].ToInt64();
                item.m_gemConsume = resValue["gemConsume"].ToInt64();
                item.m_gemRemain = getLastVal(endTime, 2, item.m_playerId, FIELDS_GEM_REMAIN);
                item.m_gemStart = getStartVal(startTime, 2, item.m_playerId, FIELDS_GEM_REMAIN);

                item.m_todayRecharge = getTotalRecharge(startTime, endTime, item.m_playerId);
                addData(item, startTime);
            }
        }
    }

    void addData(StatDragonItem item, DateTime time)
    {
        IMongoQuery imq1 = Query.EQ("playerId", BsonValue.Create(item.m_playerId));
        IMongoQuery imq2 = Query.EQ("genTime", BsonValue.Create(time));
        IMongoQuery imq = Query.And(imq1, imq2);

        Dictionary<string, object> data = new Dictionary<string, object>();

        data.Add("dbgain", item.m_dbgain);
        data.Add("dbsend", item.m_dbsend);
        data.Add("dbaccept", item.m_dbaccept);
        data.Add("dbexchange", item.m_dbexchange);
        data.Add("dbRemain", item.m_dbRemain);
        data.Add("dbStart", item.m_dbStart);

        data.Add("goldByRecharge", item.m_goldByRecharge);
        data.Add("goldByOther", item.m_goldByOther);
        data.Add("goldConsume", item.m_goldConsume);
        data.Add("goldRemain", item.m_goldRemain);
        data.Add("goldStart", item.m_goldStart);

        data.Add("gemByRecharge", item.m_gemByRecharge);
        data.Add("gemByOther", item.m_gemByOther);
        data.Add("gemConsume", item.m_gemConsume);
        data.Add("gemRemain", item.m_gemRemain);
        data.Add("gemStart", item.m_gemStart);

        data.Add("todayRecharge", item.m_todayRecharge);

        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_PLAYER_DRAGON, imq, data);
    }

    // 返回时间段imq的最后一条关于itemId的数据
    public static int getLastVal(DateTime endTime, int itemId, int playerId, string[] remainFields)
    {
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(endTime));
        IMongoQuery imq = Query.And(imq1, Query.EQ("itemId", itemId), Query.EQ("playerId", playerId));

        List<Dictionary<string, object>> dataList = MongodbLog.Instance.ExecuteGetListByQuery(TableName.PUMP_PLAYER_MONEY,
             imq, FIELDS_NEW_VALUE, "genTime", false, 0, 1);

        int retVal = 0;
        if (dataList != null && dataList.Count > 0)
        {
            if (dataList[0].ContainsKey("newValue"))
            {
                retVal = Convert.ToInt32(dataList[0]["newValue"]);
            }
        }

        return retVal;
    }

    // 得到初始值
    public static int getStartVal(DateTime startTime, int itemId, int playerId, string[] remainFields)
    {
        int retVal = 0;
        IMongoQuery imq1 = Query.LT("genTime", startTime);
        IMongoQuery imq = Query.And(imq1, Query.EQ("itemId", itemId), Query.EQ("playerId", playerId));

        List<Dictionary<string, object>> dataList = MongodbLog.Instance.ExecuteGetListByQuery(TableName.PUMP_PLAYER_MONEY,
            imq, FIELDS_NEW_VALUE, "genTime", false, 0, 1);
        if (dataList != null && dataList.Count > 0)
        {
            if (dataList[0].ContainsKey("newValue"))
            {
                retVal = Convert.ToInt32(dataList[0]["newValue"]);
            }
        }

        return retVal;
    }

    // 返回总的充值
    int getTotalRecharge(DateTime startTime, DateTime endTime, int playerId)
    {
        int retVal = 0;
        IMongoQuery imq1 = Query.LT("time", BsonValue.Create(endTime));
        IMongoQuery imq2 = Query.GTE("time", BsonValue.Create(startTime));

        Dictionary<string, object> qr = MongodbPlayer.Instance.ExecuteGetOneBykey(TableName.PLAYER_INFO,
            "player_id",
            playerId, new string[] { "account" });
        if (qr != null)
        {
            string acc = Convert.ToString(qr["account"]);
            IMongoQuery imq = Query.And(imq1, imq2, Query.EQ("acc", acc));

            MapReduceResult mapResult = MongodbPayment.Instance.executeMapReduce("PayLog",
                                                                             imq,
                                                                             MapReduceTable.getMap("recharge"),
                                                                             MapReduceTable.getReduce("recharge"));
            if (mapResult != null)
            {
                IEnumerable<BsonDocument> bson = mapResult.GetResults();
                foreach (BsonDocument d in bson)
                {
                    BsonValue resValue = d["value"];
                    retVal += resValue["total"].ToInt32();
                }
            }
        }

        return retVal;
    }

    public static int getLastValNew(IMongoQuery imqCond, int itemId, int playerId)
    {
        IMongoQuery imq1 = imqCond; // Query.LT("genTime", BsonValue.Create(endTime));
        IMongoQuery imq = Query.And(imq1, Query.EQ("itemId", itemId), Query.EQ("playerId", playerId));

        List<Dictionary<string, object>> dataList = MongodbLog.Instance.ExecuteGetListByQuery(TableName.PUMP_PLAYER_MONEY,
             imq, FIELDS_NEW_VALUE, "genTime", false, 0, 1);

        int retVal = 0;
        if (dataList != null && dataList.Count > 0)
        {
            if (dataList[0].ContainsKey("newValue"))
            {
                retVal = Convert.ToInt32(dataList[0]["newValue"]);
            }
        }
        return retVal;
    }

    public static int getStartValNew(IMongoQuery imqCond, int itemId, int playerId)
    {
        int retVal = 0;
        IMongoQuery imq1 = imqCond; // Query.GTE("genTime", startTime);
        IMongoQuery imq = Query.And(imq1, Query.EQ("itemId", itemId), Query.EQ("playerId", playerId));

        // 比startTime大的第一条记录
        List<Dictionary<string, object>> dataList = MongodbLog.Instance.ExecuteGetListByQuery(TableName.PUMP_PLAYER_MONEY,
            imq, FIELDS_OLD_VALUE, "genTime", true, 0, 1);
        if (dataList != null && dataList.Count > 0)
        {
            if (dataList[0].ContainsKey("oldValue"))
            {
                retVal = Convert.ToInt32(dataList[0]["oldValue"]);
            }
        }

        return retVal;
    }
}

//////////////////////////////////////////////////////////////////////////
// 龙珠每日总计
class StatDragonBallTotal : StatByDayBase
{
    public override string getStatKey()
    {
        return StatKey.KEY_DRAGON_DAILY;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddHours(2);

        if (DateTime.Now < st)
            return;

        beginStat("StatDragonBallTotal开始统计");

        stat();

        addStatDay();

        endStat("StatDragonBallTotal结束统计");
    }

    void stat()
    {
        DateTime startTime = m_statDay.AddDays(-1);
        DateTime endTime = m_statDay;

        StatDragonDailyItem item = new StatDragonDailyItem();
        statTodayDragonBall(startTime, endTime, item);
        statTodayRecharge(startTime, endTime, item);
        statTodayDragonRemin(startTime, endTime, item);
        addData(item, startTime);
    }

    void addData(StatDragonDailyItem item, DateTime time)
    {
        IMongoQuery imq = Query.EQ("genTime", BsonValue.Create(time));

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("todayRecharge", item.m_todayRecharge);
        data.Add("dragonBallGen", item.m_dragonBallGen);
        data.Add("dragonBallConsume", item.m_dragonBallConsume);
        data.Add("dragonBallRemain", item.m_dragonBallRemain);

        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_DRAGON_DAILY, imq, data);
    }

    // 今日产出龙珠
    void statTodayDragonBall(DateTime startTime, DateTime endTime, StatDragonDailyItem item)
    {
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(endTime));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(startTime));
        IMongoQuery imq = Query.And(imq1, imq2);

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_PLAYER_MONEY,
                                                                             imq,
                                                                             MapReduceTable.getMap("dragonBallDaily"),
                                                                             MapReduceTable.getReduce("dragonBallDaily"));

        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                BsonValue resValue = d["value"];
                item.m_dragonBallGen = resValue["dbGen"].ToInt64();
                item.m_dragonBallConsume = resValue["dbConsume"].ToInt64();
            }
        }
    }

    // 今日总充值
    void statTodayRecharge(DateTime startTime, DateTime endTime, StatDragonDailyItem item)
    {
        IMongoQuery imq1 = Query.LT("time", BsonValue.Create(endTime));
        IMongoQuery imq2 = Query.GTE("time", BsonValue.Create(startTime));
        IMongoQuery imq = Query.And(imq1, imq2);

        MapReduceResult mapResult = MongodbPayment.Instance.executeMapReduce("PayLog",
                                                                         imq,
                                                                         MapReduceTable.getMap("recharge"),
                                                                         MapReduceTable.getReduce("recharge"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                BsonValue resValue = d["value"];
                item.m_todayRecharge += resValue["total"].ToInt32();
            }
        }
    }

    // 统计剩余龙珠
    void statTodayDragonRemin(DateTime startTime, DateTime endTime, StatDragonDailyItem item)
    {
        IMongoQuery imq = Query.EQ("is_robot", false);
        MapReduceResult mapResult = MongodbPlayer.Instance.executeMapReduce(TableName.PLAYER_INFO,
                                                                         imq,
                                                                         MapReduceTable.getMap("dragonBallSum"),
                                                                         MapReduceTable.getReduce("dragonBallSum"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                BsonValue resValue = d["value"];
                item.m_dragonBallRemain += resValue["total"].ToInt64();
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 在线游戏时间统计
class StatOnlineGameTime : StatByIntervalBase
{
    public override string getStatKey()
    {
        return StatKey.KEY_ONLINE_GAME_TIME;
    }

    // 返回统计间隔(分钟)
    public override int getStatInterval()
    {
        return 10;
    }

    public override void update(double delta)
    {
        if (DateTime.Now < m_statTime)
            return;

        beginStat("StatOnlineGameTime开始统计");
        stat();

        addStatTime();
        endStat("StatOnlineGameTime结束统计");
    }

    void stat()
    {
        IMongoQuery imq1 = Query.LT("loginTime", m_statTime);
        IMongoQuery imq2 = Query.GTE("loginTime", m_statTime.AddMinutes(-getStatInterval()));
        IMongoQuery imq = Query.And(imq1, imq2);

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_PLAYER_ONLINE_TIME,
                                                                        null,
                                                                        MapReduceTable.getMap("playerOnlineTime"),
                                                                        MapReduceTable.getReduce("playerOnlineTime"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                int playerId = Convert.ToInt32(d["_id"]);
                BsonValue resValue = d["value"];
                double totalTime = resValue["onlineTimeSum"].ToInt64();

                addData(playerId, totalTime);
            }
        }

        // 删除原有数据
        MongodbLog.Instance.ExecuteRemoveAll(TableName.PUMP_PLAYER_ONLINE_TIME);
    }

    void addData(int playerId, double totalTime)
    {
        IMongoQuery imq = Query.EQ("playerId", playerId);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("totalGameTime", (long)(totalTime/1000));

        MongodbPlayer.Instance.ExecuteIncByQuery(TableName.STAT_PLAYER_GAME_TIME, imq, data);
    }
}

//////////////////////////////////////////////////////////////////////////
// 总收支表
class StatPlayerTotalIncomeExpenses : StatByDayBase
{
    List<BsonDocument> m_remain = new List<BsonDocument>();
    List<BsonDocument> m_error = new List<BsonDocument>();

    public override string getStatKey()
    {
        return StatKey.KEY_INCOME_EXPENSES;
    }

    public override void update(double delta)
    {
        if (DateTime.Now.Date < m_statDay)
            return;

        stat();

        addStatDay();
    }

    void stat()
    {
        m_remain.Clear();
        m_error.Clear();

        DateTime startTime = m_statDay.AddDays(-1);
        DateTime endTime = m_statDay;
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(endTime));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(startTime));
        IMongoQuery imq = Query.And(imq1, imq2);

        beginStat("StatPlayerTotalIncomeExpenses MapReduce开始统计");

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_PLAYER_MONEY,
                                                                             imq,
                                                                             MapReduceTable.getMap("incomeExpenses"),
                                                                             MapReduceTable.getReduce("incomeExpenses"));
        endStat("StatPlayerTotalIncomeExpenses MapReduce结束统计");

        if (mapResult != null)
        {
            beginStat("StatPlayerTotalIncomeExpenses 开始写入数据");
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            StatIncomeExpensesItemBase item = new StatIncomeExpensesItemBase();
            foreach (BsonDocument d in bson)
            {
                item.m_playerId = Convert.ToInt32(d["_id"]);
                BsonValue resValue = d["value"];

                item.m_goldFreeGain = resValue["goldFreeGain"].ToInt64();
                item.m_goldRechargeGain = resValue["goldRechargeGain"].ToInt64();
                item.m_goldConsume = resValue["goldConsume"].ToInt64();
                item.m_goldRemain = StatPlayerDragonBall.getLastValNew(imq, 1, item.m_playerId);
                item.m_goldStart = StatPlayerDragonBall.getStartValNew(imq, 1, item.m_playerId);

                item.m_gemFreeGain = resValue["gemFreeGain"].ToInt64();
                item.m_gemRechargeGain = resValue["gemRechargeGain"].ToInt64();
                item.m_gemConsume = resValue["gemConsume"].ToInt64();
                item.m_gemRemain = StatPlayerDragonBall.getLastValNew(imq, 2, item.m_playerId);
                item.m_gemStart = StatPlayerDragonBall.getStartValNew(imq, 2, item.m_playerId);

                item.m_dbFreeGain = resValue["dbFreeGain"].ToInt64();
                item.m_dbConsume = resValue["dbConsume"].ToInt64();
                item.m_dbRemain = StatPlayerDragonBall.getLastValNew(imq, 14, item.m_playerId);
                item.m_dbStart = StatPlayerDragonBall.getStartValNew(imq, 14, item.m_playerId);

                item.m_chipFreeGain = resValue["chipFreeGain"].ToInt64();
                item.m_chipConsume = resValue["chipConsume"].ToInt64();
                item.m_chipRemain = StatPlayerDragonBall.getLastValNew(imq, 11, item.m_playerId);
                item.m_chipStart = StatPlayerDragonBall.getStartValNew(imq, 11, item.m_playerId);

                long dropDb = resValue["dbDrop"].ToInt64();

                addData(item, startTime, dropDb > 0);
            }

            end();

            endStat("StatPlayerTotalIncomeExpenses 结束写入数据");
        }

        /*IMongoQuery imq3 = Query.EQ("is_robot", false);

        MapReduceResult mapResult1 = MongodbPlayer.Instance.executeMapReduce(TableName.PLAYER_INFO,
                                                                             imq3,
                                                                             MapReduceTable.getMap("incomeExpensesRemain"),
                                                                             MapReduceTable.getReduce("incomeExpensesRemain"));

        if (mapResult1 != null)
        {
            IEnumerable<BsonDocument> bson = mapResult1.GetResults();
            StatIncomeExpensesItemBase item = new StatIncomeExpensesItemBase();
            foreach (BsonDocument d in bson)
            {
                BsonValue resValue = d["value"];

                item.m_goldRemain = resValue["goldRemain"].ToInt64();
                item.m_gemRemain = resValue["gemRemain"].ToInt64();
                item.m_dbRemain = resValue["dbRemain"].ToInt64();
                item.m_chipRemain = resValue["chipRemain"].ToInt64();

                addData1(item, startTime);
            }
        }*/
    }

    void addData(StatIncomeExpensesItemBase item, DateTime time, bool dropDb)
    {
        //IMongoQuery imq1 = Query.EQ("playerId", BsonValue.Create(item.m_playerId));
       // IMongoQuery imq2 = Query.EQ("genTime", BsonValue.Create(time));
        //IMongoQuery imq = Query.And(imq1, imq2);

        /*Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("playerId", BsonValue.Create(item.m_playerId));
        data.Add("genTime", BsonValue.Create(time));

        data.Add("goldFreeGain", item.m_goldFreeGain);
        data.Add("goldRechargeGain", item.m_goldRechargeGain);
        data.Add("goldConsume", item.m_goldConsume);
        data.Add("goldRemain", item.m_goldRemain);
        data.Add("goldStart", item.m_goldStart);

        data.Add("gemFreeGain", item.m_gemFreeGain);
        data.Add("gemRechargeGain", item.m_gemRechargeGain);
        data.Add("gemConsume", item.m_gemConsume);
        data.Add("gemRemain", item.m_gemRemain);
        data.Add("gemStart", item.m_gemStart);

        data.Add("dbFreeGain", item.m_dbFreeGain);
        data.Add("dbConsume", item.m_dbConsume);
        data.Add("dbRemain", item.m_dbRemain);
        data.Add("dbStart", item.m_dbStart);

        data.Add("chipFreeGain", item.m_chipFreeGain);
        data.Add("chipConsume", item.m_chipConsume);
        data.Add("chipRemain", item.m_chipRemain);
        data.Add("chipStart", item.m_chipStart);

        data.Add("isDropDb", dropDb);

        m_remain.Add(data.ToBsonDocument());*/

        //MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_INCOME_EXPENSES, imq, data);

        //////////////////////////////////////////////////////////////////////////
        bool goldError = false, gemError = false, dbError = false, chipError = false;
        goldError =
            !isEqual(item.m_goldStart + item.m_goldRechargeGain +
            item.m_goldFreeGain - item.m_goldConsume, item.m_goldRemain);

         gemError =
            !isEqual(item.m_gemStart + item.m_gemRechargeGain +
            item.m_gemFreeGain - item.m_gemConsume, item.m_gemRemain);

        dbError = !isEqual(item.m_dbStart + item.m_dbFreeGain - item.m_dbConsume, item.m_dbRemain);

        chipError =
           !isEqual(item.m_chipStart + item.m_chipFreeGain -
           item.m_chipConsume, item.m_chipRemain);

        addError(goldError, gemError, dbError, chipError, item.m_playerId, time);
    }

    void addData1(StatIncomeExpensesItemBase item, DateTime time)
    {
        IMongoQuery imq = Query.EQ("genTime", BsonValue.Create(time));

        Dictionary<string, object> data = new Dictionary<string, object>();

        data.Add("goldRemain", item.m_goldRemain);
        data.Add("gemRemain", item.m_gemRemain);
        data.Add("dbRemain", item.m_dbRemain);
        data.Add("chipRemain", item.m_chipRemain);

        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_INCOME_EXPENSES_REMAIN, imq, data);
    }

    bool isRelativeErrorBeyond(double cur, double accuracy)
    {
        if (accuracy == 0.0)
            return false;

        double delta = Math.Abs(cur - accuracy);
        double e = delta / accuracy;
        return e > 0.01;
    }

    bool isEqual(long cur, long accuracy)
    {
        return cur == accuracy;
    }

    void addError(bool goldError, bool gemError, bool dbError, bool chipError, int playerId, DateTime time)
    {
        if (!(goldError || gemError || dbError || chipError))
            return;

        //IMongoQuery imq1 = Query.EQ("playerId", playerId);
        //IMongoQuery imq2 = Query.EQ("genTime", BsonValue.Create(time));
        //IMongoQuery imq = Query.And(imq1, imq2);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("playerId", playerId);
        data.Add("genTime", time);
        data.Add("goldError", goldError);
        data.Add("gemError", gemError);
        data.Add("dbError", dbError);
        data.Add("chipError", chipError);

        m_error.Add(data.ToBsonDocument());
       // MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_INCOME_EXPENSES + "_error", imq, data);
    }

    void end()
    {
        if (m_remain.Count > 0)
        {
            MongodbLog.Instance.ExecuteInsterList(TableName.STAT_INCOME_EXPENSES, m_remain);
            m_remain.Clear();
        }
        if (m_error.Count > 0)
        {
            MongodbLog.Instance.ExecuteInsterList(TableName.STAT_INCOME_EXPENSES + "_error", m_error);
            m_error.Clear();
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 统计每小时的付费
class StatRechargePerHour : StatByIntervalBase
{
    public override string getStatKey()
    {
        return StatKey.KEY_RECHARGE_HOUR;
    }

    // 返回统计间隔(分钟)
    public override int getStatInterval()
    {
        return 60;
    }

    public override void update(double delta)
    {
        if (DateTime.Now < m_statTime)
            return;

        try
        {
            beginStat("StatRechargePerHour开始统计");
            stat();
            addStatTime();
            endStat("StatRechargePerHour结束统计");
        }
        catch (Exception ex)
        {
            LogMgr.log.Error(ex.ToString());
        }
    }

    void stat()
    {
        IMongoQuery imq1 = Query.LT("CreateTime", BsonValue.Create(m_statTime));
        IMongoQuery imq2 = Query.GTE("CreateTime", m_statTime.AddMinutes(-getStatInterval()));
        IMongoQuery imq = Query.And(imq1, imq2);

        int totalRecharge = 0;

        MapReduceResult mapResult = MongodbPayment.Instance.executeMapReduce(TableName.RECHARGE_TOTAL,
                                                                         imq,
                                                                         MapReduceTable.getMap("rechargeNew"),
                                                                         MapReduceTable.getReduce("rechargeNew"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                BsonValue resValue = d["value"];
                totalRecharge += resValue["total"].ToInt32();
            }
        }
        addData(totalRecharge, m_statTime.AddMinutes(-getStatInterval()));

        //////////////////////////////////////////////////////////////////////////
        DateTime stime = m_statTime.AddMinutes(-getStatInterval());
        MapReduceResult mapResult2 = MongodbPayment.Instance.executeMapReduce(TableName.RECHARGE_TOTAL,
                                                                        imq,
                                                                        MapReduceTable.getMap("StatRechargeHourByChannel"),
                                                                        MapReduceTable.getReduce("StatRechargeHourByChannel"));
        if (mapResult2 != null)
        {
            IEnumerable<BsonDocument> bson = mapResult2.GetResults();
            foreach (BsonDocument d in bson)
            {
                try
                {
                    string channel = Convert.ToString(d["_id"]);
                    BsonValue resValue = d["value"];
                    int total = resValue["total"].ToInt32();
                    addData2(channel, total, stime);
                }
                catch (System.Exception ex)
                {
                }
            }
        }
    }

    void addData(int totalRecharge, DateTime curTime)
    {
        int h = curTime.Hour;
        IMongoQuery imq = Query.EQ("genTime", curTime.Date);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("h" + h, totalRecharge);

        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_RECHARGE_HOUR, imq, data);
    }

    void addData2(string channel, int totalRecharge, DateTime curTime)
    {
        int h = curTime.Hour;
        IMongoQuery imq1 = Query.EQ("genTime", curTime.Date);
        IMongoQuery imq2 = Query.EQ("channel", channel);
        IMongoQuery imq = Query.And(imq1, imq2);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("h" + h, totalRecharge);

        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_RECHARGE_HOUR_BYCHANNEL, imq, data);
    }
}

//////////////////////////////////////////////////////////////////////////
// 统计每小时在线人数
class StatOnlinePlayerNumPerHour : StatByIntervalBase
{
    static int[] FISH_ROOMS = { 1, 2, 3, 4, 11, 20 };

    public override string getStatKey()
    {
        return StatKey.KEY_ONLINE_HOUR;
    }

    // 返回统计间隔(分钟)
    public override int getStatInterval()
    {
        return 60;
    }

    public override void update(double delta)
    {
        if (DateTime.Now < m_statTime)
            return;

        beginStat("StatOnlinePlayerNumPerHour开始统计");

        stat();

        addStatTime();

        endStat("StatOnlinePlayerNumPerHour结束统计");
    }

    void stat()
    {
        int online = 0;
        Dictionary<string, object> data = MongodbPlayer.Instance.ExecuteGetOneBykey(TableName.COMMON_CONFIG, "type", "cur_playercount");
        if (data != null)
        {
            online = Convert.ToInt32(data["value"]);
        }

        addData(online, m_statTime, 0, 0);

        for (int i = 1; i < StrName.s_onlineGameIdList.Length; i++)
        {
            if (StrName.s_onlineGameIdList[i] == (int)GameId.fishlord)
            {
                for (int k = 0; k < FISH_ROOMS.Length; k++)
                {
                    online = getOnlineNum(TableName.FISHLORD_ROOM, FISH_ROOMS[k]);
                    addData(online, m_statTime, StrName.s_onlineGameIdList[i], FISH_ROOMS[k]);
                }

                online = getOnlineNum(TableName.FISHLORD_ROOM);
                addData(online, m_statTime, StrName.s_onlineGameIdList[i]);
            }
            else
            {
                online = getOnlineNum(getTableName(StrName.s_onlineGameIdList[i]));
                addData(online, m_statTime, StrName.s_onlineGameIdList[i]);
            }
        }
    }

    void addData(int onlineNum, DateTime curTime, int gameId = 0, int roomId = 0)
    {
        int h = curTime.Hour;
        IMongoQuery imq1 = Query.EQ("genTime", curTime.Date);
        IMongoQuery imq2 = Query.EQ("gameId", gameId);
        IMongoQuery imq3 = Query.EQ("roomId", roomId);
        IMongoQuery imq = Query.And(imq1, imq2, imq3);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("h" + h, onlineNum);

        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_ONLINE_HOUR, imq, data);
    }

    int getOnlineNum(string table, int roomId = 0)
    {
        int count = 0;
        if (roomId == 0) // 获取全部房间的数据
        {
            List<Dictionary<string, object>> dataList =
                MongodbGame.Instance.ExecuteGetAll(table, new string[] { "player_count" });
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].ContainsKey("player_count"))
                {
                    count += Convert.ToInt32(dataList[i]["player_count"]);
                }
            }
        }
        else
        {
            Dictionary<string, object> data =
                MongodbGame.Instance.ExecuteGetOneBykey(table, "room_id", roomId, new string[] { "player_count" });
            if (data != null && data.ContainsKey("player_count"))
            {
                count = Convert.ToInt32(data["player_count"]);
            }
        }

        return count;
    }

    string getTableName(int gameId)
    {
        string name = "";
        switch (gameId)
        {
            case (int)GameId.crocodile:
                {
                    name = TableName.CROCODILE_ROOM;
                }
                break;
            case (int)GameId.cows:
                {
                    name = TableName.COWS_ROOM;
                }
                break;
            case (int)GameId.dragon:
                {
                    name = TableName.DRAGON_ROOM;
                }
                break;
            case (int)GameId.shcd:
                {
                    name = TableName.SHCDCARDS_ROOM;
                }
                break;
            case (int)GameId.shuihz:
                {
                    name = TableName.SHUIHZ_ROOM;
                }
                break;
            case (int)GameId.bz:
                {
                    name = TableName.DB_BZ_ROOM;
                }
                break;
            case (int)GameId.jewel:
                {
                    name = TableName.JEWEL_ROOM;
                }
                break;
            default:
                break;
        }
        return name;
    }
}

//////////////////////////////////////////////////////////////////////////
// 统计用户活跃--用户各游戏在线时长，及平均游戏时长分布
class StatGameTimeForPlayerActive : StatByDayBase
{
    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_GAME_TIME_FOR_PLAYER_ACTIVE;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddHours(3);
        if (DateTime.Now < st)
            return;

        beginStat("StatGameTimeForPlayerActive开始统计");

        DateTime time = m_statDay.Date.AddDays(-1);
        IMongoQuery imq = Query.EQ("genTime", time);

        // 各游戏平均在线时间
        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_GAME_TIME_FOR_PLAYER,
                                                                        imq,
                                                                        MapReduceTable.getMap("gameTimeForPlayerFavor"),
                                                                        MapReduceTable.getReduce("gameTimeForPlayerFavor"));
        if (mapResult != null)
        {
            GameTimeForPlayerFavorBase stat = new GameTimeForPlayerFavorBase();
            stat.m_time = time;
            IEnumerable<BsonDocument> bson = mapResult.GetResults();

            foreach (BsonDocument d in bson)
            {
                stat.reset();
                int playerType = Convert.ToInt32(d["_id"]); // 用户类型 1活跃用户  2付费用户
                BsonValue resValue = d["value"];
                stat.m_playerCount = resValue["playerCount"].ToInt32();

                var allGame = StrName.getAllUseGame();
                foreach(var dg in allGame)
                {
                    stat.addGameTime(playerType, dg.Key, resValue["game" + dg.Key].ToInt64());
                }

                addFavorData(playerType, stat);
            }
        }

        statDistributionData(imq, PlayerType.TYPE_ACTIVE, time);
        statDistributionData(imq, PlayerType.TYPE_RECHARGE, time);
        statDistributionData(imq, PlayerType.TYPE_NEW, time);

        addStatDay();

        endStat("StatGameTimeForPlayerActive结束统计");
    }

    void addFavorData(int playerType, GameTimeForPlayerFavorBase stat)
    {
        Dictionary<int, long> gtime = stat.getGameTime(playerType);
        if (gtime != null)
        {

            Dictionary<string, object> newData = new Dictionary<string, object>();
            newData.Add("genTime", stat.m_time);
            newData.Add("playerType", playerType);
            newData.Add("playerCount", stat.m_playerCount);

            foreach (var d in gtime)
            {
                newData.Add("game" + d.Key.ToString(), d.Value);
            }

            MongodbLog.Instance.ExecuteInsert(TableName.STAT_GAME_TIME_FOR_PLAYER_FAVOR_RESULT, newData);
        }
    }

    void statDistributionData(IMongoQuery imq, int playerType, DateTime time)
    {
        IMongoQuery cond = imq;
        switch (playerType)
        {
            case PlayerType.TYPE_ACTIVE:
                break;
            case PlayerType.TYPE_RECHARGE:
                {
                    cond = Query.And(cond, Query.EQ("isRecharge", true));
                }
                break;
            case PlayerType.TYPE_NEW:
                {
                    IMongoQuery imq1 = Query.LT("createTime", m_statDay);
                    IMongoQuery imq2 = Query.GTE("createTime", time);
                    cond = Query.And(cond, imq1, imq2);
                }
                break;
        }
        // 平均游戏时长分布统计
        MapReduceResult mapResult1 = MongodbLog.Instance.executeMapReduce(TableName.PUMP_GAME_TIME_FOR_PLAYER,
                                                                        cond,
                                                                        MapReduceTable.getMap("gameTimeDistribution"),
                                                                        MapReduceTable.getReduce("gameTimeDistribution"));
        if (mapResult1 != null)
        {
            GameTimeForDistributionBase stat = new GameTimeForDistributionBase();
            stat.m_time = time;
            IEnumerable<BsonDocument> bson = mapResult1.GetResults();

            foreach (BsonDocument d in bson)
            {
                stat.m_gameId = Convert.ToInt32(d["_id"]);
                BsonValue resValue = d["value"];
                stat.m_Less10s = resValue["Less10s"].ToInt32();
                stat.m_Less30s = resValue["Less30s"].ToInt32();
                stat.m_Less60s = resValue["Less60s"].ToInt32();
                stat.m_Less5min = resValue["Less5min"].ToInt32();
                stat.m_Less10min = resValue["Less10min"].ToInt32();
                stat.m_Less30min = resValue["Less30min"].ToInt32();
                stat.m_Less60min = resValue["Less60min"].ToInt32();
                stat.m_GT60min = resValue["GT60min"].ToInt32();

                addDistributionData(stat, playerType);
            }
        }
    }

    // 在线时长分布数据
    void addDistributionData(GameTimeForDistributionBase stat, int playerType)
    {
        Dictionary<string, object> newData = new Dictionary<string, object>();
        newData.Add("genTime", stat.m_time);
        newData.Add("playerType", playerType);
        newData.Add("gameId", stat.m_gameId);
        newData.Add("Less10s", stat.m_Less10s);
        newData.Add("Less30s", stat.m_Less30s);
        newData.Add("Less60s", stat.m_Less60s);
        newData.Add("Less5min", stat.m_Less5min);
        newData.Add("Less10min", stat.m_Less10min);
        newData.Add("Less30min", stat.m_Less30min);
        newData.Add("Less60min", stat.m_Less60min);
        newData.Add("GT60min", stat.m_GT60min);

        MongodbLog.Instance.ExecuteInsert(TableName.STAT_GAME_TIME_FOR_DISTRIBUTION_RESULT, newData);
    }
}

//////////////////////////////////////////////////////////////////////////
class GameRoomTime
{
    public long m_room1;
    public long m_room2;
    public long m_room3;
    public long m_room4;
    public long m_room5;
    public long m_room6;
    public long m_room7;
    public long m_room8;
    public long m_room9;
    public long m_room11;

    public int m_count1;
    public int m_count2;
    public int m_count3;
    public int m_count4;
    public int m_count5;
    public int m_count6;
    public int m_count7;
    public int m_count8;
    public int m_count9;
    public int m_count11;

    public string m_channel;
}

// 捕鱼各个房间的平均游戏时长
class StatFishGameTimeForRoom : StatByDayBase
{
    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_FISH_GAME_TIME_FOR_ROOM;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddHours(2);
        if (DateTime.Now < st)
            return;

        beginStat("StatFishGameTimeForRoom开始统计");

        DateTime time = m_statDay.Date.AddDays(-1);
        IMongoQuery imq = Query.EQ("genTime", time);

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_GAME_TIME_FOR_PLAYER,
                                                                        imq,
                                                                        MapReduceTable.getMap("StatFishGameTimeForRoom"),
                                                                        MapReduceTable.getReduce("StatFishGameTimeForRoom"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();

            foreach (BsonDocument d in bson)
            {
                GameRoomTime stat = new GameRoomTime();
                stat.m_channel = ToolHelper.channelToString(Convert.ToString(d["_id"]));
                BsonValue resValue = d["value"];
                stat.m_room1 = resValue["room1"].ToInt64();
                stat.m_room2 = resValue["room2"].ToInt64();
                stat.m_room3 = resValue["room3"].ToInt64();
                stat.m_room4 = resValue["room4"].ToInt64();
                stat.m_room5 = resValue["room5"].ToInt64();
                stat.m_room6 = resValue["room6"].ToInt64();
                stat.m_room7 = resValue["room7"].ToInt64();
                stat.m_room8 = resValue["room8"].ToInt64();
                stat.m_room9 = resValue["room9"].ToInt64();
                stat.m_room11 = resValue["room11"].ToInt64();

                stat.m_count1 = resValue["count1"].ToInt32();
                stat.m_count2 = resValue["count2"].ToInt32();
                stat.m_count3 = resValue["count3"].ToInt32();
                stat.m_count4 = resValue["count4"].ToInt32();
                stat.m_count5 = resValue["count5"].ToInt32();
                stat.m_count6 = resValue["count6"].ToInt32();
                stat.m_count7 = resValue["count7"].ToInt32();
                stat.m_count8 = resValue["count8"].ToInt32();
                stat.m_count9 = resValue["count9"].ToInt32();
                stat.m_count11 = resValue["count11"].ToInt32();

                addRoomTime(stat, time);
            }
        }

        addStatDay();

        endStat("StatFishGameTimeForRoom结束统计");
    }

    void addRoomTime(GameRoomTime stat, DateTime time)
    {
        IMongoQuery imq = Query.And(Query.EQ("genTime", time), Query.EQ("channel", stat.m_channel));
        Dictionary<string, object> data = new Dictionary<string, object>();
        // data.Add("genTime", time);
       // data.Add("channel", stat.m_channel);
        data.Add("aveTimeRoom1", stat.m_count1 > 0 ? stat.m_room1 / stat.m_count1 : 0);
        data.Add("aveTimeRoom2", stat.m_count2 > 0 ? stat.m_room2 / stat.m_count2 : 0);
        data.Add("aveTimeRoom3", stat.m_count3 > 0 ? stat.m_room3 / stat.m_count3 : 0);
        data.Add("aveTimeRoom4", stat.m_count4 > 0 ? stat.m_room4 / stat.m_count4 : 0);
        data.Add("aveTimeRoom5", stat.m_count5 > 0 ? stat.m_room5 / stat.m_count5 : 0);
        data.Add("aveTimeRoom6", stat.m_count6 > 0 ? stat.m_room6 / stat.m_count6 : 0);
        data.Add("aveTimeRoom7", stat.m_count7 > 0 ? stat.m_room7 / stat.m_count7 : 0);
        data.Add("aveTimeRoom8", stat.m_count8 > 0 ? stat.m_room8 / stat.m_count8 : 0);
        data.Add("aveTimeRoom9", stat.m_count9 > 0 ? stat.m_room9 / stat.m_count9 : 0);
        data.Add("aveTimeRoom11", stat.m_count11 > 0 ? stat.m_room11 / stat.m_count11 : 0);

        data.Add("personRoom1", stat.m_count1);
        data.Add("personRoom2", stat.m_count2);
        data.Add("personRoom3", stat.m_count3);
        data.Add("personRoom4", stat.m_count4);
        data.Add("personRoom5", stat.m_count5);
        data.Add("personRoom6", stat.m_count6);
        data.Add("personRoom7", stat.m_count7);
        data.Add("personRoom8", stat.m_count8);
        data.Add("personRoom9", stat.m_count9);
        data.Add("personRoom11", stat.m_count11);

        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_PLAYER_PLAY_TIME, imq, data);
    }
}

//////////////////////////////////////////////////////////////////////////
// 首付行为 -- 首付游戏时长分布, 首次购买计费点分布
class StatFirstRecharge : StatByDayBase
{
    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_FIRST_RECHARGE_DISTRIBUTION;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddHours(3);
        if (DateTime.Now < st)
            return;

        beginStat("StatFirstRecharge开始统计");

        DateTime startTime = m_statDay.Date.AddDays(-1);
        IMongoQuery imq1 = Query.LT("firstRechargeTime", m_statDay);
        IMongoQuery imq2 = Query.GTE("firstRechargeTime", startTime);
        IMongoQuery imq = Query.And(imq1, imq2);

        // 首付游戏时长分布
        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_RECHARGE_FIRST,
                                                                        imq,
                                                                        MapReduceTable.getMap("firstRechargeGameTimeDistribution"),
                                                                        MapReduceTable.getReduce("firstRechargeGameTimeDistribution"));
        if (mapResult != null)
        {
            FirstRechargeGameTimeDistributionBase stat = new FirstRechargeGameTimeDistributionBase();
            IEnumerable<BsonDocument> bson = mapResult.GetResults();

            foreach (BsonDocument d in bson)
            {
                int playerType = Convert.ToInt32(d["_id"]); // 用户类型 1活跃用户  2付费用户
                BsonValue resValue = d["value"];
                stat.m_Less1min = resValue["Less1min"].ToInt32();
                stat.m_Less10min = resValue["Less10min"].ToInt32();
                stat.m_Less30min = resValue["Less30min"].ToInt32();
                stat.m_Less60min = resValue["Less60min"].ToInt32();
                stat.m_Less3h = resValue["Less3h"].ToInt32();
                stat.m_Less5h = resValue["Less5h"].ToInt32();
                stat.m_Less12h = resValue["Less12h"].ToInt32();
                stat.m_Less24h = resValue["Less24h"].ToInt32();
                stat.m_GT24h = resValue["GT24h"].ToInt32();

                addGameGameTimeDistribution(playerType, stat, startTime);
            }
        }

        // 首次购买计费点分布
        MapReduceResult mapResult1 = MongodbLog.Instance.executeMapReduce(TableName.PUMP_RECHARGE_FIRST,
                                                                            imq,
                                                                            MapReduceTable.getMap("firstRechargePointDistribution"),
                                                                            MapReduceTable.getReduce("firstRechargePointDistribution"));
        if (mapResult1 != null)
        {
            FirstRechargePointDistribution stat = new FirstRechargePointDistribution();
            IEnumerable<BsonDocument> bson = mapResult1.GetResults();

            foreach (BsonDocument d in bson)
            {
                stat.reset();
                int playerType = Convert.ToInt32(d["_id"]); 
                BsonValue resValue = d["value"];
                BsonDocument bdoc = (BsonDocument)resValue;
                foreach (var elem in bdoc.Elements)
                {
                    int payPoint = Convert.ToInt32(elem.Name);
                    int playerCount = elem.Value.ToInt32();
                    stat.add(payPoint, playerCount);
                }
                addFirstRechargePointDistribution(playerType, stat, startTime);
            }
        }

        addStatDay();

        endStat("StatFirstRecharge结束统计");
    }

    void addGameGameTimeDistribution(int playerType, FirstRechargeGameTimeDistributionBase stat, DateTime curTime)
    {
        Dictionary<string, object> newData = new Dictionary<string, object>();
        newData.Add("genTime", curTime);
        newData.Add("playerType", playerType);
        newData.Add("Less1min", stat.m_Less1min);
        newData.Add("Less10min", stat.m_Less10min);
        newData.Add("Less30min", stat.m_Less30min);
        newData.Add("Less60min", stat.m_Less60min);
        newData.Add("Less3h", stat.m_Less3h);
        newData.Add("Less5h", stat.m_Less5h);
        newData.Add("Less12h", stat.m_Less12h);
        newData.Add("Less24h", stat.m_Less24h);
        newData.Add("GT24h", stat.m_GT24h);

        MongodbLog.Instance.ExecuteInsert(TableName.STAT_FIRST_RECHARGE_GAME_TIME_DISTRIBUTION_RESULT, newData);
    }

    void addFirstRechargePointDistribution(int playerType, FirstRechargePointDistribution stat, DateTime curTime)
    {
        Dictionary<string, object> newData = new Dictionary<string, object>();
        newData.Add("genTime", curTime);
        newData.Add("playerType", playerType);
        foreach (var d in stat.m_point)
        {
            newData.Add(d.Key.ToString(), d.Value);
        }

        MongodbLog.Instance.ExecuteInsert(TableName.STAT_FIRST_RECHARGE_POINT_DISTRIBUTION_RESULT, newData);
    }
}

//////////////////////////////////////////////////////////////////////////
// 用户下注情况统计
// 用户的游戏币携带量（查看每局携带量和最大携带量、最小携带量）、下注量（平均、最大、最小）、当日流水。
class StatPlayerGameBet : StatByDayBase
{
    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_PLAYER_GAME_BET;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddHours(3.5);
        if (DateTime.Now < st)
            return;

        beginStat("StatPlayerGameBet开始统计");

        DateTime startTime = m_statDay.Date.AddDays(-1);
        IMongoQuery imq1 = Query.LT("genTime", m_statDay);
        IMongoQuery imq2 = Query.GTE("genTime", startTime);
        IMongoQuery imq = Query.And(imq1, imq2, Query.EQ("reason", (int)PropertyReasonType.type_reason_single_round_balance));

        for (int i = 2; i < StrName.s_onlineGameIdList.Length; i++)
        {
            int gameId = StrName.s_onlineGameIdList[i];
            IMongoQuery tmpImq = Query.And(imq, Query.EQ("gameId", gameId));
            MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_PLAYER_MONEY,
                                                                       tmpImq,
                                                                       MapReduceTable.getMap("userGameBet"),
                                                                       MapReduceTable.getReduce("userGameBet"));
            if (mapResult != null)
            {
                IEnumerable<BsonDocument> bson = mapResult.GetResults();

                foreach (BsonDocument d in bson)
                {
                    int playerId = Convert.ToInt32(d["_id"]);
                    BsonValue resValue = d["value"];

                    try
                    {
                        stat(resValue["1"], gameId, playerId, startTime, 1);
                    }
                    catch (System.Exception ex)
                    {	
                    }
                    try
                    {
                        stat(resValue["14"], gameId, playerId, startTime, 14);
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
            }
        }
       
        addStatDay();

        endStat("StatPlayerGameBet结束统计");
    }

    void stat(BsonValue value, int gameId, int playerId, DateTime curTime, int itemId)
    {
        int round = value["round"].ToInt32();
        if (round == 0)
            return;

        Dictionary<string, object> newData = new Dictionary<string, object>();
        newData.Add("sumCarry", value["sumStart"].ToInt64());
        newData.Add("maxCarry", value["maxStart"].ToInt64());
        newData.Add("minCarry", value["minStart"].ToInt64());

        newData.Add("sumOutlay", value["sumOutlay"].ToInt64());
        newData.Add("maxOutlay", value["maxOutlay"].ToInt64());
        newData.Add("minOutlay", value["minOutlay"].ToInt64());

        newData.Add("sumWin", value["sumWin"].ToInt64());
        newData.Add("maxWin", value["maxWin"].ToInt64());
        newData.Add("minWin", value["minWin"].ToInt64());

        newData.Add("sumLose", value["sumLose"].ToInt64());
        newData.Add("maxLose", value["maxLose"].ToInt64());
        newData.Add("minLose", value["minLose"].ToInt64());

        newData.Add("round", round);

        IMongoQuery imq1 = Query.EQ("genTime", curTime);
        IMongoQuery imq2 = Query.EQ("playerId", playerId);
        IMongoQuery imq3 = Query.EQ("gameId", gameId);
        IMongoQuery imq4 = Query.EQ("itemId", itemId);
        IMongoQuery imq = Query.And(imq1, imq2, imq3, imq4);

        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_PLAYER_GAME_BET_RESULT, imq, newData);
    }
}

//////////////////////////////////////////////////////////////////////////
class StatPersonTimeGlobalDay : StatByDayBase
{
    DateTime m_last = DateTime.Now;

    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_PERSON_TIME_GLOBAL_DAY;
    }

    public override void update(double delta)
    {
        TimeSpan span = DateTime.Now - m_last;
        if (span.TotalMinutes >= 10)
        {
            stat(false);
            m_last = DateTime.Now;
        }

        if (DateTime.Now.Date >= m_statDay)
        {
            stat(true);
            addStatDay();
        }       
    }

    void stat(bool delToday)
    {
        beginStat("StatPersonTimeGlobalDay开始统计");

        DateTime startTime = m_statDay.Date.AddDays(-1);
        IMongoQuery imq1 = Query.LT("date", m_statDay);
        IMongoQuery imq2 = Query.GTE("date", startTime);

        for (int i = 1; i <= 4; i++)
        {
            IMongoQuery imq = Query.And(imq1, imq2, Query.EQ("roomId", i));

            MapReduceResult mapResult = MongodbGame.Instance.executeMapReduce(TableName.PERSONTIME_GLOBAL_DAY,
                                                                       imq,
                                                                       MapReduceTable.getMap("personTimeGlobalDay"),
                                                                       MapReduceTable.getReduce("personTimeGlobalDay"));
            if (mapResult != null)
            {
                IEnumerable<BsonDocument> bson = mapResult.GetResults();

                foreach (BsonDocument d in bson)
                {
                    int fishId = Convert.ToInt32(d["_id"]);
                    BsonValue resValue = d["value"];
                    int count = resValue["count"].ToInt32();
                    updateData(startTime, i, fishId, count);
                }
            }
        }

        if (delToday)
        {
            IMongoQuery imq = Query.And(imq1, imq2);
            MongodbGame.Instance.ExecuteRemoveByQuery(TableName.PERSONTIME_GLOBAL_DAY, imq);
        }

        endStat("StatPersonTimeGlobalDay结束统计");
    }

    void updateData(DateTime date, int roomId, int fishId, int count)
    {
        IMongoQuery imq1 = Query.EQ("date", date);
        IMongoQuery imq2 = Query.EQ("roomid", roomId);
        IMongoQuery imq3 = Query.EQ("bossId", fishId);
        IMongoQuery imq = Query.And(imq1, imq2, imq3);

        Dictionary<string, object> newData = new Dictionary<string, object>();
        newData.Add("bossHitPersonTimeGlobalDay", count);

        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.PUMP_BOSSINFO, imq, newData);
    }
}

//////////////////////////////////////////////////////////////////////////
// 抽彩券的统计
class StatStarLottery : StatByDayBase
{
    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_STAR_LOTTERY;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddHours(1);

        if (DateTime.Now >= st)
        {
            stat();
            addStatDay();
        }
    }

    void stat()
    {
        beginStat("StatStarLottery开始统计");

        DateTime startTime = m_statDay.Date.AddDays(-1);
        IMongoQuery imq1 = Query.LT("genTime", m_statDay);
        IMongoQuery imq2 = Query.GTE("genTime", startTime);

        IMongoQuery imq = Query.And(imq1, imq2);

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_STAR_LOTTERY2,
                                                                    imq,
                                                                    MapReduceTable.getMap("StatStarLottery"),
                                                                    MapReduceTable.getReduce("StatStarLottery"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            Dictionary<string, object> sd = new Dictionary<string, object>();

            foreach (BsonDocument d in bson)
            {
                sd.Clear();
                int level = Convert.ToInt32(d["_id"]);

                BsonValue resValue = d["value"];
                long totalOutlay = resValue["totalOutlay"].ToInt64();
                long gold = resValue["gold"].ToInt64();
                long gem = resValue["gem"].ToInt64();
                long db = resValue["db"].ToInt64();
                long chip = resValue["chip"].ToInt64();

                sd.Add("totalOutlay", totalOutlay);
                sd.Add("gold", gold);
                sd.Add("gem", gem);
                sd.Add("db", db);
                sd.Add("chip", chip);
                updateData(startTime, sd, level);
            }
        }

        endStat("StatStarLottery结束统计");
    }

    void updateData(DateTime date, Dictionary<string, object> data, int level)
    {
        IMongoQuery imq1 = Query.EQ("genTime", date);
        IMongoQuery imq2 = Query.EQ("level", level);
        IMongoQuery imq = Query.And(imq1, imq2);

        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_STAR_LOTTERY2, imq, data);
    }
}

//////////////////////////////////////////////////////////////////////////
// 抽彩券的统计
class StatStarLotteryDetail : StatByDayBase
{
    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_STAR_LOTTERY_DETAIL;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddHours(1);

        if (DateTime.Now >= st)
        {
            stat();
            addStatDay();
        }
    }

    void stat()
    {
        beginStat("StatStarLotteryDetail开始统计");

        DateTime startTime = m_statDay.Date.AddDays(-1);
        IMongoQuery imq1 = Query.LT("genTime", m_statDay);
        IMongoQuery imq2 = Query.GTE("genTime", startTime);

        IMongoQuery imq = Query.And(imq1, imq2);

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_STAR_LOTTERY2,
                                                                    imq,
                                                                    MapReduceTable.getMap("StatStarLotteryDetail"),
                                                                    MapReduceTable.getReduce("StatStarLotteryDetail"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            Dictionary<string, object> sd = new Dictionary<string, object>();

            foreach (BsonDocument d in bson)
            {
                sd.Clear();
                // level从1开始
                int level = Convert.ToInt32(d["_id"]);

                BsonValue resValue = d["value"];
                long totalOutlay = resValue["totalOutlay"].ToInt64();
                long correspondingGold = resValue["correspondingGold"].ToInt64();
                int lotteryCount = resValue["lotteryCount"].ToInt32();
                int personCount = getPersonCount(imq, level);

                sd.Add("totalOutlay", totalOutlay);
                sd.Add("correspondingGold", correspondingGold);
                sd.Add("lotteryCount", lotteryCount);
                sd.Add("personCount", personCount);

                string str = "";
                for (int i = 0; i < 6; i++)
                {
                    str = "index" + i.ToString();
                    sd.Add(str, resValue[str].ToInt32());
                }

                updateData(startTime, sd, level);
            }
        }

        endStat("StatStarLotteryDetail结束统计");
    }

    void updateData(DateTime date, Dictionary<string, object> data, int level)
    {
        IMongoQuery imq1 = Query.EQ("genTime", date);
        IMongoQuery imq2 = Query.EQ("level", level);
        IMongoQuery imq = Query.And(imq1, imq2);

        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_STAR_LOTTERY_DETAIL, imq, data);
    }

    // 返回人次
    int getPersonCount(IMongoQuery imq, int level)
    {
        int count = 0;
        IMongoQuery tmp = Query.And(imq, Query.EQ("starlvl", level));

        count = MongodbLog.Instance.ExecuteDistinct(TableName.PUMP_STAR_LOTTERY2, "playerid", tmp);
        return count;
    }
}

//////////////////////////////////////////////////////////////////////////
// 拉霸抽奖
class StatLabaLottery : StatByDayBase
{
    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_LABA_LOTTERY;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddMinutes(10);
        if (DateTime.Now >= st)
        {
            stat();
            addStatDay();
        }
    }

    void stat()
    {
        beginStat("StatLabaLottery开始统计");

        DateTime startTime = m_statDay.Date.AddDays(-1);
        IMongoQuery imq1 = Query.LT("genTime", m_statDay);
        IMongoQuery imq2 = Query.GTE("genTime", startTime);

        IMongoQuery imq = Query.And(imq1, imq2);

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_LABA_LOTTERY,
                                                                    imq,
                                                                    MapReduceTable.getMap("StatLabaPlayer"),
                                                                    MapReduceTable.getReduce("StatLabaPlayer"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            Dictionary<string, object> sd = new Dictionary<string, object>();

            int LaPlayerCount = 0;
            int LaLotterCount = 0;
            
            foreach (BsonDocument d in bson)
            {
                sd.Clear();
                int playerId = Convert.ToInt32(d["_id"]);

                BsonValue resValue = d["value"];
                int lotteryCount = resValue["count"].ToInt32();
               
                sd.Add("lotteryCount", lotteryCount);
                updateDataPlayer(startTime, sd, playerId);

                LaPlayerCount++;
                LaLotterCount += lotteryCount;
            }

            updateDataTotal(startTime, LaPlayerCount, LaLotterCount);
        }
        else
        {
            updateDataTotal(startTime, 0, 0);
        }

        //////////////////////////////////////////////////////////////////////////
        MapReduceResult mapResult1 = MongodbLog.Instance.executeMapReduce(TableName.PUMP_LABA_LOTTERY,
                                                                    imq,
                                                                    MapReduceTable.getMap("StatLabaProb"),
                                                                    MapReduceTable.getReduce("StatLabaProb"));
        if (mapResult1 != null)
        {
            IEnumerable<BsonDocument> bson = mapResult1.GetResults();
            Dictionary<string, object> sd = new Dictionary<string, object>();

            foreach (BsonDocument d in bson)
            {
                sd.Clear();
                int resultId = Convert.ToInt32(d["_id"]);

                BsonValue resValue = d["value"];
                int appearCount = resValue["count"].ToInt32();

                sd.Add("appearCount", appearCount);
                updateDataProb(startTime, sd, resultId);
            }
        }

        endStat("StatLabaLottery结束统计");
    }

    void updateDataPlayer(DateTime date, Dictionary<string, object> data, int playerId)
    {
        IMongoQuery imq1 = Query.EQ("genTime", date);
        IMongoQuery imq2 = Query.EQ("playerId", playerId);
        IMongoQuery imq = Query.And(imq1, imq2);

        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_LABA_LOTTERY_PLAYER, imq, data);
    }

    void updateDataProb(DateTime date, Dictionary<string, object> data, int resultId)
    {
        IMongoQuery imq1 = Query.EQ("genTime", date);
        IMongoQuery imq2 = Query.EQ("resultId", resultId);
        IMongoQuery imq = Query.And(imq1, imq2);

        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_LABA_LOTTERY_PROB, imq, data);
    }

    // 拉霸整体数据
    void updateDataTotal(DateTime date, int LaPlayerCount, int LaLotterCount)
    {
        IMongoQuery imq = Query.EQ("genTime", date);
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("LaPlayerCount", LaPlayerCount);
        data.Add("LaLotteryCount", LaLotterCount);

        Dictionary<string, object> today = MongodbLog.Instance.ExecuteGetByQuery(TableName.STAT_LABA_LOTTERY_TOTAL, imq);
        if (today != null)
        {
            MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_LABA_LOTTERY_TOTAL, imq, data);

            if (!today.ContainsKey("actId"))
                return;

            int totalGainCount = 0;
            int totalLaLotteryCount = 0;
            List<Dictionary<string, object>> dataList = MongodbLog.Instance.ExecuteGetListBykey(TableName.STAT_LABA_LOTTERY_TOTAL, "actId", today["actId"]);
            foreach (var tmp in dataList)
            {
                if (tmp.ContainsKey("GainLaCount"))
                {
                    totalGainCount += Convert.ToInt32(tmp["GainLaCount"]);
                }
                if (tmp.ContainsKey("LaLotteryCount"))
                {
                    totalLaLotteryCount += Convert.ToInt32(tmp["LaLotteryCount"]);
                }
            }

            data.Clear();
            data.Add("RemainLotteryCount", totalGainCount - totalLaLotteryCount);
            MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_LABA_LOTTERY_TOTAL, imq, data);
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 玩偶统计
class StatPuppet : StatByDayBase
{
    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_PUPPET;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddMinutes(10);
        if (DateTime.Now >= st)
        {
            stat();
            addStatDay();
        }
    }

    void stat()
    {
        beginStat("StatPuppet开始统计");

        DateTime startTime = m_statDay.Date.AddDays(-1);
        IMongoQuery imq1 = Query.LT("genTime", m_statDay);
        IMongoQuery imq2 = Query.GTE("genTime", startTime);
        IMongoQuery imq = Query.And(imq1, imq2);

        statPersonCount(imq, startTime);

        endStat("StatPuppet结束统计");
    }

    void statPersonCount(IMongoQuery imqIn, DateTime startTime)
    {
        int count = 0;
        count = MongodbLog.Instance.ExecuteDistinct(TableName.PUMP_DONATE_PUPPET_REC, "playerId", imqIn);
        if (count == 0)
            return;

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("donatePersonCount", count);
        IMongoQuery up = Query.EQ("genTime", startTime);
        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_PUPPET_SVR_DONATE, up, data);   
    }
}

//////////////////////////////////////////////////////////////////////////
// 爆金场统计
class StatBaoJin : StatByDayBase
{
    Dictionary<string, object> m_dic = new Dictionary<string, object>();

    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_BAOJIN;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddMinutes(10);
        if (DateTime.Now >= st)
        {
            stat();
            addStatDay();
        }
    }

    void stat()
    {
        beginStat("StatBaoJin开始统计");
       
        DateTime startTime = m_statDay.Date.AddDays(-1);
        IMongoQuery imq1 = Query.LT("genTime", m_statDay);
        IMongoQuery imq2 = Query.GTE("genTime", startTime);
        IMongoQuery imq = Query.And(imq1, imq2);

        for (int roomId = 10; roomId <= 11; roomId++)
        {
            m_dic.Clear();
            statTotal(roomId, imq);
            statMatchPersonCount(roomId, imq);
            saveData(startTime, roomId, m_dic);
        }

        endStat("StatBaoJin结束统计");
    }

    void statTotal(int roomId, IMongoQuery imqIn)
    {
        IMongoQuery imq = Query.And(imqIn, Query.EQ("roomId", roomId));

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_BAOJIN_PLAYER,
                                                                    imq,
                                                                    MapReduceTable.getMap("StatBaoJinTotal"),
                                                                    MapReduceTable.getReduce("StatBaoJinTotal"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                BsonValue resValue = d["value"];
                int count = resValue["joinCount"].ToInt32();
                m_dic.Add("joinCount", count);

                count = resValue["giveoutGold"].ToInt32();
                m_dic.Add("giveoutGold", count);

                count = resValue["matchTime"].ToInt32();
                m_dic.Add("matchTime", count);

                count = resValue["personCount"].ToInt32();
                m_dic.Add("personCount", count);

                count = resValue["winCount"].ToInt32();
                m_dic.Add("winCount", count);

                for (int i = 1; i < 10; i++)
                {
                    string key = "baoji_" + i.ToString();
                    m_dic.Add(key, resValue[key].ToInt32());
                }
            }
        }
    }

    // 统计人数
    void statMatchPersonCount(int roomId, IMongoQuery imqIn)
    {
        IMongoQuery imq = Query.And(imqIn, Query.EQ("roomId", roomId));

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_BAOJIN_PLAYER,
                                                                    imq,
                                                                    MapReduceTable.getMap("StatBaoJinMatchPersonCount"),
                                                                    MapReduceTable.getReduce("StatBaoJinMatchPersonCount"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                int matchCount = Convert.ToInt32(d["_id"]);
                BsonValue resValue = d["value"];
                int personCount = resValue["playerCount"].ToInt32();

                // 参与personCount次比赛的人数
                string key = "matchPerson_" + matchCount.ToString();
                m_dic.Add(key, personCount);
            }
        }
    }

    void saveData(DateTime startTime, int roomId, Dictionary<string, object> data)
    {
        IMongoQuery imq1 = Query.EQ("genTime", startTime);
        IMongoQuery imq2 = Query.EQ("roomId", roomId);
        IMongoQuery imq = Query.And(imq1, imq2);

        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_BAOJIN_PLAYER, imq, data);
    }
}

//////////////////////////////////////////////////////////////////////////
class StatRechargePoint : StatByDayBase
{
    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_RECHARGE_POINT;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddHours(1);
        if (DateTime.Now >= st)
        {
            stat();
            addStatDay();
        }
    }

    void stat()
    {
        beginStat("StatRechargePoint开始统计");

        DateTime startTime = m_statDay.Date.AddDays(-1);
        IMongoQuery imq1 = Query.LT("CreateTime", m_statDay);
        IMongoQuery imq2 = Query.GTE("CreateTime", startTime);
        IMongoQuery imq = Query.And(imq1, imq2);

        statTotal(imq, startTime);

        endStat("StatRechargePoint结束统计");
    }

    void statTotal(IMongoQuery imqIn, DateTime startTime)
    {
        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

        MapReduceResult mapResult = MongodbPayment.Instance.executeMapReduce(TableName.RECHARGE_TOTAL,
                                                                    imqIn,
                                                                    MapReduceTable.getMap("StatRechargePoint"),
                                                                    MapReduceTable.getReduce("StatRechargePoint"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                int payCode = Convert.ToInt32(d["_id"]);
                BsonValue resValue = d["value"];
                int rechargeCount = resValue["rechargeCount"].ToInt32();
                int rechargeSum = resValue["rmbSum"].ToInt32();

                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("genTime", startTime);
                dic.Add("payCode", payCode);
                dic.Add("rechargeCount", rechargeCount);
                dic.Add("rechargeSum", rechargeSum);
                dataList.Add(dic);
            }
        }

        if (dataList.Count > 0)
        {
            List<BsonDocument> docList = new List<BsonDocument>();
            for (int i = 0; i < dataList.Count; i++)
            {
                docList.Add(new BsonDocument(dataList[i]));
            }

            MongodbLog.Instance.ExecuteInsterList(TableName.RECHARGE_DISTRIBUTION, docList);
        }
    }
}

//////////////////////////////////////////////////////////////////////////
class StatOldPlayerLogin : StatByDayBase
{
    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_OLD_PLAYER_LOGIN;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddHours(1);
        if (DateTime.Now >= st)
        {
            stat();
            addStatDay();
        }
    }

    void stat()
    {
        beginStat("StatOldPlayerLogin开始统计");

        DateTime startTime = m_statDay.Date.AddDays(-1);
        IMongoQuery imq1 = Query.LT("genTime", m_statDay);
        IMongoQuery imq2 = Query.GTE("genTime", startTime);
        IMongoQuery imq = Query.And(imq1, imq2);

        statTotal(imq, startTime);

        endStat("StatOldPlayerLogin结束统计");
    }

    void statTotal(IMongoQuery imqIn, DateTime startTime)
    {
        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_OLD_PLAYER_LOGIN,
                                                                    imqIn,
                                                                    MapReduceTable.getMap("StatOldPlayerLogin"),
                                                                    MapReduceTable.getReduce("StatOldPlayerLogin"));
        if (mapResult != null)
        {
            Dictionary<string, int> channelData = new Dictionary<string, int>();
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                string channelStr = Convert.ToString(d["_id"]);
                string[] arr = channelStr.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                string channel = arr[1];

                if (channelData.ContainsKey(channel))
                {
                    channelData[channel] = channelData[channel] + 1;
                }
                else
                {
                    channelData[channel] = 1;
                }
            }

            foreach (var da in channelData)
            {
                IMongoQuery tmpImq = Query.And(Query.EQ("genTime", startTime), Query.EQ("channel", da.Key));
                Dictionary<string, object> updata = new Dictionary<string, object>();
                updata.Add("oldPlayerLogin", da.Value);
                MongodbLog.Instance.ExecuteUpdateByQuery(TableName.PUMP_SIGN, tmpImq, updata);
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////
class StatSign : StatByDayBase
{
    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_SIGN;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddMinutes(3);
        if (DateTime.Now >= st)
        {
            stat();
            addStatDay();
        }
    }

    void stat()
    {
        beginStat("StatSign开始统计");

        DateTime startTime = m_statDay.Date.AddDays(-1);
        int month = startTime.Year * 100 + startTime.Month;
        IMongoQuery imqUp = Query.EQ("month", month);

        statTotal(imqUp, startTime);

        endStat("StatSign结束统计");
    }

    void statTotal(IMongoQuery imqIn, DateTime startTime)
    {
        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce("pumpMonthSign",
                                                                    imqIn,
                                                                    MapReduceTable.getMap("StatSign"),
                                                                    MapReduceTable.getReduce("StatSign"));
        if (mapResult != null)
        {
            Dictionary<string, object> updata = new Dictionary<string, object>();
            for (int i = 1; i <= 31; i++)
            {
                updata.Add(i.ToString(), 0);  // 签到次数->人数
            }
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                int signCount = Convert.ToInt32(d["_id"]);
                BsonValue resValue = d["value"];
                int count = resValue["count"].ToInt32();
                updata[signCount.ToString()] = count;
            }

            MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_SIGN_PLAYER, imqIn, updata);
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 爱贝的切支付统计， 每个渠道通过爱贝充值了多少
class StatRechargeAibei : StatByDayBase
{
    DateTime m_lastTime = DateTime.MinValue;

    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_AIBEI;
    }

    public override void update(double delta)
    {
        DateTime now = DateTime.Now;
        TimeSpan span = now - m_lastTime;
        if (span.TotalMinutes >= 30)
        {
            m_lastTime = now;
            stat();
        }

        DateTime st = m_statDay.AddMinutes(3);
        if (now >= st)
        {
            stat();
            addStatDay();
        }
    }

    void stat()
    {
        beginStat("StatRechargeAibei 开始统计");

        DateTime startTime = m_statDay.Date.AddDays(-1);
        IMongoQuery imq1 = Query.LT("PayTime", m_statDay);
        IMongoQuery imq2 = Query.GTE("PayTime", startTime);
        IMongoQuery imq = Query.And(imq1, imq2);

        statTotal(imq, startTime);

        endStat("StatRechargeAibei 结束统计");
    }

    void statTotal(IMongoQuery imq, DateTime startTime)
    {
        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

        MapReduceResult mapResult = MongodbPayment.Instance.executeMapReduce("aibei_pay",
                                                                    imq,
                                                                    MapReduceTable.getMap("StatRechargeAibei"),
                                                                    MapReduceTable.getReduce("StatRechargeAibei"));
        if (mapResult != null)
        {
            List<Dictionary<string, object>> upList = new List<Dictionary<string, object>>();

            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                string channel = Convert.ToString(d["_id"]);
                BsonValue resValue = d["value"];
                int recharge = resValue["total"].ToInt32();

                Dictionary<string, object> data = new Dictionary<string, object>();
                //data.Add("genTime", startTime);
                //data.Add("channel", channel);
                data.Add("recharge", recharge);

                IMongoQuery upimq = Query.And(Query.EQ("genTime", startTime), Query.EQ("channel", channel));
                MongodbAccount.Instance.ExecuteUpdateByQuery(TableName.CHANNEL_RECHARGE_AIBEI, upimq, data);
            }          
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 统计每分钟在线人数，通过rrdtool来统计，并绘图
class StatOnlinePlayerNumRRd : StatByIntervalBase
{
    static int[] FISH_ROOMS = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 15, 20 };
    OnlineMgr m_mgr = new OnlineMgr();
    
    public StatOnlinePlayerNumRRd()
    {
        XmlConfig xml = ResMgr.getInstance().getRes("dbserver.xml");
        m_mgr.RRDToolPath = xml.getString("rrdPath", "");
        m_mgr.init();
        //NHAWK.NHawkCommand.Instance.RunCommand("graph C:\\project\\dwc\\trunk\\Http\\WebManagerDWC_Fish3\\WebManager\\bin\\..\\excel\\GM_admin\\0_0_2017-11-08-00-00-00_2017-11-09-00-00-00.png --start 1510070400 --end 1510156800  --vertical-label \"onlinenum\" --upper-limit 非数字 --lower-limit 非数字 --width 1280 --height 800 DEF:myonline=dbTotal.rrd:online:AVERAGE LINE1:myonline#FF0000:\"online player count\"");
    }

    public override string getStatKey()
    {
        return StatKey.KEY_ONLINE_HOUR_RRD;
    }

    // 返回统计间隔(分钟)
    public override int getStatInterval()
    {
        return 1;
    }

    public override void update(double delta)
    {
        if (DateTime.Now < m_statTime)
            return;

        stat();

        addStatTime();
    }

    void stat()
    {
        int online = 0;
        Dictionary<string, object> data = MongodbPlayer.Instance.ExecuteGetOneBykey(TableName.COMMON_CONFIG, "type", "cur_playercount");
        if (data != null)
        {
            online = Convert.ToInt32(data["value"]);
        }

        addData(online, m_statTime, 0, 0);

        for (int i = 1; i < StrName.s_onlineGameIdList.Length; i++)
        {
            if (StrName.s_onlineGameIdList[i] == (int)GameId.fishlord)
            {
                for (int k = 0; k < FISH_ROOMS.Length; k++)
                {
                    online = getOnlineNum(TableName.FISHLORD_ROOM, FISH_ROOMS[k]);
                    addData(online, m_statTime, StrName.s_onlineGameIdList[i], FISH_ROOMS[k]);
                }

                online = getOnlineNum(TableName.FISHLORD_ROOM);
                addData(online, m_statTime, StrName.s_onlineGameIdList[i]);
            }
            else
            {
                online = getOnlineNum(getTableName(StrName.s_onlineGameIdList[i]));
                addData(online, m_statTime, StrName.s_onlineGameIdList[i]);
            }
        }
    }

    void addData(int onlineNum, DateTime curTime, int gameId = 0, int roomId = 0)
    {
        //onlineNum = new Random().Next(1, 1000);
        var rrd = m_mgr.getRRd(gameId, roomId);
        if (rrd != null)
        {
            rrd.insertData(curTime, onlineNum);
        }
    }

    int getOnlineNum(string table, int roomId = 0)
    {
        int count = 0;
        if (roomId == 0) // 获取全部房间的数据
        {
            List<Dictionary<string, object>> dataList =
                MongodbGame.Instance.ExecuteGetAll(table, new string[] { "player_count" });
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].ContainsKey("player_count"))
                {
                    count += Convert.ToInt32(dataList[i]["player_count"]);
                }
            }
        }
        else
        {
            Dictionary<string, object> data =
                MongodbGame.Instance.ExecuteGetOneBykey(table, "room_id", roomId, new string[] { "player_count" });
            if (data != null && data.ContainsKey("player_count"))
            {
                count = Convert.ToInt32(data["player_count"]);
            }
        }

        return count;
    }

    string getTableName(int gameId)
    {
        string name = "";
        switch (gameId)
        {
            case (int)GameId.crocodile:
                {
                    name = TableName.CROCODILE_ROOM;
                }
                break;
            case (int)GameId.cows:
                {
                    name = TableName.COWS_ROOM;
                }
                break;
            case (int)GameId.dragon:
                {
                    name = TableName.DRAGON_ROOM;
                }
                break;
            case (int)GameId.shcd:
                {
                    name = TableName.SHCDCARDS_ROOM;
                }
                break;
            case (int)GameId.shuihz:
                {
                    name = TableName.SHUIHZ_ROOM;
                }
                break;
            case (int)GameId.bz:
                {
                    name = TableName.DB_BZ_ROOM;
                }
                break;
            case (int)GameId.fruit:
                {
                    name = TableName.FRUIT_ROOM;
                }
                break;
            case (int)GameId.jewel:
                {
                    name = TableName.JEWEL_ROOM;
                }
                break;
            default:
                break;
        }
        return name;
    }
}

//////////////////////////////////////////////////////////////////////////
// 竞技场参与统计
class StatBaojinJoinDistribute : StatByDayBase
{
    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_BAOJIN_JOIN_DISTRIBUTE;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddMinutes(2);
        if (DateTime.Now >= st)
        {
            stat();
            addStatDay();
        }
    }

    void stat()
    {
        beginStat("StatBaojinJoinCount 开始统计");

        DateTime startTime = m_statDay.Date.AddDays(-1);
        IMongoQuery imq1 = Query.LT("genTime", m_statDay);
        IMongoQuery imq2 = Query.GTE("genTime", startTime);
        IMongoQuery imq = Query.And(imq1, imq2);

        statTotal(imq, startTime);
        statJoinCount(imq, startTime);

        endStat("StatBaojinJoinCount 结束统计");
    }

    void statTotal(IMongoQuery imq, DateTime startTime)
    {
        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce("pumpFishBaojinJoin",
                                                                    imq,
                                                                    MapReduceTable.getMap("StatBaoJinJoinDistribute"),
                                                                    MapReduceTable.getReduce("StatBaoJinJoinDistribute"));
        if (mapResult != null)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                int gameLevel = Convert.ToInt32(d["_id"]);
                BsonValue resValue = d["value"];
                int count = resValue["count"].ToInt32();

                data.Add("Level_" + gameLevel.ToString(), count);
            }

            IMongoQuery upimq = Query.And(Query.EQ("genTime", startTime));
            MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_BAOJIN_JOIN_DISTRIBUTE, upimq, data);
        }
    }

    void statJoinCount(IMongoQuery imq, DateTime startTime)
    {
        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce("pumpFishBaojinJoin",
                                                                    imq,
                                                                    MapReduceTable.getMap("StatBaoJinJoinCount"),
                                                                    MapReduceTable.getReduce("StatBaoJinJoinCount"));
        if (mapResult != null)
        {
            int person = 0, count = 0;
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                person++;
                BsonValue resValue = d["value"];
                count += resValue["count"].ToInt32();
            }

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("joinPerson", person);
            data.Add("joinCount", count);
            IMongoQuery upimq = Query.And(Query.EQ("genTime", startTime));
            MongodbLog.Instance.ExecuteUpdateByQuery("pumpFishBaojinIncomeOutlay", upimq, data);
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 新春礼包
class StatNYGift : StatByIntervalBase
{
    public override string getStatKey()
    {
        return StatKey.KEY_NY_GIFT;
    }

    // 返回统计间隔(分钟)
    public override int getStatInterval()
    {
        return 30;
    }

    public override void update(double delta)
    {
        if (DateTime.Now < m_statTime)
            return;

        beginStat("StatNYGift开始统计");
        stat();
        addStatTime();
        endStat("StatNYGift结束统计");
    }

    void stat()
    {
        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_NY_GIFT,
                                                                        null,
                                                                        MapReduceTable.getMap("StatNYGift"),
                                                                        MapReduceTable.getReduce("StatNYGift"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                int giftId = Convert.ToInt32(d["_id"]);
                BsonValue resValue = d["value"];
                
                Dictionary<string, object> data = new Dictionary<string, object>();

                for (int i = 1; i <= 12; i++)
                {
                    data.Add("count_" + i, resValue["count_" + i].ToInt32());
                }

                addData(giftId, data);
            }
        }
    }

    void addData(int giftId, Dictionary<string, object> data)
    {
        MongodbLog.Instance.ExecuteUpdate(TableName.STAT_NY_GIFT, "giftId", giftId, data);
    }
}

//////////////////////////////////////////////////////////////////////////
// 五一充值，抽奖
class StatWuyiRechargeLottery : StatByDayBase
{
    public override string getStatKey()
    {
        return StatKey.KEY_WUYI_RECHARGE;
    }

    public override void update(double delta)
    {
        if (DateTime.Now < m_statDay.AddMinutes(3))
            return;

        beginStat("StatWuyiRechargeLottery开始统计");
        stat();
        addStatDay();
        endStat("StatWuyiRechargeLottery结束统计");
    }

    void stat()
    {
        DateTime startTime = m_statDay.Date.AddDays(-1);
        IMongoQuery imq1 = Query.LT("genTime", m_statDay);
        IMongoQuery imq2 = Query.GTE("genTime", startTime);
        IMongoQuery imq = Query.And(imq1, imq2);

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_WUYI_JOIN,
                                                                        imq,
                                                                        MapReduceTable.getMap("StatWuyiRechargeLottery"),
                                                                        MapReduceTable.getReduce("StatWuyiRechargeLottery"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                BsonValue resValue = d["value"];

                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("genTime", startTime);
                data.Add("joinCount", resValue["joinCount"].ToInt32());
                data.Add("joinPerson", resValue["joinPerson"].ToInt32());
                MongodbLog.Instance.ExecuteInsert(TableName.STAT_WUYI_JOIN, data);
            }
        }
    }
}

// 统计世界杯某场比赛押注人数(主场，客场，平局 各的押注人数)
class StatWorldCupMatchBetPerson : StatByDayBase
{
    const string WORLD_CUP_ACTIVITY_PLAYER = "worldCupActivityPlayer";
    enum Area
    {
        homeTeam = 0,   // 主
		visitTeam = 1,  // 客
		draw = 2,       // 平局
    }
    private int m_curMatchId = 1;
    private WorldCupScheduleData m_data;

    public override void init()
    {
        readParam();
        m_data = WorldCupSchedule.getInstance().getValue(m_curMatchId);
    }

    public override string getStatKey()
    {
        return "";
    }

    public override void update(double delta)
    {
        if (m_data == null) return;

        if (DateTime.Now < m_data.m_matchTime.AddHours(2))
            return;

        beginStat("StatWorldCupMatchBetPerson开始统计");
        
        stat();

        m_curMatchId++;
        saveParam();
        m_data = WorldCupSchedule.getInstance().getValue(m_curMatchId);
        endStat("StatWorldCupMatchBetPerson结束统计");
    }

    private IMongoQuery genCond(Area area)
    {
        string field = "";
        if (area == Area.homeTeam) // 主
        {
            field = "homeBet";
        }
        else if (area == Area.visitTeam) // 客
        {
            field = "visitBet";
        }
        else if (area == Area.draw) // 平局
        {
            field = "drawBet";
        }

        IMongoQuery imq1 = Query.EQ("hasBetMatchIds", m_curMatchId);
        IMongoQuery imq2 = Query.EQ("matchId", m_curMatchId);
        IMongoQuery imq3 = Query.GT(field, 0);
        IMongoQuery imq4 = Query.ElemMatch("betList", Query.And(imq2, imq3));
        return Query.And(imq1, imq4);
    }

    void stat()
    {
        IMongoQuery imq1 = genCond(Area.homeTeam);
        int count1 = (int)MongodbPlayer.Instance.ExecuteGetCount(WORLD_CUP_ACTIVITY_PLAYER, imq1);

        IMongoQuery imq2 = genCond(Area.visitTeam);
        int count2 = (int)MongodbPlayer.Instance.ExecuteGetCount(WORLD_CUP_ACTIVITY_PLAYER, imq2);

        IMongoQuery imq3 = genCond(Area.draw);
        int count3 = (int)MongodbPlayer.Instance.ExecuteGetCount(WORLD_CUP_ACTIVITY_PLAYER, imq3);

        Dictionary<string, object> updata = new Dictionary<string, object>();
        updata.Add("matchId", m_curMatchId);
        updata.Add("homeBetPlayerCount", count1);
        updata.Add("visitBetPlayerCount", count2);
        updata.Add("drawBetPlayerCount", count3);

        MongodbLog.Instance.ExecuteInsert(TableName.STAT_WORLD_CUP_MATCH_PLAYER_JOIN, updata);       
    }

    private void saveParam()
    {
        try
        {
            FileStream fs = new FileStream("..\\data\\WorldCupStat.config", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(m_curMatchId);
            sw.Close();
            fs.Close();
        }
        catch (Exception ex)
        {
        }
    }

    private void readParam()
    {
        try
        {
            FileStream fs = new FileStream("..\\data\\WorldCupStat.config", FileMode.Open);
            StreamReader sw = new StreamReader(fs);
            string str = sw.ReadLine();
            if (string.IsNullOrEmpty(str))
            {
                m_curMatchId = 1;
            }
            else
            {
                m_curMatchId = Convert.ToInt32(str);
            }

            sw.Close();
            fs.Close();
        }
        catch (Exception ex)
        {
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 抢购宝箱
class StatPanicBox : StatByDayBase
{
    public override string getStatKey()
    {
        return StatKey.KEY_PANIC_BOX;
    }

    public override void update(double delta)
    {
        if (DateTime.Now < m_statDay.AddMinutes(3))
            return;

        beginStat("StatPanicBox开始统计");
        stat();
        addStatDay();
        endStat("StatPanicBox结束统计");
    }

    void stat()
    {
        DateTime startTime = m_statDay.Date.AddDays(-1);
        IMongoQuery imq = Query.EQ("genTime", startTime);

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce("pumpPanicBoxPlayer",
                                                                        imq,
                                                                        MapReduceTable.getMap("StatPanicBox"),
                                                                        MapReduceTable.getReduce("StatPanicBox"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                BsonValue resValue = d["value"];

                for (int i = 1; i <= 3; i++)
                {
                    int count = resValue["box" + i.ToString()].ToInt32();
                    IMongoQuery imq1 = Query.EQ("boxId", i);
                    IMongoQuery timq = Query.And(imq, imq1);
                    
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Add("lotteryPerson", count);
                    MongodbLog.Instance.ExecuteUpdateByQuery("pumpPanicBox", timq, data, UpdateFlags.None);
                }
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 龙宫参与分布统计
class StatDragonPalaceDistribute : StatByDayBase
{
    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_DRAGON_PALACE_JOIN_DISTRIBUTE;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddMinutes(2);
        if (DateTime.Now >= st)
        {
            stat();
            addStatDay();
        }
    }

    void stat()
    {
        beginStat("StatDragonPalaceDistribute 开始统计");

        DateTime startTime = m_statDay.Date.AddDays(-1);
        IMongoQuery imq1 = Query.LT("genTime", m_statDay);
        IMongoQuery imq2 = Query.GTE("genTime", startTime);
        IMongoQuery imq = Query.And(imq1, imq2);

        statTotal(imq, startTime);
        statJoinCount(imq, startTime);

        endStat("StatDragonPalaceDistribute 结束统计");
    }

    void statTotal(IMongoQuery imq, DateTime startTime)
    {
        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce("pumpDragonPalaceDistribute",
                                                                    imq,
                                                                    MapReduceTable.getMap("StatDragonJoinDistribute"),
                                                                    MapReduceTable.getReduce("StatDragonJoinDistribute"));
        if (mapResult != null)
        {
            IMongoQuery upimq = Query.And(Query.EQ("genTime", startTime));
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                try
                {
                    string s = Convert.ToString(d["_id"]);
                    string[] arr = s.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length == 2)
                    {
                        Dictionary<string, object> data = new Dictionary<string, object>();
                        int gameLevel = Convert.ToInt32(arr[0]);
                        int roomId = Convert.ToInt32(arr[1]);
                        BsonValue resValue = d["value"];
                        //BsonValue pList = resValue["playerList"];
                        int count = resValue["playerList"].ToInt32();
                       // int count = pList.AsBsonArray.Count;
                        data.Add("Level_" + gameLevel.ToString(), count);

                        IMongoQuery imq1 = Query.And(upimq, Query.EQ("roomId", roomId));
                        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_DRAGON_PALACE_JOIN_DISTRIBUTE, imq1, data);
                    }
                }
                catch (System.Exception ex)
                {	
                }
            }
        }
    }

    void statJoinCount(IMongoQuery imq, DateTime startTime)
    {
        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce("pumpDragonPalaceDistribute",
                                                                    imq,
                                                                    MapReduceTable.getMap("StatDragonPlaceJoinCount"),
                                                                    MapReduceTable.getReduce("StatDragonPlaceJoinCount"));
        if (mapResult != null)
        {
            int person = 0;
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            IMongoQuery upimq = Query.And(Query.EQ("genTime", startTime));
            foreach (BsonDocument d in bson)
            {
                //person++;
                int roomId = Convert.ToInt32(d["_id"]);

                BsonValue resValue = d["value"];
                person = resValue["count"].ToInt32();

                IMongoQuery imq1 = Query.And(upimq, Query.EQ("roomId", roomId));

                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("joinPerson", person);
                MongodbLog.Instance.ExecuteUpdateByQuery(TableName.PUMP_DRAGON_PALACE_JOIN, imq1, data);
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 2018国庆中秋乐
class StatNationDay2018 : StatByDayBase
{
    public override string getStatKey()
    {
        return StatKey.KEY_NATIONAL_DAY2018;
    }

    public override void update(double delta)
    {
        if (DateTime.Now < m_statDay.AddMinutes(3))
            return;

        beginStat("StatNationDay2018开始统计");
        stat();
        addStatDay();
        endStat("StatNationDay2018结束统计");
    }

    void stat()
    {
        DateTime startTime = m_statDay.Date.AddDays(-1);
        IMongoQuery imq = Query.EQ("genTime", startTime);

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce("pumpNationalDay2018LotteryPlayer",
                                                                        imq,
                                                                        MapReduceTable.getMap("StatNationDay2018"),
                                                                        MapReduceTable.getReduce("StatNationDay2018"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                BsonValue resValue = d["value"];

                for (int i = 1; i <= 2; i++)
                {
                    int count = resValue["lottery" + i.ToString()].ToInt32();
                    IMongoQuery imq1 = Query.EQ("lotteryId", i);
                    IMongoQuery timq = Query.And(imq, imq1);

                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Add("lotteryPerson", count);
                    MongodbLog.Instance.ExecuteUpdateByQuery("pumpNationalDay2018Lottery", timq, data, UpdateFlags.None);
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////
        mapResult = MongodbLog.Instance.executeMapReduce("pumpNationalDay2018PlayerTask",
                                                         imq,
                                                         MapReduceTable.getMap("StatNationDay2018PlayerTask"),
                                                         MapReduceTable.getReduce("StatNationDay2018PlayerTask"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                string id = Convert.ToString(d["_id"]);
                string[] arr = id.Split('_');
                if (arr.Length != 2)
                    continue;

                int p = Convert.ToInt32(arr[0]);
                if (p < 30)
                {
                    p = 29;
                }
                int taskId = Convert.ToInt32(arr[1]);

                BsonValue resValue = d["value"];
                int finish = Convert.ToInt32(resValue["finish"]);
                int recv = Convert.ToInt32(resValue["recv"]);

                IMongoQuery imq1 = Query.EQ("p", p);
                IMongoQuery imq2 = Query.And(imq, imq1);

                string keyTask = string.Format("t{0}Finish", taskId);
                string keyRecv = string.Format("t{0}Receive", taskId);

                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add(keyTask, finish);
                data.Add(keyRecv, recv);
                MongodbLog.Instance.ExecuteUpdateByQuery("statNationalDay2018PlayerTask", imq2, data, UpdateFlags.Upsert);
            }
        }
    }
}


//////////////////////////////////////////////////////////////////////////
public class ItemBuyInfo
{
    public int m_buyPerson = 0;  // 购买人数
    public int m_buyCount = 0;   // 购买次数
}

public class ItemBuyInfoMgr
{
    Dictionary<int, ItemBuyInfo> m_infos = new Dictionary<int, ItemBuyInfo>();

    public void reset()
    {
        foreach (var info in m_infos)
        {
            info.Value.m_buyPerson = 0;
            info.Value.m_buyCount = 0;
        }
    }

    public ItemBuyInfo addItem(int item)
    {
        if (m_infos.ContainsKey(item))
        {
            return m_infos[item];
        }

        ItemBuyInfo info = new ItemBuyInfo();
        m_infos[item] = info;
        return info;
    }

    public Dictionary<string, object> genData(ChannelInfo cinfo)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        foreach (var info in m_infos)
        {
            data.Add("itemPerson_" + info.Key.ToString(), info.Value.m_buyPerson);
            data.Add("itemCount_" + info.Key.ToString(), info.Value.m_buyCount);
        }
        data.Add("channel", cinfo.m_channelNum);
        return data;
    }
}

// 道具购买
class StatItemBuy : StatByDayBase
{
    ItemBuyInfoMgr m_mgr = new ItemBuyInfoMgr();
    string[] m_arr = { "5", "8", "17", "11", "12", "13", "9", "23", "16" };

    public override string getStatKey()
    {
        return StatKey.KEY_ITEM_BUY;
    }

    public override void update(double delta)
    {
        if (DateTime.Now < m_statDay.AddHours(3))
            return;

        beginStat("StatItemBuy开始统计");
        m_mgr.reset();

        List<ChannelInfo> channelList = ResMgr.getInstance().getChannelList();
        foreach (var info in channelList)
        {
            stat(info);
        }

        addStatDay();
        endStat("StatItemBuy结束统计");
    }

    void stat(ChannelInfo info)
    {
        DateTime startTime = m_statDay.Date.AddDays(-1);
        IMongoQuery imq1 = Query.GTE("genTime", BsonValue.Create(startTime));
        IMongoQuery imq2 = Query.LT("genTime", BsonValue.Create(m_statDay.Date));
        IMongoQuery imq3 = Query.EQ("channel", BsonValue.Create(info.m_channelNum));
        //IMongoQuery imq4 = Query.EQ("reason", 14);
        IMongoQuery imq = Query.And(imq1, imq2, imq3);

        bool hasData = false;
        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_PLAYER_ITEM,
                                                                        imq,
                                                                        MapReduceTable.getMap("StatItemBuy"),
                                                                        MapReduceTable.getReduce("StatItemBuy"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                hasData = true;
                BsonValue resValue = d["value"];
                for (int i = 0; i < m_arr.Length; ++i)
                {
                    ItemBuyInfo itemInfo = m_mgr.addItem(Convert.ToInt32(m_arr[i]));
                    int c = Convert.ToInt32(resValue[m_arr[i]]);
                    if (c > 0)
                    {
                        itemInfo.m_buyPerson += 1;
                    }

                    itemInfo.m_buyCount += c;
                }
            }
        }

        if (hasData)
        {
            IMongoQuery insertImq = Query.EQ("genTime", BsonValue.Create(startTime));
            MongodbLog.Instance.ExecuteUpdateByQuery("statPumpPlayerItem", insertImq, m_mgr.genData(info), UpdateFlags.Upsert);
        }
    }
}

class NewPlayerTime
{
    public int m_turretLevel;

    public string m_channel;

    public int m_onlineTime;

    public int m_count;
}

// 新玩家的各个炮倍的平均时长
class StatNewPlayerTurretGameTime : StatByDayBase
{
    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_NEW_PLAYER_TURRET_TIME;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddHours(2);
        if (DateTime.Now < st)
            return;

        beginStat("StatNewPlayerTurretGameTime开始统计");

        DateTime time = m_statDay.Date.AddDays(-1);
        IMongoQuery imq = Query.EQ("genTime", time);

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce("pumpTurretLevelTime",
                                                                        imq,
                                                                        MapReduceTable.getMap("StatTurretLevelTime"),
                                                                        MapReduceTable.getReduce("StatTurretLevelTime"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();

            foreach (BsonDocument d in bson)
            {
                string s = Convert.ToString(d["_id"]);
                string[] arr = s.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length == 2)
                {
                    NewPlayerTime stat = new NewPlayerTime();
                    stat.m_turretLevel = Convert.ToInt32(arr[0]);
                    stat.m_channel = arr[1];
                    BsonValue resValue = d["value"];
                    stat.m_onlineTime = resValue["gameTime"].ToInt32();
                    stat.m_count = resValue["count"].ToInt32();
                    addRoomTime(stat, time);
                }
            }
        }

        addStatDay();

        endStat("StatNewPlayerTurretGameTime结束统计");
    }

    void addRoomTime(NewPlayerTime stat, DateTime time)
    {
        IMongoQuery imq = Query.And(Query.EQ("genTime", time), Query.EQ("channel", stat.m_channel));
        Dictionary<string, object> data = new Dictionary<string, object>();
        string fieldName = string.Format("turretTime_{0}", stat.m_turretLevel);
        data.Add(fieldName, stat.m_count > 0 ? (int)((double)stat.m_onlineTime / stat.m_count) : 0);

        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_PUMP_NEW_GUILD_LOSE_POINT, imq, data);
    }
}

// vip特权领取统计
class StatVipRecord : StatByDayBase
{
    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_VIP_RECORD;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddHours(2);
        if (DateTime.Now < st)
            return;

        beginStat("StatVipRecord开始统计");

        DateTime time = m_statDay.Date.AddDays(-1);
        IMongoQuery imq = Query.EQ("genTime", time);

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce("pumpVipRecord",
                                                                        imq,
                                                                        MapReduceTable.getMap("StatVipRecord"),
                                                                        MapReduceTable.getReduce("StatVipRecord"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            int missCount = 0;

            foreach (BsonDocument d in bson)
            {
                int vipLevel = Convert.ToInt32(d["_id"]);
                BsonValue resValue = d["value"];
                missCount += resValue["missCount"].ToInt32();
                dic.Add("vipLevel_" + vipLevel, resValue["levelCount"].ToInt32());
            }
            dic.Add("missCount", missCount);
            addStatVip(dic, time);
        }

        addStatDay();

        endStat("StatVipRecord结束统计");
    }

    void addStatVip(Dictionary<string, object> data, DateTime time)
    {
        IMongoQuery imq = Query.And(Query.EQ("genTime", time));
        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_VIP_RECORD, imq, data);
    }
}

// 炮台等级道具统计
class StatTurretItems : StatByDayBase
{
    static string[] s_items = { "item5","item8","item9","item16","item17","item18","item19",
	"item20","item21","item23", "item24","item25","item26","item27","item52", "item72",
    "useLockCount", "useFrozeCount", "useViolentCount", "useCallCount"};

    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_TURRET_ITEMS;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddHours(2);
        if (DateTime.Now < st)
            return;

        beginStat("StatTurretItems开始统计");

        DateTime time = m_statDay.Date.AddDays(-1);
        IMongoQuery imq = Query.EQ("genTime", time);

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce("pumpTurretItems",
                                                                        imq,
                                                                        MapReduceTable.getMap("StatTurretItems"),
                                                                        MapReduceTable.getReduce("StatTurretItems"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();

            foreach (BsonDocument d in bson)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                string levelStr = Convert.ToString(d["_id"]);
                string[] arr = levelStr.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length == 2)
                {
                    int level = Convert.ToInt32(arr[0]);
                    int type = Convert.ToInt32(arr[1]);

                    //dic.Add("turretLevel", level);
                    //dic.Add("type", type);

                    BsonValue resValue = d["value"];
                    int count = resValue["count"].ToInt32();
                    if (count > 0)
                    {
                        foreach (var k in s_items)
                        {
                            int ave = (int)((double)resValue[k].ToInt64() / count);
                            dic.Add(k, ave);
                        }
                    }
                    else
                    {
                        foreach (var k in s_items)
                        {
                            dic.Add(k, 0);
                        }
                    }

                    addStatTurret(level, type, dic, time);
                }
            }
        }

        addStatDay();

        endStat("StatTurretItems结束统计");
    }

    void addStatTurret(int turretLevel, int type, Dictionary<string, object> data, DateTime time)
    {
        IMongoQuery imq = Query.And(Query.EQ("genTime", time),
            Query.EQ("turretLevel", turretLevel),
        Query.EQ("type", type));
        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_TURRET_ITEMS, imq, data);
    }
}

class TypePlayerInfo
{
    public Dictionary<string, object> []m_data;//= new Dictionary<string, object>();

    public TypePlayerInfo()
    {
        m_data = new Dictionary<string, object>[2];
        m_data[0] = new Dictionary<string, object>();
        m_data[1] = new Dictionary<string, object>();
    }

    public void reset()
    {
        m_data[0].Clear();
        m_data[1].Clear();
    }

    public void addGold(int type, long gold, int level, int count)
    {
        if (type < 0 || type >= 2) return;

        string key = string.Format("level_{0}", level);
        if (count > 0)
        {
            m_data[type].Add(key, (long)(gold / count));
        }
    }

    public Dictionary<string, object> getData(int type)
    {
        return m_data[type];
    }
}

// 玩家携带金币统计
class StatPlayerWithGold : StatByDayBase
{
    TypePlayerInfo m_data = new TypePlayerInfo();

    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_PLAYER_WITH_GOLD;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddHours(2);
        if (DateTime.Now < st)
            return;

        m_data.reset();

        beginStat("StatPlayerWithGold开始统计");

        DateTime time = m_statDay.Date.AddDays(-1);
        IMongoQuery imq = Query.EQ("genTime", time);

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.STAT_PUMP_TURRET_PROPERTY,
                                                                        imq,
                                                                        MapReduceTable.getMap("StatPlayerWithGold"),
                                                                        MapReduceTable.getReduce("StatPlayerWithGold"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();

            foreach (BsonDocument d in bson)
            {
                string levelStr = Convert.ToString(d["_id"]);
                string[] arr = levelStr.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length == 2)
                {
                    int level = Convert.ToInt32(arr[0]);
                    int type = Convert.ToInt32(arr[1]);

                    BsonValue resValue = d["value"];
                    long gold = resValue["gold"].ToInt64();
                    int count = resValue["count"].ToInt32();
                    m_data.addGold(type, gold, level, count);
                }
            }
        }
        onStatWithGold(time);
        m_data.reset();
        addStatDay();

        endStat("StatPlayerWithGold结束统计");
    }

    void onStatWithGold(DateTime time)
    {
        for (int i = 0; i < 2; ++i)
        {
            IMongoQuery imq = Query.And(Query.EQ("genTime", time), Query.EQ("type", i));

            MongodbLog.Instance.ExecuteUpdateByQuery("statTurretProperty", imq, m_data.getData(i));
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 统计玩家实物兑换
class StatPlayerExchange : StatByDayBase
{
    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_PLAYER_EXCHANGE;
    }

    public override void update(double delta)
    {
        DateTime st = m_statDay.AddHours(4);
        if (DateTime.Now < st)
            return;

        beginStat("StatPlayerExchange开始统计");

        DateTime time = m_statDay.Date.AddDays(-1);
        //IMongoQuery imq1 = Query.GTE("giveOutTime", BsonValue.Create(time));
        //IMongoQuery imq2 = Query.LT("giveOutTime", BsonValue.Create(m_statDay.Date));
        //IMongoQuery imq = Query.And(imq1, imq2);
        List<IMongoQuery> imqList = new List<IMongoQuery>();
        imqList.Add(Query.EQ("chgId", 7));
        imqList.Add(Query.EQ("chgId", 8));
        imqList.Add(Query.EQ("chgId", 9));
        imqList.Add(Query.EQ("chgId", 10));
        imqList.Add(Query.EQ("chgId", 11));
        imqList.Add(Query.EQ("chgId", 12));
        //imqList.Add(Query.EQ("chgId", 14));
        imqList.Add(Query.EQ("chgId", 15));
        imqList.Add(Query.EQ("chgId", 21));
        imqList.Add(Query.EQ("chgId", 29));
        imqList.Add(Query.EQ("chgId", 22));
        imqList.Add(Query.EQ("chgId", 30));
        imqList.Add(Query.EQ("chgId", 31));
        IMongoQuery imq1 = Query.Or(imqList);
        IMongoQuery imq = Query.And(imq1, Query.EQ("status", 4));

        MapReduceResult mapResult = MongodbPlayer.Instance.executeMapReduce(TableName.EXCHANGE,
                                                                        imq,
                                                                        MapReduceTable.getMap("StatPlayerExchange"),
                                                                        MapReduceTable.getReduce("StatPlayerExchange"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();

            foreach (BsonDocument d in bson)
            {
                int playerId = Convert.ToInt32(d["_id"]);
                BsonValue resValue = d["value"];
                int money = resValue["money"].ToInt32();
                statMoney(playerId, money);
            }
        }
       
        addStatDay();

        endStat("StatPlayerExchange结束统计");
    }

    void statMoney(int playerId, int money)
    {
        IMongoQuery imq = Query.And(Query.EQ("playerId", playerId));
        //UpdateBuilder ub = new UpdateBuilder();
        //ub.Inc("historyMoney", money);
        Dictionary<string, object> d = new Dictionary<string, object>();
        d.Add("historyMoney", money);
        // MongodbLog.Instance.ExecuteUpdateByQuery("statPlayerExchange", imq, ub);
        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_PLAYER_EXCHANGE, imq, d);
    }
}

//////////////////////////////////////////////////////////////////////////
public class Stat100003Info
{
    public Dictionary<string, object> m_data = new Dictionary<string, object>();

    public int GoldId { set; get; }
    public int RmbId { set; get; }

    public void init()
    {
        StatChannel100003_CFGData dataGold = StatChannel100003_CFG.getInstance().getValue(GoldId);
        int c = dataGold.m_dataList.Count;
        for (int i = 0; i < c; ++i)
        {
            m_data.Add("gold" + i, 0);
        }

        StatChannel100003_CFGData dataMoney = StatChannel100003_CFG.getInstance().getValue(RmbId);
        c = dataMoney.m_dataList.Count;
        for (int i = 0; i < c; ++i)
        {
            m_data.Add("pay" + i, 0);
        }
    }

    public void reset()
    {
        m_data.Clear();
        init();
    }

    // 达到指定金币的数量
    public void incrGoldCount(int goldIndex)
    {
        string key = "gold" + goldIndex;
        m_data[key] = 1 + Convert.ToInt32(m_data[key]);
    }

    // 达到指定充值的数量
    public void incrMoneyCount(int moneyIndex)
    {
        string key = "pay" + moneyIndex;
        m_data[key] = 1 + Convert.ToInt32(m_data[key]);
    }

    public void setRegCount(int count)
    {
        m_data["regeditCount"] = count;
    }
}

/*
// 渠道赢取金币统计
class StatChannel_100003 : StatByDayBase
{
    private DateTime m_statFromTime;
    private DateTime m_statToTime;

    DateTime m_lastTime = DateTime.Now;

    Stat100003Info m_statInfo = new Stat100003Info();

    public override void init()
    {
        base.init();

        m_statFromTime = new DateTime(2020, 5, 22);
        m_statToTime = new DateTime(2020, 5, 23);

        m_statInfo.GoldId = 1;
        m_statInfo.RmbId = 2;
    }

    public override string getStatKey()
    {
        return StatKey.KEY_CHANNEL_100003;
    }

    public override void update(double delta)
    {
        bool isAddDays = true;
        DateTime st = m_statDay.AddMinutes(6);
        if (DateTime.Now < st)
        {
            TimeSpan span = DateTime.Now - m_lastTime;
            if (span.TotalMinutes < 90)
            {
                return;
            }
            isAddDays = false;
            m_lastTime = DateTime.Now;
        }

        do 
        {
            if (!readActTime())
            {
                break;
            }

            DateTime time = m_statDay.Date.AddDays(-1);
            if (time > m_statToTime)
                break;
            if (time < m_statFromTime)
                break;

            beginStat("StatChannel_100003开始统计");

            m_statInfo.reset();

            IMongoQuery imq1 = Query.GTE("genTime", BsonValue.Create(m_statFromTime));
            // 截止到统计日
            IMongoQuery imq2 = Query.LT("genTime", BsonValue.Create(m_statDay));
            IMongoQuery imq = Query.And(imq1, imq2);

            MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce("pumpChannelPlayer100003",
                                                                            imq,
                                                                            MapReduceTable.getMap("StatChannelPlayer100003"),
                                                                            MapReduceTable.getReduce("StatChannelPlayer100003"));
            if (mapResult != null)
            {
                IEnumerable<BsonDocument> bson = mapResult.GetResults();

                foreach (BsonDocument d in bson)
                {
                    int playerId = Convert.ToInt32(d["_id"]);
                    BsonValue resValue = d["value"];
                    int money = resValue["totalPay"].ToInt32();
                    long totalGold = resValue["totalGold"].ToInt64();

                    statCount(money, totalGold);
                }
            }
            statReg(imq);
            statMoneyGold(time);

            endStat("StatChannel_100003结束统计");

        } while (false);

        if (isAddDays)
        {
            addStatDay();
        }
    }

    void statCount(int money, long totalGold)
    {
        int index = StatChannel100003_CFG.getInstance().getIndex(m_statInfo.GoldId, totalGold);
        for (int i = 0; i <= index; ++i)
        {
            m_statInfo.incrGoldCount(i);
        }

        index = StatChannel100003_CFG.getInstance().getIndex(m_statInfo.RmbId, money);
        for (int i = 0; i <= index; ++i)
        {
            m_statInfo.incrMoneyCount(i);
        }
    }

    // 统计注册
    void statReg(IMongoQuery imq)
    {
        IMongoQuery imq1 = Query.EQ("channel", BsonValue.Create("100003"));
        IMongoQuery imqTmp = Query.And(imq1, imq);

        MapReduceResult mapResult = MongodbAccount.Instance.executeMapReduce(TableName.CHANNEL_TD,
                                                                        imqTmp,
                                                                        MapReduceTable.getMap("StatChannelReg100003"),
                                                                        MapReduceTable.getReduce("StatChannelReg100003"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();

            foreach (BsonDocument d in bson)
            {
                BsonValue resValue = d["value"];
                int regCount = resValue["count"].ToInt32();

                m_statInfo.setRegCount(regCount);
            }
        }
    }

    void statMoneyGold(DateTime time)
    {
        IMongoQuery imq = Query.EQ("genTime", BsonValue.Create(time));
        MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_CHANNEL100003, imq, m_statInfo.m_data);
    }

    // 读取活动时间
    bool readActTime()
    {
        Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetOneBykey(TableName.ACT_CHANNEL100003, "key", 1);
        if (data != null)
        {
            m_statFromTime = Convert.ToDateTime(data["startTime"]).ToLocalTime();
            m_statToTime = Convert.ToDateTime(data["endTime"]).ToLocalTime();
            return true;
        }
        else
        {
            LogMgr.log.ErrorFormat("StatChannel_100003, 没有设置活动时间");
            return false;
        }
    }
}
*/
//////////////////////////////////////////////////////////////////////////
// 渠道赢取金币统计
class StatChannel_100003Base : StatByDayBase
{
    public const int PLAYER_ALL = 0;  // 全部玩家
    public const int PLAYER_NEW = 1;  // 新玩家
    public const int PLAYER_OLD = 2;  // 老玩家
    public const int PLAYER_MAX = 3;
    public static string[] FIELDS = { "create_time" };

    private DateTime m_statFromTime;
    private DateTime m_statToTime;
    // 期号
    private int m_qihao = 1;

    DateTime m_lastTime = DateTime.Now;

    protected Stat100003Info[] m_statInfo = new Stat100003Info[PLAYER_MAX]{
        new Stat100003Info(),
        new Stat100003Info(),
        new Stat100003Info()
    };

    public string PumpTableName { set; get; }
    // 100003
    public string ChannelId { set; get; }
    // 统计结果表 TableName.STAT_CHANNEL100003
    public string DstStatTable { set; get; }
    // 活动时间表 ID
    public int ActTimeTableId { set; get; }

    public override void init()
    {
        base.init();

        m_statFromTime = new DateTime(2020, 5, 22);
        m_statToTime = new DateTime(2020, 5, 23);
    }

    public override string getStatKey()
    {
        return StatKey.KEY_CHANNEL_100003;
    }

    public override void update(double delta)
    {
        bool isAddDays = true;
        DateTime st = m_statDay.AddMinutes(6);
        if (DateTime.Now < st)
        {
            TimeSpan span = DateTime.Now - m_lastTime;
            if (span.TotalMinutes < 90)
            {
                return;
            }
            isAddDays = false;
            m_lastTime = DateTime.Now;
        }

        do
        {
            if (!readActTime())
            {
                break;
            }

            DateTime time = m_statDay.Date.AddDays(-1);
            if (time > m_statToTime)
                break;
            if (time < m_statFromTime)
                break;

            beginStat("StatChannel_100003开始统计, {0}", ChannelId);

            for (int i = 0; i < PLAYER_MAX; i++)
            {
                m_statInfo[i].reset();
            }

            IMongoQuery imq1 = Query.GTE("genTime", BsonValue.Create(m_statFromTime));
            // 截止到统计日
            IMongoQuery imq2 = Query.LT("genTime", BsonValue.Create(m_statDay));
            IMongoQuery imq = Query.And(imq1, imq2);

            MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(PumpTableName,
                                                                            imq,
                                                                            MapReduceTable.getMap("StatChannelPlayer100003"),
                                                                            MapReduceTable.getReduce("StatChannelPlayer100003"));
            int oldPlayerCount = 0;
            int newPlayerCount = 0;
            if (mapResult != null)
            {
                IEnumerable<BsonDocument> bson = mapResult.GetResults();

                foreach (BsonDocument d in bson)
                {
                    int playerId = Convert.ToInt32(d["_id"]);
                    BsonValue resValue = d["value"];
                    int money = resValue["totalPay"].ToInt32();
                    long totalGold = resValue["totalGold"].ToInt64();
                    int playerType = getPlayerType(playerId);
                    statCount(money, totalGold, m_statInfo[playerType]);
                    statCount(money, totalGold, m_statInfo[PLAYER_ALL]);
                    if (playerType == PLAYER_NEW)
                    {
                        newPlayerCount++;
                    }
                    else if (playerType == PLAYER_OLD)
                    {
                        oldPlayerCount++;
                    }
                }
            }
            statReg(imq, oldPlayerCount, newPlayerCount);
            statMoneyGold(time);

            endStat("StatChannel_100003结束统计, {0}", ChannelId);

        } while (false);

        if (isAddDays)
        {
            addStatDay();
        }
    }

    // 实现此函数
    protected virtual void statCount(int money, long totalGold, Stat100003Info stat)
    {
        int index = StatChannel100003_CFG.getInstance().getIndex(stat.GoldId, totalGold);
        for (int i = 0; i <= index; ++i)
        {
            stat.incrGoldCount(i);
        }

        index = StatChannel100003_CFG.getInstance().getIndex(stat.RmbId, money);
        for (int i = 0; i <= index; ++i)
        {
            stat.incrMoneyCount(i);
        }
    }

    public void setID(int goldId, int rmbId)
    {
        for (int i = 0; i < PLAYER_MAX; i++)
        {
            m_statInfo[i].GoldId = goldId;
            m_statInfo[i].RmbId = rmbId;
        }
    }

    // 统计注册
    void statReg(IMongoQuery imq, int oldPlayerCount, int newPlayerCount)
    {
        IMongoQuery imq1 = Query.EQ("channel", BsonValue.Create(ChannelId));
        IMongoQuery imqTmp = Query.And(imq1, imq);

        MapReduceResult mapResult = MongodbAccount.Instance.executeMapReduce(TableName.CHANNEL_TD,
                                                                        imqTmp,
                                                                        MapReduceTable.getMap("StatChannelReg100003"),
                                                                        MapReduceTable.getReduce("StatChannelReg100003"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();

            foreach (BsonDocument d in bson)
            {
                BsonValue resValue = d["value"];
                int regCount = resValue["count"].ToInt32();

                for (int i = 0; i < PLAYER_MAX; i++)
                {
                    if (i == PLAYER_ALL)
                    {
                        m_statInfo[i].setRegCount(regCount + newPlayerCount);
                    }
                    else if (i == PLAYER_NEW)
                    {
                        m_statInfo[i].setRegCount(regCount);
                    }
                    else
                    {
                        m_statInfo[i].setRegCount(oldPlayerCount);
                    }
                }
            }
        }
    }

    void statMoneyGold(DateTime time)
    {
        for (int i = 0; i < PLAYER_MAX; i++)
        {
            IMongoQuery imq1 = Query.EQ("genTime", BsonValue.Create(time));
            IMongoQuery imq2 = Query.EQ("playerType", BsonValue.Create(i));
            IMongoQuery imq = Query.And(imq1, imq2);
            m_statInfo[i].m_data.Add("qihao", m_qihao);
            MongodbLog.Instance.ExecuteUpdateByQuery(DstStatTable, imq, m_statInfo[i].m_data);
        }
    }

    // 读取活动时间
    bool readActTime()
    {
        Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetOneBykey(TableName.ACT_CHANNEL100003, "key", ActTimeTableId);
        if (data != null)
        {
            m_statFromTime = Convert.ToDateTime(data["startTime"]).ToLocalTime();
            m_statToTime = Convert.ToDateTime(data["endTime"]).ToLocalTime();
            if (data.ContainsKey("qihao"))
            {
                m_qihao = Convert.ToInt32(data["qihao"]);
            }
            return true;
        }
        else
        {
            LogMgr.log.ErrorFormat("StatChannel_100003, id {0}, 没有设置活动时间", ActTimeTableId);
            return false;
        }
    }

    // 取得玩家类型
    int getPlayerType(int playerId)
    {
        int playerType = PLAYER_NEW;
        do
        {
            Dictionary<string, object> d = MongodbPlayer.Instance.ExecuteGetBykey(TableName.PLAYER_INFO, "player_id", playerId, FIELDS);
            if (d == null)
                break;

            if (d.ContainsKey("create_time"))
            {
                DateTime t = Convert.ToDateTime(d["create_time"]).ToLocalTime();
                if (t >= m_statFromTime && t < m_statToTime.AddDays(1))
                {
                }
                else
                {
                    playerType = PLAYER_OLD;
                }
            }
        } while (false);

        return playerType;
    }
}

//////////////////////////////////////////////////////////////////////////
// 蛋蛋赚
class StatChannel_100009 : StatChannel_100003Base
{
    public StatChannel_100009()
    {
        PumpTableName = "pumpChannelPlayer100009";
        ChannelId = "100009";
        DstStatTable = TableName.STAT_CHANNEL100009;
        ActTimeTableId = 2;
        setID(3, 4);
        //Stat100003Info.GoldId = 3;
        //Stat100003Info.RmbId = 4;
    }

    public override string getStatKey()
    {
        return StatKey.KEY_CHANNEL_100009;
    }
}

// 葫芦星球
class StatChannel_100010 : StatChannel_100003Base
{
    public StatChannel_100010()
    {
        PumpTableName = "pumpChannelPlayer100010";
        ChannelId = "100010";
        DstStatTable = TableName.STAT_CHANNEL100010;
        ActTimeTableId = 3;
        setID(5, 6);
        //m_statInfo.GoldId = 5;
        //m_statInfo.RmbId = 6;
    }

    public override string getStatKey()
    {
        return StatKey.KEY_CHANNEL_100010;
    }
}

// 有赚
class StatChannel_100011 : StatChannel_100003Base
{
    public StatChannel_100011()
    {
        PumpTableName = "pumpChannelPlayer100011";
        ChannelId = "100011";
        DstStatTable = TableName.STAT_CHANNEL100011;
        ActTimeTableId = 4;
        setID(7, 8);
        //Stat100003Info.GoldId = 7;
        //Stat100003Info.RmbId = 8;
    }

    public override string getStatKey()
    {
        return StatKey.KEY_CHANNEL_100011;
    }
}

// 闲玩
class StatChannel_100003 : StatChannel_100003Base
{
    public StatChannel_100003()
    {
        PumpTableName = "pumpChannelPlayer100003";
        ChannelId = "100003";
        DstStatTable = TableName.STAT_CHANNEL100003;
        ActTimeTableId = 1;
        setID(1, 2);
        //m_statInfo.GoldId = 1;
        //m_statInfo.RmbId = 2;
    }

    public override string getStatKey()
    {
        return StatKey.KEY_CHANNEL_100003;
    }
}

// 麦子赚
class StatChannel_100012 : StatChannel_100003Base
{
    public StatChannel_100012()
    {
        PumpTableName = "pumpChannelPlayer100012";
        ChannelId = "100012";
        DstStatTable = TableName.STAT_CHANNEL100012;
        ActTimeTableId = 5;
        setID(9, 10);
        //Stat100003Info.GoldId = 7;
        //Stat100003Info.RmbId = 8;
    }

    public override string getStatKey()
    {
        return StatKey.KEY_CHANNEL_100012;
    }
}

// 聚享游
class StatChannel_100013 : StatChannel_100003Base
{
    public StatChannel_100013()
    {
        PumpTableName = "pumpChannelPlayer100013";
        ChannelId = "100013";
        DstStatTable = TableName.STAT_CHANNEL100013;
        ActTimeTableId = 6;
        setID(11, 12);
        //Stat100003Info.GoldId = 7;
        //Stat100003Info.RmbId = 8;
    }

    public override string getStatKey()
    {
        return StatKey.KEY_CHANNEL_100013;
    }
}

// 小啄
class StatChannel_100014 : StatChannel_100003Base
{
    public StatChannel_100014()
    {
        PumpTableName = "pumpChannelPlayer100014";
        ChannelId = "100014";
        DstStatTable = TableName.STAT_CHANNEL100014;
        ActTimeTableId = 7;
        setID(13, 14);
    }

    public override string getStatKey()
    {
        return StatKey.KEY_CHANNEL_100014;
    }
}

// 泡泡赚
class StatChannel_100015 : StatChannel_100003Base
{
    public StatChannel_100015()
    {
        PumpTableName = "pumpChannelPlayer100015";
        ChannelId = "100015";
        DstStatTable = TableName.STAT_CHANNEL100015;
        ActTimeTableId = 8;
        setID(15, 16);
    }

    public override string getStatKey()
    {
        return StatKey.KEY_CHANNEL_100015;
    }
}

// 豆豆趣玩
class StatChannel_100016 : StatChannel_100003Base
{
    public StatChannel_100016()
    {
        PumpTableName = "pumpChannelPlayer100016";
        ChannelId = "100016";
        DstStatTable = TableName.STAT_CHANNEL100016;
        ActTimeTableId = 9;
        setID(17, 18);
    }

    public override string getStatKey()
    {
        return StatKey.KEY_CHANNEL_100016;
    }
}

//////////////////////////////////////////////////////////////////////////
// 玩家等级分布
class StatTouchFishPlayerDistri : StatByMonthBase
{
    DateTime m_lastTime = DateTime.Now;

    public override void init()
    {
        base.init();
    }

    public override string getStatKey()
    {
        return StatKey.KEY_TOUCH_FISH_PLAYER_DISTRI;
    }

    public override void update(double delta)
    {
        IMongoQuery imq = null;
        DateTime st = m_statDay.AddHours(1);
        if (DateTime.Now >= st)
        {
            imq = getCond(true); // 统计上个月的
            stat(imq);
            addStatDay();
        }
        else
        {
            TimeSpan span = DateTime.Now - m_lastTime;
            //if (span.TotalHours >= 0.01)
            if (span.TotalHours >= 2.0)
            {
                imq = getCond(false);
                stat(imq);
                m_lastTime = DateTime.Now;
            }
        }
    }

    void stat(IMongoQuery imq)
    {
        beginStat("StatTouchFishPlayerDistri开始统计");
        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.PUMP_TOUCH_FISH_PLAYER_DISTRI,
                                                                        imq,
                                                                        MapReduceTable.getMap("StatTouchFishPlayerDistri"),
                                                                        MapReduceTable.getReduce("StatTouchFishPlayerDistri"));
        if (mapResult != null)
        {
            Dictionary<string, object> updata = new Dictionary<string, object>();
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                updata.Clear();
                int adv = Convert.ToInt32(d["_id"]);
                BsonValue resValue = d["value"];
                for (int i = 0; i < 12; i++)
                {
                    int v = resValue["index" + i.ToString()].ToInt32();
                    updata.Add("index" + i.ToString(), v);
                }

                IMongoQuery imq1 = Query.EQ("adv", adv);
                IMongoQuery cond = Query.And(imq1, imq);
                MongodbLog.Instance.ExecuteUpdateByQuery(TableName.STAT_TOUCH_FISH_PLAYER_DISTRI, cond, updata);
            }
        }

        endStat("StatTouchFishPlayerDistri结束统计");
    }

    IMongoQuery getCond(bool isLastMonth)
    {
        DateTime now = DateTime.Now;
        
        if(isLastMonth)
        {
            now = new DateTime(now.Year, now.Month, 1);
            now = now.AddMonths(-1);
        }

        int year = now.Year;
        int month = now.Month;
        int d = year * 100 + month;
        return Query.EQ("season", d);
    }
}
