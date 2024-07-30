using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat.shuihz
{
    public partial class ShuihzPlayerScorePool : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.SHUIHZ_PLAYER_SCORE_POOL, Session, Response);
        }

        protected void onBuffScorePool(object sender, EventArgs e)
        {
            ParamQuery param = new ParamQuery();
            param.m_playerId = m_playerId.Text;
            param.m_param = m_addPoolVal.Text;
            param.m_op = 0; //添加
            GMUser user = (GMUser)Session["user"];
            OpRes res = user.doDyop(param, DyOpType.opTypeShuihzPlayerScorePool);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }
    }
}