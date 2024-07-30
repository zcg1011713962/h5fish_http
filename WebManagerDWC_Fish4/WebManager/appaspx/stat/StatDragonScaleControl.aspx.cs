using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatDragonScaleControl : System.Web.UI.Page
    {
        TableStatDragonScaleControl m_common = new TableStatDragonScaleControl();
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_DRAGON_SCALE_CONTROL, Session, Response);
            m_res.InnerHtml = "";
        }

        //玩家魔石数量查询
        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_param = m_playerId.Text;
            m_common.genExpRateTable(m_result, param, user, QueryType.queryTypeDragonScaleControl);
        }
    }
}