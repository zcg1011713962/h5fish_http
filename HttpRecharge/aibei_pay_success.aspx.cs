using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class aibei_pay_success : System.Web.UI.Page
    {
        const string PUBLIC_KEY = "<RSAKeyValue><Modulus>pYUuhWppMDzywB1UDtivpSTIvPzwj4rKlNQhQCU0GUr4PdOzfm+nyCvMmo65B7/KCHlzuT9OWubuTCV65qee0aCNIRxi5KRoV1vuqL2kVkTipdd+0a4x6tQk/vfHsh4S0vxh2/JVDHZMVRtgIxxqWvjPq3ydZkonfmg1KXGne3M=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

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