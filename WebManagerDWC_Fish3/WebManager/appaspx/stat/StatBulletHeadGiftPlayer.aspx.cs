using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatBulletHeadGiftPlayer : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "礼包类型", "玩家ID", "获得道具数量" };
        private string[] m_content = new string[s_head.Length];
        private PagePanicBoxPlayer m_gen = new PagePanicBoxPlayer(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);

            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = Convert.ToInt32(Request.QueryString["boxId"]);
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
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatBulletHeadGiftPlayer, user);

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

            List<StatBulletHeadGiftPlayerItem> qresult = (List<StatBulletHeadGiftPlayerItem>)mgr.getQueryResult(QueryType.queryTypeStatBulletHeadGiftPlayer);

            td = new TableCell();
            tr.Cells.Add(td);
            td.ColumnSpan = 3;
            td.Text = "日期：" + param.m_time;
            int i = 0, j = 0, f = 0;
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
                f = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                StatBulletHeadGiftPlayerItem item = qresult[i];
                m_content[f++] = item.getGiftName();
                m_content[f++] = item.m_playerId;
                m_content[f++] = item.m_gainItemCount.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatBulletHeadGiftPlayer.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

    }
}