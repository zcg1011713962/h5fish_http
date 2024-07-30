using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.Platform
{
    public partial class vivo_jieji_login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var obj = new LoginBase();
            PlatformSdk.getSdkByPlatform(PlatformName.PLAT_VIVO_JIEJI, ref obj.UserSdk);
            string str = obj.doLogin(LoginTable.ACC_VIVO_JIEJI, Request);
            Response.Write(str);
        }
    }
}