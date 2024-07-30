using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.OrderTransition
{
    // 微信小程序支付
    public partial class WxMiniRechargeTransition : System.Web.UI.Page
    {
        static string[] s_MidasSecret = { "8eIwfJkZd6QX2MZRyhoyKZXAZ8N0aeI6", "kzxsY3PymbVZseXjnkAFanzqcYnJw087" };

        protected void Page_Load(object sender, EventArgs e)
        {
            WeiXinMiniMidasRecharge obj = new WeiXinMiniMidasRecharge();

            obj.IsTest = 0;
            obj.OpenId = Request.QueryString["openid"];
            obj.AppId = "wx0d6cc55899d43076";
            obj.OfferId = "1450018385";
            obj.SessionKey = Request.QueryString["sessionKey"];
            obj.SessionKey = obj.SessionKey.Replace(' ', '+');   // 手动将空格转成 + 号
            obj.AppSecret = "63867210a5f2e25133cae0d4385ecd17";
            obj.MidasSecret = s_MidasSecret[obj.IsTest];
            obj.Rmb = Request.QueryString["rmb"];
            obj.SelfOrderId = Request.QueryString["selfOrderId"];
            obj.PayTableName = "wxmini_pay";
            string ret = obj.doRecharge();
            Response.Write(ret);

            /*WeiXinMiniGetId obj = new WeiXinMiniGetId();
            obj.PayNotifyURL = PAY_NOTIFY_URL;
            obj.AppId = "wx0d6cc55899d43076";
            obj.MchId = "1469497102";
            obj.Body = Request.QueryString["body"];
            obj.SelfOrderId = Request.QueryString["selfOrderId"];
            obj.TotalFee = Request.QueryString["totalFee"];
            obj.ApiSecret = "63867210a5f2e25133cae0d4385ecd17";
            string retstr = obj.trans();
            Response.Write(retstr);*/
        }
    }
}