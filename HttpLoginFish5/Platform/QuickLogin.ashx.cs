using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpLogin.Platform
{
    /// <summary>
    /// QuickLogin1 的摘要说明
    /// </summary>
    public class QuickLogin : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            QuickSdkLogin obj = new QuickSdkLogin();
            string str = obj.doLogin(context.Request);
           // context.Response.Status = "200";
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