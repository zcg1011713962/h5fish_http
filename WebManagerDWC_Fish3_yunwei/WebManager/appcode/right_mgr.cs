//#define _OLD_RIGHT_
using System;
using System.Web.SessionState;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;

/*
    all:ӵ������Ȩ��
*/

// Ȩ�޵ļ����
enum RightResCode
{
    right_success,              // �ɹ�
    right_not_login,            // δ��½
    right_no_right,             // û��Ȩ�޲���
    right_need_switch,          // ��Ҫ�л�������
}

// Ȩ�޹���
class RightMgr
{
    private static RightMgr s_mgr = null;
    // Ȩ�޴�
    Dictionary<string, string> m_rightMap = new Dictionary<string, string>();

    // �����ݿ��ж�����Ȩ�޴�����Ա����->Ȩ�޴�
    Dictionary<string, string> m_rightCheck = new Dictionary<string, string>();

    // GM��Ա����-->Ȩ�޼���
    Dictionary<string, RightSet> m_rs = new Dictionary<string, RightSet>();

    public static RightMgr getInstance()
    {
        if (s_mgr == null)
        {
            s_mgr = new RightMgr();
            s_mgr.init();
        }
        return s_mgr;
    }
#if _OLD_RIGHT_
    // �ж�account�Ƿ����Ȩ��right
    public RightResCode hasRight(string right, HttpSessionState session, string className = "")
    {
        if (session["user"] == null)
        {
            return RightResCode.right_not_login;
        }

        if (right == "")
            return RightResCode.right_success;

        GMUser account = (GMUser)session["user"];
        if(!m_rightCheck.ContainsKey(account.m_type))
            return RightResCode.right_no_right;

        if (!account.isSwitchDbServer)
            return RightResCode.right_need_switch;

        string r = m_rightCheck[account.m_type];

        // ��������Ȩ�ޣ�ֱ�ӷ���true
        if (r.IndexOf("all") >= 0)
            return RightResCode.right_success;

        if (r.IndexOf(right) >= 0)
            return RightResCode.right_success;

        return RightResCode.right_no_right;
    }

    public bool canEdit(string right, GMUser user)
    {
        return true;
    }
#else
    public RightResCode hasRight(string right, HttpSessionState session, string className = "")
    {
        if (session["user"] == null)
        {
            return RightResCode.right_not_login;
        }

        if(right == "")
            return RightResCode.right_success;

        GMUser user = (GMUser)session["user"];

        //if (!user.isSwitchDbServer)
        //    return RightResCode.right_need_switch;

        if (user.m_type == "admin")
            return RightResCode.right_success;

        if (!m_rs.ContainsKey(user.m_type))
            return RightResCode.right_no_right;

        RightSet rs = m_rs[user.m_type];
        if (!rs.canView(right))
            return RightResCode.right_no_right;

        return RightResCode.right_success;
    }  
    
    public bool canEdit(string right, GMUser user)
    {
        if(user.m_type == "admin")
            return true;

        if (!m_rs.ContainsKey(user.m_type))
            return false;

        RightSet rs = m_rs[user.m_type];
        return rs.canEdit(right);
    }

    public bool canEditSpecial(string right, GMUser user) //�ʼ� Ȩ�ޣ�����admin��service
    {
        if (user.m_type == "admin"||user.m_type=="service")
            return true;

        if (!m_rs.ContainsKey(user.m_type))
            return false;

        RightSet rs = m_rs[user.m_type];
        return rs.canEdit(right);
    }
 
#endif

    // �Ե�ǰҪ���еĲ������м���
    public bool opCheck(string right, HttpSessionState session, HttpResponse response, string className = "")
    {
        RightResCode code = hasRight(right, session);
        if (code == RightResCode.right_success)
            return true;
        if (code == RightResCode.right_not_login)
        {
            string str = string.Format("<script>location.href='{0}';</script>", DefCC.ASPX_LOGIN);
            response.Write(str);
            response.End();
            //response.Redirect("~/Account/Login.aspx");
        }
        if (code == RightResCode.right_no_right)
        {
            response.Redirect("~/appaspx/Error.aspx?right=" + right);
        }
        if (code == RightResCode.right_need_switch)
        {
            response.Redirect("~/appaspx/Error.aspx?right=" + "@@");
        }
        return false;
    }


    // ����Ȩ������
    public string getRrightName(string right)
    {
        if (m_rightMap.ContainsKey(right))
            return m_rightMap[right];
        return "";
    }

    // ��ȡȨ���б�
    public Dictionary<string, string> getRightMap()
    {
        return m_rightMap;
    }

    // ��Ȩ�ޱ��ȡ����Ȩ��
    public List<Dictionary<string, object>> getAllRight()
    {
        List<Dictionary<string, object>> data = DBMgr.getInstance().executeQuery(TableName.RIGHT, 0, DbName.DB_ACCOUNT, null);
        return data;
    }

    // ��Ȩ�ޱ��ȡ����Ȩ��
    public string getRight(string type)
    {
        List<Dictionary<string, object>> data = DBMgr.getInstance().executeQuery(TableName.RIGHT, 0, DbName.DB_ACCOUNT, null);
        foreach (Dictionary<string, object> dt in data)
        {
            if (Convert.ToString(dt["type"]) == type)
            {
                string str = Convert.ToString(dt["right"]);
                return str;
            }
        }
        return "";
    }

