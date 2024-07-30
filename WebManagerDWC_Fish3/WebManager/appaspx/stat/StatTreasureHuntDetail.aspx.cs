using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatTreasureHuntDetail : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "时间","玩家ID","场次","金币","青铜鱼雷","白银鱼雷","黄金鱼雷","钻石鱼雷",
            "青铜鱼雷碎片","白银鱼雷碎片","黄金鱼雷碎片","钻石鱼雷碎片","总折合金币"};
        private string[] m_content = new string[s_head.Length];
        private PageTreasureHuntPlayer m_gen = new PageTreasureHuntPlayer(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);

            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = Convert.ToInt32(Request.QueryString["roomId"]);
            param.m_time = Request.QueryString["time"];
            param.m_param = Request.QueryString["playerId"];
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
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatTreasureHuntDetail, user);

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

            List<statTreasureHuntItem> qresult = (List<statTreasureHuntItem>)mgr.getQueryResult(QueryType.queryTypeStatTreasureHuntDetail);

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

                statTreasureHuntItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.getRoomName();
                m_content[f++] = item.m_item1.ToString();
                m_content[f++] = item.m_item24.ToString();
                m_content[f++] = item.m_item25.ToString();
                m_content[f++] = item.m_item26.ToString();
                m_content[f++] = item.m_item27.ToString();
                m_content[f++] = item.m_item43.ToString();
                m_content[f++] = item.m_item44.ToString();
                m_content[f++] = item.m_item45.ToString();
                m_content[f++] = item.m_item46.ToString();
                m_content[f++] = item.getTotalGold().ToString();
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatTreasureHuntDetail.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}