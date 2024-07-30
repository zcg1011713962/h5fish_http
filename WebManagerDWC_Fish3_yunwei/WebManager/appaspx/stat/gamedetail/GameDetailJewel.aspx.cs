using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat.gamedetail
{
    public partial class GameDetailJewel : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "第n次", "类型", "m连","倍率","得分"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);
            string indexStr = Request.QueryString["index"];
            if (string.IsNullOrEmpty(indexStr))
                return;

            int index = 0;
            if (!int.TryParse(indexStr, out index))
                return;

            GMUser user = (GMUser)Session["user"];
            GameDetailInfo ginfo = GameDetail.parseGameInfo(GameId.jewel, index, user);
            genInfoPanel(ginfo);
        }

        private void genInfoPanel(GameDetailInfo ginfo)
        {
            if (ginfo == null)
                return;

            MoneyItemDetail item = ginfo.m_item;
            InfoJewel info = (InfoJewel)ginfo.m_detailInfo;
            divHead.InnerText = item.m_genTime;
            // 玩家ID
            tdPlayer.InnerText = "玩家ID：" + item.m_playerId.ToString();
            tdBetMoney.InnerText = info.m_bet;

            genBetTable(tableBet, info);
        }

        // 下注表
        protected void genBetTable(Table table, InfoJewel info)
        {
            GMUser user = (GMUser)Session["user"];
            int i = 0;

            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            for (; i < s_head.Length; i++)
            {
                TableCell td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.ColumnSpan = 1;
            }

            if (info.m_resInfo == null)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                TableCell td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "0消除";
                td.ColumnSpan = 5;
            }
            else 
            {
                long totalScore = 0;
                for (i = 0; i < info.m_resInfo.Count; i++)
                {
                    ResJewel item = info.m_resInfo[i];
                    m_content[0] = (i + 1).ToString();
                    m_content[1] = item.getKindName(item.kind);

                    m_content[2] = item.count.ToString();
                    m_content[3] = item.mult.ToString();
                    m_content[4] = item.score.ToString();

                    totalScore += item.score;

                    tr = new TableRow();
                    table.Rows.Add(tr);
                    for (int j = 0; j < s_head.Length; j++)
                    {
                        TableCell td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = m_content[j];
                    }
                }
                addStatFoot(table, totalScore);
            }
        }

        // 增加统计页脚
        protected void addStatFoot(Table table, long totalScore)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            m_content[0] = "总计";
            m_content[1] = "";
            m_content[2] = "";
            m_content[3] = "";
            m_content[4] = totalScore.ToString();

            for (int j = 0; j < s_head.Length; j++)
            {
                TableCell td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        }

    }
}