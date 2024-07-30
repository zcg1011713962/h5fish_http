<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ServiceChannelOpenCloseGame.aspx.cs" Inherits="WebManager.appaspx.service.ServiceChannelOpenCloseGame" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../../Scripts/service/ServiceChannelOpenCloseGame.js" type="text/javascript"></script>
    <style type="text/css">
        #buffPlayerList{ border-collapse:collapse;border:1px solid #000;}
        #buffPlayerList td{ border:1px solid #000;text-align:center;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth">
        <h2 style="text-align:center;padding:20px;">小游戏开关设置</h2>
        <table  cellspacing="0" cellpadding="0" class="inputForm" width="50%">
        	<tr>
        		<td class="tdLeft">渠道：</td>
                <td>
                    <asp:DropDownList ID="m_channel" runat="server" style="width:250px;height:30px;"></asp:DropDownList>
                </td>
        	</tr>
            <tr>
        		<td class="tdLeft">开关：</td>
                <td>
                    <asp:DropDownList ID="m_openClose" runat="server" style="width:250px;height:30px;"></asp:DropDownList>
                </td>
        	</tr>
            <tr>
        		<td class="tdLeft"><h3>开启要求：</h3></td>
                <td></td>
        	</tr>
            <tr>
        		<td class="tdLeft">炮倍率：</td>
                <td>
                    <asp:DropDownList ID="m_gameLevel" runat="server" style="width:250px;height:30px;"></asp:DropDownList>
                </td>
        	</tr>
            <tr>
        		<td class="tdLeft">VIP等级：</td>
                <td>
                    <asp:DropDownList ID="m_vipLevel" runat="server" style="width:250px;height:30px;"></asp:DropDownList>
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
                <td><input type="button" value="刷新列表" id="btnRefresh" style="width:150px;height:30px;" /></td>
                <td> 
                    <asp:Button ID="Button2" runat="server" Text="刷新服务器加载数据" onclick="onReloadData" style="width:150px;height:30px;"/>
        		    <span id="m_ReloadRes" style="font-size:medium;color:red" runat="server">操作失败</span>
                </td>
            </tr>
        </table>
        <table id="channelOpenCloseGameList" width="50%" class="inputForm"></table>
    </div>
</asp:Content>
