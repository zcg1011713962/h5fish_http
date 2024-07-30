using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatRebateGift : System.Web.UI.Page
    {
        private static string[] s_head = new string[] {"时间","打开人数","打开次数","购买人数"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_REBATE_GIFT, Session, Response);
            if (!IsPostBack)
            {
                m_type.Items.Add("1元返利");
                m_type.Items.Add("3元返利");
                m_type.Items.Add("6元返利");
                m_type.Items.Add("12元返利");
                m_type.Items.Add("30元返利");
                m_type.Items.Add("50元返利");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;

            param.m_type = m_type.SelectedIndex;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatRebateGift, user);

            genTable(m_result, res, user, mgr, param);
        }

        //生成查询表
        private void genTable(Table table, OpRes res, GMUser user, QueryMgr mgr, ParamQuery param)
        {
            string[] m_content = new string[s_head.Length];

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
            List<StatRebateGiftItem> qresult = (List<StatRebateGiftItem>)mgr.getQueryResult(QueryType.queryTypeStatRebateGift);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;
                StatRebateGiftItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_openPerson.ToString();
                m_content[f++] = item.m_openCount.ToString();
                m_content[f++] = item.m_rechargePerson.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Attributes.CssStyle.Value = "min-width:100px";
                    td.Text = m_content[j];
                }
            }
        }
    }
}