using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPlayerBw : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "场次类型", "开房玩家ID", "挑战玩家ID", "入场费", "胜负关系"};
        private string[] m_content = new string[s_head.Length];

        private PagePlayerPumpBw m_gen = new PagePlayerPumpBw(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";

            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_PLAYER_BW, Session, Response);
            if (m_gen.parse(Request))
            {
                m_time.Text = m_gen.m_time;
                onQuery(null, null);
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_countEachPage = m_gen.rowEachPage;
            param.m_curPage = m_gen.curPage;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatPlayerBw, user);
            genTable(m_result, res, param, user, mgr);
        }

        //生成查询表
        private void genTable(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            m_result.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
            TableCell td = null;
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }
            List<StatplayerBw> qresult = (List<StatplayerBw>)mgr.getQueryResult(QueryType.queryTypeStatPlayerBw);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }
            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;
                StatplayerBw item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.getRoomTypeName();
                m_content[f++] = item.m_ownerId.ToString();
                m_content[f++] = item.m_challengeId.ToString();
                m_content[f++] = item.m_enterDb.ToString();
                m_content[f++] = item.getLoseWinRes();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatPlayerBw.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}