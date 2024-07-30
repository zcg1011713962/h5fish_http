using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatJinQiuNationalDayActCtrl : System.Web.UI.Page
    {
        TableStatJinQiuNationalDayActCtrl m_common = new TableStatJinQiuNationalDayActCtrl();
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_JINQIU_NATIONAL_ACT_CTRL, Session, Response);
            m_res.InnerHtml = "";
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_param = m_playerId.Text;
            m_common.genExpRateTable(m_result, param, user, QueryType.queryTypeJinQiuNationalDayActCtrl);
        }
    }
}