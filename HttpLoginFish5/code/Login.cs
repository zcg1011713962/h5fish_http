using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

public struct CC
{
    public const int SUCCESS = 0;   // 成功

    public const int ERR_PLATFORM = 1;   // 平台错

    public const int ERR_ACC = 2;   // 账号错， 不符合格式

    public const int ERR_PWD = 3;  // 密码错

    public const int ERR_DEVICE = 4;  // 设备错

    public const int ERR_DATA = 5;  // 数据错

    public const int ERR_SIGN = 6;  // sign错

    public const int ERR_JSON = 7;  // json错

    public const int ERR_ACC_EXIST = 8;  // 账号存在

    public const int ERR_DB = 9;  // db错误

    public const int ERR_NOT_MODIFY_PWD = 10;

    public const int ERR_ACC_BLOCK = 11;

    public const int ERR_ACC_FROZEN = 12;

    public const int ERR_ACC_NOT_EXIST = 13;  // 账号不存在

    public const int ERR_NOT_BINDPHONE = 14;   // 未绑定手机

    public const int ERR_PHONE = 15;   // 手机号错

    public const int ERR_CODE = 16;   // 验证码错误

    public const int ERR_UNKNOWN = 17;   // 未知
    
    public const int ERR_CODE_TIME_CD = 18;   // 获取验证码频繁
    public const int ERR_CODE_RUN_OUT_COUNT = 19;   // 获取验证码次用完
    public const int ERR_CODE_SEND_FAILED = 21;   // 验证码发送失败

    public const int ERR_PARAM_ERROR = 20;   // 参数错
    public const int ERR_ACC_CHECK = 22;   // 账号验证错
    public const int ERR_BALANCE_NOT_ENOUGH = 23;   // 余额不足
    public const int ERR_PAY_FAILED = 24;

    // 平台验证登录
    public const int ERR_CHECK_LOGIN = 25;
    public const int ERR_PLAT_TOKEN = 26;

    // 获取信息失败
    public const int ERR_CANNOT_GET_INFO = 27;

    public const int ERR_WX_CODE = 40029;   // code 无效
    public const int ERR_WX_OFTEN = 45011;   // 频率限制，每个用户每分钟100次

    //-----------------------------------------------
    public const string KEY_CODE = "code";

    public const string KEY_TOKEN = "token";

    public const string KEY_ACC_REAL = "accReal";

    public const string EXP_ACC = @"^[a-zA-Z][0-9a-zA-Z]{5,29}$";

    // 用来密码md5额外串 pwd = md5(原始密码+ASE_KEY_PWD)
    public const string ASE_KEY_PWD = "&@*(#kas9581gajk";

    public const string EXP_FULL_DIGIT = @"^[0-9]{6,30}$";

    public const string FAST_AUTO_ACC_PREFIX = "play";

    // 普通账号登录( 采用默认登录，而不是平台登录 )
    public const string LOGIN_WAY_NORMAL = "1";

    // 手机号登录
    public const string LOGIN_WAY_MOBILE_PHONE = "2";

    public const string LOGIN_FIELD_NORMAL = "acc";
    public const string LOGIN_FIELD_MOBILE = "mobilePhone"; 
}

public class CLoginBase
{
    private string Account { set; get; }

    private string Pwd { set; get; }

    private string LoginWay { set; get; }

    private Dictionary<string, object> m_dic = new Dictionary<string, object>();

    private string m_accField;

    // 登录表名
    public string LoginTableName { set; get; }

    public CLoginBase()
    {
        LoginTableName = LoginTable.ACC_DEFAULT;
        m_dic.Add(CC.KEY_CODE, RetCode.RET_FAIL);
    }

    public void doLogin(HttpRequest Request)
    {
        string deviceId = "";
        try
        {
            Account = Request.Form["acc"];
            Pwd = Request.Form["pwd"];
            LoginWay = Request.Form["loginWay"];
            deviceId = Request.Form["deviceId"];
        }
        catch (System.Exception ex)
        {
            return;
        }

        if (string.IsNullOrEmpty(Account) || string.IsNullOrEmpty(Pwd))
            return;

        m_accField = getLoginFieldName(LoginWay);
        if (isMobileLogin(LoginWay)) // 手机号登录，查总表
        {
            LoginTableName = LoginTable.ACC_DEFAULT;
        }
        string accReal = "";
        int retCode = tryLogin(m_accField, Account, Pwd, LoginTableName, ref accReal);
        
        m_dic[CC.KEY_CODE] = retCode;
        if (retCode == RetCode.RET_SUCCESS)
        {
            Random rd = new Random();
            int randkey = rd.Next();
            DateTime now = DateTime.Now;
            var updata = LoginBase.getLoginUpData(Request, now, randkey, deviceId);
            string strerr = MongodbAccount.Instance.ExecuteUpdate(LoginTableName, m_accField, Account, updata);
            string clientkey = randkey.ToString() + ":" + Convert.ToString(updata["lasttime"]);
            m_dic.Add(CC.KEY_TOKEN, AESHelper.AESEncrypt(clientkey, LoginCommon.AES_KEY));
            m_dic.Add(CC.KEY_ACC_REAL, accReal);
        }
    }

