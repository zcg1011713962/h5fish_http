using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.Platform
{
    public partial class fan_login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var fan = new LoginBase();
            PlatformSdk.getSdkByPlatform(PlatformName.PLAT_FAN, ref fan.UserSdk);
            string str = fan.doLogin(LoginTable.ACC_FAN, Request);
            Response.Write(str);
        }
    }
}