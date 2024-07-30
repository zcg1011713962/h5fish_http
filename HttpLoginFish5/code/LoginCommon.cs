using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Configuration;
using Common;
using System.Text;
using NS_OpenApiV3;
using NS_SnsNetWork;
using NS_SnsSigCheck;
using NS_SnsStat;
using MongoDB.Driver.Builders;
using System.Runtime.Remoting.Messaging;
using MongoDB.Driver;

public struct RetCode
{
    public const int RET_SUCCESS = 0;
    public const int RET_FAIL = 1;
}

public class LoginCommon
{
    public const string AES_KEY = "&@*(#kas9081fajk";

    // 尝试登录
    // 返回 0正常  -10 账号或密码错误 -12 账号被冻结 -11 db服务器出错 
    // -16 未修改密码 -17 账号被停封
    public static int tryLogin(string acc, string pwd, string table)
    {
        int retCode = -10;
        Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetBykey(table, "acc", acc, CONST.LOGIN_FAILED_FIELD);
        if (data == null)
            return retCode;

        bool checkpwd = Convert.ToBoolean(ConfigurationManager.AppSettings["check_pwd"]);
        if (checkpwd)
        {
            if (data.ContainsKey("updatepwd") && !Convert.ToBoolean(data["updatepwd"]))
                return -16;//未修改密码
        }

        if (data.ContainsKey("block"))
        {
            bool isBlock = Convert.ToBoolean(data["block"]);
            if (isBlock)
                return -17;
        }

        int curFailedCnt = 0;
        if (data.ContainsKey("loginFailedCount"))
        {
            if (CONST.USE_LOGIN_FAILED_COUNT_CHECK == 1) // 启用了失败次数检测
            {
                DateTime cur = DateTime.Now.Date;
                if (data.ContainsKey("loginFailedDate"))
                {
                    DateTime Last = Convert.ToDateTime(data["loginFailedDate"]).ToLocalTime();

                    if (cur == Last)
                    {
                        curFailedCnt = Convert.ToInt32(data["loginFailedCount"]);
                        if (curFailedCnt >= CONST.LOGIN_FAILED_MAX_COUNT) // 账号被冻结了
                            return -12;
                    }
                }
            }
        }

        string dbPwd = Convert.ToString(data["pwd"]);
        if (pwd == dbPwd)
        {
            retCode = 0;
            curFailedCnt = 0;
        }
        else
        {
            curFailedCnt++;
        }

        if (CONST.USE_LOGIN_FAILED_COUNT_CHECK == 1)
        {
            Dictionary<string, object> updata = new Dictionary<string, object>();
            updata.Add("loginFailedDate", DateTime.Now.Date);
            updata.Add("loginFailedCount", curFailedCnt);

            string strerr = MongodbAccount.Instance.ExecuteUpdate(table, "acc", acc, updata);
            if (strerr != "")
            {
                retCode = -11;
            }
        }

        return retCode;
    }

    public static string genLuaRetString(Dictionary<string, object> data)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("ret = {};\n");

        foreach (var item in data)
        {
            if (item.Key == "_result")
            {
                builder.AppendFormat("ret.result = \"{0}\";\n", Convert.ToString(data["_result"]));
            }
            else
            {
                string v = Convert.ToString(item.Value);
                if (v.IndexOf("\"") > -1)
                {
                    builder.AppendFormat("ret.{0} = \"{1}\";\n", item.Key, v.Replace("\"", "\\\""));
                }
                else
                {
                    builder.AppendFormat("ret.{0} = \"{1}\";\n", item.Key, item.Value);
                }
            }
        }

        builder.Append("return ret;\n");

        return builder.ToString();
    }

    public static bool isAccLand()
    {
        return false;// return LoginTable.LOGIN_FOR == "fish3";
    }

    public static string channelToString(string channel)
    {
        if (string.IsNullOrEmpty(channel))
            return "000000";

        return channel.PadLeft(6, '0');
    }
}

public struct LoginTable
{
    public static string ACC_ANYSDK = "anysdk_login";

    public static string ACC_DEFAULT = "AccountTable";

    public static string PLATFORM_DEFAULT = "default";

    public static string PLATFORM_QUICKSDK = "quicksdk";

    public static string PLATFORM_WX_MINI = "wxmini";

    public static string PLATFORM_QQ_MINI = "qqmini";

    // QQ大厅
    public static string PLATFORM_QQ_GAME = "qqgame";

    public static string[] PLATFORM_LIST = { LoginTable.PLATFORM_QUICKSDK, PLATFORM_WX_MINI,
                                           PLATFORM_QQ_MINI};

