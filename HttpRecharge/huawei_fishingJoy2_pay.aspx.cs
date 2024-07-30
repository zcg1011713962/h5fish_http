using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class huawei_fishingJoy2_pay : System.Web.UI.Page
    {
        // 公钥
        public static string PUBLIC_KEY = "<RSAKeyValue><Modulus>hZfopqM3+AtJtXdPAdNZgHin3pHPPkJeVBZLR2hj6xkg6WBl41c+ltnGhrhCBH90WRboGYVJSW0uhmhwMOB8/Q==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackHuaWei pay = new PayCallbackHuaWei();
            pay.PublicKey = PUBLIC_KEY;
            pay.PayTableName = PayTable.HUAWEI_FISHING_JOY2_PAY;
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}