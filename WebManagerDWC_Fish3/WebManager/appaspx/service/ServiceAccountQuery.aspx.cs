using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.service
{
    public partial class ServiceAccountQuery : System.Web.UI.Page
    {
        private static string[] s_head =
            new string[] { "用户账号", "ID", "昵称", "ACC","平台", "角色创建时间", "VIP等级", "VIP经验","最大炮数","历史彩券", "上次登陆时间", "上次登陆IP",
                "金币", "保险箱内金币", "钻石", "话费", "绑定手机","设备号", "账号状态","背包查询"};
        private string[] m_content = new string[s_head.Length];
        private PageGenDailyTask m_gen = new PageGenDailyTask(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.SVR_ACCOUNT_QUERY, Session, Response);
            m_res.InnerHtml = "";
            if (!IsPostBack)
            {
                m_queryWay.Items.Add("通过玩家id查询");
                m_queryWay.Items.Add("通过账号查询");
                m_queryWay.Items.Add("通过昵称查询");
                m_queryWay.Items.Add("通过登陆IP查询");
                m_queryWay.Items.Add("通过手机号查询");
                m_queryWay.Items.Add("通过设备号查询");
                //m_queryWay.Items.Add("通过lastIP查询");
                //m_queryWay.Items.Add("通过deviceID查询");

                if (m_gen.parse(Request))
                {
                    m_param.Text = m_gen.m_param;
                    m_queryWay.SelectedIndex = m_gen.m_way;
                    onQueryAccount(null, null);
                }
            }
        }

        // 开始查询
        protected void onQueryAccount(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_way = (QueryWay)m_queryWay.SelectedIndex;
            param.m_param = m_param.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeAccount, user);
            genTable(m_result, res, param, user, mgr);
        }

        // 旧号变新号问题
        protected void onModifyAcc(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamPlayerInfo param = new ParamPlayerInfo();
            param.m_optype = 1;
            param.m_playerId = m_oldPlayerId.Text;
            param.m_account = m_newPlayerId.Text;

            DyOpMgr mgr = user.getSys<DyOpMgr>(SysType.sysTypeDyOp);
            OpRes res = mgr.doDyop(param, DyOpType.opTypeFasterStartForVisitor, user);
            NewMsg.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }

        //游客快速开始添加账号
        protected void onConfirm(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamPlayerInfo p = new ParamPlayerInfo();
            p.m_playerId = m_playerId.Text;
            p.m_account = m_account.Text;
            p.m_pwd = m_pwd.Text;
            
            DyOpMgr mgr = user.getSys<DyOpMgr>(SysType.sysTypeDyOp);
            OpRes res = mgr.doDyop(p, DyOpType.opTypeFasterStartForVisitor, user);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }
        private void genTable(Table table, OpRes res, ParamQuery param, GMUser user, QueryMgr mgr)
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

            List<ResultQueryAccount> qresult = (List<ResultQueryAccount>)mgr.getQueryResult(QueryType.queryTypeAccount);
            int i = 0, j = 0,f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                m_content[f++] = qresult[i].m_account;
                m_content[f++] = qresult[i].m_id.ToString();
                m_content[f++] = qresult[i].m_nickName;
                m_content[f++] = qresult[i].m_acc.ToString();  //acc
                m_content[f++] = qresult[i].m_platForm;
                m_content[f++] = qresult[i].m_createTime;

                m_content[f++] = qresult[i].m_vipLevel.ToString();
                m_content[f++] = qresult[i].m_vipExp.ToString();
                m_content[f++] = qresult[i].m_MaxFireCount.ToString();
                m_content[f++] = qresult[i].m_HistoryLottery.ToString();
                m_content[f++] = qresult[i].m_lastLoginTime;
                m_content[f++] = qresult[i].m_lastLoginIP; // 上次登陆IP
                m_content[f++] = qresult[i].m_gold.ToString();
                m_content[f++] = qresult[i].m_safeBoxGold.ToString();

                m_content[f++] = qresult[i].m_diamond.ToString();
                m_content[f++] = (qresult[i].m_ticket/100.0).ToString();
                m_content[f++] = qresult[i].m_bindMobile;
                m_content[f++] = qresult[i].m_deviceId;
                m_content[f++] = qresult[i].m_accountState;
                m_content[f++] = qresult[i].getExParam(); //背包详情
                

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/service/ServiceAccountQuery.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}
