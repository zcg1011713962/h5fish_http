using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.service
{
    public partial class ServiceAccountBeibao : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "道具名称","道具数量"};
        private string[] m_content=new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("",Session,Response);
            string playerId = Request.QueryString["playerId"];
            if (string.IsNullOrEmpty(playerId))
                return;
            
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_playerId = playerId;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeAccountBeibao, user);

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

            List<AccountBeibaoItem> qresult = (List<AccountBeibaoItem>)mgr.getQueryResult(QueryType.queryTypeAccountBeibao);
            
            td = new TableCell();
            tr.Cells.Add(td);
            td.ColumnSpan = 2;
            td.Text = "玩家"+param.m_playerId +"背包清单";
            int i = 0, j = 0;
            // 表头
            tr = new TableRow();
            table.Rows.Add(tr);
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                int index = 0;
                m_content[index++] = getItemName(qresult[i].m_itemList.m_itemId);
                m_content[index++] = qresult[i].m_itemList.m_itemCount.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }

        public string getItemName(int key)
        {
            string itemName = key.ToString();
            ItemCFGData item = ItemCFG.getInstance().getValue(key);
            if (item != null)
            {
                itemName = item.m_itemName;
            }
            return itemName;
        }
    }
}