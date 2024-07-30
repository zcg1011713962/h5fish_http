using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordDragonPalaceData : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] {"日期","参与人数","兑换总次数","玩法收入","小龙虾击杀数量",
            "小神龙击杀数量","龙王击杀数量","魔石总产出","兑换金币产出","兑换鱼雷价值",
            "普通鱼雷兑换人数/次数","青铜鱼雷兑换人数/次数","白银鱼雷兑换人数/次数","黄金鱼雷兑换人数/次数","钻石鱼雷兑换人数/次数"};
            //"兑换普通鱼雷产出", "兑换青铜鱼雷产出","兑换白银鱼雷产出","兑换黄金鱼雷产出","兑换钻石鱼雷产出"};

        private static string[] s_head3 = new string[] { "日期","场次收入","场次支出"};

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_DRAGON_PALACE_DATA_STAT, Session, Response);
            if (!IsPostBack)
            {
                m_item.Items.Add("龙宫场消耗统计");
                m_item.Items.Add("龙宫场玩家分布统计");
                m_item.Items.Add("诛龙箭统计");

                m_roomId.Items.Add(new ListItem("初级龙宫场", "5"));
                m_roomId.Items.Add(new ListItem("高级龙宫场", "7"));
                m_roomId.Items.Add(new ListItem("捕鱼大厅", "0"));
            }
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_way = (QueryWay)m_item.SelectedIndex;
            param.m_time = m_time.Text;
            param.m_op = Convert.ToInt32(m_roomId.SelectedValue);
            OpRes res = OpRes.op_res_failed;
            switch (param.m_way)
            {
                case QueryWay.by_way0://龙宫场消耗统计
                    res = user.doQuery(param, QueryType.queryTypeFishlordDragonPalaceConsumeStat);
                    genTable0(m_result, res, user, param);
                    break;
                case QueryWay.by_way1: //龙宫场玩家分布统计
                    res = user.doQuery(param, QueryType.queryTypeFishlordDragonPalacePlayerStat);
                    genTable1(m_result, res, user, param);
                    break;
                case QueryWay.by_way2: //诛龙箭
                    res = user.doQuery(param, QueryType.queryTypeFishlordDragonPalaceKillDragon);
                    genTable2(m_result, res, user, param);
                    break;
            }
        }

        //龙宫场消耗统计
        private void genTable0(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            string[] m_content = new string[s_head1.Length];

            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            table.Attributes.CssStyle.Value = "width:100%";
            TableCell td = null;

            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }

            List<DragonPalaceConsumeStat> qresult =
                (List<DragonPalaceConsumeStat>)user.getQueryResult(QueryType.queryTypeFishlordDragonPalaceConsumeStat);

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
                DragonPalaceConsumeStat item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_joinPerson.ToString();
                m_content[f++] = item.m_exchangeCount.ToString();
                m_content[f++] = item.m_goldConsume.ToString();  //金币消耗 改为  玩法收入
                m_content[f++] = item.m_killFish1.ToString();
                m_content[f++] = item.m_killFish2.ToString();
                m_content[f++] = item.m_killFish3.ToString();

                m_content[f++] = item.m_exchangeItem2.ToString(); //魔石
                m_content[f++] = item.m_exchangeItem1.ToString(); //金币

                //兑换鱼雷价值
                long goldValue = item.m_exchangeItem23 * item.getItem(23) +
                                 item.m_exchangeItem24 * item.getItem(24) +
                                 item.m_exchangeItem25 * item.getItem(25) +
                                 item.m_exchangeItem26 * item.getItem(26) +
                                 item.m_exchangeItem27 * item.getItem(27);
                m_content[f++] = goldValue.ToString();

                foreach (var da in item.m_exchangeItems) 
                {
                    m_content[f++] = da.Value[0] + "&ensp;| &ensp;" + da.Value[1];
                }

                for (j = 0; j < s_head1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }

        //东海龙宫场玩家分布统计
        private void genTable1(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);

            table.Attributes.CssStyle.Value = "width:100%";
            TableCell td = null;

            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }

            List<DragonPalacePlayerItem> qresult = (List<DragonPalacePlayerItem>)user.getQueryResult(QueryType.queryTypeFishlordDragonPalacePlayerStat);

            int i = 0, j = 0, k = 0;
            // 表头
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "日期";
            td.Attributes.CssStyle.Value = "vertical-align:middle;";
            td.RowSpan = 2;

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "等级";
            td.Attributes.CssStyle.Value = "min-width:80px;";

            if (query_param.m_op == 5)
            {
                j = 16; k = 30; 
            }
            else {
                j = 30; k = 43;
            }
            for (i = j; i <= k; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "Level_" + i;
            }

            tr = new TableRow();
            tr.Cells.Clear();
            m_result.Rows.Add(tr);
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "倍率";

            for (i = j; i <= k; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);

                Fish_LevelCFGData data = Fish_TurretLevelCFG.getInstance().getValue(i);
                if (data != null)
                {
                    td.Text = data.m_openRate.ToString();
                }
                else
                {
                    td.Text = "Level_" + i;
                }
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                DragonPalacePlayerItem item = qresult[i];

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time;

                if (i == 0)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = "参与人数";
                    td.Attributes.CssStyle.Value = "vertical-align:middle;";
                    td.RowSpan = qresult.Count;
                }
                for (int m = 0; m < item.m_data.Length; m++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = item.m_data[m].ToString();
                    td.ColumnSpan = 1;
                }
            }
        }

        //龙宫场诛龙箭统计
        private void genTable2(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            string[] m_content = new string[s_head3.Length];

            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            table.Attributes.CssStyle.Value = "width:70%;margin:0 auto";
            TableCell td = null;

            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }

            List<DragonPalaceKillItem> qresult =
                (List<DragonPalaceKillItem>)user.getQueryResult(QueryType.queryTypeFishlordDragonPalaceKillDragon);

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
                DragonPalaceKillItem item = qresult[i];

                m_content[f++] = item.m_time;

                m_content[f++] = item.m_itemIncome.ToString();
                m_content[f++] = item.m_itemOutlay.ToString();

                for (j = 0; j < s_head3.Length; j++)
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