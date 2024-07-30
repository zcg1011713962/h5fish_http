using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Configuration;

namespace WebManager
{
    public class Global : System.Web.HttpApplication
    {
        public static RedisMgr m_redisMgr = null;
        public static string REDIS_URL = "GLOBAL";// Convert.ToString(WebConfigurationManager.AppSettings["redis"]);

        void Application_Start(object sender, EventArgs e)
        {
            m_redisMgr = new RedisMgr();
           // m_redisMgr.connect(REDIS_URL);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码
        }

        void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码
        }

        void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码
            // 开始时设置为空
            Session["user"] = null;
        }

        void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
            // 或 SQLServer，则不会引发该事件。
            AccountMgr.getInstance().sessionEnd(Session);
        }
    }
}
