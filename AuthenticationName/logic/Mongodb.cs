using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MongoDB.Driver;

class MongodbPayment : MongodbHelper<MongodbPayment>
{
    protected override string get_dbname()
    {
        return "PaymentDB5";
    }

    protected override string get_url()
    {
        string ip = ResMgr.getInstance().getString(JsonCfg.JSON_CONFIG, "mongoPayment");
        return ip;
    }

    protected override void init_table()
    {
    }
}
