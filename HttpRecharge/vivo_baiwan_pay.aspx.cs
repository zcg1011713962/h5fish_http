using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    // 百万街机捕鱼-vivo
    public partial class vivo_baiwan_pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackVIVO pay = PayCallbackVIVO.createVivoPay(Request); //new PayCallbackVIVO();
            pay.PayTableName = PayTable.VIVO_BAI_WAN_PAY;
            pay.VivoCpKey = PayTable.VIVO_CP_KEY_BAI_WAN;

            string str = pay.notifyPay(Request);
            if (str != "success")
            {
                Response.StatusCode = 201;
            }
            Response.Write(str);
        }
    }
}