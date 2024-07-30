using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class vivo_pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackVIVO pay = PayCallbackVIVO.createVivoPay(Request); //new PayCallbackVIVO();
            pay.PayTableName = PayTable.VIVO_PAY;
            pay.VivoCpKey = PayTable.VIVO_CP_KEY;

            string str = pay.notifyPay(Request);
            if (str != "success")
            {
                Response.StatusCode = 201;
            }
            Response.Write(str);
        }
    }
}