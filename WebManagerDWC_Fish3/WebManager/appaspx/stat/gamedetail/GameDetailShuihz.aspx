<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GameDetailShuihz.aspx.cs" Inherits="WebManager.appaspx.stat.gamedetail.GameDetailShuihz" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>水浒传详情</title>
    <link href="../../../style/game_detail.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <div class="cFrame" style="width:850px;">
        <table>
            <tr>
                <td colspan="2" id="divHead" runat="server"></td>
            </tr>
            <tr>
                <td id="tdPlayer" colspan="2" runat="server"></td>
            </tr>
            <tr>
                <td colspan="2">开牌结果</td>
            </tr>
            <tr class="cTrAve">
                <td>是否为Bonus game</td>
                <td runat="server" id="tdIsBonusGame"></td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="cDragResult">
                        <div id="divResult1" runat="server"></div>
                        <div id="divResult2" runat="server"></div>
                        <div id="divResult3" runat="server"></div>
                    </div>
                </td>
            </tr>
            <tr id="tdTitle" runat="server">
                <td runat="server" id="tdInnerIcon"></td>
                <td runat="server" id="tdOuterIcon"></td>
            </tr>
            <tr id="BonusGametrue" runat="server">
                <td >
                    <div class="cDragResult">
                        <div id="div1" runat="server"></div>
                    </div>
                </td>
                <td>
                    <div class="cDragResult">
                        <div id="div2" runat="server"></div>
                    </div>
                </td>
            </tr>
            <tr id="tdBet" runat="server">
                <td>押注金额</td>
                <td runat="server" id="tdBetMoney"></td>
            </tr>
        </table>
        <asp:Table ID="tableBet" runat="server"></asp:Table>
    </div>
</body>
</html>

