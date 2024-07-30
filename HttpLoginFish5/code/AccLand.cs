using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using Common;

public class AccLandContext
{
    public HttpRequest Request { set; get; }

    // 第三方平台登录表
    public string PlatLoginTable { set; get; }

    public string ChannelId { set; get; }

    public string UserId { set; get; }

    public string UserSdk { set; get; }

    public string DeviceId { set; get; }

    public string Platform { set; get; }

    // 输出串
    public string m_retstr = "";

    // 输出，前端的登录名
    public string m_logicAcc = "";

    // 输出，后台的登录账号
    public string m_logicAccReal = "";

    public int m_randKey;

    public string m_ticks = "";
}

// 通过平台登录的账号落地
public class AccLand
{
    // 在是新账号的情况下才落地，对于老账号不动
    public virtual int startLand(AccLandContext context)
    {
        throw new Exception("not implement");
    }

    public virtual string getSuccessRetStr(AccLandContext context)
    {
        throw new Exception("not implement");
    }

    public virtual string getFailedRetStr()
    {
        string retStr = (HttpLogin.BuildAccount.buildLuaReturn(-11, "server error"));
        return retStr;
    }

    public string getAccByChannel(AccLandContext context)
    {
        return context.ChannelId + "_" + context.UserId;
    }

    public string getAccByUserSdk(AccLandContext context)
    {
        return context.UserSdk.ToLower() + "_" + context.UserId;
    }

    // 第一步，总表处理。 返回true已处理，渠道表不需要再处理， false，渠道表需要进一步处理
    public bool firstTotalAccProcess(AccLandContext context, ref string retStr)
    {
        bool res = false;
        string acc = getAccByUserSdk(context);
        res = MongodbAccount.Instance.KeyExistsBykey(PayTable.ACC_DEFAULT, "acc_real", acc);
        if (!res)
        {
            acc = getAccByChannel(context);
            res = MongodbAccount.Instance.KeyExistsBykey(PayTable.ACC_DEFAULT, "acc_real", acc);
        }
        if (res)
        {
            context.m_logicAcc = acc;
            context.m_logicAccReal = acc;

            Random rd = new Random();
            int randkey = rd.Next();
            DateTime now = DateTime.Now;
            var updata = LoginBase.getLoginUpData(context.Request, now, randkey, context.DeviceId);
            string strerr = MongodbAccount.Instance.ExecuteUpdate(PayTable.ACC_DEFAULT, "acc_real", acc, updata);
            if (strerr != "")
            {
                retStr = getFailedRetStr();
            }
            else
            {
                context.m_randKey = randkey;
                context.m_ticks = Convert.ToString(updata["lasttime"]);
                retStr = getSuccessRetStr(context);
                createLoginLog(context);
            }
        }

        return res;
    }

    // 第二步，渠道表处理。 返回true已处理
    public bool secondChannelAccProcess(AccLandContext context, ref string retStr)
    {
        bool res = false;
        string acc = getAccByChannel(context);

        if (MongodbAccount.Instance.KeyExistsBykey(context.PlatLoginTable, "acc", acc))
        {
            context.m_logicAcc = acc;
            context.m_logicAccReal = acc;

            Random rd = new Random();
            int randkey = rd.Next();
            DateTime now = DateTime.Now;
            var updata = LoginBase.getLoginUpData(context.Request, now, randkey, context.DeviceId);
            string strerr = MongodbAccount.Instance.ExecuteUpdate(context.PlatLoginTable, "acc", acc, updata);
            if (strerr != "")
            {
                retStr = getFailedRetStr();
            }
            else
            {
                context.m_randKey = randkey;
                context.m_ticks = Convert.ToString(updata["lasttime"]);
                retStr = getSuccessRetStr(context);
                createLoginLog(context);
            }
            res = true;
        }

        return res;
    }

    // 总表里面创建一个账号
    public bool createAccInTotal(AccLandContext context, ref string retStr)
    {
        bool res = false;
        try
        {
            Random rd = new Random();
            int randkey = rd.Next();
            Dictionary<string, object> updata = new Dictionary<string, object>();
            updata["acc"] = getAccByUserSdk(context);
            updata["acc_real"] = Convert.ToString(updata["acc"]);
            updata["pwd"] = Helper.getMD5Upper("123456");
            DateTime now = DateTime.Now;
            updata["randkey"] = randkey;
            updata["lasttime"] = now.Ticks;
            updata["regedittime"] = now;
            updata["regeditip"] = context.Request.ServerVariables.Get("Remote_Addr").ToString();
            updata["lastip"] = context.Request.ServerVariables.Get("Remote_Addr").ToString();
            updata["updatepwd"] = false;
            updata["platform"] = context.Platform;
            updata["channel"] = LoginCommon.channelToString(context.ChannelId);
            updata["deviceId"] = context.DeviceId;

            res = MongodbAccount.Instance.ExecuteInsert(PayTable.ACC_DEFAULT, updata);
            if (res)
            {
                context.m_logicAcc = Convert.ToString(updata["acc"]);
                context.m_logicAccReal = Convert.ToString(updata["acc_real"]);
                context.m_randKey = randkey;
                context.m_ticks = Convert.ToString(updata["lasttime"]);

                retStr = getSuccessRetStr(context);
                Dictionary<string, object> retdata = createRegisterLog(context);
                createLoginLog(retdata);
            }
            else
            {
                retStr = getFailedRetStr();
            }
        }
        catch (System.Exception ex)
        {
        }

        return res;
    }