    public string getRetStr()
    {
        return JsonHelper.genJson(m_dic);
    }

    public static string genToken(Dictionary<string, object> data)
    {
        string randkey = Convert.ToString(data["randkey"]);
        string lasttime = Convert.ToString(data["lasttime"]);
        string clientkey = randkey + ":" + lasttime;
        return AESHelper.AESEncrypt(clientkey, LoginCommon.AES_KEY);
    }

    public static int tryLogin(string accFieldName, string acc_reg, string pwd, string table, ref string acc)
    {
        int retCode = CC.ERR_DATA;
        Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetBykey(table, accFieldName, acc_reg, CONST.LOGIN_FAILED_FIELD);
        if (data == null)
            return CC.ERR_ACC_NOT_EXIST;

        bool checkpwd = Convert.ToBoolean(ConfigurationManager.AppSettings["check_pwd"]);
        if (checkpwd)
        {
            if (data.ContainsKey("updatepwd") && !Convert.ToBoolean(data["updatepwd"]))
                return CC.ERR_NOT_MODIFY_PWD;
        }

        if (data.ContainsKey("block"))
        {
            bool isBlock = Convert.ToBoolean(data["block"]);
            if (isBlock)
                return CC.ERR_ACC_BLOCK;
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
                            return CC.ERR_ACC_FROZEN;
                    }
                }
            }
        }

        string dbPwd = Convert.ToString(data["pwd"]);
        if (pwd == dbPwd)
        {
            retCode = CC.SUCCESS;
            curFailedCnt = 0;
            if (data.ContainsKey("acc_real"))
                acc = data["acc_real"].ToString();
        }
        else
        {
            curFailedCnt++;
            retCode = CC.ERR_PWD;
        }

        if (CONST.USE_LOGIN_FAILED_COUNT_CHECK == 1)
        {
            Dictionary<string, object> updata = new Dictionary<string, object>();
            updata.Add("loginFailedDate", DateTime.Now.Date);
            updata.Add("loginFailedCount", curFailedCnt);

            string strerr = MongodbAccount.Instance.ExecuteUpdate(table, "acc", acc_reg, updata);
            if (strerr != "")
            {
                retCode = CC.ERR_DB;
            }
        }

        return retCode;
    }

    public static string getLoginFieldName(string loginWay)
    {
        if (string.IsNullOrEmpty(loginWay))
        {
            return CC.LOGIN_FIELD_NORMAL;
        }
        if (loginWay == CC.LOGIN_WAY_NORMAL)
        {
            return CC.LOGIN_FIELD_NORMAL;
        }

        return CC.LOGIN_FIELD_MOBILE;
    }

    public static bool isMobileLogin(string loginWay)
    {
        if (string.IsNullOrEmpty(loginWay))
        {
            return false;
        }

        return loginWay == CC.LOGIN_WAY_MOBILE_PHONE;
    }
}

// 账号注册
public class CRegAcc
{
    private Dictionary<string, object> m_dic = new Dictionary<string, object>();
    public const string _AES_KEY_ = "&@*(#kas9581fatk";

    public string RegTableName { set; get; }

    public CRegAcc()
    {
        RegTableName = LoginTable.ACC_DEFAULT;
    }

