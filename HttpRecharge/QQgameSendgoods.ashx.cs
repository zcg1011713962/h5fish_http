using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpRecharge
{
    /// <summary>
    /// QQgameSendgoods 的摘要说明
    /// </summary>
    public class QQgameSendgoods : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            /**
            *  回调发货url（该url在接入申请时填写）
            * https://wiki.open.qq.com/wiki/%E5%9B%9E%E8%B0%83%E5%8F%91%E8%B4%A7URL%E7%9A%84%E5%8D%8F%E8%AE%AE%E8%AF%B4%E6%98%8E_V3
            * (回调发货接口需要对支付服务器发起的发货回调进行防重处理，避免因为触发补发发货逻辑导致业务重复发货。)
            * 
            * 1.接收腾讯支付后台发送过来的请求
            * 2.解析参数
            * 3.验签 sig
            * 4. 返回
            *              错误：参数错误      
            *              正确：
            *                          检查是否已经有这个订单号
            *                          检查订单一致性，有平台会充错账号，允许该订单再次充值   发货并返回发货结果
            *                          通知发货结果，调用接口 v3/pay/confirm_delivery 
            */

            SendgoodsQQgame obj = new SendgoodsQQgame();
            string str = obj.sendGoods(context.Request);

            context.Response.ContentType = "text/plain";
            context.Response.Write(str);
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