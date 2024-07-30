using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatSummerPlan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_SUMMER_PLAN, Session, Response);

            if (!IsPostBack)
            {
                m_optional.Items.Add(new ListItem("计划激活", "1"));
                m_optional.Items.Add(new ListItem("夏日计划", "2"));
                m_optional.Items.Add(new ListItem("蛟龙腾飞", "3"));
                m_optional.Items.Add(new ListItem("海王送宝", "4"));
                m_optional.Items.Add(new ListItem("点石成金", "5"));
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = Convert.ToInt32(m_optional.SelectedValue);
            param.m_time = m_time.Text;

            int type = Convert.ToInt32(m_optional.SelectedValue);
            TableSummerPlanBase plan = TableSummerPlanBase.create(type);
            if (plan != null)
            {
                plan.genTable(user, m_result, param);
            }
        }
    }
}