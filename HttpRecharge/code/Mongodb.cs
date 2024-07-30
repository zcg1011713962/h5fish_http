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
