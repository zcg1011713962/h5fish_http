using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat.shcd
{
    public partial class ShcdCardsSpecilListSet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.SHCD_CARDS_SPECIL_LIST, Session, Response);
            m_res.InnerHtml = "";
            if (!IsPostBack)   //客户端回发而加载
            {
                m_setType.Items.Add("加入黑名单");
                m_setType.Items.Add("加入白名单");
            }
        }

        protected void onSetSpecilList(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            CMemoryBuffer buffer = CommandMgr.getMemoryBuffer(user, true);
            buffer.Writer.Write(m_playerId.Text);
            buffer.Writer.Write(m_setType.SelectedIndex+1);
            buffer.Writer.Write(0);//0设置 1删除
            CommandBase cmd = CommandMgr.processCmd(CmdName.SetShcdCardsSpecilList, buffer, user);
            OpRes res = cmd.getOpRes();
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }
    }
}