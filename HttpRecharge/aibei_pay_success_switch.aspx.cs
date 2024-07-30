using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    // 爱贝，魅族切支付。fish3不需要正式发布
    public partial class aibei_pay_success_switch : System.Web.UI.Page
    {
        const string PUBLIC_KEY = "<RSAKeyValue><Modulus>tNELxz74iUxLvZlZYWE3f4FTemG/cwMOdcM7LcWVB/wDsi6ohuHIkWzb74W5KBEwyNVLo516M2u6H3wI1rf6Jd/JrpiN4PIdRJdtij9Vfe9cmTnFXtYMDs5d4VHTQB/hpmtrT2lLt3R4u7kzqwN2wG1f/WX3aMzVkk2oD2IPhQc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbacAiBei pay = new PayCallbacAiBei();
            pay.PublicKey = PUBLIC_KEY;
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}