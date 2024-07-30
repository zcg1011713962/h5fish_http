using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatKillDemonsActivity : System.Web.UI.Page
    {
        const int ITEM_COL_COUNT_SIGN = 5;
        const int ITEM_COL_COUNT_ACT = 20;
        const int ITEM_COL_COUNT_CHALLENGE = 24;
        const int OTHER_COL_COUTN = 2;

        // 层级完成
        const int COL_COUNT_LEVEL_FINISH = 10;
        const int OTHER_COL_COUNT_LEVEL_FINISH = 2;

        // 任务签到
        private static string[] s_headQuestSign;
        // 任务活跃
        private static string[] s_headQuestAct;
        // 任务挑战
        private static string[] s_headQuestChallenge;

        // 神兽层级完成
        private static string[] s_headMonsterLevel;
        // 神兽星完成
        private static string[] s_headMonsterStar = new string[] { "日期", "完成5星人数", "完成10星人数", "完成15星人数", "完成20星人数", "完成25星人数", "完成30星人数" };

        // 法宝
        const int COL_COUNT_WEAPON = 9;
        const int OTHER_COL_COUNT_WEAPON = 3;
        private static string[] s_headWeapon;

        // 斩妖剑
        const int COL_COUNT_SWORD = 8;
        const int OTHER_COL_COUNT_SWORD = 2;
        private static string[] s_headSword;

        private static string[] s_headRankCur = new string[] { "玩家ID", "玩家昵称", "伤害值", "排行" };
        private static string[] s_headRankHistory = new string[] { "日期", "玩家ID", "玩家昵称", "伤害值", "排行" };

        private PageJiuQiuNationalActPlayer m_gen = new PageJiuQiuNationalActPlayer(100);

        static StatKillDemonsActivity()
        {
            s_headQuestSign = new string[OTHER_COL_COUTN + ITEM_COL_COUNT_SIGN];
            s_headQuestSign[0] = "日期";
            s_headQuestSign[1] = "签到产出金币";
            for (int i = OTHER_COL_COUTN; i < s_headQuestSign.Length; i++)
            {
                s_headQuestSign[i] = "签到" + (i - OTHER_COL_COUTN + 1).ToString() + "完成人数";
            }

            s_headQuestAct = new string[OTHER_COL_COUTN + ITEM_COL_COUNT_ACT];
            s_headQuestAct[0] = "日期";
            s_headQuestAct[1] = "活跃产出金币";
            for (int i = OTHER_COL_COUTN; i < s_headQuestAct.Length; i++)
            {
                s_headQuestAct[i] = "活跃" + (i - OTHER_COL_COUTN + 1).ToString() + "完成人数";
            }

            s_headQuestChallenge = new string[OTHER_COL_COUTN + ITEM_COL_COUNT_CHALLENGE];
            s_headQuestChallenge[0] = "日期";
            s_headQuestChallenge[1] = "挑战产出金币";
            for (int i = OTHER_COL_COUTN; i < s_headQuestChallenge.Length; i++)
            {
                s_headQuestChallenge[i] = "挑战" + (i - OTHER_COL_COUTN + 1).ToString() + "完成人数";
            }

            s_headMonsterLevel = new string[OTHER_COL_COUNT_LEVEL_FINISH + COL_COUNT_LEVEL_FINISH];
            s_headMonsterLevel[0] = "日期";
            s_headMonsterLevel[1] = "参与人数";
            for (int i = OTHER_COL_COUNT_LEVEL_FINISH; i < s_headMonsterLevel.Length; i++)
            {
                s_headMonsterLevel[i] = "层" + (i - OTHER_COL_COUNT_LEVEL_FINISH + 1).ToString() + "完成人数";
            }

            s_headWeapon = new string[OTHER_COL_COUNT_WEAPON + COL_COUNT_WEAPON];
            s_headWeapon[0] = "日期";
            s_headWeapon[1] = "猎妖积分产出";
            s_headWeapon[2] = "猎妖积分回收";
            for (int i = OTHER_COL_COUNT_WEAPON; i < s_headWeapon.Length; i++)
            {
                s_headWeapon[i] = "道具" + (i - OTHER_COL_COUNT_WEAPON + 1).ToString() + "产出";
            }

            s_headSword = new string[OTHER_COL_COUNT_SWORD + COL_COUNT_SWORD];
            s_headSword[0] = "日期";
            s_headSword[1] = "剑气产出数量";
            for (int i = OTHER_COL_COUNT_SWORD; i < s_headSword.Length; i++)
            {
                s_headSword[i] = "完成任务" + (i - OTHER_COL_COUNT_SWORD + 1).ToString() + "人数";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_KILL_MONSTER, Session, Response);

            if (!IsPostBack)
            {
                m_optional.Items.Add(new ListItem("猎妖任务", DefCC.QRY_RANK_CUR.ToString()));
                m_optional.Items.Add(new ListItem("神兽", DefCC.QRY_RANK_HISTORY.ToString()));
                m_optional.Items.Add(new ListItem("法宝", DefCC.QRY_LOTTERY.ToString()));
                m_optional.Items.Add(new ListItem("斩妖剑统计", "4"));
                m_optional.Items.Add(new ListItem("当前排行", "5"));
                m_optional.Items.Add(new ListItem("历史排行", "6"));

                if (m_gen.parse(Request))
                {
                   // m_optional.SelectedIndex = m_gen.m_lotteryId;
                  //  m_time.Text = m_gen.m_time;
                  //  onQuery(null, null);
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

            m_result1.Rows.Clear();
            m_result2.Rows.Clear();
            m_result3.Rows.Clear();
            OpRes res = user.doQuery(param, QueryType.queryTypeKillDemons);
            switch (param.m_op)
            {
                case DefCC.QRY_RANK_CUR:
                    genTableQuestSign(m_result1, res, user);
                    genTableQuestAct(m_result2, res, user);
                    genTableQuestCha(m_result3, res, user);
                    break;
                case DefCC.QRY_RANK_HISTORY:
                    {
                        genTableMonsterLevel(m_result1, res, user, s_headMonsterLevel, "层完成统计");
                        genTableMonsterStar(m_result2, res, user, s_headMonsterStar, "神兽星级统计");
                    }
                    break;
                case DefCC.QRY_LOTTERY:
                    {
                        genTableWeapon(m_result1, res, user, s_headWeapon);
                    }
                    break;
                case 4:
                    {
                        genTableSword(m_result1, res, user, s_headSword);
                    }
                    break;
                case 5:
                    {
                        genTableRankCur(m_result1, res, user, s_headRankCur);
                    }
                    break;
                case 6:
                    {
                        genTableRankHistory(m_result1, res, user, s_headRankHistory);
                    }
                    break;
            }
        }

        private void genTableQuestSign(Table table, OpRes res, GMUser user)
        {
            table.Rows.Clear();
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

            string[] content = new string[s_headQuestSign.Length];

            td = new TableCell();
            td.ColumnSpan = ITEM_COL_COUNT_SIGN + OTHER_COL_COUTN;
            tr.Cells.Add(td);
            td.Text = "签到任务";

            tr = new TableRow();
            table.Rows.Add(tr);

            int i = 0, k = 0, n = 0;
            for (i = 0; i < s_headQuestSign.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_headQuestSign[i];
            }

            ResultKillDemons qresult = (ResultKillDemons)user.getQueryResult(QueryType.queryTypeKillDemons);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();
                content[n++] = item.m_data.getValue(0).ToString();

                for (int m = 1; m <= 5; m++)
                {
                    if(item.m_mapCount.m_data.ContainsKey(m))
                    {
                        content[n++] = item.m_mapCount.m_data[m].m_count.ToString();
                    }
                    else
                    {
                        content[n++] = "0";
                    }
                }
                for (k = 0; k < s_headQuestSign.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        private void genTableQuestAct(Table table, OpRes res, GMUser user)
        {
            table.Rows.Clear();
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

            string[] content = new string[s_headQuestAct.Length];

            td = new TableCell();
            td.ColumnSpan = ITEM_COL_COUNT_ACT + OTHER_COL_COUTN;
            tr.Cells.Add(td);
            td.Text = "活跃任务";

            tr = new TableRow();
            table.Rows.Add(tr);

            int i = 0, k = 0, n = 0;
            for (i = 0; i < s_headQuestAct.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_headQuestAct[i];
            }

            ResultKillDemons qresult = (ResultKillDemons)user.getQueryResult(QueryType.queryTypeKillDemons);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();
                content[n++] = item.m_data.getValue(1).ToString();

                int p = ITEM_COL_COUNT_SIGN + 1;
                for (int m = 1; m <= ITEM_COL_COUNT_ACT; m++)
                {
                    if (item.m_mapCount.m_data.ContainsKey(p))
                    {
                        content[n++] = item.m_mapCount.m_data[p].m_count.ToString();
                    }
                    else
                    {
                        content[n++] = "0";
                    }
                    p++;
                }
                for (k = 0; k < s_headQuestAct.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        private void genTableQuestCha(Table table, OpRes res, GMUser user)
        {
            table.Rows.Clear();
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

            string[] content = new string[s_headQuestChallenge.Length];

            td = new TableCell();
            td.ColumnSpan = ITEM_COL_COUNT_CHALLENGE + OTHER_COL_COUTN;
            tr.Cells.Add(td);
            td.Text = "挑战任务";

            tr = new TableRow();
            table.Rows.Add(tr);

            int i = 0, k = 0, n = 0;
            for (i = 0; i < s_headQuestChallenge.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_headQuestChallenge[i];
            }

            ResultKillDemons qresult = (ResultKillDemons)user.getQueryResult(QueryType.queryTypeKillDemons);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();
                content[n++] = item.m_data.getValue(2).ToString();

                int p = ITEM_COL_COUNT_SIGN + ITEM_COL_COUNT_ACT + 1;
                for (int m = 1; m <= ITEM_COL_COUNT_CHALLENGE; m++)
                {
                    if (item.m_mapCount.m_data.ContainsKey(p))
                    {
                        content[n++] = item.m_mapCount.m_data[p].m_count.ToString();
                    }
                    else
                    {
                        content[n++] = "0";
                    }
                    p++;
                }
                for (k = 0; k < s_headQuestChallenge.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        // 神兽星级
        private void genTableMonsterStar(Table table, OpRes res, GMUser user, string[] head, string title)
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

            string[] content = new string[head.Length];

            td = new TableCell();
            td.ColumnSpan = head.Length;
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

            ResultKillDemons qresult = (ResultKillDemons)user.getQueryResult(QueryType.queryTypeKillDemons);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();
                content[n++] = item.m_data.getValue(0).ToString();
                content[n++] = item.m_data.getValue(1).ToString();
                content[n++] = item.m_data.getValue(2).ToString();
                content[n++] = item.m_data.getValue(3).ToString();
                content[n++] = item.m_data.getValue(4).ToString();
                content[n++] = item.m_data.getValue(5).ToString();

                for (k = 0; k < head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        // 神兽层级完成
        private void genTableMonsterLevel(Table table, OpRes res, GMUser user, string[] head, string title)
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

            string[] content = new string[head.Length];

            td = new TableCell();
            td.ColumnSpan = head.Length;
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

            ResultKillDemons qresult = (ResultKillDemons)user.getQueryResult(QueryType.queryTypeKillDemons);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();
                content[n++] = item.m_data.getValue(6).ToString();

                int p = 1;
                for (int m = 1; m <= COL_COUNT_LEVEL_FINISH; m++)
                {
                    if (item.m_mapCount.m_data.ContainsKey(p))
                    {
                        content[n++] = item.m_mapCount.m_data[p].m_count.ToString();
                    }
                    else
                    {
                        content[n++] = "0";
                    }
                    p++;
                }

                for (k = 0; k < head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        // 法宝
        private void genTableWeapon(Table table, OpRes res, GMUser user, string[] head)
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

            string[] content = new string[head.Length];

            int i = 0, k = 0, n = 0;
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            ResultKillDemons qresult = (ResultKillDemons)user.getQueryResult(QueryType.queryTypeKillDemons);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();
                content[n++] = item.m_data.getValue(0).ToString();
                content[n++] = item.m_data.getValue(1).ToString();

                int p = 1;
                for (int m = 1; m <= COL_COUNT_WEAPON; m++)
                {
                    if (item.m_mapCount.m_data.ContainsKey(p))
                    {
                        content[n++] = item.m_mapCount.m_data[p].m_count.ToString();
                    }
                    else
                    {
                        content[n++] = "0";
                    }
                    p++;
                }

                for (k = 0; k < head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        // 斩妖剑
        private void genTableSword(Table table, OpRes res, GMUser user, string[] head)
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

            string[] content = new string[head.Length];

            int i = 0, k = 0, n = 0;
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            ResultKillDemons qresult = (ResultKillDemons)user.getQueryResult(QueryType.queryTypeKillDemons);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();
                content[n++] = item.m_data.getValue(0).ToString();

                int p = 1;
                for (int m = 1; m <= COL_COUNT_SWORD; m++)
                {
                    if (item.m_mapCount.m_data.ContainsKey(p))
                    {
                        content[n++] = item.m_mapCount.m_data[p].m_count.ToString();
                    }
                    else
                    {
                        content[n++] = "0";
                    }
                    p++;
                }

                for (k = 0; k < head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        // 当前榜
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

            string[] content = new string[head.Length];

            int i = 0, k = 0, n = 0;
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            ResultKillDemons qresult = (ResultKillDemons)user.getQueryResult(QueryType.queryTypeKillDemons);

            for (i = 0; i < qresult.m_dataLine.m_rank.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                IntScoreRankInfo item = qresult.m_dataLine.m_rank[i];
                content[n++] = item.m_playerId.ToString();
                content[n++] = item.m_nickName;
                content[n++] = item.m_socre.ToString();
                content[n++] = item.m_rank.ToString();

                for (k = 0; k < head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = content[k];
                }
            }
        }

        // 历史榜
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

            string[] content = new string[head.Length];

            int i = 0, k = 0, n = 0;
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            ResultKillDemons qresult = (ResultKillDemons)user.getQueryResult(QueryType.queryTypeKillDemons);

            foreach (var dinfo in qresult.m_dataDic)
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

                    content[n++] = item.m_playerId.ToString();
                    content[n++] = item.m_nickName;
                    content[n++] = item.m_rank.ToString();
                    
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
