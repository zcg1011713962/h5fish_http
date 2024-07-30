using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.Platform
{
    public partial class QuickLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            QuickSdkLogin obj = new QuickSdkLogin();
            string str = obj.doLogin(Request);
            Response.Status = "200";
            Response.Write(str);
        }
    }
}