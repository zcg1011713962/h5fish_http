﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordControlNew : RefreshPageBase
    {
        private string m_roomList = "";

        TableStatFishlordNewCtrl m_common = new TableStatFishlordNewCtrl();
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_PARAM_CONTROL, Session, Response);

            if (!IsPostBack)
            {
                GMUser user = (GMUser)Session["user"];
                m_common.genExpRateTable(m_expRateTable, user, QueryType.queryTypeFishlordNewParam);
            }
            else 
            {
                m_roomList = Request["roomList"];
                if (m_roomList == null)
                    m_roomList = "";
            }
        }

        protected void onReset(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            if (IsRefreshed)
            {
                m_common.genExpRateTable(m_expRateTable, user, QueryType.queryTypeFishlordNewParam);
                return;
            }

            OpRes res = m_common.onReset(user, m_roomList, DyOpType.opTypeFishlordParamAdjustNew);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
            m_common.genExpRateTable(m_expRateTable, user, QueryType.queryTypeFishlordNewParam);
        }
    }
}