    // �޸���ԱȨ��
    public bool modifyRight(string type, string right)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["type"] = type;
        data["right"] = right;
        m_rightCheck[type] = right;
        return DBMgr.getInstance().save(TableName.RIGHT, data, "type", type, 0, DbName.DB_ACCOUNT);
    }

    public string getRightJson()
    {
        Dictionary<string, object> ret = new Dictionary<string, object>();

        // ��Ա�����б�
        List<Dictionary<string, object>> gmTypeList = new List<Dictionary<string, object>>();
        var drs = from d in m_rs
                       orderby d.Value.m_genTime
                       select d;
        foreach (var d in drs)
        {
            Dictionary<string, object> dt = new Dictionary<string, object>();
            dt.Add("gmTypeId", d.Key);
            dt.Add("gmTypeName", d.Value.m_gmTypeName);
            gmTypeList.Add(dt);
        }
        ret.Add("gmTypeList", gmTypeList);

        // ÿ��GM���͵�Ȩ��
        foreach (var d in m_rs)
        {
            Dictionary<string, object> dt1 = new Dictionary<string, object>();
            ret.Add(d.Key, dt1);

            foreach (var sd in d.Value.getRightSet())
            {
                Dictionary<string, object> dt2 = new Dictionary<string, object>();
                dt1.Add(sd.Key, dt2);

                dt2.Add("canEdit", sd.Value.m_canEdit);
                dt2.Add("canView", sd.Value.m_canView);
            }
        }

        List<Dictionary<string, object>> rightList = new List<Dictionary<string, object>>();
        // ����Ȩ��
        var allRight = ResMgr.getInstance().getBaseRight();

        foreach (var d in allRight)
        {
            Dictionary<string, object> dt = new Dictionary<string, object>();
            dt.Add("rightId", d.Key);
            dt.Add("rightName", d.Value.m_rightName);
            dt.Add("category", d.Value.m_category);
            rightList.Add(dt);
        }
        ret.Add("rightList", rightList);

        return ItemHelp.genJsonStr(ret);
    }

    public OpRes modifyGmTypeRight(string gmType, string fun, string right)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add(fun, right);
        bool res = DBMgr.getInstance().update(TableName.GM_TYPE, data, "id", gmType, 0, DbName.DB_ACCOUNT);
        if (res)
        {
            RightSet rst = null;
            if (m_rs.ContainsKey(gmType))
            {
                rst = m_rs[gmType];
            }
            else
            {
                rst = new RightSet();
                m_rs.Add(gmType, rst);
            }
            rst.addRightByStr(fun, right);
        }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    // �������е�GM����
    public List<AccountType> getAllGmType()
    {
        List<AccountType> result = new List<AccountType>();

        var drs = from d in m_rs
                  orderby d.Value.m_genTime
                  select d;
        foreach (var d in drs)
        {
            AccountType at = new AccountType(d.Key, d.Value.m_gmTypeName);
            result.Add(at);

        }
        return result;
    }

    public string getGmTypeName(string gmType)
    {
        if (m_rs.ContainsKey(gmType))
        {
            return m_rs[gmType].m_gmTypeName;
        }
        return "";
    }

    private void init()
    {
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(HttpRuntime.BinDirectory + "..\\" + "data\\right.xml");

            XmlNode node = doc.SelectSingleNode("/configuration");

            for (node = node.FirstChild; node != null; node = node.NextSibling)
            {
                string right = node.Attributes["right"].Value;
                string fmt = node.Attributes["fmt"].Value;
                if (m_rightMap.ContainsKey(right))
                {
                    LOGW.Info("��right.xmlʱ�������˴��󣬳������ظ���Ȩ�� {0}", right);
                }
                else
                {
                    m_rightMap.Add(right, fmt);
                }
            }
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.Message);
            LOGW.Info(ex.StackTrace);
        }
        finally
        {
            modifyRight("admin", "all");  // ����Ա����
        }

        List<Dictionary<string, object>> allR = getAllRight();
        foreach (var d in allR)
        {
            string st = Convert.ToString(d["type"]);
            string rt = Convert.ToString(d["right"]);
            m_rightCheck[st] = rt;
        }

        readRightSet();
    }

    // ��ȡȨ�޼�
    public void readRightSet()
    {
        m_rs.Clear();
        List<Dictionary<string, object>> dataList =
            DBMgr.getInstance().executeQuery(TableName.GM_TYPE, 0, DbName.DB_ACCOUNT, null, 0, 0, null, "genTime");
        if (dataList == null || dataList.Count == 0)
            return;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            RightSet rs = new RightSet();
            string typeId = "";
            DateTime time = DateTime.Now;

            foreach (var d in data)
            {
                if (d.Key == "_id")
                {
                    continue;
                }
                else if (d.Key == "id")
                {
                    typeId = Convert.ToString(d.Value);
                }
                else if (d.Key == "typeName")
                {
                    rs.m_gmTypeName = Convert.ToString(d.Value);
                }
                else if (d.Key == "genTime")
                {
                    time = Convert.ToDateTime(d.Value).ToLocalTime();
                }
                else
                {
                    rs.addRightByStr(d.Key, Convert.ToString(d.Value));
                }
            }
            rs.setTime(time);
            m_rs.Add(typeId, rs);
        }
    }
}

// Ȩ�޻�����Ϣ
public class RightInfo
{
    public string m_rightName; // Ȩ�����ƣ���RightDef����
    public bool m_canEdit;     // �ɷ�༭
    public bool m_canView;     // �ɷ�鿴
}

