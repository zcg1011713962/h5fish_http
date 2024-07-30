using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatActivityLingBao : System.Web.UI.Page
    {
        private PageChip m_gen = new PageChip(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_LINGBAO, Session, Response);

            if (!IsPostBack)
            {
                //    m_optional.Items.Add(new ListItem("灵宝抽奖统计", "1"));
                //    m_optional.Items.Add(new ListItem("宝莲兑换统计", "2"));
                if (m_gen.parse(Request))
                {
                    m_time.Text = m_gen.m_time;
                   // m_playerId.Text = m_gen.m_playerId;
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQueryGift param = new ParamQueryGift();
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            // param.m_op = Convert.ToInt32(m_optional.SelectedValue);
            param.m_time = m_time.Text;
            int type = 1;// Convert.ToInt32(m_optional.SelectedValue);
            param.m_type = type;
            TableLingBaoBase plan = TableLingBaoBase.create(type);
            if (plan != null)
            {
                plan.genTable(user, m_result, param);

                string page_html = "", foot_html = "";
                m_gen.genPage(param, @"/appaspx/stat/StatActivityLingBao.aspx", ref page_html, ref foot_html, user);
                m_page.InnerHtml = page_html;
                m_foot.InnerHtml = foot_html;
            }
        }
    }
}