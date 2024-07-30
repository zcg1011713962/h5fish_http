using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace HttpLogin
{
    public class Global : System.Web.HttpApplication
    {
        static WeiXinMiniMidasTokenMgr s_mgr;
        static QQMiniMidasTokenMgr s_mgr1;
        static OfficialAccountsTokenMgr s_mgr2;

        public static WeiXinMiniMidasTokenMgr getTokenMgr()
        {
            return s_mgr;
        }

        public static QQMiniMidasTokenMgr getQQTokenMgr()
        {
            return s_mgr1;
        }

        public static OfficialAccountsTokenMgr getOfficialAccountTokenMgr()
        {
            return s_mgr2;
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            s_mgr = new WeiXinMiniMidasTokenMgr();
            s_mgr1 = new QQMiniMidasTokenMgr();
            s_mgr2 = new OfficialAccountsTokenMgr();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}