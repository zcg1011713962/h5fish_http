using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Text;

namespace SearchAccount
{
    // 发送验证码
    public partial class check_code : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            string result = "";
            string phone = Request.QueryString["phone"];
            if (phone == null)
            {
                result = "phoneError";
                Response.Write(result);
                CLOG.Info(result);
                return;
            }
            string code = Request.QueryString["code"];
            if (code == null)
            {
                result = "codeError";
                Response.Write(result);
                CLOG.Info(result);
                return;
            }
            string type = Request.QueryString["type"];
            if (type == null)
            {
                result = "typeError";
                Response.Write(result);
                CLOG.Info(result);
                return;
            }

            MsgSendBase info = MsgSendBase.createMsg();
            int iType = Convert.ToInt32(type);
            string str = info.setUpMsgInfoCheckInfo(phone, code, iType);
            string retstr = info.send(str, iType);

            retstr = info.adapterRetValue(retstr);
            Response.Write(retstr);
        }
    }
}
