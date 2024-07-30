using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.Platform
{
    public partial class vivo_baiwan_login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var obj = new LoginBase();
            PlatformSdk.getSdkByPlatform(PlatformName.PLAT_VIVO_BAIWAN, ref obj.UserSdk);
            string str = obj.doLogin(LoginTable.ACC_VIVO_BAIWAN, Request);
            Response.Write(str);
        }
    }
}