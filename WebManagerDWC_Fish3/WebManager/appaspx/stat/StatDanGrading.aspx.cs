using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatDanGrading : System.Web.UI.Page
    {
        const int ITEM_COL_COUNT = 7;
        const int OTHER_COL_COUTN = 2;

        private static string[] s_headComplete;
        private static string[] s_headBuy;
        private static string[] s_headRankCur = { "排行", "玩家名称", "玩家id", "积分" };
        private static string[] s_headRankHistroy = { "日期", "排行", "玩家名称", "玩家id", "积分" };

        private string[] m_content;

        static StatDanGrading()
        {
            s_headComplete = new string[ITEM_COL_COUNT + 1];
            s_headComplete[0] = "日期";
            for (int i = 1; i < s_headComplete.Length; i++)
            {
                s_headComplete[i] = "档位" + (i - 1 + 1).ToString() + "完成人数";
            }

            s_headBuy = new string[ITEM_COL_COUNT + OTHER_COL_COUTN];
            s_headBuy[0] = "日期";
            s_headBuy[1] = "购买人数";
            for (int i = OTHER_COL_COUTN; i < s_headBuy.Length; i++)
            {
                s_headBuy[i] = "档位" + (i - OTHER_COL_COUTN + 1).ToString() + "完成人数";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_DAN_GRADING, Session, Response);
            m_content = new string[Math.Max(s_headComplete.Length, s_headBuy.Length)];

            if (!IsPostBack)
            {
                m_optional.Items.Add(new ListItem("完成人数", QueryDanGrade.DAN_COMPLETE.ToString()));
                m_optional.Items.Add(new ListItem("礼包", QueryDanGrade.DAN_BUY.ToString()));
                m_optional.Items.Add(new ListItem("当前排行", DefCC.QRY_RANK_CUR.ToString()));
                m_optional.Items.Add(new ListItem("历史排行", DefCC.QRY_RANK_HISTORY.ToString()));
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = Convert.ToInt32(m_optional.SelectedValue);
            param.m_time = m_time.Text;

            OpRes res = user.doQuery(param, QueryType.queryTypeDanGrade);
            switch (param.m_op)
            {
                case QueryDanGrade.DAN_COMPLETE:
                    genTableComplete(m_result, res, user);
                    break;
                case QueryDanGrade.DAN_BUY:
                    genTableBuy(m_result, res, param, user);
                    break;
                case DefCC.QRY_RANK_CUR:
                    genTableRankCur(m_result, res, user);
                    break;
                case DefCC.QRY_RANK_HISTORY:
                    genTableRankHistory(m_result, res, param, user);
                    break;
            }
        }

        private void genTableComplete(Table table, OpRes res, GMUser user)
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

            int i = 0, k = 0, n = 0;
            for (i = 0; i < s_headComplete.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_headComplete[i];
            }

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryTypeDanGrade);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                m_content[n++] = item.m_time.ToShortDateString();

                for (int m = 1; m <= ITEM_COL_COUNT; m++)
                {
                    if (item.m_mapCount.m_data.ContainsKey(m))
                    {
                        m_content[n++] = item.m_mapCount.m_data[m].m_count.ToString();
                    }
                    else
                    {
                        m_content[n++] = "0";
                    }
                }

                for (k = 0; k < s_headComplete.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[k];
                }
            }
        }

        private void genTableBuy(Table table, OpRes res, ParamQuery param, GMUser user)
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

            int i = 0, k = 0, n = 0;
            for (i = 0; i < s_headBuy.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_headBuy[i];
            }

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryTypeDanGrade);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                for (i = 0; i < qresult.m_lotteryList.Count; i++)
                {
                    n = 0;

                    tr = new TableRow();
                    m_result.Rows.Add(tr);

                    QrResultItem item = qresult.m_lotteryList[i];
                    m_content[n++] = item.m_time.ToShortDateString();
                    m_content[n++] = item.m_data.getValue(0).ToString();

                    for (int m = 1; m <= ITEM_COL_COUNT; m++)
                    {
                        if (item.m_mapCount.m_data.ContainsKey(m))
                        {
                            m_content[n++] = item.m_mapCount.m_data[m].m_count.ToString();
                        }
                        else
                        {
                            m_content[n++] = "0";
                        }
                    }

                    for (k = 0; k < s_headBuy.Length; k++)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = m_content[k];
                    }
                }
            }
        }

        private void genTableRankCur(Table table, OpRes res, GMUser user)
        {
          //  m_page.InnerHtml = "";
          //  m_foot.InnerHtml = "";
          //
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

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryTypeDanGrade);

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

            int i = 0, k = 0, n = 0;
            for (i = 0; i < s_headRankHistroy.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_headRankHistroy[i];
            }

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(QueryType.queryTypeDanGrade);

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

         // param.m_op = m_optional.SelectedIndex;
         // string page_html = "", foot_html = "";
         // m_gen.genPage(param, @"/appaspx/stat/StatIntScoreSendAward.aspx", ref page_html, ref foot_html, user);
         // m_page.InnerHtml = page_html;
         // m_foot.InnerHtml = foot_html;
        }
    }
}