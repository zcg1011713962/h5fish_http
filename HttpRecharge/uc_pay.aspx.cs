using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class uc_pay : System.Web.UI.Page
    {
        const string API_KEY = "8afbce941aae60c61f97a0c0dc0b6ad4";

        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackUC pay = new PayCallbackUC();
            pay.ApiKey = API_KEY;            
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}