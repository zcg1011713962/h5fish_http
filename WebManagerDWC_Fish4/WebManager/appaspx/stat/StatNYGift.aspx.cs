using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatNYGift : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "礼包", "购买1次(人次)", "购买2次", "购买3次", "购买4次", "购买5次","购买6次", 
            "购买7次", "购买8次", "购买9次", "购买10次", "购买11次" ,"购买12次","购买13次","购买14次","购买15次","购买16次","购买17次","购买18次"};
        private string[] m_content = new string[s_head.Length];
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_NY_GIFT, Session, Response);
            if(!IsPostBack)
            {
                GMUser user = (GMUser)Session["user"];
                ParamQuery param = new ParamQuery();
                QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
                OpRes res = mgr.doQuery("", QueryType.queryTypeStatNYGift, user);
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
            List<StatNYGiftItem> qresult = (List<StatNYGiftItem>)mgr.getQueryResult(QueryType.queryTypeStatNYGift);
            int i = 0, j = 0, f = 0,k = 0 ;
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
                StatNYGiftItem item = qresult[i];
                m_content[f++] = "礼包" + item.m_giftId;
                for (k = 0; k<item.m_data.Length;k++ ) 
                {
                    m_content[f++] = item.m_data[k].ToString();
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