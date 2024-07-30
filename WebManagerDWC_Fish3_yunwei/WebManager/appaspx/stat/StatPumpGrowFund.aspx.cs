using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPumpGrowFund : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","激活人数","打开人数","打开次数","未激活打开人数","未激活打开次数",
           "2级人数", "3级人数","4级人数","5级人数","6级人数","7级人数","8级人数","9级人数","10级人数","11级人数","12级人数","13级人数","14级人数","15级人数"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_PUMP_GROW_FUND, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;

            OpRes res = user.doQuery(param, QueryType.queryTypeStatPumpGrowFund);
            genTable(m_result, res, user, param);
        }

        private void genTable(Table table, OpRes res, GMUser user, ParamQuery p)
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

                td.RowSpan = 1;
                td.ColumnSpan = 1;
                td.Text = s_head[i];
            }

            List<StatPumpGrowFundItem> qresult =
                (List<StatPumpGrowFundItem>)user.getQueryResult(QueryType.queryTypeStatPumpGrowFund);

            for (i = 0; i < qresult.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                StatPumpGrowFundItem item = qresult[i];
                m_content[n++] = item.m_time;
                m_content[n++] = item.m_activationPerson.ToString();

                m_content[n++] = item.m_openWithActivationPerson.ToString();
                m_content[n++] = item.m_openWithActivationCount.ToString();

                m_content[n++] = item.m_openWithoutActivationPerson.ToString();
                m_content[n++] = item.m_openWithoutActivationCount.ToString();

                foreach (var da in item.m_receive)
                {
                    m_content[n++] = da.Value.ToString();
                }

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