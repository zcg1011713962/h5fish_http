using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordBaojinControl : RefreshPageBase
    {
        TableStatFishlordBaojinControl m_common = new TableStatFishlordBaojinControl();
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_STAT_FISHLORD_BAOJIN_CONTROL, Session, Response);
            if (!IsPostBack)
            {
                GMUser user = (GMUser)Session["user"];
                m_common.genExpRateTable(m_result, user, QueryType.queryTypeFishlordBaojinParam);
            }
        }

        //protected void onConfirm(object sender, EventArgs e)
        //{
        //    GMUser user = (GMUser)Session["user"];
        //    if (IsRefreshed)
        //    {
        //        m_common.genExpRateTable(m_result, user, QueryType.queryTypeFishlordBaojinParam);
        //        return;
        //    }
        //    if(m_param.Text=="")
        //    {
        //        m_res.InnerText = "请填写参数";
        //        m_common.genExpRateTable(m_result, user, QueryType.queryTypeFishlordBaojinParam);
        //        return;
        //    }
        //    CMemoryBuffer buffer = CommandMgr.getMemoryBuffer(user, true);
        //    buffer.Writer.Write(m_param.Text);
        //    CommandBase cmd = CommandMgr.processCmd(CmdName.Adjust, buffer, user);
        //    OpRes res = cmd.getOpRes();
        //    m_res.InnerText = OpResMgr.getInstance().getResultString(res);
        //    m_common.genExpRateTable(m_result, user, QueryType.queryTypeFishlordBaojinParam);
        //}
    }
}