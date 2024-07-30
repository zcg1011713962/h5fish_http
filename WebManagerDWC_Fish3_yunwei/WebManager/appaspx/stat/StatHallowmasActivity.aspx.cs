using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatHallowmasActivity : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "排名", "玩家ID", "玩家昵称", "南瓜数量","购买数量（用钻石购买的南瓜总数）" };
        private string[] m_content = new string[s_head.Length];

        private static string[] s_head_1 = new string[] { "日期","奖励1领取人数","奖励2领取人数","奖励3领取人数","当日登录人数"};


        protected void Page_Load(object sender, EventArgs e)
        {
            m_resNote.InnerHtml = "";
            RightMgr.getInstance().opCheck(RightDef.DATA_HALLOWMAS_ACTIVITY, Session, Response);
            if (!IsPostBack)
            {
                m_queryType.Items.Add("活动排行榜");
                m_queryType.Items.Add("活动领取奖励人数统计");
                m_queryType.Items.Add("活动作弊功能设置");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();

            param.m_op = m_queryType.SelectedIndex;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            switch (param.m_op)
            {
                case 0:
                    OpRes res = mgr.doQuery(param, QueryType.queryTypeHallowmasActRank, user);
                    genTable0(m_result, res, param, user, mgr);
                    break;
                case 1:
                    OpRes res_1 = mgr.doQuery(param, QueryType.queryTypeHallowmasActRecvCount, user);
                    genTable1(m_result, res_1, param, user, mgr);
                    break;
                case 2:
                    CMemoryBuffer buffer = CommandMgr.getMemoryBuffer(user, true);
                    buffer.Writer.Write(m_playerId.Text);
                    buffer.Writer.Write(m_pumpkinCount.Text);
                    CommandBase cmd = CommandMgr.processCmd(CmdName.SetHallowmasActPumpkinCount, buffer, user);
                    OpRes res_2 = cmd.getOpRes();
                    m_resNote.InnerHtml = OpResMgr.getInstance().getResultString(res_2);
                    break;
            }
        }

        //活动排行榜生成查询表
        private void genTable0(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
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
            List<HallowmasActRankItem> qresult = (List<HallowmasActRankItem>)mgr.getQueryResult(QueryType.queryTypeHallowmasActRank);
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
                HallowmasActRankItem item = qresult[i];
                m_content[f++] = item.m_rankId.ToString();
                m_content[f++] = item.m_playerId;
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_pumpkinCount.ToString();
                m_content[f++] = item.m_buyPumpkinCount.ToString();
                
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

        }
        //领取奖励生成查询表
        private void genTable1(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
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
            List<HallowmasActRecvItem> qresult = (List<HallowmasActRecvItem>)mgr.getQueryResult(QueryType.queryTypeHallowmasActRecvCount);
    
            // 表头
            for (int i = 0; i < s_head_1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head_1[i];
            }

            for (int i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                HallowmasActRecvItem item = qresult[i];
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time;

                for (int k = 0; k<3; k++) 
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = item.m_data[k].ToString();
                }

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_loginCount.ToString();
            }
        }
    }
}