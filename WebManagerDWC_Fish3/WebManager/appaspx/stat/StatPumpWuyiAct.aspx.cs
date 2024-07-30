using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPumpWuyiAct : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] { "日期","任务1完成人数","任务2完成人数","任务3完成人数","任务4完成人数",
            "任务5完成人数","任务6完成人数","任务7完成人数"};
        private static string[] s_head2 = new string[] { "日期", "单抽抽奖人数", "单抽抽奖次数", "十连抽奖人数", "十连抽奖次数", "详情" };

        private static string[] s_headSign;

        static StatPumpWuyiAct()
        {
            s_headSign = new string[7 + 1];
            s_headSign[0] = "日期";
            for (int i = 1; i <= 7; i++)
            {
                s_headSign[i] = "第" + i.ToString() + "天";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_PUMP_WUYI_SET_2020_ACT, Session, Response);
            if (!IsPostBack)
            {
                m_item.Items.Add("任务统计");
                m_item.Items.Add("抽奖统计");
                m_item.Items.Add("签到统计");
                //m_item.Items.Add("五一活动");
                //m_item.Items.Add("五一宝库");
            }
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = m_item.SelectedIndex;
            param.m_time = m_time.Text;
            OpRes res = OpRes.op_res_failed;
            switch (param.m_op)
            {
                case 0: //五一活动
                    res = user.doQuery(param, QueryType.queryTypeStatPumpWuyiSetAct);
                    genTable0(m_result, res, user, param);
                    break;
                case 1: //五一宝库
                    res = user.doQuery(param, QueryType.queryTypeStatPumpWuyiSetActLottery);
                    genTable1(m_result, res, user, param);
                    break;
                case 2:
                    {
                        res = user.doQuery(param, QueryType.queryTypeStatPumpWuyiSetAct);
                        genTableSign(m_result, res, user, s_headSign);
                    }
                    break;
            }
        }

        //五一活动
        private void genTable0(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            string[] m_content = new string[s_head1.Length];

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

            List<StatWuyiSetActItem> qresult =
                (List<StatWuyiSetActItem>)user.getQueryResult(QueryType.queryTypeStatPumpWuyiSetAct);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.RowSpan = 1;
                td.Text = s_head1[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                StatWuyiSetActItem item = qresult[i];

                m_content[f++] = item.m_time;
                foreach (var da in item.m_taskList)
                {
                    m_content[f++] = da.Value.ToString();
                }

                for (j = 0; j < s_head1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }

        //五一宝库
        private void genTable1(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            string[] m_content = new string[s_head2.Length];

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

            List<StatWuyiSetActLotteryItem> qresult =
                (List<StatWuyiSetActLotteryItem>)user.getQueryResult(QueryType.queryTypeStatPumpWuyiSetActLottery);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.RowSpan = 1;
                td.Text = s_head2[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                StatWuyiSetActLotteryItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_lotteryPerson1.ToString();
                m_content[f++] = item.m_lotteryCount1.ToString();

                m_content[f++] = item.m_lotteryPerson2.ToString();
                m_content[f++] = item.m_lotteryCount2.ToString();

                m_content[f++] = item.getDetail();

                for (j = 0; j < s_head2.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }

        private void genTableSign(Table table, OpRes res, GMUser user, string[] head)
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
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            string[] content = new string[head.Length];

            ScoreWrap qresult = (ScoreWrap)user.getQueryResult(null, QueryType.queryTypeStatPumpWuyiSetAct);

            for (i = 0; i < qresult.m_lotteryList.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                table.Rows.Add(tr);

                QrResultItem item = qresult.m_lotteryList[i];
                content[n++] = item.m_time.ToShortDateString();

                for (int m = 1; m <= 7; m++)
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

    }
}