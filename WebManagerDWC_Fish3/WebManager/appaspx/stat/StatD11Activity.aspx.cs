using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatD11Activity : System.Web.UI.Page
    {
        const int ITEM_COL_COUNT_LOTTERY = 11;
        const int OTHER_COL_COUTN__LOTTERY = 1;
        private static string[] s_headLottery;

        const int ITEM_COL_COUNT_TASK = 5;
        const int OTHER_COL_COUTN__TASK = 1;
        private static string[] s_headTask;

        const int ITEM_COL_COUNT_RECHARGE = 7;
        const int OTHER_COL_COUTN__RECHARGE = 1;
        private static string[] s_headRecharge;

        private static string[] s_headRankCur = new string[] { "排行", "玩家名称", "玩家ID", "积分" };
        private static string[] s_headRankHistroy = new string[] { "日期", "排行", "玩家名称", "玩家ID", "积分" };

        const int ITEM_COL_COUNT_TIME_PANIC = 3;
        const int ITEM_COL_COUNT_GIFT_PANIC = 3;
        const int OTHER_COL_COUTN_PANIC = 1;
        private static string[] s_headPanic; // new string[] { "日期", "时间段", "抢购1人数", "抢购2人数", "抢购3人数" };

        private static string[] s_timePerios = { "13:00", "18:00", "22:00" };

        public static string getTimeRange(int timeId)
        {
            return s_timePerios[timeId];
        }

        static StatD11Activity()
        {
            s_headLottery = new string[ITEM_COL_COUNT_LOTTERY + OTHER_COL_COUTN__LOTTERY];
            s_headLottery[0] = "日期";
            for (int i = 0; i < ITEM_COL_COUNT_LOTTERY; i++)
            {
                s_headLottery[i + OTHER_COL_COUTN__LOTTERY] = "奖励" + (i + 1).ToString() + "次数";
            }

            s_headTask = new string[ITEM_COL_COUNT_TASK + OTHER_COL_COUTN__TASK];
            s_headTask[0] = "日期";
            for (int i = 0; i < ITEM_COL_COUNT_TASK; i++)
            {
                s_headTask[i + OTHER_COL_COUTN__TASK] = "任务" + (i + 1).ToString() + "人数";
            }

            s_headRecharge = new string[ITEM_COL_COUNT_RECHARGE + OTHER_COL_COUTN__RECHARGE];
            s_headRecharge[0] = "日期";
            for (int i = 0; i < ITEM_COL_COUNT_RECHARGE; i++)
            {
                s_headRecharge[i + OTHER_COL_COUTN__RECHARGE] = "充值" + (i + 1).ToString() + "人数";
            }

            s_headPanic = new string[ITEM_COL_COUNT_TIME_PANIC * ITEM_COL_COUNT_GIFT_PANIC + OTHER_COL_COUTN_PANIC];
            s_headPanic[0] = "日期";
            int k = 1;
            for (int i = 1; i <= ITEM_COL_COUNT_TIME_PANIC; i++)
            {
                for(int j = 1; j <= ITEM_COL_COUNT_GIFT_PANIC; j++)
                {
                    s_headPanic[k] = "时间段" + (i).ToString() + " 抢购" + (j) + "人数";
                    k++;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_SAILING_FES, Session, Response);

            if (!IsPostBack)
            {
                m_optional.Items.Add(new ListItem("抽奖", "1"));
                m_optional.Items.Add(new ListItem("任务", "2"));
                m_optional.Items.Add(new ListItem("累充", "3"));
                m_optional.Items.Add(new ListItem("抢购", "4"));
                m_optional.Items.Add(new ListItem("当前排行", "5"));
                m_optional.Items.Add(new ListItem("历史排行", "6"));
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = Convert.ToInt32(m_optional.SelectedValue);
            param.m_time = m_time.Text;

            OpRes res = user.doQuery(param, QueryType.queryTypeD11Activity);
            switch (param.m_op)
            {
                case 1:
                    genTableLottery(m_result, res, user, s_headLottery, "", 1);
                    break;
                case 2:
                    genTableTask(m_result, res, user, s_headTask, "", 1);
                    break;
                case 3:
                    genTableAccRecharge(m_result, res, user, s_headRecharge, "", 1);
                    break;
                case 4:
                    genTablePanic(m_result, res, user, s_headPanic, "", 1);
                    break;
                case 5:
                    genTableRankCur(m_result, res, user, s_headRankCur);
                    break;
                case 6:
                    genTableRankHistory(m_result, res, user, s_headRankHistroy);
                    break;
            }
        }

        private void genTableLottery(Table table, OpRes res, GMUser user, string[] head, string title, int from)
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

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryTypeD11Activity);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();

                for (int m = 1; m <= ITEM_COL_COUNT_LOTTERY; m++)
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

        private void genTableAccRecharge(Table table, OpRes res, GMUser user, string[] head, string title, int from)
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

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryTypeD11Activity);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();

                for (int m = 1; m <= ITEM_COL_COUNT_RECHARGE; m++)
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

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryTypeD11Activity);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();

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

        private void genTablePanic(Table table, OpRes res, GMUser user, string[] head, string title, int from)
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

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryTypeD11Activity);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();

                for (int m = 1; m <= ITEM_COL_COUNT_TIME_PANIC; m++)
                {
                    for( int w = 1;w<= ITEM_COL_COUNT_GIFT_PANIC;w++)
                    {
                        int kk = m * 10 + w;
                        if (item.m_mapCount.m_data.ContainsKey(kk))
                        {
                            content[n++] = item.m_mapCount.m_data[kk].m_count.ToString();
                        }
                        else
                        {
                            content[n++] = "0";
                        }
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

        private void genTableRankCur(Table table, OpRes res, GMUser user, string[] head)
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

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryTypeD11Activity);

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

        private void genTableRankHistory(Table table, OpRes res, GMUser user, string[] head)
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
            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryTypeD11Activity);

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
        }





    }
}