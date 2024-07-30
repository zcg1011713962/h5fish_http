using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.newAcc
{
    // 魅族(联机) sign，不作为fish3的正式发布，内网测试用
    public partial class MeizuOnlineRechargeTransition : System.Web.UI.Page
    {
        public const string APP_ID = "3135817";
        public const string APP_SECRET = "X1K2QMjaA2W6s8K70U32KkCOKdAw4Om9";

        protected void Page_Load(object sender, EventArgs e)
        {
            var obj = new MeizuOnlineTrans();
            obj.AppId = APP_ID;
            obj.AppSecret = APP_SECRET;

            string str = obj.trans(Request);
            Response.Write(str);
        }
    }
}