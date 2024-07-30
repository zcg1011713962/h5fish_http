using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationAdPutInSet : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "活动ID", "活动名称", "活动时间", "操作" };
        private string[] m_content = new string[s_head.Length];
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_ACTIVITY_CFG, Session, Response);
            if (!IsPostBack)
            {
                GMUser user = (GMUser)Session["user"];
                ParamQuery param = new ParamQuery();

                OpRes res = user.doQuery(param, QueryType.queryTypeOperationAdActSet);
                genTable(m_result, res, user);
            }
        }

        private void genTable(Table table, OpRes res, GMUser user)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            //不存在，新增
            if (res != OpRes.opres_success)
            {
                //td = new TableCell();
                //tr.Cells.Add(td);
                //td.Text = OpResMgr.getInstance().getResultString(res);

                m_AddAdTime.Attributes.CssStyle.Value = "display:block;margin-left:48%";

                return;
            }
            else {
                m_AddAdTime.Attributes.CssStyle.Value = "display:none";
            }
            int i = 0, n = 0, j = 0;
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            List<AdActItem> qresult =
                (List<AdActItem>)user.getQueryResult(QueryType.queryTypeOperationAdActSet);

            for (i = 0; i < qresult.Count; i++)
            {
                n = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                AdActItem item = qresult[i];
                m_content[n++] = item.m_key.ToString();
                m_content[n++] = "100003广告投放活动";
                m_content[n++] = item.m_startTime.ToString() + " 至 " + item.m_endTime.ToString();
                m_content[n++] = "编辑";

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.Attributes.CssStyle.Value = "vertical-align:middle";
                    if (j == 3)
                    {
                        HtmlGenericControl alink = new HtmlGenericControl();
                        alink.TagName = "a";
                        alink.InnerText = "编辑";
                        alink.Attributes.Add("class", "btn btn-primary btn-edit");
                        alink.Attributes.Add("itemId", item.m_key.ToString());
                        alink.Attributes.Add("startTime", item.m_startTime);
                        alink.Attributes.Add("endTime", item.m_endTime);

                        td.Controls.Add(alink);
                    }
                }
            }
        }
    }
}