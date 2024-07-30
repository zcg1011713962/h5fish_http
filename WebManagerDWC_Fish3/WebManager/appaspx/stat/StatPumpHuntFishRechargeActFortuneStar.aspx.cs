using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPumpHuntFishRechargeActFortuneStar : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "玩家ID","兑换类型"}; 
        private string[] m_content = new string[s_head.Length];

        private PageGenFishlordAdvancedRoom m_gen = new PageGenFishlordAdvancedRoom(50);
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
            //////////////////////////////////////////////////////////////
            genTable(m_result, param, user);
        }

        private void genTable(Table table, ParamQuery param, GMUser user)
        {
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatPumpFortuneStarDetail, user);

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

            List<StatPumpFortuneStarPlayerList> qresult =
                (List<StatPumpFortuneStarPlayerList>)mgr.getQueryResult(QueryType.queryTypeStatPumpFortuneStarDetail);

            td = new TableCell();
            tr.Cells.Add(td);
            td.ColumnSpan = 2;
            td.Text = "时间： " + param.m_time;

            int i = 0, j = 0, f = 0;
            // 表头第一行
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
                f = 0;
                tr = new TableRow();
                table.Rows.Add(tr);

                StatPumpFortuneStarPlayerList item = qresult[i];
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_index.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatPumpHuntFishRechargeActFortuneStar.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;

        }

    }
}