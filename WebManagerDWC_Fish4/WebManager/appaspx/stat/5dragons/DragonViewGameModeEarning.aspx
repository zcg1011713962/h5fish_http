<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DragonViewGameModeEarning.aspx.cs" Inherits="WebManager.appaspx.stat._5dragons.DragonViewGameModeEarning" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        td.left{width:100px;}
        table.tableForm td{padding-top:5px; padding-bottom:5px;}
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth" style="text-align:center">
        <h2 style="padding:20px;">五龙具体盈利情况</h2>
        <table style="margin:0 auto" class="tableForm">
        	<tr>
        		<td>房间：</td>
                <td>
                    <asp:DropDownList ID="m_room" runat="server" CssClass="cDropDownList" style="width:200px;"></asp:DropDownList>
                </td>
        	</tr>
            <tr>
        		<td>桌子：</td>
                <td>
                    <asp:TextBox ID="m_desk" runat="server" style="width:200px;height:30px;"></asp:TextBox>
                </td>
        	</tr>
            <tr>
                <td>&ensp;</td>
        		<td>
                    <asp:Button ID="Button1" runat="server" Text="查看" OnClick="onViewGameMode" style="width:120px;height:30px;"/>
        		</td>
        	</tr>
        </table>
        <asp:Table ID="m_gameMode" runat="server" CssClass="cTable">
        </asp:Table>
    </div>
</asp:Content>
