using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationPolarLightsPush : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_POLAR_LIGHTS_PUSH, Session, Response);
            m_res.InnerHtml = "";
            if (!IsPostBack)
            {
                //渠道
                m_channel.Items.Add(new ListItem("全部", "-1"));
                Dictionary<string, TdChannelInfo> cd = TdChannel.getInstance().getAllData();
                foreach (var item in cd.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }

                //VIP
                m_vip.Items.Add("所有");
                //for (int i = 1; i<= 10; i++ )
                //{
                //    string vip = "VIP" + i;
                //    m_vip.Items.Add(vip);
                //}

                //星期
                m_week.Items.Add(new ListItem("每天","0"));
                m_week.Items.Add(new ListItem("星期一","1"));
                m_week.Items.Add(new ListItem("星期二","2"));
                m_week.Items.Add(new ListItem("星期三","3"));
                m_week.Items.Add(new ListItem("星期四","4"));
                m_week.Items.Add(new ListItem("星期五","5"));
                m_week.Items.Add(new ListItem("星期六","6"));
                m_week.Items.Add(new ListItem("星期日","7"));
            }
        }
    }
}