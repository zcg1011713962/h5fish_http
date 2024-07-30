using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class QuicksdkPay3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackQuicksdk pay = new PayCallbackQuicksdk();
            pay.Md5Key = "saoosn4h8k4veirheh5wqhepzu8mani2";
            pay.CallbackKey = "57589296832739230987212657673669";
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}