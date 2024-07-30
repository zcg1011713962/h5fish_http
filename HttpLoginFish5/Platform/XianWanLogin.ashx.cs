using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpLogin.Platform
{
    // 闲玩login, 没有接sdk, 而是自定义了登录页面， 便于统计闲玩有关数据
    public class XianWanLogin : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            XianWanLoginService obj = new XianWanLoginService();
            obj.Op = XianWanLoginService.OP_LOGIN;
            obj.Channel = "100003";
            obj.AccTable = "xianwan_acc";
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