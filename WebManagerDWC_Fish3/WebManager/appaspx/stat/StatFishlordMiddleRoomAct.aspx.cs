using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordMiddleRoomAct : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] { "日期", "玩法收入", "玩法支出","Buff收入","Buff支出", "黑桃产出", "红心产出", "梅花产出", "方块产出", "皇冠产出" };
        private static string[] s_head2 = new string[] { "日期", "兑换1次数/人数", "兑换2次数/人数", "兑换3次数/人数", "兑换4次数/人数", "兑换5次数/人数", 
            "兑换6次数/人数", "兑换7次数/人数", "兑换8次数/人数","兑换9次数/人数", "金币产出总计","详情","详情"};

        //当前排行
        private static string[] s_head5 = new string[] { "排名", "玩家昵称", "玩家ID", "最高积分", "累计积分" };

        //历史排行
        private static string[] s_head3 = new string[] { "日期", "排名", "玩家ID", "昵称", "积分", "VIP等级" };
        private static string[] s_head4 = new string[] { "日期","福袋ICON打开人数","福袋打开次数"};

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_MIDDLE_ROOM_ACT, Session, Response);
            if (!IsPostBack)
            {
                m_queryType.Items.Add("玩法收入统计");
                m_queryType.Items.Add("兑换统计");
                m_queryType.Items.Add("排行榜统计");
                m_queryType.Items.Add("打点数据");

                m_type.Items.Add("当前排行");
                m_type.Items.Add("历史排行");

                m_rankType.Items.Add(new ListItem("当前总积分"));
                m_rankType.Items.Add(new ListItem("单次最高积分"));

                m_rankType1.Items.Add(new ListItem("牛人榜"));
                m_rankType1.Items.Add(new ListItem("幸运榜"));
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();

            param.m_op = m_queryType.SelectedIndex;
            param.m_time = m_time.Text;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = OpRes.op_res_failed;
            switch (param.m_op)
            {
                case 0://玩法收入统计
                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordMiddleRoomIncome, user);
                    genTable0(m_result, res, user, param);
                    break;
                case 1://兑换统计
                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordMiddleRoomExchange, user);
                    genTable1(m_result, res, user, param);
                    break;
                case 2: //排行榜统计

                    //当前排行 历史排行
                    if (m_type.SelectedIndex == 0)
                    {
                        param.m_type = m_rankType.SelectedIndex + 1; //1当前总积分 2单次最高积分  
                        res = mgr.doQuery(param, QueryType.queryTypeStatFishlordMiddleRoomPlayerRank, user);
                        genTable(m_result, res, user, mgr);
                    }
                    else 
                    {
                        param.m_type = m_rankType1.SelectedIndex + 1; //1牛人榜 2幸运榜  
                        res = mgr.doQuery(param, QueryType.queryTypeStatFishlordMiddleRoomRank, user);
                        genTable2(m_result, res, user, param);
                    }
                    break;
                case 3://打点数据
                    param.m_op = 6;
                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordMiddleRoomFuDai, user);
                    genTable3(m_result, res, user, param);
                    break;
            }
        }

        //玩法收入统计表
        private void genTable0(Table table, OpRes res, GMUser user, ParamQuery param)
        {
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

            List<StatFishlordMiddleRoomIncomeItem> qresult =
                (List<StatFishlordMiddleRoomIncomeItem>)user.getQueryResult(QueryType.queryTypeStatFishlordMiddleRoomIncome);

            int i = 0, j = 0, f = 0;
            string[] m_content = new string[s_head1.Length];

            // 表头
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head1[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                StatFishlordMiddleRoomIncomeItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = string.Format("{0:N0}", item.m_income);
                m_content[f++] = string.Format("{0:N0}", item.m_outlay);

                m_content[f++] = string.Format("{0:N0}", item.m_buffIncome);
                m_content[f++] = string.Format("{0:N0}", item.m_buffOutlay);

                m_content[f++] = item.m_item18.ToString();
                m_content[f++] = item.m_item19.ToString();
                m_content[f++] = item.m_item20.ToString();
                m_content[f++] = item.m_item21.ToString();
                m_content[f++] = item.m_item52.ToString();

                tr = new TableRow();
                table.Rows.Add(tr);
                for (j = 0; j < s_head1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }

        //统计表
        private void genTable1(Table table, OpRes res, GMUser user, ParamQuery param)
        {
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

            List<StatFishlordMiddleRoomExchangeItem> qresult =
                (List<StatFishlordMiddleRoomExchangeItem>)user.getQueryResult(QueryType.queryTypeStatFishlordMiddleRoomExchange);

            int i = 0, j = 0, f = 0;
            string[] m_content = new string[s_head2.Length];

            // 表头
            for (i = 0; i < s_head2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head2[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
                if(i > 0)
                    td.Attributes.CssStyle.Value = "min-width:120px";
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                StatFishlordMiddleRoomExchangeItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_exchange1.ToString() + '/' + item.m_exchangeCount[1];
                m_content[f++] = item.m_exchange2.ToString() + '/' + item.m_exchangeCount[2];
                m_content[f++] = item.m_exchange3.ToString() + '/' + item.m_exchangeCount[3];
                m_content[f++] = item.m_exchange4.ToString() + '/' + item.m_exchangeCount[4];
                m_content[f++] = item.m_exchange5.ToString() + '/' + item.m_exchangeCount[5];
                m_content[f++] = item.m_exchange6.ToString() + '/' + item.m_exchangeCount[6];
                m_content[f++] = item.m_exchange7.ToString() + '/' + item.m_exchangeCount[7];
                m_content[f++] = item.m_exchange8.ToString() + '/' + item.m_exchangeCount[8];
                m_content[f++] = item.m_exchange9.ToString() + '/' + item.m_exchangeCount[9];
                m_content[f++] = item.m_goldOutlay.ToString();
                m_content[f++] = "兑换6额外：" + item.m_exgoldvalue6 + 
                                "；兑换7额外： " + item.m_exgoldvalue7 +
                                " ；兑换8额外： " + item.m_exgoldvalue8;
                m_content[f++] = item.getDetail();

                tr = new TableRow();
                table.Rows.Add(tr);
                for (j = 0; j < s_head2.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }

        //当前排行 排行榜
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
            List<FishlordMiddleRoomPlayerRankItem> qresult =
                (List<FishlordMiddleRoomPlayerRankItem>)mgr.getQueryResult(QueryType.queryTypeStatFishlordMiddleRoomPlayerRank);
            int i = 0, j = 0, f = 0;
            string[] m_content = new string[s_head5.Length];
            // 表头
            for (i = 0; i < s_head5.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head5[i];
                td.RowSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;
                FishlordMiddleRoomPlayerRankItem item = qresult[i];
                m_content[f++] = item.m_rankId.ToString();
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_maxScore.ToString();
                m_content[f++] = item.m_accScore.ToString();
                for (j = 0; j < s_head5.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.RowSpan = 1;
                }
            }
        }

        //历史排行统计表
        private void genTable2(Table table, OpRes res, GMUser user, ParamQuery param)
        {
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

            List<StatFishlordMiddleRoomRankItem> qresult =
                (List<StatFishlordMiddleRoomRankItem>)user.getQueryResult(QueryType.queryTypeStatFishlordMiddleRoomRank);

            int i = 0, j = 0, f = 0;
            string[] m_content = new string[s_head3.Length];

            // 表头
            for (i = 0; i < s_head3.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head3[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                StatFishlordMiddleRoomRankItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_rankId.ToString();
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_score.ToString();
                m_content[f++] = item.m_vipLevel.ToString();

                tr = new TableRow();
                table.Rows.Add(tr);
                for (j = 0; j < s_head3.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }

        //统计表
        private void genTable3(Table table, OpRes res, GMUser user, ParamQuery param)
        {
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

            List<StatFishlordMiddleRoomFuDaiItem> qresult =
                (List<StatFishlordMiddleRoomFuDaiItem>)user.getQueryResult(QueryType.queryTypeStatFishlordMiddleRoomFuDai);

            int i = 0, j = 0, f = 0;
            string[] m_content = new string[s_head4.Length];

            // 表头
            for (i = 0; i < s_head4.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head4[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                StatFishlordMiddleRoomFuDaiItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_action1Count.ToString();
                m_content[f++] = item.m_action2Count.ToString();

                tr = new TableRow();
                table.Rows.Add(tr);
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