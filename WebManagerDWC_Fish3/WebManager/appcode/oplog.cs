using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Reflection;

struct LogType
{
    // 发邮件
    public const int LOG_TYPE_SEND_MAIL = 0;

    // 封IP
    public const int LOG_TYPE_BLOCK_IP = 1;

    // 封账号
    public const int LOG_TYPE_BLOCK_ACC = 2;

    // 重置密码
    public const int LOG_TYPE_RESET_PWD = 3;

    // 后台充值
    public const int LOG_TYPE_GM_RECHARGE = 4;

    // 停封玩家ID
    public const int LOG_TYPE_BLOCK_ID = 5;

    // 修改捕鱼盈利率
    public const int LOG_TYPE_MODIFY_FISHLORD_GAIN_RATE = 6;

    // 重置捕鱼盈利率
    public const int LOG_TYPE_RESET_FISHLORD_GAIN_RATE = 7;

    // 删除自定义头像
    public const int LOG_TYPE_DEL_CUSTOM_HEAD = 8;

    // 增加牛牛牌型
    public const int LOG_TYPE_COWS_ADD_CARDS_TYPE = 9;

    // 祝福与诅咒
    public const int LOG_TYPE_WISH_CURSE = 10;

    // 玩家属性操作
    public const int LOG_TYPE_PLAYER_OP = 11;

    // 重载表格
    public const int LOG_TYPE_RELOAD_TABLE = 12;

    //限时活动参数修改
    public const int LOG_TYPE_ACTIVITY_CFG = 13;

    //游戏结果操作 鳄鱼大亨
    public const int LOG_TYPE_GAME_CROCODILE_RESULT = 14;

    //黑红
    public const int LOG_TYPE_GAME_SHCD_RESULT = 15;

    //爆金比赛场抽水比例参数
    public const int LOG_TYPE_BAOJIN_PUMP_RATE = 16;

    //竞技场玩家得分修改
    public const int LOG_TYPE_BAOJIN_SCORE = 17;

    //绑定玩家手机
    public const int LOG_TYPE_BIND_PLAYER_PHONE = 18;

    //修改鳄鱼大亨参数
    public const int LOG_TYPE_MODIFY_CROCODILE_PARAM = 19;

    //奔驰宝马结果控制
    public const int LOG_TYPE_GAME_BZ_RESULT = 20;

    //修改奔驰宝马参数
    public const int LOG_TYPE_MODIFY_BZ_PARAM = 21;

    //黑红黑白名单列表
    public const int LOG_TYPE_SHCD_CARDS_SPECIL_LIST = 22;

    //捕鱼点杀点送
    public const int LOG_TYPE_SCORE_POOL_SET = 23;

    //小游戏开关设置
    public const int LOG_TYPE_CHANNEL_OPEN_CLOSE_GAME = 24;

    //国庆节活动作弊
    public const int LOG_TYPE_ND_ACT_FISHCOUNT_RESET = 25;

    //万圣节活动作弊
    public const int LOG_TYPE_HALLOWMAS_PUMPKINCOUNT_RESET = 26;

    //修改捕鱼弹头统计区间修正系数
    public const int LOG_TYPE_FISHLORD_BULLET_HEAD_RCPARAM_EDIT = 27;

    //修改机器人最高积分
    public const int LOG_TYPE_ROBOT_MAX_SCORE_EDIT = 28;

    //快速开始（游客）添加账号信息
    public const int LOG_TYPE_FASTER_START_FOR_VISITOR = 29;

    //水果机结果控制
    public const int LOG_TYPE_GAME_FRUIT_RESULT = 30;

    //金蟾夺宝活动作弊
    public const int LOG_TYPE_FISHLORD_SPITTOR_SNATCH_KILLCOUNT_SET = 31;

    //客服补单/大户随访/换包福利-操作
    public const int LOG_TYPE_SERVICE_REPAIR_ORDER_OP = 32;

    //玩家龙鳞数量修改
    public const int LOG_TYPE_DRAGON_SCALE_NUM_ALTER = 33;

    //流失大户引导完成添加记录
    public const int LOG_TYPE_GUIDE_LOST_PLAYER = 34;

    //玩家月饼数量修改
    public const int LOG_TYPE_JINQIU_NATIONALDAY_ACT_ALTER = 35;

    //发布系统空投
    public const int LOG_TYPE_AIR_DROP_SYS_PUBLISH = 36;

    //个人后台管理算法参数修改
    public const int LOG_TYPE_MODIFY_FISHLORD_SINGLE_CTRL_PARAM = 37;

    //大水池参数调整
    public const int LOG_TYPE_MODIFY_FISHLORD_CTRL_PARAM = 38;

    //场次作弊 中级场 高级场
    public const int LOG_TYPE_FISHLORD_ROOM_CHEAT = 39;

    //高级场玩法管理
    public const int LOG_TYPE_FISHLORD_ADVANCED_ROOM_CTRL = 40;

    //炸弹乐园玩家积分作弊
    public const int LOG_TYPE_BULLET_HEAD_PLAYER_SCORE_ALTER = 41;

    //巨鲲降世/定海神针作弊
    public const int LOG_TYPE_FISHLORD_LEGENDARY_RANK_CHEAT = 42;

    //修改配置表
    public const int LOG_TYPE_OPERATION_ACTIVITY_CFG = 43;

    //机器人积分管理
    public const int LOG_TYPE_OPERATION_FISHLORD_ROBOT_RANK_CFG = 44;

    //圣兽场玩家积分作弊
    public const int LOG_TYPE_FISHLORD_MYTHICAL_PLAYER_CHEAT = 45;

    //实物审核管理
    public const int LOG_TYPE_EXCHANGE_AUDIT = 46;

    //排行榜作弊
    public const int LOG_TYPE_OPERATION_RANK_PLAYER_SCORE_EDIT = 47;

    //广告投放时间设置
    public const int LOG_TYPE_OPERATION_AD_100003_ACT_SET = 48;

    //捕鱼大奖赛玩家积分作弊
    public const int LOG_TYPE_FISHLORD_GRAND_PRIX_ACT_PLAYER_SCORE_ALTER = 49;

    //玩家基本信息修改
    public const int LOG_TYPE_PLAYER_BASIC_INFO_CHG = 50;

    //新老账号问题
    public const int LOG_TYPE_NEW_OLD_ACC = 51;
}

//////////////////////////////////////////////////////////////////////////

// 各类操作参数的基础
[Serializable]
class OpParam
{
    public OpParam() { }
    // 取得描述串
    public virtual string getDescription(OpInfo info, string str) { return ""; }
    // 取得存储数据库的串
    public virtual string getString() { return ""; }
}
////////////////////////////////////////////////////////////////////////////////
//客服补单
[Serializable]
class LogServiceRepairOrder : OpParam
{
    public int m_op;
    public string m_playerId;
    public int m_repairOrder;
    public int m_repairBonus;
    public string m_operator;
    public string m_comments;

