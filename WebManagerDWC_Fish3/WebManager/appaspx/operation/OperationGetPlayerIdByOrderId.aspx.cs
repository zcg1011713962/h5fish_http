using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationGetPlayerIdByOrderId : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "玩家ID","商品名称","充值金额（元）"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_GET_PLAYERID_BY_ORDERID, Session, Response);
            if(!IsPostBack)
            {
                Dictionary<string, Pay_SdkItem> paySdk = Pay_SdkCFG.getInstance().getAllData();
                foreach (var item in paySdk.Values)
                {
                    m_table.Items.Add(item.m_key);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            ParamQuery param = new ParamQuery();
            param.m_param = m_orderId.Text;
            param.m_channelNo = m_table.SelectedValue; //支付表
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeGetPlayerIdByOrderId, user);

            genTable(m_result, res, param, user, mgr);
        }

        private void genTable(Table table, OpRes res, ParamQuery param, GMUser user, QueryMgr mgr)
        {
            table.GridLines = GridLines.Both;
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

            PlayerOrderItem qresult = (PlayerOrderItem)mgr.getQueryResult(QueryType.queryTypeGetPlayerIdByOrderId);
            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            tr = new TableRow();
            table.Rows.Add(tr);

            m_content[0] = qresult.m_playerId;
            m_content[1] = qresult.m_payCode;
            m_content[2] = qresult.m_money.ToString();

            for (j = 0; j < s_head.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        }
    }
}