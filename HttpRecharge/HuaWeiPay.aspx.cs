using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class HuaWeiPay : System.Web.UI.Page
    {
        public const string PUB_KEY = @"<RSAKeyValue><Modulus>yDhkLHLjHzRnioypp+bPRQozXYCVjQRPCO+KWhEOe4ZLcBda5gebiysIAteGit4bUbzKlkouBs+C5bdrM2RimtmB1oiRiLwduseKdlFlmmCzE/GOXjWMRvbCFZOgRlh5d4PN2ea1SUmeEBG/N6D3XWJ4SyqNFfWmT5c5Kxsi5AOor90wKlNkBrfpUHAvWZx5V+uA1LSkI9yG5Jtv+z+G5m+ffTNcR9FpzyaXHOu08BTXBmLjb9mOWdcMgrE1zhJFxK2yWe8ycVu2c8Ch+fqyos96LnXN1f7vnDTPCRiUQGFFlIJRG3mlRWSJMHZkQjljp7JlR5xnKbC/q8HwQKMWXw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackHuaWeiH5 obj = new PayCallbackHuaWeiH5();
            obj.PublicKey = PUB_KEY;
            obj.PayTableName = "huawei_pay";
            string str = obj.notifyPay(Request);
            Response.ContentType = "text/plain";
            Response.Write(str);
        }
    }
}
