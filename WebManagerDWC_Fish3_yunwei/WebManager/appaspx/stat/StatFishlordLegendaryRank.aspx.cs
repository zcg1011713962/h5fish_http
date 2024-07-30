using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordLegendaryRank : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "排行", "昵称", "玩家ID", "积分" };
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_FISHLORD_LEGENDARY_RANK, Session, Response);
            if (!IsPostBack)
            {
                m_actId.Items.Add("巨鲲降世");
                m_actId.Items.Add("定海神针");

                m_rankType1.Items.Add("当前排行");
                m_rankType1.Items.Add("历史排行");

                m_rankType2.Items.Add("单炸排行");
                m_rankType2.Items.Add("累炸排行");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = m_actId.SelectedIndex;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = OpRes.op_res_failed;
            int isTime = 0;
            if (param.m_op == 1)
            {
                //定海神针
                if (m_rankType2.SelectedIndex == 0)
                {
                    param.m_param = "goldHoopTorpedoTopCoin"; // 单炸
                }
                else {
                    param.m_param = "goldHoopTorpedoMaxCoin"; //累炸
                }
                isTime = 0;
                res = mgr.doQuery(param, QueryType.queryTypeStatFishlordGoldHoopTorpedoRank, user);
                genTable(m_result, res, param, user, mgr, QueryType.queryTypeStatFishlordGoldHoopTorpedoRank,isTime);
            }
            else { 
                //巨鲲降世
                param.m_way = (QueryWay)m_rankType1.SelectedIndex;  //当前 历史
                if (param.m_way == QueryWay.by_way0) //当前
                {
                    isTime = 0;
                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordLegendaryCurRank, user);
                    genTable(m_result, res, param, user, mgr, QueryType.queryTypeStatFishlordLegendaryCurRank, isTime);
                }
                else if (param.m_way == QueryWay.by_way1)  //历史
                {
                    isTime = 1;
                    param.m_time = m_time.Text;
                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordLegendaryHisRank, user);
                    genTable(m_result, res, param, user, mgr, QueryType.queryTypeStatFishlordLegendaryHisRank, isTime);
                }
            }
        }

        //生成查询表 //当前排行
        private void genTable(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr,QueryType queryRank, int isTime)
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
            List<StatFishlordLegendaryRankItem> qresult = (List<StatFishlordLegendaryRankItem>)mgr.getQueryResult(queryRank);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                if (i == 0 && isTime == 0)
                    continue;
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
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_rankId.ToString();
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_score.ToString();
                for (j = 0; j < s_head.Length; j++)
                {
                    if (j == 0 && isTime == 0)
                        continue;
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }
    }
}