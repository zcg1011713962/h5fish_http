using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatWelFareLottery : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "奖券产出", "物品1", "物品2", "物品3", "物品4", "物品5", "物品6", "物品7", "物品8", "物品9", "物品10"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_WEL_FARE_LOTTERY, Session, Response);
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            OpRes res = OpRes.op_res_failed;

            res = user.doQuery(param, QueryType.queryTypeStatWelFareLottery);
            genTable(m_result, res, user, param);
        }

        private void genTable(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
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

            List<StatWelFareLotteryItem> qresult = (List<StatWelFareLotteryItem>)user.getQueryResult(QueryType.queryTypeStatWelFareLottery);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.RowSpan = 1;
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                StatWelFareLotteryItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_ticketOutlay.ToString();
                foreach (var da in item.m_rewardList)
                {
                    m_content[f++] = da.Value.ToString();
                }

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }

    }
}