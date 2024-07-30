using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.OrderTransition
{
    public partial class QQMiniRechargeTransition : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            QQMiniMidasRecharge obj = new QQMiniMidasRecharge();

            obj.OpenId = Request.QueryString["openid"];
            obj.AppId = "1110059980";
            obj.SessionKey = Request.QueryString["sessionKey"];
            obj.SessionKey = obj.SessionKey.Replace(' ', '+'); 
            obj.AppSecret = "4zxAqqzjmEuBhnAR";
            obj.Rmb = Request.QueryString["rmb"];
            obj.SelfOrderId = Request.QueryString["selfOrderId"];
            obj.GoodId = Request.QueryString["goodId"];
            string ret = obj.doRecharge();
            Response.Write(ret);
        }
    }
}