    public void regAcc(HttpRequest Request)
    {
        // 密码=md5(原始密码+AES_KEY)
        // d1 = base64串
        string strdata = Request.Form["d1"];
        if (string.IsNullOrEmpty(strdata))
        {
            m_dic.Add(CC.KEY_CODE, CC.ERR_DATA);
            return;
        }

        string sign = Request.Form["d2"];
        if (string.IsNullOrEmpty(sign))
        {
            m_dic.Add(CC.KEY_CODE, CC.ERR_DATA);
            return;
        }

        strdata = Encoding.Default.GetString(Convert.FromBase64String(strdata));

        if (sign != AESHelper.MD5Encrypt(strdata + _AES_KEY_))
        {
            m_dic.Add(CC.KEY_CODE, CC.ERR_SIGN);
            return;
        }

        Dictionary<string, object> data = JsonHelper.ParseFromStr<Dictionary<string, object>>(strdata);
        if (data == null || data.Count < 2)
        {
            m_dic.Add(CC.KEY_CODE, CC.ERR_JSON);
            return;
        }

        bool res = checkAccount(data);
        if (res)
        {
            string acc = Convert.ToString(data["n1"]);
            Dictionary<string, object> accData = genAcc(acc, Convert.ToString(data["n2"]), Request);
            string strerr = MongodbAccount.Instance.ExecuteStoreBykey(RegTableName, "acc", acc, accData);
            if (strerr != "")
            {
                m_dic.Add(CC.KEY_CODE, CC.ERR_DB);
                return;
            }

            m_dic.Add(CC.KEY_CODE, CC.SUCCESS);
            string token = CLoginBase.genToken(accData);
            m_dic.Add(CC.KEY_TOKEN, token);
            m_dic.Add(CC.KEY_ACC_REAL, Convert.ToString(accData["acc_real"]));

            string channel = "000000";
            if (data.ContainsKey("n3"))
            {
                channel = data["n3"].ToString();
            }
            saveRegLog(acc, accData, channel, Request);
        }
    }

    public string getRetStr()
    {
        return JsonHelper.genJson(m_dic);
    }

    bool checkAccount(Dictionary<string, object> data)
    {
        string acc = data["n1"].ToString();

        if (!Regex.IsMatch(acc, CC.EXP_ACC))
        {
            m_dic.Add(CC.KEY_CODE, CC.ERR_ACC);
            return false;
        }

        if (Regex.IsMatch(acc, CC.EXP_FULL_DIGIT))
        {
            m_dic.Add(CC.KEY_CODE, CC.ERR_ACC);
            return false;
        }

        string sub = acc.Substring(0, CC.FAST_AUTO_ACC_PREFIX.Length);
        if (sub == CC.FAST_AUTO_ACC_PREFIX)
        {
            m_dic.Add(CC.KEY_CODE, CC.ERR_ACC);
            return false;
        }

        if (MongodbAccount.Instance.KeyExistsBykey(RegTableName, "acc", acc))
        {
            m_dic.Add(CC.KEY_CODE, CC.ERR_ACC_EXIST);
            return false;
        }

        string pwd = data["n2"].ToString();
        if (pwd.Length != 32)
        {
            m_dic.Add(CC.KEY_CODE, CC.ERR_PWD);
            return false;
        }

        return true;
    }

    public static Dictionary<string, object> genAcc(string acc, string pwd, HttpRequest Request)
    {
        Random rd = new Random();
        int randkey = rd.Next();
        Dictionary<string, object> updata = new Dictionary<string, object>();
        updata["acc"] = acc;
        updata["acc_real"] = Guid.NewGuid().ToString().Replace("-", "");
        updata["pwd"] = pwd;
        DateTime now = DateTime.Now;
        updata["randkey"] = randkey;
        updata["lasttime"] = now.Ticks;
        updata["regedittime"] = now;
        updata["regeditip"] = Request.ServerVariables.Get("Remote_Addr").ToString();
        updata["updatepwd"] = false;
        updata["lastip"] = Convert.ToString(updata["regeditip"]);
        string deviceID = Request.Params["deviceID"];
        updata["deviceId"] = (deviceID == null) ? "" : deviceID;
        updata["lastLoginTime"] = now;
        return updata;
    }

    public static void saveRegLog(string acc, Dictionary<string, object> accData, string channel, HttpRequest Request)
    {
        Dictionary<string, object> savelog = new Dictionary<string, object>();
        savelog["acc_real"] = accData["acc_real"].ToString();
        savelog["acc"] = acc;
        savelog["ip"] = Request.ServerVariables.Get("Remote_Addr").ToString();
        savelog["time"] = DateTime.Now;
        savelog["channel"] = channel;
        MongodbAccount.Instance.ExecuteInsert("RegisterLog", savelog);
    }
}

// 自动注册账号
public class CAutoRegAcc
{
    private Dictionary<string, object> m_dic = new Dictionary<string, object>();
    public const string _AES_KEY_ = "&@*(#kas9581fatk";

