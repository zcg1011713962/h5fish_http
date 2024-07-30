using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.OrderTransition
{
    public partial class letv_transition : System.Web.UI.Page
    {
        const string PAY_NOTIFY_URL = "http://123.206.84.230:26013/letv_pay.aspx";
        //const string PAY_NOTIFY_URL = "http://101.81.252.216:12140/letv_pay.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(PAY_NOTIFY_URL);
        }
    }
}