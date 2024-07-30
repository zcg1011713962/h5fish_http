using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatWjlwGoldEarnTurnInfo : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "局ID","开始时间", "收入", "支出", "盈利率", "获奖玩家ID", "获奖玩家购买注数" ,"玩家是否领取","详情"};
        private string[] m_content = new string[s_head.Length];

        private PagePanicBoxPlayer m_gen = new PagePanicBoxPlayer(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);

            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = Request.QueryString["time"];
            if (m_gen.parse(Request))
            {
                param.m_curPage = m_gen.curPage;
                param.m_countEachPage = m_gen.rowEachPage;
            }
            genTable(m_result, param, user);
        }

        private void genTable(Table table, ParamQuery param, GMUser user)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeWjlwGoldEarnTurnInfo, user);

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

            List<WjlwGoldEarnTurnInfoItem> qresult = (List<WjlwGoldEarnTurnInfoItem>)mgr.getQueryResult(QueryType.queryTypeWjlwGoldEarnTurnInfo);

            td = new TableCell();
            tr.Cells.Add(td);
            td.ColumnSpan = 8;
            td.Text = "日期：" + param.m_time;
            int i = 0, j = 0, index = 0;
            // 表头
            tr = new TableRow();
            table.Rows.Add(tr);
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                index = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                WjlwGoldEarnTurnInfoItem item = qresult[i];
                m_content[index++] = item.m_turnId.ToString();
                m_content[index++] = item.m_time;
                m_content[index++] = item.m_income.ToString();
                m_content[index++] = item.m_outlay.ToString();
                m_content[index++] = item.getEarnRate();
                m_content[index++] = item.m_winPlayerId == -1 ? "机器人" : item.m_winPlayerId.ToString();
                m_content[index++] = item.m_equipCount.ToString();
                m_content[index++] = item.m_isRecv == true ? "是" : "否";
                m_content[index++] = item.getDetail();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatWjlwGoldEarnTurnInfo.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}