using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat.shcd
{
    public partial class ShcdCardsPlayerList : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "玩家ID", "黑桃", "红心", "梅花", "方块","大小王"};
        private string[] m_content = new string[s_head.Length+5];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);

            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_param = Convert.ToString(Request.QueryString["cardId"]); //cardId
            param.m_op = Convert.ToInt32(Request.QueryString["roomId"]); //roomId

            OpRes res = user.doQuery(param, QueryType.queryTypeShcdCardsPlayerList);
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
            List<InfoShcd> qresult = (List<InfoShcd>)user.getQueryResult(QueryType.queryTypeShcdCardsPlayerList);

            //牌局ID
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "牌局ID：" + p.m_param;
            td.ColumnSpan = s_head.Length+5;

            int i = 0, j = 0;
            // 表头
            tr = new TableRow();
            table.Rows.Add(tr);
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                if (i == 0)
                {
                    td.RowSpan = 2;
                    td.ColumnSpan = 1;
                }
                else 
                {
                    td.RowSpan = 1;
                    td.ColumnSpan = 2;
                }
            }

            tr = new TableRow();
            table.Rows.Add(tr);
            for (i = 0; i<5;i++ ) 
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.ColumnSpan = 1;
                td.Text = "下注";

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "获得";
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                InfoShcd item = qresult[i];
                m_content[f++] = item.m_playerId.ToString();

                foreach(var info in item.betinfo)
                {
                    m_content[f++] = info.bet_count.ToString();
                    m_content[f++] = info.award_count.ToString();
                }

                for (j = 0; j < s_head.Length+5; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.ColumnSpan = 1;
                }
            }
        }
    }
}