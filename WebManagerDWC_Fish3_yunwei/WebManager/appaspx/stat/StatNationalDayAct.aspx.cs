using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatNationalDayAct : System.Web.UI.Page
    {

        private static string[] s_head1 = new string[] { "第一天签到人数", "第二天签到人数", "第三天签到人数", 
            "第四天签到人数", "第五天签到人数", "第六天签到人数","第七天签到人数", "签到总支出"};

        private static string[] s_head2 = new string[] { "日期", "兑换1人数 | 次数", "兑换2人数 | 次数", 
            "兑换3人数 | 次数", "兑换4人数 | 次数", "兑换5人数 | 次数", "兑换6人数 | 次数", "兑换总支出"};

        private static string[] s_head3 = new string[] { "日期","初始任务完成人数","初始支出", "任务难度", "选择人数", "完成1人数", "完成2人数", "完成3人数", "完成4人数" ,"总支出"};

        private static string[] s_head4 = new string[] { "日期", "礼包点开人数|次数", "礼包1购买人数|次数", "礼包2购买人数|次数", "礼包3购买人数|次数"};

        private static string[] s_head5 = new string[] { "日期","单次抽奖人数","单次抽奖次数","详情"};

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_NATIONAL_DAY_ACT, Session, Response);
            if (!IsPostBack)
            {
                m_queryType.Items.Add("签到统计");
                m_queryType.Items.Add("欢乐集字");
                m_queryType.Items.Add("冒险之路");
                m_queryType.Items.Add("礼包购买");
                m_queryType.Items.Add("抽奖统计");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();

            param.m_op = m_queryType.SelectedIndex;
            param.m_time = m_time.Text;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = OpRes.op_res_failed;
            switch (param.m_op)
            {
                case 0://签到
                    res = mgr.doQuery(param, QueryType.queryTypeStatActSign, user);
                    genTable0(m_result, res, user, param);
                    break;
                case 1://欢乐集字
                    res = mgr.doQuery(param, QueryType.queryTypeStatActExchange, user);
                    genTable1(m_result, res, user, param);
                    break;
                case 2://冒险之路
                    res = mgr.doQuery(param, QueryType.queryTypeStatActTask, user);
                    genTable2(m_result, res, user, param);
                    break;
                case 3: //礼包购买
                    res = mgr.doQuery(param, QueryType.queryTypeStatActGift, user);

                    genTable3(m_result, res, user, param);
                    break;
                case 4://抽奖统计
                    param.m_type = 6;
                    res = mgr.doQuery(param, QueryType.queryTypeStatActLottery, user);
                    genTable4(m_result, res, user, param);
                    break;
            }
        }

        //签到
        private void genTable0(Table table, OpRes res, GMUser user, ParamQuery param)
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

            StatActSignItem qresult =
                (StatActSignItem)user.getQueryResult(QueryType.queryTypeStatActSign);

            string[] m_content = new string[s_head1.Length];

            // 表头
            for (int i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head1[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            tr = new TableRow();
            table.Rows.Add(tr);

            foreach(var da in qresult.m_sign)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = da.Value.ToString();
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult.m_outlay.ToString();
        }

        //欢乐集字
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

            List<StatActExchangeItem> qresult =
                (List<StatActExchangeItem>)user.getQueryResult(QueryType.queryTypeStatActExchange);

            int i = 0, j = 0, f = 0;
            string[] m_content = new string[s_head2.Length];

            // 表头
            for (i = 0; i < s_head2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head2[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                StatActExchangeItem item = qresult[i];
                m_content[f++] = item.m_time;
                foreach (var da in item.m_exchange) 
                {
                    m_content[f++] = da.Value[0] + "&ensp;|&ensp;" + da.Value[1];
                }

                m_content[f++] = item.m_outlay.ToString();

                
                for (j = 0; j < s_head2.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                }
            }
        }

        //冒险之路
        private void genTable2(Table table, OpRes res, GMUser user, ParamQuery param)
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

            List<StatActTaskItem> qresult =
                (List<StatActTaskItem>)user.getQueryResult(QueryType.queryTypeStatActTask);

            int i = 0, j = 0, f = 0;
            string[] m_content = new string[s_head3.Length];

            // 表头
            for (i = 0; i < s_head3.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head3[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;

                StatActTaskItem item = qresult[i];

                foreach (var branch in item.m_branches)
                {

                    tr = new TableRow();
                    table.Rows.Add(tr);

                    if (f == 0)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_time;
                        td.RowSpan = 3;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_firstSteepCount.ToString();
                        td.RowSpan = 3;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_firstSteepOutlay.ToString();
                        td.RowSpan = 3;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = branch.Key.ToString();
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;

                    foreach (var da in branch.Value) 
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = da.ToString();
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = item.m_outlay[branch.Key].ToString();

                    f++;
                }
            }
        }

        //礼包购买
        private void genTable3(Table table, OpRes res, GMUser user, ParamQuery param)
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

            List<StatActGiftItem> qresult =
                (List<StatActGiftItem>)user.getQueryResult(QueryType.queryTypeStatActGift);

            int i = 0, j = 0, f = 0;
            string[] m_content = new string[s_head4.Length];

            // 表头
            for (i = 0; i < s_head4.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head4[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                table.Rows.Add(tr);
                StatActGiftItem item = qresult[i];

                m_content[f++] = item.m_time;

                foreach (var da in item.m_gifRecharge) 
                {
                    m_content[f++] = da.Value[0] + "&ensp;| &ensp;" + da.Value[1];
                }

                for (j = 0; j < s_head4.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                }
            }
        }

        //抽奖统计
        private void genTable4(Table table, OpRes res, GMUser user, ParamQuery param)
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

            List<StatActLotteryItem> qresult =
                (List<StatActLotteryItem>)user.getQueryResult(QueryType.queryTypeStatActLottery);

            int i = 0, j = 0, f = 0;
            string[] m_content = new string[s_head5.Length];

            // 表头
            for (i = 0; i < s_head5.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head5[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                table.Rows.Add(tr);

                StatActLotteryItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_lotteryPerson.ToString();
                m_content[f++] = item.m_lotteryCount.ToString();
                m_content[f++] = item.getDetail();

                for (j = 0; j < s_head5.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                }
            }
        }
    }
}