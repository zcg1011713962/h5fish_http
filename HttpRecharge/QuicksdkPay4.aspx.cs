using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class QuicksdkPay4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackQuicksdk pay = new PayCallbackQuicksdk();
            pay.Md5Key = "sd9lr27dtxll7sn1ygquwt4rk35aqrni";
            pay.CallbackKey = "30498283148781108038986532928772";
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}