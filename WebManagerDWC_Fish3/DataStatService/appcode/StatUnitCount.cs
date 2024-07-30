﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Windows.Forms;

// 统计纯数量相关
public class StatUnitCount : StatBase
{
    public override void doStat(object param, StatResult result)
    {
        ParamStat p = (ParamStat)param;
        ChannelInfo cinfo = p.m_channel;
        DateTime mint = cinfo.m_statDay.Date.AddDays(-1), maxt = cinfo.m_statDay.Date;

        statRegeditCount(cinfo, result, mint, maxt);
        statDeviceActivation(cinfo, result, mint, maxt);
        statEnterFishRoom(cinfo, result, mint);
    }

    private void statRegeditCount(ChannelInfo cinfo, StatResult result, DateTime mint, DateTime maxt)
    {
        IMongoQuery imq1 = Query.LT("time", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("time", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.EQ("channel", BsonValue.Create(cinfo.m_channelNum));
        IMongoQuery imq = Query.And(imq1, imq2, imq3);

        result.m_regeditCount = (int)MongodbAccount.Instance.ExecuteGetCount(cinfo.m_regeditTable, imq);
    }

    private void statDeviceActivation(ChannelInfo cinfo, StatResult result, DateTime mint, DateTime maxt)
    {
        IMongoQuery imq1 = Query.LT("active_time", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("active_time", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.EQ("channel", BsonValue.Create(cinfo.m_channelNum));
        IMongoQuery imq = Query.And(imq1, imq2, imq3);

        result.m_deviceActivationCount = (int)MongodbAccount.Instance.ExecuteGetCount(cinfo.m_deviceActivationTable, imq);
    }

    // 统计进入渔场人数
    private void statEnterFishRoom(ChannelInfo cinfo, StatResult result, DateTime mint)
    {
        IMongoQuery imq1 = Query.EQ("genTime", BsonValue.Create(mint));
        IMongoQuery imq2 = Query.EQ("channel", BsonValue.Create(cinfo.m_channelNum));
        IMongoQuery imq = Query.And(imq1, imq2);

        try
        {
            MapReduceResult mapResult = MongodbLog.Instance.executeMapReduce(TableName.STAT_PUMP_ROOM_PLAYER,
                                                                                 imq,
                                                                                 MapReduceTable.getMap("StatEnterFishRoom"),
                                                                                 MapReduceTable.getReduce("StatEnterFishRoom"));
            if (mapResult != null)
            {
                IEnumerable<BsonDocument> bson = mapResult.GetResults();
                foreach (BsonDocument d in bson)
                {
                    BsonValue resValue = d["value"];
                    result.m_enterFishRoomCount = resValue["count"].ToInt32();
                }
            }
        }
        catch (System.Exception ex)
        {	
        }
    }
}















