using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.vivo
{
    // 名称：百万街机捕鱼
    public partial class VIVOGetOrderNumBaiWan : System.Web.UI.Page
    {
        // vivo回调地址  测试
        //const string PAY_NOTIFY_URL = "http://180.154.223.101:12140/vivo_baiwan_pay.aspx";

        // 正式回调地址
        const string PAY_NOTIFY_URL = "http://123.206.84.230:26013/vivo_baiwan_pay.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            CVIVOGetOrderNum obj = CVIVOGetOrderNum.createVivoOrder(Request); //new CVIVOGetOrderNum();
            obj.PayNotifyURL = PAY_NOTIFY_URL;
            obj.VivoCpId = "20160125113929378044";
            obj.VivoAppId = "47fb29c3f0b84c55624ac773a1163dfe";
            obj.VivoCpKey = PayTable.VIVO_CP_KEY_BAI_WAN;

            obj.SelfOrderId = Request.QueryString["selfOrderId"];
            obj.Price = Request.QueryString["price"];
            obj.ProductTitle = Request.QueryString["title"];
            obj.ProductDesc = Request.QueryString["desc"];

            string ret = obj.getOrderNum();
            Response.Write(ret);
        }
    }
}