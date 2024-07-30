using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.vivo
{
    public partial class VIVOGetOrderNum2 : System.Web.UI.Page
    {
        // vivo回调地址  测试
        //const string PAY_NOTIFY_URL = "http://101.81.252.216:12140/vivo2_pay.aspx";

        // 正式回调地址
        const string PAY_NOTIFY_URL = "http://123.206.84.230:26013/vivo2_pay.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            CVIVOGetOrderNum obj = new CVIVOGetOrderNum();
            obj.PayNotifyURL = PAY_NOTIFY_URL;
            obj.VivoCpId = "20151207111019198783";
            obj.VivoAppId = "963e674f4d48a7b91d85df0cebaab18b";
            obj.VivoCpKey = PayTable.VIVO_CP_KEY2;

            obj.SelfOrderId = Request.QueryString["selfOrderId"];
            obj.Price = Request.QueryString["price"];
            obj.ProductTitle = Request.QueryString["title"];
            obj.ProductDesc = Request.QueryString["desc"];

            string ret = obj.getOrderNum();
            Response.Write(ret);
        }
    }
}