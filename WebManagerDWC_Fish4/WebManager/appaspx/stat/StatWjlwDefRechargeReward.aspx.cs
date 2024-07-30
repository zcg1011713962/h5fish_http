using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace WebManager.appaspx.stat
{
    public partial class StatWjlwDefRechargeReward : RefreshPageBase
    {
        TableStatWjlwDefRechargeReward m_common = new TableStatWjlwDefRechargeReward();
        protected void Page_Load(object sender, EventArgs e)
        {
            m_res.InnerHtml = "";
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_WJLW_DEF_RECHARGE_REWARD, Session, Response);

            GMUser user = (GMUser)Session["user"];
            if (!IsPostBack)
            {
                m_rewardId.Items.Add("一等奖");
                m_rewardId.Items.Add("二等奖");
                m_rewardId.Items.Add("三等奖");
                m_common.genTable(m_result, user);
            }
        }

        protected void onConfirm(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            if (IsRefreshed)
            {
                m_common.genTable(m_result, user);
            }

            DyOpMgr mgr = user.getSys<DyOpMgr>(SysType.sysTypeDyOp);
            ParamQuery param = new ParamQuery();
            param.m_param = m_nickname.Text;
            param.m_op = m_rewardId.SelectedIndex + 1;

            OpRes res = mgr.doDyop(param, DyOpType.opTypeWjlwDefRechargeReward, user);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);

            m_common.genTable(m_result, user);
        }

    }
}