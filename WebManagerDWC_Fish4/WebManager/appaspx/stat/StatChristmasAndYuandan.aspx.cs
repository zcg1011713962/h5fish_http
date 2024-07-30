using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatChristmasAndYuandan : System.Web.UI.Page
    {
        private static string[] s_head = new string[] {"日期", "名称", "抽取数量", "兑换数量", "兑换道具名称","兑换总数（道具总数*兑换数量）" };
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_CHRISTMAS_YUANDAN, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();

            param.m_time = m_time.Text;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeChristmasOrYuandan, user);
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
            List<ChristmasOrYuandanItem> qresult = (List<ChristmasOrYuandanItem>)mgr.getQueryResult(QueryType.queryTypeChristmasOrYuandan);
            int i = 0, j = 0, f=0;
            // 表头
            for (i = 0; i<s_head.Length;i++ )
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i<qresult.Count;i++ )
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                ChristmasOrYuandanItem item=qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_actName;
                m_content[f++] = item.m_lotteryCount.ToString();
                m_content[f++] = item.m_exchangeCount.ToString();
                m_content[f++] = item.m_toolName;
                m_content[f++] = item.m_exchangeTotal.ToString();
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