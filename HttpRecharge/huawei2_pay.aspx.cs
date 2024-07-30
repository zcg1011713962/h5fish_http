using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    // 游戏名：至尊电玩捕鱼 
    public partial class huawei2_pay : System.Web.UI.Page
    {
        // 公钥
        public static string PUBLIC_KEY = "<RSAKeyValue><Modulus>hiWnQD4whNLySq0yOaRE3CGyQ7CbCscCwM7iprcmzpf9fXa2kuBA5YLvA88ft67LQfvaOBiJMAMOwJ9h5c9i6+tPc5Rqqz+BL72OKQ9itIHoTlx4bAI4p2Z9Zrkssld6UR/8oRAhNutIBc5eWENfsSnpsmTYzyqJUig9cQxuvE0HNlL5BqmKy8HIP7Ku3Mz5WRoKgGc6sZwLxeSMqYG8ylqR+U3pTY3didqVGSsRG9PHx/kXN8QoZUv8FlKHBRVLCOkGWfhAsayJ6DKb7LKpx+C3mS58Cem8Gpo/YJldOit7dVe4CVF4shquCCNt/fzID5FYzBE4Ze8NgPHtLGZ6zw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackHuaWei pay = new PayCallbackHuaWei();
            pay.PublicKey = PUBLIC_KEY;
            pay.PayTableName = PayTable.HUAWEI2_PAY;
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}