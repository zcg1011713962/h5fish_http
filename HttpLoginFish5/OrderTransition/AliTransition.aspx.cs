using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Configuration;

namespace HttpLogin.OrderTransition
{
    public partial class AliTransition : System.Web.UI.Page
    {
        // 正式
        static string PAY_NOTIFY_URL = WebConfigurationManager.AppSettings["aliPayNotifyURL"];
        
        // 测试
        //const string PAY_NOTIFY_URL = "http://101.81.252.216:12140/ali_pay.aspx";

        public const string APP_ID = "2017031006156573";

        public const string PRIVATE_KEY = "<RSAKeyValue><Modulus>oHGmdG1ooJ5bAUrAVGK5NUAfY2FDHOsiNFWuFB8AUGmeZiDHYNfTF+6k2VrqXVpV5Wn+l8VLWSWBt6E6/juPsDAbS6kmWzEgIV9ANrYbRu7eAoqxkOFujSoezB7j00SYSlrbaj4iahXKjt7ObwKl/4ciOc6cj0Psk3916n+RqJE=</Modulus><Exponent>AQAB</Exponent><P>59vYLGFw+QvvnBDF2hXkuQzT8jL23MX79ZWLrkqGibTdXuv6llf2PwBbR6oZzCyshZQ1eN0gvKEziCD9gk8xrw==</P><Q>sSZBidT/GHfrvmQNkPMgLmEM7L0qweArTqYQGHlwox4bbwPOLfc83/i1k893nQ5LbN6UsY+d77lljEfvD1aZvw==</Q><DP>eZIJQAP7k2oRwdf9lcMjAXBbdUQJsmrRGMzHx6Rl9LBz3kCHTOtkP1Z1hhcHncnSz9uNSglQD/fKKFd79SaHGw==</DP><DQ>Pd4GdYSVso3vHwcCVeUTEB+EzAkkraEEfuswI9wFonIZUqQZlaQK9o19nKmQNKGRZew2MezeU6KD/IIC03CDMQ==</DQ><InverseQ>wxxHPkUklHdAj4b1jgvgYD6nTER/u9l0jVuG5KFYA1UkVDbFylItHTbYwY3CNWTQUckb2xWlH535ydYq+EaxqA==</InverseQ><D>FoF6sPA78fPknhzHN88VXcPd40ncaS0OgjrWjVn/6Ee4gWjtrsb3hG7kTtzy7R9j1yd0IAP72shpFsIWDV17fSkSzq/eOaQYfrXjdc0tBXgXoZF/M8UxaSHDKZ/i8DOhaRwUXXVCTH54ym1bNS51Ff4kUcu9LUxlnLPxgZkjMcU=</D></RSAKeyValue>";

        protected void Page_Load(object sender, EventArgs e)
        {
            string totalAmount = Request.QueryString["totalAmount"];
            string subject = Request.QueryString["subject"];
            string selfOrderId = Request.QueryString["selfOrderId"];
            
            string retStr = "";
            do 
            {
                try
                {
                    if (string.IsNullOrEmpty(totalAmount) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(selfOrderId))
                    {
                        break;
                    }

                    retStr = genRechargeParam(totalAmount, subject, selfOrderId);
                }
                catch (System.Exception ex)
                {
                    CLOG.Info("AliTransition:" + ex.ToString());
                }
            } while (false);

            Response.Write(retStr);
        }

        string genRechargeParam(string totalAmount, string subject, string selfOrderId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("product_code", "QUICK_MSECURITY_PAY");
            param.Add("total_amount", totalAmount);
            param.Add("subject", subject);
            param.Add("out_trade_no", selfOrderId);
            param.Add("timeout_express", "30m");
            string p = JsonHelper.genJson(param);

            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("app_id", APP_ID);
            result.Add("biz_content", p);
            result.Add("charset", "utf-8");
            result.Add("format", "JSON");
            result.Add("method", "alipay.trade.app.pay");
            result.Add("notify_url", PAY_NOTIFY_URL);
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            result.Add("timestamp", time);
            result.Add("version", "1.0");
            result.Add("sign_type", "RSA");

            PayCheck chk = new PayCheck();
            string wait = chk.getWaitSignStrByAsc(result);
            string sign = chk.signByRSA(wait, PRIVATE_KEY);

            result.Add("sign", sign);

            result = encode(result);
            wait = chk.getWaitSignStrByAsc(result);
            return wait;
        }

        Dictionary<string, object> encode(Dictionary<string, object> obj)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();

            foreach (var item in obj)
            {
                ret.Add(item.Key, urlEncode(Convert.ToString(item.Value)));
            }

            return ret;
        }

        public static string urlEncode(string str)
        {
            return HttpUtility.UrlEncode(str, Encoding.UTF8);
        }
    }
}