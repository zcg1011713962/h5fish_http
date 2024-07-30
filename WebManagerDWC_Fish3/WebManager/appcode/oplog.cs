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
    // ���ʼ�
    public const int LOG_TYPE_SEND_MAIL = 0;

    // ��IP
    public const int LOG_TYPE_BLOCK_IP = 1;

    // ���˺�
    public const int LOG_TYPE_BLOCK_ACC = 2;

    // ��������
    public const int LOG_TYPE_RESET_PWD = 3;

    // ��̨��ֵ
    public const int LOG_TYPE_GM_RECHARGE = 4;

    // ͣ�����ID
    public const int LOG_TYPE_BLOCK_ID = 5;

    // �޸Ĳ���ӯ����
    public const int LOG_TYPE_MODIFY_FISHLORD_GAIN_RATE = 6;

    // ���ò���ӯ����
    public const int LOG_TYPE_RESET_FISHLORD_GAIN_RATE = 7;

    // ɾ���Զ���ͷ��
    public const int LOG_TYPE_DEL_CUSTOM_HEAD = 8;

    // ����ţţ����
    public const int LOG_TYPE_COWS_ADD_CARDS_TYPE = 9;

    // ף��������
    public const int LOG_TYPE_WISH_CURSE = 10;

    // ������Բ���
    public const int LOG_TYPE_PLAYER_OP = 11;

    // ���ر��
    public const int LOG_TYPE_RELOAD_TABLE = 12;

    //��ʱ������޸�
    public const int LOG_TYPE_ACTIVITY_CFG = 13;

    //��Ϸ������� ������
    public const int LOG_TYPE_GAME_CROCODILE_RESULT = 14;

    //�ں�
    public const int LOG_TYPE_GAME_SHCD_RESULT = 15;

    //�����������ˮ��������
    public const int LOG_TYPE_BAOJIN_PUMP_RATE = 16;

    //��������ҵ÷��޸�
    public const int LOG_TYPE_BAOJIN_SCORE = 17;

    //������ֻ�
    public const int LOG_TYPE_BIND_PLAYER_PHONE = 18;

    //�޸����������
    public const int LOG_TYPE_MODIFY_CROCODILE_PARAM = 19;

    //���۱���������
    public const int LOG_TYPE_GAME_BZ_RESULT = 20;

    //�޸ı��۱������
    public const int LOG_TYPE_MODIFY_BZ_PARAM = 21;

    //�ں�ڰ������б�
    public const int LOG_TYPE_SHCD_CARDS_SPECIL_LIST = 22;

    //�����ɱ����
    public const int LOG_TYPE_SCORE_POOL_SET = 23;

    //С��Ϸ��������
    public const int LOG_TYPE_CHANNEL_OPEN_CLOSE_GAME = 24;

    //����ڻ����
    public const int LOG_TYPE_ND_ACT_FISHCOUNT_RESET = 25;

    //��ʥ�ڻ����
    public const int LOG_TYPE_HALLOWMAS_PUMPKINCOUNT_RESET = 26;

    //�޸Ĳ��㵯ͷͳ����������ϵ��
    public const int LOG_TYPE_FISHLORD_BULLET_HEAD_RCPARAM_EDIT = 27;

    //�޸Ļ�������߻���
    public const int LOG_TYPE_ROBOT_MAX_SCORE_EDIT = 28;

    //���ٿ�ʼ���οͣ�����˺���Ϣ
    public const int LOG_TYPE_FASTER_START_FOR_VISITOR = 29;

    //ˮ�����������
    public const int LOG_TYPE_GAME_FRUIT_RESULT = 30;

    //���ᱦ�����
    public const int LOG_TYPE_FISHLORD_SPITTOR_SNATCH_KILLCOUNT_SET = 31;

    //�ͷ�����/�����/��������-����
    public const int LOG_TYPE_SERVICE_REPAIR_ORDER_OP = 32;

    //������������޸�
    public const int LOG_TYPE_DRAGON_SCALE_NUM_ALTER = 33;

    //��ʧ�����������Ӽ�¼
    public const int LOG_TYPE_GUIDE_LOST_PLAYER = 34;

    //����±������޸�
    public const int LOG_TYPE_JINQIU_NATIONALDAY_ACT_ALTER = 35;

    //����ϵͳ��Ͷ
    public const int LOG_TYPE_AIR_DROP_SYS_PUBLISH = 36;

    //���˺�̨�����㷨�����޸�
    public const int LOG_TYPE_MODIFY_FISHLORD_SINGLE_CTRL_PARAM = 37;

    //��ˮ�ز�������
    public const int LOG_TYPE_MODIFY_FISHLORD_CTRL_PARAM = 38;

    //�������� �м��� �߼���
    public const int LOG_TYPE_FISHLORD_ROOM_CHEAT = 39;

    //�߼����淨����
    public const int LOG_TYPE_FISHLORD_ADVANCED_ROOM_CTRL = 40;

    //ը����԰��һ�������
    public const int LOG_TYPE_BULLET_HEAD_PLAYER_SCORE_ALTER = 41;

    //���ｵ��/������������
    public const int LOG_TYPE_FISHLORD_LEGENDARY_RANK_CHEAT = 42;

    //�޸����ñ�
    public const int LOG_TYPE_OPERATION_ACTIVITY_CFG = 43;

    //�����˻��ֹ���
    public const int LOG_TYPE_OPERATION_FISHLORD_ROBOT_RANK_CFG = 44;

    //ʥ�޳���һ�������
    public const int LOG_TYPE_FISHLORD_MYTHICAL_PLAYER_CHEAT = 45;

    //ʵ����˹���
    public const int LOG_TYPE_EXCHANGE_AUDIT = 46;

    //���а�����
    public const int LOG_TYPE_OPERATION_RANK_PLAYER_SCORE_EDIT = 47;

    //���Ͷ��ʱ������
    public const int LOG_TYPE_OPERATION_AD_100003_ACT_SET = 48;

    //���������һ�������
    public const int LOG_TYPE_FISHLORD_GRAND_PRIX_ACT_PLAYER_SCORE_ALTER = 49;

    //��һ�����Ϣ�޸�
    public const int LOG_TYPE_PLAYER_BASIC_INFO_CHG = 50;

    //�����˺�����
    public const int LOG_TYPE_NEW_OLD_ACC = 51;
}

