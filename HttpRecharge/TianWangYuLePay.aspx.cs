using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    // 支付宝，微信的备用支付方式。
    public partial class TianWangYuLePay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackTianWangYuLe obj = new PayCallbackTianWangYuLe();
            obj.Secret = "87rjjW1ngHoxo6BKpr9bV58hpqIRvG31";
            obj.PayTableName = "tianwangyule_pay";
            string str = obj.notifyPay(Request);
            Response.ContentType = "text/plain";
            Response.Write(str);
        }
    }
}