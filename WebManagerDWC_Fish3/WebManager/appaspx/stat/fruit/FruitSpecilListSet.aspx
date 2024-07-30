<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FruitSpecilListSet.aspx.cs" Inherits="WebManager.appaspx.stat.fruit.FruitSpecilListSet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <script src="../../../Scripts/stat/FruitSpecilListSet.js?ver=2" type="text/javascript"></script>
    <style type="text/css">
        #buffPlayerList{ border-collapse:collapse;border:1px solid #000;}
        #buffPlayerList td{ border:1px solid #000;text-align:center;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth">
        <h2 style="text-align:center;padding:20px;">水果机拉入黑白名单设置</h2>
        <table  cellspacing="0" cellpadding="0" class="inputForm" width="50%">
            <tr>
        		<td class="tdLeft">玩家ID：</td>
                <td>
                    <asp:TextBox ID="m_playerId" runat="server" style="width:250px;height:30px;"></asp:TextBox>
                </td>
        	</tr>
        	<tr>
        		<td class="tdLeft">操作：</td>
                <td>
                    <asp:DropDownList ID="m_setType" runat="server" style="width:250px;height:30px;"></asp:DropDownList>
                </td>
        	</tr>
            <tr>
                <td>&ensp;</td>
        		<td> 
                    <asp:Button ID="Button1" runat="server" Text="确定" onclick="onSetSpecilList" style="width:250px;height:30px;"/>
        		</td>
        	</tr>
            <tr>
        		<td colspan="2" style="text-align:center;">
                    <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        		</td>
        	</tr>
            <tr>
                <td><input type="button" value="刷新列表" id="btnRefresh" /></td>
            </tr>
        </table>
        <table id="playerSpecilList" width="50%" class="inputForm"></table>
    </div>
</asp:Content>
