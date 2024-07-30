using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class qingyuan_pay : System.Web.UI.Page
    {
        // 公钥
        public static string PUBLIC_KEY = "<RSAKeyValue><Modulus>76QQ2HlitVRmQsBE8+TB0s5mWoPM4Awx/PSaoCdRFA04Qax2JW5LEdwAjkX7w4eifo0ilqFyECXEtdhJcqs8FGk6U7YZ1IdI1PJiYDsybk8LZNsWSjRJO9TNPfIeLZq4iQjzFhBzJ/tFP6gqB0nc237TcF06Y0a3+i+D8hQHnj0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackQingYuan pay = new PayCallbackQingYuan();
            pay.PublicKey = PUBLIC_KEY;
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}