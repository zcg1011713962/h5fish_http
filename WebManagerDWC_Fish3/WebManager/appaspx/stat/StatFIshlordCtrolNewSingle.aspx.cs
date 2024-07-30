using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFIshlordCtrolNewSingle : System.Web.UI.Page
    {
        TableStatFishlordNewSingleCtrl m_common = new TableStatFishlordNewSingleCtrl();
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_CONTROL_NEW_SINGLE, Session, Response);
            if (!IsPostBack)
            {
                GMUser user = (GMUser)Session["user"];
                m_common.genExpRateTable(m_expRateTable, user, QueryType.queryTypeFishlordNewSingleParam);
            }
        }
    }
}