<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="CowsPlayerBanker.aspx.cs" Inherits="WebManager.appaspx.stat.cows.CowsPlayerBanker" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../../../Scripts/datepicker/daterangepicker.js"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#MainContent_txtTime').daterangepicker();
	    });
	</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth" style="text-align:center">
        <h2 style="padding:20px;">牛牛上庄查询</h2>
        玩家ID：<asp:TextBox ID="txtPlayerId" runat="server" CssClass="cTextBox" style="height:30px;"></asp:TextBox>
        &ensp;时间：<asp:TextBox ID="txtTime" runat="server" CssClass="cTextBox" style="height:30px;"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="查询" onclick="onQuery" CssClass="cButton"/>

        <asp:Table ID="m_result" runat="server" CssClass="cTable">
        </asp:Table>

        <br />
        <span id="m_page" style="text-align:center;display:block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
        </div>
</asp:Content>