    void createLoginLog(AccLandContext context)
    {
        try
        {
            Dictionary<string, object> savelog = new Dictionary<string, object>();
            savelog["acc"] = context.m_logicAcc;
            savelog["acc_dev"] = context.DeviceId;
            savelog["acc_real"] = context.m_logicAccReal;
            savelog["ip"] = context.Request.ServerVariables.Get("Remote_Addr").ToString();
            savelog["time"] = DateTime.Now;
            savelog["channel"] = LoginCommon.channelToString(context.ChannelId);
            MongodbAccount.Instance.ExecuteInsert("LoginLog", savelog);
        }
        catch (System.Exception ex)
        {
        }
    }

    void createLoginLog(Dictionary<string, object> savelog)
    {
        if (savelog != null)
        {
            MongodbAccount.Instance.ExecuteInsert("LoginLog", savelog);
        }
    }

    Dictionary<string, object> createRegisterLog(AccLandContext context)
    {
        try
        {
            Dictionary<string, object> savelog = new Dictionary<string, object>();
            savelog["acc"] = context.m_logicAcc;
            savelog["acc_dev"] = context.DeviceId;
            savelog["acc_real"] = context.m_logicAccReal;
            savelog["ip"] = context.Request.ServerVariables.Get("Remote_Addr").ToString();
            savelog["time"] = DateTime.Now;
            savelog["channel"] = LoginCommon.channelToString(context.ChannelId);
            MongodbAccount.Instance.ExecuteInsert("RegisterLog", savelog);
            return savelog;
        }
        catch (System.Exception ex)
        {
        }
        return null;
    }

    public string constructRetStr(string logicAcc, string exstr)
    {
        int len = logicAcc.Length;
        string ret = len + "z" + logicAcc + exstr;
        return ret;
    }
}

public class AccLandAnysdk : AccLand
{
    public const string URL_LOGIN = "http://oauth.anysdk.com/api/User/LoginOauth/";

    public override int startLand(AccLandContext context)
    {
        context.m_retstr = postLogin(context);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Dictionary<string, object> rets = serializer.Deserialize<Dictionary<string, object>>(context.m_retstr);

        // test-----------------
       // Dictionary<string, object> rets = new Dictionary<string, object>();
       // testData(rets, context);
        // test-----------------
        if (rets["status"].ToString() != "ok")
        {
            return 0;
        }

        buildContext(context, rets);

        bool payLoginRes = isPayLogin(context, rets);
        if (payLoginRes)
            return 0;

        string resstr = "";
        bool res = false;
        res = firstTotalAccProcess(context, ref resstr);
        if (res)
        {
            //CLOG.Info("land anysdk, 总表中存在, 最终acc:{0}, real_acc:{1} plat:{2}, channel:{3}", context.m_logicAcc, context.m_logicAccReal, context.Platform, context.ChannelId);

            rets["ext"] = constructRetStr(context.m_logicAcc, resstr);
            context.m_retstr = serializer.Serialize(rets);
            return 0;
        }

        res = secondChannelAccProcess(context, ref resstr);
        if (res)
        {
            //CLOG.Info("land anysdk, 渠道表中存在, 最终acc:{0}, real_acc:{1} plat:{2}, channel:{3}", context.m_logicAcc, context.m_logicAccReal, context.Platform, context.ChannelId);

            rets["ext"] = constructRetStr(context.m_logicAcc, resstr);
            context.m_retstr = serializer.Serialize(rets);
            return 0;
        }

        // 到此步，说明是新账号，只需要在总表中添加一个新号就可以了。
        res = createAccInTotal(context, ref resstr);
        if (res)
        {
            //CLOG.Info("land anysdk, 是新账号，在总表中直接创建, 最终acc:{0}, real_acc:{1} plat:{2}, channel:{3}", context.m_logicAcc, context.m_logicAccReal, context.Platform, context.ChannelId);

            rets["ext"] = constructRetStr(context.m_logicAcc, resstr);
            context.m_retstr = serializer.Serialize(rets);
            return 0;
        }

        return 0;
    }

    public override string getSuccessRetStr(AccLandContext context)
    {
        string clientkey = context.m_randKey.ToString() + ":" + context.m_ticks;
        string retStr = AESHelper.AESEncrypt(clientkey, LoginCommon.AES_KEY);
        return retStr;
    }

    string getQueryString(AccLandContext context)
    {
        NameValueCollection req = context.Request.Form;
        string args = "";
        foreach (string key in req.AllKeys)
        {
            args += key + "=" + req[key] + "&";
        }
        args = args.Substring(0, args.Length - 1);
        return args;
    }

