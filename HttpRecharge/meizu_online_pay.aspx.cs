using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class meizu_online_pay : System.Web.UI.Page
    {
        public const string APP_SECRET = "X1K2QMjaA2W6s8K70U32KkCOKdAw4Om9";

        protected void Page_Load(object sender, EventArgs e)
        {
            var pay = new PayCallbacMeizu();
            pay.AppSecret = APP_SECRET;
            pay.PayTableName = "meizu_online_pay";

            string retStr = pay.notifyPay(Request);
            Response.Write(retStr);
        }
    }
}