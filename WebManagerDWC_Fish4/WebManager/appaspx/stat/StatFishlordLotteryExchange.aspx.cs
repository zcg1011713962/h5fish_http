using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordLotteryExchange : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","场次","系统收入","系统支出","话费奖励"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_FISHLORD_LOTTERY_EXCHANGE, Session, Response);

            if (!IsPostBack)
            {
                m_roomId.Items.Add("全部");
                m_roomId.Items.Add("初级场");
                m_roomId.Items.Add("中级场");
                //m_roomId.Items.Add("高级场");
            }
        }

        protected void btnStat_Click(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            ParamQuery param = new ParamQuery();
            param.m_op = m_roomId.SelectedIndex;
            param.m_time = m_time.Text;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = user.doQuery(param, QueryType.queryTypeStatLotteryExchange);

            genTable(tabResult, res, user);
        }

        //生成表
        protected void genTable(Table table, OpRes res, GMUser user)
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

            List<LotteryExchangeItem> qresult = (List<LotteryExchangeItem>)user.getQueryResult(QueryType.queryTypeStatLotteryExchange);

            int i = 0, j = 0, k = 0;

            tr = new TableRow();
            table.Rows.Add(tr);

            // 生成行标题
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                k = 0;
                LotteryExchangeItem item = qresult[i];

                tr = new TableRow();
                table.Rows.Add(tr);

                m_content[k++] = item.m_time;
                m_content[k++] = item.getRoomName();
                m_content[k++] = item.m_ticketFishIncome.ToString();
                m_content[k++] = item.m_ticketFishOutlay.ToString();
                m_content[k++] = (item.m_ticketOutlay/100.0).ToString();

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