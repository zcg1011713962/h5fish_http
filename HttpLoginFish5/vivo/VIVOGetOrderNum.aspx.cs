using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using Common;
using System.Text;

namespace HttpLogin.vivo
{
    // 街机
    public partial class VIVOGetOrderNum : System.Web.UI.Page
    {
        // vivo的订单推送接口
        const string URL_GET_ORDER = "https://pay.vivo.com.cn/vivoPay/getVivoOrderNum?{0}";
        // vivo回调地址  测试
        //const string PAY_NOTIFY_URL = "http://180.154.223.101:12140/vivo_pay.aspx";

        // 正式回调地址
        const string PAY_NOTIFY_URL = "http://123.206.84.230:26013/vivo_pay.aspx";

        static List<string> s_excludeKey = new List<string>(new string[] { "signMethod", "signature" });

        protected void Page_Load(object sender, EventArgs e)
        {
            CVIVOGetOrderNum obj = CVIVOGetOrderNum.createVivoOrder(Request); //new CVIVOGetOrderNum();
            obj.PayNotifyURL = PAY_NOTIFY_URL;
            obj.VivoCpId = PayTable.VIVO_CP_ID;
            obj.VivoAppId = PayTable.VIVO_APP_ID;
            obj.VivoCpKey = PayTable.VIVO_CP_KEY;

            obj.SelfOrderId = Request.QueryString["selfOrderId"];
            obj.Price = Request.QueryString["price"];
            obj.ProductTitle = Request.QueryString["title"];
            obj.ProductDesc = Request.QueryString["desc"];

            string ret = obj.getOrderNum();
            Response.Write(ret);

            return;




            string selfOrderId = Request.QueryString["selfOrderId"];
            string price = Request.QueryString["price"];
            string productTitle = Request.QueryString["title"];
            string productDesc = Request.QueryString["desc"];

            if (string.IsNullOrEmpty(selfOrderId) ||
               string.IsNullOrEmpty(price) ||
               string.IsNullOrEmpty(productTitle) ||
               string.IsNullOrEmpty(productDesc))
            {
                VivoOrderNumInfo info = new VivoOrderNumInfo();
                info.respCode = 400;
                retMsg(info);
                return;
            }

            try
            {
                string param = genSendParam(selfOrderId, price, productTitle, productDesc);

                string url = string.Format(URL_GET_ORDER, param);
                byte[] retStr = HttpPost.Post(new Uri(url));
                string str = Encoding.UTF8.GetString(retStr);

                VivoOrderNumInfo info = JsonHelper.ParseFromStr<VivoOrderNumInfo>(str);
                retMsg(info);
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
                VivoOrderNumInfo info = new VivoOrderNumInfo();
                info.respCode = 501;
                retMsg(info);
            }
        }

        string genSendParam(string selfOrderId, string price, string productTitle, string productDesc)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("version", "1.0.0");
            dict.Add("signMethod", "MD5");

            dict.Add("storeId", PayTable.VIVO_CP_ID);
            dict.Add("appId", PayTable.VIVO_APP_ID);
            dict.Add("storeOrder", selfOrderId);
            dict.Add("notifyUrl", PAY_NOTIFY_URL);
            dict.Add("orderTime", DateTime.Now.ToString("yyyyMMddHHmmss"));
            dict.Add("orderAmount", price);
            dict.Add("orderTitle", productTitle);
            dict.Add("orderDesc", productDesc);

            PayCheck check = new PayCheck();
            string waitSign = check.getVivoWaitSigned(dict, s_excludeKey, PayTable.VIVO_CP_KEY);
            string sign = Helper.getMD5(waitSign);
            dict.Add("signature", sign);

            dict["notifyUrl"] = HttpUtility.UrlEncode(PAY_NOTIFY_URL);
            string result = check.getWaitSignStrByAsc(dict);

            return result;
        }

        void retMsg(VivoOrderNumInfo info)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ret = {};");
            sb.AppendFormat("ret.result = {0};", info.respCode);
            sb.AppendFormat("ret.vivoOrder = \"{0}\";", info.vivoOrder);
            sb.AppendFormat("ret.vivoSignature = \"{0}\";", info.vivoSignature);
            sb.Append("return ret;");

            Response.Write(sb.ToString());
        }

        class VivoOrderNumInfo
        {
            public int respCode = 0;
            public string respMsg = "";
            public string signMethod = "";
            public string signature = "";
            public string vivoSignature = "";
            public string vivoOrder = "";
            public string orderAmount = "";
        }

    }
}