using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace HttpApi
{
    public class Global : System.Web.HttpApplication
    {
        static CacheData s_cache = new CacheData();

        static TCacheData s_cacheT = new TCacheData();

        public static CacheData getCacheData()
        {
            return s_cache;
        }

        public static TCacheData getTCacheData()
        {
            return s_cacheT;
        }

        protected void Application_Start(object sender, EventArgs e)
        {

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