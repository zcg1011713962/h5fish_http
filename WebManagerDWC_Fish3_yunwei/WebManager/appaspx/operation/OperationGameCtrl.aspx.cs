using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationGameCtrl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_OPERATION_GAME_CTRL, Session, Response);
            if (!IsPostBack)
            {
                //渠道
                m_channel.Items.Add(new ListItem("全部", "-1"));
                Dictionary<string, TdChannelInfo> data = TdChannel.getInstance().getAllData();
                foreach (var item in data.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }

                m_turret.Items.Add(new ListItem("不设限", "-1"));
                Dictionary<int, Fish_LevelCFGData> turretList = Fish_TurretLevelCFG.getInstance().getAllData();
                foreach (var turret in turretList.Values)
                {
                    m_turret.Items.Add(new ListItem(turret.m_openRate.ToString(), turret.m_level.ToString()));
                }

                m_viplv.Items.Add(new ListItem("不设限", "-1"));
                for (int i = 0; i <= 10; i++)
                {
                    m_viplv.Items.Add(i.ToString());
                }

                m_onOff.Items.Add("排行");
                m_onOff.Items.Add("空投");
                m_onOff.Items.Add("兑换");
                m_onOff.Items.Add("邮件");
                m_onOff.Items.Add("客服");
            }
        }

        protected void onConfirm(object sender, EventArgs e)
        {
            ParamQuery param = new ParamQuery();
            param.m_channelNo = m_channel.SelectedValue;
            param.m_param = m_version.Text;
            param.m_op = 0; //添加
            param.m_type = Convert.ToInt32(m_turret.SelectedValue); //炮倍率
            param.m_curPage = Convert.ToInt32(m_viplv.SelectedValue);//VIP等级
            string checkStr = ""; //开关功能
            foreach(ListItem li in m_onOff.Items)
            {
                if (li.Selected)
                {
                    checkStr += 1 + ",";
                }
                else {
                    checkStr += 0 + ",";
                }
            }
            param.m_time = checkStr.TrimEnd(',');

            GMUser user = (GMUser)Session["user"];
            OpRes res = user.doDyop(param, DyOpType.opTypeOPerationGameCtrl);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }
    }
}