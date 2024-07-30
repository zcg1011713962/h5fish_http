using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

// 渠道数据周统计
public class StatChannelWeek : SysBase
{
    public const string CHANNEL_WEEK_STAT_DAY = "channelWeekStatDay";

    public override void init()
    {
        initChannel();
    }

    public override void update(double delta)
    {
        DateTime now = DateTime.Now;
        DateWeekCal dc = new DateWeekCal();
        WStatParam statParam = null;

        List<WeekChannelInfo> channelList = ResMgr.getInstance().getChannelWeekList();
        foreach (var info in channelList)
        {
            bool isStat = false;
            ChannelInfo channelInfo = info.m_channelInfo;
            WeekChannelInfo weekInfo = info;

            if (weekInfo.m_weekStatTime <= weekInfo.m_monthStatTime) // 2月份
            {
                if (now >= weekInfo.m_weekStatTime.AddHours(4))
                {
                    DateTime L = weekInfo.m_weekStatTime.AddDays(-1);
                    dc.init(L.Year, L.Month);
                    statParam = dc.getWeekStatParam(weekInfo.m_weekStatTime);
                    isStat = true;
                }
            }
            else if (now >= weekInfo.m_monthStatTime.AddHours(4))
            {
                DateTime L = weekInfo.m_monthStatTime.AddDays(-1);
                dc.init(L.Year, L.Month);
                statParam = dc.getMonthStatParam();
                isStat = true;
            }

            if (isStat)
            {
                statWeekData(statParam, weekInfo);

                if (statParam.m_week == 0)
                {
                    DateWeekCal dctmp = dc.getNextMonthCal();
                    var wi = dctmp.getWeekInfoByWeek(1);
                    resetChannelStatDay(info, dc.getNextMonthEndTime(), wi.m_endTime);
                }
                else
                {
                    resetChannelStatDay(info, weekInfo.m_monthStatTime, dc.getNextWeekEndTime(statParam.m_week));
                }
            }
        }
    }

    private void initChannel()
    {
        List<WeekChannelInfo> channelList = ResMgr.getInstance().getChannelWeekList();
        foreach (var weekInfo in channelList)
        {
            var info = weekInfo.m_channelInfo;

            Dictionary<string, object> data =
                MongodbAccount.Instance.ExecuteGetOneBykey(CHANNEL_WEEK_STAT_DAY, "channel", info.m_channelNum);
            if (data != null)
            {
                weekInfo.m_monthStatTime = Convert.ToDateTime(data["monthStatTime"]).ToLocalTime();
                weekInfo.m_weekStatTime = Convert.ToDateTime(data["weekStatTime"]).ToLocalTime();
            }
            else
            {
                DateTime now = DateTime.Now.Date;
                DateWeekCal dc = new DateWeekCal();
                dc.init(now.Year, now.Month);

                WeekInfo wi = dc.getWeekInfoByWeek(1);

                weekInfo.m_monthStatTime = dc.getMonthEndTime();
                weekInfo.m_weekStatTime = wi.m_endTime;

                Dictionary<string, object> newData = new Dictionary<string, object>();
                newData.Add("monthStatTime", weekInfo.m_monthStatTime);
                newData.Add("weekStatTime", weekInfo.m_weekStatTime);
                MongodbAccount.Instance.ExecuteStoreBykey(CHANNEL_WEEK_STAT_DAY, "channel", info.m_channelNum, newData);
            }
        }
    }

    // week 0表示整月  1-第一周 2-第二周 ... 
    private void statWeekData(WStatParam statParam, WeekChannelInfo weekInfo)
    {
        StatWeekResult result = new StatWeekResult();
        statRegedit(statParam, weekInfo.m_channelInfo, result);
        statActive(statParam, weekInfo.m_channelInfo, result);
        statRecharge(statParam, weekInfo.m_channelInfo, result);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("regeditCountWeek" + statParam.m_week, result.m_regeditCount);
        data.Add("activeCountWeek" + statParam.m_week, result.m_activeCount);
        data.Add("totalIncomeWeek" + statParam.m_week, result.m_totalIncome);
        data.Add("rechargePersonNumWeek" + statParam.m_week, result.m_rechargePersonNum);
        data.Add("rechargeCountWeek" + statParam.m_week, result.m_rechargeCount);

        long n = statParam.m_startTime.Year * 100 + statParam.m_startTime.Month;
        IMongoQuery imq1 = Query.EQ("genTime", n);
        IMongoQuery imq2 = Query.EQ("channel", BsonValue.Create(weekInfo.m_channelInfo.m_channelNum));
        IMongoQuery imq = Query.And(imq1, imq2);
        MongodbAccount.Instance.ExecuteUpdateByQuery(TableName.CHANNEL_TD_WEEK, imq, data);
    }

    private void resetChannelStatDay(WeekChannelInfo info, DateTime month, DateTime week)
    {
        Dictionary<string, object> upData = new Dictionary<string, object>();
        upData.Add("monthStatTime", month);
        upData.Add("weekStatTime", week);

        MongodbAccount.Instance.ExecuteStoreBykey(CHANNEL_WEEK_STAT_DAY, "channel", info.m_channelInfo.m_channelNum, upData);
        info.m_monthStatTime = month;
        info.m_weekStatTime = week;
    }

    void statRegedit(WStatParam statParam, ChannelInfo cinfo, StatWeekResult result)
    {
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(statParam.m_endTime));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(statParam.m_startTime));
        IMongoQuery imq3 = Query.EQ("channel", cinfo.m_channelNum);
        IMongoQuery imq = Query.And(imq1, imq2, imq3);

        MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.CHANNEL_TD,
                                                                             imq,
                                                                             MapReduceTable.getMap("StatRegeditFromChannel"),
                                                                             MapReduceTable.getReduce("StatRegeditFromChannel"));

        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                BsonValue resValue = d["value"];
                result.m_regeditCount = resValue["count"].ToInt64();
            }
        }
    }

    void statActive(WStatParam statParam, ChannelInfo cinfo, StatWeekResult result)
    {        
        IMongoQuery imq1 = Query.LT("time", BsonValue.Create(statParam.m_endTime));
        IMongoQuery imq2 = Query.GTE("time", BsonValue.Create(statParam.m_startTime));
        IMongoQuery imq3 = Query.EQ("channel", BsonValue.Create(cinfo.m_channelNum));

        IMongoQuery imq = Query.And(imq1, imq2, imq3);

        MapReduceResult mapResult = MongodbAccount.Instance.executeMapReduce(cinfo.m_loginTable,
                                                                             imq,
                                                                             MapReduceTable.getMap("active"),
                                                                             MapReduceTable.getReduce("active"));
        if (mapResult != null)
        {
            result.m_activeCount = (int)mapResult.OutputCount;
        }
    }

    void statRecharge(WStatParam statParam, ChannelInfo cinfo, StatWeekResult result)
    {
        IMongoQuery imq1 = Query.LT("CreateTime", BsonValue.Create(statParam.m_endTime));
        IMongoQuery imq2 = Query.GTE("CreateTime", BsonValue.Create(statParam.m_startTime));
        IMongoQuery imq3 = Query.EQ("channel_number", BsonValue.Create(cinfo.m_channelNum));
        IMongoQuery imq4 = Query.NE("status", 1);
        IMongoQuery imq = Query.And(imq1, imq2, imq3, imq4);

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
                result.m_totalIncome += resValue["total"].ToInt32();
                result.m_rechargePersonNum++;
                result.m_rechargeCount += resValue["rechargeCount"].ToInt32();
            }
        }
    }
}

