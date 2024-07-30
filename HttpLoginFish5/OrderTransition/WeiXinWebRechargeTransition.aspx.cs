using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.OrderTransition
{
    public partial class WeiXinWebRechargeTransition : System.Web.UI.Page
    {
        // 测试回调
        //const string PAY_NOTIFY_URL = "http://114.86.95.190:12140/WeixinWebPay.aspx";

        // 正式回调
        const string PAY_NOTIFY_URL = "http://123.207.170.249:26013/WeixinWebPay.aspx";

        // 微信统一下单API接口
        public const string WINXIN_UNIFY_API = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        protected void Page_Load(object sender, EventArgs e)
        {
            WeiXinWebTransition obj = new WeiXinWebTransition();
            obj.PayNotifyURL = PAY_NOTIFY_URL;
            obj.AppId = "wx51166ba3a645ce3b";
            obj.ApiSecret = "93dSEaf154548w7efaADF4541sd4aefa";// "de6e0c7f7b62c3274d321583a6381776";
            obj.MchId = "1595520431";           

            obj.SelfOrderId = Request.QueryString["orderId"];
            obj.TotalFee = Request.QueryString["amount"];
            obj.Body = Request.QueryString["productName"];

            try
            {
                Response.Write(obj.getHtml());
            }
            catch (System.Exception ex)
            {
            	
            }
        }
    }
}