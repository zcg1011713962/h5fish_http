using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatLimitTimeBuy : System.Web.UI.Page
    {
        const int ITEM_COL_COUNT = 8;
        const int OTHER_COL_COUTN = 3;

        private static string[] s_head;
        private string[] m_content;

        static StatLimitTimeBuy()
        {
            s_head = new string[ITEM_COL_COUNT + OTHER_COL_COUTN];
            s_head[0] = "日期";
            s_head[1] = "单次兑换总数";
            s_head[2] = "10次兑换总数";

            for (int i = 3; i < s_head.Length; i++)
            {
                s_head[i] = "道具" + (i-2).ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_LIMIT_TIME_BUY, Session, Response);
            m_content = new string[s_head.Length];
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;

            OpRes res = user.doQuery(param, QueryType.queryTypeLimitTimeBuyActivity);
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

            List<TurretLotteryActInfo> qresult = (List<TurretLotteryActInfo>)user.getQueryResult(QueryType.queryTypeLimitTimeBuyActivity);

            for (i = 0; i < qresult.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                TurretLotteryActInfo item = qresult[i];
                m_content[n++] = item.m_time.ToShortDateString();
                m_content[n++] = item.m_lotteryOnce.ToString();
                m_content[n++] = item.m_lotteryTen.ToString();

                for (int m = 1; m <= ITEM_COL_COUNT; m++)
                {
                    if (item.m_data.ContainsKey(m))
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