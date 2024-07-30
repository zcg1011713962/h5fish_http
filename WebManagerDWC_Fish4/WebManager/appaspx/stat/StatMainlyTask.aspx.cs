using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatMainlyTask : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_MAINLY_TASK, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;

            OpRes res = user.doQuery(param, QueryType.queryTypeStatMainlyTask);
            genTable(m_result, res, user);
        }

        private void genTable(Table table, OpRes res, GMUser user)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }
            string[] m_content = new string[65];
            int i = 0, k = 0, n = 0, j=0;
            for (i = 0; i < m_content.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);

                if (i == 0)
                {
                    td.Text = "日期";
                }
                else if (i == 1)
                {
                    td.Text = "100炮倍及以上活跃玩家";
                    td.Attributes.CssStyle.Value = "min-width:160px;";
                }
                else 
                {
                    if (i == 2)
                    {
                        td.Text = "task_" + (i - 1) + "(完成人数/完成率)";
                        td.Attributes.CssStyle.Value = "min-width:180px;";
                    }
                    else {
                        td.Text = "task_" + (i - 1);
                    }
                }
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            List<StatMainlyTaskItem> qresult =
                (List<StatMainlyTaskItem>)user.getQueryResult(QueryType.queryTypeStatMainlyTask);

            for (i = 0; i < qresult.Count; i++)
            {
                n = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                StatMainlyTaskItem item = qresult[i];
                m_content[n++] = item.m_time;
                m_content[n++] = item.m_activeCount.ToString();
                for (j = 1; j <= item.m_taskComplete.Count; j++) 
                {
                    m_content[n++] = item.m_taskComplete[j] + "(" + item.getRate(item.m_taskComplete[j]) + ")";
                }
                for (k = 0; k < m_content.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[k];
                }
            }
        }
    }
}