// Ȩ�޼���
public class RightSet
{
    // Ȩ������-->Ȩ����Ϣ
    Dictionary<string, RightInfo> m_rs = new Dictionary<string, RightInfo>();

    public string m_gmTypeName;
    public DateTime m_genTime;

    public RightInfo getRightInfo(string rightName)
    {
        if (m_rs.ContainsKey(rightName))
            return m_rs[rightName];

        return null;
    }

    public bool canEdit(string rightName)
    {
        RightInfo info = getRightInfo(rightName);
        return info == null ? false : info.m_canEdit;
    }

    public bool canView(string rightName)
    {
        RightInfo info = getRightInfo(rightName);
        return info == null ? false : info.m_canView;
    }

    public void reset()
    {
        m_rs.Clear();
    }

    public Dictionary<string, RightInfo> getRightSet()
    {
        return m_rs;
    }

    public bool addRight(string rightName, bool canView, bool canEdit)
    {
        RightInfo info = null;
        if (m_rs.ContainsKey(rightName))
        {
            info = m_rs[rightName];
        }
        else
        {
            info = new RightInfo();
            info.m_rightName = rightName;
            m_rs.Add(rightName, info);
        }

        info.m_canView = canView;
        info.m_canEdit = canEdit;

        return true;
    }

    // rs��,��� ��ʾ �ɲ鿴 �ɱ༭
    public bool addRightByStr(string rightName, string rs)
    {
        if (string.IsNullOrEmpty(rs))
            return true;

        string[] arr = Tool.split(rs, ',', StringSplitOptions.RemoveEmptyEntries);
        return addRight(rightName, arr[0] == "1", arr[1] == "1");
    }

    public void setTime(DateTime t)
    {
        m_genTime = t;
    }
}

//////////////////////////////////////////////////////////////////////////
#if _OLD_RIGHT_
public struct RightDef
{
    // gm�˺����͵ı༭������޸�
    public const string GM_TYPE_EDIT = "-1";

    public const string OP_PLAYER_MONEY_CHANGE = "operation";    // ��ҽ�ұ仯��ϸ
    public const string OP_BG_RECHARGE = "operation";            // ��̨��ֵ
    public const string OP_OPERATION_NOTICE = "operation";       // ��Ӫ����
    public const string OP_NOTIFY_MSG = "operation";             // ͨ����Ϣ
    public const string OP_MAINTENANCE_NOTICE = "operation";     // ά������
    public const string OP_MONEY_WARN = "operation";             // ���Ԥ��
    public const string OP_CUR_ONLINE_NUM = "operation";         // ��ǰ��������
    public const string OP_FREEZE_HEAD = "operation";            // ����ͷ��
    public const string OP_INFORM_HEAD = "operation";            // �ٱ�ͷ��
    public const string OP_CHANNEL_EDIT = "operation";            // �������Ա༭
    public const string OP_MONEY_RANK = "operation";             // �����������
    public const string OP_WISH_CURSE = "operation";             // ף������
    public const string OP_MAX_ONLINE = "operation";             // �������
    public const string OP_PLAYER_TOTAL_MONEY = "operation";     // ��ҽ���ܺ�
    public const string OP_PLAYER_OP = "operation";              // �����ز���
    public const string OP_RECHARGE_POINT = "operation";         // ���ѵ�ͳ��
    public const string OP_WEEK_CHAMPION_SETTING = "operation";         // �����ھ�����

    public const string SVR_ACCOUNT_QUERY = "service";         // �˺Ų�ѯ
    public const string SVR_RECHARGE_QUERY = "service";         // ��ֵ��¼��ѯ
    public const string SVR_SEND_MAIL = "service";             // ���ʼ�
    public const string SVR_MAIL_CHECK = "service";             // �ʼ����
    public const string SVR_RESET_PLAYER_PWD = "service";        // ��������
    public const string SVR_BLOCK_PLAYER_ID = "service";        // ͣ�����ID
    public const string SVR_EXCHANGE_MGR = "service";        // �һ�����
    public const string SVR_ADD_SERVICE_INFO = "service";        // ���ӿͷ���Ϣ

    public const string TD_ACTIVE_DATA = "operation";        // ��Ծ����
    public const string TD_LTV = "operation";                // LTVͳ��
    public const string TD_R_LOSE = "operation";                // ��R��ʧ
    public const string TD_RECHARGE_PLAYER_STAT = "operation";                // ��ֵ�û�ͳ��
    public const string TD_PLAYER_DB_MONITOR = "operation";                // ���������
    public const string TD_DAILY_DB = "operation";                // ÿ������
    public const string TD_RECHARGE_PLAYER_MONITOR = "operation";                // ��ֵ��Ҽ��
    public const string TD_INCOME_EXPENSES = "operation";                // ��Ծ�����֧ͳ��
    public const string TD_RECHARGE_PER_HOUR = "operation";                // ʵʱ����
    public const string TD_ONLINE_PER_HOUR = "operation";                // ʵʱ����

