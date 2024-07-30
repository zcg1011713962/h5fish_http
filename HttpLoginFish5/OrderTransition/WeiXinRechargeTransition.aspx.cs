using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Text;
using System.Web.Configuration;

namespace HttpLogin.OrderTransition
{
    // 微信统一下单
    public partial class WeiXinRechargeTransition : System.Web.UI.Page
    {
        // 测试回调
        //const string PAY_NOTIFY_URL = "http://101.81.252.216:12140/weixin_pay.aspx";

        // 正式回调
        static string PAY_NOTIFY_URL = WebConfigurationManager.AppSettings["weixinPayNotifyURL"];

        // 微信统一下单API接口
        public const string WINXIN_UNIFY_API = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        // 商户号
        public const string WEIXIN_MCH_ID = "1390137702";

        public const string RAND_STR = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        protected void Page_Load(object sender, EventArgs e)
        {
            // 商品描述
            string body = Request.QueryString["body"];
            // 订单ID
            string selfOrderId = Request.QueryString["selfOrderId"];
            string totalFee = Request.QueryString["totalFee"];

            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret["_result"] = "error";
            do
            {
                if (string.IsNullOrEmpty(body) || string.IsNullOrEmpty(selfOrderId) || string.IsNullOrEmpty(totalFee))
                {
                    break;
                }

                try
                {
                    string msgXML = genTranslateXMLString(body, selfOrderId, totalFee);
                    byte[] retByte = HttpPost.PostXML(new Uri(WINXIN_UNIFY_API), msgXML);

                    XmlGen xml = new XmlGen();
                    Dictionary<string, object> retObj = xml.fromXmlString("xml", Encoding.UTF8.GetString(retByte));
                    if (Convert.ToString(retObj["return_code"]) != "SUCCESS")
                    {
                        break;
                    }

                    ret = genReSignData(retObj);
                    ret["_result"] = "ok";
                }
                catch (System.Exception ex)
                {
                    CLOG.Info("WeiXinRechargeTransition:" + ex.ToString());
                }

            } while (false);
            
            Response.Write(LoginCommon.genLuaRetString(ret));
        }

        Dictionary<string, object> genTranData(string body, string selfOrderId, string totalFee)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret.Add("appid", PayTable.WEIXIN_APPID);
            ret.Add("mch_id", WEIXIN_MCH_ID);
            StringBuilder builder = new StringBuilder();
            Random r = new Random();
            for (int i = 0; i < 16; i++)
            {
                builder.Append(RAND_STR[r.Next(RAND_STR.Length)]);
            }

            ret.Add("nonce_str", builder.ToString());//随机字符串
            ret.Add("body", body);
            ret.Add("out_trade_no", selfOrderId);
            ret.Add("total_fee", totalFee);
            ret.Add("spbill_create_ip", Helper.GetWebClientIp());
            ret.Add("notify_url", PAY_NOTIFY_URL);
            ret.Add("trade_type", "APP");
            return ret;
        }

        public string genTranslateXMLString(string body, string selfOrderId, string totalFee)
        {
            PayCheck chk = new PayCheck();
            Dictionary<string, object> transData = genTranData(body, selfOrderId, totalFee);
            string waitSign = chk.getWeiXinWaitSigned(transData, null, PayTable.WEIXIN_API_SECRET);
            string sign = Helper.getMD5Upper(waitSign);
            transData.Add("sign", sign);

            XmlGen xml = new XmlGen();
            string str = xml.genXML("xml", transData);
            return str;
        }

        Dictionary<string, object> genReSignData(Dictionary<string, object> retObj)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret.Add("appid", Convert.ToString(retObj["appid"]));
            ret.Add("partnerid", Convert.ToString(retObj["mch_id"]));
            ret.Add("prepayid", Convert.ToString(retObj["prepay_id"]));
            ret.Add("package", "Sign=WXPay");
            ret.Add("noncestr", Convert.ToString(retObj["nonce_str"]));
            TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
            ret.Add("timestamp", Convert.ToInt64(ts.TotalSeconds).ToString());

            PayCheck chk = new PayCheck();
            string waitSign = chk.getWeiXinWaitSigned(ret, null, PayTable.WEIXIN_API_SECRET);
            string sign = Helper.getMD5Upper(waitSign);
            ret.Add("sign", sign);

            return ret;
        }
    }
}