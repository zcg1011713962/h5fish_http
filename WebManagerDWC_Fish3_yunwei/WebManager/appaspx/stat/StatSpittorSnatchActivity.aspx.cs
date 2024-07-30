using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatSpittorSnatchActivity : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] { "排名","玩家ID","玩家昵称","击杀金蟾数量"};
        private static string[] s_head2 = new string[] { "日期","奖励ID","领取人数"};

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_SPITTOR_SNATCH_ACT, Session, Response);
            if (!IsPostBack)
            {
                m_queryType.Items.Add("活动排行榜");
                m_queryType.Items.Add("领取奖励人数统计");
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
                    OpRes res = mgr.doQuery(param, QueryType.queryTypeSpittorSnatchActRank, user);
                    genTable0(m_result, res, param, user, mgr);
                    break;
                case 1:
                    OpRes res_1 = mgr.doQuery(param, QueryType.queryTypeSpittorSnatchActRewardList, user);
                    genTable1(m_result, res_1, param, user, mgr);
                    break;
                case 2:
                    CMemoryBuffer buffer = CommandMgr.getMemoryBuffer(user, true);
                    buffer.Writer.Write(m_playerId.Text);
                    buffer.Writer.Write(m_killCount.Text);
                    CommandBase cmd = CommandMgr.processCmd(CmdName.SetSpittorSnatchActKillCount, buffer, user);
                    OpRes res_2 = cmd.getOpRes();
                    m_resNote.InnerHtml = OpResMgr.getInstance().getResultString(res_2);
                    break;
            }
        }
        //活动排行榜生成查询表
        private void genTable0(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            string[] m_content =new string[s_head1.Length];
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
            List<SpittorSnatchActRankItem> qresult = (List<SpittorSnatchActRankItem>)mgr.getQueryResult(QueryType.queryTypeSpittorSnatchActRank);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head1[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;
                SpittorSnatchActRankItem item = qresult[i];
                m_content[f++] = item.m_rankId.ToString();
                m_content[f++] = item.m_playerId;
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_killCount.ToString();

                for (j = 0; j < s_head1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.RowSpan = 1;
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
            List<SpittorSnatchActReward> qresult = (List<SpittorSnatchActReward>)mgr.getQueryResult(QueryType.queryTypeSpittorSnatchActRewardList);

            // 表头
            int i = 0, k = 0;
            for (i = 0; i < s_head2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head2[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                SpittorSnatchActReward item = qresult[i];
                k = 0;
                foreach (var d in item.m_data) 
                {
                    tr = new TableRow();
                    m_result.Rows.Add(tr);
                    if(k == 0)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_time;
                        td.RowSpan = item.m_flag;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = d.Key.ToString();
                    td.RowSpan = 1;

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = d.Value.ToString();

                    k++;
                }
            }
        }
    }
}