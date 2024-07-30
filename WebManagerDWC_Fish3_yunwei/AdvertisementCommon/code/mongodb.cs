using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;


// 玩家数据库
class MongodbAccount : MongodbHelper<MongodbAccount>
{
    protected override string get_dbname()
    {
        return "AccountDB";
    }

    protected override string get_url()
    {
        return ConfigurationManager.ConnectionStrings["dbaccount"].ConnectionString;
    }

    protected override void init_table()
    {

    }
}
