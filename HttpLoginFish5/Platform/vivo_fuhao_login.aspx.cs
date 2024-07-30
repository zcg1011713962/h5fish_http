using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.Platform
{
    // vivo-富豪登录，与 vivo/VIVOGetOrderNum3.asp对应
    public partial class vivo_fuhao_login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var obj = new LoginBase();
            PlatformSdk.getSdkByPlatform(PlatformName.PLAT_VIVO_FUHAO, ref obj.UserSdk);
            string str = obj.doLogin(LoginTable.ACC_VIVO_FUHAO, Request);
            Response.Write(str);
        }
    }
}