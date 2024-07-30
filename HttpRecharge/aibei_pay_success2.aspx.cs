using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    // 捕鱼3爱贝成功后的回调
    public partial class aibei_pay_success2 : System.Web.UI.Page
    {
        const string PUBLIC_KEY = "<RSAKeyValue><Modulus>nDP6r7LeKMId+FO2WocsjbQZUnTlgp02vrwYZP4pkdaaVFuO/TmTkwjyEv52RJN5epItxZuOgI5zT8uRecSZ77zM3YLdVMVeQkOuPcO9cMxS1qdqwR8vesGtoCMu0NWGCokxGQUW/6zveP7Et0fGAp2VaOoEVypz/WRiMpDXf20=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        // 爱贝支付成功后的回调页面
        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbacAiBei pay = new PayCallbacAiBei();
            pay.PublicKey = PUBLIC_KEY;
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}