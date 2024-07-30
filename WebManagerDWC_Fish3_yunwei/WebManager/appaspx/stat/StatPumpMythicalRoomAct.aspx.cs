using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPumpMythicalRoomAct : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[]{"日期","朱雀系统收入","朱雀系统支出","朱雀死亡次数","礼包转盘次数"};
        private static string[] s_head2 = new string[] { "日期","白虎触发次数","白虎系统收入","白虎系统支出","白虎死亡次数","白虎青铜鱼雷","白虎白银鱼雷"};
        private static string[] s_head3 = new string[] { "日期","玄武触发次数","玄武系统收入","玄武系统支出","玄武死亡次数","逃跑玄武携带金币"};
        private static string[] s_head4 = new string[] { "日期","财神触发次数","财神系统收入","财神系统支出","财神死亡次数","双倍触发次数","双倍额外金币"};
        private static string[] s_head5 = new string[] { "日期","排名","玩家ID","昵称","积分"};
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_PUMP_MYTHICAL_ROOM_ACT, Session, Response);
            if (!IsPostBack)
            {
                m_item.Items.Add("朱雀收入统计");
                m_item.Items.Add("白虎收入统计");
                m_item.Items.Add("玄武收入统计");
                m_item.Items.Add("财神双倍玩法统计");
                m_item.Items.Add("排行榜");

                m_rank.Items.Add("当前排行");
                m_rank.Items.Add("历史排行");
            }
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = m_item.SelectedIndex;
            param.m_time = m_time.Text;
            OpRes res = OpRes.op_res_failed;
            switch (param.m_op)
            {
                case 0: //朱雀收入统计
                    res = user.doQuery(param, QueryType.queryTypeStatMythicalRoomZhuque);
                    genTable0(m_result, res, user, param);
                    break;
                case 1: //白虎收入统计
                    res = user.doQuery(param, QueryType.queryTypeStatMythicalRoomBaihu);
                    genTable1(m_result, res, user, param);
                    break;
                case 2: //玄武收入统计
                    res = user.doQuery(param, QueryType.queryTypeStatMythicalRoomXuanwu);
                    genTable2(m_result, res, user, param);
                    break;
                case 3: //财神双倍玩法统计
                    res = user.doQuery(param, QueryType.queryTypeStatMythicalRoomCaishen);
                    genTable3(m_result, res, user, param);
                    break;
                case 4: //排行榜

                    int rankType = m_rank.SelectedIndex;
                    if (rankType == 0) //当前排行
                    {
                        res = user.doQuery(param, QueryType.queryTypeStatMythicalRoomCurrRank);
                        genTable4(m_result, res, user, param, rankType, QueryType.queryTypeStatMythicalRoomCurrRank);
                    }
                    else //历史排行
                    {
                        res = user.doQuery(param, QueryType.queryTypeStatMythicalRoomHisRank);
                        genTable4(m_result, res, user, param, rankType, QueryType.queryTypeStatMythicalRoomHisRank);
                    }
                    break;
            }
        }

        //朱雀收入统计
        private void genTable0(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            string[] m_content = new string[s_head1.Length];

            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }

            List<StatMythicalRoomZhuqueItem> qresult =
                (List<StatMythicalRoomZhuqueItem>)user.getQueryResult(QueryType.queryTypeStatMythicalRoomZhuque);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.RowSpan = 1;
                td.Text = s_head1[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                StatMythicalRoomZhuqueItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_zhuqueIncome.ToString();
                m_content[f++] = item.m_zhuqueOutlay.ToString();
                m_content[f++] = item.m_zhuquedeadCount.ToString();
                m_content[f++] = item.m_giftLotteryCount.ToString();

                for (j = 0; j < s_head1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }

        //白虎收入统计
        private void genTable1(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            string[] m_content = new string[s_head2.Length];

            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }

            List<StatMythicalRoomBaihuItem> qresult =
                (List<StatMythicalRoomBaihuItem>)user.getQueryResult(QueryType.queryTypeStatMythicalRoomBaihu);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.RowSpan = 1;
                td.Text = s_head2[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                StatMythicalRoomBaihuItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_triggerCount.ToString();
                m_content[f++] = item.m_baihuIncome.ToString();
                m_content[f++] = item.m_baihuOutlay.ToString();
                m_content[f++] = item.m_baihudeadCount.ToString();
                m_content[f++] = item.m_normalTorpedoOutlay.ToString();
                m_content[f++] = item.m_silverTorpedoOutlay.ToString();

                for (j = 0; j < s_head2.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }

        //玄武收入统计
        private void genTable2(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            string[] m_content = new string[s_head3.Length];

            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }

            List<StatMythicalRoomXuanwuItem> qresult =
                (List<StatMythicalRoomXuanwuItem>)user.getQueryResult(QueryType.queryTypeStatMythicalRoomXuanwu);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head3.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.RowSpan = 1;
                td.Text = s_head3[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                StatMythicalRoomXuanwuItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_triggerCount.ToString();
                m_content[f++] = item.m_xuanwuIncome.ToString();
                m_content[f++] = item.m_xuanwuOutlay.ToString();
                m_content[f++] = item.m_xuanwudeadCount.ToString();
                m_content[f++] = item.m_xuanwuEscapeGold.ToString();

                for (j = 0; j < s_head3.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }

        //财神双倍玩法统计
        private void genTable3(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            string[] m_content = new string[s_head4.Length];

            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }

            List<StatMythicalRoomCaishenItem> qresult =
                (List<StatMythicalRoomCaishenItem>)user.getQueryResult(QueryType.queryTypeStatMythicalRoomCaishen);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head4.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.RowSpan = 1;
                td.Text = s_head4[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                StatMythicalRoomCaishenItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_triggerCount.ToString();
                m_content[f++] = item.m_caishenIncome.ToString();
                m_content[f++] = item.m_caishenOutlay.ToString();
                m_content[f++] = item.m_caishendeadCount.ToString();
                m_content[f++] = item.m_event4.ToString();
                m_content[f++] = item.m_event4Gold.ToString();

                for (j = 0; j < s_head4.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }

        //当前排行榜 / 历史排行
        private void genTable4(Table table, OpRes res, GMUser user, ParamQuery query_param, int rankType, QueryType queryType)
        {
            int len = s_head5.Length;
            if (rankType == 0)
                len -= 1;

            string[] m_content = new string[len];
            
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }

            List<StatMythicalRoomRankItem> qresult =
                (List<StatMythicalRoomRankItem>)user.getQueryResult(queryType);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < len; i++)
            {
                if (rankType == 0)
                {
                    j = i + 1;
                }
                else {
                    j = i;
                }

                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.RowSpan = 1;
                td.Text = s_head5[j];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                StatMythicalRoomRankItem item = qresult[i];
                if (rankType == 1)
                {
                    m_content[f++] = item.m_time;
                }
                m_content[f++] = item.m_rankId.ToString();
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_score.ToString();

                for (j = 0; j < len; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }
    }
}