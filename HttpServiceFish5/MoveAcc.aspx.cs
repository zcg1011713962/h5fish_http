using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AccountCheck
{
    // 账号落地。对于已绑定手机的老账号，登录后，若发现这个账号尚未落地，则从渠道账号表移一个账号到 账号总表内(AccountTable)
    public partial class MoveAcc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AccLandContext context = new AccLandContext();
                context.CreateTime = Convert.ToInt64(Request.QueryString["createTime"]);
                context.MobilePhone = Request.QueryString["bindPhone"];
                context.Acc = Request.QueryString["acc"];
                context.Platform = Request.QueryString["platform"];

                MoveAccFromThirdParty move = new MoveAccFromThirdParty();
                move.startMove(context);
            }
            catch (System.Exception ex)
            {
                CLOG.Info("MoveAcc.aspx, {0}", ex.ToString());
            }
        }
    }
}