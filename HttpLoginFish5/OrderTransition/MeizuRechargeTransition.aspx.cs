using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

namespace HttpLogin.OrderTransition
{
    public partial class MeizuRechargeTransition : System.Web.UI.Page
    {
        const string APP_ID = "3171490";
        static List<string> s_excludeKey = new List<string>(new string[] { "sign", "sign_type" });

        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret["_result"] = "error";

            do
            {
                try
                {
                    string totalPrice = Request.QueryString["totalPrice"];
                    string selfOrderId = Request.QueryString["selfOrderId"];
                    string productId = Request.QueryString["paycode"];
                    string productSubject = Request.QueryString["productname"];
                    string productBody = productSubject;

                    if (string.IsNullOrEmpty(totalPrice) ||
                        string.IsNullOrEmpty(selfOrderId) ||
                        string.IsNullOrEmpty(productId) ||
                        string.IsNullOrEmpty(productSubject))
                    {
                        CLOG.Info("MeizuRechargeTransition, param error, {0}, {1}, {2}, {3} ", totalPrice, selfOrderId, productId, productSubject);
                        break;
                    }

                    TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
                    long time = Convert.ToInt64(ts.TotalMilliseconds);

                    Dictionary<string, object> indata = new Dictionary<string, object>();
                    indata.Add("app_id", APP_ID);
                    indata.Add("cp_order_id", selfOrderId);
                    indata.Add("create_time", time);
                    indata.Add("pay_type", 0);
                    indata.Add("product_body", productBody);
                    indata.Add("product_id", productId);
                    indata.Add("product_subject", productSubject);
                    indata.Add("total_price", totalPrice);
                    indata.Add("user_info", "test");

                    PayCheck chk = new PayCheck();
                    string wait = chk.getMeizuSingleWaitSignStr(indata, s_excludeKey);
                    string sign = Helper.getMD5(wait);

                    ret["_result"] = "ok";
                    ret["sign"] = sign;
                    ret["createTime"] = time;
                }
                catch (System.Exception ex)
                {
                    CLOG.Info("MeizuRechargeTransition, " + ex.ToString());
                }
            } while (false);

            Response.Write(LoginCommon.genLuaRetString(ret));
        }
    }
}