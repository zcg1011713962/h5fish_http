using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatGrowthQuest : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","注册人数","积分1","积分2","积分3","积分4","积分5","积分6","详情"};
        private static string[] s_head1 = new string[] { "日期","注册人数","奖励1", "奖励2", "奖励3", "奖励4", "奖励5", "奖励6", "奖励7", "奖励8", "奖励9", "奖励10" };
        private static string[] s_head2 = new string[] { "日期","礼包1购买次数","礼包2购买次数", "礼包3购买人数"};

        private static string[] s_head22 = new string[] { "总", "5倍", "6倍", "7倍", "8倍"};

        private static string[] s_head3 = new string[34];
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_GROWTH_QUEST, Session, Response);
            if (!IsPostBack)
            {
                m_item.Items.Add("捕鱼王");
                m_item.Items.Add("新手返利");
                m_item.Items.Add("新手转盘");
                m_item.Items.Add("新手成就");
            }
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_op = m_item.SelectedIndex;
            OpRes res = OpRes.op_res_failed;
            switch (param.m_op)
            {
                case 0: //捕鱼王
                    res = user.doQuery(param, QueryType.queryTypeStatGrowthQuest);
                    genTable(m_result, res, user, param);
                    break;
                case 1: //新手返利
                    res = user.doQuery(param, QueryType.queryTypeStatGrowthRechargeTaskRebate);
                    genTable1(m_result, res, user, param);
                    break;
                case 2: //新手转盘
                    res = user.doQuery(param, QueryType.queryTypeStatGrowthRechargeTaskGift);
                    genTable2(m_result, res, user, param);
                    break;
                case 3: //新手成就
                    res = user.doQuery(param, QueryType.queryTypeStatGrowthAchieveTask);
                    genTable3(m_result, res, user, param);
                    break;
            }
        }

        //捕鱼王
        private void genTable(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            string[] m_content = new string[s_head.Length];

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

            List<StatGrowthQuestItem> qresult = (List<StatGrowthQuestItem>)user.getQueryResult(QueryType.queryTypeStatGrowthQuest);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.RowSpan = 1;
                td.ColumnSpan = 1;
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                StatGrowthQuestItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_regeditCount.ToString();
                foreach(var da in item.m_recvList)
                {
                    m_content[f++] = da.ToString();
                }

                m_content[f++] = item.getDetail();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }

        //新手返利
        private void genTable1(Table table, OpRes res, GMUser user, ParamQuery query_param)
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

            List<StatGrowthRechargeTaskRebateItem> qresult = 
                (List<StatGrowthRechargeTaskRebateItem>)user.getQueryResult(QueryType.queryTypeStatGrowthRechargeTaskRebate);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.RowSpan = 1;
                td.ColumnSpan = 1;
                td.Text = s_head1[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                StatGrowthRechargeTaskRebateItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_regeditCount.ToString();
                foreach (var da in item.m_itemList)
                {
                    m_content[f++] = da.Value.ToString();
                }

                for (j = 0; j < s_head1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }

        //新手转盘
        private void genTable2(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            string[] m_content = new string[16];

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

            List<StatGrowthRechargeTaskTurntable> qresult = 
                (List<StatGrowthRechargeTaskTurntable>)user.getQueryResult(QueryType.queryTypeStatGrowthRechargeTaskGift);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                
                if (i > 0)
                {
                    td.ColumnSpan = 5;
                    td.RowSpan = 1;
                }
                else {
                    td.ColumnSpan = 1;
                    td.RowSpan = 2;
                }

                td.Text = s_head2[i];
                td.Attributes.CssStyle.Value = "vertical-align:middle";
            }

            tr = new TableRow();
            table.Rows.Add(tr);
            for (i = 0;  i < s_head2.Length-1; i++) 
            {
                for (j = 0;  j < s_head22.Length; j++) 
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = s_head22[j];
                }
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                StatGrowthRechargeTaskTurntable item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = (item.m_itemList[0] + item.m_itemList[1] + item.m_itemList[2] + item.m_itemList[3]).ToString();
                m_content[f++] = item.m_itemList[0].ToString();
                m_content[f++] = item.m_itemList[1].ToString();
                m_content[f++] = item.m_itemList[2].ToString();
                m_content[f++] = item.m_itemList[3].ToString();
                m_content[f++] = (item.m_itemList[4] + item.m_itemList[5] + item.m_itemList[6] + item.m_itemList[7]).ToString();
                m_content[f++] = item.m_itemList[4].ToString();
                m_content[f++] = item.m_itemList[5].ToString();
                m_content[f++] = item.m_itemList[6].ToString();
                m_content[f++] = item.m_itemList[7].ToString();
                m_content[f++] = (item.m_itemList[8] + item.m_itemList[9] + item.m_itemList[10] + item.m_itemList[11]).ToString();
                m_content[f++] = item.m_itemList[8].ToString();
                m_content[f++] = item.m_itemList[9].ToString();
                m_content[f++] = item.m_itemList[10].ToString();
                m_content[f++] = item.m_itemList[11].ToString();

                for (j = 0; j < 16; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                }
            }
        }

        //新手成就
        private void genTable3(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            string[] m_content = new string[s_head3.Length];

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

            List<StatGrowthRechargeTaskRebateItem> qresult =
                (List<StatGrowthRechargeTaskRebateItem>)user.getQueryResult(QueryType.queryTypeStatGrowthAchieveTask);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head3.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.RowSpan = 1;
                td.ColumnSpan = 1;

                if (i == 0)
                {
                    td.Text = "日期";
                }
                else 
                {
                    td.Text = "任务_" + i;
                }
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                StatGrowthRechargeTaskRebateItem item = qresult[i];
                m_content[f++] = item.m_time;
                foreach (var da in item.m_itemList)
                {
                    m_content[f++] = da.Value.ToString();
                }

                for (j = 0; j < s_head3.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                }
            }
        }
    }
}