    public const string DATA_OLD_EARINGS_RATE = "stat";                // ӯ�������ò�ѯ
    public const string DATA_PLAYER_FAVOR = "stat";                // ���ϲ��
    public const string DATA_INDEPEND_LOBBY = "stat";                // �������ݴ���
    public const string DATA_MONEY_FLOW = "stat";                // ��Ϸ�������ͳ��
    public const string DATA_TOTAL_CONSUME = "stat";                // ���������ܼ�
    public const string DATA_GAME_INCOME_EACH_DAY = "stat";                // ��Ϸÿ������ͳ��
    public const string DATA_RELOAD_TABLE = "stat";                // ���¼��ر��
    public const string DATA_PLAYER_LOSE = "stat";                // ��ʧ��ѯ
    public const string DATA_SHOP_EXCHANGE = "stat";                // �̳Ƕһ�
    public const string DATA_STAR_LOTTERY = "stat";                // ���ǳ齱

    public const string FISH_PARAM_CONTROL = "stat";                // ���䲶���������
    public const string FISH_DESK_EARNING_QUERY = "stat";                // ���䲶������ӯ����
    public const string FISH_ALG_ANALYZE = "stat";                // ���䲶���㷨����
    public const string FISH_STAT = "stat";                // ���ͳ��
    public const string FISH_INDEPEND_DATA = "stat";                // ��������-����
    public const string FISH_CONSUME = "stat";                // ��������
    public const string FISH_WEEK_CHAMPION = "stat";                // �����ܹھ�
    public const string FISH_CHAMPION_RANK = "stat";                // ��������
    public const string FISH_BOSS_CONSUME = "stat";                // BOSS����
    public const string FISH_BOSS_CONTROL = "stat";                // ���䲶��BOSS����

    public const string CROD_PARAM_CONTROL = "stat";                // �������������
    public const string CROD_INDEPEND_DATA = "stat";                // ��������-����
    public const string CROD_RESULT_CONTROL = "stat";                // ������������

    public const string DICE_EARINGS = "stat";                // ����ӯ����
    public const string DICE_INDEPEND_DATA = "stat";                // ��������-����
    public const string DICE_RESULT_CONTROL = "stat";                // �����������

    public const string BACC_PARAM_CONTROL = "stat";                // �ټ��ֲ�������
    public const string BACC_PLAYER_BANKER = "stat";                // ��ׯ���
    public const string BACC_RESULT_CONTROL = "stat";                // �ټ��ֽ������

    public const string COW_PARAM_CONTROL = "stat";                // ţţ��������
    public const string COW_PLAYER_BANKER = "stat";                // ţţ��ׯ��ѯ
    public const string COW_INDEPEND_DATA = "stat";                // ţţ��������
    public const string COW_CARD_TYPE = "stat";                    // ţţ��������

    public const string D5_PARAM_CONTROL = "stat";                    // ������������
    public const string D5_EARNINGS = "stat";                         // ��������ӯ�����

    public const string SHCD_RESULT_CONTROL = "stat";                // �ں�÷���������
    public const string SHCD_PARAM_CONTROL = "stat";                 // �ں�÷����������
    public const string SHCD_INDEPEND_DATA = "stat";                 // ��������-�ں�÷��

    public const string CALF_PARAM_CONTROL = "stat";                 // ��ţ��������
    public const string CALF_INDEPEND_DATA = "stat";                 // ��ţ��������
    public const string CALF_LEVEL_DATA = "stat";                    // ��ţ�ؿ�����
    public const string CALF_CLASS_STAT = "stat";                    // ��ţ����ͳ��

    public const string OTHER_VIEW_LOG = "viewlog";                    // �鿴��־
}

#else

// Ȩ�����͵Ķ���
public struct RightDef
{
    // gm�˺����͵ı༭������޸�
    public const string GM_TYPE_EDIT = "-1";
    public const string GM_CD_KEY = "-2";

    public const string OP_PLAYER_MONEY_CHANGE = "1";    // ��ҽ�ұ仯��ϸ
    public const string OP_BG_RECHARGE = "2";            // ��̨��ֵ
    public const string OP_OPERATION_NOTICE = "3";       // ��Ӫ����
    public const string OP_NOTIFY_MSG = "4";             // ͨ����Ϣ
    public const string OP_MAINTENANCE_NOTICE = "5";     // ά������
    public const string OP_MONEY_WARN = "6";             // ���Ԥ��
    public const string OP_CUR_ONLINE_NUM = "7";         // ��ǰ��������
    public const string OP_FREEZE_HEAD = "8";            // ����ͷ��
    public const string OP_INFORM_HEAD = "9";            // �ٱ�ͷ��
    public const string OP_CHANNEL_EDIT = "10";            // �������Ա༭
    public const string OP_MONEY_RANK = "11";             // �����������
    public const string OP_WISH_CURSE = "12";             // ף������
    public const string OP_MAX_ONLINE = "13";             // �������
    public const string OP_PLAYER_TOTAL_MONEY = "14";     // ��ҽ���ܺ�
    public const string OP_PLAYER_OP = "15";              // �����ز���
    public const string OP_RECHARGE_POINT = "16";         // ���ѵ�ͳ��
    public const string OP_WEEK_CHAMPION_SETTING = "17";  // �����ھ�����
    public const string OP_POLAR_LIGHTS_PUSH = "18";      //��������
    public const string OP_FORCE_UPDATE_REWARD = "19";      //ǿ��������ѯ
    public const string OP_PLAYER_ITEM_RECORD = "20";       //��ҵ��߻�ȡ����
    public const string OP_WORD2_LOGIC_ITEM_ERROR = "21";   //���ͬ������ʧ��
    public const string OP_GET_PLAYERID_BY_ORDERID = "22";  //ͨ�������������Ų�ѯ���ID
    public const string OP_PLAYER_RICHES_RANK = "23";       //��ҲƸ���
    public const string OP_GAME_REAL_TIME_LOSE_WIN_LIST = "24"; //С��Ϸʵʱ��Ӯ
    public const string OP_STAT_PLAYER_MONEY_REP = "25";   //�������ͳ��
    public const string OP_PLAYER_ACT_TURRET = "26";    //�����ɳ��ֲ�
    public const string OP_PLAYER_ACT_TURRET_BY_SINGLE = "27"; //����ڱ��ɳ��ֲ�
    public const string OP_OPERATION_GAME_CTRL = "28"; //��Ϸ����
    public const string OP_STAT_TURRET_ITEMS_ON_PLAYER = "29"; //���ƽ��Я����Ʒ
    public const string OP_STAT_RANK_RECHARGE = "30"; //��ֵ���а�
    public const string OP_ACTIVITY_CFG = "31";//����ñ�
    public const string OP_FISHLORD_ROBOT_RANK_CFG = "32"; //�����˻��ֹ���
    public const string OP_STAT_RANK_CHEAT = "33"; //���а�����
    public const string OP_STAT_AD_PUTIN = "34"; //���Ͷ��ͳ��

