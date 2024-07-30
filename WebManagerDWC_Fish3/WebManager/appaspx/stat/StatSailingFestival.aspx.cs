using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatSailingFestival : System.Web.UI.Page
    {
        const int ITEM_COL_COUNT_SIGN = 7;
        const int OTHER_COL_COUTN_SIGN = 3;
        private static string[] s_headSign;

        const int ITEM_COL_COUNT_VIP = 4;
        const int OTHER_COL_COUTN_VIP = 3;
        private static string[] s_headVip;

        const int ITEM_COL_COUNT_ACHIVE = 10;
        const int ITEM_COL_COUNT_ACHIVE_1 = 70;
        const int ITEM_COL_COUNT_ACHIVE_2 = 65;
        const int OTHER_COL_COUTN_ACHIVE = 2;
        private static string[] s_headAchive;
        private static string[] s_headAchive1;
        private static string[] s_headAchive2;

        const int ITEM_COL_COUNT_ACHIVE_SHOP = 12;
        const int OTHER_COL_COUTN_ACHIVE_SHOP = 1;
        private static string[] s_headAchiveShop;

        const int ITEM_COL_COUNT_ACHIVE_LEVEL = 50;
        const int OTHER_COL_COUTN_ACHIVE_LEVEL = 2;
        private static string[] s_headAchiveLevel;

        const int ITEM_COL_COUNT_TREASURE = 6;
        const int OTHER_COL_COUTN_TREASURE = 1;
        private static string[] s_headTreasure;

        const int ITEM_COL_COUNT_ACC_OPEN = 6;
        const int OTHER_COL_COUTN_ACC_OPEN = 1;
        private static string[] s_headAccOpen;

        static int []ACC_OPEN = new int[] { 3, 5, 7, 15, 21, 28 };

        static StatSailingFestival()
        {
            s_headSign = new string[OTHER_COL_COUTN_SIGN + ITEM_COL_COUNT_SIGN * 2];
            s_headSign[0] = "日期";
            s_headSign[1] = "注册人数";
            s_headSign[2] = "金币总支出";
            for (int i = 0; i < ITEM_COL_COUNT_SIGN; i++)
            {
                s_headSign[i + OTHER_COL_COUTN_SIGN] = "签到" + (i + 1).ToString() + "人数";
                s_headSign[i + OTHER_COL_COUTN_SIGN + ITEM_COL_COUNT_SIGN] = "补卡" + (i + 1).ToString() + "人数";
            }

            s_headVip = new string[OTHER_COL_COUTN_VIP + ITEM_COL_COUNT_VIP];
            s_headVip[0] = "日期";
            s_headVip[1] = "活跃人数";
            s_headVip[2] = "金币总支出";
            for (int i = 0; i < ITEM_COL_COUNT_VIP; i++)
            {
                s_headVip[i + OTHER_COL_COUTN_VIP] = "vip" + (i).ToString() + "完成人数";
            }

            s_headAchive = new string[ITEM_COL_COUNT_ACHIVE + OTHER_COL_COUTN_ACHIVE-1];
            s_headAchive[0] = "日期";
           // s_headAchive[1] = "金币总支出";
            for (int i = 0; i < ITEM_COL_COUNT_ACHIVE; i++)
            {
                s_headAchive[i + OTHER_COL_COUTN_ACHIVE-1] = "任务" + (i + 1).ToString() + "完成人数";
            }
            s_headAchive1 = new string[ITEM_COL_COUNT_ACHIVE_1 + OTHER_COL_COUTN_ACHIVE];
            s_headAchive1[0] = "日期";
            s_headAchive1[1] = "金币总支出";
            for (int i = 0; i < ITEM_COL_COUNT_ACHIVE_1; i++)
            {
                s_headAchive1[i + OTHER_COL_COUTN_ACHIVE] = "任务" + (i + 1).ToString() + "完成人数";
            }
            s_headAchive2 = new string[ITEM_COL_COUNT_ACHIVE_2 + OTHER_COL_COUTN_ACHIVE];
            s_headAchive2[0] = "日期";
            s_headAchive2[1] = "金币总支出";
            for (int i = 0; i < ITEM_COL_COUNT_ACHIVE_2; i++)
            {
                s_headAchive2[i + OTHER_COL_COUTN_ACHIVE] = "任务" + (i + 1).ToString() + "完成人数";
            }

            s_headAchiveShop = new string[ITEM_COL_COUNT_ACHIVE_SHOP + OTHER_COL_COUTN_ACHIVE_SHOP];
            s_headAchiveShop[0] = "日期";
            for (int i = 0; i < ITEM_COL_COUNT_ACHIVE_SHOP; i++)
            {
                s_headAchiveShop[i + OTHER_COL_COUTN_ACHIVE_SHOP] = "奖品" + (i + 1).ToString() + "产出";
            }

            s_headAchiveLevel = new string[ITEM_COL_COUNT_ACHIVE_LEVEL + OTHER_COL_COUTN_ACHIVE_LEVEL];
            s_headAchiveLevel[0] = "日期";
            s_headAchiveLevel[1] = "金币总支出";
            for (int i = 0; i < ITEM_COL_COUNT_ACHIVE_LEVEL; i++)
            {
                s_headAchiveLevel[i + OTHER_COL_COUTN_ACHIVE_LEVEL] = "等级" + (i + 1).ToString() + "人数";
            }

            s_headTreasure = new string[ITEM_COL_COUNT_TREASURE + OTHER_COL_COUTN_TREASURE];
            s_headTreasure[0] = "日期";
            for (int i = 0; i < ITEM_COL_COUNT_TREASURE; i++)
            {
                s_headTreasure[i + OTHER_COL_COUTN_TREASURE] = "领取vip" + (i + 1).ToString() + "人数";
            }

            s_headAccOpen = new string[ITEM_COL_COUNT_ACC_OPEN + OTHER_COL_COUTN_ACC_OPEN];
            s_headAccOpen[0] = "日期";
            for (int i = 0; i < ITEM_COL_COUNT_ACC_OPEN; i++)
            {
                s_headAccOpen[i + OTHER_COL_COUTN_ACC_OPEN] = ACC_OPEN[i] + "天人数";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_SAILING_FES, Session, Response);

            if (!IsPostBack)
            {
                m_optional.Items.Add(new ListItem("打卡情况", "1"));
                m_optional.Items.Add(new ListItem("招财进宝", "2"));
                m_optional.Items.Add(new ListItem("成就", "3"));
                m_optional.Items.Add(new ListItem("成就商店", "4"));
                m_optional.Items.Add(new ListItem("成就等级", "5"));
                m_optional.Items.Add(new ListItem("海盗宝藏", "6"));
                m_optional.Items.Add(new ListItem("累计打开", "7"));
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = Convert.ToInt32(m_optional.SelectedValue);
            param.m_time = m_time.Text;
           
            OpRes res = user.doQuery(param, QueryType.queryTypeSailingFestival);
            switch (param.m_op)
            {
                case 1:
                    genTableSign(m_result, res, user, s_headSign, "杨帆起航", 1);
                    genTableSign(m_result1, res, user, s_headSign, "乘风破浪", 8);
                    genTableSign(m_result2, res, user, s_headSign, "纵横四海", 15);
                    genTableSign(m_result3, res, user, s_headSign, "海洋霸主", 22);
                    break;
                case 2:
                    genTabeVip(m_result, res, user, s_headVip);
                    break;
                case 3:
                    genTabeAchive(m_result, res, user, s_headAchive, "每日成就", 1, 10);
                    genTabeAchive(m_result1, res, user, s_headAchive1, "捕鱼成就", 11, 80);
                    genTabeAchive(m_result2, res, user, s_headAchive2, "黄金鱼成就", 81, 145);
                    break;
                case 4:
                    genTabeAchiveShop(m_result, res, user, s_headAchiveShop);
                    break;
                case 5:
                    genTabeAchiveLevel(m_result, res, user, s_headAchiveLevel);
                    break;
                case 6:
                    genTabeTreasure(m_result, res, user, s_headTreasure);
                    break;
                case 7:
                    genTableAccOpen(m_result, res, user, s_headAccOpen);
                    break;
            }
        }

        // 打卡
        private void genTableSign(Table table, OpRes res, GMUser user, string[] head, string title, int from)
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

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = title;

            tr = new TableRow();
            table.Rows.Add(tr);

            int i = 0, k = 0, n = 0;
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            string[] content = new string[head.Length];

            ResultSailFestival qresult = (ResultSailFestival)user.getQueryResult(QueryType.queryTypeSailingFestival);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultSF item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();
                content[n++] = item.m_data.getValue(1).ToString();
                content[n++] = item.m_data.getValue(0).ToString();

                for (int m = from, L = 0; L < ITEM_COL_COUNT_SIGN; m++, L++)
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
                for (int m = 1; m <= ITEM_COL_COUNT_SIGN; m++)
                {
                    if (item.m_mapCount2.m_data.ContainsKey(m))
                    {
                        content[n++] = item.m_mapCount2.m_data[m].m_count.ToString();
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

        // 招财进宝
        private void genTabeVip(Table table, OpRes res, GMUser user, string[] head)
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

            ResultSailFestival qresult = (ResultSailFestival)user.getQueryResult(QueryType.queryTypeSailingFestival);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                QrResultSF item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();
                content[n++] = item.m_data.getValue(1).ToString();
                content[n++] = item.m_data.getValue(0).ToString();

                for (int m = 0; m < ITEM_COL_COUNT_VIP; m++)
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

        // 成就
        private void genTabeAchive(Table table, OpRes res, GMUser user, string[] head, string title, int from, int to)
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

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = title;

            tr = new TableRow();
            table.Rows.Add(tr);

            int i = 0, k = 0, n = 0;
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            string[] content = new string[head.Length];

            ResultSailFestival qresult = (ResultSailFestival)user.getQueryResult(QueryType.queryTypeSailingFestival);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultSF item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();
                if (from != 1)
                {
                    content[n++] = item.m_data.getValue(0).ToString();
                }

                for (int m = from; m <= to; m++)
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

        // 成就商店
        private void genTabeAchiveShop(Table table, OpRes res, GMUser user, string[] head)
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

            ResultSailFestival qresult = (ResultSailFestival)user.getQueryResult(QueryType.queryTypeSailingFestival);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultSF item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();

                for (int m = 1; m <= ITEM_COL_COUNT_ACHIVE_SHOP; m++)
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

        // 成就等级
        private void genTabeAchiveLevel(Table table, OpRes res, GMUser user, string[] head)
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

            ResultSailFestival qresult = (ResultSailFestival)user.getQueryResult(QueryType.queryTypeSailingFestival);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultSF item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();
                content[n++] = item.m_data.getValue(0).ToString();

                for (int m = 1; m <= ITEM_COL_COUNT_ACHIVE_LEVEL; m++)
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

        // 海盗宝藏
        private void genTabeTreasure(Table table, OpRes res, GMUser user, string[] head)
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

            ResultSailFestival qresult = (ResultSailFestival)user.getQueryResult(QueryType.queryTypeSailingFestival);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultSF item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();

                for (int m = 1; m <= ITEM_COL_COUNT_TREASURE; m++)
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

        // 累计打开
        private void genTableAccOpen(Table table, OpRes res, GMUser user, string[] head)
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

            ResultSailFestival qresult = (ResultSailFestival)user.getQueryResult(QueryType.queryTypeSailingFestival);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultSF item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();

                for (int m = 0; m < ACC_OPEN.Length; m++)
                {
                    int a = ACC_OPEN[i];
                    if (item.m_mapCount.m_data.ContainsKey(a))
                    {
                        content[n++] = item.m_mapCount.m_data[a].m_count.ToString();
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

    }
}