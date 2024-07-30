using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Net;
using System.Configuration;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace HttpLogin.Platform
{
    public partial class anysdk_login : System.Web.UI.Page
    {
        static string login = "http://oauth.anysdk.com/api/User/LoginOauth/";
        static string AES_KEY = "&@*(#kas9081fajk";

        class LoginInfo
        {
            public string isPayLogin = null;
            public string deviceID = null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (LoginCommon.isAccLand())
            {
               // CLOG.Info("land anysdk, req login");

                AccLandAnysdk anylogin = new AccLandAnysdk();
                AccLandContext context = new AccLandContext();
                context.Request = Request;
                anylogin.startLand(context);
                Response.Write(context.m_retstr);
                return;
            }

            NameValueCollection req = Request.Form;
            string args = "";
            foreach (string key in req.AllKeys)
            {
                args += key + "=" + req[key] + "&";
            }
            args = args.Substring(0, args.Length - 1);

            LoginInfo loginInfo = null;
            try
            {
                loginInfo = JsonHelper.ParseFromStr<LoginInfo>(req["server_ext_for_login"]);
            }
            catch (Exception ex)
            {

            }
            string msg = "";
            try
            {
                Uri uri = new Uri(login);
                HttpWebRequest requester = WebRequest.Create(uri) as HttpWebRequest;
                requester.Method = "POST";
                requester.Timeout = 3000;
                byte[] bs = Encoding.UTF8.GetBytes(args);
                requester.ContentType = "application/x-www-form-urlencoded";
                requester.ContentLength = bs.Length;
                using (Stream reqStream = requester.GetRequestStream())
                {                   
                    reqStream.Write(bs, 0, bs.Length);
                }

                HttpWebResponse responser = requester.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(responser.GetResponseStream(), Encoding.UTF8);
                msg = reader.ReadToEnd();                

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> rets = serializer.Deserialize<Dictionary<string, object>>(msg);
                if (rets["status"].ToString() == "ok")
                {
                    //因为支付而调用SDK登录,记录数据
                    bool isPayLogin = false;

                    Dictionary<string, object> savedata = (Dictionary<string, object>)rets["common"];

                    if (savedata.ContainsKey("server_id"))
                    {
                        if (savedata["server_id"].ToString() == "payLogin")
                        {
                            isPayLogin = true;
                        }
                    }
                    if (loginInfo != null && loginInfo.isPayLogin.Equals("true"))
                    {
                        isPayLogin = true;
                    }

                    if (!isPayLogin)
                    {
                        Random rd = new Random();
                        DateTime now = DateTime.Now;
                        int randkey = rd.Next();
                        savedata["randkey"] = randkey;
                        savedata["lasttime"] = now.Ticks;
                        savedata["ip"] = Request.ServerVariables.Get("Remote_Addr").ToString();
                        string clientkey = randkey.ToString() + ":" + now.Ticks.ToString();
                        rets["ext"] = AESHelper.AESEncrypt(clientkey, AES_KEY);

                        string channelID = savedata["channel"].ToString();
                        channelID = LoginCommon.channelToString(channelID);

                        string tempacc = savedata["channel"].ToString() + "_" + savedata["uid"].ToString();
                        if (MongodbAccount.Instance.KeyExistsBykey("anysdk_login", "acc", tempacc) == false)
                        {
                            Dictionary<string, object> savelog = new Dictionary<string, object>();
                            savelog["acc"] = tempacc;
                            savelog["acc_real"] = tempacc;
                            savelog["ip"] = Request.ServerVariables.Get("Remote_Addr").ToString();
                            savelog["time"] = now;
                            savelog["channel"] = channelID;

                            MongodbAccount.Instance.ExecuteInsert("RegisterLog", savelog);
                        }

                        savedata["lastip"] = savedata["ip"];
                        if (loginInfo != null && !string.IsNullOrEmpty(loginInfo.deviceID))
                        {
                            savedata["deviceId"] = loginInfo.deviceID;
                        }

                        msg = MongodbAccount.Instance.ExecuteStoreBykey("anysdk_login", "acc", tempacc, savedata);
                        if (msg == "")
                        {
                            msg = serializer.Serialize(rets);

                            /*Dictionary<string, object> savelog = new Dictionary<string, object>();
                            savelog["acc"] = tempacc;
                            savelog["acc_real"] = tempacc;
                            if (loginInfo != null && !string.IsNullOrEmpty(loginInfo.deviceID))
                            {
                                savelog["acc_dev"] = loginInfo.deviceID;
                            }
                            savelog["ip"] = Request.ServerVariables.Get("Remote_Addr").ToString();
                            savelog["time"] = now;
                            savelog["channel"] = channelID;

                            MongodbAccount.Instance.ExecuteInsert("LoginLog", savelog);*/
                        }

                        if (loginInfo != null && !string.IsNullOrEmpty(loginInfo.deviceID))
                        {
                            if (Advertisement.isCheck(channelID))
                            {
                                Advertisement.call(Advertisement.OP_REG, loginInfo.deviceID);
                            }
                        }
                    }
                    else
                    {
                        CLOG.Info("anysdk_login, isPayLogin:{0}, {1}", isPayLogin,
                            savedata["channel"].ToString() + "_" + savedata["uid"].ToString());
                    }
                }
                else
                {
                    CLOG.Info("anysdk_login, status:{0}", rets["status"].ToString());
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                CLOG.Info("anysdk_login:{0}", msg);
            }

            Response.Write(msg);           
        }
    }
}