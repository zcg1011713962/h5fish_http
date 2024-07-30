using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatTreasureHunt : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "时间","玩家ID","场次","参与次数",
            "门票青铜鱼雷","门票白银鱼雷","门票黄金鱼雷","门票钻石鱼雷","门票钻石",
            "金币","青铜鱼雷","白银鱼雷","黄金鱼雷","钻石鱼雷",
            "青铜碎片","白银碎片","黄金碎片","钻石碎片",
            "总计折合金币","每场平均折合金币","详情"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_TREASURE_HUNT, Session, Response);
            if (!IsPostBack)
            {
                m_roomList.Items.Add("新手引导");
                m_roomList.Items.Add("青铜");
                m_roomList.Items.Add("白银");
                m_roomList.Items.Add("黄金");
                m_roomList.Items.Add("钻石");
            }
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_param = m_playerList.Text;
            param.m_op = m_roomList.SelectedIndex;
            param.m_time = m_time.Text;
            OpRes res = user.doQuery(param, QueryType.queryTypeStatTreasureHunt);
            genTable(m_resTable, res, user,param.m_param);
        }

        //生成表
        private void genTable(Table table, OpRes res, GMUser user, string playerList)
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

            List<statTreasureHuntItem> qresult = (List<statTreasureHuntItem>)user.getQueryResult(QueryType.queryTypeStatTreasureHunt);

            int i = 0, j = 0, f = 0;
            // 表头 第一行
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.ColumnSpan = 1;
                td.RowSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                table.Rows.Add(tr);

                statTreasureHuntItem item = qresult[i];
                m_content[f++] = i == 0 ? "总计" : item.m_time; //第一行，总计
                m_content[f++] = item.m_playerId == 0 ? "" : item.m_playerId.ToString();
                m_content[f++] = item.getRoomName();
                m_content[f++] = item.m_joinCount.ToString();
                m_content[f++] = item.m_cost24.ToString();
                m_content[f++] = item.m_cost25.ToString();
                m_content[f++] = item.m_cost26.ToString();
                m_content[f++] = item.m_cost27.ToString();
                m_content[f++] = item.m_cost2.ToString();
                m_content[f++] = item.m_item1.ToString();
                m_content[f++] = item.m_item24.ToString();
                m_content[f++] = item.m_item25.ToString();
                m_content[f++] = item.m_item26.ToString();
                m_content[f++] = item.m_item27.ToString();
                m_content[f++] = item.m_item43.ToString();
                m_content[f++] = item.m_item44.ToString();
                m_content[f++] = item.m_item45.ToString();
                m_content[f++] = item.m_item46.ToString();
                m_content[f++] = item.getTotalGold().ToString();
                m_content[f++] = item.m_joinCount == 0 ? item.getTotalGold().ToString() : (item.getTotalGold() / item.m_joinCount).ToString();
                m_content[f++] = i == 0 ? "" : item.getDetail();
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }
    }
}