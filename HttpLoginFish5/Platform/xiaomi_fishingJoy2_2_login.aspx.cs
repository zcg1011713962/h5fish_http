using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.Platform
{
    public partial class xiaomi_fishingJoy2_2_login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var ysdk = new LoginBase();
            PlatformSdk.getSdkByPlatform(PlatformName.PLAT_XIAOMI_FISHING_JOY2_2, ref ysdk.UserSdk);
            string str = ysdk.doLogin(LoginTable.ACC_XIAOMI_FISHING_JOY2_2, Request);
            Response.Write(str);
        }
    }
}