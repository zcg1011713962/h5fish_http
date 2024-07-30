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

        private static string[] s_head_2 = new string[] { "类型","玩家昵称","玩家ID","鱼雷分值"};

        private static string[] s_head_3 = new string[] { "日期", "玩家昵称", "玩家ID", "鱼雷分值" };

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_BULLET_HEAD_RANK, Session, Response);

            if(!IsPostBack)
            {
                m_type.Items.Add("普通排行");
                m_type.Items.Add("青铜排行");
                m_type.Items.Add("白银排行");
                m_type.Items.Add("黄金排行");
                m_type.Items.Add("钻石排行");
                m_type.Items.Add("周排行");

                m_type.Items.Add("普通入围");
                m_type.Items.Add("青铜入围");
                m_type.Items.Add("白银入围");
                m_type.Items.Add("黄金入围");
                m_type.Items.Add("钻石入围");

                m_actType.Items.Add("当前记录");
                m_actType.Items.Add("历史记录");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {

            GMUser user=(GMUser)Session["user"];
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);

            ParamQuery param = new ParamQuery();
            param.m_op = m_type.SelectedIndex;//0普通 1青铜 2白银 3黄金 4钻石排行 5周排行  6入围...
            param.m_way = (QueryWay)m_actType.SelectedIndex;  //当前 历史

            OpRes res = OpRes.op_res_failed;
            if (param.m_way == QueryWay.by_way0) //当前
            {
                if (param.m_op <= 5) //排行
                {
                    res = mgr.doQuery(param, QueryType.queryTypeBulletHeadCurrRank, user);
                    genTable(m_result, res, param, user, mgr);
                }
                else  //入围
                {
                    res = mgr.doQuery(param, QueryType.queryTypeBulletHeadGuaranteed, user);
                    genTable3(m_result, res, param, user, mgr);
                }

            }
            else if (param.m_way == QueryWay.by_way1)  //历史
            {
                param.m_time = m_time.Text;
                if (param.m_op <= 5)
                {
                    res = mgr.doQuery(param, QueryType.queryTypeBulletHeadRank, user);
                    if (param.m_op == 7)
                        param.m_op = 5;
                    genTable1(m_result, res, param, user, mgr);
                }
                else 
                {
                    //入围历史
                    res = mgr.doQuery(param, QueryType.queryTypeBulletHeadHisGuaranteed, user);
                    genTable2(m_result, res, param, user, mgr);
                }
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

        //生成查询表 //入围玩家 当前
        private void genTable3(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            string[] m_content = new string[s_head_2.Length];

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
            List<StatBulletHeadGuaranteedItem> qresult =
                (List<StatBulletHeadGuaranteedItem>)mgr.getQueryResult(QueryType.queryTypeBulletHeadGuaranteed);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head_2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head_2[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                StatBulletHeadGuaranteedItem item = qresult[i];

                m_content[f++] = getRankName(query_param.m_op);
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_maxGold.ToString();

                for (j = 0; j < s_head_2.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }


        //生成查询表 //入围玩家 历史
        private void genTable2(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            string[] m_content = new string[s_head_3.Length];

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
            List<StatBulletHeadGuaranteedItem> qresult =
                (List<StatBulletHeadGuaranteedItem>)mgr.getQueryResult(QueryType.queryTypeBulletHeadHisGuaranteed);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head_3.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head_3[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                StatBulletHeadGuaranteedItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_maxGold.ToString();

                for (j = 0; j < s_head_3.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }


        public string getRankName(int key) 
        {
            switch (key) {
                case 0: return "普通排行";
                case 1: return "青铜排行";
                case 2: return "白银排行";
                case 3: return "黄金排行";
                case 4: return "钻石排行";
                case 5: return "周排行";
                case 6: return "普通入围";
                case 7: return "青铜入围";
                case 8: return "白银入围";
                case 9: return "黄金入围";
                case 10: return "钻石入围";
            }
            return "";
        }
    }
}