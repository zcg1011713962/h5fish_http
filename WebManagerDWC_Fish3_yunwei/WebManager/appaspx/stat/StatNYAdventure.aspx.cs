using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatNYAdventure : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "当日参与人次", "当日参与人数", "通过LV1的人次", 
            "通过LV2的人次", "通过LV3的人次","通过LV4的人次" ,"通过LV5的人次","使用的免费总数","使用的免死总数"};
        private string[] m_content = new string[s_head.Length];
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_NY_ADVENTURE, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();

            param.m_time = m_time.Text;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatNYAdventure, user);
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
            List<StatNYAdventureItem> qresult = (List<StatNYAdventureItem>)mgr.getQueryResult(QueryType.queryTypeStatNYAdventure);
            int i = 0, j = 0, f = 0,k=0;
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
                StatNYAdventureItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_joinCount.ToString();
                m_content[f++] = item.m_joinPerson.ToString();
                for (k = 0; k < item.m_lvCount.Length; k++ )
                {
                    m_content[f++] = item.m_lvCount[k].ToString();
                }
                m_content[f++] = item.m_freeCount.ToString();
                m_content[f++] = item.m_notDieCount.ToString();

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