    public const string SVR_ACCOUNT_QUERY = "1001";         // �˺Ų�ѯ
    public const string SVR_RECHARGE_QUERY = "1002";         // ��ֵ��¼��ѯ
    public const string SVR_SEND_MAIL = "1003";             // ���ʼ�
    public const string SVR_MAIL_CHECK = "1004";             // �ʼ����
    public const string SVR_RESET_PLAYER_PWD = "1005";        // ��������
    public const string SVR_BLOCK_PLAYER_ID = "1006";        // ͣ�����ID
    public const string SVR_EXCHANGE_MGR = "1007";          // �һ�����
    public const string SVR_ADD_SERVICE_INFO = "1008";       // ���ӿͷ���Ϣ
    public const string SVR_UN_BLOCK_PLAYER_ID = "1009";   //������ID
    public const string DATA_PLAYER_CHAT = "1010";        //��������¼��ѯ
    public const string SVR_CHANNEL_OPEN_CLOSE_GAME = "1011"; //С��Ϸ��������
    public const string SVR_FASTER_START_FOR_VISITOR = "1012";//�οͿ��ٿ�ʼ����˺�
    public const string SVR_MAIL_QUERY = "1013";    //�ʼ���ѯ
    public const string SVR_BIND_UNBIND_PHONE = "1014";  //�󶨽������ֻ�
    public const string SVR_REPLACEMENT_ORDER = "1015";  //�ͷ�����/�����/��������-ϵͳ
    public const string SVR_SELECT_LOSS_PLAYER = "1016"; //��ʧ��ɸѡ
    public const string SVR_GUIDE_LOST_PLAYERS = "1017"; //��ʧ�����������Ӽ�¼
    public const string SVR_PLAYER_BASIC_INFO = "1018"; //��һ�����Ϣ
    public const string SVR_EXCHANGE_AUDIT = "1019"; //ʵ����˹���

    public const string TD_ACTIVE_DATA = "2001";        // ��Ծ����
    public const string TD_LTV = "2002";                // LTVͳ��
    public const string TD_R_LOSE = "2003";                // ��R��ʧ
    public const string TD_RECHARGE_PLAYER_STAT = "2004";                // ��ֵ�û�ͳ��
    public const string TD_PLAYER_DB_MONITOR = "2005";                // ���������
    public const string TD_DAILY_DB = "2006";                // ÿ������
    public const string TD_RECHARGE_PLAYER_MONITOR = "2007";                // ��ֵ��Ҽ��
    public const string TD_INCOME_EXPENSES = "2008";                // ��Ծ�����֧ͳ��
    public const string TD_RECHARGE_PER_HOUR = "2009";                // ʵʱ����
    public const string TD_ONLINE_PER_HOUR = "2010";                // ʵʱ����
    public const string TD_NEW_PLAYER_RECHARGE_MONITOR = "2011";  //�½���Ҹ��Ѽ��
    public const string DATA_STAT_ONLINE_REWARD = "2012"; //���߽���
    public const string DATA_STAT_NEW_GUILD_LOSE_POINT = "2013"; //��ʧ��ͳ��
    public const string DATA_STAT_NEW_PLAYER_OPENRATE = "2014"; //�����ڱ������
    public const string DATA_STAT_INNER_PLAYER = "2015"; //�ڲ���
    public const string TD_ACTIVE_OLD_PLAYER_DATA = "2016";//���û��������

