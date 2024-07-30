using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Web.Configuration;
using System.Text;
using StackExchange.Redis;
using System.Collections.Specialized;

public class CommandMgr : SysBase
{
    CMemoryBuffer m_memBuf = new CMemoryBuffer();

    public CommandMgr()
    {
        m_sysType = SysType.sysTypeCommand;
    }

    public CommandBase createCommand(string cmdName)
    {
        CmdFactoryBase f = CmdFactorySet.getInstance().getFactory(cmdName);
        if (f == null)
            return null;

        return f.createCMD();
    }

    public CMemoryBuffer getMemoryBuffer()
    {
        return m_memBuf;
    }

    public static CMemoryBuffer getMemoryBuffer(GMUser user, bool isBegin = false)
    {
        CommandMgr mgr = user.getCommandMgr();
        CMemoryBuffer buf = mgr.getMemoryBuffer();
        if (isBegin)
        {
            buf.begin();
        }
        return buf;
    }

    public static CommandBase createCommand(string cmdName, GMUser user)
    {
        CommandMgr mgr = user.getCommandMgr();
        return mgr.createCommand(cmdName);
    }

    public static CommandBase processCmd(string cmdName, CMemoryBuffer buf, GMUser user)
    {
        CommandMgr mgr = user.getCommandMgr();
        CommandBase cmd = mgr.createCommand(cmdName);
        if (cmd == null)
            return null;

        buf.begin();
        OpRes res = cmd.execute(buf, user);
        cmd.setOpRes(res);
        return cmd;
    }

    public static CommandBase processCmd(CommandBase cmd, CMemoryBuffer buf, GMUser user)
    {
        if (cmd == null)
            return null;

        buf.begin();
        OpRes res = cmd.execute(buf, user);
        cmd.setOpRes(res);
        return cmd;
    }
}

//////////////////////////////////////////////////////////////////////////
public class CommandBase
{
    protected OpRes m_opRes;

    public virtual object getResult(object param)
    {
        return null;
    }

    public virtual OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        return OpRes.op_res_failed;
    }

    public OpRes getOpRes()
    {
        return m_opRes;
    }

    public void setOpRes(OpRes res)
    {
        m_opRes = res;
    }
}

//////////////////////////////////////////////////////////////////////////
//爆金比赛场参数调整
public class CommandFishlordBaojinAdjust : CommandBase
{
    public override object getResult(object param)
    {
        return null;
    }

    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        int roomId = 11;
        string pumpRate1 = buf.Reader.ReadString();
        double pumpRate=0;
        if (!double.TryParse(pumpRate1, out pumpRate))
        {
            return OpRes.op_res_param_not_valid;
        }
        else
        {
            bool res = false;
            Dictionary<string, object> data = new Dictionary<string, object>();

            data.Clear();
            data.Add("PumpRate", Convert.ToDouble(pumpRate));
            res = DBMgr.getInstance().update(TableName.FISHLORD_ROOM, data, "room_id", roomId, user.getDbServerID(), DbName.DB_GAME);

            if (res)
            {  //操作LOG
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_BAOJIN_PUMP_RATE,
                new LogBaojinPumpRate(roomId, Convert.ToDouble(pumpRate)), user);
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }
    }
}

public class CommandJinQiuNationalDayActAlter : CommandBase 
{
    public override object getResult(object param)
    {
        return null;
    }

    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        int playerId = Convert.ToInt32(buf.Reader.ReadString());
        string score = buf.Reader.ReadString();
        string oldScore = buf.Reader.ReadString();

        int newScore = 0;//新值
        if (string.IsNullOrEmpty(score))
            return OpRes.op_res_need_at_least_one_cond;
        if (!int.TryParse(score, out newScore))
            return OpRes.op_res_param_not_valid;

        if(newScore<0)
            return OpRes.op_res_param_not_valid;

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("gainMoonCakeCount",newScore);
        data.Add("gainTime",DateTime.Now);
        bool res = DBMgr.getInstance().update(TableName.STAT_FISHLORD_NATIONAL_DAY_ACTIVITY_2018, data, 
            "playerId", playerId, user.getDbServerID(), DbName.DB_GAME);
        if (res)//操作LOG 
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_JINQIU_NATIONALDAY_ACT_ALTER, new LogJinQiuNationalDayActAlter(playerId, newScore, oldScore), user);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

}

//魔石数量调整
public class CommandDragonScaleNumAlter : CommandBase
{
    private string[] m_fields = { "weekDimensityHistory" };
    private string[] m_playerFields = { "player_id", "nickname", "VipLevel", "is_robot", "sex" };

    public override object getResult(object param)
    {
        return null;
    }

    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        int playerId = Convert.ToInt32(buf.Reader.ReadString());
        string score = buf.Reader.ReadString();
        string nickName = buf.Reader.ReadString();

        int newScore = 0;//新值
        if (string.IsNullOrEmpty(score))
            return OpRes.op_res_need_at_least_one_cond;
        if (!int.TryParse(score, out newScore))
            return OpRes.op_res_param_not_valid;

        IMongoQuery imq = Query.EQ("playerId", playerId);
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        DbInfoParam dip_1 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        Dictionary<string, object> data = new Dictionary<string, object>();
        int weekMaxScore = 0;
        int weekCount = getCurWeekCount();
        bool res = false;

        data.Clear();
        Dictionary<string, object> dataList = DBMgr.getInstance().getTableData(TableName.FISHLORD_DRAGON_PALACE_PLAYER, dip, imq, m_fields);
        if (dataList != null)
        {
            if (dataList.ContainsKey("weekDimensityHistory"))
                weekMaxScore = Convert.ToInt32(dataList["weekDimensityHistory"]);

            data.Add("weekDimensityHistory", Convert.ToInt32(newScore));
            //data.Add("weekTime", DateTime.Now);
            data.Add("weekCount", weekCount);
            res = DBMgr.getInstance().update(TableName.FISHLORD_DRAGON_PALACE_PLAYER, data, "playerId", playerId, user.getDbServerID(), DbName.DB_GAME);
        
        }else  //若不在该表中
        {
            if (playerId >= 10099001 && playerId <= 10099200)  //机器人
            {
                if(string.IsNullOrEmpty(nickName))
                    return OpRes.op_res_param_not_valid;

                data.Add("playerId", playerId);
                data.Add("nickName", nickName);
                data.Add("isRobot", true);

            } else  //非机器人
            {
                IMongoQuery imq_1 = Query.EQ("player_id", playerId);
                Dictionary<string, object> playerInfo = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, dip_1, imq_1, m_playerFields);
                if (playerInfo == null || playerInfo.Count == 0)
                    return OpRes.op_res_player_not_exist;

                data.Add("playerId", Convert.ToInt32(playerInfo["player_id"]));

                if (playerInfo.ContainsKey("nickname"))
                    data.Add("nickName", Convert.ToString(playerInfo["nickname"]));

                if (playerInfo.ContainsKey("VipLevel"))
                    data.Add("vipLevel", Convert.ToInt32(playerInfo["VipLevel"]));

                if (playerInfo.ContainsKey("is_robot"))
                    data.Add("isRobot", Convert.ToBoolean(playerInfo["is_robot"]));

                if (playerInfo.ContainsKey("sex"))
                    data.Add("gender", Convert.ToInt32(playerInfo["sex"]));
            }

            data.Add("weekDimensityHistory", Convert.ToInt32(newScore));
            //data.Add("weekTime", DateTime.Now);
            data.Add("weekCount", weekCount);

            res = DBMgr.getInstance().insertData(TableName.FISHLORD_DRAGON_PALACE_PLAYER, data, user.getDbServerID(), DbName.DB_GAME);
         }
        if (res)//操作LOG 
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_DRAGON_SCALE_NUM_ALTER, new LogDragonScaleNumAlter(playerId, newScore, weekMaxScore), user);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
    

    public int getCurWeekCount()
    {
        // 604800 一周的秒数
        // 316800 距离1970.1.1周数补齐
        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0);
        return (int)(((long)ts.TotalSeconds - 316800) / 604800);
    } 
}

//巨鲲降世
public class CommandFishlordLegendaryRankControl : CommandBase 
{
    private string[] m_playerFields = { "player_id", "nickname", "VipLevel", "is_robot", "sex" };

    public override object getResult(object param)
    {
        return null;
    }

    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        string playerId = buf.Reader.ReadString();
        int op = buf.Reader.ReadInt32();
        string score = buf.Reader.ReadString();
        string nickName = buf.Reader.ReadString();

        int newScore = 0, player_id = 0;//新值
        if (string.IsNullOrEmpty(score) || string.IsNullOrEmpty(playerId))
            return OpRes.op_res_need_at_least_one_cond;
        if (!int.TryParse(score, out newScore) || !int.TryParse(playerId, out player_id))
            return OpRes.op_res_param_not_valid;

        IMongoQuery imq = Query.EQ("playerId", playerId);
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        DbInfoParam dip_1 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        Dictionary<string, object> data = new Dictionary<string, object>();
        
        if (player_id >= 10099001 && player_id <= 10099200) //机器人
        {
            if (string.IsNullOrEmpty(nickName))
                return OpRes.op_res_param_not_valid;

            data.Add("nickName", nickName);
        }
        else //非机器人
        {
            IMongoQuery imq_player = Query.EQ("player_id", player_id);
            Dictionary<string, object> playerInfo = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, dip_1, imq_player, m_playerFields);
            if (playerInfo == null)
                return OpRes.op_res_player_not_exist;

            if (playerInfo.ContainsKey("nickname"))
                data.Add("nickName", Convert.ToString(playerInfo["nickname"]));

            if (playerInfo.ContainsKey("VipLevel"))
            {
                if (op == 0)
                {
                    data.Add("viplv", Convert.ToInt32(playerInfo["VipLevel"]));
                }
                else
                {
                    data.Add("vipLevel", Convert.ToInt32(playerInfo["VipLevel"]));
                }
            }
        } ////玩家

        bool res = false;
        string table = "", scoreName = "", modifyTime = "modifyTime";
        IMongoQuery imq0 = null;
        switch(op)
        {
            case 0://巨鲲降世
                table = TableName.STAT_FISHLORD_LEGENDARY_FISH_PLAYER;
                imq0 = Query.And(Query.EQ("playerId", player_id), Query.EQ("genTime", DateTime.Now.Date));
                scoreName = "topHatchPoints";
                break;
            case 1:
            case 2:
                if (op == 1)
                {
                    scoreName = "goldHoopTorpedoTopCoin";
                    modifyTime = "topModifyTime";
                }
                else {
                    scoreName = "goldHoopTorpedoMaxCoin";
                    modifyTime = "maxModifyTime";
                }
                table = TableName.BULLET_HEAD_ACTIVITY;
                imq0 = Query.EQ("playerId", player_id);
                break;
        }

        Dictionary<string, object> dataList1 = DBMgr.getInstance().getTableData(table, dip, imq0);
        if (dataList1 == null)
        {
            if (op == 0)
            {
                data.Add("genTime", DateTime.Now.Date);
            }
            data.Add("playerId", player_id);
            data.Add(scoreName, newScore);
            data.Add(modifyTime, DateTime.Now);
            res = DBMgr.getInstance().insertData(table, data, user.getDbServerID(), DbName.DB_GAME);
        }
        else
        {
            data.Add(scoreName, newScore);
            data.Add(modifyTime, DateTime.Now);
            res = DBMgr.getInstance().update(table, data,imq0, user.getDbServerID(), DbName.DB_GAME);
        }
        if (res)//操作LOG 
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FISHLORD_LEGENDARY_RANK_CHEAT, new LogFishlordLegendaryRankScoreAlter(player_id, newScore, op), user);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}

//圣兽场玩家积分
public class CommandFishlordMythicalRankControl : CommandBase 
{
    public override object getResult(object param)
    {
        return null;
    }

    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        string playerId = buf.Reader.ReadString();
        string score = buf.Reader.ReadString();
        string nickName = buf.Reader.ReadString();

        int newScore = 0, player_id = 0;//新值
        if (string.IsNullOrEmpty(score) || string.IsNullOrEmpty(playerId))
            return OpRes.op_res_need_at_least_one_cond;

        if (!int.TryParse(score, out newScore) || !int.TryParse(playerId, out player_id))
            return OpRes.op_res_param_not_valid;

        string[] m_playerFields = { "player_id", "nickname", "VipLevel", "is_robot", "sex" };

        IMongoQuery imq = Query.EQ("playerId", playerId);
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        DbInfoParam dip_1 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        Dictionary<string, object> data = new Dictionary<string, object>();
        bool res = false;

