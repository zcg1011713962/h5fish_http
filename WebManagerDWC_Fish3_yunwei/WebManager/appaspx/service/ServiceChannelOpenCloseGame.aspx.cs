using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace WebManager.appaspx.service
{
    public partial class ServiceChannelOpenCloseGame : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.SVR_CHANNEL_OPEN_CLOSE_GAME, Session, Response);
            m_res.InnerHtml = "";
            m_ReloadRes.InnerHtml = "";
            if (!IsPostBack)   //客户端回发而加载
            {
                //渠道
                m_channel.Items.Add(new ListItem("全部",""));
                Dictionary<string, TdChannelInfo> cd = TdChannel.getInstance().getAllData();
                foreach(var item in cd.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName,item.m_channelNo));
                }

                //开关
                m_openClose.Items.Add("整体开启");
                m_openClose.Items.Add("整体关闭");

                //炮倍率
                Dictionary<int, Fish_LevelCFGData> fl = Fish_LevelCFG.getInstance().getAllData();
                m_gameLevel.Items.Add(new ListItem("不设限","0"));
                foreach(var item in fl.Values)
                {
                    m_gameLevel.Items.Add(new ListItem(item.m_openRate.ToString()+"炮",item.m_level.ToString()));
                }
                
                //VIP
                m_vipLevel.Items.Add(new ListItem("不设限","-1"));
                for (int i = 0; i<11;i++ ) 
                {
                    m_vipLevel.Items.Add(new ListItem(i.ToString()+"级",i.ToString()));
                }
            }
        }

        protected void onSetSpecilList(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            CMemoryBuffer buffer = CommandMgr.getMemoryBuffer(user, true);
            buffer.Writer.Write(m_channel.SelectedValue);
            buffer.Writer.Write(0);//0设置 1删除
            buffer.Writer.Write(m_openClose.SelectedIndex);
            buffer.Writer.Write(Convert.ToInt32(m_gameLevel.SelectedValue));
            buffer.Writer.Write(Convert.ToInt32(m_vipLevel.SelectedValue));
            
            CommandBase cmd = CommandMgr.processCmd(CmdName.SetChannelOpenCloseGame, buffer, user);
            OpRes res = cmd.getOpRes();
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }

        protected void onReloadData(object sender, EventArgs e)
        {
            OpRes res = flushToGameServer();
            m_ReloadRes.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }

        // 刷新到游戏服务器
        public OpRes flushToGameServer()
        {
            string url = string.Format(DefCC.HTTP_MONITOR, "cmd=3");
            var ret = HttpPost.Get(new Uri(url));
            if (ret != null)
            {
                string retStr = Encoding.UTF8.GetString(ret);
                if (retStr == "ok")
                {
                    return OpRes.opres_success;
                }
            }
            return OpRes.op_res_failed;
        }
    }
}