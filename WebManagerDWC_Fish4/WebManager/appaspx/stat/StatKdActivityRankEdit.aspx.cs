using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatKdActivityRankEdit : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "排名", "玩家昵称", "玩家ID", "获取龙珠", "使用召唤数量", "消耗金币" };
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            m_res.InnerHtml = "";
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_KD_ACT_RANK, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            CMemoryBuffer buffer = CommandMgr.getMemoryBuffer(user, true);
            buffer.Writer.Write(m_playerId.Text);
            buffer.Writer.Write(m_count.Text);
            CommandBase cmd = CommandMgr.processCmd(CmdName.AlterKdActGaindb, buffer, user);
            OpRes res = cmd.getOpRes();
            string str = OpResMgr.getInstance().getResultString(res);
            m_res.InnerHtml = str;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);

            res = mgr.doQuery(null, QueryType.queryTypeStatKdActDayRank, user);
            genTable(m_result, res, user, mgr);
        }

        //排行榜
        private void genTable(Table table, OpRes res, GMUser user, QueryMgr mgr)
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
            List<kdActRankItem> qresult = (List<kdActRankItem>)mgr.getQueryResult(QueryType.queryTypeStatKdActDayRank);
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
                kdActRankItem item = qresult[i];
                m_content[f++] = item.m_rank.ToString();
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_gaindb.ToString();
                m_content[f++] = item.m_useCallup.ToString();
                m_content[f++] = item.m_costGold.ToString();

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