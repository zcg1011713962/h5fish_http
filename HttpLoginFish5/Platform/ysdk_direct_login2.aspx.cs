using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.Platform
{
    public partial class ysdk_direct_login2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var ysdk = new LoginBase();
            ysdk.UserSdk = "ysdkdirect";
            string str = ysdk.doLogin(LoginTable.ACC_YSDK_DIRECT2, Request);
            Response.Write(str);
        }
    }
}