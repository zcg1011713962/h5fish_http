using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordArena : System.Web.UI.Page
    {
        private static string[] s_headRankCur = new string[] { "排行", "玩家名称", "玩家ID", "积分" };
        private static string[] s_headRankHistroy = new string[] { "日期", "排行", "玩家名称", "玩家ID", "积分" };
        private static string[] s_headStat = new string[] { "日期", "钻石收入", "参赛人次", "参赛人数" };

        private string[] m_content = new string[Math.Max(s_headRankCur.Length, s_headRankHistroy.Length)];

        private PagePlayerBankruptDetail m_gen = new PagePlayerBankruptDetail(100);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_FISHLORD_ARENA, Session, Response);
            if (!IsPostBack)
            {
                m_optional.Items.Add(new ListItem("当前日榜", QueryFishArena.RANK_DAY_CUR.ToString()));
                m_optional.Items.Add(new ListItem("历史日榜", QueryFishArena.RANK_DAY_HISTORY.ToString()));
                m_optional.Items.Add(new ListItem("当前周榜", QueryFishArena.RANK_WEEK_CUR.ToString()));
                m_optional.Items.Add(new ListItem("历史周榜", QueryFishArena.RANK_WEEK_HISTORY.ToString()));
                m_optional.Items.Add(new ListItem("竞技场统计", QueryFishArena.ARENA_STAT.ToString()));
               // m_optional.Items.Add(new ListItem("自由赛日榜", QueryFishArena.FREE_RANK_CUR.ToString()));
               // m_optional.Items.Add(new ListItem("自由赛历史榜", QueryFishArena.FREE_RANK_HISTORY.ToString()));

                m_room.Items.Add(new ListItem("初级赛", "1"));
                m_room.Items.Add(new ListItem("中级赛", "2"));

                if (m_gen.parse(Request))
                {
                    m_optional.SelectedIndex = m_gen.m_lotteryId;
                    m_room.SelectedIndex = Convert.ToInt32(m_gen.m_param);
                    m_time.Text = m_gen.m_time;
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = Convert.ToInt32(m_optional.SelectedValue);
            param.m_type = Convert.ToInt32(m_room.SelectedValue);
            param.m_time = m_time.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            
            OpRes res = user.doQuery(param, QueryType.queryTypeFishArena);
            switch(param.m_op)
            {
                case QueryFishArena.RANK_DAY_CUR:
                case QueryFishArena.RANK_WEEK_CUR:
                    genTableRankCur(m_result, res, user);
                    break;
                case QueryFishArena.RANK_DAY_HISTORY:
                case QueryFishArena.RANK_WEEK_HISTORY:
                    genTableRankHistory(m_result, res, param, user);
                    break;
                case QueryFishArena.ARENA_STAT:
                    genTableArenaStat(m_result, res, user);
                    break;
            }
        }

        private void genTableRankCur(Table table, OpRes res, GMUser user)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";

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

            int i = 0, k = 0, n = 0;
            for (i = 0; i < s_headRankCur.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_headRankCur[i];
            }

            FishArenaRankWrap qresult = (FishArenaRankWrap)user.getQueryResult(QueryType.queryTypeFishArena);

            for (i = 0; i < qresult.m_dataLine.m_rank.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                FishArenaRankInfo item = qresult.m_dataLine.m_rank[i];
                m_content[n++] = item.m_rank.ToString();
                m_content[n++] = item.m_nickName;
                m_content[n++] = item.m_playerId.ToString();
                m_content[n++] = item.m_socre.ToString();

                for (k = 0; k < s_headRankCur.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[k];
                }
            }
        }

        private void genTableRankHistory(Table table, OpRes res, ParamQuery param, GMUser user)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";

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

            int i = 0, k = 0, n = 0;
            for (i = 0; i < s_headRankHistroy.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_headRankHistroy[i];
            }

            FishArenaRankWrap qresult = (FishArenaRankWrap)user.getQueryResult(QueryType.queryTypeFishArena);

            foreach(var dinfo in qresult.m_dataDic)
            {
                var time = dinfo.Key;
                var rankList = dinfo.Value.m_rank;
                
                for (i = 0; i < rankList.Count; i++)
                {
                    n = 0;

                    tr = new TableRow();
                    m_result.Rows.Add(tr);

                    FishArenaRankInfo item = rankList[i];
                    m_content[n++] = time.ToShortDateString();

                    m_content[n++] = item.m_rank.ToString();
                    m_content[n++] = item.m_nickName;
                    m_content[n++] = item.m_playerId.ToString();
                    m_content[n++] = item.m_socre.ToString();

                    for (k = 0; k < s_headRankHistroy.Length; k++)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = m_content[k];
                    }
                }
            }

            param.m_op = m_optional.SelectedIndex;
            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatFishlordArena.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        private void genTableArenaStat(Table table, OpRes res, GMUser user)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";

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

            int i = 0, k = 0, n = 0;
            for (i = 0; i < s_headStat.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_headStat[i];
            }

            FishArenaRankWrap qresult = (FishArenaRankWrap)user.getQueryResult(QueryType.queryTypeFishArena);

            for (i = 0; i < qresult.m_stat.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                FishArenaStat item = qresult.m_stat[i];
                m_content[n++] = item.m_time.ToShortDateString();
                m_content[n++] = item.m_gemIncome.ToString();
                m_content[n++] = item.m_joinPersonCount.ToString();
                m_content[n++] = item.m_joinPerson.ToString();

                for (k = 0; k < s_headStat.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[k];
                }
            }
        }
    }
}