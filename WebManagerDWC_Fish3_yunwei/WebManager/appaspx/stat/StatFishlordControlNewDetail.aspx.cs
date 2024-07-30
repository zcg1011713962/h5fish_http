using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordControlNewDetail : RefreshPageBase
    {
        TableStatFishlordRoomNewCtrl m_common = new TableStatFishlordRoomNewCtrl();
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);

            int roomId = Convert.ToInt32(Request.QueryString["roomId"]);
            roomName.InnerHtml = StrName.s_roomList[roomId];

            if (!IsPostBack)
                onQuery(null, null);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            int roomId = Convert.ToInt32(Request.QueryString["roomId"]);
            param.m_time = m_time.Text;
            param.m_op = roomId;
            m_common.genExpRateTable(m_expRateTable, user, QueryType.queryTypeFishlordRoomNewParam, param);
        }
    }
}