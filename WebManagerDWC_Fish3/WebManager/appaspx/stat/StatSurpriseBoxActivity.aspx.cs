using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatSurpriseBoxActivity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_SURPRISE_BOX, Session, Response);

            if (!IsPostBack)
            {
                m_optional.Items.Add(new ListItem("惊喜盒中奖情况", "1"));
                m_optional.Items.Add(new ListItem("转盘", "2"));
                m_optional.Items.Add(new ListItem("排行榜", "3"));
                m_optional.Items.Add(new ListItem("京东卡碎片持有查询", "4"));
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = Convert.ToInt32(m_optional.SelectedValue);
            param.m_time = m_time.Text;
            param.m_playerId = m_playerId.Text;
            int type = Convert.ToInt32(m_optional.SelectedValue);
            TableSurpriseBoxBase plan = TableSurpriseBoxBase.create(type);
            if (plan != null)
            {
                plan.genTable(user, m_result, param);
            }
        }
    }
}