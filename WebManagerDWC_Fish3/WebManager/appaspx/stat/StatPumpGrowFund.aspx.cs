using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPumpGrowFund : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","激活人数","打开人数","打开次数","未激活打开人数","未激活打开次数","1级人数",
           "2级人数", "3级人数","4级人数","5级人数","6级人数"};
        //等级
        private static string[] s_head1 = new string[] { "日期","购买人数","等级1", "等级2", "等级3", "等级4", "等级5", "等级6", "等级7", "等级8", "等级9", "等级10", "等级11", "等级12", "等级13", "等级14" };
        //炮倍
        private static string[] s_head2 = new string[] { "日期", "购买人数", "等级1", "等级2", "等级3", "等级4", "等级5", "等级6", "等级7", "等级8" };

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_PUMP_GROW_FUND, Session, Response);
            if (!IsPostBack)
            {
                m_actId.Items.Add("炮倍基金");
                m_actId.Items.Add("等级基金");
                m_actId.Items.Add("成长基金");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_op = m_actId.SelectedIndex;

            OpRes res = OpRes.op_res_failed;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            if (param.m_op == 0) //炮倍基金
            {
                res = mgr.doQuery(param, QueryType.queryTypeStatPumpPaoGrowthFund, user);
                genTable1(m_result, res, user, param);
            }
            else if (param.m_op == 1)  //等级基金
            {
                res = mgr.doQuery(param, QueryType.queryTypeStatPumpLevelGrowthFund, user);
                genTable2(m_result, res, user, param);
            }
            else if (param.m_op == 2)  //成长基金
            {
                res = mgr.doQuery(param, QueryType.queryTypeStatPumpGrowFund, user);
                genTable3(m_result, res, user, param);
            }
        }

        //炮倍
        private void genTable1(Table table, OpRes res, GMUser user, ParamQuery p)
        {
            string[] m_content = new string[s_head2.Length];

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

            int i = 0, k = 0, n = 0;
            for (i = 0; i < s_head2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);

                td.RowSpan = 1;
                td.ColumnSpan = 1;
                td.Text = s_head2[i];
            }

            List<StatPumpPaoLvGrowFundItem> qresult =
                (List<StatPumpPaoLvGrowFundItem>)user.getQueryResult(QueryType.queryTypeStatPumpPaoGrowthFund);

            for (i = 0; i < qresult.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                StatPumpPaoLvGrowFundItem item = qresult[i];
                m_content[n++] = item.m_time;
                m_content[n++] = item.m_rechargeCount.ToString();

                foreach (var da in item.m_receive) 
                {
                    m_content[n++] = da.Value.ToString();
                }

                for (k = 0; k < s_head2.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[k];
                }
            }
        }

        //等级基金
        private void genTable2(Table table, OpRes res, GMUser user, ParamQuery p)
        {
            string[] m_content = new string[s_head1.Length];

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

            int i = 0, k = 0, n = 0;
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);

                td.RowSpan = 1;
                td.ColumnSpan = 1;
                td.Text = s_head1[i];
            }

            List<StatPumpPaoLvGrowFundItem> qresult =
                (List<StatPumpPaoLvGrowFundItem>)user.getQueryResult(QueryType.queryTypeStatPumpLevelGrowthFund);

            for (i = 0; i < qresult.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                StatPumpPaoLvGrowFundItem item = qresult[i];
                m_content[n++] = item.m_time;
                m_content[n++] = item.m_rechargeCount.ToString();

                foreach (var da in item.m_receive)
                {
                    m_content[n++] = da.Value.ToString();
                }

                for (k = 0; k < s_head1.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[k];
                }
            }
        }

        //成长基金
        private void genTable3(Table table, OpRes res, GMUser user, ParamQuery p)
        {
            string[] m_content = new string[s_head.Length];

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

            int i = 0, k = 0, n = 0;
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);

                td.RowSpan = 1;
                td.ColumnSpan = 1;
                td.Text = s_head[i];
            }

            List<StatPumpGrowFundItem> qresult =
                (List<StatPumpGrowFundItem>)user.getQueryResult(QueryType.queryTypeStatPumpGrowFund);

            for (i = 0; i < qresult.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                StatPumpGrowFundItem item = qresult[i];
                m_content[n++] = item.m_time;
                m_content[n++] = item.m_activationPerson.ToString();

                m_content[n++] = item.m_openWithActivationPerson.ToString();
                m_content[n++] = item.m_openWithActivationCount.ToString();

                m_content[n++] = item.m_openWithoutActivationPerson.ToString();
                m_content[n++] = item.m_openWithoutActivationCount.ToString();

                foreach (var da in item.m_receive)
                {
                    m_content[n++] = da.Value.ToString();
                }

                for (k = 0; k < s_head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[k];
                }
            }
        }
    }
}