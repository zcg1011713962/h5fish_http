using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace WebManager.appaspx.stat.shuihz
{
    public partial class ShuihzSingleEarning : System.Web.UI.Page
    {
        private static string[] s_head = { "玩家ID","时间", "系统总收入", "系统总支出", "系统盈利率","充值"};
        private string[] m_content = new string[s_head.Length];
        private PageShuihzSingleEarning m_gen = new PageShuihzSingleEarning(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.SHUIHZ_SINGLE_EARNING, Session, Response);
            if (IsPostBack)   //客户端回发而加载
            {

            }
            else
            {
                if (m_gen.parse(Request))
                {
                    m_time.Text = m_gen.m_time;
                    m_param.Text = m_gen.m_param;
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_param = m_param.Text;
            param.m_time = m_time.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            OpRes res = user.doQuery(param, QueryType.queryTypeShuihzSingleEarning);
            genTable(m_result, res, user,param);

        }

        //生成表
        private void genTable(Table table, OpRes res, GMUser user,ParamQuery param)
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

            List<ResultShuihzEarningItem> qresult = (List<ResultShuihzEarningItem>)user.getQueryResult(QueryType.queryTypeShuihzSingleEarning);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                ResultShuihzEarningItem item = qresult[i];

                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_totalIncome.ToString();
                m_content[f++] = item.m_totalOutlay.ToString();
                m_content[f++] = item.getFactExpRate().ToString();
                m_content[f++] = item.m_gameTotal.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    if (j == 4)
                    {
                        setColor(td, m_content[j]);
                    }
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/shuihz/ShuihzSingleEarning.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        protected void setColor(TableCell td, string num)
        {
            if (num[0] == '-')
            {
                td.ForeColor = Color.Red;
            }
            else
            {
                td.ForeColor = Color.Green;
            }
        }
    }
}