    public const string DATA_OLD_EARINGS_RATE = "3001";                // ӯ�������ò�ѯ
    public const string DATA_PLAYER_FAVOR = "3002";                // ���ϲ��
    public const string DATA_INDEPEND_LOBBY = "3003";                // �������ݴ���
    public const string DATA_MONEY_FLOW = "3004";                // ��Ϸ�������ͳ��
    public const string DATA_TOTAL_CONSUME = "3005";                // ���������ܼ�
    public const string DATA_GAME_INCOME_EACH_DAY = "3006";                // ��Ϸÿ������ͳ��
    public const string DATA_RELOAD_TABLE = "3007";                // ���¼��ر��
    public const string DATA_PLAYER_LOSE = "3008";                // ��ʧ��ѯ
    public const string DATA_SHOP_EXCHANGE = "3009";                // �̳Ƕһ�
    public const string DATA_STAR_LOTTERY = "3010";                // ���ǳ齱
    public const string DATA_FESTIVAL_ACTIVITY = "3011";           //���ջ ���ӣ�
    public const string DATA_BUYU_LOG = "3012";                    //����LOG���ӣ�
    public const string DATA_SEVEN_DAY_ACTIVITY = "3013";          //���ջ
    public const string DATA_DAILY_MATERIAL_GIFT = "3014";          //�������ÿ�չ���
    public const string DATA_DIAL_LOTTERY = "3015";                // ת�̳齱
    public const string DATA_ACTIVITY_PANIC_BUYING_CFG = "3016";   //��ʱ�������
    public const string DATA_LABA_LOTTERY = "3017";                //���Գ齱
    public const string DATA_COLLECT_PUPPET = "3018";               //����ż
    public const string DATA_WP_ACTIVITY_STAT = "3019";             //����ʢ��
    public const string DATA_FISHLORD_FEAST_STAT= "3020";           //����ʢ��
    public const string DATA_PLAYER_ICON_CUSTOM = "3021";           //����Զ���ͷ��ͳ��
    public const string DATA_PUMP_DAILY_SIGN = "3022";              //ÿ��ǩ��
    public const string DATA_NEW_PLAYER_TASK = "3023";              //��������
    public const string DATA_DAILY_TASK = "3024";                   //ÿ������
    public const string DATA_NATIONAL_DAY_ACTIVITY = "3025";        //����ڻ
    public const string DATA_JIN_QIU_RECHARGE_LOTTERY = "3026";     //�����ػݻ
    public const string DATA_HALLOWMAS_ACTIVITY = "3027";           //��ʥ�ڻ
    public const string DATA_CHRISTMAS_YUANDAN = "3028";            //ʥ����/Ԫ���
    public const string DATA_SCRATCH_ACT = "3029";                  //�ι��ֻ
    public const string DATA_STAT_NY_GIFT = "3030";                 //�������
    public const string DATA_STAT_NY_ACC_RECHARGE = "3031";         //�´��ط�
    public const string DATA_STAT_NY_ADVENTURE = "3032";            //���ߴ�ð��
    public const string DATA_STAT_BULLET_HEAD_RANK = "3033";        //����ըըը
    public const string DATA_STAT_WUYI_REWARD_RESULT = "3034";      //��һ�䷵�
    public const string DATA_PUMP_NEW_GUIDE = "3035";               //�����������
    public const string DATA_STAT_SPITTOR_SNATCH_ACT = "3036";      //���ᱦ�
    public const string DATA_STAT_WORLD_CUP_MATCH = "3037";         //���籭��������
    public const string DATA_STAT_WORLD_CUP_MATCH_PLAYER_JOIN = "3038"; //���籭�󾺲����Ѻעͳ��
    public const string DATA_STAT_PANIC_BOX = "3039";               //����˱���
    public const string DATA_STAT_DRAGON_SCALE_RANK = "3040";       //��������
    public const string DATA_STAT_DRAGON_SCALE_CONTROL = "3041";    //���������޸�
    public const string DATA_STAT_JINQIU_NATIONAL_ACT = "3042";     //�����������
    public const string DATA_STAT_JINQIU_NATIONAL_ACT_CTRL = "3043";//���������±��޸�
    public const string DATA_STAT_PLAYER_BW = "3044";   //���䳡����ͳ��
    public const string DATA_STAT_WJLW_DEF_RECHARGE_REWARD = "3045"; //Χ�������Զ��嵱��ĸ���������յ��������
    public const string DATA_STAT_WJLW_GOLD_EARN = "3046";          //Χ����������淨ͳ��
    public const string DATA_STAT_WJLW_RECHARGE_EARN = "3047";  //Χ�����������淨ͳ��
    public const string DATA_STAT_BULLET_HEAD_GIFT = "3048";    //��ͷ���ͳ��
    public const string DATA_STAT_KD_ACT_RANK = "3049"; //������
    public const string DATA_STAT_FISHLORD_LOTTERY_EXCHANGE = "3050";   //��ȯ��ͳ��
    public const string DATA_STAT_DAILY_WEEK_TASK = "3051";     //ÿ�������񼰽���ͳ��
    public const string DATA_STAT_GOLD_FISH_LOTTERY = "3052";  //���˳齱
    public const string DATA_STAT_MAINLY_TASK = "3053";     //��������
    public const string FISH_LORD_MIDDLE_ROOM_ACT = "3054"; //�м�������ͳ��
    public const string FISH_LORD_MIDDLE_ROOM_CTRL = "3055";//�м�������
    public const string FISH_LORD_ADVANCED_ROOM_CTRL = "3056"; //�߼������ؿ���
    public const string FISH_LORD_ADVANCED_ROOM_ACT = "3057";   //�߼�������ͳ��
    public const string FISH_LORD_STAT_DAILY_GIFT = "3058"; //ÿ�����ͳ��
    public const string DATA_STAT_TREASURE_HUNT = "3059";    //�Ϻ�Ѱ��
    public const string DATA_STAT_TOMORROW_GIFT = "3061"; //�������
    public const string DATA_STAT_MIDDLE_ROOM_EXCHANGE = "3062"; //�м������ͳ��
    public const string DATA_STAT_SD_ACT = "3063"; //�����ͳ��
    public const string DATA_STAT_SD_ACT_LOTTERY = "3064"; //�����齱ͳ��
    public const string FISH_LORD_ADVANCED_ROOM_CHEAT_CTRL = "3066";//�߼�������
    public const string FISHLORD_BULLET_HEAD_RANK_CHEAT = "3067"; //ը����԰����
    public const string FISH_LORD_SHARK_ROOM_ACT = "3068";//���賡����ͳ��
    public const string FISH_LORD_SHARK_ROOM_CHEAT_CTRL = "3069"; //���賡����
    public const string DATA_STAT_NATIONAL_DAY_ACT = "3071";//ʮһ�
    public const string DATA_STAT_KILL_CRAB_ACT = "3072"; //׷��з���
    public const string DTAT_STAT_TOTAL_PLAYER_OPENRATE_TASK = "3073"; //�ڱ�����
    public const string DTAT_STAT_PLAYER_ITEM_RECHARGE = "3074"; //��������
    public const string DATA_STAT_REBATE_GIFT = "3075"; //�������
    public const string DATA_TURRET_CHIP = "3076"; //������Ƭͳ��
    public const string DATA_STAT_PLAYER_MAIL = "3077"; //����ʼ�ͳ��
    public const string DATA_STAT_PUMP_GROW_FUND = "3078"; //�ɳ�����
    public const string DATA_STAT_PUMP_VIP_GIFT = "3079"; //vip��Ȩ���
    public const string DATA_STAT_PUMP_MONTH_CARD = "3080"; //�¿�����ͳ��
    public const string DATA_STAT_PLAT_AD = "3081"; //������Ƶͳ��
    public const string DATA_STAT_YUAN_DAN_SIGN = "3082"; //Ԫ��ǩ��
    public const string DATA_STAT_WECHAT_GIFT = "3083";//���ں�ͳ��
    public const string DATA_STAT_VIP_EXCLUSIVE = "3084"; //VIPר��
    public const string DATA_STAT_DAILY_HOUR_MAX_ONLINE_PLAYER = "3085"; //ʱ����������
    public const string DATA_STAT_FISHLORD_LEGENDARY_RANK = "3086";//���ｵ��/�����������а�
    public const string DATA_STAT_FISHLORD_LEGENDARY_RANK_CHEAT = "3087"; //���ｵ��/������������
    public const string DATA_STAT_FISHLORD_LEGENDARY_FISH_ROOM = "3088"; //���ﳡ�淨
    public const string DATA_STAT_PUMP_HUNT_FISH_RECHARGE_ACT = "3089";//���λ
    public const string DATA_STAT_PUMP_NP_7_DAY_RECHARGE_ACT = "3090"; //��������
    public const string DATA_STAT_PUMP_RED_ENVELOP = "3091"; //�������
    public const string DATA_STAT_PUMP_WUYI_SET_2020_ACT = "3092"; //��һ�
    public const string DATA_STAT_PUMP_RED_ENVELOP_EXCHANGE = "3093"; //��������һ�
    public const string DATA_STAT_PUMP_MYTHICAL_ROOM_ACT = "3094";//ʥ�޳��淨ͳ��
    public const string DATA_STAT_PUMP_MYTHICAL_ROOM_RANK_CHEAT = "3095";// ʥ�޳���ǰ������һ���
    public const string DATA_STAT_PUMP_FISH = "3096"; //�淨��ͳ��
    public const string DATA_STAT_GROWTH_QUEST = "3097"; //������ͳ��

