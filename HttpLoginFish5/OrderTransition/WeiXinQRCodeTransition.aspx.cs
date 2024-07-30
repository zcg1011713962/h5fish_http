using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace HttpLogin.OrderTransition
{
    // 微信二维码支付
    public partial class WeiXinQRCodeTransition : System.Web.UI.Page
    {
        // 测试回调
        const string PAY_NOTIFY_URL_TEST = "http://124.78.172.46:12140/WeixinQRPay.aspx";

        // 正式回调
        const string PAY_NOTIFY_URL = "http://123.207.170.249:26013/WeixinQRPay.aspx";

        public bool IS_TEST = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            WeiXinQRCode obj = new WeiXinQRCode();
            obj.PayNotifyURL = IS_TEST ? PAY_NOTIFY_URL_TEST : PAY_NOTIFY_URL;
            obj.AppId = "wx51166ba3a645ce3b";
            obj.ApiSecret = "93dSEaf154548w7efaADF4541sd4aefa";
            obj.MchId = "1595520431";
            obj.ClientIp = Common.Helper.GetWebClientIp();

            string str = obj.setUpParam(Request);
            if (obj.OpcodeResult != 0) // 获取参数不成功，直接返回错误
            {
                Response.ContentType = "text/plain";
                Response.Write(str);
                return;
            }

            string codeUrl = obj.getCodeUrl();
            MemoryStream ms = obj.genQR(codeUrl);
            Response.ContentType = "image/png";
            Response.BinaryWrite(ms.GetBuffer());
            Response.End();
        }
    }
}