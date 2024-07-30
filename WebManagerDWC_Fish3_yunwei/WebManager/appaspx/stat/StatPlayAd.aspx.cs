using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPlayAd : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "视频次数", "视频人数", "领取救济金翻倍人数", "支出金币" };
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_PLAT_AD, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;

            OpRes res = user.doQuery(param, QueryType.queryTypeStatPlayAd);
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
            int i = 0, n = 0, j = 0;
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            List<StatPlayAdItem> qresult =
                (List<StatPlayAdItem>)user.getQueryResult(QueryType.queryTypeStatPlayAd);

            for (i = 0; i < qresult.Count; i++)
            {
                n = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                StatPlayAdItem item = qresult[i];
                m_content[n++] = item.m_time;
                m_content[n++] = item.m_playCount.ToString();
                m_content[n++] = item.m_playPerson.ToString();
                m_content[n++] = item.m_doubleBenifitsCount.ToString();
                m_content[n++] = item.getTotalOutlay().ToString();

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