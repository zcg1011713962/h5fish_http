﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatKillCrabAct : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] { "日期","抽奖类型","金币消耗","大宝剑产出","抽奖人数","抽奖次数","奖励1人次","奖励2人次",
            "奖励3人次","奖励4人次","奖励5人次","奖励6人次","奖励7人次","奖励8人次","奖励9人次","奖励总支出","详情"};

        // private static string[] s_head2 = new string[] { "日期","鳄鱼场消耗","巨鲨场消耗","龙宫场消耗","大圣场消耗","魔鲲场消耗","圣兽场消耗",
        //     "鳄鱼场击杀鱼数量","巨鲨场击杀鱼数量","龙宫场击杀鱼数量","大圣场击杀鱼数量","魔鲲场击杀鱼数量","圣兽场击杀鱼数量",
        //     "鳄鱼场掉落数量","巨鲨场掉落数量","龙宫场掉落数量","大圣场掉落数量","魔鲲场掉落数量","圣兽场掉落数量"};
        //
        private static string[] s_head2;

        private static string[] s_head3 = new string[] { "日期", "击杀1次人数", "击杀2次人数", "击杀3次人数", "击杀4次人数", "击杀5次人数", 
            "击杀6次人数", "击杀7次人数","击杀8次人数","击杀9次人数","奖励总支出"};

        private static int[] s_roomId = new int[] { 2, 6, 5, 7, 3, 8, 9, 10 };

        private static string[] s_headRankCur = { "排名", "玩家ID", "昵称", "积分" };
        private static string[] s_headRankHistory = { "日期", "排名", "玩家ID", "昵称", "积分" };

        static StatKillCrabAct()
        {
            s_head2 = new string[s_roomId.Length * 3 + 1];
            s_head2[0] = "日期";

            int j = 1;
            for(int i = 0; i < s_roomId.Length;i++)
            {
                s_head2[j] = StrName.getRoomName(s_roomId[i]) + "消耗";
                s_head2[j + s_roomId.Length] = StrName.getRoomName(s_roomId[i]) + "杀鱼数量";
                s_head2[j + 2 * s_roomId.Length] = StrName.getRoomName(s_roomId[i]) + "掉落数量";
                j++;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_KILL_CRAB_ACT, Session, Response);
            if (!IsPostBack)
            {
                m_queryType.Items.Add("抽奖统计");
                m_queryType.Items.Add("场次统计");
                m_queryType.Items.Add("击杀统计");
                m_queryType.Items.Add("当前排行");
                m_queryType.Items.Add("历史排行");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();

            param.m_op = m_queryType.SelectedIndex;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = OpRes.op_res_failed;

            param.m_time = m_time.Text;

            switch (param.m_op)
            {
                case 0: //抽奖
                    res = mgr.doQuery(param, QueryType.queryTypeStatKillCrabActLottery, user);
                    genTable1(m_result, res, param, user, mgr);
                    break;
                case 1: //场次统计
                    res = mgr.doQuery(param, QueryType.queryTypeStatKillCrabActRoom, user);
                    genTable2(m_result, res, param, user, mgr);
                    break;
                case 2: //击杀统计
                    res = mgr.doQuery(param, QueryType.queryTypeStatKillCrabActTask, user);
                    genTable3(m_result, res, param, user, mgr);
                    break;
                case 3: // 当前排行
                    {
                        res = mgr.doQuery(param, QueryType.queryTypeStatKillCrabActTask, user);
                        genTableRankCur(m_result, res, user, s_headRankCur);
                    }
                    break;
                case 4: // 历史排行
                    {
                        res = mgr.doQuery(param, QueryType.queryTypeStatKillCrabActTask, user);
                        genTableRankHistory(m_result, res, param, user, s_headRankHistory);
                    }
                    break;
            }
        }

        //活动抽奖统计
        private void genTable1(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            string[] m_content = new string[s_head1.Length];
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
            List<StatKillCrabActLotteryList> qresult = (List<StatKillCrabActLotteryList>)mgr.getQueryResult(QueryType.queryTypeStatKillCrabActLottery);
            int i = 0;
            // 表头
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head1[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                StatKillCrabActLotteryList item = qresult[i];

                int m = 0;
                foreach (var da in item.m_data)
                {
                    tr = new TableRow();
                    m_result.Rows.Add(tr);

                    if (m == 0)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = da.m_time;
                        td.RowSpan = item.m_data.Count;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.getLotteryTypeName();
                    td.RowSpan = 1;

                    if (m == 0)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_goldIncome.ToString();
                        td.RowSpan = item.m_data.Count;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_dropCount.ToString();
                        td.RowSpan = item.m_data.Count;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";
                    }

                    //抽奖人数
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.m_lotteryPerson.ToString();
                    td.RowSpan = 1;

                    //抽奖人次
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.m_lotteryCount.ToString();
                    td.RowSpan = 1;

                    //少宝剑抽奖 多宝剑抽奖10004645
                    int index = da.m_lotteryType - 1;
                    for (int k = 0; k < 9; k++)
                    {
                        string str_content = "";
                        if (da.m_reward.ContainsKey(k))
                            str_content = da.m_reward[k].ToString();

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = str_content;
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.m_outlay.ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.getExParam(i);

                    m++;
                }
            }
        }

        //场次统计
        private void genTable2(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
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
            List<KillCrabActRoomStatList> qresult = (List<KillCrabActRoomStatList>)mgr.getQueryResult(QueryType.queryTypeStatKillCrabActRoom);
            int i = 0, k = 0, len = s_roomId.Length;
            // 表头
            for (i = 0; i < s_head2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head2[i];
                td.RowSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                KillCrabActRoomStatList item = qresult[i];

                tr = new TableRow();
                m_result.Rows.Add(tr);

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time;
                td.RowSpan = 1;

                for (k = 0; k < len; k++)
                {
                    int index = s_roomId[k];
                    td = new TableCell();
                    tr.Cells.Add(td);
                    if (item.m_data.ContainsKey(index))
                    {
                        td.Text = item.m_data[index].m_goldIncome.ToString();
                    }
                    else
                    {
                        td.Text = "";
                    }
                    td.RowSpan = 1;
                }

                for (k = 0; k < len; k++)
                {
                    int index = s_roomId[k];
                    td = new TableCell();
                    tr.Cells.Add(td);
                    if (item.m_data.ContainsKey(index))
                    {
                        td.Text = item.m_data[index].m_killCount.ToString();
                    }
                    else
                    {
                        td.Text = "";
                    }
                    td.RowSpan = 1;
                }

                for (k = 0; k < len; k++)
                {
                    int index = s_roomId[k];
                    td = new TableCell();
                    tr.Cells.Add(td);
                    if (item.m_data.ContainsKey(index))
                    {
                        td.Text = item.m_data[index].m_dropCount.ToString();
                    }
                    else
                    {
                        td.Text = "";
                    }
                    td.RowSpan = 1;
                }

            }
        }

        //任务统计
        private void genTable3(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            string[] m_content = new string[s_head3.Length];
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
            ResultKillAct qresult = (ResultKillAct)mgr.getQueryResult(QueryType.queryTypeStatKillCrabActTask);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head3.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head3[i];
            }

            for (i = 0; i < qresult.m_result.Count; i++)
            {
                KillActTaskItem item = qresult.m_result[i];

                tr = new TableRow();
                m_result.Rows.Add(tr);

                f = 0;
                m_content[f++] = item.m_time;
               
                foreach(var da in item.m_killCount)
                {
                    m_content[f++] = da.Value.ToString();
                }

                m_content[f++] = item.m_outlay.ToString();

                for (j = 0; j < s_head3.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                }
            }
        }

        private void genTableRankCur(Table table, OpRes res, GMUser user, string []head)
        {
            // m_page.InnerHtml = "";
            // m_foot.InnerHtml = "";

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

            string[] content = new string[head.Length];
            int i = 0, k = 0, n = 0;
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            ResultKillAct qresult = (ResultKillAct)user.getQueryResult(QueryType.queryTypeStatKillCrabActTask);

            for (i = 0; i < qresult.m_dataLine.m_rank.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                IntScoreRankInfo item = qresult.m_dataLine.m_rank[i];
                content[n++] = item.m_rank.ToString();
                content[n++] = item.m_nickName;
                content[n++] = item.m_playerId.ToString();
                content[n++] = item.m_socre.ToString();

                for (k = 0; k < head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        private void genTableRankHistory(Table table, OpRes res, ParamQuery param, GMUser user, string[] head)
        {
          //  m_page.InnerHtml = "";
          //  m_foot.InnerHtml = "";

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

            string[] content = new string[head.Length];

            int i = 0, k = 0, n = 0;
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            ResultKillAct qresult = (ResultKillAct)user.getQueryResult(QueryType.queryTypeStatKillCrabActTask);

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
                    content[n++] = time.ToShortDateString();

                    content[n++] = item.m_rank.ToString();
                    content[n++] = item.m_nickName;
                    content[n++] = item.m_playerId.ToString();
                    content[n++] = item.m_socre.ToString();

                    for (k = 0; k < head.Length; k++)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = content[k];
                    }
                }
            }

         //   param.m_op = m_optional.SelectedIndex;
         //   string page_html = "", foot_html = "";
         //   m_gen.genPage(param, @"/appaspx/stat/StatIntScoreSendAward.aspx", ref page_html, ref foot_html, user);
         //   m_page.InnerHtml = page_html;
         //   m_foot.InnerHtml = foot_html;
        }

    }
}