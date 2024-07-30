using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatRoomPoint : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] { "日期", "玩家ID", "BOSS类型", "使用炮倍", "BOSS倍率", "获得积分" };
        private static string[] s_head2 = new string[] { "日期","玩家ID","轰炸积分"};

        private static string[] s_head3 = new string[] { "日期","玩家ID","使用炮倍","获得金币","额外奖励"};
        private static string[] s_head4 = new string[] { "日期","玩家ID","使用炮倍","鱼类型","获得金币","获得鲲币"};

        private PageFishlordFeast m_gen = new PageFishlordFeast(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";

            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_ROOM_POINT, Session, Response);
            if (!IsPostBack)
            {
                m_item.Items.Add("圣兽场打点");
                m_item.Items.Add("巨鲨场打点");
                m_item.Items.Add("聚宝鱼统计");
                m_item.Items.Add("魔鲲场统计");

                if (m_gen.parse(Request))
                {
                    m_item.SelectedIndex = m_gen.m_op;
                    m_time.Text = m_gen.m_time;
                    OnStat(null, null);
                }
            }
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = m_item.SelectedIndex;
            param.m_time = m_time.Text;

            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;

            OpRes res = OpRes.op_res_failed;
            switch (param.m_op)
            {
                case 0: //圣兽场
                    res = user.doQuery(param, QueryType.queryTypeStatMythicalBossPoint);
                    genTable0(m_result, res, user, param);
                    break;
                case 1: //巨鲨场
                    res = user.doQuery(param, QueryType.queryTypeStatSharkRoomBombPoint);
                    genTable1(m_result, res, user, param);
                    break;
                case 2: //聚宝鱼
                    res = user.doQuery(param, QueryType.queryTypeStatTreasureBowlPoint);
                    genTable2(m_result, res, user, param);
                    break;
                case 3: //魔鲲场
                    res = user.doQuery(param, QueryType.queryTypeStatLegendaryFishRoomPoint);
                    genTable3(m_result, res, user, param);
                    break;
            }
        }

        //圣兽场
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

            List<StatMythicalBossPointItem> qresult =
                (List<StatMythicalBossPointItem>)user.getQueryResult(QueryType.queryTypeStatMythicalBossPoint);

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

                StatMythicalBossPointItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.getBossName();
                m_content[f++] = item.m_turretRate.ToString();
                m_content[f++] = item.m_bossRate.ToString();
                m_content[f++] = item.m_points.ToString();

                for (j = 0; j < s_head1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatRoomPoint.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        //巨鲨场
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

            List<StatSharkRoomBombPointItem> qresult =
                (List<StatSharkRoomBombPointItem>)user.getQueryResult(QueryType.queryTypeStatSharkRoomBombPoint);

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
                StatSharkRoomBombPointItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_topPoint.ToString();

                for (j = 0; j < s_head2.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatRoomPoint.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        //聚宝鱼统计
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

            List<StatTreasureBowlItem> qresult =
                (List<StatTreasureBowlItem>)user.getQueryResult(QueryType.queryTypeStatTreasureBowlPoint);

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
                StatTreasureBowlItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_bulletRate.ToString();
                m_content[f++] = item.m_gold.ToString();
                m_content[f++] = item.m_index.ToString(); //额外奖励

                for (j = 0; j < s_head3.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatRoomPoint.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        //魔鲲场统计
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

            List<StatLegendaryFishRoomPointItem> qresult =
                (List<StatLegendaryFishRoomPointItem>)user.getQueryResult(QueryType.queryTypeStatLegendaryFishRoomPoint);

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
                StatLegendaryFishRoomPointItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_bulletRate.ToString();
                m_content[f++] = item.getFishName();
                m_content[f++] = item.m_gold.ToString();
                m_content[f++] = item.m_coin.ToString();

                for (j = 0; j < s_head4.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatRoomPoint.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}