    public static string getAccountTableByPlatform(string platform)
    {
        if (platform == PLATFORM_DEFAULT)
            return ACC_DEFAULT;

        return platform + "_acc";
    }
}

//////////////////////////////////////////////////////////////////////////
public class LoginBaseParam
{
    public string m_platform;
    public string m_acc;
    public string m_deviceId;
    public string m_channelId;
    public string m_loginTable;
    public HttpRequest Request;
    // 邀请人账号
    public string m_accInvite = "";

    public Action<Dictionary<string, object>> m_addDataFun;
}

public class LoginBase
{
    public const int CHK_PARAM_PLATFORM = 1 << 0;
    public const int CHK_PARAM_ACCOUNT = 1 << 1;
    public const int CHK_PARAM_DEVICEID = 1 << 2;
    public const int CHK_PARAM_CHANNEL = 1 << 3;
    public const int CHK_PARAM_ALL = CHK_PARAM_PLATFORM | CHK_PARAM_ACCOUNT | CHK_PARAM_DEVICEID | CHK_PARAM_CHANNEL;

    public static string[] S_FIELDS = { "acc_real" };

    public string UserSdk;

    public static Dictionary<string, object> getLoginUpData(HttpRequest Request,
                                                            DateTime now,
                                                            int randkey,
                                                            string deviceID)
    {
        Dictionary<string, object> updata = new Dictionary<string, object>();
        updata["randkey"] = randkey;
        updata["lasttime"] = now.Ticks;
        updata["lastip"] = Request.ServerVariables.Get("Remote_Addr").ToString();
        updata["deviceId"] = deviceID == null ? "" : deviceID;
        updata["lastLoginTime"] = DateTime.Now;
        return updata;
    }

    public bool chekOrReply(HttpRequest Request, HttpResponse rep, int flag)
    {
        string str = "";
        bool res = check(Request, flag, ref str);
        if (!res)
        {
            rep.ContentType = "text/plain";
            rep.Write(str);
        }
        return res;
    }

    public bool check(HttpRequest Request, int flag, ref string retstr)
    {
        if ((flag & CHK_PARAM_PLATFORM) > 0)
        {
            string s = Request.Form["platform"];
            if (string.IsNullOrEmpty(s))
            {
                retstr = JsonHelper.genJson(genDic(CC.ERR_PLATFORM, 0, 0, ""));
                return false;
            }
        }
        if ((flag & CHK_PARAM_ACCOUNT) > 0)
        {
            string s = Request.Form["acc"];
            if (string.IsNullOrEmpty(s))
            {
                retstr = JsonHelper.genJson(genDic(CC.ERR_ACC, 0, 0, ""));
                return false;
            }
        }
        if ((flag & CHK_PARAM_CHANNEL) > 0)
        {
            string s = Request.Form["channel"];
            if (string.IsNullOrEmpty(s))
            {
                retstr = JsonHelper.genJson(genDic(CC.ERR_DATA, 0, 0, ""));
                return false;
            }
        }
        return true;
    }

    // 登录处理 
    // loginTable 登录表
    public string doLogin(string loginTable, object param)
    {
        HttpRequest Request = (HttpRequest)param;
        LoginBaseParam objParam = new LoginBaseParam();
        objParam.m_platform = Request.Form["platform"];
        objParam.m_acc = Request.Form["acc"];
        objParam.m_deviceId = Request.Form["deviceID"];
        objParam.m_channelId = Request.Form["channel"];
        objParam.Request = Request;
        return startLogin(objParam);
    }

