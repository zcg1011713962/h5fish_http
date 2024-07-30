using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatActivityTouchFish : System.Web.UI.Page
    {
        private static string[] s_headRankCur = new string[] { "排行", "玩家名称", "玩家ID", "积分" };
        private static string[] s_headRankHistroy = new string[] { "赛季", "排行", "玩家名称", "玩家ID", "积分" };

        private static string[] s_headLevelDistri; // new string[] { "赛季", "类型", "1-10级", "11-20级", "21-30级", "3" };

        const int ITEM_COL_COUNT_GIFT = 3;
        const int OTHER_COL_COUTN_GIFT = 1;
        private static string[] s_headGift;

        const int ITEM_COL_COUNT_TASK = 11;
        const int OTHER_COL_COUTN_TASK = 1;
        private static string[] s_headTask;

        const int ITEM_COL_COUNT_LEVEL = 12;
        const int OTHER_COL_COUNT_LEVEL = 2;

        static StatActivityTouchFish()
        {
            s_headGift = new string[ITEM_COL_COUNT_GIFT + OTHER_COL_COUTN_GIFT];
            s_headGift[0] = "日期";
            for (int i = 0; i < ITEM_COL_COUNT_GIFT; i++)
            {
                s_headGift[i + OTHER_COL_COUTN_GIFT] = "礼包" + (i + 1).ToString() + "购买次数";
            }

            s_headTask = new string[ITEM_COL_COUNT_TASK + OTHER_COL_COUTN_TASK];
            s_headTask[0] = "类型";
            for (int i = 0; i < ITEM_COL_COUNT_TASK; i++)
            {
                s_headTask[i + OTHER_COL_COUTN_TASK] = "任务" + (i + 1).ToString();
            }

            s_headLevelDistri = new string[ITEM_COL_COUNT_LEVEL + OTHER_COL_COUNT_LEVEL];
            s_headLevelDistri[0] = "赛季";
            s_headLevelDistri[1] = "类型";
            for (int i = 1, j = 0; i <= ITEM_COL_COUNT_LEVEL * 10; i += 10, j++)
            {
                s_headLevelDistri[OTHER_COL_COUNT_LEVEL + j] = i.ToString() + "-" + (i + 9).ToString() + "级";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_TOUCH_FISH, Session, Response);

            if (!IsPostBack)
            {
                m_optional.Items.Add(new ListItem("当前赛季等级分布", "1"));
                m_optional.Items.Add(new ListItem("历史赛季等级分布", "2"));
                m_optional.Items.Add(new ListItem("当前赛季任务", "3"));
                m_optional.Items.Add(new ListItem("历史赛季任务", "4"));

                m_optional.Items.Add(new ListItem("礼包购买", "5"));
                m_optional.Items.Add(new ListItem("当前排行", "6"));
                m_optional.Items.Add(new ListItem("历史排行", "7"));
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = Convert.ToInt32(m_optional.SelectedValue);
            param.m_param = m_season.Text;  // 赛季
            param.m_time = m_time.Text;

            OpRes res = user.doQuery(param, QueryType.queryTypeTouchFish);
            switch (param.m_op)
            {
                case 1: // 当前赛季等级
                    {
                        param.m_param = "";
                        genTableLevel(m_result, res, user, s_headLevelDistri, param);
                    }
                    break;
                case 2: // 历史赛季等级
                    genTableLevel(m_result, res, user, s_headLevelDistri, param);
                    break;
                case 3: // 当前赛季任务
                    genTableTask(m_result, res, user, s_headTask, "", 333);
                    break;
                case 4: // 历史赛季任务
                    genTableTask(m_result, res, user, s_headTask, "", 333);
                    break;
                case 5:
                    genTableGiftBuy(m_result, res, user, s_headGift, "", 333);
                    break;
                case 6:
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

        private void genTableLevel(Table table, OpRes res, GMUser user, string[] head, ParamQuery param)
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

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = param.m_param == "" ? "当前赛季" : param.m_param;
                content[n++] = item.m_data.getValue(0) == 1 ? "进阶" : "普通";

                for (int m = 0; m < ITEM_COL_COUNT_LEVEL; m++)
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

        private void genTableTask(Table table, OpRes res, GMUser user, string[] head, string title, int from)
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

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_data.getValue(0) == 1 ? "进阶" : "普通";

                for (int m = 1; m <= ITEM_COL_COUNT_TASK; m++)
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

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryTypeTouchFish);

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

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryTypeTouchFish);

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