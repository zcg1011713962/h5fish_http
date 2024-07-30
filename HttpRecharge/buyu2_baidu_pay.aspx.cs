using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class buyu2_baidu_pay : System.Web.UI.Page
    {
        public const string BAIDU2_SECRET_KEY = "5hT1aBW6g1knmhgSkp7d2PzvkS2qhMpG";

        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackBaidu2 pay = new PayCallbackBaidu2();
            pay.SecretKey = BAIDU2_SECRET_KEY;
            pay.PayTableName = PayTable.BUYU2_BAIDU_PAY;
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}