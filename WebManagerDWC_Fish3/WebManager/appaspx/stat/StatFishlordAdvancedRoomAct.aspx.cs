using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordAdvancedRoomAct : System.Web.UI.Page
    {
        // private static string[] s_head1 = new string[] { "时间","排名","玩家ID","昵称","积分"};
        //  private static string[] s_head = new string[] { "日期","抽奖人数","抽奖次数","奖池收入","奖池支出","大奖支出","系统回收","一等奖","二等奖","三等奖","详情"};

        static string[] s_headPlayFish = new string[] { "日期", "玩法鱼收入", "玩法鱼支出", "玩法鱼奖池收入", "玩法鱼奖池支出", "奖池支出次数" };
        static string[] s_headDaSheng = new string[] { "日期", "大圣收入", "大圣支出", "大圣奖池收入", "大圣奖池支出", "奖池支出次数", "风产出数量", "雨产出数量", "雷产出数量", "电产出数量" };

        private static string[] s_headRankDayCur = new string[] { "排名", "玩家ID", "昵称", "积分" };
        private static string[] s_headRankWeekCur = new string[] { "排名", "玩家ID", "昵称", "积分" };

        private static string[] s_headRankDayHistory = new string[] {"日期", "排名", "玩家ID", "昵称", "积分" };
        private static string[] s_headRankWeekHistory = new string[] { "日期", "排名", "玩家ID", "昵称", "积分" };

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_ADVANCED_ROOM_ACT, Session, Response);
            if (!IsPostBack)
            {
                m_queryType.Items.Add(new ListItem("奖池统计", DefCC.QRY_LOTTERY.ToString()));
                m_queryType.Items.Add(new ListItem("当前日榜", (DefCC.QRY_RANK_CUR).ToString()));
                int v = ItemHelp.genOpType(DefCC.QRY_RANK_CUR, 1);
                m_queryType.Items.Add(new ListItem("当前周榜", v.ToString()));

                m_queryType.Items.Add(new ListItem("历史日榜", (DefCC.QRY_RANK_HISTORY).ToString()));
                v = ItemHelp.genOpType(DefCC.QRY_RANK_HISTORY, 1);
                m_queryType.Items.Add(new ListItem("历史周榜", v.ToString()));
                //   m_queryType.Items.Add("奖池统计");
                // m_queryType.Items.Add("排行榜统计");

                // m_rankType.Items.Add(new ListItem("当前活动"));
                // m_rankType.Items.Add(new ListItem("历史排行"));
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();

            param.m_op = Convert.ToInt32(m_queryType.SelectedValue);
            param.m_time = m_time.Text;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatFishlordAdvancedRoomActRank, user);
            int op = 0, type = 0;
            ItemHelp.parseOpType(param.m_op, ref op, ref type);

            switch (op)
            {
                case DefCC.QRY_LOTTERY:
                    {
                        genTableRewardPoolPlayFish(m_result, res, user, s_headPlayFish, "玩法鱼统计");
                        genTableRewardPoolDaSheng(m_result1, res, user, s_headDaSheng, "大圣统计");
                    }
                    break;
                case DefCC.QRY_RANK_CUR:
                    {
                        if (type == 0)
                        {
                            genTableRankDayCur(m_result, res, user, s_headRankDayCur, "");
                        }
                        else
                        {
                            genTableRankWeekCur(m_result, res, user, s_headRankWeekCur, "");
                        }
                    }
                    break;
                case DefCC.QRY_RANK_HISTORY:
                    {
                        if (type == 0)
                        {
                            genTableRankHistory(m_result, res, user, s_headRankDayHistory, "");
                        }
                        else
                        {
                            genTableRankHistory(m_result, res, user, s_headRankWeekHistory, "");
                        }
                    }
                    break;
                    /* case 0:
                         param.m_param = m_rankType.SelectedIndex.ToString(); //0 当前  1历史
                         if (Convert.ToInt32(param.m_param) == 1)
                             param.m_time = m_time.Text;

                         res = mgr.doQuery(param, QueryType.queryTypeStatFishlordAdvancedRoomActRank, user);
                         genTable0(m_result, res, user, param);
                         break;
                     case 1:
                         param.m_param = m_time.Text;
                         res = mgr.doQuery(param, QueryType.queryTypeStatFishlordAdvancedRoomAct, user);
                         genTable1(m_result, res, user, param);
                         break;*/
            }
        }

        //统计表
        /*private void genTable0(Table table, OpRes res, GMUser user, ParamQuery param)
        {
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

            List<StatFishlordAdvancedRoomActRankItem> qresult =
                (List<StatFishlordAdvancedRoomActRankItem>)user.getQueryResult(QueryType.queryTypeStatFishlordAdvancedRoomActRank);

            int i = 0, j = 0, f = 0;
            string[] m_content = new string[s_head1.Length];

            // 表头
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head1[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                StatFishlordAdvancedRoomActRankItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_rankId.ToString();
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_score.ToString();

                tr = new TableRow();
                table.Rows.Add(tr);
                for (j = 0; j < s_head1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }

        //统计表
        private void genTable1(Table table, OpRes res, GMUser user, ParamQuery param)
        {
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

            List<StatFishlordAdvancedRoomActItem> qresult = (List<StatFishlordAdvancedRoomActItem>)user.getQueryResult(QueryType.queryTypeStatFishlordAdvancedRoomAct);

            int i = 0, j = 0, f = 0;
            string[] m_content = new string[s_head.Length];
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                StatFishlordAdvancedRoomActItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_lotteryPerson.ToString();
                m_content[f++] = item.m_lotteryCount.ToString();
                m_content[f++] = item.m_poolIncome.ToString();
                m_content[f++] = item.m_poolOutlay.ToString();
                m_content[f++] = item.m_dailyAdvanceRoomGrandPriceOutlay.ToString();//大奖支出
                m_content[f++] = item.m_recycleGold.ToString();
                m_content[f++] = item.m_level1.ToString();
                m_content[f++] = item.m_level2.ToString();
                m_content[f++] = item.m_level3.ToString();
                m_content[f++] = i == 0 ? " " : item.getDetail();

                tr = new TableRow();
                table.Rows.Add(tr);
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }
        */

        private void genTableRewardPoolPlayFish(Table table, OpRes res, GMUser user, string[] head, string title)
        {
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

            td = new TableCell();
            td.Text = title;
            td.ColumnSpan = head.Length;
            tr.Cells.Add(td);

            tr = new TableRow();
            table.Rows.Add(tr);

            int i = 0, k = 0, n = 0;
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            ResultDaShengRoom qresult = (ResultDaShengRoom)user.getQueryResult(QueryType.queryTypeStatFishlordAdvancedRoomActRank);

            for (i = 0; i < qresult.m_items.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                DaShengRoomItem item = qresult.m_items[i];
                content[n++] = item.m_time.ToShortDateString();
                content[n++] = item.m_monkeyFishIncome.ToString();
                content[n++] = item.m_monkeyFishOutlay.ToString();
                content[n++] = item.m_monkeyFishJackpotIncome.ToString();
                content[n++] = item.m_monkeyFishJackpotOutlay.ToString();
                content[n++] = item.m_monkeyFishJackpotTimes.ToString();

                for (k = 0; k < head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        private void genTableRewardPoolDaSheng(Table table, OpRes res, GMUser user, string[] head, string title)
        {
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

            td = new TableCell();
            td.Text = title;
            td.ColumnSpan = head.Length;
            tr.Cells.Add(td);

            tr = new TableRow();
            table.Rows.Add(tr);

            int i = 0, k = 0, n = 0;
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            ResultDaShengRoom qresult = (ResultDaShengRoom)user.getQueryResult(QueryType.queryTypeStatFishlordAdvancedRoomActRank);

            for (i = 0; i < qresult.m_items.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                DaShengRoomItem item = qresult.m_items[i];
                content[n++] = item.m_time.ToShortDateString();
                content[n++] = item.m_monkeyIncome.ToString();
                content[n++] = item.m_monkeyOutlay.ToString();
                content[n++] = item.m_monkeyJackpotIncome.ToString();
                content[n++] = item.m_monkeyJackpotOutlay.ToString();
                content[n++] = item.m_monkeyJackpotTimes.ToString();

                content[n++] = item.m_fengDropTimes.ToString();
                content[n++] = item.m_yuDropTimes.ToString();
                content[n++] = item.m_leiDropTimes.ToString();
                content[n++] = item.m_dianDropTimes.ToString();

                for (k = 0; k < head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        private void genTableRankDayCur(Table table, OpRes res, GMUser user, string[] head, string title)
        {
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

            ResultDaShengRoom qresult = (ResultDaShengRoom)user.getQueryResult(QueryType.queryTypeStatFishlordAdvancedRoomActRank);

            for (i = 0; i < qresult.m_dataLine.m_rank.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                IntScoreRankInfo item = qresult.m_dataLine.m_rank[i];
                content[n++] = item.m_rank.ToString();
                content[n++] = item.m_playerId.ToString();
                content[n++] = item.m_nickName;
                content[n++] = item.m_socre.ToString();
                
                for (k = 0; k < head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        private void genTableRankWeekCur(Table table, OpRes res, GMUser user, string[] head, string title)
        {
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

            ResultDaShengRoom qresult = (ResultDaShengRoom)user.getQueryResult(QueryType.queryTypeStatFishlordAdvancedRoomActRank);

            for (i = 0; i < qresult.m_dataLine.m_rank.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                IntScoreRankInfo item = qresult.m_dataLine.m_rank[i];
                content[n++] = item.m_rank.ToString();
                content[n++] = item.m_playerId.ToString();
                content[n++] = item.m_nickName;
                content[n++] = item.m_socre.ToString();

                for (k = 0; k < head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        private void genTableRankHistory(Table table, OpRes res, GMUser user, string[] head, string title)
        {
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

            ResultDaShengRoom qresult = (ResultDaShengRoom)user.getQueryResult(QueryType.queryTypeStatFishlordAdvancedRoomActRank);

            foreach(var dinfo in qresult.m_dataDic)
            {
                var time = dinfo.Key;
                var rankList = dinfo.Value.m_rank;

                for (i = 0; i < rankList.Count; i++)
                {
                    n = 0;

                    tr = new TableRow();
                    table.Rows.Add(tr);

                    IntScoreRankInfo item = rankList[i];
                    content[n++] = time.ToShortDateString();
                    content[n++] = item.m_rank.ToString();
                    content[n++] = item.m_playerId.ToString();
                    content[n++] = item.m_nickName;
                    content[n++] = item.m_socre.ToString();

                    for (k = 0; k < head.Length; k++)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = content[k];
                    }
                }
            }
        }

    }
}