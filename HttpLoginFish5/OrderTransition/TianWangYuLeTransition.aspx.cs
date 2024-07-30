using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.OrderTransition
{
    public partial class TianWangYuLeTransition : System.Web.UI.Page
    {
        static UrlInfo m_url = new UrlInfo(false);

        protected void Page_Load(object sender, EventArgs e)
        {
            TianWangYuLe obj = new TianWangYuLe();
            obj.Merchantid = "YIR1EPGGHT";
            obj.Subject = Request.QueryString["subject"];
            obj.OrderId = Request.QueryString["orderId"];
            obj.Amount = Request.QueryString["amount"];
            obj.NotifyURL = m_url.getNotifyURL();
            obj.TypePay = Request.QueryString["payType"]; // ali, wx
            obj.Secret = "87rjjW1ngHoxo6BKpr9bV58hpqIRvG31";
            string str = obj.createOrder();
            if(str == "ok")
            {
                if (obj.m_ret.type == "URL")
                {
                    Response.Redirect(obj.m_ret.data);
                }
                else
                {
                    Response.Write(obj.m_ret.data);
                }
            }
            else
            {
                Response.Write(str);
            }
        }
    }

    class UrlInfo
    {
        protected string m_url;

        public UrlInfo(bool isTest)
        {
            if(isTest)
            {
                m_url = "http://124.78.175.121:12140/TianWangYuLePay.aspx";
            }
            else
            {
                m_url = "http://123.207.170.249:26013/TianWangYuLePay.aspx";
            }
        }

        public string getNotifyURL()
        {
            return m_url;
        }
    }
}