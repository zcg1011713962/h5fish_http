<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="GameDetailJewel.aspx.cs" Inherits="WebManager.appaspx.stat.gamedetail.GameDetailJewel" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>宝石迷阵详情</title>
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
            
            <tr id="tdBet" runat="server">
                <td>押注金额</td>
                <td runat="server" id="tdBetMoney"></td>
            </tr>
        </table>
        <asp:Table ID="tableBet" runat="server"></asp:Table>
    </div>
</body>
</html>

