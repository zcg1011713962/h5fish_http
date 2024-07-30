using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.Platform
{
    public partial class meizu_online_login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var obj = new LoginBase();
            PlatformSdk.getSdkByPlatform(PlatformName.PLAT_MEIZU_ONLINE, ref obj.UserSdk);
            string str = obj.doLogin(LoginTable.ACC_MEIZU_ONLINE, Request);
            Response.Write(str);
        }
    }
}