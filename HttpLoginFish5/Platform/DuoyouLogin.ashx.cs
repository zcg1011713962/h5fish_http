using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net;

namespace HttpLogin.Platform
{
    public class DuoyouLogin : IHttpHandler
    {
        const string DUOYOU_CHANNEL = "100006";
        const string CHECK_URL = "https://api.aiduoyou.com/member/login_check?app_id={0}&uid={1}&access_token={2}";
        const string APP_ID = "52B78620A50F940AFB93199550567757";

        public void ProcessRequest(HttpContext context)
        {
            CDuoYouLogin objLogin = new CDuoYouLogin();
            objLogin.AppId = APP_ID;
            objLogin.ChannelId = DUOYOU_CHANNEL;
            objLogin.doLogin(context);
            return;



            CLOG.Info("DuoyouLogin enter");

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
                    param.m_acc = DUOYOU_CHANNEL + "_" + context.Request.Form["acc"];
                    param.m_platform = "duoyou";
                    param.m_channelId = DUOYOU_CHANNEL;
                    param.m_deviceId = context.Request.Form["deviceID"];
                    param.Request = context.Request;
                    str = obj.startLogin(param);
                    CLOG.Info("DuoyouLogin, {0} success", param.m_acc);
                }
                else
                {
                    CLOG.Info("DuoyouLogin checklogin error, {0}", token);
                }
                
                context.Response.ContentType = "text/plain";
                context.Response.Write(str);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // 检测登录
        bool checkLogin(LoginBase obj, string uid, string token, ref string outstr)
        {
            try
            {
                string url = string.Format(CHECK_URL, APP_ID, uid, token);
                //CLOG.Info("duoyou: " + url);

                // byte[] ret = WxPayAPI.HttpService.Get(url);
                string retstr = WxPayAPI.HttpService.Get(url, (req)=> {
                   // req.KeepAlive = false;
                   // req.ProtocolVersion = HttpVersion.Version10;
                }, 
                
                (hd)=> {
                    hd.Add("SecurityProtocol", SecurityProtocolType.Tls11);
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
}