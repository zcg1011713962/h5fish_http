using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.vivo
{
    // 名称：电玩捕鱼大富豪
    public partial class VIVOGetOrderNum3 : System.Web.UI.Page
    {
        // vivo回调地址  测试
        //const string PAY_NOTIFY_URL = "http://180.154.223.101:12140/vivo3_pay.aspx";

        // 正式回调地址
        const string PAY_NOTIFY_URL = "http://123.206.84.230:26013/vivo3_pay.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            CVIVOGetOrderNum obj = CVIVOGetOrderNum.createVivoOrder(Request);//new CVIVOGetOrderNum();
            obj.PayNotifyURL = PAY_NOTIFY_URL;
            obj.VivoCpId = "1b572d7781ab0ee5ee5d";
            obj.VivoAppId = "19c85d7e41e0b4ea1956645ee54204ad";
            obj.VivoCpKey = PayTable.VIVO_CP_KEY3;

            obj.SelfOrderId = Request.QueryString["selfOrderId"];
            obj.Price = Request.QueryString["price"];
            obj.ProductTitle = Request.QueryString["title"];
            obj.ProductDesc = Request.QueryString["desc"];

            string ret = obj.getOrderNum();
            Response.Write(ret);
        }
    }
}