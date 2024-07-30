using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class DuoyouPay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var pay = new PayCallbackDuoyouApple();
            pay.AppId = "52B78620A50F940AFB93199550567757";
            pay.AppKey = "53db1882f141ffb580417a0eaa9ed3ee";
            pay.PayTableName = "duoyou_pay";

            string retStr = pay.notifyPay(Request);
            Response.Write(retStr);
        }
    }
}