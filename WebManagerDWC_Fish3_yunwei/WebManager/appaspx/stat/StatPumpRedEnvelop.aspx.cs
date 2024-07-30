using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPumpRedEnvelop : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "渠道", "红包1", "红包2", "红包3", "红包4", "红包5", "红包6", "红包7", 
            "红包8", "红包9" ,"红包10","红包11","红包12","红包13","红包14","红包15","红包16","红包17","红包18","红包19","红包20"};
        private string[] m_content = new string[s_head.Length];
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_PUMP_RED_ENVELOP, Session, Response);
            if (!IsPostBack)
            {
                m_channel.Items.Add(new ListItem("总体", "-1"));
                m_channel.Items.Add(new ListItem("安卓总体", "-2"));

                m_channel.Items.Add(new ListItem("微信", "100001"));
                m_channel.Items.Add(new ListItem("QQ", "100002"));
                m_channel.Items.Add(new ListItem("街机疯狂捕鱼-武汉多游", "000971"));
            }
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_param = m_channel.SelectedValue;
            OpRes res = user.doQuery(param, QueryType.queryTypeStatPumpRedEnvelop);
            genTable(m_result, res, user, param);
        }

        //红包方案
        private void genTable(Table table, OpRes res, GMUser user, ParamQuery query_param)
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

            List<StataPumpRedEnvelopItem> qresult =
                (List<StataPumpRedEnvelopItem>)user.getQueryResult(QueryType.queryTypeStatPumpRedEnvelop);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.RowSpan = 1;
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                StataPumpRedEnvelopItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.getChannelName(query_param.m_param);
                foreach (var da in item.m_data)
                {
                    m_content[f++] = da.Value.ToString();
                }
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }
    }
}