        IMongoQuery imq_rank = Query.And(Query.EQ("playerId", player_id), Query.EQ("genTime", DateTime.Now.Date));
        Dictionary<string, object> dataList1 = DBMgr.getInstance().getTableData(TableName.FISHLORD_MYTHICAL_PLAYER, dip, imq_rank);
        if (dataList1 == null) //不在当前排行榜
        {
            if (player_id >= 10099001 && player_id <= 10099200)
            { //机器人
                if(string.IsNullOrEmpty(nickName))
                    return OpRes.op_res_param_not_valid;

                data.Add("nickName", nickName);
            } else 
            {
                //查询玩家表
                IMongoQuery imq_player = Query.EQ("player_id", player_id);
                Dictionary<string, object> playerInfo = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, dip_1, imq_player, m_playerFields);
                if (playerInfo == null)
                    return OpRes.op_res_player_not_exist;

                if (playerInfo.ContainsKey("nickname"))
                    data.Add("nickName", Convert.ToString(playerInfo["nickname"]));
                if (playerInfo.ContainsKey("VipLevel"))
                    data.Add("viplv", Convert.ToInt32(playerInfo["VipLevel"]));

            }

            data.Add("genTime", DateTime.Now.Date);
            data.Add("playerId", player_id);
            data.Add("totalPoints", newScore);
            data.Add("modifyTime", DateTime.Now);
            res = DBMgr.getInstance().insertData(TableName.FISHLORD_MYTHICAL_PLAYER, data, user.getDbServerID(), DbName.DB_GAME);
        }
        else
        {
            data.Add("totalPoints", newScore);
            data.Add("modifyTime", DateTime.Now);
            res = DBMgr.getInstance().update(TableName.FISHLORD_MYTHICAL_PLAYER, data, imq_rank, user.getDbServerID(), DbName.DB_GAME);
        }
        if (res)//操作LOG 
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FISHLORD_MYTHICAL_PLAYER_CHEAT, new LogFishlordMythicalRankScoreAlter(player_id, newScore), user);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}

//竞技场得分调整
public class CommandFishlordBaojinScoreAlter : CommandBase 
{
    public override object getResult(object param)
    {
        return null;
    }

    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        int playerId = Convert.ToInt32(buf.Reader.ReadString());
        string score = buf.Reader.ReadString();

        int newScore = 0;
        if (!string.IsNullOrEmpty(score))
        {
            if (!int.TryParse(score, out newScore))
            {
                return OpRes.op_res_param_not_valid;
            }
            else
            {
                newScore = Convert.ToInt32(score);//新值
                IMongoQuery imq = Query.EQ("playerId", playerId);
                string[] m_fields = {"todayMaxScore", "weekMaxScore"};
                DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
                Dictionary<string, object> dataList =
                DBMgr.getInstance().getTableData(TableName.FISHLORD_BAOJIN_PLAYER, dip, imq, m_fields);
                if (dataList != null)
                {
                    int todayMaxScore = 0;
                    int weekMaxScore = 0;
                    if (dataList.ContainsKey("todayMaxScore"))
                    {
                        todayMaxScore = Convert.ToInt32(dataList["todayMaxScore"]);
                    }
                    if (dataList.ContainsKey("weekMaxScore"))
                    {
                        weekMaxScore = Convert.ToInt32(dataList["weekMaxScore"]);
                    }

                    bool res = false;
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Clear();
                    bool flag = false; //修改日志时标志（false 只修改todayMaxScore true todayMaxScore和weekMaxScore都修改）
                    data.Add("todayMaxScore", Convert.ToInt32(newScore));
                    data.Add("dailyTime", DateTime.Now);
                    if (newScore > weekMaxScore)
                    {
                        flag = true;
                        data.Add("weekMaxScore", Convert.ToInt32(newScore));
                        data.Add("weekTime", DateTime.Now);
                    }
                    res = DBMgr.getInstance().update(TableName.FISHLORD_BAOJIN_PLAYER, data, "playerId", playerId, user.getDbServerID(), DbName.DB_GAME);
                    if (res)
                    {  //操作LOG
                        OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_BAOJIN_SCORE,new LogBaojinScoreAlter(playerId, newScore,flag), user);
                    }
                    return res ? OpRes.opres_success : OpRes.op_res_failed;
                }
                else 
                {
                    return OpRes.op_res_not_found_data;
                }
            }
        }
        else
        {
            return OpRes.op_res_need_at_least_one_cond;
        }
    }
}

/////////////////////////////////////////////////////////////////////////
//黑红梅方黑白名单设置
public class CommandShcdCardsSpecilListSet : CommandBase 
{
    public override object getResult(object param)
    {
        return null;
    }

    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        string playerId = buf.Reader.ReadString();
        int type = buf.Reader.ReadInt32();
        int op = buf.Reader.ReadInt32();   //0表示设置   1表示删除

        int player_id = 0;
        if (!string.IsNullOrEmpty(playerId))
        {
            if (!int.TryParse(playerId, out player_id))
            {
                return OpRes.op_res_param_not_valid;
            }
        }else
        {
            return OpRes.op_res_param_not_valid;
        }

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

        bool res = DBMgr.getInstance().keyExists(TableName.SHCD_CARD_SPECIL_LIST, "player_id", player_id, dip);
        if (!res) // 1、不存在     2、存在（type 为0   1   2）
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Clear();
            data.Add("player_id",player_id);
            data.Add("type", Convert.ToInt32(type));
            bool res_1 = DBMgr.getInstance().insertData(TableName.SHCD_CARD_SPECIL_LIST, data, user.getDbServerID(), DbName.DB_GAME);
            if (res_1)
            {  //操作LOG
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.shcd,player_id, type, 0), user);
            }
        }
        else  //如果已经存在
        {
            if (op == 0)
            {
                IMongoQuery imq = Query.EQ("player_id", player_id);
                string[] m_fields = { "type" };

                Dictionary<string, object> dataList = DBMgr.getInstance().getTableData(TableName.SHCD_CARD_SPECIL_LIST, dip, imq, m_fields);
                if (dataList != null)
                {
                    int specilList_type = specilList_type = Convert.ToInt32(dataList["type"]);

                    if (specilList_type == 0)//当前是删除状态 改为设置1 或2
                    {
                        Dictionary<string, object> data = new Dictionary<string, object>();
                        data.Clear();
                        data.Add("type", Convert.ToInt32(type));
                        bool res_2 = DBMgr.getInstance().update(TableName.SHCD_CARD_SPECIL_LIST, data, "player_id", player_id, user.getDbServerID(), DbName.DB_GAME);
                        if (res_2)
                        {  //操作LOG
                            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.shcd,player_id, type, 0), user);
                        }
                    }
                    else if (specilList_type != type)
                    {
                        return OpRes.op_res_not_exist_in_two_list;  //不允许同时在黑白名单中
                    }
                }
                else
                {
                    return OpRes.op_res_not_found_data;
                }
            }
            else  //删除
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Clear();
                data.Add("type", 0);
                bool res_2 = DBMgr.getInstance().update(TableName.SHCD_CARD_SPECIL_LIST, data, "player_id", player_id, user.getDbServerID(), DbName.DB_GAME);
                if (res_2)
                {  //操作LOG
                    OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.shcd,player_id, 0, 1), user);
                }
            }
            
        }
        return OpRes.opres_success;
         
    }
}
///////////////////////////////////////////////////////////////////////////
//牛牛黑白名单设置
public class CommandCowCardsSpecilListSet : CommandBase
{
    public override object getResult(object param)
    {
        return null;
    }

    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        string playerId = buf.Reader.ReadString();
        int type = buf.Reader.ReadInt32();
        int op = buf.Reader.ReadInt32();   //0表示设置   1表示删除

        int player_id = 0;
        if (!string.IsNullOrEmpty(playerId))
        {
            if (!int.TryParse(playerId, out player_id))
            {
                return OpRes.op_res_param_not_valid;
            }
        }
        else
        {
            return OpRes.op_res_param_not_valid;
        }

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

        if (op == 0)
        {
            bool res = DBMgr.getInstance().keyExists(TableName.COW_CARD_SPECIL_LIST, "playerId", player_id, dip);
            if (!res) //不存在 直接插入 存在 看是在黑还是白名单
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Clear();
                data.Add("playerId", player_id);
                data.Add("type", Convert.ToInt32(type));
                bool res_1 = DBMgr.getInstance().insertData(TableName.COW_CARD_SPECIL_LIST, data, user.getDbServerID(), DbName.DB_GAME);
                if (res_1)
                {  //操作LOG
                    OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.cows, player_id, type, 0), user);
                }
            }
            else
            {
                IMongoQuery imq = Query.EQ("playerId", player_id);
                string[] m_fields = { "type" };

                Dictionary<string, object> dataList = DBMgr.getInstance().getTableData(TableName.COW_CARD_SPECIL_LIST, dip, imq, m_fields);
                if (Convert.ToInt32(dataList["type"]) == type)
                {
                    return OpRes.op_res_data_duplicate;
                }
                else
                {
                    return OpRes.op_res_not_exist_in_two_list;
                }
            }
        }
        else 
        {
            bool res = DBMgr.getInstance().remove(TableName.COW_CARD_SPECIL_LIST, "playerId", player_id, user.getDbServerID(), DbName.DB_GAME);
            if (res)
            {  //操作LOG
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.cows, player_id, 0, 1), user);
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }
        
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////
//鳄鱼大亨黑白名单设置
public class CommandCrocodileSpecilListSet : CommandBase 
{
    public override object getResult(object param)
    {
        return null;
    }
    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        string playerId = buf.Reader.ReadString();
        int type = buf.Reader.ReadInt32();
        int op = buf.Reader.ReadInt32();   //0表示设置   1表示删除

        int player_id = 0;
        if (!string.IsNullOrEmpty(playerId))
        {
            if (!int.TryParse(playerId, out player_id))
            {
                return OpRes.op_res_param_not_valid;
            }
        }
        else
        {
            return OpRes.op_res_param_not_valid;
        }

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

        if (op == 0)
        {
            bool res = DBMgr.getInstance().keyExists(TableName.CROCODILE_WB_LIST, "playerId", player_id, dip);
            if (!res) //不存在 直接插入 存在 看是在黑还是白名单
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Clear();
                data.Add("playerId", player_id);
                data.Add("type", Convert.ToInt32(type));
                bool res_1 = DBMgr.getInstance().insertData(TableName.CROCODILE_WB_LIST, data, user.getDbServerID(), DbName.DB_GAME);
                if (res_1)
                {  //操作LOG
                    OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.crocodile, player_id, type, 0), user);
                }
            }
            else
            {
                IMongoQuery imq = Query.EQ("playerId", player_id);
                string[] m_fields = { "type" };

                Dictionary<string, object> dataList = DBMgr.getInstance().getTableData(TableName.CROCODILE_WB_LIST, dip, imq, m_fields);
                if (Convert.ToInt32(dataList["type"]) == type)
                {
                    return OpRes.op_res_data_duplicate;
                }
                else
                {
                    return OpRes.op_res_not_exist_in_two_list;
                }
            }
        }
        else
        {
            bool res = DBMgr.getInstance().remove(TableName.CROCODILE_WB_LIST, "playerId", player_id, user.getDbServerID(), DbName.DB_GAME);
            if (res)
            {  //操作LOG
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.crocodile, player_id, 0, 1), user);
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        return OpRes.opres_success;
    }
}

//奔驰宝马黑白名单设置
public class CommandBzSpecilListSet : CommandBase 
{
    public override object getResult(object param)
    {
        return null;
    }
    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        string playerId = buf.Reader.ReadString();
        int type = buf.Reader.ReadInt32();
        int op = buf.Reader.ReadInt32();   //0表示设置   1表示删除

        int player_id = 0;
        if (!string.IsNullOrEmpty(playerId))
        {
            if (!int.TryParse(playerId, out player_id))
            {
                return OpRes.op_res_param_not_valid;
            }
        }
        else
        {
            return OpRes.op_res_param_not_valid;
        }

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

        if (op == 0)
        {
            bool res = DBMgr.getInstance().keyExists(TableName.Bz_WB_LIST, "playerId", player_id, dip);
            if (!res) //不存在 直接插入 存在 看是在黑还是白名单
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Clear();
                data.Add("playerId", player_id);
                data.Add("type", Convert.ToInt32(type));
                bool res_1 = DBMgr.getInstance().insertData(TableName.Bz_WB_LIST, data, user.getDbServerID(), DbName.DB_GAME);
                if (res_1)
                {  //操作LOG
                    OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.bz, player_id, type, 0), user);
                }
            }
            else
            {
                IMongoQuery imq = Query.EQ("playerId", player_id);
                string[] m_fields = { "type" };

                Dictionary<string, object> dataList = DBMgr.getInstance().getTableData(TableName.Bz_WB_LIST, dip, imq, m_fields);
                if (Convert.ToInt32(dataList["type"]) == type)
                {
                    return OpRes.op_res_data_duplicate;
                }
                else
                {
                    return OpRes.op_res_not_exist_in_two_list;
                }
            }
        }
        else
        {
            bool res = DBMgr.getInstance().remove(TableName.Bz_WB_LIST, "playerId", player_id, user.getDbServerID(), DbName.DB_GAME);
            if (res)
            {  //操作LOG
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.bz, player_id, 0, 1), user);
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        return OpRes.opres_success;
    }
}

