using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpLogin.Platform
{
    public class PaoPaoZhuanLogin : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            FastLoginParam loginParam = new FastLoginParam();
            XianWanLoginService obj = new XianWanLoginService();
            obj.Channel = "100015";
            obj.AccTable = PayTable.PAOPAOZHUAN_ACC;
            string deviceId = context.Request.Form["deviceId"];
            if (!string.IsNullOrEmpty(deviceId))
            {
                loginParam.m_deviceId = deviceId.ToUpper();
            }

            string str = obj.doLogin2(loginParam, context.Request);
            context.Response.ContentType = "text/plain";
            context.Response.Write(str);
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