<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShcdCardsDetail.aspx.cs" Inherits="WebManager.appaspx.stat.shcd.ShcdCardsDetail" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>黑红梅方牌局详情</title>
    <link href="../../../style/game_detail.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="cFrame" style="min-width:950px;">
        <table>
            <tr>
                <td colspan="11" id="divHead" runat="server">
                    <h3>牌局详情</h3>
                </td>
            </tr>
            <tr>
                <td id="tdCardId" runat="server" colspan="11"></td>
            </tr>
            <tr>
                <td rowspan="2" style="min-width:100px;">开牌结果</td>
                <td colspan="2" style="min-width:140px;">黑桃下注</td>
                <td colspan="2" style="min-width:140px;">红心下注</td>
                <td colspan="2" style="min-width:140px;">梅花下注</td>
                <td colspan="2" style="min-width:140px;">方块下注</td>
                <td colspan="2" style="min-width:140px;">大小王下注</td>
            </tr>
            <tr>
                <td>机器人</td>
                <td>玩家</td>
                <td>机器人</td>
                <td>玩家</td>
                <td>机器人</td>
                <td>玩家</td>
                <td>机器人</td>
                <td>玩家</td>
                <td>机器人</td>
                <td>玩家</td>
            </tr>
            <tr>
                <td id="tdCardResult" runat="server"></td>
                <td id="tdSpadeRobot" runat="server"></td>
                <td id="tdSpadePlayer" runat="server"></td>
                <td id="tdHeartRobot" runat="server"></td>
                <td id="tdHeartPlayer" runat="server"></td>
                <td id="tdClubRobot" runat="server"></td>
                <td id="tdCluPlayer" runat="server"></td>
                <td id="tdDiamondRobot" runat="server"></td>
                <td id="tdDiamondPlayer" runat="server"></td>
                <td id="tdJokerRobot" runat="server"></td>
                <td id="tdJokerPlayer" runat="server"></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
