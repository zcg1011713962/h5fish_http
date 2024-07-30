using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationPlayerMoneyRep : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "金币总携带库存", "龙珠总携带库存", "金币实际携带库存", "龙珠实际携带库存", 
            "金币大用户库存", "龙珠大用户库存" };
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_STAT_PLAYER_MONEY_REP, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            OpRes res = user.doQuery(param, QueryType.queryTypeStatPlayerMoneyRep);
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

            List<statPlayerMoneyRepItem> qresult = (List<statPlayerMoneyRepItem>)user.getQueryResult(QueryType.queryTypeStatPlayerMoneyRep);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                statPlayerMoneyRepItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_goldTotalRep.ToString();
                m_content[f++] = item.m_dragonBallTotalRep.ToString();
                m_content[f++] = item.m_goldActualRep.ToString();
                m_content[f++] = item.m_dragonBallActualRep.ToString();
                m_content[f++] = item.m_goldPlayerRep.ToString();
                m_content[f++] = item.m_dragonBallPlayerRep.ToString();
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