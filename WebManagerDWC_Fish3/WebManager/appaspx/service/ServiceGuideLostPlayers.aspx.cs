using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.service
{
    public partial class ServiceGuideLostPlayers : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "玩家ID", "用户名", "手机号", "渠道", 
            "所选时间内充值金额","VIP等级","注册时间","最后登录时间","剩余金币","回流引导时间","备注"};
        private string[] m_content = new string[s_head.Length];

        private PageGenRepairOrder m_gen = new PageGenRepairOrder(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            m_res.InnerHtml = "";
            RightMgr.getInstance().opCheck(RightDef.SVR_GUIDE_LOST_PLAYERS, Session, Response);
            if (IsPostBack){
                m_gen.m_time = m_time.Text;
            }else
            {
                if (m_gen.parse(Request))
                {
                    m_time.Text = m_gen.m_time;
                    onQuery(null, null);
                }
            }
        }

        protected void onSubmit(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_playerId = m_playerId.Text;
            param.m_param = m_comment.Text;

            DyOpMgr mgr = user.getSys<DyOpMgr>(SysType.sysTypeDyOp);
            OpRes res = mgr.doDyop(param, DyOpType.opTypeGuideLostPlayers, user);

            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            param.m_time = m_time.Text;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeGuideLostPlayersRes, user);
            genTable(m_result, res, param, user, mgr);
        }

        //引导记录效果查询
        private void genTable(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";

            m_result.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
            TableCell td = null;
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }
            List<SelectLostPlayersItem> qresult = (List<SelectLostPlayersItem>)mgr.getQueryResult(QueryType.queryTypeGuideLostPlayersRes);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;
                SelectLostPlayersItem item = qresult[i];
                m_content[f++] = item.m_playerId;
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_phone;
                m_content[f++] = item.m_channel;
                m_content[f++] = item.m_RechargeRMB.ToString();
                m_content[f++] = item.m_vip.ToString();
                m_content[f++] = item.m_creatTime;
                m_content[f++] = item.m_lastLoginTime;
                m_content[f++] = item.m_leftGold.ToString();
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_guideRecord;

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/service/ServiceGuideLostPlayers.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}