using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatMiddleRoomExchange : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "兑换组合1总金币", "兑换组合2总金币", "兑换组合3总金币", 
            "兑换组合4总金币", "兑换组合5总金币", "兑换组合6总金币","兑换组合7总金币","兑换组合8总金币","兑换组合9总金币"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_MIDDLE_ROOM_EXCHANGE, Session, Response);
            if (!IsPostBack)
            {
                m_giftId.Items.Add("初级礼包");
                m_giftId.Items.Add("高级礼包");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_op = m_giftId.SelectedIndex+1;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatMiddleRoomExchange, user);
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
            List<StatMiddleRoomExchangeItem> qresult =
                (List<StatMiddleRoomExchangeItem>)mgr.getQueryResult(QueryType.queryTypeStatMiddleRoomExchange);
            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            //内容
            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                StatMiddleRoomExchangeItem item = qresult[i];

                m_content[f++] = item.m_time;

                foreach(var da in item.m_giftgoldlv)
                {
                    m_content[f++] = da.ToString();
                }

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