    public string startLogin(LoginBaseParam param)
    {
        var dic = startLogin2(param);
        return JsonHelper.ConvertToStr(dic);
        /* string platform = param.m_platform;
         string acc = param.m_acc;
         string deviceID = param.m_deviceId;
         string channelID = param.m_channelId;
         channelID = LoginCommon.channelToString(channelID);
         string loginTable = param.m_loginTable;
         HttpRequest Request = param.Request;

         string retStr = "";
         if (string.IsNullOrEmpty(platform))
         {
             retStr = (Helper.buildLuaReturn(-1, "platform is empty"));
             CLOG.Info("startLogin.doLogin, platform is empty");
             return retStr;
         }

         if (string.IsNullOrEmpty(loginTable))
         {
             loginTable = LoginTable.getAccountTableByPlatform(platform);
         }
         if (string.IsNullOrEmpty(acc))
         {
             retStr = (Helper.buildLuaReturn(-1, "acc is empty"));
             CLOG.Info("startLogin.doLogin, acc is empty");
             return retStr;
         }
         if (string.IsNullOrEmpty(deviceID))
         {
             deviceID = "";
         }

         Dictionary<string, object> dic = null;

         string pwd = Helper.getMD5Upper("111111" + CC.ASE_KEY_PWD);
         //判断是否存在帐号
         if (MongodbAccount.Instance.KeyExistsBykey(loginTable, "acc", acc))
         {
             string tmpAcc = "";
             //检测帐号是否能登陆
             int retCode = CLoginBase.tryLogin(CC.LOGIN_FIELD_NORMAL, acc, pwd, loginTable, ref tmpAcc);
             if (retCode == CC.SUCCESS)
             {
                 Random rd = new Random();
                 int randkey = rd.Next();
                 DateTime now = DateTime.Now;
                 var updata = LoginBase.getLoginUpData(Request, now, randkey, deviceID);
                 if (param.m_addDataFun != null)
                 {
                     param.m_addDataFun(updata);
                 }
                 string strerr = MongodbAccount.Instance.ExecuteUpdate(loginTable, "acc", acc, updata);

                 if (strerr != "")
                 {
                     dic = genDic(CC.ERR_DB, 0, 0, "");
                 }
                 else
                 {
                     dic = genDic(CC.SUCCESS, randkey, Convert.ToInt64(updata["lasttime"]), tmpAcc);
                 }
             }
             else
             {
                 dic = genDic(CC.ERR_DB, 0, 0, "");
             }
         }
         else
         {
             //注册新帐号
             Random rd = new Random();
             int randkey = rd.Next();
             Dictionary<string, object> updata = new Dictionary<string, object>();
             updata["acc"] = acc;
             updata["pwd"] = pwd;
             DateTime now = DateTime.Now;
             updata["randkey"] = randkey;
             updata["lasttime"] = now.Ticks;
             updata["regedittime"] = now;
             updata["regeditip"] = Request.ServerVariables.Get("Remote_Addr").ToString();
             updata["updatepwd"] = false;
             updata["platform"] = platform;
             updata["lastip"] = Convert.ToString(updata["regeditip"]);
             updata["deviceId"] = deviceID;
             string endacc = Guid.NewGuid().ToString().Replace("-", "");
             updata["acc_real"] = endacc;
             updata["lastLoginTime"] = DateTime.Now;
             updata["accInvite"] = param.m_accInvite;

             if (param.m_addDataFun != null)
             {
                 param.m_addDataFun(updata);
             }

             string strerr = MongodbAccount.Instance.ExecuteStoreBykey(loginTable, "acc", acc, updata);
             if (strerr != "")
             {
                 dic = genDic(CC.ERR_DB, 0, 0, "");
             }
             else
             {
                 dic = genDic(CC.SUCCESS, randkey, Convert.ToInt64(updata["lasttime"]), endacc);

                 Dictionary<string, object> savelog = new Dictionary<string, object>();
                 savelog["acc"] = acc;
                 savelog["acc_dev"] = deviceID;
                 savelog["acc_real"] = endacc;
                 savelog["ip"] = Request.ServerVariables.Get("Remote_Addr").ToString();
                 savelog["time"] = now;
                 savelog["channel"] = channelID;
                 MongodbAccount.Instance.ExecuteInsert("RegisterLog", savelog);
             }
         }

         return JsonHelper.genJson(dic);
         */
    }

    public Dictionary<string, object> startLogin2(LoginBaseParam param)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        string platform = param.m_platform;
        string acc = param.m_acc;
        string deviceID = param.m_deviceId;
        string channelID = param.m_channelId;
        channelID = LoginCommon.channelToString(channelID);
        string loginTable = param.m_loginTable;
        HttpRequest Request = param.Request;

       // string retStr = "";
        if (string.IsNullOrEmpty(platform))
        {
           // retStr = (Helper.buildLuaReturn(-1, "platform is empty"));
            CLOG.Info("startLogin.doLogin, platform is empty");
            dic.Add(CC.KEY_CODE, CC.ERR_PLATFORM);
            return dic;
        }

        if (string.IsNullOrEmpty(loginTable))
        {
            loginTable = LoginTable.getAccountTableByPlatform(platform);
        }
        if (string.IsNullOrEmpty(acc))
        {
          //  retStr = (Helper.buildLuaReturn(-1, "acc is empty"));
            CLOG.Info("startLogin.doLogin, acc is empty");
            dic.Add(CC.KEY_CODE, CC.ERR_ACC);
            return dic;
        }
        if (string.IsNullOrEmpty(deviceID))
        {
            deviceID = "";
        }

