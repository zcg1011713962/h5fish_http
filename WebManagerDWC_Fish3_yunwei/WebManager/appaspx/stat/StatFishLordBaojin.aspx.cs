using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishLordBaojin : System.Web.UI.Page
    {
        private static string[] s_head_1 = new string[] { "日期", "比赛参与总次数","比赛门票收入","玩家胜率","比赛参与人数","比赛活跃人数", "比赛档位奖励总计","比赛1次人数", "比赛3次人数",
            "比赛5次人数", "比赛10次人数", "比赛20次人数" ,"比赛平均时间","比赛1档爆机次数","比赛2档爆机次数","比赛3档爆机次数","比赛4档爆机次数","比赛5档爆机次数"};
        private string[] m_content_1=new string[s_head_1.Length];
        private static string[] s_head_2 = new string[] {"日排行榜（前100）", "玩家ID","昵称", "日最高积分", "最高炮台倍率","VIP等级", "是否机器人"};
        private string[] m_content_2 = new string[s_head_2.Length];
        private static string[] s_head_3 = new string[] { "周排行榜", "玩家ID", "昵称", "周最高积分", "最高炮台倍率"/*,"详情"*/, "是否机器人" };
        private string[] m_content_3 = new string[s_head_3.Length];
        private static string[] s_head_4 = new string[] { "日期", "排名", "昵称", "玩家ID", "日最高积分", "VIP等级", "最高炮台倍率", "是否机器人" };
        private string[] m_content_4 = new string[s_head_4.Length];
        private static string[] s_head_5 = new string[] { "日期", "排名", "昵称", "玩家ID"/*, "该玩家累计充值", "冠军累计获得次数", "前10名累计获得次数"*/, "是否机器人" };
        private string[] m_content_5=new string[s_head_5.Length];
       
        private PageFishBaojin m_gen = new PageFishBaojin(100);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_STAT_FISHLORD_BAOJIN, Session, Response);
            if (!IsPostBack)
            {
                //m_queryType.Items.Add("竞技场统计");
               // m_queryType.Items.Add("");
                m_queryType.Items.Add("排行榜统计");

                m_rank.Items.Add("日排行榜");
                m_rank.Items.Add("周排行榜");

                m_actType.Items.Add("当前活动");
                m_actType.Items.Add("历史活动");

                if (m_gen.parse(Request))
                {
                    m_queryType.SelectedIndex = m_gen.m_way;
                    m_time.Text = m_gen.m_time;
                    if (m_gen.m_way == 1)
                    {
                        m_rank.SelectedIndex = m_gen.m_param;
                        m_actType.SelectedIndex = m_gen.m_showWay;
                    }
                    onQuery(null, null);
                }
            }
        }
        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_way = (QueryWay)m_queryType.SelectedIndex;
            param.m_time = m_time.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            /*if (param.m_way == QueryWay.by_way0) //爆金比赛场
            {
                OpRes res = user.doQuery(param, QueryType.queryTypeFishlordBaojinStat);
                genTable1(m_result, res, user, param);
            }*/
            if (param.m_way == QueryWay.by_way0) //爆金排行榜
            {
                param.m_param = Convert.ToString(m_rank.SelectedIndex);
                param.m_showWay = Convert.ToString(m_actType.SelectedIndex);
                OpRes res = user.doQuery(param, QueryType.queryTypeFishlordBaojinRank);
                if (m_rank.SelectedIndex==0)  //日排行榜
                {
                    switch (m_actType.SelectedIndex) 
                    {
                        case 0: genTable2(m_result, res, user); break;//日排行榜(当前)
                        case 1: genTable4(s_head_4, m_content_4, m_result, res, user, param); break;//日排行榜(历史)
                    }
                }else if (m_rank.SelectedIndex == 1) //周排行榜
                {
                    switch (m_actType.SelectedIndex) 
                    {
                        case 0: genTable3(m_result, res, user); break;//周排行榜(当前)
                        case 1: genTable4(s_head_5,m_content_5,m_result, res, user, param); break;//周排行榜(历史)
                    }
                }
            }
        }
        //爆金比赛场
        private void genTable1(Table table, OpRes res, GMUser user, ParamQuery query_param)
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

            List<FishlordBaojinStatItem> qresult = (List<FishlordBaojinStatItem>)user.getQueryResult(QueryType.queryTypeFishlordBaojinStat);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head_1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.Text = s_head_1[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                FishlordBaojinStatItem item = qresult[i];

                m_content_1[f++] = item.m_time;
                m_content_1[f++] = item.m_joinCount.ToString();
                m_content_1[f++] = String.Format("{0:N0}",item.m_ticketIncome);
                m_content_1[f++] = item.m_winRate.ToString();
                m_content_1[f++] = item.m_personCount.ToString();
                m_content_1[f++] = item.m_activeCount.ToString();

                m_content_1[f++] = item.m_giveOutGold.ToString();
                m_content_1[f++] = item.m_matchPerson1.ToString();
                m_content_1[f++] = item.m_matchPerson2.ToString();
                m_content_1[f++] = item.m_matchPerson3.ToString();
                m_content_1[f++] = item.m_matchPerson4.ToString();
                m_content_1[f++] = item.m_matchPerson5.ToString();
                m_content_1[f++] = item.m_matchTime.ToString();
                m_content_1[f++] = item.m_baoji1.ToString();
                m_content_1[f++] = item.m_baoji2.ToString();
                m_content_1[f++] = item.m_baoji3.ToString();
                m_content_1[f++] = item.m_baoji4.ToString();
                m_content_1[f++] = item.m_baoji5.ToString();
                
                for (j = 0; j < s_head_1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content_1[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatFishLordBaojin.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
        //爆金排行榜 日（当前）
        private void genTable2(Table table, OpRes res, GMUser user)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }

            List<FishlordBaojinRankItem> qresult = (List<FishlordBaojinRankItem>)user.getQueryResult(QueryType.queryTypeFishlordBaojinRank);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head_2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head_2[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                FishlordBaojinRankItem item = qresult[i];
                m_content_2[f++] = item.m_rank.ToString();
                m_content_2[f++] = item.m_playerId.ToString();
                m_content_2[f++] = item.m_nickName;
                m_content_2[f++] = item.m_maxScore.ToString();
                //m_content_2[f++] = item.m_joinCount.ToString();
                //m_content_2[f++] = item.m_baoji_1.ToString();
                //m_content_2[f++] = item.m_baoji_2.ToString();
                //m_content_2[f++] = item.m_baoji_3.ToString();
                //m_content_2[f++] = item.m_baoji_4.ToString();
                //m_content_2[f++] = item.m_baoji_5.ToString();
                m_content_2[f++] = item.m_fishLevel.ToString();
                m_content_2[f++] = item.m_vipLevel.ToString();
                m_content_2[f++] = item.m_isRobot ? "是" : "";

                for (j = 0; j < s_head_2.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content_2[j];
                }
            }
        }
        //爆金排行榜 日\周（历史）
        private void genTable4(string[]s_head,string[] m_content,Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }
            List<FishlordBaojinRankItem> qresult = (List<FishlordBaojinRankItem>)user.getQueryResult(QueryType.queryTypeFishlordBaojinRank);

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

                FishlordBaojinRankItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_rank.ToString();
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_playerId.ToString();
                if(Convert.ToInt32(query_param.m_param)==1) //周
                {
                   // m_content[f++] = item.m_totalRecharge.ToString();
                   // m_content[f++] = item.m_weekChampionCount.ToString();
                   // m_content[f++] = item.m_weekTop10Count.ToString();
                }
                else if (Convert.ToInt32(query_param.m_param) == 0)//日
                {
                    m_content[f++] = item.m_maxScore.ToString();
                    m_content[f++] = item.m_vipLevel.ToString();
                    m_content[f++] = item.m_fishLevel.ToString();
                }
                m_content[f++] = item.m_isRobot ? "是" : "";

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatFishLordBaojin.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
        //爆金排行榜 周（当前）
        private void genTable3(Table table, OpRes res, GMUser user)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }

            List<FishlordBaojinRankItem> qresult = (List<FishlordBaojinRankItem>)user.getQueryResult(QueryType.queryTypeFishlordBaojinRank);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head_3.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head_3[i];
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                FishlordBaojinRankItem item = qresult[i];
                m_content_3[f++] = item.m_rank.ToString();
                m_content_3[f++] = item.m_playerId.ToString();
                m_content_3[f++] = item.m_nickName;
                m_content_3[f++] = item.m_maxScore.ToString();
                m_content_3[f++] = item.m_fishLevel.ToString();
                //m_content_3[f++] = item.getExParam(i);
                m_content_3[f++] = item.m_isRobot ? "是" : "";

                for (j = 0; j < s_head_3.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content_3[j];
                }
            }
        }
    }
}