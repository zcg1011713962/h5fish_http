using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class hospital_pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackHospital pay = new PayCallbackHospital();
            pay.PayTableName = PayTable.HOSPITAL_PAY;
            pay.AppSecret = "3EAFC22B2F65759C57D012F6B20225";
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}