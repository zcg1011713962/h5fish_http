using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operationNew
{
    public partial class OperationNewGameActive : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "渠道", "注册人数", "设备激活", "活跃人数", "设备活跃", 
            "次日留存", "3日留存", "7日留存", "30日留存"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OPNEW_GAME_ACTIVE_DATA, Session, Response);
            if (!IsPostBack)
            {
                m_channel.Items.Add(new ListItem("总体", "-1"));
                m_channel.Items.Add(new ListItem("全渠道", "-2"));
                Dictionary<string, TdChannelInfo> data = TdChannel.getInstance().getAllData();
                foreach (var item in data.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }
            }
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            ParamQuery param = new ParamQuery();
            param.m_param = m_channel.SelectedValue;
            param.m_time = m_time.Text;
            param.m_param = m_channel.SelectedValue;
            OpRes res = user.doQuery(param, QueryType.queryTypeOpnewGameActive);
            genTable(user, m_resTable, res);
        }

        public void genTable(GMUser user, Table table, OpRes res)
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

            int i = 0, f = 0;
            for (; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                if (i == 0 || i == 1)
                    td.Attributes.CssStyle.Value = "min-width:120px";
            }

            List<gameIncomeItem> qresult = (List<gameIncomeItem>)user.getQueryResult(QueryType.queryTypeOpnewGameActive);
            foreach (var data in qresult)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                m_content[f++] = data.m_genTime.ToShortDateString();
                TdChannelInfo info = TdChannel.getInstance().getValue(data.m_channel);
                if (info != null)
                {
                    m_content[f++] = info.m_channelName;
                }
                else
                {
                    m_content[f++] = data.m_channel;
                }

                m_content[f++] = data.m_regeditCount.ToString();
                m_content[f++] = data.m_deviceActivationCount.ToString();

                //活跃人数
                m_content[f++] = data.m_activeCount.ToString();
                //设备活跃
                m_content[f++] = data.m_deviceLoginCount.ToString();

                m_content[f++] = data.getRechargeRate(data.m_2DayRemainCount, data.m_deviceActivationCount);
                m_content[f++] = data.getRechargeRate(data.m_3DayRemainCount, data.m_deviceActivationCount);
                m_content[f++] = data.getRechargeRate(data.m_7DayRemainCount, data.m_deviceActivationCount);
                m_content[f++] = data.getRechargeRate(data.m_30DayRemainCount, data.m_deviceActivationCount);
                for (int j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }

    }
}