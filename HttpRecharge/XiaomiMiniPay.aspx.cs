using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    /*这个回调没有用到*/
    public partial class xiaomi_pay : System.Web.UI.Page
    {
        const string APPSECRET = "240w8v4yXUkQ1nDpLcRSvA==";

        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbacXiaoMi pay = new PayCallbacXiaoMi();
            pay.AppSecret = APPSECRET;
            pay.PayTableName = PayTable.XIAOMI_PAY;
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}