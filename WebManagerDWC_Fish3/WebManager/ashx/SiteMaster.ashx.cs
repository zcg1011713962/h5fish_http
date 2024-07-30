using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// SiteMaster 的摘要说明
    /// </summary>
    public class SiteMaster : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            int op = Convert.ToInt32(context.Request["op"]);
            GMUser user = (GMUser)context.Session["user"];

            if(op == 1)
                changeMenu(user);

            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        //隐藏热点切换菜单栏
        public void changeMenu(GMUser user) 
        {
            user.changeMenuGroup();
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