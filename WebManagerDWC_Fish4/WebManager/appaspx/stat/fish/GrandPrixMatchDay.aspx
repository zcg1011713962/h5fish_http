<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="GrandPrixMatchDay.aspx.cs" Inherits="WebManager.appaspx.stat.fish.GrandPrixMatchDay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../../../Scripts/datepicker/daterangepicker.js"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#MainContent_m_time').daterangepicker();
	    });
	</script>
    <style type="text/css">
        td{padding:2px;}
        td input[type=submit]{width:100px;height:30px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth">
        <h2 style="text-align:center;padding:20px;">大奖赛名次</h2>
        <table style="margin:0 auto;">
            <tr>
                <td>时间：</td>
                <td colspan="2"><asp:TextBox ID="m_time" runat="server" CssClass="cTextBox" style="height:30px;"></asp:TextBox></td>
            </tr>
            <tr>
                <td>玩家ID：</td>           
                <td colspan="2"><asp:TextBox ID="m_playerId" runat="server" CssClass="cTextBox" style="height:30px;"></asp:TextBox></td>
            </tr>
            <tr>
               <td></td>
                <td>
                    <asp:Button ID="btnQuery" runat="server" Text="查询" CssClass="cButton" OnClick="onQueryPlayer" style="height:30px;"/>
                </td>
                <td>
                    <asp:Button ID="btnQueryTop100" runat="server" Text="查询前100名" CssClass="cButton" OnClick="onQueryTop100" style="height:30px;"/>
                </td>
            </tr>
        </table>

        <asp:Table ID="m_result" runat="server" CssClass="cTable"></asp:Table>
    </div>
</asp:Content>
