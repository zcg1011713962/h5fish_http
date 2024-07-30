using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class QQMiniPay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackQQMinisdk obj = new PayCallbackQQMinisdk();
            obj.AppSecret = "4zxAqqzjmEuBhnAR";
            obj.PayTableName = "qqmini_pay";
            string str = obj.notifyPay(Request);
            Response.Write(str);
        }
    }
}