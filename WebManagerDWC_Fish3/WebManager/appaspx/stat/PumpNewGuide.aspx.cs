using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class PumpNewGuide : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "ID", "内容", "统计总人数(人)", "当日完成(人)", "后续完成(人)", "当日新增(人)", "完成百分比(统计总人数/当日新增)" };
        private string[] m_content = new string[s_head.Length];
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_PUMP_NEW_GUIDE, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypePumpNewGuide, user);
            genTable(m_result, res, param, user, mgr);
        }

        //生成查询表
        private void genTable(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
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
            List<pumpNewGuideItem> qresult = (List<pumpNewGuideItem>)mgr.getQueryResult(QueryType.queryTypePumpNewGuide);
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
                pumpNewGuideItem item = qresult[i];

                List<newGuideStepItem> list = item.m_data;
                for (j = 0; j < list.Count; j++)
                {
                    tr = new TableRow();
                    table.Rows.Add(tr);

                    if(j==0)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_time;
                        td.RowSpan = item.m_flag;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = list[j].m_step.ToString();
                    td.RowSpan = 1;

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = list[j].m_stepName.ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = list[j].m_totalFinish.ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = list[j].m_thisDayFinish.ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = list[j].m_followFinish.ToString();

                    if(j==0)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_thisDayAdd.ToString();
                        td.RowSpan = item.m_flag;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = list[j].getFinishPercent(list[j].m_totalFinish,item.m_thisDayAdd);
                    td.RowSpan = 1;
                }
            }
        }

    }
}