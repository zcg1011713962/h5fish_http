using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationStatChannelTalkingDataWeek : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_STAT_CHANNEL_TALKING_DATA_WEEK, Session, Response);

            if (!IsPostBack)
            {
                m_channel.Items.Add(new ListItem("总体", "-1"));
                m_channel.Items.Add(new ListItem("安卓总体", "-2"));
                Dictionary<string, TdChannelInfo> data = TdChannel.getInstance().getAllData();
                foreach (var item in data.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }

                int year = Convert.ToInt32(DateTime.Now.Year);
                int new_year = year;
                for (int i = -1; i<= 10; i++) 
                {
                    new_year = year + i;
                    m_year.Items.Add(new ListItem(new_year + "年", new_year.ToString()));
                }

                for (int k = 1; k<= 12; k++) 
                {
                    m_month.Items.Add(k + "月");
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            TableStatCHannelTalkingDataWeek view = new TableStatCHannelTalkingDataWeek();
            ParamQuery param = new ParamQuery();
            param.m_param = m_channel.SelectedValue;
            param.m_op = Convert.ToInt32(m_year.SelectedValue);
            param.m_type = m_month.SelectedIndex + 1; //月份
            OpRes res = user.doQuery(param, QueryType.queryTypeStatChannelTalkingDataWeek);
            view.genTable(user, m_result,param.m_param, res);
        }
    }
}