using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpLogin.Platform
{
    /// <summary>
    /// WeiXinMiniLogin 的摘要说明
    /// </summary>
    public class WeiXinMiniLogin : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            WxMinSdkLoginService obj = new WxMinSdkLoginService();
            obj.AppId = "wxe15933c4db0cc3b5";
            obj.AppSecret = "bf950ed42d6dff541aa05521e3cfa9e2";
            obj.InviteAcc = context.Request.QueryString["invite"]; // 邀请人账号
            CLOG.Info("initeacc= {0}", obj.InviteAcc);
            obj.Code = context.Request.QueryString["code"];
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