    string postLogin(AccLandContext context)
    {
        string msg = "";
        try
        {
            Uri uri = new Uri(URL_LOGIN);
            HttpWebRequest requester = WebRequest.Create(uri) as HttpWebRequest;
            requester.Method = "POST";
            requester.Timeout = 10000;
            string qstr = getQueryString(context);
            byte[] bs = Encoding.UTF8.GetBytes(qstr);
            requester.ContentType = "application/x-www-form-urlencoded";
            requester.ContentLength = bs.Length;
            using (Stream reqStream = requester.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }

            HttpWebResponse responser = requester.GetResponse() as HttpWebResponse;
            StreamReader reader = new StreamReader(responser.GetResponseStream(), Encoding.UTF8);
            msg = reader.ReadToEnd(); 
        }
        catch (System.Exception ex)
        {
            CLOG.Info("anysdk_login:{0}", ex.ToString());
        }
        return msg;
    }

    bool isPayLogin(AccLandContext context, Dictionary<string, object> rets)
    {
        LoginInfo loginInfo = null;
        bool res = false;
        try
        {
            loginInfo = JsonHelper.ParseFromStr<LoginInfo>(context.Request.Form["server_ext_for_login"]);
        }
        catch (Exception ex)
        {
        }

        try
        {
            Dictionary<string, object> savedata = (Dictionary<string, object>)rets["common"];

            if (savedata.ContainsKey("server_id"))
            {
                if (savedata["server_id"].ToString() == "payLogin")
                {
                    res = true;
                }
            }
            if (loginInfo != null && loginInfo.isPayLogin.Equals("true"))
            {
                res = true;
            }
        }
        catch (Exception ex)
        {
        }
        return res;
    }

    void buildContext(AccLandContext context, Dictionary<string, object> rets)
    {
        try
        {
            context.PlatLoginTable = "anysdk_login";

            Dictionary<string, object> common = (Dictionary<string, object>)rets["common"];
            context.ChannelId = Convert.ToString(common["channel"]);
            context.UserId = Convert.ToString(common["uid"]);
            context.UserSdk = Convert.ToString(common["user_sdk"]);

            LoginInfo loginInfo = null;
            try
            {
                loginInfo = JsonHelper.ParseFromStr<LoginInfo>(context.Request.Form["server_ext_for_login"]);
            }
            catch (Exception ex)
            {
            }
            if (loginInfo != null && !string.IsNullOrEmpty(loginInfo.deviceID))
            {
                context.DeviceId = loginInfo.deviceID;
            }
            else
            {
                context.DeviceId = "";
            }

            context.Platform = "anysdk";
        }
        catch (System.Exception ex)
        {
        }
    }

    void testData(Dictionary<string, object> rets, AccLandContext context)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("channel", context.Request.QueryString["channel"]);
        data.Add("uid", context.Request.QueryString["uid"]);
        data.Add("user_sdk", context.Request.QueryString["sdk"]);
        rets.Add("common", data);
        rets.Add("status", "ok");
    }

    class LoginInfo
    {
        public string isPayLogin = null;
        public string deviceID = null;
    }
}

//////////////////////////////////////////////////////////////////////////
// 单接的sdk
public class AccLandThirdPartySdk : AccLand
{
    public override int startLand(AccLandContext context)
    {
        bool res = false;
        res = firstTotalAccProcess(context, ref context.m_retstr);
        if (res)
        {
            //CLOG.Info("land thirdparty, 总表中存在, 最终acc:{0}, real_acc:{1} plat:{2}, channel:{3}", context.m_logicAcc, context.m_logicAccReal, context.Platform, context.ChannelId);
            return 0;
        }

        res = secondChannelAccProcess(context, ref context.m_retstr);
        if (res)
        {
           // CLOG.Info("land thirdparty, 渠道表中存在, 最终acc:{0}, real_acc:{1} plat:{2}, channel:{3}", context.m_logicAcc, context.m_logicAccReal, context.Platform, context.ChannelId);
            return 0;
        }

        // 到此步，说明是新账号，只需要在总表中添加一个新号就可以了。
        res = createAccInTotal(context, ref context.m_retstr);
        if (res)
        {
           // CLOG.Info("land thirdparty, 是新账号，在总表中直接创建, 最终acc:{0}, real_acc:{1} plat:{2}, channel:{3}", context.m_logicAcc, context.m_logicAccReal, context.Platform, context.ChannelId);
            return 0;
        }

        return 0;
    }

    public override string getSuccessRetStr(AccLandContext context)
    {
        RSAHelper rsa = new RSAHelper();
        string clientkey = context.m_randKey.ToString() + ":" + context.m_ticks;
        string aeskey = AESHelper.AESEncrypt(clientkey, LoginCommon.AES_KEY);
        string tmp = constructRetStr(context.m_logicAcc, aeskey);
        string retStr = HttpLogin.BuildAccount.buildLuaReturn(0, tmp);
        return retStr;
    }
}





















































