using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.ashx
{
    public partial class OperationRankRecharge : System.Web.UI.Page
    {
        private static string[] s_head = new string[] {"排行","渠道","玩家ID","玩家昵称","炮倍","累计充值","最后登录"};
        private string[] m_content = new string[s_head.Length];
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_STAT_RANK_RECHARGE, Session, Response);
            if (!IsPostBack) 
            {
                onQuery(null, null);
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            OpRes res = user.doQuery(null, QueryType.queryTypeOperationRankRecharge);
            genTable(m_result, res, user);
        }

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

            List<OpRankRechargeItem> qresult = (List<OpRankRechargeItem>)user.getQueryResult(QueryType.queryTypeOperationRankRecharge);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                OpRankRechargeItem item = qresult[i];

                m_content[f++] = item.m_rankId.ToString();
                m_content[f++] = item.getChannelName();
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_nickName;
                m_content[f++] = getOpenRate(item.m_turretId);
                m_content[f++] = item.m_recharged.ToString();
                m_content[f++] = item.m_lastLogoutTime;
                
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }

        //炮倍等级
        public string getOpenRate(int turretId) 
        {
            string openRate = turretId.ToString();

            var openRateItem = Fish_TurretLevelCFG.getInstance().getValue(turretId);
            if (openRateItem != null)
                openRate = openRateItem.m_openRate.ToString();

            return openRate;
        }
    }
}