//小游戏开关设置
public class channelOpenCloseGameItem 
{
    public string m_channelNo;
    public bool m_isCloseAll;
    public int m_condGameLevel;
    public int m_condVipLevel;
}
public class CommandChannelOpenCloseGameSet : CommandBase
{
    public override object getResult(object param)
    {
        return null;
    }

    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();

        channelOpenCloseGameItem item=new channelOpenCloseGameItem();
        item.m_channelNo = buf.Reader.ReadString();
        int op = buf.Reader.ReadInt32();//0设置 1删除
        if(op!=1)
        {
            item.m_isCloseAll = buf.Reader.ReadInt32() == 0 ? false : true;
            item.m_condGameLevel = buf.Reader.ReadInt32();
            item.m_condVipLevel = buf.Reader.ReadInt32();
        }
        //DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        if (op == 0)
        {
            if (item.m_channelNo != "")  //单个渠道
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Clear();
                data.Add("channel", item.m_channelNo);
                data.Add("condGameLevel", item.m_condGameLevel);
                data.Add("isCloseAll", item.m_isCloseAll);
                data.Add("condVipLevel", item.m_condVipLevel);
                updateOrInsertData(data,user,item);  //插入或者更新数据
            }
            else 
            {
                Dictionary<string, TdChannelInfo> cd = TdChannel.getInstance().getAllData();
                foreach (var row in cd.Values)
                {
                    string channel = row.m_channelNo;
                    item.m_channelNo = channel;
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Clear();
                    data.Add("channel", item.m_channelNo);
                    data.Add("condGameLevel", item.m_condGameLevel);
                    data.Add("isCloseAll", item.m_isCloseAll);
                    data.Add("condVipLevel", item.m_condVipLevel);
                    updateOrInsertData(data, user, item);
                }
            }
        }
        else 
        {
            bool res = false;
            if (item.m_channelNo != "")
            {
                 res = DBMgr.getInstance().remove(TableName.CHANNEL_OPEN_CLOSE_GAME, "channel", item.m_channelNo, user.getDbServerID(), DbName.DB_PLAYER);
                if(res)
                {
                    OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_CHANNEL_OPEN_CLOSE_GAME,
                    new LogChannelOpenCloseGameSet(1,item.m_channelNo, true, 0, -1), user);
                }
            }
            
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }
        
        return OpRes.opres_success;
    }

    public void updateOrInsertData(Dictionary<string,object> data,GMUser user,channelOpenCloseGameItem param) 
    {
        bool res = DBMgr.getInstance().keyExists(TableName.CHANNEL_OPEN_CLOSE_GAME, "channel", param.m_channelNo, user.getDbServerID(), DbName.DB_PLAYER);
        if (!res) //不存在 直接插入
        {
            bool res_1 = DBMgr.getInstance().insertData(TableName.CHANNEL_OPEN_CLOSE_GAME, data, user.getDbServerID(), DbName.DB_PLAYER);
            if (res_1)
            {  //操作LOG
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_CHANNEL_OPEN_CLOSE_GAME,
                    new LogChannelOpenCloseGameSet(0,param.m_channelNo, param.m_isCloseAll, param.m_condGameLevel, param.m_condVipLevel), user);
            }
        }
        else  //存在修改
        {
            IMongoQuery imq = Query.EQ("channel", param.m_channelNo);
            data.Remove("channel");
            Dictionary<string, object> dataList = DBMgr.getInstance().getTableData(TableName.CHANNEL_OPEN_CLOSE_GAME, user.getDbServerID(), DbName.DB_PLAYER, imq);
            if (dataList != null && dataList.Count != 0)
            {
                res = DBMgr.getInstance().update(TableName.CHANNEL_OPEN_CLOSE_GAME, data, "channel", param.m_channelNo, user.getDbServerID(), DbName.DB_PLAYER);
                if (res)
                {  //操作LOG
                    OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_CHANNEL_OPEN_CLOSE_GAME,
                      new LogChannelOpenCloseGameSet(0,param.m_channelNo, param.m_isCloseAll, param.m_condGameLevel, param.m_condVipLevel), user);
                }
            }
        }
    }
}

//国庆节活动玩家击杀数量设置
public class CommandNdActPlayerFishCountSet : CommandBase 
{
    public override object getResult(object param)
    {
        return null;
    }
    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        string playerId = buf.Reader.ReadString();
        string killCount = buf.Reader.ReadString();

        int player_id = 0;
        if (!string.IsNullOrEmpty(playerId))
        {
            if (!int.TryParse(playerId, out player_id))
            {
                return OpRes.op_res_param_not_valid;
            }
        }
        else
        {
            return OpRes.op_res_param_not_valid;
        }

        int fishCount = 0;
        if (!string.IsNullOrEmpty(killCount))
        {
            if (!int.TryParse(killCount, out fishCount))
            {
                return OpRes.op_res_param_not_valid;
            }
        }
        else
        {
            return OpRes.op_res_param_not_valid;
        }

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        bool res = DBMgr.getInstance().keyExists(TableName.PUMP_NATIONAL_DAY_ACTIVITY, "playerId", player_id, dip);
        if (!res) 
        {
            return OpRes.op_res_player_not_exist;
        }
        else
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Clear();
            data.Add("fishCount", Convert.ToInt32(fishCount));
            bool res_1 = DBMgr.getInstance().update(TableName.PUMP_NATIONAL_DAY_ACTIVITY, data, "playerId", player_id, user.getDbServerID(), DbName.DB_PLAYER);
            if (res_1)
            {  //操作LOG
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_ND_ACT_FISHCOUNT_RESET, new LogNdActPlayerFishCountReset(player_id, fishCount), user);
            }
        }

        return OpRes.opres_success;
    }
}
//金蟾夺宝活动击杀金蟾数量修改
public class CommandSpittorSnatchKillCountSet : CommandBase 
{
    public override object getResult(object param) 
    {
        return null;
    }
    public override OpRes execute(CMemoryBuffer buf,GMUser user) 
    {
        buf.begin();
        string playerId = buf.Reader.ReadString();
        string count = buf.Reader.ReadString();

        int player_id = 0;
        if (string.IsNullOrEmpty(playerId))
            return OpRes.op_res_param_not_valid;
        if (!int.TryParse(playerId, out player_id))
            return OpRes.op_res_param_not_valid;
       
        int pumpKillCount = 0;
        if (string.IsNullOrEmpty(count))
            return OpRes.op_res_param_not_valid;
        if (!int.TryParse(count, out pumpKillCount))
            return OpRes.op_res_param_not_valid;

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

        bool res = DBMgr.getInstance().keyExists(TableName.FISHLORD_SPITTOR_SNATCH_ACTIVITY, "playerId", player_id, dip);
        if (!res)
            return OpRes.op_res_player_not_exist;
        
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Clear();
        data.Add("killCount", Convert.ToInt32(pumpKillCount));
        bool res_1 = DBMgr.getInstance().update(TableName.FISHLORD_SPITTOR_SNATCH_ACTIVITY, data, 
            "playerId", player_id, user.getDbServerID(), DbName.DB_GAME);
        if (res_1)
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FISHLORD_SPITTOR_SNATCH_KILLCOUNT_SET, 
                new LogSpittorSnatchKillCountReset(player_id, pumpKillCount), user);
        
        return OpRes.opres_success;
    }
}

//万圣节活动南瓜获取数量修改
public class CommandHallowmasActPumpkinCountSet : CommandBase 
{
    public override object getResult(object param)
    {
        return null;
    }
    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        string playerId = buf.Reader.ReadString();
        string count = buf.Reader.ReadString();

        int player_id = 0;
        if (!string.IsNullOrEmpty(playerId))
        {
            if (!int.TryParse(playerId, out player_id))
                return OpRes.op_res_param_not_valid;
        }else
        {
            return OpRes.op_res_param_not_valid;
        }

        int pumpkinCount = 0;
        if (!string.IsNullOrEmpty(count))
        {
            if (!int.TryParse(count, out pumpkinCount))
                return OpRes.op_res_param_not_valid;
        } else
        {
            return OpRes.op_res_param_not_valid;
        }

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        bool res = DBMgr.getInstance().keyExists(TableName.PUMP_HALLOWMAS_ACT_RANK, "playerId", player_id, dip);
        if (!res) 
        {
            return OpRes.op_res_player_not_exist;
        }
        else
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Clear();
            data.Add("pumpkinCount", Convert.ToInt32(pumpkinCount));
            bool res_1 = DBMgr.getInstance().update(TableName.PUMP_HALLOWMAS_ACT_RANK, data, "playerId", player_id, user.getDbServerID(), DbName.DB_PLAYER);
            if (res_1)
            {  //操作LOG
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_HALLOWMAS_PUMPKINCOUNT_RESET, new LogHallowmasPumpkinCountReset(player_id, pumpkinCount), user);
            }
        }

        return OpRes.opres_success;
    }
}

//水果机黑白名单设置
public class CommandFruitSpecilListSet : CommandBase
{
    public override object getResult(object param)
    {
        return null;
    }
    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        string playerId = buf.Reader.ReadString();
        int type = buf.Reader.ReadInt32();
        int op = buf.Reader.ReadInt32();   //0表示设置   1表示删除

        int player_id = 0;
        if (!string.IsNullOrEmpty(playerId))
        {
            if (!int.TryParse(playerId, out player_id))
            {
                return OpRes.op_res_param_not_valid;
            }
        }
        else
        {
            return OpRes.op_res_param_not_valid;
        }

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

        if (op == 0)
        {
            bool res = DBMgr.getInstance().keyExists(TableName.FRUIT_BW_LIST, "playerId", player_id, dip);
            if (!res) //不存在 直接插入 存在 看是在黑还是白名单
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Clear();
                data.Add("playerId", player_id);
                data.Add("type", Convert.ToInt32(type));
                bool res_1 = DBMgr.getInstance().insertData(TableName.FRUIT_BW_LIST, data, user.getDbServerID(), DbName.DB_GAME);
                if (res_1)
                {  //操作LOG
                    OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.fruit, player_id, type, 0), user);
                }
            }
            else
            {
                IMongoQuery imq = Query.EQ("playerId", player_id);
                string[] m_fields = { "type" };

                Dictionary<string, object> dataList = DBMgr.getInstance().getTableData(TableName.FRUIT_BW_LIST, dip, imq, m_fields);
                if (Convert.ToInt32(dataList["type"]) == type)
                {
                    return OpRes.op_res_data_duplicate;
                }
                else
                {
                    return OpRes.op_res_not_exist_in_two_list;
                }
            }
        }
        else
        {
            bool res = DBMgr.getInstance().remove(TableName.FRUIT_BW_LIST, "playerId", player_id, user.getDbServerID(), DbName.DB_GAME);
            if (res)
            {  //操作LOG
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.fruit, player_id, 0, 1), user);
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        return OpRes.opres_success;
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////
//中级场玩家积分修改
public class CommandFishlordMiddleRoomPlayerScoreEdit : CommandBase
{
    IMongoQuery imq = Query.EQ("room_id", 2);
    public string[] m_fields = new string[] { "CheatCheck"};
    public override object getResult(object param)
    {
        return null;
    }
    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        string playerId = buf.Reader.ReadString();
        int type = buf.Reader.ReadInt32();
        string score = buf.Reader.ReadString();
        string nickName = buf.Reader.ReadString();

        int player_id = 0, player_score = 0;
        if (string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(score))
            return OpRes.op_res_param_not_valid;

        if (!int.TryParse(playerId, out player_id) || !int.TryParse(score, out player_score))
            return OpRes.op_res_param_not_valid;

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        //DbInfoParam dip1 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        //查询今天有没有玩游戏
        //IMongoQuery imq = Query.And(Query.EQ("playerId", player_id), Query.EQ("genTime", DateTime.Now.Date));
        //Dictionary<string, object> resPlayer = DBMgr.getInstance().getTableData(TableName.STAT_FISHLORD_MIDDLE_PLAYER, dip, imq);

        ////不存在没有参加活动
        //if (resPlayer == null || resPlayer.Count == 0)
        //    return OpRes.op_res_not_join_match;

        bool res = false;
        Dictionary<string, object> data = new Dictionary<string, object>();

