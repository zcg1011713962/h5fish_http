using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;

namespace HttpLogin.OrderTransition
{
    // 捕鱼3爱贝
    public partial class AiBeiTransition2 : System.Web.UI.Page
    {
        const string PRIVATE_KEY = "<RSAKeyValue><Modulus>0cY5s+0kHBRBqcO/8Igm0nTb5CGjEwZiosJsf0gXsgReHx1MjpFB00ORpBq0ILHrD0v7FBCTFe4+7PBHCMT1fD4ark79+psH4nBLUCrX1Wo9Jw23sAPgFSQXKQqmybXGhDK9rm2GRH7jkGPcOBH0ls1JBQzji5IQlMlHp/maqT0=</Modulus><Exponent>AQAB</Exponent><P>8p71XBDfQIF7Hfx+VqjfXNe04ErHwJk7HBeuo6eX74vIfT1WxdLKjl8NYxy3xsa5r3pk0bc1EtBWf8AVBfuNww==</P><Q>3VeTlIzcht7X7cRDR111tjA7p0fbZS9jVJRexlI0jv1hCsSqgZGpvTRiDIrLboDtpKYF2kDNKYxAp+i0+0p8/w==</Q><DP>v9cI0x9UJ8E6BF/d1c/5rYOyNZdrktKWdoQTRrwB2xuAD+cscYdXOnPWNgRDHB2OPT5d8aUXhiTOAH11IiHh2Q==</DP><DQ>JDqcblksE2tQPpu8Q2cZyEtWyEersoEyKfrrFF0KO0lf4+pS2khkVxLG5bSmHZ0+yI9gL9wheKZ7QsCFNwh3Mw==</DQ><InverseQ>KdKNSaVKgTaNTul8jr1rMINVxaoNFXDo1kk1LgW600xhKjhO0bFnNcNSK4FPwOivYqzHY5wley4VhJycr+mIUA==</InverseQ><D>JW5HcZGI9fGvXcluAE9rDfaIHgbagFSNWRl0HqoQgFVDLi4eMPo+UqIl5LBXH9ZfaRVXsdbbR/iBBepB4pCydKweiwRyDAPcTDvWdWUvJMVq21XIWqluamHNZNqgw3YN3pXEczrwoaPmUlsA5JzTxs9JrXJJ1PGT8lfabyUWhEk=</D></RSAKeyValue>";

        const string APP_ID = "3008961589";

        //const string CALL_BACK_SUCCESS = "http://116.231.67.250:12140/aibei_pay_success2.aspx";

        //const string CALL_BACK_FAIL = "http://116.231.67.250:12140/aibei_pay_abandon.aspx";

        const string CALL_BACK_SUCCESS = "http://123.206.84.230:26013/aibei_pay_success2.aspx";

        const string CALL_BACK_FAIL = "http://123.206.84.230:26013/aibei_pay_abandon.aspx";

        static RechargeEx s_trans;

        static AiBeiTransition2()
        {
            s_trans = new RechargeEx();
            s_trans.init();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int payId = Convert.ToInt32(Request.QueryString["payId"]);
                payId = s_trans.getPayCode(payId);
                AiBeiTransParam param = new AiBeiTransParam();
                param.m_playerId = Convert.ToInt32(Request.QueryString["playerId"]);
                param.m_payId = payId;
                param.m_productName = Request.QueryString["productName"];
                param.m_orderId = Request.QueryString["orderId"];
                param.m_price = (float)Convert.ToDouble(Request.QueryString["price"]);

                AiBeiTrans obj = new AiBeiTrans();
                obj.PrivateKey = PRIVATE_KEY;
                obj.NotifyURL = CALL_BACK_SUCCESS;
                obj.NotifyFailURL = CALL_BACK_FAIL;
                obj.AppId = APP_ID;
                string url = obj.transRL(param);
                Response.Redirect(url);
            }
            catch (System.Exception ex)
            {
                CLOG.Info("aibei:" + ex.ToString());
            }
        }
    }

    public class RechargeEx
    {
        private Dictionary<int, int> m_dic = new Dictionary<int, int>();

        public int getPayCode(int rechargeId)
        {
            if (m_dic.ContainsKey(rechargeId))
            {
                return m_dic[rechargeId];
            }
            return -1;
        }

        public void init()
        {
            string path = HttpRuntime.BinDirectory + "..\\download";
            string file = Path.Combine(path, "M_RechangeExCFG.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            XmlNode node = doc.SelectSingleNode("/Root");

            for (node = node.FirstChild; node != null; node = node.NextSibling)
            {
                string sid = node.Attributes["ID"].Value;
                if (sid == "800094")
                {
                    string ids = node.Attributes["RechargeIDs"].Value;
                    string paycodes = node.Attributes["PayCodes"].Value;

                    string[] aids = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] apaycodes = paycodes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < aids.Length; i++)
                    {
                        int key = Convert.ToInt32(aids[i]);
                        if (!m_dic.ContainsKey(key))
                        {
                            m_dic.Add(key, Convert.ToInt32(apaycodes[i]));
                        }
                    }
                    break;
                }
            }
        }
    }
}