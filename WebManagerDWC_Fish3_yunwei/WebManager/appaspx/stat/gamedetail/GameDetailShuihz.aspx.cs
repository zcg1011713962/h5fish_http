using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat.gamedetail
{
    public partial class GameDetailShuihz : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "中奖结果", "倍率", "得奖" };
        private string[] m_content = new string[s_head.Length];
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);
            string indexStr = Request.QueryString["index"];
            if (string.IsNullOrEmpty(indexStr))
                return;

            int index = 0;
            if (!int.TryParse(indexStr, out index))
            {
                return;
            }

            GMUser user = (GMUser)Session["user"];
            GameDetailInfo ginfo = GameDetail.parseGameInfo(GameId.shuihz, index, user);
            genInfoPanel(ginfo);
        }

         private void genInfoPanel(GameDetailInfo ginfo)
        {
            if (ginfo == null)
                return;

            MoneyItemDetail item = ginfo.m_item;
            InfoShuihz info = (InfoShuihz)ginfo.m_detailInfo;
            divHead.InnerText = item.m_genTime;
            // 玩家ID
            tdPlayer.InnerText = "玩家ID:" + item.m_playerId.ToString();
            tdIsBonusGame.InnerText = info.isBonusGame == true ? "是" : "否";

            if (info.isBonusGame)
            {
                tdInnerIcon.InnerText = "内圈";
                tdOuterIcon.InnerText = "外圈";
                genCardInfo(div1, info, 0);
                genCardInfo(div2, info, 1);
                tdBet.Attributes.CssStyle.Value = "display:none";
            }
            else 
            {
                BonusGametrue.Attributes.CssStyle.Value = "display:none";
                tdTitle.Attributes.CssStyle.Value = "display:none";
                tdBetMoney.InnerText = info.bet;
                genCardInfo(divResult1, info, 0);
                genCardInfo(divResult2, info, 1);
                genCardInfo(divResult3, info, 2);
            }
            genBetTable(tableBet, info, item); 
        }

        private void genCardInfo(System.Web.UI.HtmlControls.HtmlGenericControl div, InfoShuihz info, int row)
        {
            int len = 5;
            if (info.isBonusGame)
            {
                len = 4;
            }
            for (int i = 0; i < len; i++)
            {
                Image img = new Image();
                if(len==4)
                {
                    img.Attributes.CssStyle.Value = "width:100px;height:100px;";
                }
                img.ImageUrl = "/data/image/shuihz/" + "head_image0" + info.getResult(row, i) + ".png";
                div.Controls.Add(img);
                if(info.isBonusGame && row==1)
                {
                    break;
                }
            }
        }

        //下注表
        protected void genBetTable(Table table, InfoShuihz info, MoneyItemDetail item)
        {
            GMUser user = (GMUser)Session["user"];

            TableRow tr = new TableRow();
            table.Rows.Add(tr);

            int i = 0;
            for (; i < s_head.Length; i++)
            {
                TableCell td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            if (info.isBonusGame)
            {
                int f = 0;
                m_content[f++] = "下注倍率";
                m_content[f++] = info.rate;
                m_content[f++] = info.winMoney;

                tr = new TableRow();
                table.Rows.Add(tr);
                for (int j = 0; j < s_head.Length; j++)
                {
                    TableCell td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            else 
            {
                for (i = 0; i < StrName.s_shuihzArea.Length; i++)
                {
                    int c = 0;
                    m_content[c++] = StrName.s_shuihzArea[i];
                    if (i == 0)
                    {
                        m_content[c++] = info.rate;
                        m_content[c++] = info.winMoney;
                    }
                    else 
                    {
                        m_content[c++] = info.bonusCount;
                        m_content[c++] = "";
                    }
                    
                    tr = new TableRow();
                    table.Rows.Add(tr);
                    for (int j = 0; j < s_head.Length; j++)
                    {
                        TableCell td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = m_content[j];
                    }
                }
            }
        }

    }
}