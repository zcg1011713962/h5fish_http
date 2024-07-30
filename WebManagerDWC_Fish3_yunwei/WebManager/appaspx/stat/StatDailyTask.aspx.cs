using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatDailyTask : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "任务ID", "任务组", "开始人数", "完成人数" };
        private string[] m_content = new string[s_head.Length];
        private PageDialLottery m_gen = new PageDialLottery(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_DAILY_TASK, Session, Response);
            if (m_gen.parse(Request))
            {
                m_time.Text = m_gen.m_time;
                onQuery(null, null);
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_countEachPage = m_gen.rowEachPage;
            param.m_curPage = m_gen.curPage;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeDailyTask, user);
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
            List<DailyTaskItem> qresult = (List<DailyTaskItem>)mgr.getQueryResult(QueryType.queryTypeDailyTask);
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
                DailyTaskItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_taskId.ToString();
                m_content[f++] = item.m_taskGroup.ToString();
                m_content[f++] = item.m_startPersonCount.ToString();
                m_content[f++] = item.m_finishPersonCount.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatDailyTask.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}