        if (player_id >= 10099001 && player_id <= 10099200)  //机器人
        {
            IMongoQuery imq_player = Query.And(Query.EQ("playerId", player_id), Query.EQ("genTime", DateTime.Now.Date));

            Dictionary<string, object> playerInScore = DBMgr.getInstance().getTableData(
                TableName.STAT_FISHLORD_MIDDLE_PLAYER, dip, imq_player, new string[] { "playerId", "genTime" });

            if (playerInScore == null || playerInScore.Count == 0) //未参加游戏
            {
                if (string.IsNullOrEmpty(nickName))
                    return OpRes.op_res_param_not_valid;

                data.Clear();
                data.Add("playerId", player_id);
                data.Add("nickName", nickName);
                data.Add("isRobot", true);
                if (type == 1)
                {
                    data.Add("points", player_score);
                    data.Add("pointModifyTime", DateTime.Now);
                }
                else
                {
                    data.Add("singleMax", player_score);
                    data.Add("singleModifyTime", DateTime.Now);
                }
                data.Add("genTime", DateTime.Now.Date);

                res = DBMgr.getInstance().insertData(TableName.STAT_FISHLORD_MIDDLE_PLAYER, data, user.getDbServerID(), DbName.DB_GAME);
            }
            else //参加
            {
                if (type == 1)
                {
                    data.Add("points", player_score);
                    data.Add("pointModifyTime", DateTime.Now);
                }
                else
                {
                    data.Add("singleMax", player_score);
                    data.Add("singleModifyTime", DateTime.Now);
                }
                data.Add("genTime", DateTime.Now.Date);

                IMongoQuery imq_update = Query.And(Query.EQ("genTime", DateTime.Now.Date), Query.EQ("playerId", player_id));
                res = DBMgr.getInstance().update(TableName.STAT_FISHLORD_MIDDLE_PLAYER, data, imq_update, user.getDbServerID(), DbName.DB_GAME);
            }
        }
        else 
        {
            //如果参加了活动
            Dictionary<string, object> dataCheatList = DBMgr.getInstance().getTableData(TableName.FISHLORD_ROOM, dip, imq, m_fields);

            data.Clear();
            data.Add("check_id", player_id);
            data.Add("check_type", type);
            data.Add("check_value", player_score);

            Dictionary<string, object> dataCheat = new Dictionary<string, object>();
            //如果之前没有作弊记录
            if (dataCheatList == null || dataCheatList.Count == 0)
            {
                dataCheat.Add("CheatCheck", new object[] { data });
            }
            else
            {
                object[] da = (object[])dataCheatList["CheatCheck"];

                List<object> dada = new List<object>(da);
                dada.Add(data);

                object[] da_arr = dada.ToArray();

                dataCheat.Add("CheatCheck", da_arr);
            }

            res = DBMgr.getInstance().update(TableName.FISHLORD_ROOM, dataCheat, "room_id", 2, user.getDbServerID(), DbName.DB_GAME);
        }

       if (res)  //操作LOG
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FISHLORD_ROOM_CHEAT, new LogFishlordRoomCheat(2, player_id, type, player_score), user);
        
        return OpRes.opres_success;
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////
//高级场玩家积分修改
public class CommandFishlordAdvancedRoomPlayerScoreEdit : CommandBase 
{
    IMongoQuery imq = Query.EQ("room_id", 3);

    public string[] m_fields = new string[] { "CheatCheck" };
    public override object getResult(object param)
    {
        return null;
    }
    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        string playerId = buf.Reader.ReadString();
        string score = buf.Reader.ReadString();
        string nickName = buf.Reader.ReadString();

        int player_id = 0, player_score = 0;
        if (string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(score))
            return OpRes.op_res_param_not_valid;

        if (!int.TryParse(playerId, out player_id) || !int.TryParse(score, out player_score))
            return OpRes.op_res_param_not_valid;

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        DbInfoParam dip1 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        //查询今天有没有玩游戏
        //IMongoQuery imq = Query.And(Query.EQ("playerId", player_id), Query.EQ("genTime", DateTime.Now.Date));
        //Dictionary<string, object> resPlayer = DBMgr.getInstance().getTableData(TableName.FISHLORD_ADVANCED_ROOM_ACT_RANK_CURR, dip, imq);

        ////不存在没有参加活动
        //if (resPlayer == null || resPlayer.Count == 0)
        //    return OpRes.op_res_not_join_match;

         bool res = false;
        Dictionary<string, object> data = new Dictionary<string, object>();

        if (player_id >= 10099001 && player_id <= 10099200)  //机器人
        {
            IMongoQuery imq_player = Query.And(Query.EQ("playerId", player_id), Query.EQ("genTime", DateTime.Now.Date));

            Dictionary<string, object> playerInScore = DBMgr.getInstance().getTableData(
                TableName.FISHLORD_ADVANCED_ROOM_ACT_RANK_CURR, dip, imq_player, new string[] { "playerId", "genTime" });

            if (playerInScore == null || playerInScore.Count == 0) //未参加游戏
            {
                if (string.IsNullOrEmpty(nickName))
                    return OpRes.op_res_param_not_valid;

                data.Clear();
                data.Add("playerId", player_id);
                data.Add("nickName", nickName);
                data.Add("isRobot", true);

                data.Add("points", player_score);
                data.Add("modifyTime", DateTime.Now);

                data.Add("genTime", DateTime.Now.Date);

                res = DBMgr.getInstance().insertData(TableName.FISHLORD_ADVANCED_ROOM_ACT_RANK_CURR, data, user.getDbServerID(), DbName.DB_GAME);
            }
            else //参加
            {
                data.Add("points", player_score);
                data.Add("modifyTime", DateTime.Now);

                data.Add("genTime", DateTime.Now.Date);

                IMongoQuery imq_update = Query.And(Query.EQ("genTime", DateTime.Now.Date), Query.EQ("playerId", player_id));
                res = DBMgr.getInstance().update(TableName.FISHLORD_ADVANCED_ROOM_ACT_RANK_CURR, data, imq_update, user.getDbServerID(), DbName.DB_GAME);
            }
        }
        else
        {
            //如果参加了活动
            Dictionary<string, object> dataCheatList = DBMgr.getInstance().getTableData(TableName.FISHLORD_ROOM, dip, imq, m_fields);

            data.Clear();
            data.Add("check_id", player_id);
            data.Add("check_value", player_score);

            Dictionary<string, object> dataCheat = new Dictionary<string, object>();
            //如果之前没有作弊记录
            if (dataCheatList == null || dataCheatList.Count == 0)
            {
                dataCheat.Add("CheatCheck", new object[] { data });
            }
            else
            {
                object[] da = (object[])dataCheatList["CheatCheck"];

                List<object> dada = new List<object>(da);
                dada.Add(data);

                object[] da_arr = dada.ToArray();

                dataCheat.Add("CheatCheck", da_arr);
            }

            res = DBMgr.getInstance().update(TableName.FISHLORD_ROOM, dataCheat, "room_id", 3, user.getDbServerID(), DbName.DB_GAME);
        }
        if (res)  //操作LOG
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FISHLORD_ROOM_CHEAT, new LogFishlordRoomCheat(3, player_id, 1, player_score), user);

        return OpRes.opres_success;
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////
//巨鲨场玩家积分修改
public class CommandFishlordSharkRoomPlayerScoreEdit : CommandBase 
{
    public string[] m_fields = new string[] { "player_id", "VipLevel", "nickname" };
    public override object getResult(object param)
    {
        return null;
    }
    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        string playerId = buf.Reader.ReadString();
        string score = buf.Reader.ReadString();
        string nickName = buf.Reader.ReadString();

        int player_id = 0, player_score = 0;
        if (string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(score))
            return OpRes.op_res_param_not_valid;

        if (!int.TryParse(playerId, out player_id) || !int.TryParse(score, out player_score))
            return OpRes.op_res_param_not_valid;

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        DbInfoParam dip1 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        //查询今天有没有玩游戏
        IMongoQuery imq = Query.And(Query.EQ("playerId", player_id), Query.EQ("genTime", DateTime.Now.Date));
        Dictionary<string, object> resPlayer = DBMgr.getInstance().getTableData(TableName.STAT_FISHLORD_ARMED_SHARK_PLAYER, dip, imq);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Clear();

        data.Add("dailyBasePoints", player_score);
        data.Add("pointModifyTime", DateTime.Now);
        //不存在没有参加活动
        bool res = false;
        if (resPlayer == null || resPlayer.Count == 0)
        {
            //机器人
            if (player_id >= 10099001 && player_id <= 10099200)
            {
                if(string.IsNullOrEmpty(nickName))
                    return OpRes.op_res_param_not_valid;

                data.Add("playerId", player_id);
                data.Add("nickName", nickName);
            }
            else 
            {
                IMongoQuery imq1 = Query.EQ("player_id", player_id);
                Dictionary<string, object> playerInfo = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, dip1, imq1, m_fields);
                if (playerInfo == null || playerInfo.Count == 0)
                    return OpRes.op_res_player_not_exist;

                if (playerInfo.ContainsKey("nickname"))
                    nickName = Convert.ToString(playerInfo["nickname"]);
                int vipLevel = 0;
                if (playerInfo.ContainsKey("VipLevel"))
                    vipLevel = Convert.ToInt32(playerInfo["VipLevel"]);

                data.Add("playerId", player_id);
                data.Add("nickName", nickName);
                data.Add("vip", vipLevel);
            }

            data.Add("genTime", DateTime.Now.Date);
            data.Add("dailyTotalPoints", player_score);

            res = DBMgr.getInstance().insertData(TableName.STAT_FISHLORD_ARMED_SHARK_PLAYER, data, user.getDbServerID(), DbName.DB_GAME);
        }
        else
        {

            int dailyTopBomb = 0, dailyExPoints = 0;
            if (resPlayer.ContainsKey("dailyTopBomb"))
                dailyTopBomb = Convert.ToInt32(resPlayer["dailyTopBomb"]);

            if (resPlayer.ContainsKey("dailyExPoints"))
                dailyExPoints = Convert.ToInt32(resPlayer["dailyExPoints"]);

            data.Add("dailyTotalPoints", dailyTopBomb + dailyExPoints + player_score);

            IMongoQuery imq_update = Query.And(Query.EQ("genTime", DateTime.Now.Date), Query.EQ("playerId", player_id));
            res = DBMgr.getInstance().update(TableName.STAT_FISHLORD_ARMED_SHARK_PLAYER, data, imq_update, user.getDbServerID(), DbName.DB_GAME);
        }

        if (res)  //操作LOG
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FISHLORD_ROOM_CHEAT, new LogFishlordRoomCheat(6, player_id, 0, player_score), user);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////
//炸弹乐园玩家积分修改
public class CommandBulletHeadPlayerScoreEdit : CommandBase 
{
    public string[] m_fields = new string[] { "player_id", "VipLevel", "nickname" };
    public override object getResult(object param)
    {
        return null;
    }
    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        string playerId = buf.Reader.ReadString();
        int type = buf.Reader.ReadInt32();
        string score = buf.Reader.ReadString();
        string m_nickName = buf.Reader.ReadString();

        int player_id = 0, player_score = 0;
        if (string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(score))
            return OpRes.op_res_param_not_valid;

        if (!int.TryParse(playerId, out player_id) || !int.TryParse(score, out player_score))
            return OpRes.op_res_param_not_valid;

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        DbInfoParam dip1 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        //查询今天有没有玩游戏
        IMongoQuery imq = Query.EQ("playerId", player_id);
        Dictionary<string, object> resPlayer = DBMgr.getInstance().getTableData(TableName.BULLET_HEAD_ACTIVITY, dip, imq);

        string torpedoMaxCoin = "";
        switch (type)
        {
            case 0: torpedoMaxCoin = "normalTorpedoMaxCoin"; break;
            case 1: torpedoMaxCoin = "copperTorpedoMaxCoin"; break;
            case 2: torpedoMaxCoin = "sliverTorpedoMaxCoin"; break;
            case 3: torpedoMaxCoin = "goldenTorpedoMaxCoin"; break;
            case 4: torpedoMaxCoin = "diamondTorpedoMaxCoin"; break;
            case 5: torpedoMaxCoin = "weekCoin"; break;
            default: 
                return OpRes.op_res_failed;
        }

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Clear();
        //不存在没有参加活动
        bool res = false;
        if (resPlayer == null || resPlayer.Count == 0)
        {
            string nickName = "";

            //机器人
            if (player_id >= 10099001 && player_id <= 10099200) 
            {  
                if(string.IsNullOrEmpty(m_nickName))
                    return OpRes.op_res_param_not_valid;

                nickName = m_nickName;
            } else 
            {
                //player_info
                IMongoQuery imq1 = Query.EQ("player_id", player_id);
                Dictionary<string, object> playerInfo = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, dip1, imq1, m_fields);
                if (playerInfo == null || playerInfo.Count == 0)
                    return OpRes.op_res_player_not_exist;

                if (playerInfo.ContainsKey("nickname"))
                    nickName = Convert.ToString(playerInfo["nickname"]);
                int vipLevel = 0;
                if (playerInfo.ContainsKey("VipLevel"))
                    vipLevel = Convert.ToInt32(playerInfo["VipLevel"]);

                data.Add("vipLevel", vipLevel);
            }

