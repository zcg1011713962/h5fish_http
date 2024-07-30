using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationActivityCFGNew : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "活动ID", "活动名称", /*"日期", */"开始日期","结束日期", "操作" };
        private string[] m_content = new string[s_head.Length];
        private PageStatActCFG m_gen = new PageStatActCFG(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            m_foot.InnerHtml = "";
            m_page.InnerHtml = "";

            m_res.InnerHtml = "";

            RightMgr.getInstance().opCheck(RightDef.OP_ACTIVITY_CFG_NEW, Session, Response);

            if (!IsPostBack)
            {
                var data = ActivityCFG.getInstance().getAllData();
                foreach (var item in data.Values) 
                {
                    m_actList.Items.Add(new ListItem(item.m_activityName, item.m_activityId.ToString()));
                }

                m_actList1.Items.Add(new ListItem("全部", "-1"));
                foreach (var item in data.Values)
                {
                    m_actList1.Items.Add(new ListItem(item.m_activityName, item.m_activityId.ToString()));
                }

                if (m_gen.parse(Request))
                {
                    //m_time.Text = m_gen.m_time;
                    m_actList1.SelectedValue = m_gen.m_op.ToString();
                    onQuery(null, null);
                }
            }
        }

        protected void onEdit(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            OpRes res = OpRes.op_res_failed;
            ParamQuery p = new ParamQuery();
            p.m_op = 0; //0新增   1编辑  2删除
            p.m_type = Convert.ToInt32(m_actList.SelectedValue);
            p.m_param = m_monthday.Text;

            res = user.doDyop(p, DyOpType.opTypeOperationActivityCFGNew);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }


        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery p = new ParamQuery();
            p.m_op = Convert.ToInt32(m_actList1.SelectedValue);
            //p.m_time = m_time.Text;

            p.m_curPage = m_gen.curPage;
            p.m_countEachPage = m_gen.rowEachPage;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(p, QueryType.queryTypeOperationActivityCFGNew, user);

            genTable(res, p, user, mgr);
        }

        private void genTable(OpRes res, ParamQuery param, GMUser user, QueryMgr mgr)
        {
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
            List<StatOperationActivityCFGNewItem> qresult =
                (List<StatOperationActivityCFGNewItem>)mgr.getQueryResult(QueryType.queryTypeOperationActivityCFGNew);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for(i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;
                m_content[f++] = qresult[i].m_actId.ToString();
                m_content[f++] = qresult[i].getActName();
                //m_content[f++] = qresult[i].m_date;
                m_content[f++] = qresult[i].m_startTime;
                m_content[f++] = qresult[i].m_endTime;
                m_content[f++] = "0";
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    if (j == 4)
                    {
                     // HtmlGenericControl alink = new HtmlGenericControl();
                     // alink.TagName = "a";
                     // alink.InnerText = "编辑";
                     // alink.Attributes.Add("class", "btn btn_edit");
                     // alink.Attributes.Add("id", qresult[i].m_id);
                     // alink.Attributes.Add("monday", qresult[i].m_date);
                     // alink.Attributes.Add("actId", qresult[i].m_actId.ToString());
                     // td.Controls.Add(alink);

                        HtmlGenericControl alink1 = new HtmlGenericControl();
                        alink1.TagName = "a";
                        alink1.InnerText = "删除";
                        alink1.Attributes.Add("class", "btn btn_delete");
                        alink1.Attributes.Add("id", qresult[i].m_id);
                        td.Controls.Add(alink1);
                    }

                    td.Attributes.CssStyle.Value = "vertical-align:middle";
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/operation/OperationActivityCFGNew.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}