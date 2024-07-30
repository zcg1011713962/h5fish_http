using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.Platform
{
    public partial class tthy_qtt_login : System.Web.UI.Page
    {
        /*
            应用名称：天天口袋捕鱼趣头条z         
            应用appid：1725CC9C8B283BCC2
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            var obj = new LoginBase();
            PlatformSdk.getSdkByPlatform(PlatformName.PLAT_TTHY_QTT, ref obj.UserSdk);
            string str = obj.doLogin(LoginTable.ACC_TTHY_QTT, Request);
            Response.Write(str);
        }
    }
}