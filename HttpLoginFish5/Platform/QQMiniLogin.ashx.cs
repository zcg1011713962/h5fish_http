using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpLogin.Platform
{
    // 手机QQ游戏登录
    public class QQMiniLogin : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            QQMiniSdkLoginService obj = new QQMiniSdkLoginService();
            obj.AppId = "1110059980";
            obj.AppSecret = "4zxAqqzjmEuBhnAR";
            obj.Code = context.Request.QueryString["code"];
            obj.PhoneSys = context.Request.QueryString["phoneSys"];
            string str = obj.doLogin(context.Request);
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