using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Web.Configuration;
using System.Text;

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
    public override object getResult(object param)
    {
        return null;
    }

    public override OpRes execute(CMemoryBuffer buf, GMUser user)
    {
        buf.begin();
        int playerId = Convert.ToInt32(buf.Reader.ReadString());
        string score = buf.Reader.ReadString();

        int newScore = 0;//新值
        if (string.IsNullOrEmpty(score))
            return OpRes.op_res_need_at_least_one_cond;
        if (!int.TryParse(score, out newScore))
            return OpRes.op_res_param_not_valid;

        Dictionary<string, object> data = new Dictionary<string, object>();
        int weekMaxScore = 0;

        string[] m_fields = { "weekDimensityHistory" };
        string[] m_playerFields = { "player_id", "nickname", "VipLevel", "is_robot", "sex" };

        IMongoQuery imq = Query.EQ("playerId", playerId);
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        DbInfoParam dip_1 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        Dictionary<string, object> dataList = DBMgr.getInstance().getTableData(TableName.FISHLORD_DRAGON_PALACE_PLAYER, dip, imq, m_fields);
        if (dataList != null)
        {
            if (dataList.ContainsKey("weekDimensityHistory"))
                weekMaxScore = Convert.ToInt32(dataList["weekDimensityHistory"]);

            data.Clear();
            data.Add("weekDimensityHistory", Convert.ToInt32(newScore));
            data.Add("weekTime", DateTime.Now);

            bool res = DBMgr.getInstance().update(TableName.FISHLORD_DRAGON_PALACE_PLAYER, data, "playerId", playerId, user.getDbServerID(), DbName.DB_GAME);
            if (res)//操作LOG 
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_DRAGON_SCALE_NUM_ALTER, new LogDragonScaleNumAlter(playerId, newScore, weekMaxScore), user);

            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }
        else  //若不在该表中
        {
            IMongoQuery imq_1 = Query.EQ("player_id", playerId);
            Dictionary<string, object> playerInfo = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, dip_1, imq_1, m_playerFields);
            if (playerInfo != null)
            {
                data.Clear();
                data.Add("playerId", Convert.ToInt32(playerInfo["player_id"]));
                if (playerInfo.ContainsKey("nickname"))
                    data.Add("nickName", Convert.ToString(playerInfo["nickname"]));

                if (playerInfo.ContainsKey("VipLevel"))
                    data.Add("vipLevel", Convert.ToInt32(playerInfo["VipLevel"]));

                if (playerInfo.ContainsKey("is_robot"))
                    data.Add("isRobot", Convert.ToBoolean(playerInfo["is_robot"]));

                if (playerInfo.ContainsKey("sex"))
                    data.Add("gender", Convert.ToInt32(playerInfo["sex"]));

                data.Add("weekDimensityHistory", Convert.ToInt32(newScore));
                data.Add("weekTime", DateTime.Now);

                bool res = DBMgr.getInstance().insertData(TableName.FISHLORD_DRAGON_PALACE_PLAYER, data, user.getDbServerID(), DbName.DB_GAME);
                if (res)//操作LOG 
                    OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_DRAGON_SCALE_NUM_ALTER, new LogDragonScaleNumAlter(playerId, newScore, weekMaxScore), user);

                return res ? OpRes.opres_success : OpRes.op_res_failed;
            }
            else
            {
                return OpRes.op_res_player_not_exist;
            }
        }
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

        int player_id = 0, player_score = 0;
        if (string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(score))
            return OpRes.op_res_param_not_valid;

        if (!int.TryParse(playerId, out player_id) || !int.TryParse(score, out player_score))
            return OpRes.op_res_param_not_valid;

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        DbInfoParam dip1 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        //查询今天有没有玩游戏
        //IMongoQuery imq = Query.And(Query.EQ("playerId", player_id), Query.EQ("genTime", DateTime.Now.Date));
        //Dictionary<string, object> resPlayer = DBMgr.getInstance().getTableData(TableName.STAT_FISHLORD_MIDDLE_PLAYER, dip, imq);

        ////不存在没有参加活动
        //if (resPlayer == null || resPlayer.Count == 0)
        //    return OpRes.op_res_not_join_match;

        //如果参加了活动
        Dictionary<string, object> dataCheatList = DBMgr.getInstance().getTableData(TableName.FISHLORD_ROOM, dip, imq, m_fields);
       
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Clear();
        data.Add("check_id", player_id);
        data.Add("check_type", type);
        data.Add("check_value", player_score);

        Dictionary<string, object> dataCheat = new Dictionary<string, object>();
        //如果之前没有作弊记录
        if (dataCheatList == null || dataCheatList.Count == 0)
        {
            dataCheat.Add("CheatCheck", new object[] {data});
        }
        else
        {
            object[] da = (object[])dataCheatList["CheatCheck"];

            List<object> dada = new List<object>(da);
            dada.Add(data);

            object[] da_arr = dada.ToArray();

            dataCheat.Add("CheatCheck",da_arr);
        }

        bool res = DBMgr.getInstance().update(TableName.FISHLORD_ROOM, dataCheat, "room_id", 2, user.getDbServerID(), DbName.DB_GAME);
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

        //如果参加了活动
        Dictionary<string, object> dataCheatList = DBMgr.getInstance().getTableData(TableName.FISHLORD_ROOM, dip, imq, m_fields);

        Dictionary<string, object> data = new Dictionary<string, object>();
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

        bool res = DBMgr.getInstance().update(TableName.FISHLORD_ROOM, dataCheat, "room_id", 3, user.getDbServerID(), DbName.DB_GAME);
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
            IMongoQuery imq1 = Query.EQ("player_id", player_id);
            Dictionary<string, object> playerInfo = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, dip1, imq1, m_fields);
            if (playerInfo == null || playerInfo.Count == 0)
                return OpRes.op_res_player_not_exist;

            string nickName = "";
            if (playerInfo.ContainsKey("nickname"))
                nickName = Convert.ToString(playerInfo["nickname"]);
            int vipLevel = 0;
            if (playerInfo.ContainsKey("VipLevel"))
                vipLevel = Convert.ToInt32(playerInfo["VipLevel"]);

            data.Add("playerId", player_id);
            data.Add("nickName", nickName);
            data.Add("vip", vipLevel);
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

        string torpedoMaxCoin = "copperTorpedoMaxCoin";
        switch (type)
        {
            case 1: torpedoMaxCoin = "copperTorpedoMaxCoin"; break;
            case 2: torpedoMaxCoin = "sliverTorpedoMaxCoin"; break;
            case 3: torpedoMaxCoin = "goldenTorpedoMaxCoin"; break;
            case 4: torpedoMaxCoin = "diamondTorpedoMaxCoin"; break;
        }

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Clear();
        //不存在没有参加活动
        bool res = false;
        if (resPlayer == null || resPlayer.Count == 0)
        {
            IMongoQuery imq1 = Query.EQ("player_id", player_id);
            Dictionary<string, object> playerInfo = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, dip1, imq1, m_fields);
            if (playerInfo == null || playerInfo.Count == 0)
                return OpRes.op_res_player_not_exist;

            string nickName = "";
            if(playerInfo.ContainsKey("nickname"))
                nickName = Convert.ToString(playerInfo["nickname"]);
            int vipLevel = 0;
            if(playerInfo.ContainsKey("VipLevel"))
                vipLevel = Convert.ToInt32(playerInfo["VipLevel"]);

            data.Add("playerId", player_id);
            data.Add("nickName", nickName);
            data.Add("vipLevel", vipLevel);
            data.Add(torpedoMaxCoin, player_score);
            string week = DateTime.Now.DayOfWeek.ToString();
            if (week == "Tuesday" || week == "Thursday" || week == "Saturday")
            {
                data.Add("curRankType", 1);
            }
            else {
                data.Add("curRankType", 0);
            }

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