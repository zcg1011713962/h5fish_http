<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatPlayerGameBet.aspx.cs" Inherits="WebManager.appaspx.stat.StatPlayerGameBet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script src="../../Scripts/module/sea.js" type="text/javascript"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#txtTime').daterangepicker();
	    });
	    seajs.use("../../Scripts/stat/PlayerGameBet.js");
    </script>
    <style type="text/css">
         .cTable tr:hover td{background:#ddd;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth">
        <h2 style="text-align:center;padding:20px;">用户下注情况统计</h2>
        <table class="SearchCondition" style="margin:0 auto">
            <tr>
                <td>游戏:</td>
                <td><asp:DropDownList ID="m_gameList" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>玩家ID:</td>
                <td><input type="text" id="txtPlayerId"/></td>
            </tr>
            <tr>
                <td>时间:</td>
                <td><input type="text" id="txtTime"/></td>
                <td><input type="button" value="查询" id="btnQuery"/></td>
            </tr>
        </table>
    </div>

    <table class="cTable" id="resultTable">

    </table>
</asp:Content>
