using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatGoldFishLottery : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "档位", "总投入", "奖励1", "奖励2", "奖励3", "奖励4", "奖励5", "奖励6",
            "抽奖次数","抽奖人数","系统产出金币","盈利值","盈利率",""};

        private static string[] s_head1 = new string[] { "日期","总投入","抽奖次数","抽奖人数","系统产出金币","盈利值","盈利率"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_GOLD_FISH_LOTTERY, Session, Response);
            if (!IsPostBack)
            {
                m_lotteryId.Items.Add("总计抽奖");
                m_lotteryId.Items.Add("普通抽奖");
                m_lotteryId.Items.Add("青铜抽奖");
                m_lotteryId.Items.Add("白银抽奖");
                m_lotteryId.Items.Add("黄金抽奖");
                m_lotteryId.Items.Add("钻石抽奖");
                m_lotteryId.Items.Add("至尊抽奖");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = Convert.ToInt32(m_lotteryId.SelectedIndex);
            param.m_time = m_time.Text;

            OpRes res = OpRes.op_res_failed;

            if (param.m_op == 0)
            {
                res = user.doQuery(param, QueryType.queryTypeStatGoldFishLotteryTotal);
                genTable1(m_result, res, user);
            }
            else {
                res = user.doQuery(param, QueryType.queryTypeStatGoldFishLottery);
                genTable(m_result, res, user);
            }
        }

        private void genTable(Table table, OpRes res, GMUser user)
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
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            List<StatGoldFishLotteryItem> qresult =
                (List<StatGoldFishLotteryItem>)user.getQueryResult(QueryType.queryTypeStatGoldFishLottery);

            for (i = 0; i < qresult.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                StatGoldFishLotteryItem item = qresult[i];
                m_content[n++] = item.m_time;
                m_content[n++] = item.getGearName();
                m_content[n++] = item.m_incomeGold.ToString();
                m_content[n++] = item.m_gear1Count.ToString();
                m_content[n++] = item.m_gear2Count.ToString();
                m_content[n++] = item.m_gear3Count.ToString();
                m_content[n++] = item.m_gear4Count.ToString();
                m_content[n++] = item.m_gear5Count.ToString();
                m_content[n++] = item.m_gear6Count.ToString();
                m_content[n++] = item.m_totalCount.ToString();
                m_content[n++] = item.m_totalPerson.ToString();
                m_content[n++] = item.m_outlayGold.ToString();
                m_content[n++] = (item.m_incomeGold - item.m_outlayGold).ToString();
                m_content[n++] = item.getRate();
                m_content[n++] = item.getDetail();

                for (k = 0; k < s_head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[k];
                }
            }
        }

        private void genTable1(Table table, OpRes res, GMUser user)
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

            int i = 0, k = 0, n = 0;
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head1[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            List<StatGoldFishLotteryTotalItem> qresult =
                (List<StatGoldFishLotteryTotalItem>)user.getQueryResult(QueryType.queryTypeStatGoldFishLotteryTotal);

            for (i = 0; i < qresult.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                StatGoldFishLotteryTotalItem item = qresult[i];
                m_content[n++] = item.m_time;
                m_content[n++] = item.m_incomeGold.ToString();

                int m_lotteryCount = item.m_gear1Count + item.m_gear2Count + item.m_gear3Count + item.m_gear4Count + item.m_gear5Count + item.m_gear6Count;

                m_content[n++] = m_lotteryCount.ToString();
                m_content[n++] = item.m_lotteryPerson.ToString();
                m_content[n++] = item.m_outlayGold.ToString();
                m_content[n++] = (item.m_incomeGold - item.m_outlayGold).ToString();
                m_content[n++] = item.getRate();

                for (k = 0; k < s_head1.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[k];
                }
            }
        }
    }
}