using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatDailyHourMaxOnlinePlayer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_DAILY_HOUR_MAX_ONLINE_PLAYER, Session, Response);
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            OpRes res = OpRes.op_res_failed;

            res = user.doQuery(param, QueryType.queryTypeStatDailyHourMaxOnlinePlayer);
            genTable(m_resTable, res, user);
        }

            
        private void genTable(Table table, OpRes res, GMUser user)
        {
            string[] s_head = new string[25];
            string[] m_content = new string[s_head.Length];
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

            List<StatVipExclusiveTaskItem> qresult =
                (List<StatVipExclusiveTaskItem>)user.getQueryResult(QueryType.queryTypeStatDailyHourMaxOnlinePlayer);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                if (i == 0)
                {
                    td.Text = "日期";
                }
                else {
                    td.Text = (i-1).ToString() + ":00";
                }
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                table.Rows.Add(tr);

                StatVipExclusiveTaskItem item = qresult[i];

                m_content[f++] = item.m_time;
                foreach (var da in item.m_task)
                {
                    m_content[f++] = da.ToString();
                }

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