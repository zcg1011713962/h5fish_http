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
//�����������������
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
            {  //����LOG
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

        int newScore = 0;//��ֵ
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
        if (res)//����LOG 
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_JINQIU_NATIONALDAY_ACT_ALTER, new LogJinQiuNationalDayActAlter(playerId, newScore, oldScore), user);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

}

//ħʯ��������
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

        int newScore = 0;//��ֵ
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
        
        }else  //�����ڸñ���
        {
            if (playerId >= 10099001 && playerId <= 10099200)  //������
            {
                if(string.IsNullOrEmpty(nickName))
                    return OpRes.op_res_param_not_valid;

                data.Add("playerId", playerId);
                data.Add("nickName", nickName);
                data.Add("isRobot", true);

            } else  //�ǻ�����
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
        if (res)//����LOG 
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_DRAGON_SCALE_NUM_ALTER, new LogDragonScaleNumAlter(playerId, newScore, weekMaxScore), user);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
    

    public int getCurWeekCount()
    {
        // 604800 һ�ܵ�����
        // 316800 ����1970.1.1��������
        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0);
        return (int)(((long)ts.TotalSeconds - 316800) / 604800);
    } 
}

//���ｵ��
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

        int newScore = 0, player_id = 0;//��ֵ
        if (string.IsNullOrEmpty(score) || string.IsNullOrEmpty(playerId))
            return OpRes.op_res_need_at_least_one_cond;
        if (!int.TryParse(score, out newScore) || !int.TryParse(playerId, out player_id))
            return OpRes.op_res_param_not_valid;

        IMongoQuery imq = Query.EQ("playerId", playerId);
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        DbInfoParam dip_1 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        Dictionary<string, object> data = new Dictionary<string, object>();
        
        if (player_id >= 10099001 && player_id <= 10099200) //������
        {
            if (string.IsNullOrEmpty(nickName))
                return OpRes.op_res_param_not_valid;

            data.Add("nickName", nickName);
        }
        else //�ǻ�����
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
        } ////���

        bool res = false;
        string table = "", scoreName = "", modifyTime = "modifyTime";
        IMongoQuery imq0 = null;
        switch(op)
        {
            case 0://���ｵ��
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
        if (res)//����LOG 
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FISHLORD_LEGENDARY_RANK_CHEAT, new LogFishlordLegendaryRankScoreAlter(player_id, newScore, op), user);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}

//ʥ�޳���һ���
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

        int newScore = 0, player_id = 0;//��ֵ
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
        if (dataList1 == null) //���ڵ�ǰ���а�
        {
            if (player_id >= 10099001 && player_id <= 10099200)
            { //������
                if(string.IsNullOrEmpty(nickName))
                    return OpRes.op_res_param_not_valid;

                data.Add("nickName", nickName);
            } else 
            {
                //��ѯ��ұ�
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
        if (res)//����LOG 
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FISHLORD_MYTHICAL_PLAYER_CHEAT, new LogFishlordMythicalRankScoreAlter(player_id, newScore), user);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}

