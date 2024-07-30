using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPlayerOpenRateTask : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","10炮2天炮人数","20炮鱼雷人数","40炮箱子人数","90炮箱子人数","200炮箱子人数",
        "300炮箱子人数","400炮箱子人数","500炮箱子人数","1500炮箱子人数","2000炮箱子人数","系统总支出"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DTAT_STAT_TOTAL_PLAYER_OPENRATE_TASK, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;

            OpRes res = OpRes.op_res_failed;

            res = user.doQuery(param, QueryType.queryTypeStatPlayerOpenRateTask);
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

            int i = 0, k = 0, n = 0;
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            List<StatPlayerOpenRateTaskItem> qresult =
                (List<StatPlayerOpenRateTaskItem>)user.getQueryResult(QueryType.queryTypeStatPlayerOpenRateTask);

            for (i = 0; i < qresult.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                StatPlayerOpenRateTaskItem item = qresult[i];
                m_content[n++] = item.m_time;
                foreach(var da in item.m_openRateCount)
                {
                    m_content[n++] = da.Value.ToString();
                }
                m_content[n++] = item.m_outlay.ToString();

                for (k = 0; k < s_head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[k];
                }
            }
        }
    }
}