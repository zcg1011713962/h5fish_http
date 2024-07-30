using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class DuoyouPayAndroid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var pay = new PayCallbackDuoyouApple();
            pay.AppId = "FC0A563E8BB41792998A345AA73449BF";
            pay.AppKey = "0675c4054149989421d2501b33f1ecba";
            pay.PayTableName = PayTable.QUICKSDK_PAY;

            string retStr = pay.notifyPay(Request);
            Response.Write(retStr);
        }
    }
}