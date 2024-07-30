<%@ Page Language="C#" CodeBehind="GameDetailFruit.aspx.cs" Inherits="WebManager.appaspx.stat.gamedetail.GameDetailFruit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>水果机详情</title>
    <link href="../../../style/game_detail.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="cFrame">
        <table>
            <tr>
                <td colspan="2" id="divHead" runat="server"></td>
            </tr>
            <tr>
                <td id="tdPlayerFruit" colspan="2" runat="server"></td>
            </tr>
            <tr class="cTrAve">
                <td>转灯结果</td>
                <td>
                    <div id="divNormalResult" runat="server"></div>
                </td>
            </tr>
            <tr class="cTrAve">
                <td>射灯</td>
                <td style="min-width:520px">
                    <div id="divSpotLightResult" runat="server"></div>
                </td>
            </tr>
            <tr class="cTrAve">
                <td>三七机/联机大奖</td>
                <td id="tdAllPrizesResult" runat="server"></td>
            </tr>
        </table>
        <asp:Table ID="tableBet" runat="server"></asp:Table>
    </div>
    </form>
</body>
</html>

