using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFestivalActivity : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "活动ID","活动名称","活动周期","活跃人数","完成人数" };
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_FESTIVAL_ACTIVITY, Session, Response);  //权限
        }
        //节日活动查询
        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            OpRes res = user.doQuery(null, QueryType.queryTypeFestivalActivity);
            genTable(m_result, res, user);
        }
        //生成表
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

            List<FestivalActivityItem> qresult = (List<FestivalActivityItem>)user.getQueryResult(QueryType.queryTypeFestivalActivity);

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
                FestivalActivityItem item = qresult[i];
                m_content[f++] = item.m_cfg.m_activityId.ToString();
                m_content[f++] = item.m_cfg.m_activityName;
                m_content[f++] = item.m_cfg.m_activityStartTime + " 至 " + item.m_cfg.m_activityEndTime;
                m_content[f++] = item.m_activeCount.ToString();
                m_content[f++] = item.m_finishPerson.ToString();

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