using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.service
{
    public partial class ServiceBlockId : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.SVR_BLOCK_PLAYER_ID, Session, Response);
        }

        protected void onBlockPlayerId(object sender, EventArgs e)  //停封玩家ID
        {
            ParamBlock p = new ParamBlock();
            p.kickPlayer = OpRes.op_res_failed;//初始化
            p.m_isBlock = true;
            p.m_param = m_playerId.Text;
            p.m_comment = m_comment.Text;
            GMUser user = (GMUser)Session["user"];
            DyOpMgr mgr = user.getSys<DyOpMgr>(SysType.sysTypeDyOp);
            OpRes res = mgr.doDyop(p, DyOpType.opTypeBlockId, user);
            m_res.InnerHtml = "封停玩家ID" + OpResMgr.getInstance().getResultString(res) + "，踢出玩家" + OpResMgr.getInstance().getResultString(p.kickPlayer);
        }
    }
}
