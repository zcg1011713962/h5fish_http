using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpLogin.Platform
{
    public class DanDanZhuanLogin : IHttpHandler
    {
        // 蛋蛋赚login, 没有接sdk, 而是自定义了登录页面， 便于统计闲玩有关数据
        public void ProcessRequest(HttpContext context)
        {
            XianWanLoginService obj = new XianWanLoginService();
            obj.Op = XianWanLoginService.OP_LOGIN;
            obj.Channel = "100009";
            obj.AccTable = "dandanzhuan_acc";
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