    public string CheckTableName { set; get; }

    public string AutoTableName { set; get; }

    public CAutoRegAcc()
    {
        CheckTableName = LoginTable.ACC_DEFAULT;
        AutoTableName = LoginTable.ACC_DEFAULT;
    }

    public void regAcc(HttpRequest Request)
    {
       // string table = LoginTable.ACC_DEFAULT;

        string acc_reg = HttpLogin.BuildAccount.getAutoAccount(AutoTableName, CheckTableName);
        if (string.IsNullOrEmpty(acc_reg))
        {
            m_dic.Add(CC.KEY_CODE, CC.ERR_DB);
            return;
        }
        string encrypt = Request.Params["encrypt"];
        bool pwd_encrypt = false;
        if (!string.IsNullOrEmpty(encrypt) && encrypt == "true")
        {
            pwd_encrypt = true;
        }
        string pwd = null;
        string out_pwd = null;
        string save_pwd = null;
        if (pwd_encrypt)
        {
            pwd = HttpLogin.BuildAccount.getAutoPassword(6);
            out_pwd = AESHelper.AESEncrypt(pwd, _AES_KEY_);
            save_pwd = AESHelper.MD5Encrypt(pwd);
        }
        else
        {
            out_pwd = HttpLogin.BuildAccount.getAutoPassword(20);
            pwd = string.Format("{0}{1}{2}{3}{4}{5}", out_pwd[8], out_pwd[16], out_pwd[4], out_pwd[11], out_pwd[2], out_pwd[9]);//password
            save_pwd = AESHelper.MD5Encrypt(pwd + CC.ASE_KEY_PWD);
        }
        Random rd = new Random();
        int randkey = rd.Next();
        Dictionary<string, object> updata = new Dictionary<string, object>();
        updata["acc"] = acc_reg;
        string endacc = Guid.NewGuid().ToString().Replace("-", "");
        updata["acc_real"] = endacc;
        updata["pwd"] = save_pwd;
        DateTime now = DateTime.Now;
        updata["randkey"] = randkey;
        updata["lasttime"] = now.Ticks;
        updata["regedittime"] = now;
        updata["regeditip"] = Request.ServerVariables.Get("Remote_Addr").ToString();
        updata["updatepwd"] = false;
        //updata["platform"] = Platform;
        updata["lastip"] = Convert.ToString(updata["regeditip"]);
        string deviceID = Request.Params["deviceID"];
        updata["deviceId"] = (deviceID == null) ? "" : deviceID;

        string strerr = MongodbAccount.Instance.ExecuteStoreBykey(CheckTableName, "acc", acc_reg, updata);
        if (strerr != "")
        {
            m_dic.Add(CC.KEY_CODE, CC.ERR_DB);
            return;
        }
        else
        {
            string channelID = Request.Params["channelID"];
            channelID = LoginCommon.channelToString(channelID);

            Dictionary<string, object> savelog = new Dictionary<string, object>();
            savelog["acc"] = acc_reg;
            savelog["acc_real"] = endacc;

            if (!string.IsNullOrEmpty(deviceID))
            {
                savelog["acc_dev"] = deviceID;
                LoginCheck.incRegAcc(deviceID);
            }
            savelog["ip"] = Request.ServerVariables.Get("Remote_Addr").ToString();
            savelog["time"] = now;
            savelog["channel"] = channelID;
            MongodbAccount.Instance.ExecuteInsert("RegisterLog", savelog);

           // string ret = string.Format("local ret = {{code = 0, acc=\"{0}\", pwd=\"{1}\", acc_real=\"{2}\"}}; return ret;", acc_reg, out_pwd, endacc);
            // Response.Write(ret);

            m_dic.Add(CC.KEY_CODE, CC.SUCCESS);
            m_dic.Add("acc", acc_reg);
            m_dic.Add("pwd", pwd);
            m_dic.Add(CC.KEY_ACC_REAL, endacc);
        }
    }

    public string getRetStr()
    {
        return JsonHelper.genJson(m_dic);
    }
}

public class FastLoginParam
{
    public string m_deviceId;
  //  public string m_remoteIp;
}

// 快速登录, 以设备号作为账号登录
public class CFastLogin
{
    public string LoginTableName { set; get; }
    public string Channel { set; get; }

