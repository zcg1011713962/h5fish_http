using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatKdActivityRank : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","排名", "玩家昵称", "玩家ID", "获取龙珠", "使用召唤数量", "消耗金币" };
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_KD_ACT_RANK, Session, Response);
            if (!IsPostBack)
            {
                m_queryType.Items.Add("日榜");
                m_queryType.Items.Add("周榜");

                m_timeType.Items.Add(new ListItem("当日榜"));
                m_timeType.Items.Add(new ListItem("昨日榜"));
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();

            param.m_op = m_queryType.SelectedIndex;
            if (param.m_op == 1)
            {
                param.m_param = "0";
            }
            else {
                param.m_param = (m_timeType.SelectedIndex+1).ToString();
                if(m_timeType.SelectedIndex == 1)
                    param.m_time = m_time.Text;
            }
            
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = OpRes.op_res_failed;
            res = mgr.doQuery(param, QueryType.queryTypeStatKdActRank, user);
            genTable(m_result, res, param, user, mgr);
        }

        //排行榜
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
            List<kdActRankItem> qresult = (List<kdActRankItem>)mgr.getQueryResult(QueryType.queryTypeStatKdActRank);
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
                m_content[f++] = item.m_time;
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