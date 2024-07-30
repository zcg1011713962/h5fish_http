using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPumpNp7DayRechargeDetail : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "渠道","玩家ID","购买次数"};
        private string[] m_content = new string[s_head.Length];

        private PageGenStatNp7DayRechargeDetail m_gen = new PageGenStatNp7DayRechargeDetail(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);

            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = Request.QueryString["time"];
            param.m_param = Request.QueryString["channel"];
            if (m_gen.parse(Request))
            {
                param.m_curPage = m_gen.curPage;
                param.m_countEachPage = m_gen.rowEachPage;
            }
            //////////////////////////////////////////////////////////////
            genTable(m_result, param, user);
        }

        private void genTable(Table table, ParamQuery param, GMUser user)
        {
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatPumpNp7DaysRechargeDetail, user);

            table.GridLines = GridLines.Both;
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

            List<StatPumpNp7DayRechargeDetailItem> qresult = (List<StatPumpNp7DayRechargeDetailItem>)mgr.getQueryResult(QueryType.queryTypeStatPumpNp7DaysRechargeDetail);

            int i = 0, j = 0, f = 0;
            // 表头第一行
            tr = new TableRow();
            table.Rows.Add(tr);
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "日期："+param.m_time;
            td.ColumnSpan = 3;

            tr = new TableRow();
            table.Rows.Add(tr);
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.ColumnSpan = 1;
                td.RowSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                table.Rows.Add(tr);

                StatPumpNp7DayRechargeDetailItem item = qresult[i];
                m_content[f++] = item.getChannelName();
                m_content[f++] = item.m_playerId.ToString();
                //m_content[f++] = item.m_loginDays.ToString();
                m_content[f++] = item.m_rechargeCount.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatPumpNp7DayRechargeDetail.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;

        }
    }
}