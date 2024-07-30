using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat.cows
{
    public partial class CowsCardCtrlList : System.Web.UI.Page
    {
        public static string[] s_head = new string[] { "牌局", "日期时间", "用户ID", "房间", "杀分或放分"};
        public string[] m_content = new string[s_head.Length];
        private PageShcdCards m_gen = new PageShcdCards(100);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.COW_CARDS_CTRL_LIST, Session, Response);
            if (!IsPostBack)
            {
                m_queryType.Items.Add("牌局ID");
                m_queryType.Items.Add("玩家ID");

                if (m_gen.parse(Request))
                {
                    m_queryType.SelectedIndex = m_gen.m_op;
                    m_time.Text = m_gen.m_time;
                    m_param.Text = m_gen.m_param;
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_op = m_queryType.SelectedIndex;
            param.m_param = m_param.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            OpRes res = user.doQuery(param, QueryType.queryTypeCowsCardCtrlList);
            genTable(m_result, res, user, param);
        }

        private void genTable(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                m_page.InnerHtml = "";
                m_foot.InnerHtml = "";
                return;
            }

            List<killSendScoreCtrlListItem> qresult = (List<killSendScoreCtrlListItem>)user.getQueryResult(QueryType.queryTypeCowsCardCtrlList);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                killSendScoreCtrlListItem item = qresult[i];
                m_content[f++] = item.m_cardId.ToString();
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_playerId;
                m_content[f++] = StrName.s_shcdRoomName[item.m_roomId];
                m_content[f++] = item.m_type;

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/cows/CowsCardCtrlList.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}