using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatExchangeActivity : System.Web.UI.Page
    {
        const int ITEM_COL_COUNT = 15;
        const int OTHER_COL_COUTN = 1;

        private static string[] s_head;
        private string[] m_content;

        private PageJiuQiuNationalActPlayer m_gen = new PageJiuQiuNationalActPlayer(100);

        static StatExchangeActivity()
        {
            s_head = new string[ITEM_COL_COUNT + OTHER_COL_COUTN];
            s_head[0] = "日期";
            for (int i = 1; i < s_head.Length; i++)
            {
                s_head[i] = "兑换" + i.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_TURRET_EXCHANGE, Session, Response);
            m_content = new string[s_head.Length];
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;

            OpRes res = user.doQuery(param, QueryType.queryTypeExchangeActivity);
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
            }

            List<ExchangeActInfo> qresult = (List<ExchangeActInfo>)user.getQueryResult(QueryType.queryTypeExchangeActivity);

            for (i = 0; i < qresult.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                ExchangeActInfo item = qresult[i];
                m_content[n++] = item.m_time.ToShortDateString();
                for (int m = 1; m <= ITEM_COL_COUNT; m++)
                {
                    if(item.m_data.ContainsKey(m))
                    {
                        m_content[n++] = item.m_data[m].m_count.ToString();
                    }
                    else
                    {
                        m_content[n++] = "0";
                    }
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