//�������÷ֵ���
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
                newScore = Convert.ToInt32(score);//��ֵ
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
                    bool flag = false; //�޸���־ʱ��־��false ֻ�޸�todayMaxScore true todayMaxScore��weekMaxScore���޸ģ�
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
                    {  //����LOG
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
//�ں�÷���ڰ���������
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
        int op = buf.Reader.ReadInt32();   //0��ʾ����   1��ʾɾ��

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
        if (!res) // 1��������     2�����ڣ�type Ϊ0   1   2��
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Clear();
            data.Add("player_id",player_id);
            data.Add("type", Convert.ToInt32(type));
            bool res_1 = DBMgr.getInstance().insertData(TableName.SHCD_CARD_SPECIL_LIST, data, user.getDbServerID(), DbName.DB_GAME);
            if (res_1)
            {  //����LOG
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.shcd,player_id, type, 0), user);
            }
        }
        else  //����Ѿ�����
        {
            if (op == 0)
            {
                IMongoQuery imq = Query.EQ("player_id", player_id);
                string[] m_fields = { "type" };

                Dictionary<string, object> dataList = DBMgr.getInstance().getTableData(TableName.SHCD_CARD_SPECIL_LIST, dip, imq, m_fields);
                if (dataList != null)
                {
                    int specilList_type = specilList_type = Convert.ToInt32(dataList["type"]);

                    if (specilList_type == 0)//��ǰ��ɾ��״̬ ��Ϊ����1 ��2
                    {
                        Dictionary<string, object> data = new Dictionary<string, object>();
                        data.Clear();
                        data.Add("type", Convert.ToInt32(type));
                        bool res_2 = DBMgr.getInstance().update(TableName.SHCD_CARD_SPECIL_LIST, data, "player_id", player_id, user.getDbServerID(), DbName.DB_GAME);
                        if (res_2)
                        {  //����LOG
                            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.shcd,player_id, type, 0), user);
                        }
                    }
                    else if (specilList_type != type)
                    {
                        return OpRes.op_res_not_exist_in_two_list;  //������ͬʱ�ںڰ�������
                    }
                }
                else
                {
                    return OpRes.op_res_not_found_data;
                }
            }
            else  //ɾ��
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Clear();
                data.Add("type", 0);
                bool res_2 = DBMgr.getInstance().update(TableName.SHCD_CARD_SPECIL_LIST, data, "player_id", player_id, user.getDbServerID(), DbName.DB_GAME);
                if (res_2)
                {  //����LOG
                    OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.shcd,player_id, 0, 1), user);
                }
            }
            
        }
        return OpRes.opres_success;
         
    }
}
///////////////////////////////////////////////////////////////////////////
//ţţ�ڰ���������
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
        int op = buf.Reader.ReadInt32();   //0��ʾ����   1��ʾɾ��

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
            if (!res) //������ ֱ�Ӳ��� ���� �����ںڻ��ǰ�����
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Clear();
                data.Add("playerId", player_id);
                data.Add("type", Convert.ToInt32(type));
                bool res_1 = DBMgr.getInstance().insertData(TableName.COW_CARD_SPECIL_LIST, data, user.getDbServerID(), DbName.DB_GAME);
                if (res_1)
                {  //����LOG
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
            {  //����LOG
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.cows, player_id, 0, 1), user);
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }
        
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////
//������ڰ���������
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
        int op = buf.Reader.ReadInt32();   //0��ʾ����   1��ʾɾ��

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
            if (!res) //������ ֱ�Ӳ��� ���� �����ںڻ��ǰ�����
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Clear();
                data.Add("playerId", player_id);
                data.Add("type", Convert.ToInt32(type));
                bool res_1 = DBMgr.getInstance().insertData(TableName.CROCODILE_WB_LIST, data, user.getDbServerID(), DbName.DB_GAME);
                if (res_1)
                {  //����LOG
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
            {  //����LOG
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.crocodile, player_id, 0, 1), user);
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        return OpRes.opres_success;
    }
}

//���۱����ڰ���������
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
        int op = buf.Reader.ReadInt32();   //0��ʾ����   1��ʾɾ��

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
            if (!res) //������ ֱ�Ӳ��� ���� �����ںڻ��ǰ�����
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Clear();
                data.Add("playerId", player_id);
                data.Add("type", Convert.ToInt32(type));
                bool res_1 = DBMgr.getInstance().insertData(TableName.Bz_WB_LIST, data, user.getDbServerID(), DbName.DB_GAME);
                if (res_1)
                {  //����LOG
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
            {  //����LOG
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.bz, player_id, 0, 1), user);
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        return OpRes.opres_success;
    }
}

//С��Ϸ��������
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
        int op = buf.Reader.ReadInt32();//0���� 1ɾ��
        if(op!=1)
        {
            item.m_isCloseAll = buf.Reader.ReadInt32() == 0 ? false : true;
            item.m_condGameLevel = buf.Reader.ReadInt32();
            item.m_condVipLevel = buf.Reader.ReadInt32();
        }
        //DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        if (op == 0)
        {
            if (item.m_channelNo != "")  //��������
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Clear();
                data.Add("channel", item.m_channelNo);
                data.Add("condGameLevel", item.m_condGameLevel);
                data.Add("isCloseAll", item.m_isCloseAll);
                data.Add("condVipLevel", item.m_condVipLevel);
                updateOrInsertData(data,user,item);  //������߸�������
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
        if (!res) //������ ֱ�Ӳ���
        {
            bool res_1 = DBMgr.getInstance().insertData(TableName.CHANNEL_OPEN_CLOSE_GAME, data, user.getDbServerID(), DbName.DB_PLAYER);
            if (res_1)
            {  //����LOG
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_CHANNEL_OPEN_CLOSE_GAME,
                    new LogChannelOpenCloseGameSet(0,param.m_channelNo, param.m_isCloseAll, param.m_condGameLevel, param.m_condVipLevel), user);
            }
        }
        else  //�����޸�
        {
            IMongoQuery imq = Query.EQ("channel", param.m_channelNo);
            data.Remove("channel");
            Dictionary<string, object> dataList = DBMgr.getInstance().getTableData(TableName.CHANNEL_OPEN_CLOSE_GAME, user.getDbServerID(), DbName.DB_PLAYER, imq);
            if (dataList != null && dataList.Count != 0)
            {
                res = DBMgr.getInstance().update(TableName.CHANNEL_OPEN_CLOSE_GAME, data, "channel", param.m_channelNo, user.getDbServerID(), DbName.DB_PLAYER);
                if (res)
                {  //����LOG
                    OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_CHANNEL_OPEN_CLOSE_GAME,
                      new LogChannelOpenCloseGameSet(0,param.m_channelNo, param.m_isCloseAll, param.m_condGameLevel, param.m_condVipLevel), user);
                }
            }
        }
    }
}

