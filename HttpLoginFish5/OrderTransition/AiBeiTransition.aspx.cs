using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.OrderTransition
{
    // 爱贝网页端支付下单(来电)
    public partial class AiBeiTransition : System.Web.UI.Page
    {
        const string PRIVATE_KEY = "<RSAKeyValue><Modulus>5pSia0rU4apC+yIvlsWptOsP75HfTEe6mypQj/o48WENZjSE6G5H3qwBCicZ5PrzKxxo+g0nXU1C7BW8t1oO2tJ2bRLxW0Zo0Vj1U9eh5WzZiIsQL70y+urY9COph8yWsMdPnLexVbsV0RYNsMfVcr8XLx5KU/XMkh0JmmqNEF0=</Modulus><Exponent>AQAB</Exponent><P>+gEIQqMKtC/2ec4AEXC9YRPbJ0wCtmhxQ9LJ99gajyaE4owvzQ/AUkHJrUT1zPmfC/hpHyO4NfKetBivm0tIKQ==</P><Q>7BxYxTjwUjXNJYtZ11XY4RoEfRyQD+PQNDAIqnxMAHsuPKbBHkihmQCp+huPY26MgFkERbs+QHpregz3YQedFQ==</Q><DP>2/Kwi7/ZmPFhjYQZNz7SvmeztX0AHe8BR0RaAD0WEL84xf/DxkuHTlcm0dQL1MwAi41/HrBUTtInohbd2GiBEQ==</DP><DQ>OAwekmqKuakl0oS6xeAOBJlNeXl/RRZaBRll5TpuPCsBdcpLy2mIWq6KquFB72N9nLYEypzBEUM+IhHT40eQZQ==</DQ><InverseQ>5Yg6OmhFy74fRlcjx2IypY/WKkDvE56TbUvMdQtaZQgEy9R8z9YTvSFozeIdAhSQ3Py4hc5S2iwK96zQIKPI4g==</InverseQ><D>RE0g83E+L/50StmsiRfSFmJO3SMzpu/UaeQV6yAuv+mEw5KpEASiy2XeBPjiJb7kSw/mVLOSxoN9YFtsBE/r9/vCwNuDoHg1r1ocxuMnGYNFSAnS+p70Z1iJ73DVG7xnwb+LhqRIcShNru5641++ogl7JNBc6lwxmx17bn9nQaE=</D></RSAKeyValue>";

        const string APP_ID = "3011545330";

        //const string CALL_BACK_SUCCESS = "http://116.226.49.114:12140/aibei_pay_success.aspx";

        //const string CALL_BACK_FAIL = "http://116.226.49.114:12140/aibei_pay_abandon.aspx";

        const string CALL_BACK_SUCCESS = "http://123.206.84.230:26013/aibei_pay_success.aspx";

        const string CALL_BACK_FAIL = "http://123.206.84.230:26013/aibei_pay_abandon.aspx";

        static int[] s_srcPayCode = new int[] { 9, 2, 3, 4, 5, 6, 7, 8, 26, 10, 1 };
        static int[] s_dstPayCode = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int payId = Convert.ToInt32(Request.QueryString["payId"]);
                payId = getDstPayCode(payId);
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

        int getDstPayCode(int srcPayCode)
        {
            int index = -1;
            for (int i = 0; i < s_srcPayCode.Length; i++)
            {
                if (s_srcPayCode[i] == srcPayCode)
                {
                    index = i;
                    break;
                }
            }

            int result = -1;
            if (index > -1)
            {
                result = s_dstPayCode[index];
            }
            return result;
        }
    }
}