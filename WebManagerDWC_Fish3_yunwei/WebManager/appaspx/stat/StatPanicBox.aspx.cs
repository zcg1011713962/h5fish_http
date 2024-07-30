using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPanicBox : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "宝箱类型", "当日抽取人次", "当日抽取人数",
            "获得奖励1人次", "获得奖励2人次", "获得奖励3人次", "获得奖励4人次", "获得奖励5人次", "获得奖励6人次", "获得奖励7人次", "获得奖励8人次","详情"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_PANIC_BOX, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatPanicBox, user);
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
            List<StatPanicBoxItem> qresult = (List<StatPanicBoxItem>)mgr.getQueryResult(QueryType.queryTypeStatPanicBox);
            int i = 0, j = 0,f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }
            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;
                StatPanicBoxItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.getBoxTypeName();
                m_content[f++] = item.m_lotteryCount.ToString();
                m_content[f++] = item.m_lotteryPerson.ToString();

                for (int k = 0; k<8; k++) 
                {
                    if (item.m_reward.ContainsKey(k))
                    {
                        m_content[f++] = item.m_reward[k].ToString();
                    }
                    else {
                        m_content[f++] = "";
                    }
                }
                m_content[f++] = qresult[i].getExParam(i);
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }
    }
}