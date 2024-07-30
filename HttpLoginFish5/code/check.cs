using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

// 登陆时检测，是否封IP或账号
public class LoginCheck
{
    // 0不限制一个设备创建账号的数量
    public static int m_LimitRegCount = 0;

    static LoginCheck()
    {
        string isLimit = ConfigurationManager.AppSettings["LimitReg"];
        if (!string.IsNullOrEmpty(isLimit))
        {
            m_LimitRegCount = Convert.ToInt32(isLimit);
        }
    }

    public static string checkIP(HttpRequest request)
    {
        // 封账号
        string remoteIp = request.ServerVariables.Get("Remote_Addr").ToString();
        bool res = MongodbAccount.Instance.KeyExistsBykey("blockIP", "ip", remoteIp);
        if (res)
        {
            return "-100"; // 停封IP
        }
        return "";
    }

    // 可否注册账号
    public static bool canRegAcc(string deviceId)
    {
        if (m_LimitRegCount == 0 || string.IsNullOrEmpty(deviceId))
        {
            return true;
        }

        Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetOneBykey(CONST.DEVICE_MAP_ACC, "deviceId", deviceId);
        if (data == null)
            return true;

        try
        {
            int count = Convert.ToInt32(data["count"]);
            if (count >= m_LimitRegCount)
            {
                return false;
            }
            return true;
        }
        catch (System.Exception ex)
        {

        }
        return false;
    }

    public static void incRegAcc(string deviceId)
    {
        if (m_LimitRegCount == 0 || string.IsNullOrEmpty(deviceId))
        {
            return;
        }

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("count", 1);

        IMongoQuery imq = Query.EQ("deviceId", deviceId);
        MongodbAccount.Instance.ExecuteIncByQuery(CONST.DEVICE_MAP_ACC, imq, data);
    }
}

public struct CONST
{
    // 启用登录失败检测
    public static int USE_LOGIN_FAILED_COUNT_CHECK = Convert.ToInt32(WebConfigurationManager.AppSettings["useLoginFailedCountCheck"]);

    // 登录失败允许的最大次数
    public static int LOGIN_FAILED_MAX_COUNT = Convert.ToInt32(WebConfigurationManager.AppSettings["loginFailedMaxCount"]);

    // 登录失败字段
    public static string[] LOGIN_FAILED_FIELD = { "loginFailedDate", "loginFailedCount", "pwd", "updatepwd", "block", "acc_real" };

    // 设备到账号数量
    public const string DEVICE_MAP_ACC = "deviceMapAccCount";
}

////////////////////////////////////////////////////////////////////////////////////
// 极光广告包回调
public class Advertisement
{
    public const string ADVERT_TABLE = "advertisement_xgt";
    public const int OP_ACTIVE = 1;
    public const int OP_REG = 2;

    public static bool isCheck(string channel)
    {
        return channel == "800020";
    }

    public static void call(int op, string deviceId)
    {
        if (op == OP_ACTIVE)
        {
            callActive(deviceId);
        }
        else if (op == OP_REG)
        {
            callReg(deviceId);
        }
    }

    // 激活的回调
    static void callActive(string deviceId)
    {
        string muid = Common.Helper.getMD5(deviceId);
        Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetOneBykey(ADVERT_TABLE, "muid", muid);
        if (data != null)
        {
            bool isActiveCall = false;
            if (data.ContainsKey("isActiveCall"))
            {
                isActiveCall = Convert.ToBoolean(data["isActiveCall"]);
            }
            if (!isActiveCall && data.ContainsKey("active_cb"))
            {
                string cb = Convert.ToString(data["active_cb"]);
                try
                {
                    HttpPost.Get(new Uri(cb));

                    Dictionary<string, object> updata = new Dictionary<string, object>();
                    updata.Add("isActiveCall", true);
                    MongodbAccount.Instance.ExecuteUpdate(ADVERT_TABLE, "muid", muid, updata);
                }
                catch (System.Exception ex)
                {
                }
            }
        }
    }

    // 注册的回调
    static void callReg(string deviceId)
    {
        string muid = Common.Helper.getMD5(deviceId);
        Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetOneBykey(ADVERT_TABLE, "muid", muid);
        if (data != null)
        {
            bool isRegCall = false;
            if (data.ContainsKey("isRegCall"))
            {
                isRegCall = Convert.ToBoolean(data["isRegCall"]);
            }

            if (!isRegCall && data.ContainsKey("reg_cb"))
            {
                string cb = Convert.ToString(data["reg_cb"]);
                try
                {
                    HttpPost.Get(new Uri(cb));

                    Dictionary<string, object> updata = new Dictionary<string, object>();
                    updata.Add("isRegCall", true);
                    MongodbAccount.Instance.ExecuteUpdate(ADVERT_TABLE, "muid", muid, updata);
                }
                catch (System.Exception ex)
                {
                }
            }
        }
    }
}
