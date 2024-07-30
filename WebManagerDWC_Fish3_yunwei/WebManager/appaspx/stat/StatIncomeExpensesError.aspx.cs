using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatIncomeExpensesError : System.Web.UI.Page
    {
        private static string[] s_head0 = new String[] {"时间","玩家ID", "金币错误", "钻石错误", "龙珠错误", "话费券错误" };
        private static string[] s_head1 = new String[] { "时间", "玩家ID", "金币错误" };
        private static string[] s_head2 = new String[] { "时间", "玩家ID", "钻石错误" };
        private static string[] s_head3 = new String[] { "时间", "玩家ID", "龙珠错误" };
        private static string[] s_head4 = new String[] { "时间", "玩家ID", "话费券错误"};
        private string[] m_content = new String[s_head0.Length];
        private PageGenIncomeExpensesError m_gen = new PageGenIncomeExpensesError(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);
            if(!IsPostBack)
            {
                m_showWay.Items.Add(new ListItem("全部", ErrorType.ERRORTYPE_ALL.ToString()));
                m_showWay.Items.Add(new ListItem("金币错误", ErrorType.ERRORTYPE_GOLD.ToString()));
                m_showWay.Items.Add(new ListItem("钻石错误", ErrorType.ERRORTYPE_GEM.ToString()));
                m_showWay.Items.Add(new ListItem("龙珠错误", ErrorType.ERRORTYPE_DB.ToString()));
                m_showWay.Items.Add(new ListItem("话费券错误", ErrorType.ERRORTYPE_CHIP.ToString()));

                if(m_gen.parse(Request))
                {
                    m_time.Text = m_gen.m_time;
                    for (int i = 0; i < m_showWay.Items.Count; i++)
                    {
                        if (m_showWay.Items[i].Value == m_gen.m_showWay.ToString())
                        {
                            m_showWay.Items[i].Selected = true;
                            break;
                        }
                    }
                    onQueryError(null,null);
                }
            }
        }

        protected void onQueryError(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = genParamQuery();
            QueryMgr mgr=user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeIncomeExpensesError,user);
            genTable(m_result,res,param,user,mgr);
        }

        private ParamQuery genParamQuery() 
        {
            ParamQuery param = new ParamQuery();
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            param.m_time = m_time.Text;
            param.m_showWay = m_showWay.SelectedIndex.ToString();
            return param;
        }

        private void genTable(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";
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
            List<IncomeExpensesError> qresult = (List<IncomeExpensesError>)mgr.getQueryResult(QueryType.queryTypeIncomeExpensesError);
            int i = 0, j = 0;

            int way = Convert.ToInt32(query_param.m_showWay); //查看方式
            switch (way)
            {
                case 0:  // 表头 
                    for (i = 0; i < s_head0.Length; i++)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = s_head0[i];
                    }
                    for (i = 0; i < qresult.Count; i++)
                    {
                        tr = new TableRow();
                        m_result.Rows.Add(tr);
                        int f = 0;
                        IncomeExpensesError item = qresult[i];
                        m_content[f++] = item.m_createTime;
                        m_content[f++] = item.m_playerId.ToString();
                        m_content[f++] = getError(item.m_goldError);
                        m_content[f++] = getError(item.m_gemError);
                        m_content[f++] = getError(item.m_dbError);
                        m_content[f++] = getError(item.m_chipError);
                        for (j = 0; j < s_head0.Length; j++)
                        {
                            td = new TableCell();
                            tr.Cells.Add(td);
                            td.Text = m_content[j];
                        }
                    }
                    break;
                case 1:// 表头  金币错误
                    for (i = 0; i < s_head1.Length; i++)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = s_head1[i];
                    }
                    for (i = 0; i < qresult.Count; i++)
                    {
                        tr = new TableRow();
                        m_result.Rows.Add(tr);
                        int f = 0;
                        IncomeExpensesError item = qresult[i];
                        m_content[f++] = item.m_createTime;
                        m_content[f++] = item.m_playerId.ToString();
                        m_content[f++] = getError(item.m_goldError);
                        for (j = 0; j < s_head1.Length; j++)
                        {
                            td = new TableCell();
                            tr.Cells.Add(td);
                            td.Text = m_content[j];
                        }
                    }
                    break;
                case 2:// 表头 
                    for (i = 0; i < s_head2.Length; i++)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = s_head2[i];
                    }
                    for (i = 0; i < qresult.Count; i++)
                    {
                        tr = new TableRow();
                        m_result.Rows.Add(tr);
                        int f = 0;
                        IncomeExpensesError item = qresult[i];
                        m_content[f++] = item.m_createTime;
                        m_content[f++] = item.m_playerId.ToString();
                        m_content[f++] = getError(item.m_gemError);

                        for (j = 0; j < s_head2.Length; j++)
                        {
                            td = new TableCell();
                            tr.Cells.Add(td);
                            td.Text = m_content[j];
                        }
                    }
                    break;
                case 3:// 表头 
                    for (i = 0; i < s_head3.Length; i++)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = s_head3[i];
                    }
                    for (i = 0; i < qresult.Count; i++)
                    {
                        tr = new TableRow();
                        m_result.Rows.Add(tr);
                        int f = 0;
                        IncomeExpensesError item = qresult[i];
                        m_content[f++] = item.m_createTime;
                        m_content[f++] = item.m_playerId.ToString();
                        m_content[f++] = getError(item.m_dbError);

                        for (j = 0; j < s_head3.Length; j++)
                        {
                            td = new TableCell();
                            tr.Cells.Add(td);
                            td.Text = m_content[j];
                        }
                    }
                    break;
                case 4:// 表头 
                    for (i = 0; i < s_head4.Length; i++)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = s_head4[i];
                    }
                    for (i = 0; i < qresult.Count; i++)
                    {
                        tr = new TableRow();
                        m_result.Rows.Add(tr);
                        int f = 0;
                        IncomeExpensesError item = qresult[i];
                        m_content[f++] = item.m_createTime;
                        m_content[f++] = item.m_playerId.ToString();
                        m_content[f++] = getError(item.m_chipError);

                        for (j = 0; j < s_head4.Length; j++)
                        {
                            td = new TableCell();
                            tr.Cells.Add(td);
                            td.Text = m_content[j];
                        }
                    }
                    break;
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatIncomeExpensesError.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        public string getError(string p)
        {
            if (Convert.ToBoolean(p))
            {
                return "是";
            }
            else
            {
                return "否";
            }

        }
    }
}