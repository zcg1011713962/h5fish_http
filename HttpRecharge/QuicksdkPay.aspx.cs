using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class QuicksdkPay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackQuicksdk pay = new PayCallbackQuicksdk();
            pay.Md5Key= "57jfsrwsi6mfuayesgg3rqzyvp2jdp5j";
            pay.CallbackKey= "52795318485433831236655560248833";
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}