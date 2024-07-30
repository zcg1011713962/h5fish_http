using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordAdvancedRoomActDetail : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","中奖金额","中奖奖项","使用炮倍","玩家ID","玩家昵称"};
        private string[] m_content = new string[s_head.Length];

        private PageGenFishlordAdvancedRoom m_gen = new PageGenFishlordAdvancedRoom(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_ADVANCED_ROOM_ACT, Session, Response);

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
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatFishlordAdvancedRoomActDetail, user);

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

            List<FishlordAdvancedRoomDetailItem> qresult =
                (List<FishlordAdvancedRoomDetailItem>)mgr.getQueryResult(QueryType.queryTypeStatFishlordAdvancedRoomActDetail);

            int i = 0, j = 0, f = 0;
            // 表头第一行
            tr = new TableRow();
            table.Rows.Add(tr);
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                table.Rows.Add(tr);

                FishlordAdvancedRoomDetailItem item = qresult[i];
                m_content[f++] = item.m_time.ToString();
                m_content[f++] = item.m_gold.ToString();
                m_content[f++] = item.getLevelName();
                m_content[f++] = item.m_bulletRate.ToString();
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_nickName;

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatFishlordAdvancedRoomActDetail.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;

        }
    }
}