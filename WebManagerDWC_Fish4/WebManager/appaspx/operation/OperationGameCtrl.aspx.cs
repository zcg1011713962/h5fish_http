using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationGameCtrl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_OPERATION_GAME_CTRL, Session, Response);
        }

        protected void onConfirm(object sender, EventArgs e)
        {
            ParamQuery param = new ParamQuery();
            param.m_channelNo = m_channel.Text;
            param.m_param = m_version.Text;
            param.m_op = 0; //添加
            GMUser user = (GMUser)Session["user"];
            OpRes res = user.doDyop(param, DyOpType.opTypeOPerationGameCtrl);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }
    }
}