            data.Add("nickName", nickName);
            data.Add("playerId", player_id);
            data.Add(torpedoMaxCoin, player_score);
            //string week = DateTime.Now.DayOfWeek.ToString();
            //if (week == "Tuesday" || week == "Thursday" || week == "Saturday")
            //{
            //    data.Add("curRankType", 1);
            //}
            //else {
            //    data.Add("curRankType", 0);
            //}
            data.Add("curRankType", 0);
            res = DBMgr.getInstance().insertData(TableName.BULLET_HEAD_ACTIVITY, data, user.getDbServerID(), DbName.DB_GAME);
        }
        else 
        {
            data.Add(torpedoMaxCoin, player_score);
            res = DBMgr.getInstance().update(TableName.BULLET_HEAD_ACTIVITY, data, "playerId", player_id, user.getDbServerID(), DbName.DB_GAME);
        }

        if (res)  //操作LOG
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_BULLET_HEAD_PLAYER_SCORE_ALTER, new LogStatBulletHeadPlayerScoreAlter(player_id, type, player_score), user);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////
//捕鱼大奖赛排行榜积分修改
public class CommandFishlordGrandPrixActPlayerScoreEdit : CommandBase 
{
    public string[] m_fields = new string[] { "player_id", "VipLevel", "nickname" };

    public static ConnectionMultiplexer redisClient;


    public override object getResult(object param)
    {
        return null;
    }
    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        //连接redis
        //try {
        //    redisClient = ConnectionMultiplexer.Connect(user.m_dbIP + ":6159, defaultDatabase=0, password=qwer1234");
        //} catch (Exception e) {
        //    return OpRes.op_res_failed;
        //}

        //try
        //{
        //    RedisMgr.getInstance().connect(user.m_dbIP + ":6159, defaultDatabase=0, password=qwer1234");
        //}
        //catch (Exception e)
        //{
        //    return OpRes.op_res_failed;
        //}

        buf.begin();
        string playerId = buf.Reader.ReadString();
        int type = buf.Reader.ReadInt32();
        string score = buf.Reader.ReadString();
        string m_nickName = buf.Reader.ReadString();

        int player_id = 0;
        long player_score = 0;
        if (string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(score))
            return OpRes.op_res_param_not_valid;

        if (!int.TryParse(playerId, out player_id) || !long.TryParse(score, out player_score))
            return OpRes.op_res_param_not_valid;

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        DbInfoParam dip1 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        //查询今天有没有玩游戏
        IMongoQuery imq = Query.EQ("playerId", player_id);
        
        Dictionary<string, object> resPlayer = DBMgr.getInstance().getTableData(TableName.STAT_GRAND_PRIX_ACTIVITY, dip, imq);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Clear();
        string param_points = "";
        switch (type)
        {
            case 0:
                data.Add("genTime", DateTime.Now.Date);
                param_points = "dailyPoints"; 
                break;
            case 1: 
                param_points = "seasonPoints"; 
                break;
            default:
                return OpRes.op_res_failed;
        }

        //不存在没有参加活动
        bool res = false;
        if (resPlayer == null || resPlayer.Count == 0)
        {
            string nickName = "";

            //机器人
            if (player_id >= 10099001 && player_id <= 10099200)
            {
                if (string.IsNullOrEmpty(m_nickName))
                    return OpRes.op_res_param_not_valid;

                nickName = m_nickName;
            }
            else
            {
                //player_info
                IMongoQuery imq1 = Query.EQ("player_id", player_id);
                Dictionary<string, object> playerInfo = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, dip1, imq1, m_fields);
                if (playerInfo == null || playerInfo.Count == 0)
                    return OpRes.op_res_player_not_exist;

                if (playerInfo.ContainsKey("nickname"))
                    nickName = Convert.ToString(playerInfo["nickname"]);
                int vipLevel = 0;
                if (playerInfo.ContainsKey("VipLevel"))
                    vipLevel = Convert.ToInt32(playerInfo["VipLevel"]);

                data.Add("vip", vipLevel);
            }

            data.Add("nickName", nickName);
            data.Add("playerId", player_id);
            data.Add(param_points, player_score);

            res = DBMgr.getInstance().insertData(TableName.STAT_GRAND_PRIX_ACTIVITY, data, user.getDbServerID(), DbName.DB_GAME);
            //redis 写入
        }
        else
        {
            data.Add(param_points, player_score);
            res = DBMgr.getInstance().update(TableName.STAT_GRAND_PRIX_ACTIVITY, data, "playerId", player_id, user.getDbServerID(), DbName.DB_GAME);
            //redis 写入
        }

        if (res)
        {
            //redis 写入
            if (type == 0)
            {
                user.getRedisServerGroup().zadd(RedisCC.REDIS_GRAND_PRIX, "rankDaily", player_id.ToString(), player_score);
            }
            else
            {
                user.getRedisServerGroup().zadd(RedisCC.REDIS_GRAND_PRIX, "rankSeason", player_id.ToString(), player_score);
            }

            //操作LOG
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FISHLORD_GRAND_PRIX_ACT_PLAYER_SCORE_ALTER,
                new LogStatFishlordGrandPrixActPlayerScoreAlter(player_id, type, player_score), user);
        }

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////
//排行榜作弊
public class CommandOperationRankPlayerScoreEdit : CommandBase 
{
    public string[] m_fields = new string[] { "player_id", "VipLevel", "nickname" };
    public override object getResult(object param)
    {
        return null;
    }
    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        string playerId = buf.Reader.ReadString();
        int type = buf.Reader.ReadInt32();
        string score = buf.Reader.ReadString();
        string m_nickName = buf.Reader.ReadString();

        int player_id = 0, player_score = 0;
        if (string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(score))
            return OpRes.op_res_param_not_valid;

        if (!int.TryParse(playerId, out player_id) || !int.TryParse(score, out player_score))
            return OpRes.op_res_param_not_valid;

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        DbInfoParam dip1 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        string tableName = "", item = "";
        if (type == 0) //金币
        {
            tableName = TableName.STAT_PUMP_DAILY_GOLD_RANK;
            item ="gold";
        }
        else 
        {
            tableName = TableName.STAT_PUMP_DAILY_ITEM_RANK;
            switch(type){
                case 1: item = "item_24"; break;
                case 2: item = "item_25"; break;
                case 3: item = "item_26"; break;
                case 4: item = "item_27"; break;
            }
        }

        //查询当天是否有记录
        IMongoQuery imq = Query.And(Query.EQ("genTime", DateTime.Now.Date), Query.EQ("playerId", player_id));
        Dictionary<string, object> resPlayer = DBMgr.getInstance().getTableData(tableName, dip, imq);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Clear();
        
        //没有当天记录
        bool res = false;
        if (resPlayer == null || resPlayer.Count == 0)
        {
            string nickName = "";
            //机器人
            if (player_id >= 10099001 && player_id <= 10099200)
            {
                if (string.IsNullOrEmpty(m_nickName))
                    return OpRes.op_res_param_not_valid;

                nickName = m_nickName;
            }
            else
            {
                //player_info
                IMongoQuery imq1 = Query.EQ("player_id", player_id);
                Dictionary<string, object> playerInfo = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, dip1, imq1, m_fields);
                if (playerInfo == null || playerInfo.Count == 0)
                    return OpRes.op_res_player_not_exist;

                if (playerInfo.ContainsKey("nickname"))
                    nickName = Convert.ToString(playerInfo["nickname"]);
            }
            data.Add("genTime", DateTime.Now.Date);
            data.Add("nickName", nickName);
            data.Add("playerId", player_id);
            data.Add(item, player_score);

            res = DBMgr.getInstance().insertData(tableName, data, dip);
        }
        else
        {
            data.Add(item, player_score);
            res = DBMgr.getInstance().update(tableName, data, "playerId", player_id, dip);
        }

        if (res)  //操作LOG
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_OPERATION_RANK_PLAYER_SCORE_EDIT, new LogOperationRankPlayerScoreAlter(player_id, type, player_score), user);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////
//广告投放时间设置
public class CommandOperationAd100003ActEdit : CommandBase 
{
    public override object getResult(object param)
    {
        return null;
    }
    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        int key = buf.Reader.ReadInt32();
        string code = buf.Reader.ReadString();
        string startTime = buf.Reader.ReadString();
        string endTime = buf.Reader.ReadString();

        //期号必须存在
        if (string.IsNullOrEmpty(code))
            return OpRes.op_res_param_not_valid;
        int qihao = 0;
        if (!int.TryParse(code, out qihao))
            return OpRes.op_res_param_not_valid;

        //判断时间大小
        DateTime time1 = Convert.ToDateTime(startTime);
        DateTime time2 = Convert.ToDateTime(endTime);
        if (DateTime.Compare(time1, time2) > 0)
            return OpRes.op_res_param_not_valid;

        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_PAYMENT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("startTime", time1);
        data.Add("endTime", time2);
        data.Add("qihao", qihao);
        bool res = DBMgr.getInstance().keyExists(TableName.ACT_CHANNEL100003, "key", key, serverId, DbName.DB_ACCOUNT);
        if (res) //修改
        {
            res = DBMgr.getInstance().update(TableName.ACT_CHANNEL100003, data, "key", key, serverId, DbName.DB_ACCOUNT);
        }
        else  //新增
        {
            data.Add("key", key);
            res = DBMgr.getInstance().insertData(TableName.ACT_CHANNEL100003, data, serverId, DbName.DB_ACCOUNT);
        }

        if (res)
        {  //操作LOG
            string time = time1.ToShortDateString() + " - " + time2.ToShortDateString();
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_OPERATION_AD_100003_ACT_SET, new LogOperationAd100003ActSet(key, time), user);
        }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////
//修改当日获取龙珠数量
public class CommandKdActRankGaindbEdit : CommandBase 
{
    public override object getResult(object param)
    {
        return null;
    }
    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        string playerId = buf.Reader.ReadString();
        string count = buf.Reader.ReadString();

        int player_id = 0, db_count = 0;
        if (string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(count))
            return OpRes.op_res_param_not_valid;

        if (!int.TryParse(playerId, out player_id) || !int.TryParse(count, out db_count))
            return OpRes.op_res_param_not_valid;

        Dictionary<string, object> data = new Dictionary<string, object>();

        int todayGaindb = db_count, gaindb = db_count;

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        bool res = DBMgr.getInstance().keyExists(TableName.STAT_PUMP_KD_ACTIVITY, "playerId", player_id, dip);
        if (!res) //不存在
        {
            //获取player_info表数据，若存在，则填入   若不存在 返回
            Dictionary<string, object> ret = QueryBase.getPlayerProperty(player_id, user, new string[] { "player_id", "nickname" ,"sex","icon_custom"});
            if (ret == null)
                return OpRes.op_res_player_not_exist;

            data.Clear();
            data.Add("playerId", player_id);

            if (ret.ContainsKey("nickname"))
                data.Add("nickName", Convert.ToString(ret["nickname"]));

            if(ret.ContainsKey("sex"))
                data.Add("gender",Convert.ToInt32(ret["sex"]));

            if(ret.ContainsKey("icon_custom"))
                data.Add("head",Convert.ToString(ret["icon_custom"]));

            data.Add("todayGaindb", todayGaindb);
            data.Add("todayGainTime", DateTime.Now);
            data.Add("gaindb", gaindb);
            DBMgr.getInstance().insertData(TableName.STAT_PUMP_KD_ACTIVITY, data, user.getDbServerID(), DbName.DB_GAME);
        }
        else  //存在，在原有值上增加修改
        {
            IMongoQuery imq = Query.EQ("playerId",player_id);
            string[] fields = new string[] { "todayGaindb","gaindb"};
            Dictionary<string, object> dataList = DBMgr.getInstance().getTableData(TableName.STAT_PUMP_KD_ACTIVITY, dip, imq, fields);
            if (dataList != null && dataList.Count != 0)
            {
                if (dataList.ContainsKey("todayGaindb"))
                    todayGaindb += Convert.ToInt32(dataList["todayGaindb"]);

                if (dataList.ContainsKey("gaindb"))
                    gaindb += Convert.ToInt32(dataList["gaindb"]);
                
            }

            data.Clear();
            data.Add("todayGaindb", todayGaindb);
            data.Add("todayGainTime",DateTime.Now);
            data.Add("gaindb", gaindb);
            DBMgr.getInstance().update(TableName.STAT_PUMP_KD_ACTIVITY,data,"playerId",player_id,user.getDbServerID(),DbName.DB_GAME);
        }
        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////////////
public class RobotCheatBase
{
    public virtual OpRes search(GMUser user, object param) { return OpRes.op_res_failed; }

    public virtual OpRes modify(GMUser user, object param) { return OpRes.op_res_failed; }

    public virtual object getSearchResult() { return null; }

    public static RobotCheatBase create(string robot)
    {
        switch (robot)
        {
            case "RobotArena":
                return new RobotCheatArena();
                break;
            case "RobotIntScoreSend":
                return new RobotCheatIntScore();
                break;
            case "RobotBreakEgg":
                return new RobotCheatBreakEgg();
                break;
            case "RobotSummerDay":
                return new RobotCheatSummerDay();
                break;
            case "RobotDanGrading":
                return new RobotCheatDanGrading();
                break;
            case "RobotArenaFree":
                return new RobotCheatArenaFree();
                break;
            case "RobotDaSheng":
                return new RobotCheatDaSheng();
                break;
            case "RobotKillMonster":
                return new RobotCheatKillMonster();
                break;
            case "RobotKillFireDragon":
                return new RobotCheatKillFireDragon();
                break;
            case "RobotD11":
                return new RobotCheatD11();
                break;
        }

        return null;
    }
}

///////////////////////////////////////////////////////////////////////////////
public class RobotCheatArenaRes
{
    public int m_playerId;
    public string m_nickName = "";
    public long m_dayScore;
    public int m_weekScore;

