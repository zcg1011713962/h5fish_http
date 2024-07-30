using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

using MongoDB.Driver;

class MongodbAccount : MongodbHelper<MongodbAccount>
{
    protected override string get_dbname()
    {
        return "AccountDB5";
    }

    protected override string get_url()
    {
        return ConfigurationManager.ConnectionStrings["Mongodb"].ConnectionString;
    }

    protected override void init_table()
    {
        //default
        var tmp = mMongodbClient.GetCollection("AccountTable");

        if (!tmp.IndexExists("acc"))
            tmp.CreateIndex("acc");

        if (!tmp.IndexExists("acc_dev"))
            tmp.CreateIndex("acc_dev");

        if (!tmp.IndexExists("acc_real"))
            tmp.CreateIndex("acc_real");

        if (!tmp.IndexExists("bindPhone"))
            tmp.CreateIndex("bindPhone");

        //////////////////////////////////////////////////////////
        tmp = mMongodbClient.GetCollection(PayTable.XIANWAN_ACC);
        if (!tmp.IndexExists("acc"))
            tmp.CreateIndex("acc");

        if (!tmp.IndexExists("deviceId"))
            tmp.CreateIndex("deviceId");

        if (!tmp.IndexExists("acc_real"))
            tmp.CreateIndex("acc_real");

        addIndex(PayTable.DUOYOU_ACC);
        addIndex(PayTable.JUYAN_ACC);
        addIndex(PayTable.WANYOU_ACC);
        addIndex(PayTable.HULU_ACC);
        addIndex(PayTable.DANDANZHUAN_ACC);
        addIndex(PayTable.YOUZHUAN_ACC);
        addIndex(PayTable.MAIZIZHUAN_ACC);
        addIndex(PayTable.JUXIANGWAN_ACC);
        addIndex(PayTable.XIAOZHUO_ACC);
        addIndex(PayTable.PAOPAOZHUAN_ACC);
        addIndex(PayTable.DDQW_ACC);
        addIndex(PayTable.HUAWEI_ACC);
        addIndex(PayTable.OPPO_ACC);
        addIndex(PayTable.XIAOMI_ACC);
        //////////////////////////////////////////////////////////
        //anysdk
        tmp = mMongodbClient.GetCollection("anysdk_login");

        if (!tmp.IndexExists("acc"))
            tmp.CreateIndex("acc");

        //////////////////////////////////////////////////////////
        //day info
        tmp = mMongodbClient.GetCollection("link_phone");
        if (!tmp.IndexExists("phone"))
            tmp.CreateIndex("phone");

        tmp = mMongodbClient.GetCollection("day_activation");
        if (!tmp.IndexExists("date"))
            tmp.CreateIndex("date");

        tmp = mMongodbClient.GetCollection("day_regedit");
        if (!tmp.IndexExists("date"))
            tmp.CreateIndex("date");

        //////////////////////////////////////////////////////////////////////////
        tmp = mMongodbClient.GetCollection(CONST.DEVICE_MAP_ACC);
        tmp.CreateIndex("deviceId");

        tmp = mMongodbClient.GetCollection(Advertisement.ADVERT_TABLE);
        tmp.CreateIndex("muid");

        //////////////////////////////////////////////////////////////////////////
        tmp = mMongodbClient.GetCollection("channelVerControl");
        if (!tmp.IndexExists("channel"))
            tmp.CreateIndex("channel");

        //////////////////////////////////////////////////////////////////////////
        for (int i = 0; i < LoginTable.PLATFORM_LIST.Length; i++)
        {
            if (!string.IsNullOrEmpty(LoginTable.PLATFORM_LIST[i]))
            {
                tmp = mMongodbClient.GetCollection(LoginTable.getAccountTableByPlatform(LoginTable.PLATFORM_LIST[i]));
                tmp.CreateIndex("acc");
                tmp.CreateIndex("acc_real");
            }
        }
    }

    void addIndex(string tableName)
    {
        var tmp = mMongodbClient.GetCollection(tableName);
        if (!tmp.IndexExists("acc"))
            tmp.CreateIndex("acc");

        if (!tmp.IndexExists("deviceId"))
            tmp.CreateIndex("deviceId");

        if (!tmp.IndexExists("acc_real"))
            tmp.CreateIndex("acc_real");
    }
}

class MongodbConfig : MongodbHelper<MongodbConfig>
{
    protected override string get_dbname()
    {
        return "ConfigDB5";
    }

    protected override string get_url()
    {
        return ConfigurationManager.ConnectionStrings["Mongodb"].ConnectionString;
    }

    protected override void init_table()
    {
        var tmp = mMongodbClient.GetCollection("Versions");
        tmp.CreateIndex("type");

        tmp = mMongodbClient.GetCollection("Errors");
        tmp.CreateIndex("ver");
        tmp.CreateIndex("game");
        tmp.CreateIndex("time");

        tmp = mMongodbClient.GetCollection("TestServers");
        tmp.CreateIndex("channel");
    }
}



class MongodbPayment : MongodbHelper<MongodbPayment>
{
    protected override string get_dbname()
    {
        return "PaymentDB5";
    }

    protected override string get_url()
    {
        return ConfigurationManager.ConnectionStrings["Mongodb"].ConnectionString;
    }

    protected override void init_table()
    {
        //anysdk
        var tmp = mMongodbClient.GetCollection("ex_orderinfo");
        if (!tmp.IndexExists("OrderID"))
            tmp.CreateIndex("OrderID");

        tmp = mMongodbClient.GetCollection(PayTable.BAIDU2_TRANSITION);
        if (!tmp.IndexExists("playerId"))
            tmp.CreateIndex("playerId");

        tmp = mMongodbClient.GetCollection("tthy_ysdk_log");
        if (!tmp.IndexExists("OrderID"))
            tmp.CreateIndex("OrderID");
    }
}