        var oriData = MongodbAccount.Instance.ExecuteGetBykey(loginTable, "acc", acc, S_FIELDS);
        string pwd = Helper.getMD5Upper("111111" + CC.ASE_KEY_PWD);
        //判断是否存在帐号
        // if (MongodbAccount.Instance.KeyExistsBykey(loginTable, "acc", acc))
        if (oriData != null)
        {
            string tmpAcc = "";
            //检测帐号是否能登陆
            int retCode = CLoginBase.tryLogin(CC.LOGIN_FIELD_NORMAL, acc, pwd, loginTable, ref tmpAcc);
            if (retCode == CC.SUCCESS)
            {
                Random rd = new Random();
                int randkey = rd.Next();
                DateTime now = DateTime.Now;
                var updata = LoginBase.getLoginUpData(Request, now, randkey, deviceID);
                if (param.m_addDataFun != null)
                {
                    param.m_addDataFun(updata);
                }

                string strerr = "";
                if (oriData.ContainsKey("acc_real"))
                {
                    string accReal = Convert.ToString(oriData["acc_real"]);
                    strerr = MongodbAccount.Instance.ExecuteUpdate(loginTable, "acc_real", accReal, updata, UpdateFlags.Multi);
                }
                else
                {
                    strerr = MongodbAccount.Instance.ExecuteUpdate(loginTable, "acc", acc, updata);
                }

                if (strerr != "")
                {
                    dic = genDic(CC.ERR_DB, 0, 0, "");
                }
                else
                {
                    dic = genDic(CC.SUCCESS, randkey, Convert.ToInt64(updata["lasttime"]), tmpAcc);
                }
            }
            else
            {
                dic = genDic(CC.ERR_DB, 0, 0, "");
            }
        }
        else
        {
            //注册新帐号
            Random rd = new Random();
            int randkey = rd.Next();
            Dictionary<string, object> updata = new Dictionary<string, object>();
            updata["acc"] = acc;
            updata["pwd"] = pwd;
            DateTime now = DateTime.Now;
            updata["randkey"] = randkey;
            updata["lasttime"] = now.Ticks;
            updata["regedittime"] = now;
            updata["regeditip"] = Request.ServerVariables.Get("Remote_Addr").ToString();
            updata["updatepwd"] = false;
            updata["platform"] = platform;
            updata["lastip"] = Convert.ToString(updata["regeditip"]);
            updata["deviceId"] = deviceID;
            string endacc = Guid.NewGuid().ToString().Replace("-", "");
            updata["acc_real"] = endacc;
            updata["lastLoginTime"] = DateTime.Now;
            updata["accInvite"] = param.m_accInvite;

            if (param.m_addDataFun != null)
            {
                param.m_addDataFun(updata);
            }

            string strerr = MongodbAccount.Instance.ExecuteStoreBykey(loginTable, "acc", acc, updata);
            if (strerr != "")
            {
                dic = genDic(CC.ERR_DB, 0, 0, "");
            }
            else
            {
                dic = genDic(CC.SUCCESS, randkey, Convert.ToInt64(updata["lasttime"]), endacc);

                Dictionary<string, object> savelog = new Dictionary<string, object>();
                savelog["acc"] = acc;
                savelog["acc_dev"] = deviceID;
                savelog["acc_real"] = endacc;
                savelog["ip"] = Request.ServerVariables.Get("Remote_Addr").ToString();
                savelog["time"] = now;
                savelog["channel"] = channelID;
                MongodbAccount.Instance.ExecuteInsert("RegisterLog", savelog);
            }
        }

        return dic;
    }

    public Dictionary<string, object> genDic(int code, int randKey, long ticks, string accReal)
    {
        Dictionary<string, object> dicResult = new Dictionary<string, object>();
        dicResult.Add(CC.KEY_CODE, code);
        if (code == RetCode.RET_SUCCESS)
        {
            string clientkey = randKey.ToString() + ":" + ticks.ToString();
            dicResult.Add(CC.KEY_TOKEN, AESHelper.AESEncrypt(clientkey, LoginCommon.AES_KEY));
            dicResult.Add(CC.KEY_ACC_REAL, accReal);
        }

        return dicResult;
    }
}


//////////////////////////////////////////////////////////////////////////
// quick sdk 登录，是个通用登录
public class QuickSdkLogin
{
    // 检测quick start 是否登录的url
    public const string CHECK_URL = "http://checkuser.sdk.quicksdk.net/v2/checkUserInfo?token={0}&product_code={1}&uid={2}";

