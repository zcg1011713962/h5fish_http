using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat.shcd
{
    public partial class ShcdCardsQuery : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "牌局ID","开牌结果","系统支出", "系统收入","牌局详情","下注玩家列表"};
        private string[] m_content = new string[s_head.Length];

        private PageShcdCards m_gen = new PageShcdCards(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.SHCD_CARDS_QUERY, Session, Response);
            if (!IsPostBack)
            {
                m_roomId.Items.Add("金币场");
                m_roomId.Items.Add("龙珠场");

                if (m_gen.parse(Request))
                {
                    m_time.Text = m_gen.m_time;
                    m_cardId.Text = m_gen.m_param;
                    m_roomId.SelectedIndex = m_gen.m_op - 1;
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_param = m_cardId.Text;
            param.m_op = m_roomId.SelectedIndex + 1;

            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;

            OpRes res = user.doQuery(param, QueryType.queryTypeShcdCardsQuery);
            genTable(m_result, res, user, param);
        }

        //黑红梅方牌局调整
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

            List<ShcdCardsItem> qresult = (List<ShcdCardsItem>)user.getQueryResult(QueryType.queryTypeShcdCardsQuery);

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
                ShcdCardsItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_cardId.ToString();
                m_content[f++] = DefCC.s_pokerColorShcd[item.m_cardType];
                m_content[f++] = item.m_sysOutlay.ToString();
                m_content[f++] = item.m_sysIncome.ToString();
                m_content[f++] = item.getExParam(i);
                m_content[f++] = item.getExParam1(i);
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/shcd/ShcdCardsQuery.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}