//����ڻ��һ�ɱ��������
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
            {  //����LOG
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_ND_ACT_FISHCOUNT_RESET, new LogNdActPlayerFishCountReset(player_id, fishCount), user);
            }
        }

        return OpRes.opres_success;
    }
}
//���ᱦ���ɱ��������޸�
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

//��ʥ�ڻ�Ϲϻ�ȡ�����޸�
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
            {  //����LOG
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_HALLOWMAS_PUMPKINCOUNT_RESET, new LogHallowmasPumpkinCountReset(player_id, pumpkinCount), user);
            }
        }

        return OpRes.opres_success;
    }
}

//ˮ�����ڰ���������
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
        int op = buf.Reader.ReadInt32();   //0��ʾ����   1��ʾɾ��

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
            if (!res) //������ ֱ�Ӳ��� ���� �����ںڻ��ǰ�����
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Clear();
                data.Add("playerId", player_id);
                data.Add("type", Convert.ToInt32(type));
                bool res_1 = DBMgr.getInstance().insertData(TableName.FRUIT_BW_LIST, data, user.getDbServerID(), DbName.DB_GAME);
                if (res_1)
                {  //����LOG
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
            {  //����LOG
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SHCD_CARDS_SPECIL_LIST, new LogShcdCardsSpecilList((int)GameId.fruit, player_id, 0, 1), user);
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        return OpRes.opres_success;
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////
//�м�����һ����޸�
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

        //��ѯ������û������Ϸ
        //IMongoQuery imq = Query.And(Query.EQ("playerId", player_id), Query.EQ("genTime", DateTime.Now.Date));
        //Dictionary<string, object> resPlayer = DBMgr.getInstance().getTableData(TableName.STAT_FISHLORD_MIDDLE_PLAYER, dip, imq);

        ////������û�вμӻ
        //if (resPlayer == null || resPlayer.Count == 0)
        //    return OpRes.op_res_not_join_match;

        bool res = false;
        Dictionary<string, object> data = new Dictionary<string, object>();

        if (player_id >= 10099001 && player_id <= 10099200)  //������
        {
            IMongoQuery imq_player = Query.And(Query.EQ("playerId", player_id), Query.EQ("genTime", DateTime.Now.Date));

            Dictionary<string, object> playerInScore = DBMgr.getInstance().getTableData(
                TableName.STAT_FISHLORD_MIDDLE_PLAYER, dip, imq_player, new string[] { "playerId", "genTime" });

            if (playerInScore == null || playerInScore.Count == 0) //δ�μ���Ϸ
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
            else //�μ�
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
            //����μ��˻
            Dictionary<string, object> dataCheatList = DBMgr.getInstance().getTableData(TableName.FISHLORD_ROOM, dip, imq, m_fields);

            data.Clear();
            data.Add("check_id", player_id);
            data.Add("check_type", type);
            data.Add("check_value", player_score);

            Dictionary<string, object> dataCheat = new Dictionary<string, object>();
            //���֮ǰû�����׼�¼
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

       if (res)  //����LOG
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FISHLORD_ROOM_CHEAT, new LogFishlordRoomCheat(2, player_id, type, player_score), user);
        
        return OpRes.opres_success;
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////
//�߼�����һ����޸�
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

        //��ѯ������û������Ϸ
        //IMongoQuery imq = Query.And(Query.EQ("playerId", player_id), Query.EQ("genTime", DateTime.Now.Date));
        //Dictionary<string, object> resPlayer = DBMgr.getInstance().getTableData(TableName.FISHLORD_ADVANCED_ROOM_ACT_RANK_CURR, dip, imq);

        ////������û�вμӻ
        //if (resPlayer == null || resPlayer.Count == 0)
        //    return OpRes.op_res_not_join_match;

         bool res = false;
        Dictionary<string, object> data = new Dictionary<string, object>();

        if (player_id >= 10099001 && player_id <= 10099200)  //������
        {
            IMongoQuery imq_player = Query.And(Query.EQ("playerId", player_id), Query.EQ("genTime", DateTime.Now.Date));

            Dictionary<string, object> playerInScore = DBMgr.getInstance().getTableData(
                TableName.FISHLORD_ADVANCED_ROOM_ACT_RANK_CURR, dip, imq_player, new string[] { "playerId", "genTime" });

            if (playerInScore == null || playerInScore.Count == 0) //δ�μ���Ϸ
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
            else //�μ�
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
            //����μ��˻
            Dictionary<string, object> dataCheatList = DBMgr.getInstance().getTableData(TableName.FISHLORD_ROOM, dip, imq, m_fields);

            data.Clear();
            data.Add("check_id", player_id);
            data.Add("check_value", player_score);

            Dictionary<string, object> dataCheat = new Dictionary<string, object>();
            //���֮ǰû�����׼�¼
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
        if (res)  //����LOG
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FISHLORD_ROOM_CHEAT, new LogFishlordRoomCheat(3, player_id, 1, player_score), user);

        return OpRes.opres_success;
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////
//���賡��һ����޸�
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

        //��ѯ������û������Ϸ
        IMongoQuery imq = Query.And(Query.EQ("playerId", player_id), Query.EQ("genTime", DateTime.Now.Date));
        Dictionary<string, object> resPlayer = DBMgr.getInstance().getTableData(TableName.STAT_FISHLORD_ARMED_SHARK_PLAYER, dip, imq);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Clear();

        data.Add("dailyBasePoints", player_score);
        data.Add("pointModifyTime", DateTime.Now);
        //������û�вμӻ
        bool res = false;
        if (resPlayer == null || resPlayer.Count == 0)
        {
            //������
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

        if (res)  //����LOG
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FISHLORD_ROOM_CHEAT, new LogFishlordRoomCheat(6, player_id, 0, player_score), user);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////
//ը����԰��һ����޸�
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

        //��ѯ������û������Ϸ
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
            default: 
                return OpRes.op_res_failed;
        }

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Clear();
        //������û�вμӻ
        bool res = false;
        if (resPlayer == null || resPlayer.Count == 0)
        {
            string nickName = "";

            //������
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

        if (res)  //����LOG
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_BULLET_HEAD_PLAYER_SCORE_ALTER, new LogStatBulletHeadPlayerScoreAlter(player_id, type, player_score), user);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////
//���а�����
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
        if (type == 0) //���
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

        //��ѯ�����Ƿ��м�¼
        IMongoQuery imq = Query.And(Query.EQ("genTime", DateTime.Now.Date), Query.EQ("playerId", player_id));
        Dictionary<string, object> resPlayer = DBMgr.getInstance().getTableData(tableName, dip, imq);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Clear();
        
        //û�е����¼
        bool res = false;
        if (resPlayer == null || resPlayer.Count == 0)
        {
            string nickName = "";
            //������
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

        if (res)  //����LOG
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_OPERATION_RANK_PLAYER_SCORE_EDIT, new LogOperationRankPlayerScoreAlter(player_id, type, player_score), user);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////
//���Ͷ��ʱ������
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
        string startTime = buf.Reader.ReadString();
        string endTime = buf.Reader.ReadString();

        //�ж�ʱ���С
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

        bool res = DBMgr.getInstance().keyExists(TableName.ACT_CHANNEL100003, "key", 1, serverId, DbName.DB_ACCOUNT);
        if (res) //�޸�
        {
            res = DBMgr.getInstance().update(TableName.ACT_CHANNEL100003, data, "key", key, serverId, DbName.DB_ACCOUNT);
        }
        else  //����
        {
            data.Add("key", 1);
            res = DBMgr.getInstance().insertData(TableName.ACT_CHANNEL100003, data, serverId, DbName.DB_ACCOUNT);
        }

        if (res)
        {  //����LOG
            string time = time1.ToShortDateString() + " - " + time2.ToShortDateString();
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_OPERATION_AD_100003_ACT_SET, new LogOperationAd100003ActSet(key, time), user);
        }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////
//�޸ĵ��ջ�ȡ��������
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
        if (!res) //������
        {
            //��ȡplayer_info�����ݣ������ڣ�������   �������� ����
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
        else  //���ڣ���ԭ��ֵ�������޸�
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