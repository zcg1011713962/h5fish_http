using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.ysdk
{
    public partial class tthy_ysdk_pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            YsdkPay ysdk = new YsdkPay();
            ysdk.IsTest = false;
            ysdk.YsdkLogTable = PayTable.TTHY_YSDK_PAY_LOG;
            ysdk.YsdkPayTable = PayTable.TTHY_YSDK_PAY;
            YsdkCommon.getTthyAppKeyId(ysdk);

            string str = ysdk.notifyPay(Request);
            Response.Write(str);
        }
    }
}