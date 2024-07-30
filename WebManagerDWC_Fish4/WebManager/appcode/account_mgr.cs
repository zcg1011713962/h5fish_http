using System;
using System.Web.SessionState;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public class AccountInfo
{
    public string m_user = "";
    public string m_right = "";
    public string m_pwd = "";
    public string m_ip = "";  // �ʺ�IP
    public string m_type = "";
}

public enum enumLoginResult
{
    result_success,      // �ɹ�
    result_has_login,    // �ѵ�½
    result_error,        // �˺Ż��������
}

public class LoginResult
{
    public AccountInfo m_info;
    public enumLoginResult m_code;

    public bool isSuccess()
    {
        return m_code == enumLoginResult.result_success;
    }
}

public class AccountType
{
    public AccountType(string type, string name)
    {
        m_type = type;
        m_name = name;
    }

    public string m_type;
    public string m_name;
}

class AccountMgr
{
    private static AccountMgr s_mgr = null;
    // �洢��½�û����û�����Ựӳ��
    private Dictionary<string, HttpSessionState> m_user = new Dictionary<string, HttpSessionState>();
    // �˺������б�
    private List<AccountType> m_accountType = new List<AccountType>();

    public static AccountMgr getInstance()
    {
        if (s_mgr == null)
        {
            s_mgr = new AccountMgr();
            s_mgr.initAccount();
        }
        return s_mgr;
    }

    // ��½����
    public LoginResult onLogin(string user, string pwd)
    {
        LoginResult result = new LoginResult();
        string user_flag = "GM_" + user;

        DBMgr db = DBMgr.getInstance();

        AccountInfo info = db.loadUser(user_flag);
        if (info == null)
        {
            result.m_code = enumLoginResult.result_error;
            return result;
        }

        result = new LoginResult();
        result.m_code = info.m_pwd == Tool.getMD5Hash(pwd) ? enumLoginResult.result_success : enumLoginResult.result_error;
        result.m_info = info;
        return result;
    }

    public void onLoginSuccess(HttpSessionState session, LoginResult res, string ip, bool keep)
    {
        if (res.isSuccess())
        {
            res.m_info.m_ip = ip;
            GMUser gmusr = new GMUser(res.m_info);
            gmusr.init();

            // �����û���Ϣ
            session["user"] = gmusr;
            if (m_user.ContainsKey(res.m_info.m_user))
            {
                m_user[res.m_info.m_user].Abandon();
                m_user[res.m_info.m_user] = session;
            }
            else
            {
                m_user[res.m_info.m_user] = session;
            }
            if (keep)
            {
                // ����һ��ĵ�½״̬
                session.Timeout = 24 * 60; 
            }
        }
    }

    // �Ự�˳�
    public void sessionEnd(HttpSessionState session)
    {
        if (session["user"] == null)
            return;

        GMUser info = (GMUser)session["user"];
        if (m_user.ContainsKey(info.m_user))
        {
            m_user.Remove(info.m_user);
        }
    }

    // �û�user�Ƿ��ѵ�½
    public bool isLogin(string user)
    {
        return m_user.ContainsKey(user);
    }

    public List<AccountType> getAccountTypeList()
    {
        return m_accountType;
    }

    public AccountType getAccountTypeByType(string type)
    {
        int i = 0;
        for (; i < m_accountType.Count; i++)
        {
            if (m_accountType[i].m_type == type)
            {
                return m_accountType[i];
            }
        }
        return null;
    }

    // ����һ���˺�
    public bool addAccount(string account, string key1, string key2, int sel, GMUser user)
    {
        bool res = true;
        if (account == null || account == "")
            res = false;
        if (key1 != key2)
            res = false;
        if (key1 == null)
        {
            res = false;
        }
        if (sel < 0 || sel >= m_accountType.Count)
            res = false;
        // ����˺�
        if (res)
        {
            Dictionary<string, object> data = genAccount("GM_" + account, m_accountType[sel].m_type, key1);
            res = DBMgr.getInstance().addTableData(TableName.GM_ACCOUNT, data, "user", Convert.ToString(data["user"]), 0, DbName.DB_ACCOUNT);

            if (res) // ���LOG
            {
                //OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_ADD_ACCOUNT, new ParamAddAccount(account), user);
            }
        }
        user.setOpResult(res ? OpRes.opres_success : OpRes.op_res_failed);
        return res;
    }

