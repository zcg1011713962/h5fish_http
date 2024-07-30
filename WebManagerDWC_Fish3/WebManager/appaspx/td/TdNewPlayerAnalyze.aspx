<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="TdNewPlayerAnalyze.aspx.cs" Inherits="WebManager.appaspx.td.TdNewPlayerAnalyze" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script src="../../Scripts/module/sea.js" type="text/javascript"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#txtTime').daterangepicker();
	    });

	    seajs.use("../../Scripts/td/TdNewPlayerAnalyze.js");
	</script>
    <style type="text/css">
        #txtTime{width:400px;height:30px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth">
        <h2 style="text-align:center;padding:20px;">新进用户分析</h2>
        <table style="float:left;">
            <tr>
                <td>时间&ensp;:&ensp;</td>
                <td><input type="text" id="txtTime"/></td>
            </tr>
            <tr><td>&ensp;</td></tr>
            <tr>
                <td>渠道&ensp;:&ensp;</td>
                <td><asp:DropDownList ID="m_channel" runat="server" style="height:30px;"></asp:DropDownList></td>
            </tr>
        </table>
        <ul class="SelCard" style="float:left;">
            <li class="Active" data="1">炮数成长分布</li><li data="2">金币下注分布</li><li data="3">捕鱼活跃统计</li><li data="5">登录房间次数</li>
        </ul>
    </div>
    <div class="clear"></div>
    <br />
    <table id="m_result" class="cTable Report"></table>
</asp:Content>
