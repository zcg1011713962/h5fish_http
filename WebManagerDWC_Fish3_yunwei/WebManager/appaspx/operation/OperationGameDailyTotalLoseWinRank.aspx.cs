using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationGameDailyTotalLoseWinRank : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "玩家ID", "玩家昵称", "排行", "局数", "投入货币量", "获得货币量", "输赢和", "累计充值", "最后登录时间" };
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_GAME_REAL_TIME_LOSE_WIN_LIST, Session, Response);
            if (!IsPostBack)
            {
                //总计和小游戏
                m_gameType.Items.Add(new ListItem("总计", "0"));

                int k = 1;
                foreach (var game in StrName.s_gameName3)
                {
                    if (game.Key == 0 || game.Key == 1)
                        continue;

                    if (!string.IsNullOrEmpty(game.Value))
                        m_gameType.Items.Add(new ListItem(game.Value, k.ToString()));
                    k++;
                }
                //排行类型
                m_rankType.Items.Add("赢金币榜");
                m_rankType.Items.Add("输金币榜");
                m_rankType.Items.Add("赢龙珠榜");
                m_rankType.Items.Add("输龙珠榜");

                //月份
                for (int i = 1; i<= 12; i++ ) 
                {
                    m_month.Items.Add(i.ToString());
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_param = m_gameType.SelectedValue;
            param.m_op = m_rankType.SelectedIndex;
            param.m_time = m_year.Text;
            param.m_showWay = m_month.SelectedValue;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeGameDailyTotalLoseWinRank, user);
            genTable(m_result, res, param, user, mgr);
        }

        //活动排行榜生成查询表
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
            List<MiniGameRealLoseWinRankItem> qresult =
                (List<MiniGameRealLoseWinRankItem>)mgr.getQueryResult(QueryType.queryTypeGameDailyTotalLoseWinRank);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.RowSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;
                MiniGameRealLoseWinRankItem item = qresult[i];
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_rank.ToString();
                m_content[f++] = item.m_turn.ToString();
                m_content[f++] = item.m_outlay.ToString();
                m_content[f++] = item.m_income.ToString();
                m_content[f++] = item.m_addValue.ToString();
                m_content[f++] = item.m_recharge.ToString();
                m_content[f++] = item.m_lastLogin;

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.RowSpan = 1;
                }
            }
        }

    }
}