    public string doLogin(FastLoginParam param, HttpRequest request)
    {
        Dictionary<string, object> ret = new Dictionary<string, object>();

        do
        {
            try
            {
                string deviceId = param.m_deviceId; // request.Form["deviceId"];
                if (string.IsNullOrEmpty(deviceId))
                {
                    CLOG.Info("CFastLogin, device emtpy");
                    ret.Add(CC.KEY_CODE, CC.ERR_DEVICE);
                    break;
                }

                string acc = "play_" + deviceId;
                Dictionary<string, object> accData = MongodbAccount.Instance.ExecuteGetBykey(LoginTableName, "acc", acc);
                string token = "";
                if (accData == null)
                {
                    accData = CRegAcc.genAcc(acc, "123456", request);
                    if (accData != null)
                    {
                        string strerr = MongodbAccount.Instance.ExecuteStoreBykey(LoginTableName, "acc", acc, accData);
                        if (strerr != "")
                        {
                            ret.Add(CC.KEY_CODE, CC.ERR_DB);
                            break;
                        }
                        CRegAcc.saveRegLog(acc, accData, Channel, request);
                        token = CLoginBase.genToken(accData);
                    }
                    else
                    {
                        ret.Add(CC.KEY_CODE, CC.ERR_PWD);
                        break;
                    }
                }
                else
                {
                    string accReal = "";
                    if (accData.ContainsKey("acc_real"))
                    {
                        accReal = Convert.ToString(accData["acc_real"]);
                    }
                    token = login(request, acc, accReal, deviceId);
                }

                ret.Add(CC.KEY_CODE, CC.SUCCESS);
                ret.Add(CC.KEY_TOKEN, token);
                ret.Add(CC.KEY_ACC_REAL, Convert.ToString(accData["acc_real"]));
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }
        } while (false);

        return JsonHelper.genJson(ret);
    }

    public string doLogin(HttpRequest request)
    {
        FastLoginParam param = new FastLoginParam();
        param.m_deviceId = request.Form["deviceId"];
        return doLogin(param, request);

        /*
        Dictionary<string, object> ret = new Dictionary<string, object>();

        do
        {
            try
            {
                string deviceId = request.Form["deviceId"];
                if (string.IsNullOrEmpty(deviceId))
                {
                    CLOG.Info("CFastLogin, device emtpy");
                    ret.Add(CC.KEY_CODE, CC.ERR_DEVICE);
                    break;
                }

                string acc = "play_" + deviceId;
                Dictionary<string, object> accData = MongodbAccount.Instance.ExecuteGetBykey(LoginTableName, "acc", acc);
                string token = "";
                if (accData == null)
                {
                    accData = CRegAcc.genAcc(acc, "123456", request);
                    if (accData != null)
                    {
                        string strerr = MongodbAccount.Instance.ExecuteStoreBykey(LoginTableName, "acc", acc, accData);
                        if (strerr != "")
                        {
                            ret.Add(CC.KEY_CODE, CC.ERR_DB);
                            break;
                        }
                        CRegAcc.saveRegLog(acc, accData, Channel, request);
                        token = CLoginBase.genToken(accData);
                    }
                    else
                    {
                        ret.Add(CC.KEY_CODE, CC.ERR_PWD);
                        break;
                    }
                }
                else
                {
                    string accReal = "";
                    if (accData.ContainsKey("acc_real"))
                    {
                        accReal = Convert.ToString(accData["acc_real"]);
                    }
                    token = login(request, acc, accReal, deviceId);
                }

                ret.Add(CC.KEY_CODE, CC.SUCCESS);
                ret.Add(CC.KEY_TOKEN, token);
                ret.Add(CC.KEY_ACC_REAL, Convert.ToString(accData["acc_real"]));
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }
        } while (false);

        return JsonHelper.genJson(ret);
        */
    }

    // 返回login key
    string login(HttpRequest request, string acc, string accReal, string deviceId)
    {
        Random rd = new Random();
        int randkey = rd.Next();
        DateTime now = DateTime.Now;
        var updata = LoginBase.getLoginUpData(request, now, randkey, deviceId);

        if (string.IsNullOrEmpty(accReal))
        {
            string strerr = MongodbAccount.Instance.ExecuteUpdate(LoginTableName, "acc", acc, updata);
        }
        else
        {
            string strerr = MongodbAccount.Instance.ExecuteUpdate(LoginTableName, "acc_real", accReal, updata, UpdateFlags.Multi);
        }
        
        string clientkey = randkey.ToString() + ":" + Convert.ToString(updata["lasttime"]);
        return AESHelper.AESEncrypt(clientkey, LoginCommon.AES_KEY);
    }
}





