    // ����һ���˺�
    public bool addAccount(string account, string key1, string key2, string gmType, GMUser user)
    {
        bool res = true;
        if (account == null || account == "")
            res = false;
        if (key1 != key2)
            res = false;
        if (key1 == null)
        {
            res = false;
        }
        
        // ����˺�
        if (res)
        {
            Dictionary<string, object> data = genAccount("GM_" + account, gmType, key1);
            res = DBMgr.getInstance().addTableData(TableName.GM_ACCOUNT, data, "user", Convert.ToString(data["user"]), 0, DbName.DB_ACCOUNT);

            if (res) // ���LOG
            {
                //OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_ADD_ACCOUNT, new ParamAddAccount(account), user);
            }
        }
        user.setOpResult(res ? OpRes.opres_success : OpRes.op_res_failed);
        return res;
    }

    public OpRes delAccount(string account, GMUser user)
    {
        bool res = DBMgr.getInstance().remove(TableName.GM_ACCOUNT, "user", account, 0, DbName.DB_ACCOUNT);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    // �����˺ŷ�����Ϣ
    public void updateAccount(List<GMAccountItem> gm_list)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        for (int i = 0; i < gm_list.Count; i++)
        {
            data["type"] = gm_list[i].m_type;
            DBMgr.getInstance().update(TableName.GM_ACCOUNT, data, "user", gm_list[i].m_user, 0, DbName.DB_ACCOUNT);
        }
    }

    public OpRes updateAccountType(string acc, string newGmType)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["type"] = newGmType;
        bool res = DBMgr.getInstance().update(TableName.GM_ACCOUNT, data, "user", acc, 0, DbName.DB_ACCOUNT);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    // ��ʼ��Ĭ���ʺ�
    private void initAccount()
    {
        List<Dictionary<string, object>> accountlist = new List<Dictionary<string, object>>();
        accountlist.Add(genAccount("GM_admin", "admin", "123456"));
       
        foreach(Dictionary<string, object> acc in accountlist)
        {
            DBMgr.getInstance().addTableData(TableName.GM_ACCOUNT, acc, "user", Convert.ToString(acc["user"]), 0, DbName.DB_ACCOUNT);
        }
        
        // �˺����͵��б�
        m_accountType.Add(new AccountType("program", "����"));
        m_accountType.Add(new AccountType("plan", "�߻�"));
        m_accountType.Add(new AccountType("operation", "��Ӫ"));
        m_accountType.Add(new AccountType("service", "�ͷ�"));
        m_accountType.Add(new AccountType("opDirector", "��Ӫ�ܼ�"));
        m_accountType.Add(new AccountType("ceo", "CEO"));

        bool res = true;
        foreach (var at in m_accountType)
        {
            res = addDefaultGmType(at.m_type, at.m_name);
        }
        if (res)
        {
            RightMgr.getInstance().readRightSet();
        }
    }

    // ����һ���˺�
    private Dictionary<string, object> genAccount(string user, string type, string password)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["user"] = user;
        data["type"] = type;
        data["password"] = Tool.getMD5Hash(password);
        return data;
    }

    // ����Ĭ�ϵ�gm����
    bool addDefaultGmType(string gmTypeId, string gmTypeName)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["typeName"] = gmTypeName;
        data["id"] = gmTypeId;
        data["genTime"] = DateTime.Now;

        return DBMgr.getInstance().addTableData(TableName.GM_TYPE, data, "id", gmTypeId, 0, DbName.DB_ACCOUNT);
    }
}



















