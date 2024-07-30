using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordSharkRoomAct : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] { "日期","玩法收入","玩法支出","抽奖类型",
            "豹子A次","豹子K次","豹子Q次","豹子J次","豹子10次","三元素次","同元素次","元素顺次","双元素次","单元素次","详情"};

        private static string[] s_head2 = new string[] { "日期", "轰炸类型", "轰炸产出金币", "轰炸次数", "轰炸人数", "使用金币轰炸次数", "使用金币轰炸产出金币"};

        //private static string[] s_head3 = new string[] { "日期","抽奖人数","抽奖次数","系统产出","详情"};
        private static string[] s_head3 = new string[] { "日期","当前奖池","类型","产出数量","系统支出"};

        private static string[] s_head4 = new string[] { "日期","排名","玩家ID","昵称","VIP等级","积分"};

        private static string[] s_head5 = new string[] { "日期", "礼包打开人数", "礼包打开次数" };

        private static string[] s_head6 = new string[] { "日期","参与人数","金币消耗","武装巨鲨击杀数量","能量产出数量"};

        private static string[] s_head7 = new string[] { "日期","拆解支出","拆解收入",
            "豹子A次","豹子K次","豹子Q次","豹子J次","豹子10次","三元素次","同元素次","详情"};

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_SHARK_ROOM_ACT, Session, Response);
            if (!IsPostBack)
            {
                m_queryType.Items.Add("玩法收入统计");
                m_queryType.Items.Add("轰炸机统计");
                m_queryType.Items.Add("巨鲨抽奖");
                m_queryType.Items.Add("排行榜");
                m_queryType.Items.Add("打点数据");
                m_queryType.Items.Add("能量统计");
                m_queryType.Items.Add("拆解统计");

                m_type.Items.Add("当前排行");
                m_type.Items.Add("历史排行");
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
                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordSharkRoomIncome, user);
                    genTable0(m_result, res, user, param);
                    break;
                case 1://轰炸机统计
                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordSharkRoomBomb, user);
                    genTable1(m_result, res, user, param);
                    break;
                case 2://巨鲨抽奖统计
                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordSharkRoomLottery, user);
                    genTable2(m_result, res, user, param);
                    break;
                case 3: //排行榜统计
                    //0当前排行 1历史排行
                    param.m_type = m_type.SelectedIndex + 1;
                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordSharkRoomRank, user);

                    genTable3(m_result, res, user, param);
                    break;
                case 4://打点数据
                    param.m_op = 14;
                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordMiddleRoomFuDai, user);
                    genTable4(m_result, res, user, param);
                    break;
                case 5://能量统计
                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordSharkRoomEnergy, user);
                    genTable5(m_result, res, user, param);
                    break;
                case 6://拆解统计
                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordSharkRoomChaijieIncome, user);
                    genTable6(m_result, res, user, param);
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

            List<StatSharkRoomItem> qresult =
                (List<StatSharkRoomItem>)user.getQueryResult(QueryType.queryTypeStatFishlordSharkRoomIncome);

            int i = 0, f = 0;
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
                StatSharkRoomItem item = qresult[i];

                foreach(var da in item.m_dataIndex)
                {
                    tr = new TableRow();
                    table.Rows.Add(tr);

                    if (f == 0)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_time;
                        td.RowSpan = item.m_dataIndex.Count;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = string.Format("{0:N0}", item.m_income);
                        td.RowSpan = item.m_dataIndex.Count;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = string.Format("{0:N0}", item.m_outlay);
                        td.RowSpan = item.m_dataIndex.Count;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = item.getLotteryName(da.Key);
                    td.RowSpan = 1;

                    foreach(var index in da.Value)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = index.ToString();
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = item.getDetail(da.Key);

                    f++;
                }
            }
        }

        //轰炸机统计
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

            List<StatSharkRoomBombItem> qresult =
                (List<StatSharkRoomBombItem>)user.getQueryResult(QueryType.queryTypeStatFishlordSharkRoomBomb);

            int i = 0;
            string[] m_content = new string[s_head2.Length];

            // 表头
            for (i = 0; i < s_head2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head2[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                StatSharkRoomBombItem item = qresult[i];

                foreach(var da in item.m_data)
                {
                    tr = new TableRow();
                    table.Rows.Add(tr);

                    if(da.Key == 0)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_time;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";
                        td.RowSpan = 2;
                        td.ColumnSpan = 1;
                    }
                     
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.Key == 0 ? "普通轰炸" : "黄金轰炸";
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;

                    if (da.Key == 0) 
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = da.Value[0].ToString();
                        td.Attributes.CssStyle.Value = "vertical-align:middle";
                        td.RowSpan = 2;
                        td.ColumnSpan = 1;
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.Value[1].ToString();
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.Value[2].ToString();
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.Value[3].ToString();
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.Value[4].ToString();
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                }
            }
        }

        //抽奖统计
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

            List<StatSharkRoomLotteryItem> qresult =
                (List<StatSharkRoomLotteryItem>)user.getQueryResult(QueryType.queryTypeStatFishlordSharkRoomLottery);

            int i = 0;
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
                StatSharkRoomLotteryItem item = qresult[i];

                tr = new TableRow();
                table.Rows.Add(tr);

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time;
                td.RowSpan = 3;
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_sysOutlay.ToString();
                td.RowSpan = 3;
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "铜钥匙";
                td.RowSpan = 1;

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_keyCount1.ToString();
                td.RowSpan = 1;

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_outlay1.ToString();
                td.RowSpan = 1;

                tr = new TableRow();
                table.Rows.Add(tr);

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "银钥匙";
                td.RowSpan = 1;

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_keyCount2.ToString();
                td.RowSpan = 1;

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_outlay2.ToString();
                td.RowSpan = 1;

                tr = new TableRow();
                table.Rows.Add(tr);

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "金钥匙";
                td.RowSpan = 1;

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_keyCount3.ToString();
                td.RowSpan = 1;

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_outlay3.ToString();
                td.RowSpan = 1;
            }
        }
        //private void genTable2(Table table, OpRes res, GMUser user, ParamQuery param)
        //{
        //    TableRow tr = new TableRow();
        //    table.Rows.Add(tr);
        //    TableCell td = null;

        //    if (res != OpRes.opres_success)
        //    {
        //        td = new TableCell();
        //        tr.Cells.Add(td);
        //        td.Text = OpResMgr.getInstance().getResultString(res);
        //        return;
        //    }

        //    List<StatSharkRoomLotteryItem> qresult =
        //        (List<StatSharkRoomLotteryItem>)user.getQueryResult(QueryType.queryTypeStatFishlordSharkRoomLottery);

        //    int i = 0, j = 0, f = 0;
        //    string[] m_content = new string[s_head3.Length];

        //    // 表头
        //    for (i = 0; i < s_head3.Length; i++)
        //    {
        //        td = new TableCell();
        //        tr.Cells.Add(td);
        //        td.Text = s_head3[i];
        //        td.RowSpan = 1;
        //        td.ColumnSpan = 1;
        //    }

        //    for (i = 0; i < qresult.Count; i++)
        //    {
        //        f = 0;
        //        tr = new TableRow();
        //        table.Rows.Add(tr);
        //        StatSharkRoomLotteryItem item = qresult[i];

        //        m_content[f++] = item.m_time;
        //        m_content[f++] = item.m_playerNum.ToString();
        //        m_content[f++] = item.m_playerCount.ToString();
        //        m_content[f++] = item.m_outlay.ToString();
        //        m_content[f++] = item.getDetail();

        //        for (j = 0; j < s_head3.Length; j++)
        //        {
        //            td = new TableCell();
        //            tr.Cells.Add(td);
        //            td.Text = m_content[j];
        //            td.RowSpan = 1;
        //            td.ColumnSpan = 1;
        //        }
        //    }
        //}

        //排行榜
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

            List<StatFishlordSharkRoomRank> qresult =
                (List<StatFishlordSharkRoomRank>)user.getQueryResult(QueryType.queryTypeStatFishlordSharkRoomRank);

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
                tr = new TableRow();
                table.Rows.Add(tr);
                StatFishlordSharkRoomRank item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_rankId.ToString();
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_vipLevel.ToString();
                m_content[f++] = item.m_points.ToString();

                for (j = 0; j < s_head4.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                }
            }
        }

        //打点统计
        private void genTable4(Table table, OpRes res, GMUser user, ParamQuery param)
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
            string[] m_content = new string[s_head5.Length];

            // 表头
            for (i = 0; i < s_head5.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head5[i];
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
                for (j = 0; j < s_head5.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                }
            }
        }

        //能量统计
        private void genTable5(Table table, OpRes res, GMUser user, ParamQuery param)
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

            List<StatFishlordSharkRoomEnergy> qresult =
                (List<StatFishlordSharkRoomEnergy>)user.getQueryResult(QueryType.queryTypeStatFishlordSharkRoomEnergy);

            int i = 0, j = 0, f = 0;
            string[] m_content = new string[s_head6.Length];

            // 表头
            for (i = 0; i < s_head6.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head6[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                table.Rows.Add(tr);
                StatFishlordSharkRoomEnergy item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_joinCount.ToString();
                m_content[f++] = item.m_goldConsume.ToString();
                m_content[f++] = item.m_sharkKillCount.ToString();
                m_content[f++] = item.m_energyOutlay.ToString();

                for (j = 0; j < s_head6.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                }
            }
        }

        //拆解统计表
        private void genTable6(Table table, OpRes res, GMUser user, ParamQuery param)
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

            List<StatSharkRoomChaijieItem> qresult =
                (List<StatSharkRoomChaijieItem>)user.getQueryResult(QueryType.queryTypeStatFishlordSharkRoomChaijieIncome);

            int i = 0;// f = 0;
            string[] m_content = new string[s_head7.Length];

            // 表头
            for (i = 0; i < s_head7.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head7[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
               // f = 0;
                StatSharkRoomChaijieItem item = qresult[i];

                tr = new TableRow();
                table.Rows.Add(tr);

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time;
                td.RowSpan = 1;
                td.ColumnSpan = 1;

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = string.Format("{0:N0}", item.m_outlay);
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = string.Format("{0:N0}", item.m_income);
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                foreach (var da in item.m_dataIndex)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.ToString();
                }

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.getDetail(0);
            }
        }
    }
}