    public string doLogin(object param)
    {
        HttpRequest Request = (HttpRequest)param;
        string retstr = "";
        do 
        {
            try
            {
                LoginBase obj = new LoginBase();

                string token = Request.QueryString["token"];
                if (string.IsNullOrEmpty(token))
                {
                    CLOG.Info("QuickSdkLogin.doLogin, token empty");
                    retstr = JsonHelper.genJson(obj.genDic(CC.ERR_PARAM_ERROR, 0, 0, ""));
                    break;
                }
                string productCode = Request.QueryString["product_code"];
                if (string.IsNullOrEmpty(productCode))
                {
                    CLOG.Info("QuickSdkLogin.doLogin, product_code empty");
                    retstr = JsonHelper.genJson(obj.genDic(CC.ERR_PARAM_ERROR, 0, 0, ""));
                    break;
                }
                string uid = Request.QueryString["uid"];
                if (string.IsNullOrEmpty(uid))
                {
                    CLOG.Info("QuickSdkLogin.doLogin, uid empty");
                    retstr = JsonHelper.genJson(obj.genDic(CC.ERR_PARAM_ERROR, 0, 0, ""));
                    break;
                }
                string channelId = Request.QueryString["channelId"];
                if (string.IsNullOrEmpty(channelId))
                {
                    CLOG.Info("QuickSdkLogin.doLogin, channelId empty");
                    retstr = JsonHelper.genJson(obj.genDic(CC.ERR_PARAM_ERROR, 0, 0, ""));
                    break;
                }
                string deviceId = Request.QueryString["deviceId"];

                string url = string.Format(CHECK_URL, token, productCode, uid);
                byte[] ret = HttpPost.Post(new Uri(url));
                string str = Encoding.UTF8.GetString(ret);
                if (str == "1")
                {
                    LoginBaseParam objParam = new LoginBaseParam();
                    objParam.m_acc = channelId + "_" + uid;
                    objParam.m_channelId = channelId;
                    objParam.m_deviceId = deviceId;
                    objParam.m_loginTable = null;
                    objParam.m_platform = LoginTable.PLATFORM_QUICKSDK;
                    objParam.Request = Request;
                    retstr = obj.startLogin(objParam);
                }
                else
                {
                    retstr = JsonHelper.genJson(obj.genDic(CC.ERR_ACC_CHECK, 0, 0, ""));
                    CLOG.Info("QuickSdkLogin, login failed, ret={0}", str);
                    break;
                }
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }

        } while (false);

        return retstr;
    }
}

//////////////////////////////////////////////////////////////////////////
public class WxMiniInfo
{
    public string m_openid;
    public string m_sessionKey;
    public string m_unionid;
}

// 服务器调用微信的接口，根据客户端传来的临时code，得到openid, sessionkey等
public class WxMinSdkLoginService
{
    public string AppId { set; get; }

    public string AppSecret { set; get; }

    public string Code { set; get; }

    // 邀请人账号
    public string InviteAcc { set; get; }

    public static string WX_URL = "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";

    // 微信小程序渠道号
    public const string CHANNEL = "100001";

    public string doLogin(object param)
    {
        Dictionary<string, object> retobj = new Dictionary<string, object>();

        do
        {
            try
            {
                HttpRequest Request = (HttpRequest)param;

                string url = string.Format(WX_URL, AppId, AppSecret, Code);
                byte[] ret = HttpPost.Get(new Uri(url));
                string retstr = Encoding.UTF8.GetString(ret);
                CLOG.Info(retstr);

                Dictionary<string, object> tmpRet = JsonHelper.ParseFromStr<Dictionary<string, object>>(retstr);
                if (tmpRet.ContainsKey("errcode") && Convert.ToInt32(tmpRet["errcode"]) != 0)
                {
                    retobj.Add(CC.KEY_CODE, Convert.ToInt32(tmpRet["errcode"]));
                    break;
                }

                WxMiniInfo info = new WxMiniInfo();
                info.m_openid = Convert.ToString(tmpRet["openid"]);
                info.m_sessionKey = Convert.ToString(tmpRet["session_key"]);
                if (tmpRet.ContainsKey("unionid"))
                {
                    info.m_unionid = Convert.ToString(tmpRet["unionid"]);
                }
                
                LoginBase loginObj = new LoginBase();
                LoginBaseParam loginParam = new LoginBaseParam();
                loginParam.m_acc = CHANNEL + "_" + info.m_openid;
                loginParam.m_channelId = CHANNEL;
                loginParam.m_deviceId = "";
                loginParam.m_loginTable = null;
                loginParam.m_platform = LoginTable.PLATFORM_WX_MINI;
                loginParam.Request = Request;
                loginParam.m_accInvite = string.IsNullOrEmpty(InviteAcc) ? "" : InviteAcc;
                string loginStr = loginObj.startLogin(loginParam);
                retobj = JsonHelper.ParseFromStr<Dictionary<string, object>>(loginStr);
                retobj.Add("openid", info.m_openid);
                retobj.Add("sessionKey", info.m_sessionKey);
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }
        } while (false);

        return JsonHelper.ConvertToStr(retobj);
    }
}


