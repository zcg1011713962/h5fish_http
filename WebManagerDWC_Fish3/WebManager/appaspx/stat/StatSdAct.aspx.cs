using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatSdAct : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] { "日期", "第1天签到人数", "第2天", "第3天", "第4天", 
            "第5天", "第6天", "第7天", "第8天", "第9天", "第10天", "第11天", "第12天","第13天","第14天","第15天"};
        private static string[] s_head2 = new string[] { "日期", "完成任务1人数 | 次数", "完成任务2人数 | 次数", "完成任务3人数 | 次数", "完成任务4人数 | 次数", "完成任务5人数 | 次数"};
        private static string[] s_head3 = new string[] { "日期", "兑换奖励1人数 | 次数", "兑换奖励2人数 | 次数", "兑换奖励3人数 | 次数", "兑换奖励4人数 | 次数", "兑换奖励5人数 | 次数" };
        private static string[] s_head4 = new string[] { "日期", "开服礼包点开人数 | 次数","礼包1购买人数 | 次数","礼包2购买人数 | 次数","礼包3购买人数 | 次数"};

        private string[] m_content = new string[s_head1.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_SD_ACT, Session, Response);
            if (!IsPostBack) {
                m_queryId.Items.Add("签到");
                m_queryId.Items.Add("任务");
                m_queryId.Items.Add("兑换");
                m_queryId.Items.Add("礼包");
            }
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            TableTdActivation view = new TableTdActivation();
            ParamQuery param = new ParamQuery();
            param.m_op = m_queryId.SelectedIndex + 1;
            param.m_time = m_time.Text;

            OpRes res = user.doQuery(param, QueryType.queryTypeStatSdAct);

            switch(param.m_op)
            {
                case 1:
                    genTable(user, m_resTable, s_head1, 1, res);
                    break;
                case 2:
                    genTable(user, m_resTable, s_head2, 2, res);
                    break;
                case 3:
                    genTable(user, m_resTable, s_head3, 3, res);
                    break;
                case 4:
                    genTable(user, m_resTable, s_head4, 4, res);
                    break;
            }
        }

        public void genTable(GMUser user, Table table,string[] s_head, int op, OpRes res)
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

            int i = 0, f = 0;
            for (; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.ColumnSpan = 1;
                td.RowSpan = 1;
            }

            List<StatSdActItem> qresult = (List<StatSdActItem>)user.getQueryResult(QueryType.queryTypeStatSdAct);
            foreach (var data in qresult)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                m_content[f++] = data.m_time;

                switch(op)
                {
                    case 1:
                        foreach(var da in data.m_sign_data)
                        {
                            m_content[f++] = da.Value.ToString();
                        }
                        break;

                    default:
                        Dictionary<int, int[]> data1 = new Dictionary<int, int[]>();
                        if (op == 2) {
                            data1 = data.m_task_data;
                        }
                        else if (op == 3)
                        {
                            data1 = data.m_exchange_data;
                        }
                        else {
                            data1 = data.m_gift_data;
                        }

                        foreach (var da in data1)
                        {
                            m_content[f++] = da.Value[0].ToString() + " | " + da.Value[1].ToString();
                        }
                        break;
                }

                for (int j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }

    }
}