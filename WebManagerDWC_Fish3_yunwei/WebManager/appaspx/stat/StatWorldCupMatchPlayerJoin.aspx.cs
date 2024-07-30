using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace WebManager.appaspx.stat
{
    public partial class StatWorldCupMatchPlayerJoin : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "场次ID","比赛时间","组别","队伍1","队伍2","押注队伍1人数","押注队伍1总资金","队伍1赔率",
            "押注队伍2人数","押注队伍2总资金","队伍2赔率","押注平局人数","押注平局总资金","平局赔率","总押注","奖励支出","总盈利","总盈利率","是否结算完奖励"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_WORLD_CUP_MATCH_PLAYER_JOIN, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeWorldCupMatchPlayerJoin, user);
            genTable(m_result, res, param, user, mgr);
        }
        //生成查询表
        private void genTable(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
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
            List<worldCupMatchPlayerJoinItem> qresult = (List<worldCupMatchPlayerJoinItem>)mgr.getQueryResult(QueryType.queryTypeWorldCupMatchPlayerJoin);
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
                worldCupMatchPlayerJoinItem item = qresult[i];
                m_content[f++] = item.m_matchId.ToString();
                m_content[f++] = item.m_matchTime;
                m_content[f++] = item.getMatchName(item.m_matchName);
                m_content[f++] = item.getTeamName(item.m_homeTeam);
                m_content[f++] = item.getTeamName(item.m_visitTeam);

                m_content[f++] = item.m_homeBetPlayerCount == -1 ? "暂未统计" : item.m_homeBetPlayerCount.ToString();
                m_content[f++] = item.m_homeBet.ToString();
                m_content[f++] = item.m_homeOdds.ToString();

                m_content[f++] = item.m_visitBetPlayerCount == -1 ? "暂未统计" : item.m_visitBetPlayerCount.ToString();
                m_content[f++] = item.m_visitBet.ToString();
                m_content[f++] = item.m_visitOdds.ToString();

                m_content[f++] = item.m_drawBetPlayerCount == -1 ? "暂未统计" : item.m_drawBetPlayerCount.ToString();
                m_content[f++] = item.m_drawBet.ToString();
                m_content[f++] = item.m_drawOdds.ToString();

                m_content[f++] = item.m_totalBet.ToString();
                m_content[f++] = item.m_rewardOutlay.ToString();
                m_content[f++] = item.m_totalEarn.ToString();
                m_content[f++] = item.getEarnRate();
                m_content[f++] = item.m_isSendReward;

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    if (j==16 || j == 17 && !string.IsNullOrEmpty(m_content[j]))  //值为负数 红 否则 绿
                        setColor(td, m_content[j]);
                }
            }
        }

        protected void setColor(TableCell td, string num)
        {
            if (num[0] == '-'){
                td.ForeColor = Color.Red;
            }else{
                td.ForeColor = Color.Green;
            }
        }
    }
}