//////////////////////////////////////////////////////////////////////////

// ������������Ļ���
[Serializable]
class OpParam
{
    public OpParam() { }
    // ȡ��������
    public virtual string getDescription(OpInfo info, string str) { return ""; }
    // ȡ�ô洢���ݿ�Ĵ�
    public virtual string getString() { return ""; }
}
////////////////////////////////////////////////////////////////////////////////
//�ͷ�����
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
            return "���ÿ�";

        LogServiceRepairOrder param = BaseJsonSerializer.deserialize<LogServiceRepairOrder>(str);
        if (param != null)
        {
            string strCon = "����Ա��"+ param.m_operator + " �����˿ͷ�����/�����/��������-ϵͳ ������ϸ������ԭ��=> ";
            
            switch(param.m_op){
                case 0: strCon += "������"; break;
                case 1: strCon += "����������"; break;
                case 2: strCon += "���ʿͷ�������"; break;
                case 3: strCon += "�󻧻���������"; break;
            }

            strCon += "������Ŀ=>";
            if (param.m_repairOrder == -1)
            {
                strCon += "false ��";
            }
            else
            {
                var orderItem = RechargeCFG.getInstance().getValue(param.m_repairOrder);
                if (orderItem != null)
                {
                    strCon += orderItem.m_name+"��";
                }
                else {
                    strCon += " ��";
                }  
            }

            strCon += "��������/�ͷ��طø���=>";
            if (param.m_repairBonus == -1)
            {
                strCon += "false ��";
            }
            else
            {
                var bonusItem = RepairOrderItem.getInstance().getValue(param.m_repairBonus);
                if (bonusItem != null)
                {
                    strCon += bonusItem.m_itemName+"��";
                }
                else {
                    strCon += " ��";
                }
            }

            strCon += "��ע=>"+param.m_comments;
            return string.Format(info.m_fmt, strCon);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////////////
//��ʧ�����������Ӽ�¼
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
            return "���ÿ�";

        LogGuideLostPlayer param = BaseJsonSerializer.deserialize<LogGuideLostPlayer>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_time,param.m_playerId,param.m_comments);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////////////
