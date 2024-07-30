using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatNewSevenDay : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "渠道", "新注册", "第2天完成任务 | 领取奖励人数", "第3天完成任务 | 领取奖励",
            "第4天完成任务 | 领取奖励", "第5天完成任务 | 领取奖励", "第6天完成任务 | 领取奖励","第7天完成任务 | 领取奖励", "第8天完成任务 | 领取奖励"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_SEVEN_DAY_ACTIVITY, Session, Response);
            if (!IsPostBack)
            {
                m_channel.Items.Add(new ListItem("全部", "")); //渠道

                Dictionary<string, TdChannelInfo> cd = TdChannel.getInstance().getAllData();
                foreach (var item in cd.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_channelNo = m_channel.SelectedValue;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeNewSevenDay, user);
            genTable(m_result, res, user, mgr, param);
        }

        //生成查询表
        private void genTable(Table table, OpRes res, GMUser user, QueryMgr mgr, ParamQuery param)
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
            List<newSevenDayItem> qresult = (List<newSevenDayItem>)mgr.getQueryResult(QueryType.queryTypeNewSevenDay);
            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            int f = 0;
            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;
                newSevenDayItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.getChannelName(param.m_channelNo);
                m_content[f++] = item.m_regeditCount.ToString();

                foreach (var da in item.m_dayRecv) 
                {
                    m_content[f++] = da.Value[0] + " | " + da.Value[1];
                }

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Attributes.CssStyle.Value = "min-width:100px";
                    td.Text = m_content[j];
                }
            }
        }
    }
}