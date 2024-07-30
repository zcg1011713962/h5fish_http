using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatBulletHeadRank : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "排行榜","类型", "排行", "玩家昵称", "玩家ID","VipLevel", "鱼雷分值" };
        private string[] m_content = new string[s_head.Length];

        private static string[] s_head_1 = new string[] { "获奖日期","类型","获奖类型","排名","玩家昵称","玩家id","鱼雷分值","奖励项","使用鱼雷数量"};
        private string[] m_content_1 = new string[s_head_1.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_BULLET_HEAD_RANK, Session, Response);

            if(!IsPostBack)
            {
                m_type.Items.Add("青铜排行");
                m_type.Items.Add("白银排行");
                m_type.Items.Add("黄金排行");
                m_type.Items.Add("钻石排行");

                m_actType.Items.Add("当前排行");
                m_actType.Items.Add("历史排行");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {

            GMUser user=(GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = m_type.SelectedIndex;//0青铜 1白银 2黄金 3钻石排行
            param.m_way = (QueryWay)m_actType.SelectedIndex;  //当前 历史
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            if (param.m_way == QueryWay.by_way0) //当前
            {
                OpRes res = mgr.doQuery(param, QueryType.queryTypeBulletHeadCurrRank, user);
                genTable(m_result, res, param, user, mgr);

            }else if(param.m_way==QueryWay.by_way1)  //历史
            {
                param.m_time = m_time.Text;
                OpRes res = mgr.doQuery(param, QueryType.queryTypeBulletHeadRank, user);
                genTable1(m_result, res, param, user, mgr);
            }
        }

        //生成查询表 //当前排行
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
            List<bulletHeadRankItem> qresult = (List<bulletHeadRankItem>)mgr.getQueryResult(QueryType.queryTypeBulletHeadCurrRank);
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
                bulletHeadRankItem item = qresult[i];
                m_content[f++] = getRankName(query_param.m_op);
                m_content[f++] = item.getContentType();
                m_content[f++] = item.m_rank.ToString();
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_type.ToString();
                m_content[f++] = item.m_maxGold.ToString();
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }

        //生成查询表 //历史排行
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
            List<bulletHeadRankItem> qresult = (List<bulletHeadRankItem>)mgr.getQueryResult(QueryType.queryTypeBulletHeadRank);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head_1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head_1[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                bulletHeadRankItem item = qresult[i];
                m_content_1[f++] = item.m_time;
                m_content_1[f++] = item.getContentType();
                m_content_1[f++] = getRankName(query_param.m_op);
                m_content_1[f++] = item.m_rank.ToString();
                m_content_1[f++] = item.m_nickName;
                m_content_1[f++] = item.m_playerId.ToString();
                m_content_1[f++] = item.m_maxGold.ToString();
                m_content_1[f++] = item.m_rewardName;
                m_content_1[f++] = item.m_useCount.ToString();
                for (j = 0; j < s_head_1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content_1[j];
                }
            }
        }

        public string getRankName(int key) 
        {
            switch (key) {
                case 0: return "青铜排行";
                case 1: return "白银排行";
                case 2: return "黄金排行";
                case 3: return "钻石排行";
            }
            return "";
        }
    }
}