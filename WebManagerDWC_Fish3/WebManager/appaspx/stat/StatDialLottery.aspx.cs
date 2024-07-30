using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatDialLottery : System.Web.UI.Page
    {
        private PageDialLottery m_gen = new PageDialLottery(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_DIAL_LOTTERY, Session, Response);
            if (!IsPostBack)   //客户端回发而加载
            {
                if (m_gen.parse(Request))
                {
                    m_time.Text = m_gen.m_time;
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            OpRes res = user.doQuery(param, QueryType.queryTypeDialLottery);
            genTable(m_result, res, user,param);
        }

        //查询表
        protected void genTable(Table table, OpRes res, GMUser user,ParamQuery query_param)
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

            ResultMaterialGiftRecharge qresult = (ResultMaterialGiftRecharge)user.getQueryResult(QueryType.queryTypeDialLottery);

            var fields = from f in qresult.m_fields orderby f ascending select f;

            // 生成行标题  表头
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "日期";
            foreach (var reason in fields)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = qresult.getTitleName(Convert.ToInt32(reason));
                td.Attributes.CssStyle.Value = "min-width:150px;";
            }

            //内容
            for (int i = 0; i < qresult.m_result.Count(); i++)
            {
                MaterialGiftItem item = qresult.m_result[i];
                tr = new TableRow();
                m_result.Rows.Add(tr);
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time.ToShortDateString();

                // 生成这个结果
                foreach (var reason in fields)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    if (item.m_number.ContainsKey(reason))
                    {
                        td.Text = item.m_number[reason].ToString();
                    }
                    else
                    {
                        td.Text = "0";
                    }
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatDialLottery.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}