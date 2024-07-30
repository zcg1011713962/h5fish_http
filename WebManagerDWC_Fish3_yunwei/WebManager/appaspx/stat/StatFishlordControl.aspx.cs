using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordControl : RefreshPageBase
    {
        private string m_roomList = "";

        TableStatFishlordControl m_common = new TableStatFishlordControl();

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_PARAM_CONTROL, Session, Response);

            if (!IsPostBack)
            {
                //genExpRateTable(m_expRateTable);
                GMUser user = (GMUser)Session["user"];
                m_common.genExpRateTable(m_expRateTable, user, QueryType.queryTypeFishlordParam);
            }
            else
            {
                m_roomList = Request["roomList"];
                if (m_roomList == null)
                {
                    m_roomList = "";
                }
            }
        }

        protected void onModifyExpRate(object sender, EventArgs e)  //修改期望盈利率
        {
        }

        protected void onReset(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            if (IsRefreshed)
            {
                m_common.genExpRateTable(m_expRateTable, user, QueryType.queryTypeFishlordParam);
                return;
            }

            OpRes res = m_common.onReset(user, m_roomList, DyOpType.opTypeFishlordParamAdjust);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
            m_common.genExpRateTable(m_expRateTable, user, QueryType.queryTypeFishlordParam);
        }

        protected void onBoss(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            m_common.getBossTable(m_bossTable1, user, QueryType.queryTypeFishlordParam,
               m_resetMidTime.Text, m_resetHighTime.Text);

            m_common.genExpRateTable(m_expRateTable, user, QueryType.queryTypeFishlordParam);
        }
    }
}