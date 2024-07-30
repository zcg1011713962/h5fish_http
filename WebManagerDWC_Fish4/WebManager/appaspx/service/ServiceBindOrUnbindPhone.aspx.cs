using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.service
{
    public partial class ServiceBindOrUnbindPhone : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.SVR_BIND_UNBIND_PHONE, Session, Response);
            if(!IsPostBack)
            {
                m_type.Items.Add("绑定手机");
                m_type.Items.Add("解绑手机");
            }
        }

        protected void onClick(object sender, EventArgs e)
        {
            //GMUser user = (GMUser)Session["user"];
            //ParamQuery param = new ParamQuery();
            //param.m_playerId = m_playerId.Text;
            //param.m_param = m_phone.Text;  //号码
            //param.m_op = m_type.SelectedIndex;

            //DyOpMgr mgr = user.getSys<DyOpMgr>(SysType.sysTypeDyOp);
            //OpRes res = mgr.doDyop(param, DyOpType.opTypeBindUnbindPhone, user);

            //m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }

        protected void onVertify(object sender, EventArgs e)
        {

        }

        protected void btn_query_Click(object sender, EventArgs e)
        {

        }
    }
}