using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatNYAccRecharge : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "奖励ID","充值金额", "达成人数","领取人数"};
        private string[] m_content = new string[s_head.Length];
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_NY_ACC_RECHARGE, Session, Response);
            if (!IsPostBack)
            {
                GMUser user = (GMUser)Session["user"];
                ParamQuery param = new ParamQuery();
                QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
                OpRes res = mgr.doQuery("", QueryType.queryTypeStatNYAccRecharge, user);
                genTable(m_result, res, param, user, mgr);
            }
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
            List<StatNYAccRechargeItem> qresult = (List<StatNYAccRechargeItem>)mgr.getQueryResult(QueryType.queryTypeStatNYAccRecharge);
            int i = 0, j = 0, f = 0;
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
                table.Rows.Add(tr);
                f = 0;
                StatNYAccRechargeItem item = qresult[i];
                m_content[f++] = item.m_rewardId.ToString();
                m_content[f++] = item.getRechargeRMB().ToString();
                m_content[f++] = item.m_reachCount.ToString();
                m_content[f++] = item.m_recvCount.ToString();
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