﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="OperationWishCurse.aspx.cs" Inherits="WebManager.appaspx.operation.OperationWishCurse" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../Scripts/operation/OperationWishCurse.js" type="text/javascript"></script>
    <style type="text/css">
        #bufferPlayerList{ border-collapse:collapse;border:1px solid #000;}
        #bufferPlayerList td{ border:1px solid #000;text-align:center;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth">
        <h2 style="text-align:center;"> GM祝福诅咒</h2>
        <table border="0" cellspacing="0" cellpadding="0" class="inputForm" width="50%">
            <tr>
        		<td class="tdLeft">操作类型:</td>
                <td>
                    <asp:DropDownList ID="m_opType" runat="server" style="width:150px;"></asp:DropDownList>
                </td>
        	</tr>
        	<tr>
        		<td class="tdLeft">祝福or诅咒:</td>
                <td>
                    <asp:DropDownList ID="m_type" runat="server" style="width:150px;"></asp:DropDownList>
                </td>
        	</tr>
            <tr>
        		<td class="tdLeft">玩家ID:</td>
                <td>
                    <asp:TextBox ID="m_playerId" runat="server"></asp:TextBox>
                </td>
        	</tr>
            <tr class="tdAlt">
        		<td class="tdLeft">升降命中率:</td>
                <td>
                    <asp:TextBox ID="m_rate" runat="server"></asp:TextBox>
                </td>
        	</tr>
            <%-- <tr>
        		<td class="tdLeft">所在游戏:</td>
                <td>
                    <asp:DropDownList ID="m_game" runat="server" style="width:150px;"></asp:DropDownList>
                </td>
        	</tr>--%>
            <tr>
        		<td colspan="2" class="tdButton">
                    <asp:Button ID="Button1" runat="server" Text="确定" onclick="onAddWishCurse"/>
        		</td>
        	</tr>
            <tr>
        		<td colspan="2" style="text-align:center;">
                    <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        		</td>
        	</tr>
            <tr>
                <td><input type="button" value="刷新列表" id="btnRefresh"/></td>
            </tr>
        </table>

        <table id="bufferPlayerList" width="50%" class="inputForm"></table>
    </div>
</asp:Content>
