using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    // 阿里从页面支付的回调
    public partial class AliWebPay : System.Web.UI.Page
    {
        //public static string PUBLIC_KEY = "<RSAKeyValue><Modulus>iEucpUP2fuzKqQNZ38fJrUFOrRJEhUc0uu1x1GjWvO3kxaXF37SLXNbHAB4xedDtEfRR3V27AbShhI5UIjaW0OILkconPa3PwVqTbYcasBC6/1EI2Hr0GQMq+px0500Oi3w38Gil3WtBTNmZwmGEDkReRYgKV0xTYi69Y7uLh2sQJ2PS/UkPHW+CPnKDbDjmvQoLpCmVbK/B3KqhF5hzFDXuEqS2qNifZ3fNmF17ag0o6N3JbFzmNplj6xGJnVxskX2okElTo0odbYhi7etPIuA7ffqZ9glHcNu/5bHjuf3z0CIbiR0jXIopeJPEs1HRnfkEmLxGfmJ5q3kSl46jgw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        public const string TABLE_NAME = PayTable.ALI_WEB_PAY;
      //  public const string ALI_APP_ID = "2021001161687578";

        public static string PUBLIC_KEY = @"<RSAKeyValue><Modulus>gv/X/QlHzu52fSJG8eb8I4atz1BXAjcXOuJpN/w/0MY4UQj6U7q40Kl4/a+cEQSpL2m303yBNk6agVM13pPmtZpG7vpfE2ZprgkGPslqwIueYhNNZUS2Apyzy6u10/XcMk1a0F1QI2E1vS7CZiitDBt4FQLudFEFgNxf4kT4CUV/LztpjJjeGHPNxdMAMQTJo08UVRMPLibUDylQjmbk7lksx4G4suKb8wbwtWG82AmmOSR5hOscJ2RYeDdQEITiJzBP7lo3mr8khtqPRjOLwNqj221dqGHIZQsWZLl5Wmb91OTZ7wmyJWm45kDTnKBvX/Pjfasp7mCGgm7/G9BvhQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        public const string ALI_APP_ID = "2019082966655948";

        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackALiWeb pay = new PayCallbackALiWeb();
            pay.PublicKey = PUBLIC_KEY;
            pay.PayTableName = TABLE_NAME;
            pay.AppId = ALI_APP_ID;

            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}