    public const string FISH_PARAM_CONTROL = "4001";                // ���䲶���������
    public const string FISH_DESK_EARNING_QUERY = "4002";          // ���䲶������ӯ����
    public const string FISH_ALG_ANALYZE = "4003";                // ���䲶���㷨����
    public const string FISH_STAT = "4004";                // ���ͳ��
    public const string FISH_INDEPEND_DATA = "4005";                // ��������-����
    public const string FISH_CONSUME = "4006";                // ��������
    public const string FISH_WEEK_CHAMPION = "4007";                // �����ܹھ�
    public const string FISH_CHAMPION_RANK = "4008";                // ��������
    public const string FISH_BOSS_CONSUME = "4009";                // BOSS����
    public const string FISH_BOSS_CONTROL = "4010";                // ���䲶��BOSS����
    public const string FISH_STAT_FISHLORD_BAOJIN = "4011";              //�����������������
    public const string FISH_STAT_FISHLORD_BAOJIN_CONTROL = "4012";              //�����������������
    public const string FISH_STAT_FISHLORD_BAOJIN_SCORE_CONTROL = "4013";    //�������÷��޸�
    public const string FISH_STAT_PUMP_CHIP_FISH = "4014";                       //������ͳ��
    public const string FISH_PLAYER_SCORE_POOL = "4015";                //�����ɱ����
    public const string FISH_LORD_BULLET_HEAD_HEAD = "4016";            //���㵯ͷͳ��
    public const string FISH_LORD_JINGJI_DATA_STAT = "4017";              //����������ͳ��
    public const string FISH_LORD_DRAGON_PALACE_DATA_STAT = "4018";     //������������ͳ��
    public const string FISH_LORD_BULLET_HEAD_OUTPUT = "4019";          //���㵯ͷ����ͳ��
    public const string FISH_LORD_CONTROL_NEW_SINGLE = "4020";          //���˺�̨����
    public const string FISH_LORD_AIR_DROP_SYS = "4021";                //ϵͳ��Ͷ
    public const string FISH_LORD_PLAYER_BANKRUPT = "4022";           //�Ʋ�ͳ��
    public const string FISH_LORD_PLAYER_OPENRATE_BANKRUPT_LIST = "4023"; //�Ʋ�����
    public const string FISH_STAT_GOLD_ON_PLAYER = "4024";  //���Я�����
    public const string FISH_LORD_AIR_DROP_SYS_PUB = "4025";   //ϵͳ��Ͷ����

