using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationPlayerBankrupt : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","炮倍","活跃人数","破产人数","破产次数","破产率（破产人数/活跃人数）",
            "破产付费人数","破产付费次数","人均破产次数（破产次数/破产人数）","人均破产付费率（破产付费人数/破产人数）","次数破产付费率（破产付费次数/破产次数）",
            "详情"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_PLAYER_BANKRUPT, Session, Response);
            if (!IsPostBack)
            {
                m_level.Items.Add(new ListItem("全部", "0"));
                var turret = Fish_TurretLevelCFG.getInstance().getAllData();
                if (turret != null) 
                {
                    foreach(var item in turret.Values)
                    {
                        m_level.Items.Add(new ListItem( item.m_openRate +"炮", item.m_level.ToString()));
                    }
                }

                //渠道
                m_channel.Items.Add(new ListItem("总体", "-1"));
                m_channel.Items.Add(new ListItem("安卓总体", "-2"));
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = Convert.ToInt32(m_level.SelectedValue);
            param.m_time = m_time.Text;
            param.m_param = m_channel.SelectedValue;
            OpRes res = OpRes.op_res_failed;

            res = user.doQuery(param, QueryType.queryTypeOperationPlayerBankruptStat);
            genTable(m_result, res, user, param);
        }

        private void genTable(Table table, OpRes res, GMUser user, ParamQuery p)
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

            int i = 0, k = 0, n = 0;
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            List<StatBankruptItem> qresult =
                (List<StatBankruptItem>)user.getQueryResult(QueryType.queryTypeOperationPlayerBankruptStat);

            for (i = 0; i < qresult.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                StatBankruptItem item = qresult[i];
                m_content[n++] = item.m_time;
                m_content[n++] = item.getPaoName(p.m_op);
                m_content[n++] = item.m_todayActivePlayerCheck.ToString();
                m_content[n++] = item.m_benifitPlayerCheck.ToString();
                m_content[n++] = item.m_todayBenifitTimes.ToString();
                m_content[n++] = item.getRate(item.m_benifitPlayerCheck, item.m_todayActivePlayerCheck);
                m_content[n++] = item.m_payAfterBenifitPlayerCheck.ToString();
                m_content[n++] = item.m_todayPayAfterBenifitTimes.ToString();
                m_content[n++] = item.getRate(item.m_todayBenifitTimes, item.m_benifitPlayerCheck);
                m_content[n++] = item.getRate(item.m_payAfterBenifitPlayerCheck, item.m_benifitPlayerCheck);
                m_content[n++] = item.getRate(item.m_todayPayAfterBenifitTimes, item.m_todayBenifitTimes);
                m_content[n++] = item.getDetail(p.m_op, p.m_param);

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