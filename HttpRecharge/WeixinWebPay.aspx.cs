using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class WeixinWebPay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var pay = new PayCallbackWeixinWeb();
            pay.ApiSecret = "93dSEaf154548w7efaADF4541sd4aefa";
            pay.PayTableName = "weixin_web_pay";
            string retStr = pay.notifyPay(Request);
            Response.Write(retStr);
        }
    }
}