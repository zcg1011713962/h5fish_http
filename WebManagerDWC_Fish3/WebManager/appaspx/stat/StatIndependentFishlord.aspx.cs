using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatIndependentFishlord : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "类型", "明细" };
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_INDEPEND_DATA, Session, Response);
        }

        protected void onStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(null, QueryType.queryTypeIndependentFishlord, user);
            genTable(m_result, res, user, mgr);
        }

        private void genTable(Table table, OpRes res, GMUser user, QueryMgr mgr)
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

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.Attributes.CssStyle.Value = "min-width:100px";
            }

            ResultFishLord qresult = (ResultFishLord)mgr.getQueryResult(QueryType.queryTypeIndependentFishlord);

            for (i = 1; i < qresult.getRoomCount(); i++)
            {
                if (!StrName.s_roomList.ContainsKey(i) || string.IsNullOrEmpty(StrName.s_roomList[i]))
                    continue;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                m_content[0] = qresult.getRoomName(i);
                m_content[1] = qresult.getEnterRoomCount(i);

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