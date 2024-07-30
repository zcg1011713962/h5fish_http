using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatWuyiRewardResult : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "参与人数", "参与次数", "奖励ID", "奖励类型", "奖励数量", "获取该奖励的人数" };
        private string[] m_content = new string[s_head.Length];
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_WUYI_REWARD_RESULT, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeWuyiRewardResult, user);
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
            List<wuYiRewardResItem> qresult = (List<wuYiRewardResItem>)mgr.getQueryResult(QueryType.queryTypeWuyiRewardResult);

            List<string> timeList = new List<string>();

            int i = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                wuYiRewardResItem item = qresult[i];

                foreach (rewardResList rlist in item.m_data)
                {
                    tr = new TableRow();
                    table.Rows.Add(tr);

                    //第一次的时候
                    if (!timeList.Contains(item.m_time))
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_time;
                        td.RowSpan = item.m_flag;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_joinPerson.ToString();
                        td.RowSpan = item.m_flag;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_joinCount.ToString();
                        td.RowSpan = item.m_flag;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";

                        timeList.Add(item.m_time);
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = rlist.m_rewardId.ToString();
                    td.RowSpan = 1;

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = rlist.m_rewardType;

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = rlist.m_rewardCount.ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = rlist.m_rewardPerson.ToString();
                }
            }
        }
    }
}