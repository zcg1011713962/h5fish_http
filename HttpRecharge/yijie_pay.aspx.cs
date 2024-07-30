using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class yijie_pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackYiJie pay = new PayCallbackYiJie();
            pay.ShareKey = "93ZVWKK1S984ZLFI0P8WCYO62B50JAVJ";
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}