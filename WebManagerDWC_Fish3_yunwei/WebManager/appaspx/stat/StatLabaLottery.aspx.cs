using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatLabaLottery : System.Web.UI.Page
    {
        private static string[] s_head_1=new string[]{"日期","档位名称","出现次数","金币发放量","钻石发放量"};
        private static string[] s_head_2 = new string[] { "日期", "玩家ID", "奖励ID"};
        private static string[] s_head_3 = new string[] { "日期", "玩家ID", "抽奖次数"};
        private static string[] s_head_4 = new string[] { "日期","每日游戏次数","每日充值人数","每日游戏人数","每日新增游戏次数","每日剩余游戏次数"};
        private static string[] m_content_1=new string[s_head_1.Length];
        private static string[] m_content_2 = new string[s_head_2.Length];
        private static string[] m_content_3 = new string[s_head_2.Length];
        private static string[] m_content_4=new string[s_head_4.Length];
        private PageLabaLottery m_gen = new PageLabaLottery(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_LABA_LOTTERY, Session, Response);
            if(!IsPostBack)
            {
                m_queryType.Items.Add("拉霸抽奖档位查询");
                m_queryType.Items.Add("玩家抽奖次数统计");
                //m_queryType.Items.Add("玩家抽奖记录查询");
                m_queryType.Items.Add("拉霸活动统计");
                
                if (m_gen.parse(Request))
                {
                    m_queryType.SelectedIndex = m_gen.m_way;
                    m_time.Text = m_gen.m_time;
                    m_playerId.Text = m_gen.m_playerId;
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_way = (QueryWay)m_queryType.SelectedIndex;
            if (param.m_way == QueryWay.by_way0) //拉霸抽奖档位查询
            {
                param.m_curPage = m_gen.curPage;
                param.m_countEachPage = m_gen.rowEachPage;
                OpRes res = user.doQuery(param, QueryType.queryTypeLabaLotteryProb);
                genTable1(m_result, res, user,param);
            }

            if (param.m_way == QueryWay.by_way1) //玩家抽奖次数统计
            {
                param.m_param = m_playerId.Text;
                param.m_curPage = m_gen.curPage;
                param.m_countEachPage = m_gen.rowEachPage;
                OpRes res = user.doQuery(param, QueryType.queryTypeLabaLotteryStat);
                genTable3(m_result, res, user, param);
            }

            //if (param.m_way == QueryWay.by_way2) //玩家抽奖记录查询
            //{
            //    param.m_param = m_playerId.Text;
            //    param.m_curPage = m_gen.curPage;
            //    param.m_countEachPage = m_gen.rowEachPage;
            //    OpRes res = user.doQuery(param, QueryType.queryTypeLabaLotteryQuery);
            //    genTable2(m_result, res, user,param);
            //}

            if(param.m_way==QueryWay.by_way2) //拉霸活动统计
            {
                param.m_curPage = m_gen.curPage;
                param.m_countEachPage = m_gen.rowEachPage;
                OpRes res = user.doQuery(param, QueryType.queryTypeLabaActivityStat);
                genTable4(m_result, res, user, param);
            }
        }

        //生成表 拉霸抽奖档位查询
        private void genTable1(Table table, OpRes res, GMUser user,ParamQuery query_param)
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

            List<LabaLotteryProbItem> qresult = (List<LabaLotteryProbItem>)user.getQueryResult(QueryType.queryTypeLabaLotteryProb);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head_1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head_1[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                LabaLotteryProbItem item = qresult[i];

                m_content_1[f++] = item.m_time;
                m_content_1[f++] = getName(item.m_labaProbId);
                m_content_1[f++] = item.m_appearCount.ToString();
                m_content_1[f++] = item.m_goldReward.ToString();
                m_content_1[f++] = item.m_diamondReward.ToString();

                for (j = 0; j < s_head_1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content_1[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatLabaLottery.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        //生成表 玩家抽奖记录查询
        private void genTable2(Table table, OpRes res, GMUser user,ParamQuery query_param)
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

            List<LabaLotteryPlayerRecordItem> qresult = (List<LabaLotteryPlayerRecordItem>)user.getQueryResult(QueryType.queryTypeLabaLotteryQuery);

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

                LabaLotteryPlayerRecordItem item = qresult[i];

                m_content_2[f++] = item.m_time;
                m_content_2[f++] = item.m_playerId.ToString();
                m_content_2[f++] = getName(item.m_rewardId);

                for (j = 0; j < s_head_2.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content_2[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatLabaLottery.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        //生成表 玩家抽奖记录查询
        private void genTable3(Table table, OpRes res, GMUser user, ParamQuery query_param)
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

            List<LabaLotteryPlayerRecordItem> qresult = (List<LabaLotteryPlayerRecordItem>)user.getQueryResult(QueryType.queryTypeLabaLotteryStat);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head_3.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head_3[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                LabaLotteryPlayerRecordItem item = qresult[i];

                m_content_3[f++] = item.m_time;
                m_content_3[f++] = item.m_playerId.ToString();
                m_content_3[f++] = item.m_rewardCount.ToString();

                for (j = 0; j < s_head_3.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content_3[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatLabaLottery.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        //生成表 玩家抽奖记录查询
        private void genTable4(Table table, OpRes res, GMUser user, ParamQuery query_param)
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

            List<LabaActivityStatItem> qresult = (List<LabaActivityStatItem>)user.getQueryResult(QueryType.queryTypeLabaActivityStat);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head_4.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head_4[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                LabaActivityStatItem item = qresult[i];

                m_content_4[f++] = item.m_time;
                m_content_4[f++] = item.m_dailyGameCount.ToString();
                m_content_4[f++] = item.m_dailyRechargePersonNum.ToString();
                m_content_4[f++] = item.m_dailyPlayerCount.ToString();
                m_content_4[f++] = item.m_dailyGainGameCount.ToString();
                m_content_4[f++] = item.m_dailyRemainGameCount.ToString();

                for (j = 0; j < s_head_4.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content_4[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatLabaLottery.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
        public string getName(int key)
        {
            string name = key.ToString();
            switch (key)
            {
                case 1: name = "Vip2以下——5个全相同"; break;
                case 2: name = "Vip2以下——5个全不相同"; break;
                case 3: name = "Vip2以下——4个相同，1个不同"; break;
                case 4: name = "Vip2以下——3个相同，2个不同"; break;
                case 5: name = "Vip2以下——2个相同，3个不同"; break;
                case 6: name = "Vip2以下——22个相同（AABBC）"; break;
                case 7: name = "Vip2以下——32个相同（AAABB）"; break;

                case 10: name = "Vip2及以上——5个全相同"; break;
                case 11: name = "Vip2及以上——5个全不相同"; break;
                case 12: name = "Vip2及以上——4个相同，1个不同"; break;
                case 13: name = "Vip2及以上——3个相同，2个不同"; break;
                case 14: name = "Vip2及以上——2个相同，3个不同"; break;
                case 15: name = "Vip2及以上——22个相同（AABBC）"; break;
                case 16: name = "Vip2及以上——32个相同（AAABB）"; break;
            }
            return name;
        }
    }
}