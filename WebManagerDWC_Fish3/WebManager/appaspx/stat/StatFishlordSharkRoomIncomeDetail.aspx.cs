using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordSharkRoomIncomeDetail : System.Web.UI.Page
    {

        private static string[] s_head = new string[] {"玩家ID","玩家昵称","中奖内容","中奖金额","使用炮倍"};
        private string[] m_content = new string[s_head.Length];

        private PageJiuQiuNationalActPlayer m_gen = new PageJiuQiuNationalActPlayer(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);
            ////////////////////////////////////////////////////////////////
            string lottery_id = Request.QueryString["lotteryId"];
            if (string.IsNullOrEmpty(lottery_id))
                return;

            int lotteryId = 0;
            if (!int.TryParse(lottery_id, out lotteryId))
                return;

            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = lotteryId;
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
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatFishlordSharkRoomIncomeDetail, user);

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

            List<StatSharkRoomDetailItem> qresult = 
                (List<StatSharkRoomDetailItem>)mgr.getQueryResult(QueryType.queryTypeStatFishlordSharkRoomIncomeDetail);

            td = new TableCell();
            tr.Cells.Add(td);
            td.ColumnSpan = 5;

             if(param.m_op ==22)
             {
                 td.Text = "时间： " + param.m_time + "&nbsp;&nbsp;&nbsp;抽奖类型：武装巨蟹";
             }
             else if (param.m_op == 23)
             {
                 td.Text = "时间： " + param.m_time + "&nbsp;&nbsp;&nbsp;抽奖类型：武装海豹";
             }
             else 
             {
                 td.Text = "时间： " + param.m_time + "&nbsp;&nbsp;&nbsp;";
             }

            int i = 0, j = 0, index = 0;
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
                index = 0;
                tr = new TableRow();
                table.Rows.Add(tr);

                StatSharkRoomDetailItem item = qresult[i];
                m_content[index++] = item.m_playerId.ToString();
                m_content[index++] = item.m_nickName;
                m_content[index++] = item.getIndexName();
                m_content[index++] = item.m_gold.ToString();
                m_content[index++] = item.m_bulletrate.ToString();
               
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatFishlordSharkRoomIncomeDetail.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

    }
}