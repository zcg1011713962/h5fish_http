using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatActivityFishingCelebration : System.Web.UI.Page
    {
        private static string[] s_headRankCur = new string[] { "排行", "玩家名称", "玩家ID", "积分" };
        private static string[] s_headRankHistroy = new string[] { "赛季", "排行", "玩家名称", "玩家ID", "积分" };

        const int ITEM_COL_COUNT_GIFT = 14;
        const int OTHER_COL_COUTN_GIFT = 1;
        private static string[] s_headGift;

        const int ITEM_COL_COUNT_EXCHANGE = 11;
        const int OTHER_COL_COUTN_EXCHANGE = 2;
        private static string[] s_headExchange;

        const int ITEM_COL_COUNT_LOTTERY = 6;
        const int OTHER_COL_COUNT_LOTTERY = 3;
        private static string[] s_headLottery;

        const int ITEM_COL_COUNT_PAOPAO = 6;
        const int OTHER_COL_COUNT_PAOPAO = 2;
        private static string[] s_headPaoPao;

        const int ITEM_COL_COUNT_DIAL_PAN = 18;
        const int OTHER_COL_COUNT_DIAL_PAN = 1;
        private static string[] s_headDialPan;

        static StatActivityFishingCelebration()
        {
            s_headGift = new string[ITEM_COL_COUNT_GIFT + OTHER_COL_COUTN_GIFT];
            s_headGift[0] = "日期";
            for (int i = 0; i < ITEM_COL_COUNT_GIFT / 2; i++)
            {
                s_headGift[i + OTHER_COL_COUTN_GIFT] = "福袋" + (i + 1).ToString();
                s_headGift[i + OTHER_COL_COUTN_GIFT + 7] = "礼包" + (i + 1).ToString();
            }

            s_headExchange = new string[ITEM_COL_COUNT_EXCHANGE + OTHER_COL_COUTN_EXCHANGE];
            s_headExchange[0] = "日期";
            s_headExchange[1] = "阶段";
            for (int i = 0; i < ITEM_COL_COUNT_EXCHANGE; i++)
            {
                s_headExchange[i + OTHER_COL_COUTN_EXCHANGE] = "道具" + (i + 1).ToString();
            }

            s_headLottery = new string[ITEM_COL_COUNT_LOTTERY + OTHER_COL_COUNT_LOTTERY];
            s_headLottery[0] = "日期";
            s_headLottery[1] = "阶段";
            s_headLottery[2] = "激活奖励";
            for (int i = 0; i < ITEM_COL_COUNT_LOTTERY; i++)
            {
                s_headLottery[OTHER_COL_COUNT_LOTTERY + i] = "道具" + (i + 1).ToString();
            }

            s_headPaoPao = new string[ITEM_COL_COUNT_PAOPAO + OTHER_COL_COUNT_PAOPAO];
            s_headPaoPao[0] = "日期";
            s_headPaoPao[1] = "积分产出数量";
            for (int i = 0; i < ITEM_COL_COUNT_PAOPAO; i++)
            {
                s_headPaoPao[OTHER_COL_COUNT_PAOPAO + i] = "兑换" + (i + 1).ToString();
            }

            s_headDialPan = new string[ITEM_COL_COUNT_DIAL_PAN + OTHER_COL_COUNT_DIAL_PAN];
            s_headDialPan[0] = "日期";
            for (int i = 0; i < ITEM_COL_COUNT_DIAL_PAN/3; i++)
            {
                s_headDialPan[OTHER_COL_COUNT_DIAL_PAN + i] = "初级" + (i + 1).ToString();
                s_headDialPan[OTHER_COL_COUNT_DIAL_PAN + 6 + i] = "中级" + (i + 1).ToString();
                s_headDialPan[OTHER_COL_COUNT_DIAL_PAN + 12 + i] = "高级" + (i + 1).ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_FISH_CELE, Session, Response);

            if (!IsPostBack)
            {
                m_optional.Items.Add(new ListItem("抽奖", "1"));
                m_optional.Items.Add(new ListItem("庆典券兑换", "2"));
                m_optional.Items.Add(new ListItem("泡泡积分统计", "3"));
                m_optional.Items.Add(new ListItem("幸运转盘统计", "4"));
                m_optional.Items.Add(new ListItem("礼包统计", "5"));
                m_optional.Items.Add(new ListItem("当前排行榜", "6"));
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = Convert.ToInt32(m_optional.SelectedValue);
            param.m_time = m_time.Text;

            OpRes res = user.doQuery(param, QueryType.queryFishCelebration);
            switch (param.m_op)
            {
                case 1: // 抽奖
                    {
                        param.m_param = "";
                        genTableLottery(m_result, res, user, s_headLottery, param);
                    }
                    break;
                case 2: // 庆典券兑换
                    genTableExchange(m_result, res, user, s_headExchange, "", 333);
                    break;
                case 3: // 泡泡积分统计
                    genTablePaoPaoScore(m_result, res, user, s_headPaoPao, "", 333);
                    break;
                case 4: // 幸运转盘
                    genTableDialPan(m_result, res, user, s_headDialPan, "", 333);
                    break;
                case 5: // 礼包统计
                    genTableGiftBuy(m_result, res, user, s_headGift, "", 354);
                    break;
                case 6: // 排行榜
                    {
                        param.m_param = "";
                        genTableRankCur(m_result, res, user, s_headRankCur, param);
                    }
                    break;
                case 7:
                    {
                        genTableRankHistory(m_result, res, user, s_headRankHistroy, param);
                    }
                    break;
            }
        }

        private void genTableLottery(Table table, OpRes res, GMUser user, string[] head, ParamQuery param)
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
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            string[] content = new string[head.Length];

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryFishCelebration);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();
                content[n++] = item.m_data.getValue(1).ToString();
                content[n++] = item.m_data.getValue(0).ToString();

                for (int m = 0; m < ITEM_COL_COUNT_LOTTERY; m++)
                {
                    if (item.m_mapCount.m_data.ContainsKey(m))
                    {
                        content[n++] = item.m_mapCount.m_data[m].m_count.ToString();
                    }
                    else
                    {
                        content[n++] = "0";
                    }
                }

                for (k = 0; k < head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        private void genTableExchange(Table table, OpRes res, GMUser user, string[] head, string title, int from)
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
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            string[] content = new string[head.Length];

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryFishCelebration);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();
                content[n++] = item.m_data.getValue(0).ToString();

                for (int m = 1; m <= ITEM_COL_COUNT_EXCHANGE; m++)
                {
                    if (item.m_mapCount.m_data.ContainsKey(m))
                    {
                        content[n++] = item.m_mapCount.m_data[m].m_count.ToString();
                    }
                    else
                    {
                        content[n++] = "0";
                    }
                }

                for (k = 0; k < head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        private void genTablePaoPaoScore(Table table, OpRes res, GMUser user, string[] head, string title, int from)
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
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            string[] content = new string[head.Length];

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryFishCelebration);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();
                content[n++] = item.m_data.getValue(0).ToString();

                for (int m = 1; m <= ITEM_COL_COUNT_PAOPAO; m++)
                {
                    if (item.m_mapCount.m_data.ContainsKey(m))
                    {
                        content[n++] = item.m_mapCount.m_data[m].m_count.ToString();
                    }
                    else
                    {
                        content[n++] = "0";
                    }
                }

                for (k = 0; k < head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        private void genTableDialPan(Table table, OpRes res, GMUser user, string[] head, string title, int from)
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
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            string[] content = new string[head.Length];

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryFishCelebration);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();

                for (int m = 1; m <= ITEM_COL_COUNT_DIAL_PAN; m++)
                {
                    if (item.m_mapCount.m_data.ContainsKey(m))
                    {
                        content[n++] = item.m_mapCount.m_data[m].m_count.ToString();
                    }
                    else
                    {
                        content[n++] = "0";
                    }
                }

                for (k = 0; k < head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        private void genTableGiftBuy(Table table, OpRes res, GMUser user, string[] head, string title, int from)
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
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            string[] content = new string[head.Length];

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryFishCelebration);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();

                int giftId = from;
                for (int m = 1; m <= ITEM_COL_COUNT_GIFT; m++)
                {
                    if (item.m_mapCount.m_data.ContainsKey(giftId))
                    {
                        content[n++] = item.m_mapCount.m_data[giftId].m_count.ToString();
                    }
                    else
                    {
                        content[n++] = "0";
                    }
                    giftId++;
                }

                for (k = 0; k < head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        private void genTableRankCur(Table table, OpRes res, GMUser user, string[] head, ParamQuery param)
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
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            string[] content = new string[head.Length];

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryFishCelebration);

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

        private void genTableRankHistory(Table table, OpRes res, GMUser user, string[] head, ParamQuery param)
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
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            string[] content = new string[head.Length];
            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryTypeTouchFish);

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
                    content[n++] = param.m_param == "" ? "当前赛季" : param.m_param;

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
        }
    }
}