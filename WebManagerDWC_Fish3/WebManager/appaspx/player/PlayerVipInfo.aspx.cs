using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.player
{
    public partial class PlayerVipInfo : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "玩家ID","用户名","手机号","渠道","Vip经验值","Vip等级","注册时间","最后上线时间","剩余金币","当前贡献值","历史贡献值","总充值额"};
        private string[] m_content = new string[s_head.Length];
        private PageSelectLostPlayers m_gen = new PageSelectLostPlayers(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.PLAYER_VIP_INFO, Session, Response);
            GMUser user = (GMUser)Session["user"];
            if (IsPostBack)
            {
                m_gen.m_vipLevel = m_vip.Text;
                m_gen.m_isBindPhone = (1 - m_isBindPhone.SelectedIndex);
                m_gen.m_sort = m_sort.SelectedIndex;
            }
            else
            {
                m_isBindPhone.Items.Add("是");
                m_isBindPhone.Items.Add("否");

                m_sort.Items.Add("最后上线时间");
                m_sort.Items.Add("Vip等级");
                if (m_gen.parse(Request))
                {
                    m_vip.Text = m_gen.m_vipLevel;
                    m_isBindPhone.SelectedIndex = 1 - m_gen.m_isBindPhone;
                    m_sort.SelectedIndex = m_gen.m_sort;
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamSelectLostPlayer param = getParamSelectLostPlayer();
            
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeSelectLostPlayers, user);
            genTable(m_result, res, param, user, mgr);
        }
        //导出
        protected void onExport(object sender, EventArgs e) 
        {
            GMUser user = (GMUser)Session["user"];
            ParamSelectLostPlayer param = getParamSelectLostPlayer();

            ExportMgr mgr = user.getSys<ExportMgr>(SysType.sysTypeExport);
            OpRes res = mgr.doExport(param, ExportType.exportTypeVipPlayerInfo, user);
            m_resInfo.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }
        private ParamSelectLostPlayer getParamSelectLostPlayer()
        {
            ParamSelectLostPlayer param = new ParamSelectLostPlayer();
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            param.m_vipLevel = m_vip.Text;
            //param.m_time = m_time.Text;
            param.m_isBindPhone = (1 - m_isBindPhone.SelectedIndex); // 1是 0否
            param.m_op = m_sort.SelectedIndex; //0最后上线时间  1VIP等级
            return param;
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
                m_content[f++] = item.m_vipExp.ToString();
                m_content[f++] = item.m_vip.ToString();
                m_content[f++] = item.m_creatTime;
                m_content[f++] = item.m_lastLoginTime;
                m_content[f++] = item.m_leftGold.ToString();
                m_content[f++] = item.m_contributionValue.ToString();
                m_content[f++] = item.m_contributeHistory.ToString();
                m_content[f++] = item.m_totalRecharge.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/player/PlayerVipInfo.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}