    public const string CROD_PARAM_CONTROL = "5001";                // �������������
    public const string CROD_INDEPEND_DATA = "5002";                // ��������-����
    public const string CROD_RESULT_CONTROL = "5003";                // ������������
    public const string CROD_SPECIL_LIST_SET = "5004";              //������ڰ���������

    public const string DICE_EARINGS = "6001";                      // ����ӯ����
    public const string DICE_INDEPEND_DATA = "6002";                // ��������-����
    public const string DICE_RESULT_CONTROL = "6003";                // �����������

    public const string BACC_PARAM_CONTROL = "7001";                // �ټ��ֲ�������
    public const string BACC_PLAYER_BANKER = "7002";                // ��ׯ���
    public const string BACC_RESULT_CONTROL = "7003";                // �ټ��ֽ������

    public const string COW_PARAM_CONTROL = "8001";                // ţţ��������
    public const string COW_PLAYER_BANKER = "8002";                // ţţ��ׯ��ѯ
    public const string COW_INDEPEND_DATA = "8003";                // ţţ��������
    public const string COW_CARD_TYPE = "8004";                    // ţţ��������
    public const string COW_CARDS_QUERY = "8005";                  // ţţ�ƾֲ�ѯ
    public const string COW_CARDS_SPECIL_LIST = "8006";             //ţţ���úڰ�����
    public const string COW_CARDS_CTRL_LIST = "8007";               //ţţɱ�ַŷ�LOG��¼�б�

    public const string D5_PARAM_CONTROL = "9001";                    // ������������
    public const string D5_EARNINGS = "9002";                         // ��������ӯ�����

    public const string SHCD_RESULT_CONTROL = "10001";                // �ں�÷���������
    public const string SHCD_PARAM_CONTROL = "10002";                 // �ں�÷����������
    public const string SHCD_INDEPEND_DATA = "10003";                 // ��������-�ں�÷��
    public const string SHCD_CARDS_QUERY = "10004";                   //�ں�÷���ƾֲ�ѯ
    public const string SHCD_CARDS_SPECIL_LIST = "10005";             //�ں�÷�����úڰ�����
    public const string SHCD_CARDS_CTRL_LIST = "10006";               //�ں�÷��ɱ�ַŷ�LOG��¼�б�

    public const string SHUIHZ_TOTAL_EARNING = "12001";                // ˮ䰴���ӯ����
    public const string SHUIHZ_SINGLE_EARNING = "12002";              // ˮ䰴��������ӯ����
    public const string SHUIHZ_DAILY_STATE = "12003";                 // ˮ䰴�ÿ����Ϸ����鿴
    public const string SHUIHZ_REACH_LIMIT = "12004";                 // ˮ䰴�ÿ�մ�����������ͳ��
    public const string SHUIHZ_PLAYER_SCORE_POOL = "12005";            //ˮ䰴���ɱ����

    public const string CALF_PARAM_CONTROL = "11001";                 // ��ţ��������
    public const string CALF_INDEPEND_DATA = "11002";                 // ��ţ��������
    public const string CALF_LEVEL_DATA = "11003";                    // ��ţ�ؿ�����
    public const string CALF_CLASS_STAT = "11004";                    // ��ţ����ͳ��

    public const string BZ_RESULT_CONTROL = "13001";                //���۱���������
    public const string BZ_INDEPEND_DATA = "13002";                 //��������-���۱���
    public const string BZ_PARAM_CONTROL = "13003";                 //���۱���ε���
    public const string BZ_SPECIL_LIST_SET = "13004";               //���۱���ڰ���������

    public const string FRUIT_PARAM_CONTROL = "14001";              //ˮ������������
    public const string FRUIT_RESULT_CONTROL = "14002";             //ˮ�����������
    public const string FRUIT_INDEPEND_DATA = "14003";              //ˮ������������
    public const string FRUIT_SPECIL_LIST_SET = "14004";           //ˮ�����ڰ���������

    public const string OTHER_VIEW_LOG = "10000001";                  // �鿴��־
    public const string OTHER_CD_KEY = "10000002";                     //cdkey���ӣ�
    public const string OTHER_WECHAT_RECV_STAT = "10000003";            //΢�Ź��ں�ǩ��ͳ��


    public const string PLAYER_PLAYER_BASIC_INFO = "100001";   //��һ�����Ϣ

    public const string FISHING_ROOM_INFO = "200001";   //�泡���

    public const string OPNEW_GAME_INCOME_DATA = "300001";   //��������
    public const string OPNEW_GAME_ACTIVE_DATA = "300002";   //��Ծ����
    public const string OPNEW_PLAYER_RECHARGE = "300003";  //��ҳ�ֵ��Ϣ
    public const string OPNEW_TURRET_TIMES = "300004";//�ڱ����
    public const string OPNEW_PLAYER_OP = "300005";//�����ز���
}

#endif