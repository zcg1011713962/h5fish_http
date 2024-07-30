using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.service
{
    public partial class ServiceSelectTheLossOfPlayers : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "玩家ID", "用户名", "手机号", "渠道", "所选时间内充值金额", 
            "vip等级", "注册时间" ,"最后上线时间","剩余金币","引导记录"};
        private string[] m_content = new string[s_head.Length];
        private PageSelectLostPlayers m_gen = new PageSelectLostPlayers(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.SVR_SELECT_LOSS_PLAYER, Session, Response);
            GMUser user = (GMUser)Session["user"];
            if (IsPostBack)
            {
                m_gen.m_vipLevel = m_vip.Text;
                m_gen.m_days = m_days.Text;
                m_gen.m_time = m_time.Text;
                m_gen.m_isBindPhone = (1-m_isBindPhone.SelectedIndex);
            }
            else
            {
                m_isBindPhone.Items.Add("是");
                m_isBindPhone.Items.Add("否");

                if (m_gen.parse(Request))
                {
                    m_vip.Text = m_gen.m_vipLevel;
                    m_days.Text = m_gen.m_days;
                    m_time.Text = m_gen.m_time;
                    m_isBindPhone.SelectedIndex = 1-m_gen.m_isBindPhone;
                    
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamSelectLostPlayer param = new ParamSelectLostPlayer();
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            param.m_vipLevel = m_vip.Text;
            param.m_days = m_days.Text;
            param.m_time = m_time.Text;
            param.m_isBindPhone = (1-m_isBindPhone.SelectedIndex); // 1是 0否

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeSelectLostPlayers, user);
            genTable(m_result, res, param, user, mgr);
        }

        //生成查询表
        private void genTable(Table table, OpRes res, ParamSelectLostPlayer query_param, GMUser user, QueryMgr mgr)
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
            List<SelectLostPlayersItem> qresult = (List<SelectLostPlayersItem>)mgr.getQueryResult(QueryType.queryTypeSelectLostPlayers);
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
                m_content[f++] = item.m_guideRecord;

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            //string page_html = "", foot_html = "";
            //m_gen.genPage(query_param, @"/appaspx/service/ServiceSelectTheLossOfPlayers.aspx", ref page_html, ref foot_html, user);
            //m_page.InnerHtml = page_html;
            //m_foot.InnerHtml = foot_html;
        }
    }
}