    public string getJsonStr()
    {
        return JsonHelper.ConvertToStr(this);
    }
}

// 竞技场作弊
public class RobotCheatArena : RobotCheatBase
{
    RobotCheatArenaRes m_data = null;

    public override OpRes search(GMUser user, object param)
    {
        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();

        Dictionary<string, object> d = DBMgr.getInstance().getTableData(TableName.ARENA_MATCH_PLAYER, "player_id", playerId, user.getDbServerID(), DbName.DB_GAME);
        if (d == null)
            return OpRes.op_res_not_found_data;

        m_data = new RobotCheatArenaRes();
        if (d.ContainsKey("player_id"))
        {
            m_data.m_playerId = Convert.ToInt32(d["player_id"]);
        }
        if (d.ContainsKey("nickName"))
        {
            m_data.m_nickName = Convert.ToString(d["nickName"]);
        }
        if (d.ContainsKey("dayPoints"))
        {
            m_data.m_dayScore = Convert.ToInt32(d["dayPoints"]);
        }
        if (d.ContainsKey("weekPoints"))
        {
            m_data.m_weekScore = Convert.ToInt32(d["weekPoints"]);
        }
       
        return OpRes.opres_success; 
    }

    public override OpRes modify(GMUser user, object param)
    {
        Dictionary<string, object> updata = new Dictionary<string, object>();

        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();
        string nickName = buf.Reader.ReadString();
        int scoreDay = buf.Reader.ReadInt32();
        int scoreWeek = buf.Reader.ReadInt32();
        bool isRobot = buf.Reader.ReadBoolean();

        updata.Add("dayPoints", scoreDay);
        updata.Add("weekPoints", scoreWeek);
        IMongoQuery imq = Query.EQ("player_id", playerId);
        bool res = false;

        if (!isRobot) // 不是机器人，若数据库表中不存在，则不能动态添加
        {        
            res = DBMgr.getInstance().keyExists(TableName.ARENA_MATCH_PLAYER, "player_id", playerId, user.getDbServerID(), DbName.DB_GAME);
            if (!res)
                return OpRes.op_res_not_found_data;
        }
        else // 机器人修改昵称
        {
            if (string.IsNullOrEmpty(nickName))
                return OpRes.op_res_param_not_valid;

            bool r = DBMgr.getInstance().keyExists(TableName.ARENA_MATCH_PLAYER, "player_id", playerId, user.getDbServerID(), DbName.DB_GAME);
            if (!r)
            {
                updata.Add("viplv", new Random().Next(1, 4));
            }

            int week = ItemHelp.getCurWeekCount(DateTime.Now);
            updata.Add("week", week);
            updata.Add("genTime", DateTime.Now.Date);
            updata.Add("nickName", nickName);
        }

        res = DBMgr.getInstance().update(TableName.ARENA_MATCH_PLAYER, updata, imq, user.getDbServerID(), DbName.DB_GAME, true);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    public override object getSearchResult() { return m_data; }

}

///////////////////////////////////////////////////////////////////////////////
// 积分送大奖作弊
public class RobotCheatIntScore : RobotCheatBase
{
    RobotCheatArenaRes m_data = null;

    public override OpRes search(GMUser user, object param)
    {
        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();

        Dictionary<string, object> d = DBMgr.getInstance().getTableData(TableName.ACTIVITY_INT_SCORE_RANK_CUR, "playerId", playerId, user.getDbServerID(), DbName.DB_PLAYER);
        if (d == null)
            return OpRes.op_res_not_found_data;

        m_data = new RobotCheatArenaRes();
        if (d.ContainsKey("playerId"))
        {
            m_data.m_playerId = Convert.ToInt32(d["playerId"]);
        }
        if (d.ContainsKey("nickname"))
        {
            m_data.m_nickName = Convert.ToString(d["nickname"]);
        }
        if (d.ContainsKey("rankPoints"))
        {
            m_data.m_dayScore = Convert.ToInt32(d["rankPoints"]);
        }

        return OpRes.opres_success;
    }

    public override OpRes modify(GMUser user, object param)
    {
        Dictionary<string, object> updata = new Dictionary<string, object>();

        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();
        string nickName = buf.Reader.ReadString();
        int scoreDay = buf.Reader.ReadInt32();
        buf.Reader.ReadInt32();
        bool isRobot = buf.Reader.ReadBoolean();

        updata.Add("rankPoints", scoreDay);
        bool res = false;
        IMongoQuery imq = Query.EQ("playerId", playerId);

        if (!isRobot) // 不是机器人，若数据库表中不存在，则不能动态添加
        {
            res = DBMgr.getInstance().keyExists(TableName.ACTIVITY_INT_SCORE_RANK_CUR, "playerId", playerId, user.getDbServerID(), DbName.DB_PLAYER);
            if (!res)
                return OpRes.op_res_not_found_data;
        }
        else // 机器人修改昵称
        {
            if (string.IsNullOrEmpty(nickName))
                return OpRes.op_res_param_not_valid;

            updata.Add("nickname", nickName);
        }

        res = DBMgr.getInstance().update(TableName.ACTIVITY_INT_SCORE_RANK_CUR, updata, imq, user.getDbServerID(), DbName.DB_PLAYER, true);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    public override object getSearchResult() { return m_data; }

}

///////////////////////////////////////////////////////////////////////////////
// 砸蛋作弊
public class RobotCheatBreakEgg : RobotCheatBase
{
    RobotCheatArenaRes m_data = null;

    public override OpRes search(GMUser user, object param)
    {
        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();

        Dictionary<string, object> d = DBMgr.getInstance().getTableData(TableName.ACTIVITY_BREAK_EGG, "playerId", playerId, user.getDbServerID(), DbName.DB_PLAYER);
        if (d == null)
            return OpRes.op_res_not_found_data;

        m_data = new RobotCheatArenaRes();
        if (d.ContainsKey("playerId"))
        {
            m_data.m_playerId = Convert.ToInt32(d["playerId"]);
        }
        if (d.ContainsKey("nickname"))
        {
            m_data.m_nickName = Convert.ToString(d["nickname"]);
        }
        if (d.ContainsKey("score"))
        {
            m_data.m_dayScore = Convert.ToInt32(d["score"]);
        }

        return OpRes.opres_success;
    }

    public override OpRes modify(GMUser user, object param)
    {
        Dictionary<string, object> updata = new Dictionary<string, object>();

        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();
        string nickName = buf.Reader.ReadString();
        int scoreDay = buf.Reader.ReadInt32();
        buf.Reader.ReadInt32();
        bool isRobot = buf.Reader.ReadBoolean();

        updata.Add("score", scoreDay);
        bool res = false;
        IMongoQuery imq = Query.EQ("playerId", playerId);

        if (!isRobot) // 不是机器人，若数据库表中不存在，则不能动态添加
        {
            res = DBMgr.getInstance().keyExists(TableName.ACTIVITY_BREAK_EGG, "playerId", playerId, user.getDbServerID(), DbName.DB_PLAYER);
            if (!res)
                return OpRes.op_res_not_found_data;
        }
        else // 机器人修改昵称
        {
            if (string.IsNullOrEmpty(nickName))
                return OpRes.op_res_param_not_valid;

            updata.Add("nickname", nickName);
        }

        res = DBMgr.getInstance().update(TableName.ACTIVITY_BREAK_EGG, updata, imq, user.getDbServerID(), DbName.DB_PLAYER, true);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    public override object getSearchResult() { return m_data; }

}

///////////////////////////////////////////////////////////////////////////////
// 夏日狂欢作弊
public class RobotCheatSummerDay : RobotCheatBase
{
    RobotCheatArenaRes m_data = null;

    public override OpRes search(GMUser user, object param)
    {
        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();

        Dictionary<string, object> d = DBMgr.getInstance().getTableData(TableName.ACTIVITY_SUMMERDAY, "playerId", playerId, user.getDbServerID(), DbName.DB_PLAYER);
        if (d == null)
            return OpRes.op_res_not_found_data;

        m_data = new RobotCheatArenaRes();
        if (d.ContainsKey("playerId"))
        {
            m_data.m_playerId = Convert.ToInt32(d["playerId"]);
        }
        if (d.ContainsKey("nickname"))
        {
            m_data.m_nickName = Convert.ToString(d["nickname"]);
        }
        if (d.ContainsKey("score"))
        {
            m_data.m_dayScore = Convert.ToInt32(d["score"]);
        }

        return OpRes.opres_success;
    }

    public override OpRes modify(GMUser user, object param)
    {
        Dictionary<string, object> updata = new Dictionary<string, object>();

        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();
        string nickName = buf.Reader.ReadString();
        int scoreDay = buf.Reader.ReadInt32();
        buf.Reader.ReadInt32();
        bool isRobot = buf.Reader.ReadBoolean();

        updata.Add("score", scoreDay);
        bool res = false;
        IMongoQuery imq = Query.EQ("playerId", playerId);

        if (!isRobot) // 不是机器人，若数据库表中不存在，则不能动态添加
        {
            res = DBMgr.getInstance().keyExists(TableName.ACTIVITY_SUMMERDAY, "playerId", playerId, user.getDbServerID(), DbName.DB_PLAYER);
            if (!res)
                return OpRes.op_res_not_found_data;
        }
        else // 机器人修改昵称
        {
            if (string.IsNullOrEmpty(nickName))
                return OpRes.op_res_param_not_valid;

            bool r = DBMgr.getInstance().keyExists(TableName.ACTIVITY_SUMMERDAY, "playerId", playerId, user.getDbServerID(), DbName.DB_PLAYER);
            if (!r)
            {
                updata.Add("viplv", new Random().Next(1, 4));
            }

            updata.Add("nickname", nickName);
        }

        res = DBMgr.getInstance().update(TableName.ACTIVITY_SUMMERDAY, updata, imq, user.getDbServerID(), DbName.DB_PLAYER, true);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    public override object getSearchResult() { return m_data; }
}

///////////////////////////////////////////////////////////////////////////////
// 段位赛作弊
public class RobotCheatDanGrading : RobotCheatBase
{
    RobotCheatArenaRes m_data = null;

    public override OpRes search(GMUser user, object param)
    {
        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();

        Dictionary<string, object> d = DBMgr.getInstance().getTableData(TableName.ACTIVITY_DAN_GRADING, "playerId", playerId, user.getDbServerID(), DbName.DB_GAME);
        if (d == null)
            return OpRes.op_res_not_found_data;

        m_data = new RobotCheatArenaRes();
        if (d.ContainsKey("playerId"))
        {
            m_data.m_playerId = Convert.ToInt32(d["playerId"]);
        }
        if (d.ContainsKey("nickName"))
        {
            m_data.m_nickName = Convert.ToString(d["nickName"]);
        }
        if (d.ContainsKey("score"))
        {
            m_data.m_dayScore = Convert.ToInt64(d["score"]);
        }

        return OpRes.opres_success;
    }

    public override OpRes modify(GMUser user, object param)
    {
        Dictionary<string, object> updata = new Dictionary<string, object>();

        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();
        string nickName = buf.Reader.ReadString();
        long scoreDay = buf.Reader.ReadInt32();
        buf.Reader.ReadInt32();
        bool isRobot = buf.Reader.ReadBoolean();

        updata.Add("score", scoreDay);
        bool res = false;
        IMongoQuery imq = Query.EQ("playerId", playerId);

        if (!isRobot) // 不是机器人，若数据库表中不存在，则不能动态添加
        {
            res = DBMgr.getInstance().keyExists(TableName.ACTIVITY_DAN_GRADING, "playerId", playerId, user.getDbServerID(), DbName.DB_GAME);
            if (!res)
                return OpRes.op_res_not_found_data;
        }
        else // 机器人修改昵称
        {
            if (string.IsNullOrEmpty(nickName))
                return OpRes.op_res_param_not_valid;

            bool r = DBMgr.getInstance().keyExists(TableName.ACTIVITY_DAN_GRADING, "playerId", playerId, user.getDbServerID(), DbName.DB_GAME);
            if (!r)
            {
                updata.Add("vip", new Random().Next(1, 4));
            }

            updata.Add("nickName", nickName);
        }

        res = DBMgr.getInstance().update(TableName.ACTIVITY_DAN_GRADING, updata, imq, user.getDbServerID(), DbName.DB_GAME, true);
        writeRedis(user, playerId, (int)scoreDay);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    public override object getSearchResult() { return m_data; }

    void writeRedis(GMUser user, int playerId, int score)
    {
        try
        {
            DateTime now = DateTime.Now;
            string table = now.Year.ToString() + now.Month.ToString().PadLeft(2, '0');
            user.getRedisServerGroup().zadd(RedisCC.REDIS_QUALIFY_PRIX, table, playerId.ToString(), (double)score);
        }
        catch (Exception ex)
        {
            CLOG.Info(ex.ToString());
        }
    }
}

///////////////////////////////////////////////////////////////////////////////
// 自由赛作弊
public class RobotCheatArenaFree : RobotCheatBase
{
    RobotCheatArenaRes m_data = null;

