using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpLogin.OrderTransition
{
    /// <summary>
    /// QQgameTransition 的摘要说明    
    /// 应用调用支付接口  返回获取交易token以及物品URL  以及注册订单
    /// </summary>
    public class QQgameTransition : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            QQgameRachrage obj = new QQgameRachrage();
            obj.Channel = context.Request.QueryString["100000"];
            obj.OpenId = context.Request.QueryString["openid"];
            obj.OpenKey = context.Request.QueryString["openkey"];
            obj.Pf = context.Request.QueryString["pf"];

            obj.Pfkey = context.Request.QueryString["pfkey"];

            obj.Rmb = context.Request.QueryString["rmb"];
            obj.IsBluevip = context.Request.QueryString["isBluevip"];
            obj.SelfOrderId = context.Request.QueryString["selfOrderId"];
            obj.GoodsId = context.Request.QueryString["goodsId"];
            obj.Goodsmeta = context.Request.QueryString["goodsmeta"];
            obj.Goodsurl = context.Request.QueryString["goodsurl"];
            obj.Zoneid = "1";


            obj.PayTableName = PayTable.QQGAME_PAY;
            obj.IsTest = true;

            string ret = obj.doRecharge();

            context.Response.ContentType = "text/plain";
            context.Response.Write(ret);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}