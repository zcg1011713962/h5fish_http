using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.OrderTransition
{
    public partial class baidu2_transition : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string playerId = Request.Form["playerId"];
            string orderId = Request.Form["orderId"];
            if (string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(orderId))
            {
                Response.Write("paramError");
                return;
            }

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("orderId", orderId);

            string res = MongodbPayment.Instance.ExecuteUpdate(PayTable.BAIDU2_TRANSITION, "playerId", playerId, data);
            if (res == "")
            {
                Response.Write("ok");
            }
            else
            {
                Response.Write("dbError");
            }
        }
    }
}