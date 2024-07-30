using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPumpRedEnvelopExchange : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","礼包","京东卡","支出金币"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_PUMP_RED_ENVELOP_EXCHANGE, Session, Response);
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            OpRes res = user.doQuery(param, QueryType.queryTypeStatPumpRedEnvelopExchange);
            genTable(m_result, res, user, param);
        }

        //红包方案
        private void genTable(Table table, OpRes res, GMUser user, ParamQuery query_param)
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

            List<StatPumpRedEnvelopExchangeItem> qresult =
                (List<StatPumpRedEnvelopExchangeItem>)user.getQueryResult(QueryType.queryTypeStatPumpRedEnvelopExchange);

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
                StatPumpRedEnvelopExchangeItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_giftExchange.ToString();
                m_content[f++] = item.m_cardExchange.ToString();
                m_content[f++] = item.m_goldOutlay.ToString();
                
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