//////////////////////////////////////////////////////////////////////////
// QQ手游戏登录，根据客户端传来的临时code，得到openid, sessionkey等
public class QQMiniSdkLoginService
{
    public string AppId { set; get; }

    public string AppSecret { set; get; }

    public string Code { set; get; }

    // android 还是 ios
    public string PhoneSys { set; get; }

    public static string QQ_URL = "https://api.q.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";

    // QQ手游小程序渠道号
    public const string CHANNEL = "100002";

    public string doLogin(object param)
    {
        Dictionary<string, object> retobj = new Dictionary<string, object>();

        do
        {
            try
            {
                HttpRequest Request = (HttpRequest)param;

                string url = string.Format(QQ_URL, AppId, AppSecret, Code);
                byte[] ret = HttpPost.Get(new Uri(url));
                string retstr = Encoding.UTF8.GetString(ret);
                CLOG.Info(retstr);

                Dictionary<string, object> tmpRet = JsonHelper.ParseFromStr<Dictionary<string, object>>(retstr);
                if (tmpRet.ContainsKey("errcode") && Convert.ToInt32(tmpRet["errcode"]) != 0)
                {
                    retobj.Add(CC.KEY_CODE, Convert.ToInt32(tmpRet["errcode"]));
                    break;
                }

                WxMiniInfo info = new WxMiniInfo();
                info.m_openid = Convert.ToString(tmpRet["openid"]);
                info.m_sessionKey = Convert.ToString(tmpRet["session_key"]);
                if (tmpRet.ContainsKey("unionid"))
                {
                    info.m_unionid = Convert.ToString(tmpRet["unionid"]);
                }

                LoginBase loginObj = new LoginBase();
                LoginBaseParam loginParam = new LoginBaseParam();
                loginParam.m_acc = CHANNEL + "_" + PhoneSys + "_" + info.m_openid;
                loginParam.m_channelId = CHANNEL;
                loginParam.m_deviceId = "";
                loginParam.m_loginTable = null;
                loginParam.m_platform = LoginTable.PLATFORM_QQ_MINI;
                loginParam.Request = Request;
                string loginStr = loginObj.startLogin(loginParam);
                retobj = JsonHelper.ParseFromStr<Dictionary<string, object>>(loginStr);
                retobj.Add("openid", info.m_openid);
                retobj.Add("sessionKey", info.m_sessionKey);
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }
        } while (false);

        return JsonHelper.ConvertToStr(retobj);
    }
}

//////////////////////////////////////////////////////////////////////////
// 闲玩的注册，登录，全部转到本类。 闲玩采用自带的登录，注册，但需要传平台，渠道
public class XianWanLoginService
{
    // 操作 -- 登录
    public const int OP_LOGIN = 1;
    // 操作 -- 注册账号
    public const int OP_REGACC = 2;

    // 注册账号方式- 自动注册
    public const int REGACC_AUTO = 1;
    // 注册账号方式 - 账号，密码注册
    public const int REGACC_ACC_PWD = 2;

    // 快速登录(通过设备号登录)
    public const int REGACC_FAST = 3;

    public string AccTable { set; get; }

    public string Channel { set; get; }

    public int Op { set; get; }

   // const string ACC_TABLE = "xianwan_acc";

    public XianWanLoginService()
    {
        Op = -1;
    }

    public string doLogin2(FastLoginParam param, HttpRequest request)
    {
        CFastLogin obj = new CFastLogin();
        obj.LoginTableName = AccTable;
        obj.Channel = Channel;
        return obj.doLogin(param, request);
    }

    public string doLogin(object param)
    {
        do
        {
            try
            {
                HttpRequest request = (HttpRequest)param;
                int op = Convert.ToInt32(request.Form["op"]);
                if(isValidOp())
                {
                    op = Op;
                }
                if (op == OP_LOGIN) // 登录采用设备号登录
                {
                    CFastLogin obj = new CFastLogin();
                    obj.LoginTableName = AccTable;
                    obj.Channel = Channel;
                    return obj.doLogin(request);
                    /*else
                    {
                        CLoginBase L = new CLoginBase();
                        L.LoginTableName = ACC_TABLE;
                        L.doLogin(request);
                        return L.getRetStr();
                    }*/
                }
                else if (op == OP_REGACC)
                {
                    int regWay = Convert.ToInt32(request.Form["regWay"]); // 注册方式
                    if (regWay == REGACC_AUTO)
                    {
                        CAutoRegAcc obj = new CAutoRegAcc();
                        obj.CheckTableName = AccTable;
                        obj.regAcc(request);
                        return obj.getRetStr();
                    }
                    else if (regWay == REGACC_ACC_PWD)
                    {
                        CRegAcc obj = new CRegAcc();
                        obj.RegTableName = AccTable;
                        obj.regAcc(request);
                        return obj.getRetStr();
                    }
                }
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }
        } while (false);

        Dictionary<string, object> ret = new Dictionary<string, object>();
        ret.Add(CC.KEY_CODE, RetCode.RET_FAIL);
        return JsonHelper.genJson(ret);
    }

