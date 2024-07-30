using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat.cows
{
    public partial class CowsCardsDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);
            string indexStr = Request.QueryString["cardId"];
            if (string.IsNullOrEmpty(indexStr))
                return;

            long index = 0;
            if (!long.TryParse(indexStr, out index))
            {
                return;
            }

            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_param = Request.QueryString["cardId"];
            OpRes res = user.doQuery(param, QueryType.queryTypeCowsCardsDetail);
            genInfoPanel(res,user,param);
        }

        private void genInfoPanel(OpRes res, GMUser user,ParamQuery param)
        {
            List<InfoCows> info = (List<InfoCows>)user.getQueryResult(QueryType.queryTypeCowsCardsDetail);

            for (int i = 0; i < info.Count; i++)
            {
                InfoCows item = (InfoCows)info[i];
                // 牌局ID
                tdPlayer.InnerText = "牌局ID：" + param.m_param;
                // 玩家是否上庄
                tdIsBanker.InnerText = "庄家："+item.m_bankerId;

                //庄家抽水
                tdPumpMoney.InnerText = "庄家抽水："+item.m_pumpBankerTotal.ToString();

                // 庄家牌型
                genCardInfo(divBankerCard, tdBankerCardType, tdBankerCardLoseWin, item.m_bankerCard);
                // 东牌型
                genCardInfo(divEastCard, tdEastCardType, tdEastCardLoseWin, item.m_eastCard);
                // 南牌型
                genCardInfo(divSouthCard, tdSouthCardType, tdSouthCardLoseWin, item.m_southCard);
                // 西牌型
                genCardInfo(divWestCard, tdWestCardType, tdWestCardLoseWin, item.m_westCard);
                // 北牌型
                genCardInfo(divNorthCard, tdNorthCardType, tdNorthCardLoseWin, item.m_northCard);
            }
        }

        private void genCardInfo(System.Web.UI.HtmlControls.HtmlGenericControl div,
                                 System.Web.UI.HtmlControls.HtmlTableCell cell, System.Web.UI.HtmlControls.HtmlTableCell cell1,
                                 CowsCard cards)
        {
            if (cards == null)
                return;

            Cows_CardsCFGData d = Cows_CardsCFG.getInstance().getValue(cards.m_cardType);
            if (d != null)
            {
                cell.InnerText = d.m_cardName;
            }

            cell1.InnerText = cards.m_cardWinLose.ToString();

            foreach (var card in cards.m_cards)
            {
                Image img = new Image();
                img.ImageUrl = "/data/image/poker/" + DefCC.s_pokerCows[card.flower] + "_" + card.point + ".png";
                div.Controls.Add(img);
            }
        }

    }
}