using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpLogin.Platform
{
    public class XiaomiLogin : IHttpHandler
    {
        const string CHANNEL = "100504";

        public void ProcessRequest(HttpContext context)
        {
            LoginBase obj = new LoginBase();

            if (obj.chekOrReply(context.Request, context.Response, LoginBase.CHK_PARAM_ACCOUNT))
            {
                LoginBaseParam param = new LoginBaseParam();
                param.m_acc = CHANNEL + "_" + context.Request.Form["acc"];
                param.m_platform = "xiaomi";
                param.m_channelId = CHANNEL;
                param.m_deviceId = context.Request.Form["deviceID"];
                param.Request = context.Request;
                string str = obj.startLogin(param);

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
    }
}