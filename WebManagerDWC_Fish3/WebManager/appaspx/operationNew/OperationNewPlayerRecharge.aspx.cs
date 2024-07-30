using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operationNew
{
    public partial class OperationNewPlayerRecharge : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "玩家ID","渠道","充值次数","充值金额","注册时间","上线次数","剩余金币","最后上线时间"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OPNEW_PLAYER_RECHARGE, Session, Response);
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            TableTdActivation view = new TableTdActivation();
            ParamQuery param = new ParamQuery();
            param.m_playerId = m_playerId.Text;
            param.m_time = m_time.Text;
            OpRes res = user.doQuery(param, QueryType.queryTypeOpnewPlayerRecharge);
            genTable(user, m_resTable, res);
        }

        public void genTable(GMUser user, Table table, OpRes res)
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

            int i = 0, f = 0;
            for (; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.ColumnSpan = 1;
                td.RowSpan = 1;
            }

            List<PlayerRechargeItem> qresult = (List<PlayerRechargeItem>)user.getQueryResult(QueryType.queryTypeOpnewPlayerRecharge);
            foreach (var data in qresult)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                m_content[f++] = data.m_playerId.ToString();
                m_content[f++] = data.getChannelName();
                m_content[f++] = data.m_rechargeCount.ToString();
                m_content[f++] = data.m_rechargeRmb.ToString();
                m_content[f++] = data.m_createTime;
                m_content[f++] = data.m_loginCount.ToString();
                m_content[f++] = data.m_leftGold.ToString();
                m_content[f++] = data.m_lastLoginTime;

                for (int j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }
    }
}