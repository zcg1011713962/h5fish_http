using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    // 支付宝二维码支付回调
    public partial class AliQRPay : System.Web.UI.Page
    {
        public static string PUBLIC_KEY = @"<RSAKeyValue><Modulus>gv/X/QlHzu52fSJG8eb8I4atz1BXAjcXOuJpN/w/0MY4UQj6U7q40Kl4/a+cEQSpL2m303yBNk6agVM13pPmtZpG7vpfE2ZprgkGPslqwIueYhNNZUS2Apyzy6u10/XcMk1a0F1QI2E1vS7CZiitDBt4FQLudFEFgNxf4kT4CUV/LztpjJjeGHPNxdMAMQTJo08UVRMPLibUDylQjmbk7lksx4G4suKb8wbwtWG82AmmOSR5hOscJ2RYeDdQEITiJzBP7lo3mr8khtqPRjOLwNqj221dqGHIZQsWZLl5Wmb91OTZ7wmyJWm45kDTnKBvX/Pjfasp7mCGgm7/G9BvhQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        public const string ALI_APP_ID = "2019082966655948";

        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackALiWeb pay = new PayCallbackALiWeb();
            pay.PublicKey = PUBLIC_KEY;
            pay.PayTableName = PayTable.ALI_WEB_PAY;
            pay.AppId = ALI_APP_ID;
            pay.ErrorPrefix = "PayCallbackALiQR";

            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}