using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Configuration;
using System.Text;

namespace HttpLogin.Platform
{
    public partial class xunlei_login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var ysdk = new LoginBase();
            PlatformSdk.getSdkByPlatform(PlatformName.PLAT_XUNLEI, ref ysdk.UserSdk);
            string str = ysdk.doLogin(LoginTable.ACC_XUN_LEI, Request);
            Response.Write(str);
        }
    }
}