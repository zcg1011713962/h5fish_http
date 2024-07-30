<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OperationGameCtrl.aspx.cs" Inherits="WebManager.appaspx.operation.OperationGameCtrl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../../Scripts/operation/OperationGameCtrl.js" type="text/javascript"></script>
    <style type="text/css">
        #channelVersionList{ border-collapse:collapse;border:1px solid #000;width:800px;}
        #channelVersionList td{ border:1px solid #000;text-align:center;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div class="cSafeWidth" style="width:1200px;background-color:#eee;padding:15px;">
        <table border="0" class="inputForm" style="width:350px; float:left">
            <tr>
                <td colspan="2">
                    <h2 style="text-align:center;padding:15px">游戏控制</h2>
                </td>
            </tr>
            <tr>
        		<td class="tdLeft">渠道号：</td>
                <td>
                    <asp:DropDownList ID="m_channel" runat="server" style="width:250px;height:30px;"></asp:DropDownList>
                </td>
        	</tr>
        	<tr>
        		<td class="tdLeft">版本号：</td>
                <td>
                    <asp:TextBox ID="m_version" runat="server" style="width:250px;height:30px;"></asp:TextBox>
                </td>
        	</tr>
            <tr>
        		<td class="tdLeft">炮倍率：</td>
                <td>
                    <asp:DropDownList ID="m_turret" runat="server" style="width:250px;height:30px;"></asp:DropDownList>
                </td>
        	</tr>
            <tr>
        		<td class="tdLeft">VIP等级：</td>
                <td>
                    <asp:DropDownList ID="m_viplv" runat="server" style="width:250px;height:30px;"></asp:DropDownList>
                </td>
        	</tr>
             <tr>
        		<td class="tdLeft">功能开关：</td>
                <td>
                    <asp:CheckBoxList ID="m_onOff" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal" 
                        style="width:250px;height:30px;"></asp:CheckBoxList>
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
                <td colspan="2"><hr/></td>
            </tr>
            <tr>
                <td></td>
                <td><input type="button" value="刷新列表" id="btnRefresh" style="width:150px;height:30px;"/></td>
            </tr>
        </table>
        <table id="channelVersionList" class="inputForm" style="margin-left:20px; float:left"></table>
        <div style="clear:both"></div>
    </div>
    <br />
        
</asp:Content>
