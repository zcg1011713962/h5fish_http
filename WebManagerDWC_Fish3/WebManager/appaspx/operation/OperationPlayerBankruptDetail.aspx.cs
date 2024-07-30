using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationPlayerBankruptDetail : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "时间","玩家ID","炮倍","场次"};
        private string[] m_content = new string[s_head.Length];
        private PagePlayerBankruptDetail m_gen = new PagePlayerBankruptDetail(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);

            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = Request.QueryString["time"];
            param.m_op = Convert.ToInt32(Request.QueryString["lotteryId"]);
            param.m_param = Request.QueryString["channelType"];
            if (m_gen.parse(Request))
            {
                param.m_curPage = m_gen.curPage;
                param.m_countEachPage = m_gen.rowEachPage;
            }
            //////////////////////////////////////////////////////////////
            genTable(m_result, param, user);
        }

        private void genTable(Table table, ParamQuery param, GMUser user)
        {
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeOperationPlayerBankruptDetail, user);

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

            List<StatBankruptDetailItem> qresult =
                (List<StatBankruptDetailItem>)mgr.getQueryResult(QueryType.queryTypeOperationPlayerBankruptDetail);

            int i = 0, j = 0, f = 0;
            // 表头第一行
            tr = new TableRow();
            table.Rows.Add(tr);
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = param.m_param == "-1" ? "渠道总体":"安卓总体";
            td.ColumnSpan = 4;

            tr = new TableRow();
            table.Rows.Add(tr);
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                table.Rows.Add(tr);

                StatBankruptDetailItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.getTurretName();
                m_content[f++] = StrName.s_roomList[item.m_roomId];

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/operation/OperationPlayerBankruptDetail.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;

        }
    }
}