//��Ϸˮ�����������
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
            return "���ÿ�";

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
                case 0: opName = "����"; break;
                case 1: opName = "�޸�"; break;
            }

            return string.Format(info.m_fmt, opName, param.m_playerId, result);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//���ٿ�ʼ���οͣ�����˺���Ϣ
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
            return "���ÿ�";
        LogFasterStartForVisitor param = BaseJsonSerializer.deserialize<LogFasterStartForVisitor>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_playerId);
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//С��Ϸ��������
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
            return "���ÿ�";

        LogChannelOpenCloseGameSet param = BaseJsonSerializer.deserialize<LogChannelOpenCloseGameSet>(str);
        if (param != null)
        {
            string channel = "";
            //����
            var cd = TdChannel.getInstance().getValue(param.m_channel);
            if (cd != null)
            {
                channel += cd.m_channelName;
            }

            string strNote="";

            if (param.m_flag == 0)
            {
                // ������ ������ĳĳ������С��Ϸ���أ�״̬������رգ�����Ҫ�󣺲��㱶�ʣ�VIP�ȼ�
                strNote += "������";
                strNote += channel;
                strNote += "����С��Ϸ���أ�״̬��";

                if (param.m_isCloseAll)
                {
                    strNote += "����ر�";
                }
                else
                {
                    strNote += "���忪��";
                }

                strNote += "������Ҫ�󣺲��㱶�ʣ�";

                var fl = Fish_LevelCFG.getInstance().getValue(param.m_gameLevel);
                if (fl != null)
                {
                    strNote += Convert.ToString(fl.m_openRate) + "��";
                }
                if (param.m_gameLevel == 0)
                {
                    strNote += "������";
                }

                strNote += "��vip�ȼ���";

                if (param.m_vipLevel == -1)
                {
                    strNote += "������";
                }
                else
                {
                    strNote += param.m_vipLevel + "��";
                }
            }
            else 
            {
                strNote += "ɾ����"+channel+"����С��Ϸ��������";
            }
            return string.Format(info.m_fmt, strNote);
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// ���ʼ�
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
            return "���ÿ�";

        LogSendMail param = BaseJsonSerializer.deserialize<LogSendMail>(str);
        if (param != null)
        {
            if (param.m_playerList == "")
            {
                return string.Format(info.m_fmt, "ȫ��", param.m_title, param.m_sender, param.m_content, param.m_itemList, param.m_validDay);
            }
            return string.Format(info.m_fmt, param.m_playerList, param.m_title, param.m_sender, param.m_content, param.m_itemList, param.m_validDay);
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////////
//��һ�����Ϣ�޸�
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
            return "���ÿ�";

        LogPlayerBasicInfoChg param = BaseJsonSerializer.deserialize<LogPlayerBasicInfoChg>(str);
        if (param != null)
        {
            string str_chg = "";
            switch (param.m_op)
            {
                case 1: str_chg = "����"; break;
                case 2: str_chg = "����ֵ"; break;
            }
            string str_info = "����� [" + param.m_playerId + "] ��" + str_chg+ "�޸�Ϊ [" + param.m_param + "]"; 

            return string.Format(info.m_fmt, str_info);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////////
//�߼����淨����
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
            return "���ÿ�";

        LogFishlordAdvancedRoomCtrl param = BaseJsonSerializer.deserialize<LogFishlordAdvancedRoomCtrl>(str);
        if (param != null)
        {
            string str_info = "";
            switch(param.m_op)
            {
                case 0: str_info = "��һ�Ƚ������Ϊ [" + param.m_maxWinCount + "] �� ���������Ϊ [" + param.m_ratio + "]%"; break;
                case 1: str_info = "�����Ƚ������Ϊ [" + param.m_maxWinCount + "] �� ���������Ϊ [" + param.m_ratio + "]%"; break;
                case 2: str_info = "�����Ƚ������Ϊ [" + param.m_maxWinCount + "] �� ���������Ϊ [" + param.m_ratio + "]%"; break;
                case 3: str_info = "����������ֵ��Ϊ [" + param.m_ratio + "]%"; break;
                case 4: str_info = "������ϵ����Ϊ [" + param.m_ratio + "]%"; break;
                case 5: str_info = "��С����ˮ��Ϊ [" + param.m_ratio + "]%"; break;  
            }
            return string.Format(info.m_fmt, str_info);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
/////////////////////////////////////////////////////////////////////////////
//ϵͳ��Ͷ����
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
            return "���ÿ�";

        LogAirDropSysPublish param = BaseJsonSerializer.deserialize<LogAirDropSysPublish>(str);
        if (param != null)
        {
            string type_str = (param.m_op == 0) ? "����" : "ɾ��";
            return string.Format(info.m_fmt, type_str, param.m_uuid);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
/////////////////////////////////////////////////////////////////////////////
//ʵ����˹���
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
            return "���ÿ�";

        LogServiceExchangeAudit param = BaseJsonSerializer.deserialize<LogServiceExchangeAudit>(str);
        if (param != null)
        {
            string str_op = param.m_op == 1 ? "δͨ��" : "ͨ��";
            return string.Format(info.m_fmt, param.m_exchangeId, str_op);
        }

        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////////
//���㵯ͷͳ����������ϵ��
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
            return "���ÿ�";

        LogBulletHeadRangeCorrectParamEdit param = BaseJsonSerializer.deserialize<LogBulletHeadRangeCorrectParamEdit>(str);
        if (param != null)
        {
            string type_str = (param.m_type == 4) ? "ʹ�÷�Χ" : "ɱ�㷶Χ";
            return string.Format(info.m_fmt, param.m_bulletHeadId,param.m_rangeCorrectParam,type_str);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
////////////////////////////////////////////////////////////////////////////
//��������߻����޸�
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
            return "���ÿ�";

        LogFishlordRobotMaxScoreEdit param = BaseJsonSerializer.deserialize<LogFishlordRobotMaxScoreEdit>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_robotMaxScore);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// ��Ϸ������
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
            return "���ÿ�";

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
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////////
// ��Ϸ���۱���������
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
            return "���ÿ�";

        LogGameBzResult param = BaseJsonSerializer.deserialize<LogGameBzResult>(str);
        if (param != null)
        {
            string result = StrName.s_bzArea[param.m_result-1];
            return string.Format(info.m_fmt, StrName.s_roomName[param.m_roomId - 1], result);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////////
// ������ֻ�
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
                op = "��";
                op_param = "�������Ϊ" + param.m_phone;
            }else if (param.m_op == "unbind")
            {
                op = "���";
            }
            return string.Format(info.m_fmt, op, param.m_playerId,op_param);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// ��Ϸ�ں�
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
            return "���ÿ�";

        LogGameShcdResult param = BaseJsonSerializer.deserialize<LogGameShcdResult>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, StrName.s_shcdRoomName[param.m_roomId - 1], StrName.s_shcdArea[param.m_result]);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////////
//����ڻ���û�ɱ����
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
            return "���ÿ�";

        LogNdActPlayerFishCountReset param = BaseJsonSerializer.deserialize<LogNdActPlayerFishCountReset>(str);
        if (param != null)
        {

            return string.Format(info.m_fmt, param.m_playerId, param.m_fishCount);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
/////////////////////////////////////////////////////////////////////////////

//��ʥ�ڻ����Ϲ���������
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
            return "���ÿ�";

        LogHallowmasPumpkinCountReset param = BaseJsonSerializer.deserialize<LogHallowmasPumpkinCountReset>(str);
        if (param != null)
        {

            return string.Format(info.m_fmt, param.m_playerId, param.m_pumpkinCount);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
////////////////////////////////////////////////////////////////////////////
//���ᱦ���һ�ɱ�����������
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
            return "���ÿ�";

        LogSpittorSnatchKillCountReset param = BaseJsonSerializer.deserialize<LogSpittorSnatchKillCountReset>(str);
        if (param != null)
            return string.Format(info.m_fmt, param.m_playerId, param.m_killCount);
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
///////////////////////////////////////////////////////////////////////////
//ը����԰��һ�������
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
            return "���ÿ�";

        LogStatBulletHeadPlayerScoreAlter param = BaseJsonSerializer.deserialize<LogStatBulletHeadPlayerScoreAlter>(str);
        if (param != null)
        {
            string type = "";
            switch (param.m_type)
            {
                case 0: type = "��ͨ����"; break;
                case 1: type = "��ͭ����"; break;
                case 2: type = "��������"; break;
                case 3: type = "�ƽ�����"; break;
                case 4: type = "��ʯ����"; break;
            }
            return string.Format(info.m_fmt, param.m_playerId, type, param.m_score);
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}

///////////////////////////////////////////////////////////////////////////
//���������һ�������
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
            return "���ÿ�";

        LogStatFishlordGrandPrixActPlayerScoreAlter param = BaseJsonSerializer.deserialize<LogStatFishlordGrandPrixActPlayerScoreAlter>(str);
        if (param != null)
        {
            string type = "";
            switch (param.m_type)
            {
                case 0: type = "��������"; break;
                case 1: type = "��������"; break;
            }
            return string.Format(info.m_fmt, param.m_playerId, type, param.m_score);
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
///////////////////////////////////////////////////////////////////////////
//���а���һ�������
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
            return "���ÿ�";

        LogOperationRankPlayerScoreAlter param = BaseJsonSerializer.deserialize<LogOperationRankPlayerScoreAlter>(str);
        if (param != null)
        {
            string type = "";
            switch (param.m_type)
            {
                case 0: type = "�������"; break;
                case 1: type = "��ͭ����"; break;
                case 2: type = "��������"; break;
                case 3: type = "�ƽ�����"; break;
                case 4: type = "��ʯ����"; break;
            }
            return string.Format(info.m_fmt, param.m_playerId, type, param.m_score);
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
///////////////////////////////////////////////////////////////////////////
//��������
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
            return "���ÿ�";

        LogFishlordRoomCheat param = BaseJsonSerializer.deserialize<LogFishlordRoomCheat>(str);
        if (param != null)
        {
            string type = "����";
            switch (param.m_type)
            {
                case 1: type = "��ǰ�ܻ���"; break;
                case 2: type = "������߻���"; break;
            }
            return string.Format(info.m_fmt, StrName.s_roomList[param.m_roomId], param.m_playerId, type, param.m_score);
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
///////////////////////////////////////////////////////////////////////////
//�ں�ڰ���������
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
            return "���ÿ�";

        LogShcdCardsSpecilList param = BaseJsonSerializer.deserialize<LogShcdCardsSpecilList>(str);
        if (param != null)
        {
            string op = "";
            switch(param.m_op)
            {
                case 0: op = "����"; break;
                case 1: op = "����"; break;
            }

            string type = "";
            switch(param.m_type)
            {
                case 0: type = "�ڰ�����"; break;
                case 1: type = "������"; break;
                case 2: type = "������"; break;
            }
            string game=StrName.s_gameName3[param.m_gameId];
            return string.Format(info.m_fmt, param.m_playerId, op, type,game);
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// �����������ˮ����
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
            return "���ÿ�";

        LogBaojinPumpRate param = BaseJsonSerializer.deserialize<LogBaojinPumpRate>(str);
        if (param != null)
        {
            string strRoomName = "";
            if (param.m_roomId == 11)
            {
                strRoomName += "[����������]";
            }
            return string.Format(info.m_fmt, strRoomName, param.m_pumpRate);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//���Ͷ��ʱ������
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
            return "���ÿ�";

        LogOperationAd100003ActSet param = BaseJsonSerializer.deserialize<LogOperationAd100003ActSet>(str);
        if (param != null)
        {
            string strRoomName = "�༭��";
            switch(param.m_key)
            {
                case 1: strRoomName += "��100003 - ����"; break;
                case 2: strRoomName += "��100009 - ��«����"; break;
                case 3: strRoomName += "��100010 - ����׬"; break;
                case 4: strRoomName += "��100011 - ��׬"; break;
                case 5: strRoomName += "��100012 - ����׬"; break;
                case 6: strRoomName += "��100013 - ������"; break;
                case 7: strRoomName += "��100014 - С��"; break;
                case 8: strRoomName += "��100015 - ����׬"; break;
            }
            
            strRoomName += "���Ͷ�Żʱ�䡿";
            return string.Format(info.m_fmt, strRoomName, param.m_time);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// �������÷��޸�
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
            return "���ÿ�";

        LogBaojinScoreAlter param = BaseJsonSerializer.deserialize<LogBaojinScoreAlter>(str);
        if (param != null)
        {
            string strTitle = "";
            if (param.m_flag)
            {
                strTitle = " �����ֵ�������ֵ ";
            }
            else 
            {
                strTitle = " �����ֵ ";
            }
            return string.Format(info.m_fmt, param.m_playerId,strTitle, param.m_newScore);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//������������޸�
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
            return "���ÿ�";

        LogDragonScaleNumAlter param = BaseJsonSerializer.deserialize<LogDragonScaleNumAlter>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_playerId, param.m_oldNum, param.m_newScore);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//���ｵ��/������������޸�
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
            return "���ÿ�";

        LogFishlordLegendaryRankScoreAlter param = BaseJsonSerializer.deserialize<LogFishlordLegendaryRankScoreAlter>(str);
        if (param != null)
        {
            string str_note = "��IDΪ["+ param.m_playerId +"]�������";
            switch(param.m_op)
            {
                case 0:
                    str_note += "���ｵ���Ļ��ָ�Ϊ��";
                    break;
                case 1:
                    str_note += "�������뵥ը���ָ�Ϊ��";
                    break;
                case 2:
                    str_note += "����������ը���ָ�Ϊ��";
                    break;
            }

            str_note += param.m_newScore;
            return string.Format(info.m_fmt, str_note);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//ʥ�޳������޸�
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
            return "���ÿ�";

        LogFishlordMythicalRankScoreAlter param = BaseJsonSerializer.deserialize<LogFishlordMythicalRankScoreAlter>(str);
        if (param != null)
        {
            string str_note = "��IDΪ[" + param.m_playerId + "]�������ʥ�޳����ָ�Ϊ";

            str_note += param.m_newScore;
            return string.Format(info.m_fmt, str_note);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//����±����޸�
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
            return "���ÿ�";

        LogJinQiuNationalDayActAlter param = BaseJsonSerializer.deserialize<LogJinQiuNationalDayActAlter>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_playerId, param.m_oldNum, param.m_newScore);
        }
        return "";
    }
    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// ��IP
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
            return "���ÿ�";

        LogBlockIP param = BaseJsonSerializer.deserialize<LogBlockIP>(str);
        if (param != null)
        {
            if (param.m_isBlock)
            {
                return string.Format(info.m_fmt, "��", param.m_ip);
            }
            return string.Format(info.m_fmt, "���", param.m_ip);
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}

//////////////////////////////////////////////////////////////////////////

// ���˺�
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
            return "���ÿ�";

        LogBlockAcc param = BaseJsonSerializer.deserialize<LogBlockAcc>(str);
        if (param != null)
        {
            if (param.m_isBlock)
            {
                return string.Format(info.m_fmt, "��", param.m_acc);
            }
            return string.Format(info.m_fmt, "���", param.m_acc);
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}

//////////////////////////////////////////////////////////////////////////

// ��������
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
            return "���ÿ�";

        LogResetPwd param = BaseJsonSerializer.deserialize<LogResetPwd>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_acc, param.m_phone);
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}

//////////////////////////////////////////////////////////////////////////

// ��̨��ֵ
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
            return "���ÿ�";

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

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// ��ID
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
            return "���ÿ�";

        LogBlockId param = BaseJsonSerializer.deserialize<LogBlockId>(str);
        if (param != null)
        {
            if (param.m_isBlock)
            {
                return string.Format(info.m_fmt, "��", param.m_playerId);
            }
            return string.Format(info.m_fmt, "���", param.m_playerId);
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}

//////////////////////////////////////////////////////////////////////////
//�޸�����ʱ�������
[Serializable]
class LogModifyActivityPanicBuyingCfg : OpParam
{
    // ��б�
    public string m_activityList = "";

    // ������
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
            return "���ÿ�";

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

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//���˺�̨���� �㷨�����޸�
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
            return "���ÿ�";

        LogModifyFishlordSingleCtrlParam param = BaseJsonSerializer.deserialize<LogModifyFishlordSingleCtrlParam>(str);
        if (param != null)
            return string.Format(info.m_fmt, param.m_baseRate, param.m_deviationFix, param.m_checkRate);

        return "";
        
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// �޸��˷��������ӯ����
[Serializable]
class LogModifyFishlordRoomExpRate : OpParam
{
    // �����б�
    public string m_roomList = "";   
    
    // ӯ����ʵ��ֵ
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
            return "���ÿ�";

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

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//�޸��˱��۱������
[Serializable]
class LogModifyBzParam : OpParam
{
    // �����б�
    public string m_roomList = "";
    //�����Ĳ���
    public int m_op;
    //����ֵ
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
            return "���ÿ�";

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
            case 1: op_param = "������ע��ֵ"; break;
            case 2: op_param = "�����÷ŷָ���"; break;
            case 3: op_param = "С���÷ŷָ���"; break;
            case 4: op_param = "�����ɱ�ָ���"; break;
            case 5: op_param = "С����ɱ�ָ���"; break;
        }

        return string.Format(info.m_fmt, room, op_param, param.m_value);
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//�޸������������
[Serializable]
class LogModifyCrocodileParam : OpParam
{
    // �����б�
    public string m_roomList = "";
    //�����Ĳ���
    public int m_op;
    //����ֵ
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
            return "���ÿ�";

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
            case 1: op_param = "������ע��ֵ"; break;
            case 2: op_param = "�����÷ŷָ���"; break;
            case 3: op_param = "С���÷ŷָ���"; break;
            case 4: op_param = "�����ɱ�ָ���"; break;
            case 5: op_param = "С����ɱ�ָ���"; break;
        }

        return string.Format(info.m_fmt, room, op_param, param.m_value);
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//��ˮ�ز�������
[Serializable]
class LogModifyFishlordCtrlNewParam : OpParam
{
    // �����б�
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
            return "���ÿ�";

        LogModifyFishlordCtrlNewParam param = BaseJsonSerializer.deserialize<LogModifyFishlordCtrlNewParam>(str);
        if (param != null) 
        {
            string roomName = StrName.s_roomList[param.m_roomId];

            string strinfo = "��������Ϊ��";

            //if (param.m_roomId == 8)
            //    strinfo += "���ת��ϵ��[" + param.m_legendaryFishRate/1000.0 + "]��";

            //�߼���
            if(param.m_roomId == 3)
                strinfo += " �󽱳�ˮϵ�� [" + param.m_jsckpotGrandPump + "]��С����ˮϵ��[ " + param.m_jsckpotSmallPump + "]��С���ˮϵ�� [" + param.m_normalFishRoomPoolPumpParam + "]��";

            strinfo += "��ˮ�� [" + param.m_earnRatemCtrMax + "]������ӯ���� [" + param.m_earnRatemCtrMin +
                        "]����������ֵ [" + param.m_incomeThreshold + "]�����ʿ��� [" + param.m_baseRate + "]�� ˢ������ [" + param.m_checkRate + "]";

            //�м��� �߼��� ������
            if (param.m_roomId == 2 || param.m_roomId == 3 || param.m_roomId == 5)
                strinfo += " ���淨���ֵ [" + param.m_trickDeviationFix + "]";

            if (param.m_roomId == 2) {
                strinfo += " �����������һ���� [" + param.m_colorFishCtrCount1 + "], ��������������� [" + param.m_colorFishCtrCount2 + 
                    "], ��ɫ������ϵ����һ����[" +  param.m_colorFishCtrRate1 + "], ��ɫ������ϵ����������[" + param.m_colorFishCtrRate2 + "]";
            }

            if (param.m_roomId == 9) 
            {
                strinfo += ", ��ȸת������ϵ�� [" + param.m_mythicalScoreTurnRate + "], �����۷�ϵ�� [ " + param.m_mythicalFishRate + " ]" ;
            }

            return string.Format(info.m_fmt, roomName, strinfo);
        }

        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
///////////////////////////////////////////////////////////////////////////
// �����˷��������ӯ����
[Serializable]
class LogResetFishlordRoomExpRate : OpParam
{
    // �����б�
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
            return "���ÿ�";

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

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
//����ˮ䰴���ɱ����
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
            return "���ÿ�";

        LogSetScorePool param = BaseJsonSerializer.deserialize<LogSetScorePool>(str);
        if (param.m_playerId == "")
            return "";

        string str_note = "";

        str_note += StrName.s_gameName3[param.m_gameId]+"��ɱ���ͣ�";
        str_note += "������IDΪ[" + param.m_playerId + "]����Ҳ�����";

        string typeName = "";
        if (param.m_type == 1)
        {
            typeName += "���";
        }
        else if (param.m_type == 19)
        {
            typeName += "������Ƭ";
        }

        if (param.m_op == 0) //���
        {
            if (param.m_gameId == (int)GameId.shuihz)
            {
                str_note += "���" + typeName + "BUFFˮ��Ϊ" + param.m_setValue;
            }
            else 
            {
                str_note += "���" + typeName + "BUFFˮ��Ϊ" + param.m_setValue + ",�������䵽������Ϊ" + param.m_time;
            }
        }else if(param.m_op==3)
        {
            str_note += "��������" + typeName + "BUFFˮ�ص�ǰ����ֵ";
        }
        else  //ɾ��
        {
            str_note += "�Ƴ�����" + typeName + "BUFF";
        }
        return string.Format(info.m_fmt, str_note);
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
///////////////////////////////////////////////////////////////////////////
//�����˻��ֹ���
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
            return "���ÿ�";

        LogFishlordRobotRankCFG param = BaseJsonSerializer.deserialize<LogFishlordRobotRankCFG>(str);
        if (param != null)
        {
            string str_type = "";
            switch(param.m_op){
                case 1:
                    str_type = "�м������˰�";break;
                case 2:
                    str_type = "�м���ţ�˰�"; break;
                case 3:
                    str_type = "�߼������а�"; break;
                case 4:
                    str_type = "��װ�����а�"; break;
                case 5:
                    str_type = "���ﳡ���а�"; break;
                case 6:
                    str_type = "����ը-��ͨ"; break;
                case 7:
                    str_type = "����ը-��ͭ"; break;
                case 8:
                    str_type = "����ը-����"; break;
                case 9:
                    str_type = "����ը-�ƽ�"; break;
                case 10:
                    str_type = "����ը-��ʯ"; break;
                case 11:
                    str_type = "˥��ը-��ͨ"; break;
                case 12:
                    str_type = "˥��ը-��ͭ"; break;
                case 13:
                    str_type = "˥��ը-����"; break;
                case 14:
                    str_type = "˥��ը-�ƽ�"; break;
                case 15:
                    str_type = "˥��ը-��ʯ"; break;
            }
            return string.Format(info.m_fmt,str_type, param.m_param);
        }
        return "";

    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
///////////////////////////////////////////////////////////////////////////
// ����ͷ��
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
            return "���ÿ�";

        LogFreezeHead param = BaseJsonSerializer.deserialize<LogFreezeHead>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_playerId, param.m_deadTime);
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
///////////////////////////////////////////////////////////////////////////
// �޸Ļ���ñ�ʱ��
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
            return "���ÿ�";

        LogOperationActivityCFGEdit param = BaseJsonSerializer.deserialize<LogOperationActivityCFGEdit>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_itemId, param.m_startTime, param.m_endTime);
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}
//////////////////////////////////////////////////////////////////////////
// ����ţţ����
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
            return "���ÿ�";

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

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}

//////////////////////////////////////////////////////////////////////////
// ף������
[Serializable]
class LogWishCurse : OpParam
{
    public int m_gameId;     // ��ϷID
    public int m_playerId;   // ���ID
    public int m_wishType;   // ף����������
    public int m_opType;     // �������ͣ���� or ȥ��
    
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
            return "���ÿ�";

        LogWishCurse param = BaseJsonSerializer.deserialize<LogWishCurse>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt,
                StrName.getGameName3(param.m_gameId),
                param.m_playerId,
                param.m_opType == 0 ? "���" : "ȥ��",
                StrName.s_wishCurse[param.m_wishType]);
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}

//////////////////////////////////////////////////////////////////////////
// ������Բ���
[Serializable]
class LogPlayerOp : OpParam
{
    public int m_playerId;     // ���ID
    public int m_op;           // ��������
    public string m_prop;      // ����
    public int m_value;        // ֵ

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
            return "���ÿ�";

        LogPlayerOp param = BaseJsonSerializer.deserialize<LogPlayerOp>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt,
                param.m_playerId,
                param.m_op == 1 ? "����" : "����",
                param.m_value,
                getProp(param));
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }

    public string getProp(LogPlayerOp p)
    {
        if (p.m_prop == "gold")
            return "���";

        if (p.m_prop == "gem")
            return "��ʯ";

        if (p.m_prop == "vip")
            return "vip����";

        if (p.m_prop == "dragonBall")
            return "����";

        if (p.m_prop == "chip")
            return "��Ƭ";

        if (p.m_prop == "moshi")
            return "ħʯ";

        if (p.m_prop == "xp")
            return "��Ҿ���";

        return "";
    }
}

//////////////////////////////////////////////////////////////////////////
// ���¼��ر��
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
            return "���ÿ�";

        LogReloadTable param = BaseJsonSerializer.deserialize<LogReloadTable>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt,
                StrName.getGameName3(param.m_gameId));
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}

//////////////////////////////////////////////////////////////////////////
// �����˺�����
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
            return "���ÿ�";

        LogOldNewAcc param = BaseJsonSerializer.deserialize<LogOldNewAcc>(str);
        if (param != null)
        {
            return string.Format(info.m_fmt, param.m_oldId, param.m_newId);
        }
        return "";
    }

    // ����Ҫ�������ݿ�Ĳ�����
    public override string getString()
    {
        return BaseJsonSerializer.serialize(this);
    }
}