using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class jinli_pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackBase pay = new PayCallbackAmigo();
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}