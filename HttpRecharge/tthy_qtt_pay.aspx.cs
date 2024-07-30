using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    /*
        应用名称：天天口袋捕鱼趣头条z。 暂时不用。        
        应用appid：1725CC9C8B283BCC2
     */
    public partial class tthy_qtt_pay : System.Web.UI.Page
    {
        const string APP_KEY = "LSFDLGLLLLIIJJJJIEEEERTIWE7638EGI";

        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackTianTianhy pay = new PayCallbackTianTianhy();
            pay.AppKey = APP_KEY;
            pay.PayTableName = PayTable.TTHY_QTT_PAY;
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}