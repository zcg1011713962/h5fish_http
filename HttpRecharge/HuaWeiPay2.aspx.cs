using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class HuaWeiPay2 : System.Web.UI.Page
    {
        public const string PUB_KEY = @"<RSAKeyValue><Modulus>hiWnQD4whNLySq0yOaRE3CGyQ7CbCscCwM7iprcmzpf9fXa2kuBA5YLvA88ft67LQfvaOBiJMAMOwJ9h5c9i6+tPc5Rqqz+BL72OKQ9itIHoTlx4bAI4p2Z9Zrkssld6UR/8oRAhNutIBc5eWENfsSnpsmTYzyqJUig9cQxuvE0HNlL5BqmKy8HIP7Ku3Mz5WRoKgGc6sZwLxeSMqYG8ylqR+U3pTY3didqVGSsRG9PHx/kXN8QoZUv8FlKHBRVLCOkGWfhAsayJ6DKb7LKpx+C3mS58Cem8Gpo/YJldOit7dVe4CVF4shquCCNt/fzID5FYzBE4Ze8NgPHtLGZ6zw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

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