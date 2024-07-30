using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace HttpLogin.OrderTransition
{
    public partial class WeiXinLoginTransition : System.Web.UI.Page
    {
        public const string APP_KEY = "e0c4b0ede0f4c9cb062b6ab1643021bd";
        public const string FMT = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";

        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            do
            {
                string code = Request.QueryString["code"];
                if (string.IsNullOrEmpty(code))
                {
                    ret["_result"] = "error";
                    break;
                }

                try
                {
                    string url = string.Format(FMT, PayTable.WEIXIN_APPID, APP_KEY, code);
                    byte[] retArr = HttpPost.Get(new Uri(url));

                    string retStr = Encoding.UTF8.GetString(retArr);
                    ret = JsonHelper.ParseFromStr<Dictionary<string, object>>(retStr);

                    if (ret.ContainsKey("errcode"))
                    {
                        ret["_result"] = "error";
                        break;
                    }

                    ret["_result"] = "ok";
                }
                catch (System.Exception ex)
                {
                    CLOG.Info("WeiXinLoginTransition:" + ex.ToString());
                }
            } while (false);

            Response.Write(LoginCommon.genLuaRetString(ret));
        }
    }
}