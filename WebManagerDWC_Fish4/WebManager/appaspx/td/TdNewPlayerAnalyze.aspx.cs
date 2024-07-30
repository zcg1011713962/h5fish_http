using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.td
{
    public partial class TdNewPlayerAnalyze : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);
            m_channel.Items.Add(new ListItem("全部", ""));

            Dictionary<string, TdChannelInfo> cd = TdChannel.getInstance().getAllData();
            foreach (var item in cd.Values)
            {
                m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
            }

        }
    }
}