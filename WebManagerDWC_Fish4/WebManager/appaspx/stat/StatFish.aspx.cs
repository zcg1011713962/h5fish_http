using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFish : System.Web.UI.Page
    {
        TableStatFish m_common = new TableStatFish(@"/appaspx/stat/StatFish.aspx");

        PageStatFish m_gen = new PageStatFish(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_STAT, Session, Response);
            if (!IsPostBack)
            {
                m_room.Items.Add(new ListItem("全部","0"));
                foreach(var room in StrName.s_roomList)
                {
                    m_room.Items.Add(new ListItem(room.Value,room.Key.ToString()));
                }

                if (m_gen.parse(Request))
                {
                    m_time.Text = m_gen.m_time;
                    m_room.SelectedValue = m_gen.m_roomid.ToString();
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_op = Convert.ToInt32(m_room.SelectedValue);
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            m_common.onQuery(user, m_result, param, QueryType.queryTypeFishStat, m_gen);

            m_page.InnerHtml = m_common.getPage();
            m_foot.InnerHtml = m_common.getFoot();
        }

        protected void onClearFishTable(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            m_common.onClearFishTable(user, TableName.PUMP_ALL_FISH, m_result);
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";
        }
    }
}