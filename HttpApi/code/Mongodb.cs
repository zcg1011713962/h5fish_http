using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

using MongoDB.Driver;

class MongodbLog : MongodbHelper<MongodbLog>
{
    protected override string get_dbname()
    {
        return "LogDB_DWC5";
    }

    protected override string get_url()
    {
        return ConfigurationManager.ConnectionStrings["Mongodb"].ConnectionString;
    }

    protected override void init_table()
    {
    }
}

class MongodbPlayer : MongodbHelper<MongodbPlayer>
{
    protected override string get_dbname()
    {
        return "PlayerDB_DWC5";
    }

    protected override string get_url()
    {
        return ConfigurationManager.ConnectionStrings["Mongodb"].ConnectionString;
    }

    protected override void init_table()
    {
    }
}

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
        createIndex(PayTable.XIANWAN_ACC);
        createIndex(PayTable.DANDANZHUAN_ACC);
        createIndex(PayTable.HULU_ACC);
        createIndex(PayTable.YOUZHUAN_ACC);
        createIndex(PayTable.MAIZIZHUAN_ACC);
        createIndex(PayTable.JUXIANGWAN_ACC);
    }

    void createIndex(string tableName)
    {
        var tmp = mMongodbClient.GetCollection(tableName);

        if (!tmp.IndexExists("acc"))
            tmp.CreateIndex("acc");

        if (!tmp.IndexExists("acc_real"))
            tmp.CreateIndex("acc_real");

        if (!tmp.IndexExists("deviceId"))
            tmp.CreateIndex("deviceId");
    }
}
