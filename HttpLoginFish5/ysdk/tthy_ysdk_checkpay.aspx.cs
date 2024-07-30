using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.ysdk
{
    public partial class tthy_ysdk_checkpay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            YsdkCheckPay ysdk = new YsdkCheckPay();
            ysdk.IsTest = false;
            YsdkCommon.getTthyAppKeyId(ysdk);

            string str = ysdk.check(Request);
            Response.Write(str);
        }
    }
}