    public override OpRes search(GMUser user, object param)
    {
        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();
        int roomId = Convert.ToInt32(buf.Reader.ReadString());
        IMongoQuery imq1 = Query.And(Query.EQ("player_id", playerId), Query.EQ("roomId", roomId));

        Dictionary<string, object> d = DBMgr.getInstance().getTableData(TableName.ARENA_FREE_MATCH_PLAYER, user.getDbServerID(), DbName.DB_GAME, imq1);
        if (d == null)
            return OpRes.op_res_not_found_data;

        m_data = new RobotCheatArenaRes();
        if (d.ContainsKey("player_id"))
        {
            m_data.m_playerId = Convert.ToInt32(d["player_id"]);
        }
        if (d.ContainsKey("nickName"))
        {
            m_data.m_nickName = Convert.ToString(d["nickName"]);
        }
        if (d.ContainsKey("rankScore"))
        {
            m_data.m_dayScore = Convert.ToInt32(d["rankScore"]);
        }
        if (d.ContainsKey("roomId"))
        {
            m_data.m_weekScore = Convert.ToInt32(d["roomId"]);
        }
        return OpRes.opres_success;
    }

    public override OpRes modify(GMUser user, object param)
    {
        Dictionary<string, object> updata = new Dictionary<string, object>();

        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();
        string nickName = buf.Reader.ReadString();
        int scoreDay = buf.Reader.ReadInt32();
        int roomId = buf.Reader.ReadInt32();
        bool isRobot = buf.Reader.ReadBoolean();

        updata.Add("rankScore", scoreDay);
        updata.Add("roomId", roomId);
        bool res = false;
        //IMongoQuery imq = Query.EQ("player_id", playerId);
        IMongoQuery imq1 = Query.And(Query.EQ("player_id", playerId), Query.EQ("roomId", roomId));

        if (!isRobot) // 不是机器人，若数据库表中不存在，则不能动态添加
        {
            res = DBMgr.getInstance().keyExists(TableName.ARENA_FREE_MATCH_PLAYER, imq1, user.getDbServerID(), DbName.DB_GAME);
            if (!res)
                return OpRes.op_res_not_found_data;
        }
        else // 机器人修改昵称
        {
            if (string.IsNullOrEmpty(nickName))
                return OpRes.op_res_param_not_valid;

            bool r = DBMgr.getInstance().keyExists(TableName.ARENA_FREE_MATCH_PLAYER, imq1, user.getDbServerID(), DbName.DB_GAME);
            if (!r)
            {
                updata.Add("viplv", new Random().Next(1, 4));
            }

            updata.Add("nickName", nickName);
        }

        res = DBMgr.getInstance().update(TableName.ARENA_FREE_MATCH_PLAYER, updata, imq1, user.getDbServerID(), DbName.DB_GAME, true);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    public override object getSearchResult() { return m_data; }
}

// 大圣场作弊
public class RobotCheatDaSheng : RobotCheatBase
{
    RobotCheatArenaRes m_data = null;

    public override OpRes search(GMUser user, object param)
    {
        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();
        NameValueCollection nc = (NameValueCollection)buf.Param1;
        int type = Convert.ToInt32(nc["other"]);

        string tableName = "";
        if (type == 0) // 日榜
        {
            tableName = TableName.FISHLORD_ADVANCED_DAILY_PLAYER;
        }
        else // 周榜
        {
            tableName = TableName.FISHLORD_ADVANCED_WEEKLY_PLAYER;
        }
        Dictionary<string, object> d = DBMgr.getInstance().getTableData(tableName, "playerId", playerId, user.getDbServerID(), DbName.DB_GAME);
        if (d == null)
            return OpRes.op_res_not_found_data;

        m_data = new RobotCheatArenaRes();
        if (d.ContainsKey("playerId"))
        {
            m_data.m_playerId = Convert.ToInt32(d["playerId"]);
        }
        if (d.ContainsKey("nickName"))
        {
            m_data.m_nickName = Convert.ToString(d["nickName"]);
        }
        if (d.ContainsKey("rankScore"))
        {
            m_data.m_dayScore = Convert.ToInt32(d["rankScore"]);
        }

        return OpRes.opres_success;
    }

    public override OpRes modify(GMUser user, object param)
    {
        Dictionary<string, object> updata = new Dictionary<string, object>();

        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();
        string nickName = buf.Reader.ReadString();
        int scoreDay = buf.Reader.ReadInt32();
        int type = buf.Reader.ReadInt32();
        bool isRobot = buf.Reader.ReadBoolean();

        string tableName = "";
        if (type == 0) // 日榜
        {
            tableName = TableName.FISHLORD_ADVANCED_DAILY_PLAYER;
        }
        else // 周榜
        {
            tableName = TableName.FISHLORD_ADVANCED_WEEKLY_PLAYER;
        }

        updata.Add("rankScore", scoreDay);
        updata.Add("fishScore", 0);
        updata.Add("monkeyScore", scoreDay);
        IMongoQuery imq = Query.EQ("playerId", playerId);
        bool res = false;

        if (!isRobot) // 不是机器人，若数据库表中不存在，则不能动态添加
        {
            res = DBMgr.getInstance().keyExists(tableName, "playerId", playerId, user.getDbServerID(), DbName.DB_GAME);
            if (!res)
                return OpRes.op_res_not_found_data;
        }
        else // 机器人修改昵称
        {
            if (string.IsNullOrEmpty(nickName))
                return OpRes.op_res_param_not_valid;

            bool r = DBMgr.getInstance().keyExists(tableName, "playerId", playerId, user.getDbServerID(), DbName.DB_GAME);
            if (!r)
            {
                updata.Add("viplv", new Random().Next(1, 4));
            }

            if (type == 1) // 修改的是周榜
            {
                int week = ItemHelp.getCurWeekCount(DateTime.Now);
                updata.Add("week", week);
            }
                
            updata.Add("nickName", nickName);
        }

        res = DBMgr.getInstance().update(tableName, updata, imq, user.getDbServerID(), DbName.DB_GAME, true);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    public override object getSearchResult() { return m_data; }

}

///////////////////////////////////////////////////////////////////////////////
// 猎妖作弊
public class RobotCheatKillMonster : RobotCheatBase
{
    RobotCheatArenaRes m_data = null;

    public override OpRes search(GMUser user, object param)
    {
        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();

        Dictionary<string, object> d = DBMgr.getInstance().getTableData(TableName.KILL_MONSTER_ACTIVITY, "playerId", playerId, user.getDbServerID(), DbName.DB_PLAYER);
        if (d == null)
            return OpRes.op_res_not_found_data;

        m_data = new RobotCheatArenaRes();
        if (d.ContainsKey("playerId"))
        {
            m_data.m_playerId = Convert.ToInt32(d["playerId"]);
        }
        if (d.ContainsKey("nickname"))
        {
            m_data.m_nickName = Convert.ToString(d["nickname"]);
        }
        if (d.ContainsKey("harmValue"))
        {
            m_data.m_dayScore = Convert.ToInt32(d["harmValue"]);
        }

        return OpRes.opres_success;
    }

    public override OpRes modify(GMUser user, object param)
    {
        Dictionary<string, object> updata = new Dictionary<string, object>();

        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();
        string nickName = buf.Reader.ReadString();
        int scoreDay = buf.Reader.ReadInt32();
        buf.Reader.ReadInt32();
        bool isRobot = buf.Reader.ReadBoolean();

        updata.Add("harmValue", scoreDay);
        bool res = false;
        IMongoQuery imq = Query.EQ("playerId", playerId);

        if (!isRobot) // 不是机器人，若数据库表中不存在，则不能动态添加
        {
            res = DBMgr.getInstance().keyExists(TableName.KILL_MONSTER_ACTIVITY, "playerId", playerId, user.getDbServerID(), DbName.DB_PLAYER);
            if (!res)
                return OpRes.op_res_not_found_data;
        }
        else // 机器人修改昵称
        {
            if (string.IsNullOrEmpty(nickName))
                return OpRes.op_res_param_not_valid;

            bool r = DBMgr.getInstance().keyExists(TableName.KILL_MONSTER_ACTIVITY, "playerId", playerId, user.getDbServerID(), DbName.DB_PLAYER);
            if (!r)
            {
                updata.Add("viplv", new Random().Next(1, 4));
            }

            updata.Add("nickname", nickName);
        }

        res = DBMgr.getInstance().update(TableName.KILL_MONSTER_ACTIVITY, updata, imq, user.getDbServerID(), DbName.DB_PLAYER, true);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    public override object getSearchResult() { return m_data; }

}

///////////////////////////////////////////////////////////////////////////////
// 追击蟹将作弊
public class RobotCheatKillFireDragon : RobotCheatBase
{
    RobotCheatArenaRes m_data = null;

    public override OpRes search(GMUser user, object param)
    {
        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();

        Dictionary<string, object> d = DBMgr.getInstance().getTableData(TableName.ACTIVITY_KILL_CRAB, "playerId", playerId, user.getDbServerID(), DbName.DB_GAME);
        if (d == null)
            return OpRes.op_res_not_found_data;

        m_data = new RobotCheatArenaRes();
        if (d.ContainsKey("playerId"))
        {
            m_data.m_playerId = Convert.ToInt32(d["playerId"]);
        }
        if (d.ContainsKey("nickName"))
        {
            m_data.m_nickName = Convert.ToString(d["nickName"]);
        }
        if (d.ContainsKey("harmValue"))
        {
            m_data.m_dayScore = Convert.ToInt32(d["harmValue"]);
        }

        return OpRes.opres_success;
    }

    public override OpRes modify(GMUser user, object param)
    {
        Dictionary<string, object> updata = new Dictionary<string, object>();

        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();
        string nickName = buf.Reader.ReadString();
        int scoreDay = buf.Reader.ReadInt32();
        buf.Reader.ReadInt32();
        bool isRobot = buf.Reader.ReadBoolean();

        updata.Add("harmValue", scoreDay);
        updata.Add("harmValueTime", DateTime.Now);
        bool res = false;
        IMongoQuery imq = Query.EQ("playerId", playerId);

        if (!isRobot) // 不是机器人，若数据库表中不存在，则不能动态添加
        {
            res = DBMgr.getInstance().keyExists(TableName.ACTIVITY_KILL_CRAB, "playerId", playerId, user.getDbServerID(), DbName.DB_GAME);
            if (!res)
                return OpRes.op_res_not_found_data;
        }
        else // 机器人修改昵称
        {
            if (string.IsNullOrEmpty(nickName))
                return OpRes.op_res_param_not_valid;

          /*  bool r = DBMgr.getInstance().keyExists(TableName.ACTIVITY_KILL_CRAB, "playerId", playerId, user.getDbServerID(), DbName.DB_GAME);
            if (!r)
            {
                updata.Add("viplv", new Random().Next(1, 4));
            }*/

            updata.Add("nickName", nickName);
        }

        res = DBMgr.getInstance().update(TableName.ACTIVITY_KILL_CRAB, updata, imq, user.getDbServerID(), DbName.DB_GAME, true);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    public override object getSearchResult() { return m_data; }

}

///////////////////////////////////////////////////////////////////////////////
// d11作弊
public class RobotCheatD11 : RobotCheatBase
{
    RobotCheatArenaRes m_data = null;

    public override OpRes search(GMUser user, object param)
    {
        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();

        Dictionary<string, object> d = DBMgr.getInstance().getTableData(TableName.D11_ACT, "player_id", playerId, user.getDbServerID(), DbName.DB_PLAYER);
        if (d == null)
            return OpRes.op_res_not_found_data;

        m_data = new RobotCheatArenaRes();
        if (d.ContainsKey("player_id"))
        {
            m_data.m_playerId = Convert.ToInt32(d["player_id"]);
        }
        if (d.ContainsKey("nickname"))
        {
            m_data.m_nickName = Convert.ToString(d["nickname"]);
        }
        if (d.ContainsKey("lotteryCount"))
        {
            m_data.m_dayScore = Convert.ToInt32(d["lotteryCount"]);
        }

        return OpRes.opres_success;
    }

