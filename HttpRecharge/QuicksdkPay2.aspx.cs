using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class QuicksdkPay2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackQuicksdk pay = new PayCallbackQuicksdk();
            pay.Md5Key = "dif5qigh2ehk8l8ogeh1a36hyjrspsw3";
            pay.CallbackKey = "55214501439029486414797015259004";
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}