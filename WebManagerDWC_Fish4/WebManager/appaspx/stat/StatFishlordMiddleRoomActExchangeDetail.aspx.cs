using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordMiddleRoomActExchangeDatail : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "玩家ID", "兑换类型6", "兑换类型7", "兑换类型8"};
        private static string[] s_head1 = new string[]{"兑换次数","额外次数"};

        private PagePlayerPumpBw m_gen = new PagePlayerPumpBw(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);
            ////////////////////////////////////////////////////////////////

            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = Request.QueryString["time"];
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
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatFishlordMiddleRoomExchangeDetail, user);

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

            List<StatMiddleRoomExchangeDetailItem> qresult =
                (List<StatMiddleRoomExchangeDetailItem>)mgr.getQueryResult(QueryType.queryTypeStatFishlordMiddleRoomExchangeDetail);

            td = new TableCell();
            tr.Cells.Add(td);
            td.ColumnSpan = 7;
            td.Text = "时间： " + param.m_time + "&nbsp;&nbsp;&nbsp;";

            int i = 0, j = 0, key = 0;
            // 表头第一行
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

            //表头第二行
            tr = new TableRow();
            table.Rows.Add(tr);
            for (i = 0; i < s_head.Length - 1; i++)
            {
                for (j = 0; j < s_head1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = s_head1[j];
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                }
            }

            for (i = 0; i < qresult.Count; i++)
            {
                StatMiddleRoomExchangeDetailItem item = qresult[i];

                tr = new TableRow();
                table.Rows.Add(tr);

                key = 0;
                foreach (var da in item.m_exchangeList)
                {
                    if (key == 0)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_playerId.ToString();
                        td.RowSpan = 1;
                        td.ColumnSpan = 1;
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.Value[0].ToString();
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.Value[1].ToString();
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;

                    key++;
                }
                
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatFishlordMiddleRoomActExchangeDetail.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}