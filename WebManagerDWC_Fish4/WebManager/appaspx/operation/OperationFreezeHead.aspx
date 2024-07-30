<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="OperationFreezeHead.aspx.cs" Inherits="WebManager.appaspx.operation.OperationFreezeHead" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth">
        <h2 style="text-align:center;padding:20px;">冻结头像</h2>
        <table border="0" cellspacing="0" cellpadding="0" width="70%" class="inputForm">
        	<tr>
        		<td class="tdLeft">玩家ID:</td>
                <td><asp:TextBox ID="m_playerId" runat="server" style="width:180px;height:30px"></asp:TextBox></td>
        	</tr>
            <tr>
        		<td class="tdLeft">冻结天数(默认7天):</td>
                <td><asp:TextBox ID="m_freezeDays" runat="server" style="width:180px;height:30px"></asp:TextBox></td>
        	</tr>
            <tr>
        		<td colspan="2" class="tdButton">
                    <asp:Button ID="Button1" runat="server" onclick="onViewHead" Text="查看头像" style="width:100px;height:30px" />
                    <asp:Button ID="Button3" runat="server" onclick="onFreeze" Text="冻结" style="width:100px;height:30px" />
                    <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        		</td>
        	</tr>
            <tr>
        		<td colspan="2" style="text-align:center;padding:5px;">
                    <asp:Image ID="m_headImg" runat="server" />
        		</td>
        	</tr>
        </table>
    </div>
</asp:Content>
