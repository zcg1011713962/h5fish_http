using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    // 采用oppo的支付回调
    public partial class tthy_oppo_pay : System.Web.UI.Page
    {
        // 公钥是通用的，在sdk压缩包中
        public static string PUBLIC_KEY = "<RSAKeyValue><Modulus>pq3mCJD8FaLykfKyx1pRVcO2A35vbkDiSi/PUpresqVxlXawCipdMDWHN/43nAKD4tspQoX2tokbuYLpycjZqVmc5QjUSGrueX8RltFxLuedyIqazmXGsOTrmBpX+tIGvEE+qk6D9P3aYi/2W5jVDvZFIaLCIxLttrhYEfuXGMk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackOppo pay = new PayCallbackOppo();
            pay.PublicKey = PUBLIC_KEY;
            pay.PayTableName = PayTable.TTHY_OPPO_PAY;
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}