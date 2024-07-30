using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordGrandPrixActivity : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "排名", "玩家ID", "玩家昵称", "VIP等级", "排行积分"};

        private PageJiuQiuNationalActPlayer m_gen = new PageJiuQiuNationalActPlayer(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_DRAGON_SCALE_RANK, Session, Response);
            if (!IsPostBack)
            {
                m_actType.Items.Add("今日排行");
                m_actType.Items.Add("赛季排行");
                m_actType.Items.Add("历史排行");
                m_actType.Items.Add("赛季历史排行");

                GMUser user = (GMUser)Session["user"];
                ParamQuery param = new ParamQuery();

                if (m_gen.parse(Request))
                {
                    m_actType.SelectedIndex = m_gen.m_lotteryId;
                    m_time.Text = m_gen.m_time;

                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();

            param.m_op = m_actType.SelectedIndex;//当前 历史

            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            if (param.m_op == 0 || param.m_op == 1) //当前 今日排行 赛季排行
            {
                OpRes res = mgr.doQuery(param, QueryType.queryTypeStatFishlordGrandPrixActDailyRank, user);
                genTable(m_result, res, param, user, mgr);
            }
            else if (param.m_op == 2)  // 历史 历史排行
            {
                param.m_time = m_time.Text;
                OpRes res = mgr.doQuery(param, QueryType.queryTypeStatFishlordGrandPrixActHisDailyRank, user);
                genTable1(m_result, res, param, user, mgr);
            }
            else if (param.m_op == 3)  // 历史 赛季历史排行
            {
                param.m_time = m_time.Text;
                OpRes res = mgr.doQuery(param, QueryType.queryTypeStatFishlordGrandPrixActHisDailyRank, user);
                genTable1(m_result, res, param, user, mgr);
            }
        }

        //生成查询表 //当前排行
        private void genTable(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            string[] m_content = new string[s_head.Length];

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
            List<FishlordGrandPrixActRankItem> qresult = 
                (List<FishlordGrandPrixActRankItem>)mgr.getQueryResult(QueryType.queryTypeStatFishlordGrandPrixActDailyRank);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                if (i == 0)
                    continue;

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                FishlordGrandPrixActRankItem item = qresult[i];
                m_content[f++] = item.m_rankId.ToString();
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_vip.ToString();
                m_content[f++] = item.m_points.ToString();
                for (j = 0; j < s_head.Length - 1; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }

        //生成查询表 //历史排行
        private void genTable1(Table table, OpRes res, ParamQuery param, GMUser user, QueryMgr mgr)
        {
            string[] m_content = new string[s_head.Length];

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
            List<FishlordGrandPrixActRankItem> qresult = 
                (List<FishlordGrandPrixActRankItem>)mgr.getQueryResult(QueryType.queryTypeStatFishlordGrandPrixActHisDailyRank);
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
                table.Rows.Add(tr);
                f = 0;
                FishlordGrandPrixActRankItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_rankId.ToString();
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_vip.ToString();
                m_content[f++] = item.m_points.ToString();
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatFishlordGrandPrixActivity.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}