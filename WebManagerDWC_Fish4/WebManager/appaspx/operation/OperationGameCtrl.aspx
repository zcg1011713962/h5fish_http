<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OperationGameCtrl.aspx.cs" Inherits="WebManager.appaspx.operation.OperationGameCtrl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../../Scripts/operation/OperationGameCtrl.js" type="text/javascript"></script>
    <style type="text/css">
        #channelVersionList{ border-collapse:collapse;border:1px solid #000;width:800px;}
        #channelVersionList td{ border:1px solid #000;text-align:center;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth">
        <h2 style="text-align:center;padding:20px;">游戏控制</h2>
        <table border="0"  class="inputForm">
            <tr>
        		<td class="tdLeft">渠道号：</td>
                <td>
                    <asp:TextBox ID="m_channel" runat="server" style="width:250px;height:30px;"></asp:TextBox>
                </td>
        	</tr>
        	<tr>
        		<td class="tdLeft">版本号：</td>
                <td>
                    <asp:TextBox ID="m_version" runat="server" style="width:250px;height:30px;"></asp:TextBox>
                </td>
        	</tr>
            <tr>
                <td>&ensp;</td>
        		<td> 
                    <asp:Button ID="Button1" runat="server" Text="确定" onclick="onConfirm" style="width:150px;height:30px;"/>
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
        <br />
        <br />
        <table id="channelVersionList" class="inputForm"></table>
    </div>
        
</asp:Content>
