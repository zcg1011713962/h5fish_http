using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Windows.Forms;

// 统计付费
public class StatUnitRecharge : StatBase
{
    public override void doStat(object param, StatResult result)
    {
        ParamStat p = (ParamStat)param;

        ChannelInfo cinfo = p.m_channel;

        DateTime mint = cinfo.m_statDay.Date.AddDays(-1), maxt = cinfo.m_statDay.Date;
        IMongoQuery imq1 = Query.LT("CreateTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("CreateTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.EQ("channel_number", BsonValue.Create(cinfo.m_channelNum));
        IMongoQuery imq4 = Query.NE("status", 1);
        IMongoQuery imq = Query.And(imq1, imq2, imq3, imq4);

        MapReduceResult mapResult = MongodbPayment.Instance.executeMapReduce(TableName.RECHARGE_TOTAL, //cinfo.m_paymentTable,
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

        statPaytype(imq, cinfo);
    }

    void statPaytype(IMongoQuery imq, ChannelInfo cinfo)
    {
        MapReduceResult mapResult = MongodbPayment.Instance.executeMapReduce(TableName.RECHARGE_TOTAL,
                                                                             imq,
                                                                             MapReduceTable.getMap("StatRechargePayType"),
                                                                             MapReduceTable.getReduce("StatRechargePayType"));
        if (mapResult != null)
        {
            IEnumerable<BsonDocument> bson = mapResult.GetResults();
            foreach (BsonDocument d in bson)
            {
                string payType = Convert.ToString(d["_id"]);
                BsonValue resValue = d["value"];

                int total = resValue["total"].ToInt32();
                int rechargePerson = resValue["rechargePerson"].ToInt32();
                int rechargeCount = resValue["rechargeCount"].ToInt32();

                Dictionary<string, object> upData = new Dictionary<string, object>();
                upData.Add("genTime", cinfo.m_statDay.Date.AddDays(-1));
                upData.Add("channel", cinfo.m_channelNum);
                upData.Add(payType.ToString() + "_rmb", total);
                upData.Add(payType.ToString() + "_rechargePerson", rechargePerson);
                upData.Add(payType.ToString() + "_rechargeCount", rechargeCount);

                IMongoQuery upCond = Query.And(Query.EQ("genTime", cinfo.m_statDay.Date.AddDays(-1)),
                    Query.EQ("channel", cinfo.m_channelNum));
                string str = MongodbAccount.Instance.ExecuteStoreByQuery(TableName.CHANNEL_TD_PAY, upCond, upData);
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 玩家价值
public class StatLTV : StatUnitRemain
{
    public const string ACC_KEY_1 = "real_acc";

    public override void doStat(object param, StatResult result)
    {
        ParamStat p = (ParamStat)param;
        statXDayTotalRecharge1(p, 0, ref result.m_newAccIncome, result);
        result.m_2DayRechargePersonNumTmp = statXDayTotalRecharge(p, CRemainConst.DAY_2_REMAIN, ref result.m_1DayTotalRechargeTmp);

        statXDayTotalRechargeLtv(p, 0/*CRemainConst.DAY_2_REMAIN*/, ref result.m_1DayTotalRechargeTmp);
        statXDayTotalRechargeLtv(p, CRemainConst.DAY_3_REMAIN, ref result.m_3DayTotalRechargeTmp);
        statXDayTotalRechargeLtv(p, CRemainConst.DAY_7_REMAIN, ref result.m_7DayTotalRechargeTmp);
        statXDayTotalRechargeLtv(p, CRemainConst.DAY_14_REMAIN, ref result.m_14DayTotalRechargeTmp);
        statXDayTotalRechargeLtv(p, CRemainConst.DAY_30_REMAIN, ref result.m_30DayTotalRechargeTmp);
        statXDayTotalRechargeLtv(p, CRemainConst.DAY_60_REMAIN, ref result.m_60DayTotalRechargeTmp);
        statXDayTotalRechargeLtv(p, CRemainConst.DAY_90_REMAIN, ref result.m_90DayTotalRechargeTmp);
    }

    // 统计几天前，注册玩家充值总额
    private int statXDayTotalRecharge(ParamStat param, int days, ref int totalRecharge, StatResult result = null)
    {
        totalRecharge = 0;
        m_regeditAccList.Clear();

        ChannelInfo statDay = (ChannelInfo)param.m_channel;
        queryRegeditAcc(param, StatBase.getRemainRegTime(statDay, days));
        if (m_regeditAccList.Count == 0) // 没有注册
            return 0;

        int playerCount = 0;
        IMongoQuery imq = StatBase.getRechargeCond(param, 1);
        foreach (var acc in m_regeditAccList)
        {
            int tmp = statPlayerRecharge(statDay, imq, acc);
            totalRecharge += tmp;

            if (tmp > 0)
            {
                playerCount++;
            }
        }

        if (days == 0)
        {
            result.m_newAccRechargePersonNum = playerCount;
        }

        return playerCount;
    }

    // 统计几天前，注册玩家充值总额
    private int statXDayTotalRechargeLtv(ParamStat param, int days, ref int totalRecharge, StatResult result = null)
    {
        totalRecharge = 0;
        m_regeditAccList.Clear();

        ChannelInfo statDay = (ChannelInfo)param.m_channel;
        queryRegeditAcc(param, StatBase.getRemainRegTime(statDay, days));
        if (m_regeditAccList.Count == 0) // 没有注册
            return 0;

        int playerCount = 0;
        IMongoQuery imq = StatBase.getRechargeCond(param, days + 1);
        foreach (var acc in m_regeditAccList)
        {
            int tmp = statPlayerRecharge(statDay, imq, acc);
            totalRecharge += tmp;

            if (tmp > 0)
            {
                playerCount++;
            }
        }

        if (days == 0 && result != null)
        {
            result.m_newAccRechargePersonNum = playerCount;
        }

        return playerCount;
    }

    int statPlayerRecharge(ChannelInfo cinfo, IMongoQuery cond, string acc)
    {
        IMongoQuery imq = Query.And(cond, Query.EQ(StatLTV.ACC_KEY_1, acc));
        try
        {
            MapReduceResult mapResult = MongodbPayment.Instance.executeMapReduce(cinfo.m_paymentTable,
                                                                            imq,
                                                                            MapReduceTable.getMap("recharge"),
                                                                            MapReduceTable.getReduce("recharge"));
            int total = 0;
            if (mapResult != null)
            {
                IEnumerable<BsonDocument> bson = mapResult.GetResults();
                foreach (BsonDocument d in bson)
                {
                    BsonValue resValue = d["value"];
                    total += resValue["total"].ToInt32();
                }
            }
            return total;
        }
        catch (System.Exception ex)
        {
            LogMgr.log.Error(ex.ToString());
        }

        return 0;
    }

    protected List<Dictionary<string, object>> queryRegeditAcc1(string regTable, ParamStat param, DateTime dt)
    {
        ChannelInfo cinfo = param.m_channel;

        DateTime mint = dt.Date, maxt = dt.Date.AddDays(1);

        IMongoQuery imq1 = Query.LT("time", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("time", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.EQ("channel", BsonValue.Create(cinfo.m_channelNum));

        IMongoQuery imq = Query.And(imq1, imq2, imq3);

        List<Dictionary<string, object>> dataList =
            MongodbAccount.Instance.ExecuteGetListByQuery(regTable, imq, new string[] { ACC_KEY, "time", "channel" });
        /*for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].ContainsKey(ACC_KEY))
            {
                string acc = Convert.ToString(dataList[i][ACC_KEY]);
                m_regeditAccList.Add(acc);
            }
        }*/

        return dataList;
    }

    // 重新存储付费玩家
    private int statXDayTotalRecharge1(ParamStat param, int days, ref int totalRecharge, StatResult result = null)
    {
        totalRecharge = 0;
        m_regeditAccList.Clear();

        ChannelInfo statDay = (ChannelInfo)param.m_channel;

        List<Dictionary<string, object>> accList = queryRegeditAcc1(statDay.m_regeditTable, param, StatBase.getRemainRegTime(statDay, days));

        if (accList.Count == 0) // 没有注册
            return 0;

        List<BsonDocument> rechargeList = new List<BsonDocument>();

        int playerCount = 0;
        IMongoQuery imq = StatBase.getRechargeCond(param, 1);
        foreach (var accData in accList)
        {
            if (accData.ContainsKey(ACC_KEY))
            {
                string acc = Convert.ToString(accData[ACC_KEY]);

                int tmp = statPlayerRecharge(statDay, imq, acc);
                totalRecharge += tmp;

                if (tmp > 0)
                {
                    playerCount++;

                    var bsonDoc = accData.ToBsonDocument();
                    if (bsonDoc.Contains("_id"))
                    {
                        bsonDoc.Remove("_id");
                    }
                    rechargeList.Add(bsonDoc);
                }
            }
        }

        if (days == 0)
        {
            result.m_newAccRechargePersonNum = playerCount;
        }

        if (rechargeList.Count > 0)
        {
            DateTime mint = statDay.m_statDay.AddDays(-1), maxt = statDay.m_statDay;
            IMongoQuery imq1 = Query.LT("time", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("time", BsonValue.Create(mint));
            IMongoQuery imq3 = Query.EQ("channel", statDay.m_channelNum);
            IMongoQuery imqTmp = Query.And(imq1, imq2, imq3);
            MongodbAccount.Instance.ExecuteRemoveByQuery(StatUnitRemain.ACC_RECHARGE, imqTmp);

            MongodbAccount.Instance.ExecuteInsterList(StatUnitRemain.ACC_RECHARGE, rechargeList);
        }
        return playerCount;
    }
}













