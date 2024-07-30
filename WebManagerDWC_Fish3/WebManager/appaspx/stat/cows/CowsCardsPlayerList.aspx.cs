using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat.cows
{
    public partial class CowsCardsPlayerList : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "玩家ID", "抽水情况", "东", "输赢情况", "南", "输赢情况", "西", "输赢情况", "北" ,"输赢情况"};
        private string[] m_content = new string[s_head.Length];
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);
            
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_param = Convert.ToString(Request.QueryString["cardId"]); //cardId
            param.m_op = Convert.ToInt32(Request.QueryString["roomId"]); //roomId
            
            OpRes res = user.doQuery(param, QueryType.queryTypeCowsCardsPlayerList);
            genTable(m_result, res, user,param);
        }

        private void genTable(Table table, OpRes res, GMUser user,ParamQuery p)
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
            List<CowsPlayerItem> qresult = (List<CowsPlayerItem>)user.getQueryResult(QueryType.queryTypeCowsCardsPlayerList);

            //牌局ID
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "牌局ID：" + p.m_param;
            td.ColumnSpan = s_head.Length;

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
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                CowsPlayerItem item = qresult[i];
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_pumpMoney.ToString();
                m_content[f++] = item.m_betMoney0.ToString();
                m_content[f++] = item.m_winGold0.ToString();
                m_content[f++] = item.m_betMoney1.ToString();
                m_content[f++] = item.m_winGold1.ToString();
                m_content[f++] = item.m_betMoney2.ToString();
                m_content[f++] = item.m_winGold2.ToString();
                m_content[f++] = item.m_betMoney3.ToString();
                m_content[f++] = item.m_winGold3.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.ColumnSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }
    }
}