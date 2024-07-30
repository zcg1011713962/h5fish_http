using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpLogin.Platform
{
    public class QQGameLogin : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            QQLobby obj = new QQLobby();
            obj.Channel = context.Request.QueryString["Channel"];
            obj.OpenId = context.Request.QueryString["openid"];
            obj.OpenKey = context.Request.QueryString["openkey"];
            obj.Pf = context.Request.QueryString["pf"];
            obj.IsTest = true;

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