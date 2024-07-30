using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatAirdropSysPub : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_AIR_DROP_SYS_PUB, Session, Response);
        }

        protected void onPublish(object sender, EventArgs e)
        {
            ParamAirdropSysItem p = new ParamAirdropSysItem();
            p.m_op = 0;
            p.m_uuid = m_uuid.Text;
            p.m_itemId = m_itemId.Text;
            p.m_pwd = m_pwd.Text;
            GMUser user = (GMUser)Session["user"];
            DyOpMgr mgr = user.getSys<DyOpMgr>(SysType.sysTypeDyOp);
            OpRes res = mgr.doDyop(p, DyOpType.opTypeStatAirdropSys, user);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }
    }
}