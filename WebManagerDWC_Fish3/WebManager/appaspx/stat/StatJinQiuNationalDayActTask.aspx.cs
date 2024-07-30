using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatJinQiuNationalDayActTask : System.Web.UI.Page
    {
        private static int[] s_pao = new int[] { 29,30,40,50,60,70,80,90,100,150,200,250,300,350,400,450,500,550,600,650,700,750,800,850,
            900,950,1000,1200,1400,1600,1800,2000,2300,2600,3000,3500,4000,4500,5000,5500,6000,6500,7000,7500,8000,8500,9000,9500,10000};

        private PageJiuQiuNationalActPlayer m_gen = new PageJiuQiuNationalActPlayer(50);

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
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeJinQiuNationalDayActTaskDetail, user);

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

            List<ActTaskDetailList> qresult = (List<ActTaskDetailList>)mgr.getQueryResult(QueryType.queryTypeJinQiuNationalDayActTaskDetail);

            int i = 0, j = 0, pao = 0;
            // 表头
            tr = new TableRow();
            table.Rows.Add(tr);

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "日期";
            td.ColumnSpan = 1;

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "类型";
            td.ColumnSpan = 1;
            td.Attributes.CssStyle.Value = "min-width:100px";

            for (i = 0; i < s_pao.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                if (i == 0) {
                    td.Text = "<30";
                }else{
                    td.Text = s_pao[i].ToString();
                }
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                ActTaskDetailList item = qresult[i];

                tr = new TableRow();
                table.Rows.Add(tr);

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time;
                td.RowSpan = 6;
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "t1完成人次";
                td.RowSpan = 1;

                for (j = 0; j < s_pao.Length; j++) 
                {
                    pao = s_pao[j];
                    td = new TableCell();
                    tr.Cells.Add(td);
                    if (item.m_data.ContainsKey(pao))
                    {
                        td.Text = item.m_data[pao].m_t1Finish.ToString();
                    }else {
                        td.Text = "";
                    }
                }

                tr = new TableRow();
                table.Rows.Add(tr);
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "t2完成人次";

                for (j = 0; j < s_pao.Length; j++)
                {
                    pao = s_pao[j];

                    td = new TableCell();
                    tr.Cells.Add(td);
                    if (item.m_data.ContainsKey(pao))
                    {
                        td.Text = item.m_data[pao].m_t2Finish.ToString();
                    }else {
                        td.Text = "";
                    }
                }
                tr = new TableRow();
                table.Rows.Add(tr);
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "t3完成人次";

                for (j = 0; j < s_pao.Length; j++)
                {
                    pao = s_pao[j];

                    td = new TableCell();
                    tr.Cells.Add(td);
                    if (item.m_data.ContainsKey(pao))
                    {
                        td.Text = item.m_data[pao].m_t3Finish.ToString();
                    }else {
                        td.Text = "";
                    }
                }
                tr = new TableRow();
                table.Rows.Add(tr);
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "t1领取人次";

                for (j = 0; j < s_pao.Length; j++)
                {
                    pao = s_pao[j];
                    td = new TableCell();
                    tr.Cells.Add(td);
                    if (item.m_data.ContainsKey(pao))
                    {
                        td.Text = item.m_data[pao].m_t1Receive.ToString();
                    }else {
                        td.Text = "";
                    }
                }
                tr = new TableRow();
                table.Rows.Add(tr);
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "t2领取人次";

                for (j = 0; j < s_pao.Length; j++)
                {
                    pao = s_pao[j];
                    td = new TableCell();
                    tr.Cells.Add(td);
                    if (item.m_data.ContainsKey(pao))
                    {
                        td.Text = item.m_data[pao].m_t2Receive.ToString();
                    }else
                    {
                        td.Text = "";
                    }
                }
                tr = new TableRow();
                table.Rows.Add(tr);
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "t3领取人次";

                for (j = 0; j < s_pao.Length; j++)
                {
                    pao = s_pao[j];
                    td = new TableCell();
                    tr.Cells.Add(td);
                    if (item.m_data.ContainsKey(pao))
                    {
                        td.Text = item.m_data[pao].m_t3Receive.ToString();
                    }else{
                        td.Text = "";
                    }
                }
            }
        }
    }
}