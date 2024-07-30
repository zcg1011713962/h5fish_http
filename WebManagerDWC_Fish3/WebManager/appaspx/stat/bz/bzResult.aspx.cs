using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat.bz
{
    public partial class bzResult : RefreshPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.BZ_RESULT_CONTROL, Session, Response);

            if (!IsPostBack)
            {
                for (int i = 0; i < StrName.s_roomName.Length; i++)
                {
                    //只显示一个初级场
                    if (i > 0) { continue; }
                    m_room.Items.Add(new ListItem(StrName.s_roomName[i], (i + 1).ToString()));
                }

                for(int i=0;i < StrName.s_bzArea.Length; i++)
                {
                    m_result.Items.Add(new ListItem(StrName.s_bzArea[i], (i+1).ToString()));
                }
            }
        }

        protected void onSetResult(object sender, EventArgs e)
        {
            if (IsRefreshed)
                return;

            GMUser user = (GMUser)Session["user"];
            ParamGameResultCrocodile param = new ParamGameResultCrocodile();
            param.m_roomId = Convert.ToInt32(m_room.SelectedValue);
            param.m_gameId = (int)GameId.bz;
            param.m_result = Convert.ToInt32(m_result.SelectedValue);
            OpRes res = user.doDyop(param, DyOpType.opTypeDyOpGameResult);
            m_res.InnerText = OpResMgr.getInstance().getResultString(res);
        }
    }
}