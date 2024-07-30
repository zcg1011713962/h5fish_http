using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.td
{
    public partial class TdLTV : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.TD_LTV, Session, Response);

            if (!IsPostBack)
            {
                //渠道
                m_channel.Items.Add(new ListItem("全部平均", ""));
                m_channel.Items.Add(new ListItem("安卓总体","-1")); //安卓总体，除去微信和QQ
                m_channel.Items.Add(new ListItem("APP总体", "-4"));
                m_channel.Items.Add(new ListItem("CPL总体", "-3"));
                m_channel.Items.Add(new ListItem("WEB总体", "-5"));
                Dictionary<string, TdChannelInfo> data = TdChannel.getInstance().getAllData();
                foreach (var item in data.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            TableTdLTVBase view = TableTdLTVBase.create(m_channel.SelectedIndex);
            if (view == null)
                return;

            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_param = m_channel.SelectedValue;
            param.m_time = m_time.Text;
            OpRes res = view.query(user, param);
            view.genTable(user, m_result, res);
        }
    }
}