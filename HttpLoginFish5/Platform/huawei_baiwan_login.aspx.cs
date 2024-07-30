using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.Platform
{
    public partial class huawei_baiwan_login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var huawei = new LoginBase();
            PlatformSdk.getSdkByPlatform(PlatformName.PLAT_HUAWEI_BAIWAN, ref huawei.UserSdk);
            string str = huawei.doLogin(LoginTable.ACC_HUA_WEI_BAIWAN, Request);
            Response.Write(str);
        }
    }
}