    public override OpRes modify(GMUser user, object param)
    {
        Dictionary<string, object> updata = new Dictionary<string, object>();

        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int playerId = buf.Reader.ReadInt32();
        string nickName = buf.Reader.ReadString();
        int scoreDay = buf.Reader.ReadInt32();
        buf.Reader.ReadInt32();
        bool isRobot = buf.Reader.ReadBoolean();

        updata.Add("lotteryCount", scoreDay);
       // updata.Add("harmValueTime", DateTime.Now);
        bool res = false;
        IMongoQuery imq = Query.EQ("player_id", playerId);

        if (!isRobot) // 不是机器人，若数据库表中不存在，则不能动态添加
        {
            res = DBMgr.getInstance().keyExists(TableName.D11_ACT, "player_id", playerId, user.getDbServerID(), DbName.DB_PLAYER);
            if (!res)
                return OpRes.op_res_not_found_data;
        }
        else // 机器人修改昵称
        {
            if (string.IsNullOrEmpty(nickName))
                return OpRes.op_res_param_not_valid;

            /*  bool r = DBMgr.getInstance().keyExists(TableName.ACTIVITY_KILL_CRAB, "playerId", playerId, user.getDbServerID(), DbName.DB_GAME);
              if (!r)
              {
                  updata.Add("viplv", new Random().Next(1, 4));
              }*/

            updata.Add("nickname", nickName);
        }

        res = DBMgr.getInstance().update(TableName.D11_ACT, updata, imq, user.getDbServerID(), DbName.DB_PLAYER, true);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    public override object getSearchResult() { return m_data; }

}

///////////////////////////////////////////////////////////////////////////////
// 修改兑换设置
public class ExchangeSetting
{
    Dictionary<string, object> m_data = new Dictionary<string, object>();

    public OpRes search(GMUser user, object param)
    {
        List<Dictionary<string, object>> dlist = DBMgr.getInstance().executeQuery1(TableName.EXCHANGE_SETTING, user.getDbServerID(), DbName.DB_PLAYER);
        if (dlist == null)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dlist.Count; i++)
        {
            Dictionary<string, object> d = dlist[i];
            m_data.Add(Convert.ToString(d["key"]), d["count"]);
        }

        return OpRes.opres_success;
    }

    public OpRes modify(GMUser user, object param)
    {
        Dictionary<string, object> updata = new Dictionary<string, object>();

        CMemoryBuffer buf = (CMemoryBuffer)param;
        buf.begin();
        int id = buf.Reader.ReadInt32();
        int count = buf.Reader.ReadInt32();

        updata.Add("count", count);
        bool res = false;
        IMongoQuery imq = Query.EQ("key", id);

        res = DBMgr.getInstance().update(TableName.EXCHANGE_SETTING, updata, imq, user.getDbServerID(), DbName.DB_PLAYER, true);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    public object getSearchResult() { return m_data; }
}

//////////////////////////////////////////////////////////////////////////////
public class CommandModifyNickName : CommandBase
{
    public override object getResult(object param)
    {
        return null;
    }
    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        string key = buf.Reader.ReadString();
        string name = buf.Reader.ReadString();
        Match m = Regex.Match(key, Exp.SINGLE_NUM);
        if (!m.Success)
            return OpRes.op_res_param_not_valid;

        int playerId = Convert.ToInt32(key);
        bool res = DBMgr.getInstance().keyExists(TableName.PLAYER_INFO, "player_id", playerId, user.getDbServerID(), DbName.DB_PLAYER);
        if (!res)
            return OpRes.op_res_not_found_data;

        res = DBMgr.getInstance().keyExists(TableName.PLAYER_INFO, "nickname", name, user.getDbServerID(), DbName.DB_PLAYER);
        if (res)
            return OpRes.op_res_data_duplicate;

        m = Regex.Match(name, Exp.SINGLE_NUM);
        if (m.Success)
            return OpRes.op_res_param_not_valid;

        Dictionary<string, object> updata = new Dictionary<string, object>();
        updata.Add("nickname", name);
        IMongoQuery imq = Query.EQ("player_id", playerId);
        res = DBMgr.getInstance().update(TableName.PLAYER_INFO, updata, imq, user.getDbServerID(), DbName.DB_PLAYER, true);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}

//////////////////////////////////////////////////////////////////////////////
public class RobotInfoResult
{
   // public string txtRobotId = "";
    public string txtNickName = "";
    public string txtVipLevel = "0";
    public string txtHead = "0";
    public string txtFrameId = "0";
    public string txtGold = "0";
    public string txtItem24 = "0";
    public string txtItem25 = "0";
    public string txtItem26 = "0";
    public string txtItem27 = "0";
}

public class CommandRobotRole : CommandBase
{
    static Dictionary<string, string> s_keyMap = new Dictionary<string, string>();
    static string[] s_fields = { "nickname", "VipLevel", "headid", "frameid" };

    RobotInfoResult m_result = new RobotInfoResult();
    List<ParamItem> m_itemList = new List<ParamItem>();

    static CommandRobotRole()
    {
        s_keyMap.Add("txtRobotId", "player_id");
        s_keyMap.Add("txtNickName", "nickname");
        s_keyMap.Add("txtVipLevel", "VipLevel");
        s_keyMap.Add("txtHead", "headid");
        s_keyMap.Add("txtFrameId", "frameid");
        s_keyMap.Add("txtGold", "gold");
    }

    public override object getResult(object param)
    {
        return m_result;
    }

    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        int op = buf.Reader.ReadInt32();
        int robotId = buf.Reader.ReadInt32();
        if (robotId < DefCC.ROBOT_MIN_ID || robotId > DefCC.ROBOT_MAX_ID)
            return OpRes.op_res_not_robot;

        if (op == DefCC.OP_ADD || op == DefCC.OP_MODIFY)
        {
            return addOrUpdate(op, robotId, buf, user);
        }
        else if(op == DefCC.OP_VIEW)
        {
            Dictionary<string, object> d = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "player_id", robotId, user.getDbServerID(), DbName.DB_PLAYER);
            if (d == null)
                return OpRes.op_res_not_found_data;

            if (!d.ContainsKey("is_robot"))
                return OpRes.op_res_not_robot;

            bool r = Convert.ToBoolean(d["is_robot"]);
            if (!r) return OpRes.op_res_not_robot;

            if (d.ContainsKey("player_id"))
            {
               // m_result.txtRobotId = Convert.ToString(d["player_id"]);
            }
            if (d.ContainsKey("nickname"))
            {
                m_result.txtNickName = Convert.ToString(d["nickname"]);
            }
            if (d.ContainsKey("VipLevel"))
            {
                m_result.txtVipLevel = Convert.ToString(d["VipLevel"]);
            }
            if (d.ContainsKey("headid"))
            {
                m_result.txtHead = Convert.ToString(d["headid"]);
            }
            if (d.ContainsKey("frameid"))
            {
                m_result.txtFrameId = Convert.ToString(d["frameid"]);
            }
            if (d.ContainsKey("gold"))
            {
                m_result.txtGold = Convert.ToString(d["gold"]);
            }

            d = DBMgr.getInstance().getTableData(TableName.FISHLORD_PLAYER, "player_id", robotId, user.getDbServerID(), DbName.DB_GAME);
            if (d != null && d.ContainsKey("items"))
            {
                object[] arr = (object[])d["items"];
                for (int k = 0; k < arr.Length; k++)
                {
                    ParamItem item = new ParamItem();
                    Tool.parseItemFromDicNew(arr[k] as Dictionary<string, object>, item);
                    if (item.m_itemId == 24)
                    {
                        m_result.txtItem24 = item.m_itemCount.ToString();
                    }
                    else if (item.m_itemId == 25)
                    {
                        m_result.txtItem25 = item.m_itemCount.ToString();
                    }
                    else if (item.m_itemId == 26)
                    {
                        m_result.txtItem26 = item.m_itemCount.ToString();
                    }
                    else if (item.m_itemId == 27)
                    {
                        m_result.txtItem27 = item.m_itemCount.ToString();
                    }
                }
            }

            return OpRes.opres_success;
        }

        return OpRes.op_res_failed;
    }

    OpRes fromObject(string key, string v, ref object outRes)
    {
        if (key == "txtRobotId" || key == "txtVipLevel" || key == "txtHead" || key == "txtFrameId")
        {
            int i = 0;
            if (int.TryParse(v, out i))
            {    
                outRes = i;
                return OpRes.opres_success;
            }
        }
        else if(key == "txtNickName")
        {
            outRes = v;
            return OpRes.opres_success;
        }
        else if (key == "txtGold")
        {
            long i = 0;
            if (long.TryParse(v, out i))
            {
                outRes = i;
                return OpRes.opres_success;
            }
        }
        else
        {
            return fromObjectItem(key, v);
        }

        return OpRes.op_res_failed;
    }

    OpRes fromObjectItem(string key, string v)
    {
        string KEY_CONST = "txtItem";
        int index = key.IndexOf(KEY_CONST);
        if (index >= 0)
        {
            string s = key.Substring(KEY_CONST.Length);

            int i = 0;
            if (int.TryParse(v, out i))
            {
                ParamItem item = new ParamItem();
                item.m_itemId = Convert.ToInt32(s);
                item.m_itemCount = i;
                m_itemList.Add(item);
                return OpRes.opres_success;
            }
        }

        return OpRes.op_res_failed;
    }

    OpRes addOrUpdate(int op, int robotId, CMemoryBuffer buf, GMUser user)
    {
       // updateItem(robotId, 24, 100, user);
      //  updateItem(robotId, 24, 200, user);

        Dictionary<string, object> dataDest = new Dictionary<string, object>();
        Dictionary<string, object> srcd = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "player_id", robotId, new string[] { "is_robot" }, user.getDbServerID(), DbName.DB_PLAYER);
        if (op == DefCC.OP_MODIFY)
        {
            if (srcd == null)
                return OpRes.op_res_not_found_data;
        }
        if (srcd != null)
        {
            if (!srcd.ContainsKey("is_robot"))
                return OpRes.op_res_not_robot;

            bool r = Convert.ToBoolean(srcd["is_robot"]);
            if (!r) return OpRes.op_res_not_robot;
        }

        string d = (string)buf.Param1;
        Dictionary<string, string> dataSrc = JsonHelper.ParseFromStr<Dictionary<string, string>>(d);

        foreach (var k in dataSrc)
        {
            object v = null;
            OpRes r = fromObject(k.Key, k.Value, ref v);
            if (r != OpRes.opres_success)
                return r;

            if(s_keyMap.ContainsKey(k.Key))
            {
                dataDest.Add(s_keyMap[k.Key], v);
            }
        }
        if(op == DefCC.OP_ADD)
        {
            dataDest.Add("is_robot", true);
        }

        bool res = true;
        IMongoQuery imq = Query.EQ("player_id", robotId);
        if(dataDest.Count > 0)
        {
            res = DBMgr.getInstance().update(TableName.PLAYER_INFO, dataDest, imq, user.getDbServerID(), DbName.DB_PLAYER, true);
        }
        
        foreach (var item in m_itemList)
        {
            updateItem(robotId, item.m_itemId, item.m_itemCount, user);
            updateBulletRankd(robotId, item.m_itemId, item.m_itemCount, user);
        }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    // 更新fishlord表中的道具
    void updateItem(int robotId, int itemId, int itemCount, GMUser user)
    {
        IMongoQuery imq1 = Query.EQ("player_id", robotId);
        IMongoQuery imq2 = Query.EQ("items.item_id", itemId);
        IMongoQuery imq = Query.And(imq1, imq2);

        bool res = DBMgr.getInstance().keyExists(TableName.FISHLORD_PLAYER, imq, user.getDbServerID(), DbName.DB_GAME);
        if(!res)
        {
            UpdateBuilder ub = new UpdateBuilder();
            BsonDocument bd = new BsonDocument();
            bd.Add("item_id", BsonValue.Create(itemId));
            bd.Add("item_count", BsonValue.Create(itemCount));
            ub.AddToSet("items", BsonValue.Create(bd));
            DBMgr.getInstance().update(TableName.FISHLORD_PLAYER, ub, imq1, user.getDbServerID(), DbName.DB_GAME, true);
        }
        else
        {
            UpdateBuilder ub = new UpdateBuilder();
            ub.Set("items.$.item_count", BsonValue.Create(itemCount));

            DBMgr.getInstance().update(TableName.FISHLORD_PLAYER, ub, imq, user.getDbServerID(), DbName.DB_GAME, true);
        }       
    }

    // 修改鱼雷排行
    void updateBulletRankd(int robotId, int itemId, int itemCount, GMUser user)
    {
        var robotData = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "player_id", robotId, s_fields, user.getDbServerID(), DbName.DB_PLAYER);
        if (robotData == null) return;

        IMongoQuery imq = Query.EQ("playerId", robotId);

        Dictionary<string, object> updata = new Dictionary<string, object>();
        if(robotData.ContainsKey("nickname"))
        {
            updata.Add("nickName", Convert.ToString(robotData["nickname"]));
        }
        if (robotData.ContainsKey("VipLevel"))
        {
            updata.Add("VipLevel", Convert.ToInt32(robotData["VipLevel"]));
        }
        if (robotData.ContainsKey("headid"))
        {
            updata.Add("headid", Convert.ToInt32(robotData["headid"]));
        }
        if (robotData.ContainsKey("frameid"))
        {
            updata.Add("frameid", Convert.ToInt32(robotData["frameid"]));
        }
        updata.Add("item_" + itemId.ToString(), itemCount);
        DBMgr.getInstance().update("rankItem", updata, imq, user.getDbServerID(), DbName.DB_GAME, true);
    }
}

