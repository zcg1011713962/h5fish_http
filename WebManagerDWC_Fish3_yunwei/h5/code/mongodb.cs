using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

// 游戏数据库
class MongodbGame : MongodbHelper<MongodbGame>
{
    protected override string get_dbname()
    {
        return "GameDB";
    }

    protected override string get_url()
    {
        return ConfigurationManager.ConnectionStrings["dbgame"].ConnectionString;
    }

    protected override void init_table()
    {
        try
        {
            var table = GetDB.GetCollection(TableName.FISHLORD_ACT_BENEFIT_RECV);
            table.CreateIndex(IndexKeys.Ascending("playerId"), IndexOptions.SetBackground(true));
            table.CreateIndex(IndexKeys.Ascending("recvDateTime"), IndexOptions.SetBackground(true));
        }
        catch (System.Exception ex)
        {
        }
    }
}

// 玩家数据库
class MongodbPlayer : MongodbHelper<MongodbPlayer>
{
    protected override string get_dbname()
    {
        return "PlayerDB_DWC";
    }

    protected override string get_url()
    {
        return ConfigurationManager.ConnectionStrings["dbplayer"].ConnectionString;
    }

    protected override void init_table()
    {
        try
        {
            var table = GetDB.GetCollection(TableName.OPERATION_NOTIFY);
            table.CreateIndex(IndexKeys.Ascending("order"), IndexOptions.SetBackground(true));
        }
        catch (System.Exception ex)
        {
        }
    }
}
