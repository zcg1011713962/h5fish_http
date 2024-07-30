using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.td
{
    public partial class TdActivationOfOldPlayer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.TD_ACTIVE_OLD_PLAYER_DATA, Session, Response);

            if (!IsPostBack)
            {
                m_channel.Items.Add(new ListItem("总体", ""));
                m_channel.Items.Add(new ListItem("全渠道", "-1"));
                m_channel.Items.Add(new ListItem("安卓总体", "-2"));
                Dictionary<string, TdChannelInfo> data = TdChannel.getInstance().getAllData();
                foreach (var item in data.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            TableTdActivationOfOldPlayer view = new TableTdActivationOfOldPlayer();
            ParamQuery param = new ParamQuery();
            param.m_param = m_channel.SelectedValue;
            param.m_time = m_time.Text;
            OpRes res = user.doQuery(param, QueryType.queryTypeTdActivation);
            view.genTable(user, m_result, res);
        }
    }
}