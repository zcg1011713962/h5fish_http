using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operationNew
{
    public partial class OperationNewGameIncome : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "渠道", "收入", "注册人数", "设备激活", "活跃人数", "设备活跃", "付费人数", "付费次数", "付费率", "ARPU", "ARPPU", "新增用户付费", "新增用户付费率" };
        private string[] m_content = new string[s_head.Length];
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OPNEW_GAME_INCOME_DATA, Session, Response);
            if (!IsPostBack) 
            {
                m_channel.Items.Add(new ListItem("总体","-1"));
                m_channel.Items.Add(new ListItem("全渠道","-2"));
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

            TableTdActivation view = new TableTdActivation();
            ParamQuery param = new ParamQuery();
            param.m_param = m_channel.SelectedValue;
            param.m_time = m_time.Text;
            param.m_param = m_channel.SelectedValue;
            OpRes res = user.doQuery(param, QueryType.queryTypeOpnewGameIncome);
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

            i = 0;
            List<gameIncomeItem> qresult = (List<gameIncomeItem>)user.getQueryResult(QueryType.queryTypeOpnewGameIncome);
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
                else{
                    m_content[f++] = data.m_channel;
                }

                m_content[f++] = data.m_totalIncome.ToString();

                //注册
                m_content[f++] = data.m_regeditCount.ToString();
                //设备激活
                m_content[f++] = data.m_deviceActivationCount.ToString();
                //活跃人数
                m_content[f++] = data.m_activeCount.ToString();
                //设备活跃
                m_content[f++] = data.m_deviceLoginCount.ToString();

                m_content[f++] = data.m_rechargePersonNum.ToString();
                m_content[f++] = data.m_rechargeCount.ToString();
                //付费率 = 付费人数/活跃人数
                m_content[f++] = data.getRechargeRate(data.m_rechargePersonNum, data.m_activeCount);

                m_content[f++] = data.getARPU();
                m_content[f++] = data.getARPPU();

                m_content[f++] = data.m_newAccIncome > -1 ? data.m_newAccIncome.ToString() : "";
                //新增用户付费率 = 新增用户付费人数/注册人数
                m_content[f++] = data.getRechargeRate(data.m_newAccRechargePersonNum, data.m_regeditCount);

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