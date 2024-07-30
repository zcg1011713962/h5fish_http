using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpLogin.Platform
{
    // 华为登录
    public class HuaWeiLogin : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            LoginBase obj = new LoginBase();

            if (obj.chekOrReply(context.Request, context.Response, LoginBase.CHK_PARAM_ACCOUNT))
            {
                string channel = context.Request.Form["channel"];
                if (string.IsNullOrEmpty(channel))
                {
                    channel = PayTable.CHANNEL_HUAWEI;
                }

                LoginBaseParam param = new LoginBaseParam();
                param.m_acc = channel + "_" + context.Request.Form["acc"];
                param.m_platform = "huawei";
                param.m_channelId = channel;
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