    bool isValidOp()
    {
        return Op != -1;
    }
}

//////////////////////////////////////////////////////////////////////////////
// qq 大厅登录
public class QQLobby
{
    public string OpenId { set; get; }

    public string OpenKey { set; get; }

    public string Pf { set; get; }

    public string Channel { set; get; }

    public bool IsTest { set; get; }

    public string doLogin(HttpRequest Request)
    {
        Dictionary<string, object> retobj = new Dictionary<string, object>();
        CLOG.Info("enter {0} {1} {2} ", OpenId, OpenKey, Pf);

        try
        {
            do
            {
                if (string.IsNullOrEmpty(OpenId) || string.IsNullOrEmpty(OpenKey) || string.IsNullOrEmpty(Pf))
                {
                    retobj.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
                    break;
                }

                OpenApiV3 sdk = new OpenApiV3(QQgameCFG.appid, QQgameCFG.appkey);
                if(IsTest)
                {
                    sdk.SetServerName(QQgameCFG.SERVER_NAME_TEST);
                }
                else
                {
                    sdk.SetServerName(QQgameCFG.SERVER_NAME);
                }

                Dictionary<string, object> result = GetUserInfo(sdk, OpenId, OpenKey, Pf);
                if (result == null || result.Count < 2)   // userInfo   userBlueVipInfo 都需要
                {
                    retobj.Add(CC.KEY_CODE, CC.ERR_CANNOT_GET_INFO);
                    break;
                }

                retobj = after(Request, result);

                //数据上报
                //post_data();

            } while (false);
        }
        catch (Exception ex)
        {
            CLOG.Info(ex.ToString());
        }

        return JsonHelper.ConvertToStr(retobj);
    }

    Dictionary<string, object> GetUserInfo(OpenApiV3 sdk, string openid, string openkey, string pf)
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("openid", openid);
        param.Add("openkey", openkey);
        param.Add("pf", pf);
       // param.Add("userip", "112.90.139.30");

        Dictionary<string, object> result = new Dictionary<string, object>();
        //玩家信息
        RstArray user_info = sdk.Api("/v3/user/get_info", param);
      //  CLOG.Info(sdk.URLString);

        if (user_info.Ret == 0)
        {
            Dictionary<string, object> tmpRet = JsonHelper.ParseFromStr<Dictionary<string, object>>(user_info.Msg);
            if (Convert.ToInt32(tmpRet["ret"]) != 0)
            {
                CLOG.Info("QQLobby error info {0}", user_info.Msg);
                return null;
            }
            result.Add("userInfo", tmpRet);
        }