    public LogServiceRepairOrder() { }
    public LogServiceRepairOrder(int op, string playerId, int repairOrder, int repairBonus, string operators, string comments)
    {
        m_op = op;
        m_playerId = playerId;
        m_repairOrder = repairOrder;
        m_repairBonus = repairBonus;
        m_operator = operators;
        m_comments = comments;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogServiceRepairOrder param = BaseJsonSerializer.deserialize<LogServiceRepairOrder>(str);
        if (param != null)
        {
            string strCon = "操作员："+ param.m_operator + " 操作了客服补单/大户随访/换包福利-系统 操作明细：操作原因=> ";
            
            switch(param.m_op){
                case 0: strCon += "补单；"; break;
                case 1: strCon += "换包福利；"; break;
                case 2: strCon += "访问客服福利；"; break;
                case 3: strCon += "大户回流引导；"; break;
            }

            strCon += "补单项目=>";
            if (param.m_repairOrder == -1)
            {
                strCon += "false ；";
            }
            else
            {
                var orderItem = RechargeCFG.getInstance().getValue(param.m_repairOrder);
                if (orderItem != null)
                {
                    strCon += orderItem.m_name+"；";
                }
                else {
                    strCon += " ；";
                }  
            }

            strCon += "补单补贴/客服回访福利=>";
            if (param.m_repairBonus == -1)
            {
                strCon += "false ；";
            }
            else
            {
                var bonusItem = RepairOrderItem.getInstance().getValue(param.m_repairBonus);
                if (bonusItem != null)
                {
                    strCon += bonusItem.m_itemName+"；";
                }
                else {
                    strCon += " ；";
                }
            }

            strCon += "备注=>"+param.m_comments;
            return string.Format(info.m_fmt, strCon);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////////////
//流失大户引导完成添加记录
[Serializable]
class LogGuideLostPlayer : OpParam
{
    public string m_time;
    public string m_playerId;
    public string m_comments;

    public LogGuideLostPlayer() { }
    public LogGuideLostPlayer(string time, string playerId, string comments)
    {
        m_time = time;
        m_playerId = playerId;
        m_comments = comments;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogGuideLostPlayer param = BaseJsonSerializer.deserialize<LogGuideLostPlayer>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_time,param.m_playerId,param.m_comments);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////////////
//游戏水果机结果控制
[Serializable]
class LogGameFruitResult : OpParam
{
    public int m_op;
    public int m_result;
    public string m_playerId;

    public LogGameFruitResult() { }
    public LogGameFruitResult(int op, int result,string playerId)
    {
        m_op = op;
        m_result = result;
        m_playerId = playerId;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogGameFruitResult param = BaseJsonSerializer.deserialize<LogGameFruitResult>(str);
        if (param != null)
        {
            string result = param.m_result.ToString();
            Crocodile_RateCFGData data = Fruit_RateCFG.getInstance().getValue(param.m_result);
            if (data != null)
            {
                result = data.m_name;
            }

            string opName = "";
            switch (param.m_op)
            {
                case 0: opName = "设置"; break;
                case 1: opName = "修改"; break;
            }

            return string.Format(info.m_fmt, opName, param.m_playerId, result);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//快速开始（游客）添加账号信息
[Serializable]
class LogFasterStartForVisitor : OpParam 
{
    public string m_playerId;
    public LogFasterStartForVisitor() { }
    public LogFasterStartForVisitor(string playerId) 
    {
        m_playerId = playerId;
    }
    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";
        LogFasterStartForVisitor param = BaseJsonSerializer.deserialize<LogFasterStartForVisitor>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_playerId);
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//小游戏开关设置
[Serializable]
class LogChannelOpenCloseGameSet : OpParam
{
    public int m_flag;
    public string m_channel;
    public bool m_isCloseAll;
    public int m_gameLevel;
    public int m_vipLevel;

    public LogChannelOpenCloseGameSet() { }

    public LogChannelOpenCloseGameSet(int flag, string channel,bool isCloseAll,int gameLevel,int vipLevel)
    {
        m_flag = flag;
        m_channel = channel;
        m_isCloseAll = isCloseAll;
        m_gameLevel = gameLevel;
        m_vipLevel = vipLevel;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogChannelOpenCloseGameSet param = BaseJsonSerializer.deserialize<LogChannelOpenCloseGameSet>(str);
        if (param != null)
        {
            string channel = "";
            //渠道
            var cd = TdChannel.getInstance().getValue(param.m_channel);
            if (cd != null)
            {
                channel += cd.m_channelName;
            }

            string strNote="";

            if (param.m_flag == 0)
            {
                // 设置了 设置了某某渠道的小游戏开关，状态：整体关闭，开启要求：捕鱼倍率，VIP等级
                strNote += "设置了";
                strNote += channel;
                strNote += "渠道小游戏开关，状态：";

                if (param.m_isCloseAll)
                {
                    strNote += "整体关闭";
                }
                else
                {
                    strNote += "整体开启";
                }

                strNote += "，开启要求：捕鱼倍率：";

                var fl = Fish_LevelCFG.getInstance().getValue(param.m_gameLevel);
                if (fl != null)
                {
                    strNote += Convert.ToString(fl.m_openRate) + "炮";
                }
                if (param.m_gameLevel == 0)
                {
                    strNote += "不设限";
                }

                strNote += "；vip等级：";

                if (param.m_vipLevel == -1)
                {
                    strNote += "不设限";
                }
                else
                {
                    strNote += param.m_vipLevel + "级";
                }
            }
            else 
            {
                strNote += "删除了"+channel+"渠道小游戏开关设置";
            }
            return string.Format(info.m_fmt, strNote);
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// 发邮件
[Serializable]
class LogSendMail : OpParam
{
    public string m_title = "";
    public string m_sender = "";
    public string m_content = "";
    public string m_playerList = "";
    public string m_itemList = "";
    public int m_validDay;

    public LogSendMail() { }
    public LogSendMail(string title, string sender, string content, string playerList, string itemList, int days)
    {
        m_title = title;
        m_sender = sender;
        m_content = content;
        m_playerList = playerList;
        m_itemList = itemList;
        m_validDay = days;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogSendMail param = BaseJsonSerializer.deserialize<LogSendMail>(str);
        if (param != null)
        {
            if (param.m_playerList == "")
            {
                return string.Format(info.m_fmt, "全服", param.m_title, param.m_sender, param.m_content, param.m_itemList, param.m_validDay);
            }
            return string.Format(info.m_fmt, param.m_playerList, param.m_title, param.m_sender, param.m_content, param.m_itemList, param.m_validDay);
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////////
//玩家基本信息修改
[Serializable]
class LogPlayerBasicInfoChg : OpParam
{
    public int m_op;
    public string m_playerId;
    public string m_param;

    public LogPlayerBasicInfoChg() { }
    public LogPlayerBasicInfoChg(int op, string playerId, string param)
    {
        m_op = op;
        m_playerId = playerId;
        m_param = param;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogPlayerBasicInfoChg param = BaseJsonSerializer.deserialize<LogPlayerBasicInfoChg>(str);
        if (param != null)
        {
            string str_chg = "";
            switch (param.m_op)
            {
                case 1: str_chg = "昵修"; break;
                case 2: str_chg = "贡献值"; break;
            }
            string str_info = "将玩家 [" + param.m_playerId + "] 的" + str_chg+ "修改为 [" + param.m_param + "]"; 

            return string.Format(info.m_fmt, str_info);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////////
//高级场玩法管理
[Serializable]
class LogFishlordAdvancedRoomCtrl : OpParam
{
    public int m_op;
    public int m_ratio;
    public int m_maxWinCount;


    public LogFishlordAdvancedRoomCtrl() { }
    public LogFishlordAdvancedRoomCtrl(int op, int ratio, int maxWinCount = 0)
    {
        m_op = op;
        m_ratio = ratio;
        m_maxWinCount = maxWinCount;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogFishlordAdvancedRoomCtrl param = BaseJsonSerializer.deserialize<LogFishlordAdvancedRoomCtrl>(str);
        if (param != null)
        {
            string str_info = "";
            switch(param.m_op)
            {
                case 0: str_info = "将一等奖名额改为 [" + param.m_maxWinCount + "] ， 奖金比例改为 [" + param.m_ratio + "]%"; break;
                case 1: str_info = "将二等奖名额改为 [" + param.m_maxWinCount + "] ， 奖金比例改为 [" + param.m_ratio + "]%"; break;
                case 2: str_info = "将三等奖名额改为 [" + param.m_maxWinCount + "] ， 奖金比例改为 [" + param.m_ratio + "]%"; break;
                case 3: str_info = "将奖池期望值改为 [" + param.m_ratio + "]%"; break;
                case 4: str_info = "将控制系数改为 [" + param.m_ratio + "]%"; break;
                case 5: str_info = "将小奖抽水改为 [" + param.m_ratio + "]%"; break;  
            }
            return string.Format(info.m_fmt, str_info);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
/////////////////////////////////////////////////////////////////////////////
//系统空投发布
[Serializable]
class LogAirDropSysPublish : OpParam
{
    public int m_op;
    public int m_uuid;

    public LogAirDropSysPublish() { }
    public LogAirDropSysPublish(int op, int uuid)
    {
        m_op = op;
        m_uuid = uuid;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogAirDropSysPublish param = BaseJsonSerializer.deserialize<LogAirDropSysPublish>(str);
        if (param != null)
        {
            string type_str = (param.m_op == 0) ? "发布" : "删除";
            return string.Format(info.m_fmt, type_str, param.m_uuid);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
/////////////////////////////////////////////////////////////////////////////
//实物审核管理
[Serializable]
class LogServiceExchangeAudit : OpParam
{
    public int m_op;
    public string m_exchangeId;

    public LogServiceExchangeAudit() { }
    public LogServiceExchangeAudit(int op, string exchangeId)
    {
        m_op = op;
        m_exchangeId = exchangeId;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogServiceExchangeAudit param = BaseJsonSerializer.deserialize<LogServiceExchangeAudit>(str);
        if (param != null)
        {
            string str_op = param.m_op == 1 ? "未通过" : "通过";
            return string.Format(info.m_fmt, param.m_exchangeId, str_op);
        }

        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////////
//捕鱼弹头统计区间修正系数
[Serializable]
class LogBulletHeadRangeCorrectParamEdit :OpParam
{
    public int m_bulletHeadId;
    public string m_rangeCorrectParam;
    public int m_type;

    public LogBulletHeadRangeCorrectParamEdit() { }
    public LogBulletHeadRangeCorrectParamEdit(int bulletHead, string rangeCorrectParam, int type)
    {
        m_bulletHeadId = bulletHead;
        m_rangeCorrectParam = rangeCorrectParam;
        m_type = type;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogBulletHeadRangeCorrectParamEdit param = BaseJsonSerializer.deserialize<LogBulletHeadRangeCorrectParamEdit>(str);
        if (param != null)
        {
            string type_str = (param.m_type == 4) ? "使用范围" : "杀鱼范围";
            return string.Format(info.m_fmt, param.m_bulletHeadId,param.m_rangeCorrectParam,type_str);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
////////////////////////////////////////////////////////////////////////////
//机器人最高积分修改
[Serializable]
class LogFishlordRobotMaxScoreEdit : OpParam
{
    public int m_robotMaxScore;

    public LogFishlordRobotMaxScoreEdit() { }
    public LogFishlordRobotMaxScoreEdit(int robotMaxScore)
    {
        m_robotMaxScore = robotMaxScore;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogFishlordRobotMaxScoreEdit param = BaseJsonSerializer.deserialize<LogFishlordRobotMaxScoreEdit>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_robotMaxScore);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// 游戏鳄鱼大亨
[Serializable]
class LogGameCrocodileResult : OpParam
{
    public int m_roomId;
    public int m_result;

    public LogGameCrocodileResult() { }
    public LogGameCrocodileResult(int roomId,int result)
    {
        m_roomId = roomId;
        m_result = result;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogGameCrocodileResult param = BaseJsonSerializer.deserialize<LogGameCrocodileResult>(str);
        if (param != null)
        {
            string result = param.m_result.ToString();
            Crocodile_RateCFGData data = Crocodile_RateCFG.getInstance().getValue(param.m_result);
            if(data!=null)
            {
                result = data.m_name;
            }
            return string.Format(info.m_fmt, StrName.s_roomName[param.m_roomId - 1], result);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////////
// 游戏奔驰宝马结果控制
[Serializable]
class LogGameBzResult : OpParam
{
    public int m_roomId;
    public int m_result;

    public LogGameBzResult() { }
    public LogGameBzResult(int roomId, int result)
    {
        m_roomId = roomId;
        m_result = result;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogGameBzResult param = BaseJsonSerializer.deserialize<LogGameBzResult>(str);
        if (param != null)
        {
            string result = StrName.s_bzArea[param.m_result-1];
            return string.Format(info.m_fmt, StrName.s_roomName[param.m_roomId - 1], result);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////////
// 绑定玩家手机
[Serializable]
class LogBindPlayerPhone : OpParam
{
    public int m_playerId;
    public string m_phone;
    public string m_op;

    public LogBindPlayerPhone() { }
    public LogBindPlayerPhone(int playerId, string op, string phone)
    {
        m_playerId = playerId;
        m_op = op;
        m_phone = phone;
    }

    public override string getDescription(OpInfo info, string str)
    {
        LogBindPlayerPhone param = BaseJsonSerializer.deserialize<LogBindPlayerPhone>(str);
        if (param != null)
        {
            string op = "";
            string op_param = "";
            if (param.m_op == "bind")
            {
                op = "绑定";
                op_param = "，其号码为" + param.m_phone;
            }else if (param.m_op == "unbind")
            {
                op = "解绑";
            }
            return string.Format(info.m_fmt, op, param.m_playerId,op_param);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// 游戏黑红
[Serializable]
class LogGameShcdResult : OpParam
{
    public int m_roomId;
    public int m_result;

    public LogGameShcdResult() { }
    public LogGameShcdResult(int roomId,int result)
    {
        m_roomId = roomId;
        m_result = result;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogGameShcdResult param = BaseJsonSerializer.deserialize<LogGameShcdResult>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, StrName.s_shcdRoomName[param.m_roomId - 1], StrName.s_shcdArea[param.m_result]);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////////
//国庆节活动设置击杀数量
[Serializable]
class LogNdActPlayerFishCountReset : OpParam
{
    public int m_playerId;
    public int m_fishCount;

    public LogNdActPlayerFishCountReset() { }
    public LogNdActPlayerFishCountReset(int playerId, int fishCount)
    {
        m_playerId = playerId;
        m_fishCount = fishCount;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogNdActPlayerFishCountReset param = BaseJsonSerializer.deserialize<LogNdActPlayerFishCountReset>(str);
        if (param != null)
        {

            return string.Format(info.m_fmt, param.m_playerId, param.m_fishCount);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
/////////////////////////////////////////////////////////////////////////////

//万圣节活动玩家南瓜数量设置
[Serializable]
class LogHallowmasPumpkinCountReset : OpParam
{
    public int m_playerId;
    public int m_pumpkinCount;

    public LogHallowmasPumpkinCountReset() { }
    public LogHallowmasPumpkinCountReset(int playerId, int pumpkinCount)
    {
        m_playerId = playerId;
        m_pumpkinCount = pumpkinCount;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogHallowmasPumpkinCountReset param = BaseJsonSerializer.deserialize<LogHallowmasPumpkinCountReset>(str);
        if (param != null)
        {

            return string.Format(info.m_fmt, param.m_playerId, param.m_pumpkinCount);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
////////////////////////////////////////////////////////////////////////////
//金蟾夺宝活动玩家击杀金蟾数量设置
[Serializable]
class LogSpittorSnatchKillCountReset : OpParam
{
    public int m_playerId;
    public int m_killCount;

    public LogSpittorSnatchKillCountReset() { }
    public LogSpittorSnatchKillCountReset(int playerId, int killCount)
    {
        m_playerId = playerId;
        m_killCount = killCount;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogSpittorSnatchKillCountReset param = BaseJsonSerializer.deserialize<LogSpittorSnatchKillCountReset>(str);
        if (param != null)
            return string.Format(info.m_fmt, param.m_playerId, param.m_killCount);
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
///////////////////////////////////////////////////////////////////////////
//炸弹乐园玩家积分作弊
[Serializable]
class LogStatBulletHeadPlayerScoreAlter : OpParam
{
    public int m_playerId;
    public int m_type;
    public int m_score;

    public LogStatBulletHeadPlayerScoreAlter() { }

    public LogStatBulletHeadPlayerScoreAlter(int playerId, int type, int score)
    {
        m_playerId = playerId;
        m_type = type;
        m_score = score;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogStatBulletHeadPlayerScoreAlter param = BaseJsonSerializer.deserialize<LogStatBulletHeadPlayerScoreAlter>(str);
        if (param != null)
        {
            string type = "";
            switch (param.m_type)
            {
                case 0: type = "普通排行"; break;
                case 1: type = "青铜排行"; break;
                case 2: type = "白银排行"; break;
                case 3: type = "黄金排行"; break;
                case 4: type = "钻石排行"; break;
            }
            return string.Format(info.m_fmt, param.m_playerId, type, param.m_score);
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}

///////////////////////////////////////////////////////////////////////////
//捕鱼大奖赛玩家积分作弊
[Serializable]
class LogStatFishlordGrandPrixActPlayerScoreAlter : OpParam
{
    public int m_playerId;
    public int m_type;
    public long m_score;

    public LogStatFishlordGrandPrixActPlayerScoreAlter() { }

    public LogStatFishlordGrandPrixActPlayerScoreAlter(int playerId, int type, long score)
    {
        m_playerId = playerId;
        m_type = type;
        m_score = score;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogStatFishlordGrandPrixActPlayerScoreAlter param = BaseJsonSerializer.deserialize<LogStatFishlordGrandPrixActPlayerScoreAlter>(str);
        if (param != null)
        {
            string type = "";
            switch (param.m_type)
            {
                case 0: type = "今日排行"; break;
                case 1: type = "赛季排行"; break;
            }
            return string.Format(info.m_fmt, param.m_playerId, type, param.m_score);
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
///////////////////////////////////////////////////////////////////////////
//排行榜玩家积分作弊
[Serializable]
class LogOperationRankPlayerScoreAlter : OpParam
{
    public int m_playerId;
    public int m_type;
    public int m_score;

    public LogOperationRankPlayerScoreAlter() { }

    public LogOperationRankPlayerScoreAlter(int playerId, int type, int score)
    {
        m_playerId = playerId;
        m_type = type;
        m_score = score;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogOperationRankPlayerScoreAlter param = BaseJsonSerializer.deserialize<LogOperationRankPlayerScoreAlter>(str);
        if (param != null)
        {
            string type = "";
            switch (param.m_type)
            {
                case 0: type = "金币排行"; break;
                case 1: type = "青铜排行"; break;
                case 2: type = "白银排行"; break;
                case 3: type = "黄金排行"; break;
                case 4: type = "钻石排行"; break;
            }
            return string.Format(info.m_fmt, param.m_playerId, type, param.m_score);
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
///////////////////////////////////////////////////////////////////////////
//场次作弊
[Serializable]
class LogFishlordRoomCheat : OpParam
{
    public int m_roomId;
    public int m_playerId;
    public int m_type;
    public int m_score;

    public LogFishlordRoomCheat() { }

    public LogFishlordRoomCheat(int roomId, int playerId, int type, int score)
    {
        m_roomId = roomId;
        m_playerId = playerId;
        m_type = type;
        m_score = score;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogFishlordRoomCheat param = BaseJsonSerializer.deserialize<LogFishlordRoomCheat>(str);
        if (param != null)
        {
            string type = "积分";
            switch (param.m_type)
            {
                case 1: type = "当前总积分"; break;
                case 2: type = "单次最高积分"; break;
            }
            return string.Format(info.m_fmt, StrName.s_roomList[param.m_roomId], param.m_playerId, type, param.m_score);
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
///////////////////////////////////////////////////////////////////////////
//黑红黑白名单设置
[Serializable]
class LogShcdCardsSpecilList : OpParam 
{
    public int m_gameId;
    public int m_playerId;
    public int m_type;
    public int m_op;

    public LogShcdCardsSpecilList() { }

    public LogShcdCardsSpecilList(int gameId,int playerId,int type,int op) 
    {
        m_gameId = gameId;
        m_playerId = playerId;
        m_type = type;
        m_op = op;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogShcdCardsSpecilList param = BaseJsonSerializer.deserialize<LogShcdCardsSpecilList>(str);
        if (param != null)
        {
            string op = "";
            switch(param.m_op)
            {
                case 0: op = "拉入"; break;
                case 1: op = "拉出"; break;
            }

            string type = "";
            switch(param.m_type)
            {
                case 0: type = "黑白名单"; break;
                case 1: type = "黑名单"; break;
                case 2: type = "白名单"; break;
            }
            string game=StrName.s_gameName3[param.m_gameId];
            return string.Format(info.m_fmt, param.m_playerId, op, type,game);
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// 爆金比赛场抽水比例
[Serializable]
class LogBaojinPumpRate : OpParam
{
    public int m_roomId;
    public double m_pumpRate;

    public LogBaojinPumpRate() { }
    public LogBaojinPumpRate(int roomId, double pumpRate)
    {
        m_roomId = roomId;
        m_pumpRate = pumpRate;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogBaojinPumpRate param = BaseJsonSerializer.deserialize<LogBaojinPumpRate>(str);
        if (param != null)
        {
            string strRoomName = "";
            if (param.m_roomId == 11)
            {
                strRoomName += "[富豪竞技场]";
            }
            return string.Format(info.m_fmt, strRoomName, param.m_pumpRate);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//广告投放时间设置
[Serializable]
class LogOperationAd100003ActSet : OpParam
{
    public int m_key;
    public string m_time;

    public LogOperationAd100003ActSet() { }
    public LogOperationAd100003ActSet(int key, string time)
    {
        m_key = key;
        m_time = time;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogOperationAd100003ActSet param = BaseJsonSerializer.deserialize<LogOperationAd100003ActSet>(str);
        if (param != null)
        {
            string strRoomName = "编辑了";
            switch(param.m_key)
            {
                case 1: strRoomName += "【100003 - 闲玩"; break;
                case 2: strRoomName += "【100009 - 葫芦星球"; break;
                case 3: strRoomName += "【100010 - 蛋蛋赚"; break;
                case 4: strRoomName += "【100011 - 有赚"; break;
                case 5: strRoomName += "【100012 - 麦子赚"; break;
                case 6: strRoomName += "【100013 - 聚享游"; break;
                case 7: strRoomName += "【100014 - 小啄"; break;
                case 8: strRoomName += "【100015 - 泡泡赚"; break;
            }
            
            strRoomName += "广告投放活动时间】";
            return string.Format(info.m_fmt, strRoomName, param.m_time);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// 竞技场得分修改
[Serializable]
class LogBaojinScoreAlter : OpParam
{
    public int m_playerId;
    public int m_newScore;
    public bool m_flag;

    public LogBaojinScoreAlter() { }
    public LogBaojinScoreAlter(int playerId, int newScore,bool flag)
    {
        m_playerId = playerId;
        m_newScore = newScore;
        m_flag = flag;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogBaojinScoreAlter param = BaseJsonSerializer.deserialize<LogBaojinScoreAlter>(str);
        if (param != null)
        {
            string strTitle = "";
            if (param.m_flag)
            {
                strTitle = " 日最高值和周最高值 ";
            }
            else 
            {
                strTitle = " 日最高值 ";
            }
            return string.Format(info.m_fmt, param.m_playerId,strTitle, param.m_newScore);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//玩家龙鳞数量修改
[Serializable]
class LogDragonScaleNumAlter : OpParam
{
    public int m_playerId;
    public int m_newScore;
    public int m_oldNum;

    public LogDragonScaleNumAlter() { }
    public LogDragonScaleNumAlter(int playerId, int newScore,int oldNum)
    {
        m_playerId = playerId;
        m_newScore = newScore;
        m_oldNum = oldNum;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogDragonScaleNumAlter param = BaseJsonSerializer.deserialize<LogDragonScaleNumAlter>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_playerId, param.m_oldNum, param.m_newScore);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//巨鲲降世/定海神针积分修改
[Serializable]
class LogFishlordLegendaryRankScoreAlter : OpParam
{
    public int m_playerId;
    public int m_newScore;
    public int m_op;

    public LogFishlordLegendaryRankScoreAlter() { }
    public LogFishlordLegendaryRankScoreAlter(int playerId, int newScore, int op)
    {
        m_playerId = playerId;
        m_newScore = newScore;
        m_op = op;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogFishlordLegendaryRankScoreAlter param = BaseJsonSerializer.deserialize<LogFishlordLegendaryRankScoreAlter>(str);
        if (param != null)
        {
            string str_note = "将ID为["+ param.m_playerId +"]的玩家在";
            switch(param.m_op)
            {
                case 0:
                    str_note += "巨鲲降世的积分改为：";
                    break;
                case 1:
                    str_note += "定海神针单炸积分改为：";
                    break;
                case 2:
                    str_note += "定海神针累炸积分改为：";
                    break;
            }

            str_note += param.m_newScore;
            return string.Format(info.m_fmt, str_note);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//圣兽场积分修改
[Serializable]
class LogFishlordMythicalRankScoreAlter : OpParam
{
    public int m_playerId;
    public int m_newScore;

    public LogFishlordMythicalRankScoreAlter() { }
    public LogFishlordMythicalRankScoreAlter(int playerId, int newScore)
    {
        m_playerId = playerId;
        m_newScore = newScore;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogFishlordMythicalRankScoreAlter param = BaseJsonSerializer.deserialize<LogFishlordMythicalRankScoreAlter>(str);
        if (param != null)
        {
            string str_note = "将ID为[" + param.m_playerId + "]的玩家在圣兽场积分改为";

            str_note += param.m_newScore;
            return string.Format(info.m_fmt, str_note);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//玩家月饼量修改
[Serializable]
class LogJinQiuNationalDayActAlter : OpParam
{
    public int m_playerId;
    public int m_newScore;
    public string m_oldNum;

    public LogJinQiuNationalDayActAlter() { }
    public LogJinQiuNationalDayActAlter(int playerId, int newScore, string oldNum)
    {
        m_playerId = playerId;
        m_newScore = newScore;
        m_oldNum = oldNum;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogJinQiuNationalDayActAlter param = BaseJsonSerializer.deserialize<LogJinQiuNationalDayActAlter>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_playerId, param.m_oldNum, param.m_newScore);
        }
        return "";
    }
    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// 封IP
[Serializable]
class LogBlockIP : OpParam
{
    public string m_ip = "";
    public bool m_isBlock;

    public LogBlockIP() { }
    public LogBlockIP(string ip, bool block)
    {
        m_ip = ip;
        m_isBlock = block;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogBlockIP param = BaseJsonSerializer.deserialize<LogBlockIP>(str);
        if (param != null)
        {
            if (param.m_isBlock)
            {
                return string.Format(info.m_fmt, "封", param.m_ip);
            }
            return string.Format(info.m_fmt, "解封", param.m_ip);
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}

//////////////////////////////////////////////////////////////////////////

// 封账号
[Serializable]
class LogBlockAcc : OpParam
{
    public string m_acc = "";
    public bool m_isBlock;

    public LogBlockAcc() { }
    public LogBlockAcc(string acc, bool block)
    {
        m_acc = acc;
        m_isBlock = block;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogBlockAcc param = BaseJsonSerializer.deserialize<LogBlockAcc>(str);
        if (param != null)
        {
            if (param.m_isBlock)
            {
                return string.Format(info.m_fmt, "封", param.m_acc);
            }
            return string.Format(info.m_fmt, "解封", param.m_acc);
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}

//////////////////////////////////////////////////////////////////////////

// 重置密码
[Serializable]
class LogResetPwd : OpParam
{
    public string m_acc = "";
    public string m_phone = "";

    public LogResetPwd() { }
    public LogResetPwd(string acc, string phone)
    {
        m_acc = acc;
        m_phone = phone;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogResetPwd param = BaseJsonSerializer.deserialize<LogResetPwd>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_acc, param.m_phone);
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}

//////////////////////////////////////////////////////////////////////////

// 后台充值
[Serializable]
class LogGmRecharge : OpParam
{
    public int m_playerId;
    public int m_rtype;
    public int m_param;
    
    public LogGmRecharge() { }
    public LogGmRecharge(int playerId, int rtype, int param)
    {
        m_playerId = playerId;
        m_rtype = rtype;
        m_param = param;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogGmRecharge param = BaseJsonSerializer.deserialize<LogGmRecharge>(str);
        if (param != null)
        {
            RechargeCFGData rd = RechargeCFG.getInstance().getValue(param.m_param);
            if (rd != null)
            {
                return string.Format(info.m_fmt, param.m_playerId, StrName.s_rechargeType[param.m_rtype], rd.m_price);
            }
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// 封ID
[Serializable]
class LogBlockId : OpParam
{
    public string m_playerId;
    public bool m_isBlock;

    public LogBlockId() { }
    public LogBlockId(string playerId, bool block)
    {
        m_playerId = playerId;
        m_isBlock = block;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogBlockId param = BaseJsonSerializer.deserialize<LogBlockId>(str);
        if (param != null)
        {
            if (param.m_isBlock)
            {
                return string.Format(info.m_fmt, "封", param.m_playerId);
            }
            return string.Format(info.m_fmt, "解封", param.m_playerId);
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}

//////////////////////////////////////////////////////////////////////////
//修改了限时活动最大次数
[Serializable]
class LogModifyActivityPanicBuyingCfg : OpParam
{
    // 活动列表
    public string m_activityList = "";

    // 最大次数
    public int m_value;

    public int m_activityId;

    public LogModifyActivityPanicBuyingCfg()
    {
    }
    public LogModifyActivityPanicBuyingCfg(string activityList, int value, int activityId)
    {
        m_activityList = activityList;
        m_value = value;
        m_activityId = activityId;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogModifyActivityPanicBuyingCfg param = BaseJsonSerializer.deserialize<LogModifyActivityPanicBuyingCfg>(str);
        if (param.m_activityList == "")
            return "";

        string[] arrs = Tool.split(param.m_activityList, ',', StringSplitOptions.RemoveEmptyEntries);
        string activity = "";

        for (int i = 0; i < arrs.Length; i++)
        {
            int activityType = Convert.ToInt32(arrs[i]);
            var activity_item = ActivityPanicBuyingCFG.getInstance().getValue(activityType);

            activity += activity_item.m_activityName + ",";
        }

        return string.Format(info.m_fmt, activity, param.m_value);
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//个人后台管理 算法参数修改
[Serializable]
class LogModifyFishlordSingleCtrlParam : OpParam
{
    public double m_baseRate;
    public double m_deviationFix;
    public double m_checkRate;

    public LogModifyFishlordSingleCtrlParam() { }

    public LogModifyFishlordSingleCtrlParam(double baseRate, double deviationFix, double checkRate)
    {
        m_baseRate = baseRate;
        m_deviationFix = deviationFix;
        m_checkRate = checkRate;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogModifyFishlordSingleCtrlParam param = BaseJsonSerializer.deserialize<LogModifyFishlordSingleCtrlParam>(str);
        if (param != null)
            return string.Format(info.m_fmt, param.m_baseRate, param.m_deviationFix, param.m_checkRate);

        return "";
        
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// 修改了房间的期望盈利率
[Serializable]
class LogModifyFishlordRoomExpRate : OpParam
{
    // 房间列表
    public string m_roomList = "";   
    
    // 盈利率实际值
    public double m_value;

    public int m_gameId;

    public LogModifyFishlordRoomExpRate() { }

    public LogModifyFishlordRoomExpRate(string roomList, double value, int gameId)
    {
        m_roomList = roomList;
        m_value = value;
        m_gameId = gameId;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogModifyFishlordRoomExpRate param = BaseJsonSerializer.deserialize<LogModifyFishlordRoomExpRate>(str);
        if (param.m_roomList == "")
            return "";

        string[] arrs = Tool.split(param.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
        string room = "";

        for (int i = 0; i < arrs.Length; i++)
        {
            int roomType = Convert.ToInt32(arrs[i]);
            if (StrName.s_roomList.ContainsKey(roomType))
            {
                room += StrName.s_roomList[roomType] + ",";
            }
            else
            {
                room += roomType + ",";
            }
        }

        return string.Format(info.m_fmt, room, param.m_value, StrName.getGameName3(param.m_gameId));
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//修改了奔驰宝马参数
[Serializable]
class LogModifyBzParam : OpParam
{
    // 房间列表
    public string m_roomList = "";
    //操作的参数
    public int m_op;
    //参数值
    public string m_value;

    public LogModifyBzParam() { }

    public LogModifyBzParam(string roomList, int op, string value)
    {
        m_roomList = roomList;
        m_op = op;
        m_value = value;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogModifyBzParam param = BaseJsonSerializer.deserialize<LogModifyBzParam>(str);
        if (param.m_roomList == "")
            return "";

        string[] arrs = Tool.split(param.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
        string room = "";
        for (int i = 0; i < arrs.Length; i++)
        {
            int roomType = Convert.ToInt32(arrs[i]);
            if (StrName.s_roomList.ContainsKey(roomType))
            {
                room += StrName.s_roomList[roomType] + ",";
            }
            else
            {
                room += roomType + ",";
            }
        }

        string op_param = "";
        switch(param.m_op)
        {
            case 1: op_param = "广告版下注阀值"; break;
            case 2: op_param = "大天堂放分概率"; break;
            case 3: op_param = "小天堂放分概率"; break;
            case 4: op_param = "大地狱杀分概率"; break;
            case 5: op_param = "小地狱杀分概率"; break;
        }

        return string.Format(info.m_fmt, room, op_param, param.m_value);
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//修改了鳄鱼大亨参数
[Serializable]
class LogModifyCrocodileParam : OpParam
{
    // 房间列表
    public string m_roomList = "";
    //操作的参数
    public int m_op;
    //参数值
    public string m_value;

    public LogModifyCrocodileParam() { }

    public LogModifyCrocodileParam(string roomList,int op, string value)
    {
        m_roomList = roomList;
        m_op = op;
        m_value = value;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogModifyCrocodileParam param = BaseJsonSerializer.deserialize<LogModifyCrocodileParam>(str);
        if (param.m_roomList == "")
            return "";

        string[] arrs = Tool.split(param.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
        string room = "";
        for (int i = 0; i < arrs.Length; i++)
        {
            int roomType = Convert.ToInt32(arrs[i]);
            if (StrName.s_roomList.ContainsKey(roomType))
            {
                room += StrName.s_roomList[roomType] + ",";
            }
            else
            {
                room += roomType + ",";
            }
        }

        string op_param = "";
        switch(param.m_op)
        {
            case 1: op_param = "广告版下注阀值"; break;
            case 2: op_param = "大天堂放分概率"; break;
            case 3: op_param = "小天堂放分概率"; break;
            case 4: op_param = "大地狱杀分概率"; break;
            case 5: op_param = "小地狱杀分概率"; break;
        }

        return string.Format(info.m_fmt, room, op_param, param.m_value);
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//大水池参数调整
[Serializable]
class LogModifyFishlordCtrlNewParam : OpParam
{
    // 房间列表
    public int m_roomId;
    public double m_jsckpotGrandPump;
    public double m_jsckpotSmallPump;
    public double m_normalFishRoomPoolPumpParam;

    public double m_baseRate;
    public double m_checkRate;
    public double m_trickDeviationFix;

    public long m_incomeThreshold;
    public int m_earnRatemCtrMax;
    public int m_earnRatemCtrMin;

    //public int m_legendaryFishRate;

    public int m_mythicalScoreTurnRate;
    public int m_mythicalFishRate;

    public int m_colorFishCtrCount1;
    public int m_colorFishCtrCount2;
    public int m_colorFishCtrRate1;
    public int m_colorFishCtrRate2;

    public LogModifyFishlordCtrlNewParam() { }

    public LogModifyFishlordCtrlNewParam(int roomId, double jsckpotGrandPump, double jsckpotSmallPump, double normalFishRoomPoolPumpParam, double baseRate,
        double checkRate, double trickDeviationFix, long incomeThreshold,int earnRatemCtrMax,int earnRatemCtrMin, 
        //int legendaryFishRate,
        int mythicalScoreTurnRate, int mythicalFishRate, int colorFishCtrCount1, int colorFishCtrCount2, int colorFishCtrRate1, int colorFishCtrRate2)
    {
        m_roomId = roomId;
        m_jsckpotGrandPump = jsckpotGrandPump;
        m_jsckpotSmallPump = jsckpotSmallPump;
        m_normalFishRoomPoolPumpParam = normalFishRoomPoolPumpParam;
        m_baseRate = baseRate;
        m_checkRate = checkRate;
        m_trickDeviationFix = trickDeviationFix;
        m_incomeThreshold = incomeThreshold;
        m_earnRatemCtrMax = earnRatemCtrMax;
        m_earnRatemCtrMin = earnRatemCtrMin;
        //m_legendaryFishRate = legendaryFishRate;

        m_mythicalScoreTurnRate = mythicalScoreTurnRate;
        m_mythicalFishRate = mythicalFishRate;

        m_colorFishCtrCount1 = colorFishCtrCount1;
        m_colorFishCtrCount2 = colorFishCtrCount2;
        m_colorFishCtrRate1 = colorFishCtrRate1;
        m_colorFishCtrRate2 = colorFishCtrRate2;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogModifyFishlordCtrlNewParam param = BaseJsonSerializer.deserialize<LogModifyFishlordCtrlNewParam>(str);
        if (param != null) 
        {
            string roomName = StrName.s_roomList[param.m_roomId];

            string strinfo = "将参数改为：";

            //if (param.m_roomId == 8)
            //    strinfo += "鲲币转换系数[" + param.m_legendaryFishRate/1000.0 + "]，";

            //高级场
            if(param.m_roomId == 3)
                strinfo += " 大奖抽水系数 [" + param.m_jsckpotGrandPump + "]，小奖抽水系数[ " + param.m_jsckpotSmallPump + "]，小鱼抽水系数 [" + param.m_normalFishRoomPoolPumpParam + "]，";

            strinfo += "抽水率 [" + param.m_earnRatemCtrMax + "]，期望盈利率 [" + param.m_earnRatemCtrMin +
                        "]，码量控制值 [" + param.m_incomeThreshold + "]，机率控制 [" + param.m_baseRate + "]， 刷新周期 [" + param.m_checkRate + "]";

            //中级场 高级场 龙宫场
            if (param.m_roomId == 2 || param.m_roomId == 3 || param.m_roomId == 5)
                strinfo += " ，玩法误差值 [" + param.m_trickDeviationFix + "]";

            if (param.m_roomId == 2) {
                strinfo += " ，捕获次数（一档） [" + param.m_colorFishCtrCount1 + "], 捕获次数（二档） [" + param.m_colorFishCtrCount2 + 
                    "], 花色鱼命中系数（一档）[" +  param.m_colorFishCtrRate1 + "], 花色鱼命中系数（二档）[" + param.m_colorFishCtrRate2 + "]";
            }

            if (param.m_roomId == 9) 
            {
                strinfo += ", 朱雀转化玄武系数 [" + param.m_mythicalScoreTurnRate + "], 玄武累分系数 [ " + param.m_mythicalFishRate + " ]" ;
            }

            return string.Format(info.m_fmt, roomName, strinfo);
        }

        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
///////////////////////////////////////////////////////////////////////////
// 重置了房间的期望盈利率
[Serializable]
class LogResetFishlordRoomExpRate : OpParam
{
    // 房间列表
    public string m_roomList = "";
    public int m_gameId;

    public LogResetFishlordRoomExpRate() { }

    public LogResetFishlordRoomExpRate(string roomList, int gameId)
    {
        m_roomList = roomList;
        m_gameId = gameId;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogModifyFishlordRoomExpRate param = BaseJsonSerializer.deserialize<LogModifyFishlordRoomExpRate>(str);
        if (param.m_roomList == "")
            return "";

        string[] arrs = Tool.split(param.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
        string room = "";

        for (int i = 0; i < arrs.Length; i++)
        {
            int roomType = Convert.ToInt32(arrs[i]);
            try
            {
                if (StrName.s_roomList.ContainsKey(roomType))
                {
                    room += StrName.s_roomList[roomType] + ",";
                }
                else
                {
                    room += roomType + ",";
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        return string.Format(info.m_fmt, room, StrName.getGameName3(param.m_gameId));
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//捕鱼水浒传点杀点送
[Serializable]
class LogSetScorePool : OpParam
{
    public int m_gameId;
    public int m_op;
    public string m_playerId;
    public string m_setValue;
    public string m_time;
    public int m_type;

    public LogSetScorePool() { }

    public LogSetScorePool(int gameId, int op, string playerId, string setValue, string time, int type = 0)
    {
        m_gameId = gameId;
        m_op = op;
        m_playerId = playerId;
        m_setValue = setValue;
        m_time = time;
        m_type = type;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogSetScorePool param = BaseJsonSerializer.deserialize<LogSetScorePool>(str);
        if (param.m_playerId == "")
            return "";

        string str_note = "";

        str_note += StrName.s_gameName3[param.m_gameId]+"点杀点送：";
        str_note += "操作了ID为[" + param.m_playerId + "]的玩家参数，";

        string typeName = "";
        if (param.m_type == 1)
        {
            typeName += "金币";
        }
        else if (param.m_type == 19)
        {
            typeName += "龙珠碎片";
        }

        if (param.m_op == 0) //添加
        {
            if (param.m_gameId == (int)GameId.shuihz)
            {
                str_note += "添加" + typeName + "BUFF水池为" + param.m_setValue;
            }
            else 
            {
                str_note += "添加" + typeName + "BUFF水池为" + param.m_setValue + ",且设置其到期日期为" + param.m_time;
            }
        }else if(param.m_op==3)
        {
            str_note += "清零了其" + typeName + "BUFF水池当前操作值";
        }
        else  //删除
        {
            str_note += "移除了其" + typeName + "BUFF";
        }
        return string.Format(info.m_fmt, str_note);
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
///////////////////////////////////////////////////////////////////////////
//机器人积分管理
[Serializable]
class LogFishlordRobotRankCFG : OpParam
{
    public int m_op;
    public string m_param;

    public LogFishlordRobotRankCFG() { }

    public LogFishlordRobotRankCFG(int op, string param)
    {
        m_op = op;
        m_param = param;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogFishlordRobotRankCFG param = BaseJsonSerializer.deserialize<LogFishlordRobotRankCFG>(str);
        if (param != null)
        {
            string str_type = "";
            switch(param.m_op){
                case 1:
                    str_type = "中级场幸运榜";break;
                case 2:
                    str_type = "中级场牛人榜"; break;
                case 3:
                    str_type = "高级场排行榜"; break;
                case 4:
                    str_type = "武装场排行榜"; break;
                case 5:
                    str_type = "巨鲲场排行榜"; break;
                case 6:
                    str_type = "欢乐炸-普通"; break;
                case 7:
                    str_type = "欢乐炸-青铜"; break;
                case 8:
                    str_type = "欢乐炸-白银"; break;
                case 9:
                    str_type = "欢乐炸-黄金"; break;
                case 10:
                    str_type = "欢乐炸-钻石"; break;
                case 11:
                    str_type = "衰神炸-普通"; break;
                case 12:
                    str_type = "衰神炸-青铜"; break;
                case 13:
                    str_type = "衰神炸-白银"; break;
                case 14:
                    str_type = "衰神炸-黄金"; break;
                case 15:
                    str_type = "衰神炸-钻石"; break;
            }
            return string.Format(info.m_fmt,str_type, param.m_param);
        }
        return "";

    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
///////////////////////////////////////////////////////////////////////////
// 冻结头像
[Serializable]
class LogFreezeHead : OpParam
{
    public int m_playerId;
    public DateTime m_deadTime;

    public LogFreezeHead() { }
    public LogFreezeHead(int playerId, DateTime deadTime)
    {
        m_playerId = playerId;
        m_deadTime = deadTime;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogFreezeHead param = BaseJsonSerializer.deserialize<LogFreezeHead>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_playerId, param.m_deadTime);
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
///////////////////////////////////////////////////////////////////////////
// 修改活动配置表时间
[Serializable]
class LogOperationActivityCFGEdit : OpParam
{
    public int m_itemId;
    public string m_startTime;
    public string m_endTime;

    public LogOperationActivityCFGEdit() { }
    public LogOperationActivityCFGEdit(int itemId, string startTime, string endTime)
    {
        m_itemId = itemId;
        m_startTime = startTime;
        m_endTime = endTime;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogOperationActivityCFGEdit param = BaseJsonSerializer.deserialize<LogOperationActivityCFGEdit>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_itemId, param.m_startTime, param.m_endTime);
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// 增加牛牛牌型
[Serializable]
class LogCowsAddCardType : OpParam
{
    public int m_bankerType;
    public int m_other1Type;
    public int m_other2Type;
    public int m_other3Type;
    public int m_other4Type;

    public LogCowsAddCardType() { }
    public LogCowsAddCardType(int bankerType, 
        int other1Type,int other2Type,int other3Type,int other4Type)
    {
        m_bankerType = bankerType;
        m_other1Type = other1Type;
        m_other2Type = other2Type;
        m_other3Type = other3Type;
        m_other4Type = other4Type;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogCowsAddCardType param = BaseJsonSerializer.deserialize<LogCowsAddCardType>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, 
                ItemHelp.getCowsCardTypeName(param.m_bankerType),
                ItemHelp.getCowsCardTypeName(param.m_other1Type),
                ItemHelp.getCowsCardTypeName(param.m_other2Type),
                ItemHelp.getCowsCardTypeName(param.m_other3Type),
                ItemHelp.getCowsCardTypeName(param.m_other4Type));
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}

//////////////////////////////////////////////////////////////////////////
// 祝福诅咒
[Serializable]
class LogWishCurse : OpParam
{
    public int m_gameId;     // 游戏ID
    public int m_playerId;   // 玩家ID
    public int m_wishType;   // 祝福诅咒类型
    public int m_opType;     // 操作类型，添加 or 去除
    
    public LogWishCurse() { }
    public LogWishCurse(int gameId, int playerId, int wishType, int opType)
    {
        m_gameId = gameId;
        m_playerId = playerId;
        m_wishType = wishType;
        m_opType = opType;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogWishCurse param = BaseJsonSerializer.deserialize<LogWishCurse>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt,
                StrName.getGameName3(param.m_gameId),
                param.m_playerId,
                param.m_opType == 0 ? "添加" : "去除",
                StrName.s_wishCurse[param.m_wishType]);
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}

//////////////////////////////////////////////////////////////////////////
// 玩家属性操作
[Serializable]
class LogPlayerOp : OpParam
{
    public int m_playerId;     // 玩家ID
    public int m_op;           // 操作类型
    public string m_prop;      // 属性
    public int m_value;        // 值

    public LogPlayerOp() { }
    public LogPlayerOp(int playerId, int op, string prop, int value)
    {
        m_playerId = playerId;
        m_op = op;
        m_prop = prop;
        m_value = value;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogPlayerOp param = BaseJsonSerializer.deserialize<LogPlayerOp>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt,
                param.m_playerId,
                param.m_op == 1 ? "增加" : "减少",
                param.m_value,
                getProp(param));
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }

    public string getProp(LogPlayerOp p)
    {
        if (p.m_prop == "gold")
            return "金币";

        if (p.m_prop == "gem")
            return "钻石";

        if (p.m_prop == "vip")
            return "vip经验";

        if (p.m_prop == "dragonBall")
            return "龙珠";

        if (p.m_prop == "chip")
            return "碎片";

        if (p.m_prop == "moshi")
            return "魔石";

        if (p.m_prop == "xp")
            return "玩家经验";

        return "";
    }
}

//////////////////////////////////////////////////////////////////////////
// 重新加载表格
[Serializable]
class LogReloadTable : OpParam
{
    public int m_gameId;

    public LogReloadTable() { }
    public LogReloadTable(int gameId)
    {
        m_gameId = gameId;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogReloadTable param = BaseJsonSerializer.deserialize<LogReloadTable>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt,
                StrName.getGameName3(param.m_gameId));
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}

//////////////////////////////////////////////////////////////////////////
// 新老账号问题
[Serializable]
class LogOldNewAcc : OpParam
{
    public int m_oldId;
    public int m_newId;

    public LogOldNewAcc() { }
    public LogOldNewAcc(int oldId, int newId)
    {
        m_oldId = oldId;
        m_newId = newId;
    }

    public override string getDescription(OpInfo info, string str)
    {
        if (info == null)
            return "引用空";

        LogOldNewAcc param = BaseJsonSerializer.deserialize<LogOldNewAcc>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_oldId, param.m_newId);
        }
        return "";
    }

    // 返回要存入数据库的参数串
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}