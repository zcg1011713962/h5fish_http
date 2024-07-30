<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ShuihzPlayerScorePool.aspx.cs" Inherits="WebManager.appaspx.stat.shuihz.ShuihzPlayerScorePool" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../../Scripts/stat/ShuihzPlayerScorePool.js?ver=3" type="text/javascript"></script>
    <style type="text/css">
        #buffPlayerList{ border-collapse:collapse;border:1px solid #000;}
        #buffPlayerList td{ border:1px solid #000;text-align:center;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="cSafeWidth">
        <h2 style="text-align:center;padding:20px;">水浒传点杀点送</h2>
        <table border="0"  class="inputForm">
            <tr>
        		<td class="tdLeft">玩家ID：</td>
                <td>
                    <asp:TextBox ID="m_playerId" runat="server" style="width:250px;height:30px;"></asp:TextBox>
                </td>
        	</tr>
        	<tr>
        		<td class="tdLeft">添加BUFF水池：</td>
                <td>
                    <asp:TextBox ID="m_addPoolVal" runat="server" style="width:250px;height:30px;"></asp:TextBox>
                </td>
        	</tr>
            <tr>
                <td>&ensp;</td>
        		<td> 
                    <asp:Button ID="Button1" runat="server" Text="确定" onclick="onBuffScorePool" style="width:150px;height:30px;"/>
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
        <table id="buffPlayerList" class="inputForm"></table>
    </div>
</asp:Content>
