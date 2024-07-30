<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="TdGameTimeDistribution.aspx.cs" Inherits="WebManager.appaspx.td.TdGameTimeDistribution" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script src="../../Scripts/stat/highcharts.js" type="text/javascript"></script>
    <script src="../../Scripts/module/sea.js" type="text/javascript"></script>
    <style>
        .gameTable  td{
            min-width:110px;
            height:30px;
        }
    </style>
	<script type="text/javascript">
	    $(function () {
	        $('#txtTime').daterangepicker();
	    });

	    seajs.use("../../Scripts/td/TdGameTimeDistribution.js?ver=1");
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth" style="text-align:center">
        <h2 style="padding:20px;">平均游戏时长分布</h2>
        <table style="float:left;">
            <tr>
                <td>时间：</td>
                <td><input type="text" style="width:400px;height:30px" id="txtTime"/></td>
            </tr>
        </table>
        <ul class="SelCard">
            <li class="Active" data="1">活跃玩家</li><li data="2">付费玩家</li><li data="3">新增玩家</li>
        </ul>
        <div class="clear"></div>
    </div>
    <div id="divTemplate" style="display:none">
        <h2 style="text-align:center;background:#ccc;padding:6px;font-size:30px;" id="{0}"></h2>
        <div style="width:950px; height: 100px; margin: 0 auto;display:none;" id="{1}"></div>
        <div style="width:950px; height: 100px; margin: 0 auto;display:none;" id="{2}"></div>
        <div style="width:950px; height: 100px; margin: 0 auto;display:none;" id="{3}"></div>
        <div style="width:950px; height: 100px; margin: 0 auto;display:none;" id="{4}"></div>
        <div style="width:950px; height: 100px; margin: 0 auto;display:none;" id="{5}"></div>
    </div>
    <div id="divContent"></div>
</asp:Content>
