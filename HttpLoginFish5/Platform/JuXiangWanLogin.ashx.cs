using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpLogin.Platform
{
    public class JuXiangWanLogin : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            XianWanLoginService obj = new XianWanLoginService();
            obj.Channel = "100013";
            obj.AccTable = PayTable.JUXIANGWAN_ACC;
            string str = obj.doLogin(context.Request);
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