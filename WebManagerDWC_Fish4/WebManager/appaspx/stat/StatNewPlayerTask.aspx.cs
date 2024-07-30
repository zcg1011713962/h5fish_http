using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatNewPlayerTask : System.Web.UI.Page
    {
        private static string[] s_head = new string[31];
        private static string[] s_head1 = new string[] { "完成任务人数","任务完成率"};

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_NEW_PLAYER_TASK, Session, Response);
            //if (!IsPostBack)
            //{
            //    m_channel.Items.Add(new ListItem("全部", "")); //渠道

            //    Dictionary<string, TdChannelInfo> cd = TdChannel.getInstance().getAllData();
            //    foreach (var item in cd.Values)
            //    {
            //        m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
            //    }
            //}
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            //param.m_channelNo = m_channel.SelectedValue;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeNewPlayerTask, user);
            genTable(m_result, res, param, user, mgr);
        }

        //生成查询表
        private void genTable(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            m_result.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
            TableCell td = null;
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }
            newPlayerTask qres = (newPlayerTask)mgr.getQueryResult(QueryType.queryTypeNewPlayerTask);

            List<newPlayerTaskItem> qresult = (List<newPlayerTaskItem>)qres.m_result;
            int i = 0, j = 0;
            // 表头

            td = new TableCell();
            tr.Cells.Add(td);

            td.ColumnSpan = 1;
            td.RowSpan = 2;
            td.Text = "日期";
            td.Attributes.CssStyle.Value = "vertical-align:middle";

            td = new TableCell();
            tr.Cells.Add(td);

            td.ColumnSpan = 1;
            td.RowSpan = 2;
            td.Text = "注册人数";
            td.Attributes.CssStyle.Value = "vertical-align:middle";

            var newPlayerQuestList = NewPlayerQuestCFG.getInstance().getAllData();

            foreach(var quest in newPlayerQuestList)
            {
                td = new TableCell();
                tr.Cells.Add(td);

                td.ColumnSpan = 2;
                td.RowSpan = 1;
                td.Text = quest.Value.m_itemName;
            }

            tr = new TableRow();
            m_result.Rows.Add(tr);

            for (i = 0; i< s_head.Length - 2; i++) 
            {
                for (j = 0; j< s_head1.Length; j++) 
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                    td.Text = s_head1[j];
                }
            }

            for (i = 0; i < qresult.Count; i++)
            {
                newPlayerTaskItem item = qresult[i];

                tr = new TableRow();
                m_result.Rows.Add(tr);

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time;
                td.RowSpan = 1;

                td = new TableCell();
                tr.Cells.Add(td);
                td.RowSpan = 1;
                td.Text = item.m_regeditCount.ToString();

                for (int k = 1; k <= 29; k++) 
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    string str = "task_" + k;
                    int task_num = 0;
                    if (item.m_task.ContainsKey(str))
                    {
                        task_num = item.m_task[str];
                    }else{
                        task_num = 0;
                    }
                    td.Text = task_num.ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = item.getPercent(task_num,item.m_regeditCount);
                }
            }
        }
    }
}