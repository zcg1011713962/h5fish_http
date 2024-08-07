﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatIntScoreSendAward : System.Web.UI.Page
    {
        const int ITEM_COL_COUNT = 10;
        const int OTHER_COL_COUTN = 3;

        private static string[] s_headRankCur = new string[] { "排行", "玩家名称", "玩家ID", "积分" };
        private static string[] s_headRankHistroy = new string[] { "日期", "排行", "玩家名称", "玩家ID", "积分" };
        private string[] m_content = new string[Math.Max(s_headRankCur.Length, s_headRankHistroy.Length)];
        private static string[] s_headLottery = null;

        private PageJiuQiuNationalActPlayer m_gen = new PageJiuQiuNationalActPlayer(100);

        static StatIntScoreSendAward()
        {
            s_headLottery = new string[ITEM_COL_COUNT + OTHER_COL_COUTN];
            s_headLottery[0] = "日期";
            s_headLottery[1] = "单次兑换总数";
            s_headLottery[2] = "10次兑换总数";
            for (int i = OTHER_COL_COUTN; i < s_headLottery.Length; i++)
            {
                s_headLottery[i] = "道具" + (i - OTHER_COL_COUTN + 1).ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_INT_SCORE_SEND, Session, Response);

            m_content = new string[s_headLottery.Length];

            if (!IsPostBack)
            {
                m_optional.Items.Add(new ListItem("当前排行", QueryIntegrationScoreSendBigAward.RANK_CUR.ToString()));
                m_optional.Items.Add(new ListItem("历史排行", QueryIntegrationScoreSendBigAward.RANK_HISTORY.ToString()));
                m_optional.Items.Add(new ListItem("抽奖统计", QueryIntegrationScoreSendBigAward.LOTTERY.ToString()));

                if (m_gen.parse(Request))
                {
                    m_optional.SelectedIndex = m_gen.m_lotteryId;
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
            param.m_time = m_time.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;

            OpRes res = user.doQuery(param, QueryType.queryTypeIntScoreSendAward);
            switch (param.m_op)
            {
                case QueryIntegrationScoreSendBigAward.RANK_CUR:
                    genTableRankCur(m_result, res, user);
                    break;
                case QueryIntegrationScoreSendBigAward.RANK_HISTORY:
                    genTableRankHistory(m_result, res, param, user);
                    break;
                case QueryIntegrationScoreSendBigAward.LOTTERY:
                    genTableLottery(m_result, res, user);
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

            IntScoreWrap qresult = (IntScoreWrap)user.getQueryResult(QueryType.queryTypeIntScoreSendAward);

            for (i = 0; i < qresult.m_dataLine.m_rank.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                IntScoreRankInfo item = qresult.m_dataLine.m_rank[i];
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

            IntScoreWrap qresult = (IntScoreWrap)user.getQueryResult(QueryType.queryTypeIntScoreSendAward);

            foreach (var dinfo in qresult.m_dataDic)
            {
                var time = dinfo.Key;
                var rankList = dinfo.Value.m_rank;

                for (i = 0; i < rankList.Count; i++)
                {
                    n = 0;

                    tr = new TableRow();
                    m_result.Rows.Add(tr);

                    IntScoreRankInfo item = rankList[i];
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
            m_gen.genPage(param, @"/appaspx/stat/StatIntScoreSendAward.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        private void genTableLottery(Table table, OpRes res, GMUser user)
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
            for (i = 0; i < s_headLottery.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_headLottery[i];
            }

            IntScoreWrap qresult = (IntScoreWrap)user.getQueryResult(QueryType.queryTypeIntScoreSendAward);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                TurretLotteryActInfo item = qresult.m_lotteryList[i];
                m_content[n++] = item.m_time.ToShortDateString();
                m_content[n++] = item.m_lotteryOnce.ToString();
                m_content[n++] = item.m_lotteryTen.ToString();

                for (int m = 1; m <= ITEM_COL_COUNT; m++)
                {
                    if (item.m_data.ContainsKey(m))
                    {
                        m_content[n++] = item.m_data[m].m_count.ToString();
                    }
                    else
                    {
                        m_content[n++] = "0";
                    }
                }

                for (k = 0; k < s_headLottery.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[k];
                }
            }
        }
    }
}