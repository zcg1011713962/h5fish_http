using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordLegendaryRankCheat : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "玩家ID","昵称","积分"};
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_FISHLORD_LEGENDARY_RANK_CHEAT, Session, Response);
            if (!IsPostBack) {
                m_editType.Items.Add("巨鲲降世积分");
                m_editType.Items.Add("定海神针单炸积分");
                m_editType.Items.Add("定海神针累积分");
            }
            m_res.InnerHtml = "";
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = m_editType.SelectedIndex;
            param.m_playerId = m_playerId.Text;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatFishlordGoldHoopTorpedoScore, user);
            genTable(m_result, res, param, user, mgr);
        }
        //生成查询表 //当前排行
        private void genTable(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            string[] m_content = new string[s_head.Length];

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
            List<StatFishlordLegendaryRankItem> qresult = (List<StatFishlordLegendaryRankItem>)mgr.getQueryResult(QueryType.queryTypeStatFishlordGoldHoopTorpedoScore);
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
                table.Rows.Add(tr);
                f = 0;
                StatFishlordLegendaryRankItem item = qresult[i];
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_score.ToString();
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }
    }
}