        //蓝钻信息
        RstArray user_blue_vip_info = sdk.Api("/v3/user/blue_vip_info", param);
        if (user_blue_vip_info.Ret == 0)
        {
            Dictionary<string, object> tmpRet = JsonHelper.ParseFromStr<Dictionary<string, object>>(user_blue_vip_info.Msg);
            if (Convert.ToInt32(tmpRet["ret"]) != 0)
            {
                CLOG.Info("QQLobby error blue {0}", user_blue_vip_info.Msg);
                return null;
            }

            result.Add("userBlueVipInfo", tmpRet);
        }
        return result;
    }

    Dictionary<string, object> after(HttpRequest Request, Dictionary<string, object> result)
    {
        LoginBase loginObj = new LoginBase();
        LoginBaseParam loginParam = new LoginBaseParam();
        loginParam.m_acc = Channel + "_" + OpenId;
        loginParam.m_channelId = Channel;
        loginParam.m_deviceId = "";
        loginParam.m_loginTable = null;
        loginParam.m_platform = LoginTable.PLATFORM_QQ_GAME;
        loginParam.Request = Request;
        loginParam.m_addDataFun = (upData) => {

            Dictionary<string, object> blueInfo = (Dictionary<string,object>)result["userBlueVipInfo"];

            //蓝钻等级
            if (blueInfo.ContainsKey("blue_vip_level")){
                upData.Add("BlueDiamondLv", Convert.ToInt32(blueInfo["blue_vip_level"]));
            }else {
                upData.Add("BlueDiamondLv", 0);
            }

            //蓝钻到期时间没有则为0
            if (blueInfo.ContainsKey("vip_valid_time")){
                upData.Add("BlueDiamondLvEndTime", Convert.ToInt32(blueInfo["vip_valid_time"]));
            }else{
                upData.Add("BlueDiamondLvEndTime", 0);
            }

            //蓝钻年费到期时间没有则为0
            if (blueInfo.ContainsKey("year_vip_valid_time")) {
                upData.Add("BlueYearEndTime", Convert.ToInt32(blueInfo["year_vip_valid_time"]));
            }else{
                upData.Add("BlueYearEndTime", 0);
            }

            //豪华蓝钻到期时间没有则为0
            if (blueInfo.ContainsKey("super_vip_valid_time")){
                upData.Add("BlueLuxEndTime", Convert.ToInt32(blueInfo["super_vip_valid_time"]));
            } else{
                upData.Add("BlueLuxEndTime", 0);
            }
        };

        Dictionary<string, object> retobj = loginObj.startLogin2(loginParam);

        Dictionary<string, object> userInfo = (Dictionary<string, object>)result["userInfo"];
        retobj.Add("nickname", Convert.ToString(userInfo["nickname"]));
        retobj.Add("figureurl", Convert.ToString(userInfo["figureurl"]));
        return retobj;
    }

    //void post_compress() {string pf}
}

////////////////////////////////////////////////////////////////////
// 多游登录
public class CDuoYouLogin
{
    public string AppId { set; get; }
    public string ChannelId { set; get; }
   // public string Platform { set; get; } = "duoyou";

    const string CHECK_URL = "https://api.aiduoyou.com/member/login_check?app_id={0}&uid={1}&access_token={2}";

    public void doLogin(HttpContext context)
    {
        LoginBase obj = new LoginBase();
        string token = context.Request.Form["access_token"];
        if (string.IsNullOrEmpty(token))
        {
            CLOG.Info("DuoyouLogin checklogin token empty");
            string str = JsonHelper.genJson(obj.genDic(CC.ERR_PLAT_TOKEN, 0, 0, ""));
            context.Response.ContentType = "text/plain";
            context.Response.Write(str);
            return;
        }

        if (obj.chekOrReply(context.Request, context.Response, LoginBase.CHK_PARAM_ACCOUNT))
        {
            string str = "";
            if (checkLogin(obj, context.Request.Form["acc"], token, ref str))
            {
                LoginBaseParam param = new LoginBaseParam();
                param.m_acc = ChannelId + "_" + context.Request.Form["acc"];
                //param.m_platform = Platform;
                param.m_channelId = ChannelId;
                param.m_deviceId = context.Request.Form["deviceID"];
                param.Request = context.Request;
                str = obj.startLogin(param);
               // CLOG.Info("DuoyouLogin, {0} success", param.m_acc);
            }
            else
            {
                CLOG.Info("DuoyouLogin checklogin error, {0}", token);
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(str);
        }
    }

    bool checkLogin(LoginBase obj, string uid, string token, ref string outstr)
    {
        try
        {
            string url = string.Format(CHECK_URL, AppId, uid, token);
            //CLOG.Info("duoyou: " + url);

            // byte[] ret = WxPayAPI.HttpService.Get(url);
            string retstr = WxPayAPI.HttpService.Get(url, (req) =>
            {
                // req.KeepAlive = false;
                // req.ProtocolVersion = HttpVersion.Version10;
            },

            (hd) =>
            {
                hd.Add("SecurityProtocol", System.Net.SecurityProtocolType.Tls11);
            });
            // string retstr = Encoding.UTF8.GetString(ret);

            Dictionary<string, object> tmpRet = JsonHelper.ParseFromStr<Dictionary<string, object>>(retstr);
            if (tmpRet.ContainsKey("code") && Convert.ToInt32(tmpRet["code"]) == 0)
            {
                return true;
            }
            else
            {
                CLOG.Info("DuoyouLogin, checkLogin code = {0}", Convert.ToInt32(tmpRet["code"]));
            }
        }
        catch (System.Exception ex)
        {
            CLOG.Info(ex.ToString());
            // return true;
        }

        CLOG.Info("DuoyouLogin failed");

        outstr = JsonHelper.genJson(obj.genDic(CC.ERR_CHECK_LOGIN, 0, 0, ""));
        return false;
    }
}





