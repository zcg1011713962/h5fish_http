using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPumpNp7DayRechargeAct : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] { "注册时间","渠道","触发人数","第一天购买人数","第二天购买人数","第三天购买人数","第四天购买人数","第五天购买人数",
            "第六天购买人数","满六天人数","兑换话费次数","详情"};
        private static string[] s_head2 = new string[] { "日期","渠道","兑换1","兑换2","兑换3","兑换4","兑换券收入"};

        private static string[] s_head3 = new string[] { "日期","打开人数","打开次数"};
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_PUMP_NP_7_DAY_RECHARGE_ACT, Session, Response);
            if (!IsPostBack)
            {
                m_channel.Items.Add(new ListItem("总体", ""));
                m_channel.Items.Add(new ListItem("全渠道", "-1"));
                m_channel.Items.Add(new ListItem("安卓总体", "-2"));
                Dictionary<string, TdChannelInfo> data = TdChannel.getInstance().getAllData();
                foreach (var item in data.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }

                m_item.Items.Add("购买情况");
                m_item.Items.Add("兑换数据");
                m_item.Items.Add("第一天礼包打点");
            }
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = m_item.SelectedIndex;
            param.m_time = m_time.Text;
            param.m_param = m_channel.SelectedValue;
            OpRes res = OpRes.op_res_failed;
            switch (param.m_op)
            {
                case 0: //购买情况
                    res = user.doQuery(param, QueryType.queryTypeStatPumpNp7DaysRecharge);
                    genTable0(m_result, res, user, param);
                    break;
                case 1: //兑换数据
                    res = user.doQuery(param, QueryType.queryTypeStatPumpNp7DaysRechangeExg);
                    genTable1(m_result, res, user, param);
                    break;
                case 2: //第一天礼包打点
                    param.m_op = 110;
                    res = user.doQuery(param, QueryType.queryTypeStatFishlordMiddleRoomFuDai);
                    genTable2(m_result, res, user, param);
                    break;
            }
        }

        //购买情况
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

            List<StatPumpNp7DaysRechargeItem> qresult =
                (List<StatPumpNp7DaysRechargeItem>)user.getQueryResult(QueryType.queryTypeStatPumpNp7DaysRecharge);

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
                StatPumpNp7DaysRechargeItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.getChannelName(query_param.m_param);
                m_content[f++] = item.m_triggerCount.ToString();
                foreach (var da in item.m_buyDay)
                {
                    m_content[f++] = da.Value.ToString();
                }

                m_content[f++] = item.m_buyFullDay.ToString();
                m_content[f++] = item.m_ticketConsume.ToString();
                m_content[f++] = item.getDetail(query_param.m_channelNo);

                for (j = 0; j < s_head1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }

        //兑换数据
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

            List<StatPumpNp7DaysExgItem> qresult =
                (List<StatPumpNp7DaysExgItem>)user.getQueryResult(QueryType.queryTypeStatPumpNp7DaysRechangeExg);

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
                StatPumpNp7DaysExgItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.getChannelName(query_param.m_param);
                foreach (var da in item.m_exg)
                {
                    m_content[f++] = da.Value.ToString();
                }
                m_content[f++] = item.m_ticketConsume.ToString();
                for (j = 0; j < s_head2.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }

        //打点统计
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

            List<StatFishlordMiddleRoomFuDaiItem> qresult =
                (List<StatFishlordMiddleRoomFuDaiItem>)user.getQueryResult(QueryType.queryTypeStatFishlordMiddleRoomFuDai);

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
                StatFishlordMiddleRoomFuDaiItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_action1Count.ToString();
                m_content[f++] = item.m_action2Count.ToString();

                tr = new TableRow();
                table.Rows.Add(tr);
                for (j = 0; j < s_head3.Length; j++)
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