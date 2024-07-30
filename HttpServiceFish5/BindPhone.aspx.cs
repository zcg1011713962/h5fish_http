using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace AccountCheck
{
    public partial class BindPhone : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.RequestType != "GET")
                return;

            string sacc = Request.Params["acc"];
            string phone = Request.Params["phone"];

            if (string.IsNullOrEmpty(sacc) || string.IsNullOrEmpty(phone))            
                return;

            string acckey = ConfigurationManager.AppSettings["acckey_default"];
            if (string.IsNullOrEmpty(acckey))
            {
                acckey = "acc";
            }

            if (MongodbAccount.Instance.KeyExistsBykey("AccountTable", acckey, sacc))
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data["bindPhone"] = phone;
                MongodbAccount.Instance.ExecuteUpdate("AccountTable", acckey, sacc, data);
            }         
        }
    }
}