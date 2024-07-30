using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatDailyWeekTask : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] { "日期", "每日签到", "10次锁定", "100条鱼", "1次普通鱼雷", "8条黄金鱼", "捕获巨鳄", "10次花色", 
            "在线30分钟", "魔石兑换", "500魔石","捕获巨鲨", "捕获同元素", "捕获大圣","充值"}; //每日

        private static string[] s_head3 = new string[] { "日期","累计签到","1500条鱼","50次锁定","30次花色","在线150分钟","魔石兑换15次","轰炸10次","4天普通鱼雷",
            "5000魔石","大圣5次","捕获豹子元素","400日活跃","充值20次"}; //

        private static string[] s_head2 = new string[] { "完成人数", "系统支出" };

        private static string[] s_head = new string[] { "日期", "第1档完成人数", "第2档完成人数", "第3档完成人数", "第4档完成人数", "系统支出" };
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_DAILY_WEEK_TASK, Session, Response);
            if (!IsPostBack) 
            {
                m_taskType.Items.Add("每日");
                m_taskType.Items.Add("每周");

                m_actType.Items.Add("任务");
                m_actType.Items.Add("奖励");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = Convert.ToInt32(m_taskType.SelectedIndex);
            param.m_time = m_time.Text;

            OpRes res = OpRes.op_res_failed;

            if (m_actType.SelectedIndex == 0) //任务
            {
                res = user.doQuery(param, QueryType.queryTypeStatDailyWeekTask);
                if (param.m_op == 0)
                {  //每日
                    genTable(m_result, res, user, s_head1);
                }
                else {
                    genTable(m_result, res, user, s_head3);
                }
            }
            else 
            { //奖励
                res = user.doQuery(param, QueryType.queryTypeStatDailyWeekReward);
                genTable1(m_result, res, user);
            }
        }

        private void genTable(Table table, OpRes res, GMUser user, string[] s_head)
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

            int i = 0, k = 0;


            for (i = 0; i < s_head.Length ; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                if(i == 0)
                {
                    td.RowSpan = 2;
                    td.ColumnSpan = 1;
                }else
                {
                    td.RowSpan = 1;
                    td.ColumnSpan = 2;
                }
            }

            tr = new TableRow();
            m_result.Rows.Add(tr);
            for (i = 0; i < s_head.Length - 1; i++) 
            {
                for (k = 0; k < s_head2.Length; k++) 
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = s_head2[k];
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                }
            }

            List<StatDailyWeekTaskItem> qresult =
                (List<StatDailyWeekTaskItem>)user.getQueryResult(QueryType.queryTypeStatDailyWeekTask);

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                StatDailyWeekTaskItem item = qresult[i];

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time;

                for (k = 1; k <= s_head.Length - 1; k++) 
                {
                    if (item.m_data.ContainsKey(k))
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_data[k][0].ToString();

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_data[k][1].ToString();
                    }
                    else {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = "";

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = "";
                    }
                }
            }
        }

        private void genTable1(Table table, OpRes res, GMUser user)
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

            List<StatDailyWeekRewardItem> qresult =
                (List<StatDailyWeekRewardItem>)user.getQueryResult(QueryType.queryTypeStatDailyWeekReward);

            for (i = 0; i < qresult.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                StatDailyWeekRewardItem item = qresult[i];
                m_content[n++] = item.m_time;
                m_content[n++] = item.m_gear1Count.ToString();
                m_content[n++] = item.m_gear2Count.ToString();
                m_content[n++] = item.m_gear3Count.ToString();
                m_content[n++] = item.m_gear4Count.ToString();
                m_content[n++] = item.m_sysOutlay.ToString();

                for (k = 0; k < s_head.Length; k++) 
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[k];
                }
            }
        }
    }
}