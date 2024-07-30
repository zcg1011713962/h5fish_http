using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpLogin.Platform
{
    public class XiaoZhuoLogin : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            XianWanLoginService obj = new XianWanLoginService();
            obj.Channel = "100014";
            obj.AccTable = PayTable.XIAOZHUO_ACC;
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