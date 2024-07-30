using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class ActivityPanicBuyingCFG : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "选择" ,"活动ID", "活动名称", "最大次数"};
        private string[] m_content = new string[s_head.Length];
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_ACTIVITY_PANIC_BUYING_CFG, Session, Response);
            GMUser user = (GMUser)Session["user"];
            OpRes res = user.doQuery("", QueryType.queryTypeActivityPanicBuyingCfg);
            genTable(m_result, res, user);
        }

        //生成查询表
        private void genTable(Table table, OpRes res, GMUser user)
        {
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
            TableCell td = null;
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }
            List<ActivityPanicBuyingItem> qresult = (List<ActivityPanicBuyingItem>)user.getQueryResult(QueryType.queryTypeActivityPanicBuyingCfg);
            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            int f = 0;
            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;
                ActivityPanicBuyingItem item = qresult[i];

                m_content[f++] = Tool.getCheckBoxHtml("activityList", item.m_activityId.ToString(), false);
                m_content[f++] = item.m_activityId.ToString();
                m_content[f++] = item.m_activityName;
                m_content[f++] = item.m_maxCount.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }
    }
}