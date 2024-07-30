using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordBaojinScoreControl : RefreshPageBase
    {
        TableStatFishlordBaojinScoreControl m_common = new TableStatFishlordBaojinScoreControl();
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_STAT_FISHLORD_BAOJIN_SCORE_CONTROL, Session, Response);
            m_resEdit.InnerHtml = "";
            if(!IsPostBack)
            {
                onQueryRobotMaxScore(null, null);
            }
        }
        //竞技场得分查询
        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_param = m_playerId.Text;
            m_common.genExpRateTable(m_result, param, user, QueryType.queryTypeFishlordBaojinScoreParam);
        }
        //机器人最高积分修改
        protected void onEdit(object sender, EventArgs e)
        {
            if (IsRefreshed)
            {
                onQueryRobotMaxScore(null, null);
                return;
            }

            GMUser user=(GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_param = m_value.Text;
            DyOpMgr mgr = user.getSys<DyOpMgr>(SysType.sysTypeDyOp);
            OpRes res = mgr.doDyop(param, DyOpType.opTypeRobotMaxScoreEdit, user);
            m_resEdit.InnerHtml = OpResMgr.getInstance().getResultString(res);
            onQueryRobotMaxScore(null, null);
        }
        //机器人最高积分查询
        protected void onQueryRobotMaxScore(object sender, EventArgs e) 
        {
            GMUser user = (GMUser)Session["user"];
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(null, QueryType.queryTypeRobotMaxScore, user);
            int c = (int)mgr.getQueryResult(QueryType.queryTypeRobotMaxScore);
            m_count.Text = c.ToString();
        }
    }
}