using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.OrderTransition
{
    // 爱贝，魅族切支付。fish3不需要正式发布
    public partial class AiBeiTransitionSwitch : System.Web.UI.Page
    {
        const string PRIVATE_KEY = "<RSAKeyValue><Modulus>tNOLK7ZYkOnG98/j3ppG0mcXOuWgHeMViPbzboQbGEIwGNkOQpql2tjm+TNKwyo+fIGoKxrfUZHqMgm2X8lFRKYT2CZoOqXFcndqJK6WUA1TfNH8E6ZBWiduaFVAacKLPNXS2YJFrWMavGwkdLX0OTo0h2sKJZ2HczZCVWYoF/E=</Modulus><Exponent>AQAB</Exponent><P>7NHaNugIvbdTnRP/VTA2jgLNYzBq8l6s6RknTT99Vt5vJRlDGtNDddB8bPioub7WC3MTu02fYSfPRd/wNUXNMw==</P><Q>w3i9zh0SooSciMKmiTaZTy7cUleFn2fkwFzyZJgYawQUYJfAxdjrRA18hybl/qadNpbJXQFYSma8GjYeVrMeSw==</Q><DP>N0EjU+pBl9o9VQoEaiDsqae3uivi2BgE6gInbFui5/DQJ+zD/m9KbyOs1FQUMfp3wIYiFXKf/DAoqVn9lsBphw==</DP><DQ>Y2PCRsobjo0VNqiamwwq+cse9bNQ2xOtiW35RdLcH5XscozW1QKN5YVh+yp5KXk4WOhkrKihZvtDy6QW2wiqbw==</DQ><InverseQ>GKuQ/92B6+D36fs3KoDAHpamyawzr2YWQyaqFr8CbUBAXGPGj/EfZaT7yJLBnowOU8uCVMf0SM6iBVJL4RZohA==</InverseQ><D>FpNKjryHNiBZnNqxgjsUmHQSbGH6qYCOUbkzDxsjHtY0lwWSSDJyfm4R+A61SMRfZL/zNN53/wEOkgywXikMop148K66Z08RKdnxZiWrqH9Z2bd1sGOAKMPBWmAAWGd0qR4d+0Pfm/aZBXZYdjY/uuuOuBD4GXZaMueEQ5w9w3k=</D></RSAKeyValue>";

        const string APP_ID = "3003751412";

        const string CALL_BACK_SUCCESS = "http://101.81.29.15:12140/aibei_pay_success_switch.aspx";

        const string CALL_BACK_FAIL = "http://101.81.29.15:12140/aibei_pay_abandon.aspx";

       // const string CALL_BACK_SUCCESS = "http://123.206.84.230:26013/aibei_pay_success_switch.aspx";

       // const string CALL_BACK_FAIL = "http://123.206.84.230:26013/aibei_pay_abandon.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // aibei sdk不需要转换对应关系，客户端传过来的是已经转换过的payid
                int payId = Convert.ToInt32(Request.QueryString["payId"]);
                AiBeiTransParam param = new AiBeiTransParam();
                param.m_playerId = Convert.ToInt32(Request.QueryString["playerId"]);
                param.m_payId = payId;
                param.m_productName = Request.QueryString["productName"];
                param.m_orderId = Request.QueryString["orderId"];
                param.m_price = (float)Convert.ToDouble(Request.QueryString["price"]);

                AiBeiTransSwitch obj = new AiBeiTransSwitch();
                obj.PrivateKey = PRIVATE_KEY;
                obj.NotifyURL = CALL_BACK_SUCCESS;
                obj.NotifyFailURL = CALL_BACK_FAIL;
                obj.AppId = APP_ID;
                string retstr = obj.transRL(param);
                Response.Write(retstr);
            }
            catch (System.Exception ex)
            {
                CLOG.Info("AiBeiTransitionSwitch:" + ex.ToString());
            }
        }
    }
}