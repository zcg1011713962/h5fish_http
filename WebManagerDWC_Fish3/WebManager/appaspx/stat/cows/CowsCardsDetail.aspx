<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CowsCardsDetail.aspx.cs" Inherits="WebManager.appaspx.stat.cows.CowsCardsDetail" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>牛牛牌局详情</title>
    <link href="../../../style/game_detail.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="cFrame" style="min-width:850px;">
        <table>
            <tr>
                <td colspan="3" id="divHead" runat="server">
                    <h3>牌局详情</h3>
                </td>
            </tr>
            <tr>
                <td id="tdPlayer" runat="server"></td>
                <td id="tdIsBanker" runat="server"> </td>
                <td id="tdPumpMoney" runat="server"></td>
            </tr>
            <tr>
                <td style="min-width:350px;">牌型</td>
                <td>结果</td>
                <td style="min-width:200px;">输赢金币总计</td>
            </tr>
            <tr>
                <td>
                    <h3>庄家牌型</h3>
                    <div id="divBankerCard" runat="server"></div>
                </td>
                <td id="tdBankerCardType" runat="server">
                </td>
                <td id="tdBankerCardLoseWin" runat="server">
                </td>
            </tr>
            <tr>
                <td>
                    <h3>东牌型</h3>
                    <div id="divEastCard" runat="server"></div>
                </td>
                <td id="tdEastCardType" runat="server">
                </td>
                <td id="tdEastCardLoseWin" runat="server">
                </td>
            </tr>
            <tr>
                <td>
                    <h3>南牌型</h3>
                    <div id="divSouthCard" runat="server"></div>
                </td>
                <td id="tdSouthCardType" runat="server"></td>
                <td id="tdSouthCardLoseWin" runat="server"></td>
            </tr>
            <tr>
                <td>
                    <h3>西牌型</h3>
                    <div id="divWestCard" runat="server"></div>
                </td>
                <td id="tdWestCardType" runat="server"></td>
                <td id="tdWestCardLoseWin" runat="server"></td>
            </tr>
            <tr>
                <td>
                    <h3>北牌型</h3>
                    <div id="divNorthCard" runat="server"></div>
                </td>
                <td id="tdNorthCardType" runat="server"></td>
                <td id="tdNorthCardLoseWin" runat="server"></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

