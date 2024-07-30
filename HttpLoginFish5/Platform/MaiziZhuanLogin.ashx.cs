using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpLogin.Platform
{
    public class MaiziZhuanLogin : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            XianWanLoginService obj = new XianWanLoginService();
            obj.Channel = "100012";
            obj.AccTable = PayTable.MAIZIZHUAN_ACC;
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