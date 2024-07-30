using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat.shcd
{
    public partial class ShcdCardsDetail : System.Web.UI.Page
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
            OpRes res = user.doQuery(param, QueryType.queryTypeShcdCardsDetail);
            genInfoPanel(res, user, param);
        }
        private void genInfoPanel(OpRes res, GMUser user, ParamQuery param)
        {
            List<ShcdCardsDetailItem> info = (List<ShcdCardsDetailItem>)user.getQueryResult(QueryType.queryTypeShcdCardsDetail);

            for (int i = 0; i < info.Count; i++)
            {
                ShcdCardsDetailItem item = (ShcdCardsDetailItem)info[i];
                // 牌局ID
                tdCardId.InnerText = "牌局ID：" + param.m_param;

                // 牌局结果
                tdCardResult.InnerText = DefCC.s_pokerColorShcd[item.m_cardType];

                //黑红梅方大小王下注情况
                tdSpadeRobot.InnerText = string.Format("{0:N0}",item.m_area0[0]);
                tdSpadePlayer.InnerText=string.Format("{0:N0}",item.m_area0[1]);

                tdHeartRobot.InnerText=string.Format("{0:N0}",item.m_area1[0]);
                tdHeartPlayer.InnerText=string.Format("{0:N0}",item.m_area1[1]);

                tdClubRobot.InnerText=string.Format("{0:N0}",item.m_area2[0]);
                tdCluPlayer.InnerText=string.Format("{0:N0}",item.m_area2[1]);

                tdDiamondRobot.InnerText=string.Format("{0:N0}",item.m_area3[0]);
                tdDiamondPlayer.InnerText=string.Format("{0:N0}",item.m_area3[1]);

                tdJokerRobot.InnerText=string.Format("{0:N0}",item.m_area4[0]);
                tdJokerPlayer.InnerText=string.Format("{0:N0}",item.m_area4[1]);
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