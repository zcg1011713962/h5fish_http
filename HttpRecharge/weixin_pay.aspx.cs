using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class weixin_pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var pay = new PayCallbackWeixin();
            string retStr = pay.notifyPay(Request);
            Response.Write(retStr);
        }
    }
}