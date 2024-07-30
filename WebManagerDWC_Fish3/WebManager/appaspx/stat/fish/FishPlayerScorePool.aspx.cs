using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat.fish
{
    public partial class FishPlayerScorePool : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_PLAYER_SCORE_POOL, Session, Response);
            if (!IsPostBack)
            {
                m_moneyType.Items.Add(new ListItem("金币", ((int)PropertyType.property_type_gold).ToString()));
                //m_moneyType.Items.Add(new ListItem("龙珠碎片", ((int)PropertyType.e_itd_dragon_ball_chip).ToString()));
            }
        }

        protected void onBuffScorePool(object sender, EventArgs e)
        {
            ParamGameBItem param = new ParamGameBItem();
            param.m_playerId = m_playerId.Text;
            param.m_type = Convert.ToInt32(m_moneyType.SelectedValue);
            param.m_param = m_addPoolVal.Text;
            param.m_time = m_time.Text;
            param.m_op = 0; //添加
            GMUser user = (GMUser)Session["user"];
            OpRes res = user.doDyop(param, DyOpType.opTypePlayerScorePool);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }
    }
}