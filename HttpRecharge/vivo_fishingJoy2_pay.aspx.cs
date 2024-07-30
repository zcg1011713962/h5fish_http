using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class vivo_fishingJoy2_pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackVIVO pay = new PayCallbackVIVO();
            pay.PayTableName = PayTable.VIVO_FISHING_JOY2;
            pay.VivoCpKey = PayTable.VIVO_CP_KEY_FISHING_JOY2;

            string str = pay.notifyPay(Request);
            if (str != "success")
            {
                Response.StatusCode = 201;
            }
            Response.Write(str);
        }
    }
}