using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordLegendaryFishRoom : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] { "日期","玩法收入","玩法支出","巨鲲击杀数量","巨鲲币产出","消耗钻石","召唤产出","分红产出"};
        private static string[] s_head2 = new string[] { "日期","孵化数量","加速数量","奖励1数量","奖励2数量","奖励3数量","奖励4数量","孵化价值",
            "礼包水晶数量","定海神针产出"};
        private static string[] s_head3 = new string[] { "日期","系统收入","系统支出","鲲币支出"};
        private static string[] s_head4 = new string[] { "日期","鲲币消耗","钻石消耗","刷新令消耗"};
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_FISHLORD_LEGENDARY_FISH_ROOM, Session, Response);
            if (!IsPostBack)
            {
                m_actId.Items.Add("玩法收入统计");
                m_actId.Items.Add("孵化巨鲲统计");
                m_actId.Items.Add("鲲蛋鱼统计");
                m_actId.Items.Add("魔鲲商店");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = m_actId.SelectedIndex;
            param.m_time = m_time.Text;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = OpRes.op_res_failed;
            switch (param.m_op) 
            {
                case 0 :  //玩法收入统计
                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordLegendaryFishRoomplay, user);
                    genTable(m_result, res, param, user, mgr);
                    break;
                case 1 :  //孵化巨鲲统计
                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordLegendaryFishRoom, user);
                    genTable1(m_result, res, param, user, mgr);
                    break;
                case 2 : //鲲蛋鱼统计
                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordLegendaryFishRoomCoin, user);
                    genTable2(m_result, res, param, user, mgr);
                    break;
                case 3: //魔鲲商店
                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordLegendaryFishRoomShop, user);
                    genTable3(m_result, res, param, user, mgr);
                    break;
            }
        }

        //生成查询表 玩法收入统计
        private void genTable(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            string[] m_content = new string[s_head1.Length];

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
            List<StatLegendaryFishRoomPlayItem> qresult = (List<StatLegendaryFishRoomPlayItem>)mgr.getQueryResult(QueryType.queryTypeStatFishlordLegendaryFishRoomplay);
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
                table.Rows.Add(tr);
                f = 0;
                StatLegendaryFishRoomPlayItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_income.ToString();
                m_content[f++] = item.m_outlay.ToString();
                m_content[f++] = item.m_killCount.ToString();
                m_content[f++] = item.m_coinOutlay.ToString();
                m_content[f++] = item.m_callBossDiamondIncome.ToString();
                m_content[f++] = item.m_callBossGoldOutlay.ToString();
                m_content[f++] = item.m_bounsOutlay.ToString();
                for (j = 0; j < s_head1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }
        //生成查询表 孵化巨鲲统计
        private void genTable1(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            string[] m_content = new string[s_head2.Length];

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
            List<StatLegendaryFishRoomHatchItem> qresult = (List<StatLegendaryFishRoomHatchItem>)mgr.getQueryResult(QueryType.queryTypeStatFishlordLegendaryFishRoom);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head2[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                StatLegendaryFishRoomHatchItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_hatchCount.ToString();
                m_content[f++] = item.m_hatchAtOnceCount.ToString();
                m_content[f++] = item.m_hatchRewardCount1.ToString();
                m_content[f++] = item.m_hatchRewardCount2.ToString();
                m_content[f++] = item.m_hatchRewardCount3.ToString();
                m_content[f++] = item.m_hatchRewardCount4.ToString();
                m_content[f++] = item.m_hatchOutlayValue.ToString();
                m_content[f++] = item.m_HoopTorpedoChipFromGift.ToString();
                m_content[f++] = item.m_composeHoopTorpedoCount.ToString();

                for (j = 0; j < s_head2.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }

        //鲲蛋鱼统计
        private void genTable2(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            string[] m_content = new string[s_head3.Length];

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
            List<StatLegendaryFishRoomPlayItem> qresult = (List<StatLegendaryFishRoomPlayItem>)mgr.getQueryResult(QueryType.queryTypeStatFishlordLegendaryFishRoomCoin);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head3.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head3[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                StatLegendaryFishRoomPlayItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_income.ToString();
                m_content[f++] = item.m_outlay.ToString();
                m_content[f++] = item.m_coinOutlay.ToString();
                for (j = 0; j < s_head3.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }

        //魔鲲商店
        private void genTable3(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            string[] m_content = new string[s_head3.Length];

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
            List<StatLegendaryFishRoomShop> qresult = 
                    (List<StatLegendaryFishRoomShop>)mgr.getQueryResult(QueryType.queryTypeStatFishlordLegendaryFishRoomShop);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head4.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head4[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                StatLegendaryFishRoomShop item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_coinIncome.ToString();
                m_content[f++] = item.m_diamondCost.ToString();
                m_content[f++] = item.m_refreshItemCost.ToString();
                for (j = 0; j < s_head4.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }
    }
}