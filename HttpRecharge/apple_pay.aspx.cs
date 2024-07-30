using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    // 苹果支付成功后，由客户端调用，进行验证票据处理
    public partial class apple_pay : System.Web.UI.Page
    {
        // 测试环境
        const string TEST_URL = "https://sandbox.itunes.apple.com/verifyReceipt";

        // 生产环境
        const string PRODUCT_URL = "https://buy.itunes.apple.com/verifyReceipt";

        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbacApple pay = new PayCallbacApple();
            pay.TestVerifyURL = TEST_URL;
            pay.ProductVerifyURL = PRODUCT_URL;
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}