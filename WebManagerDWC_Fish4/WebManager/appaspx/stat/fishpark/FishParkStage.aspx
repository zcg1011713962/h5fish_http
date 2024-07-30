﻿<%@ Page Title="" Language="C#" MasterPageFile="~/appaspx/stat/StatCommonFishPark.master" AutoEventWireup="true" CodeBehind="FishParkStage.aspx.cs" Inherits="WebManager.appaspx.stat.fishpark.FishParkStage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="StatCommonFishPark_HeadContent" runat="server">
    <script type="text/javascript" src="/Scripts/DateRange/js/jquery-1.3.1.min.js"></script>
	<script type="text/javascript" src="/Scripts/DateRange/js/jquery-ui-1.7.1.custom.min.js"></script>
	<script type="text/javascript" src="/Scripts/DateRange/js/daterangepicker.jQuery.js"></script>
	<link rel="stylesheet" href="/Scripts/DateRange/css/ui.daterangepicker.css" type="text/css" />
	<link rel="stylesheet" href="/Scripts/DateRange/css/redmond/jquery-ui-1.7.1.custom.css" type="text/css" title="ui-theme" />
	<script type="text/javascript">
	    $(function () {
	        $('#MainContent_stat_common_StatCommonFishPark_Content_m_time').daterangepicker({ arrows: false });
	    });
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="StatCommonFishPark_Content" runat="server">
    <div class="cSafeWidth">
        <h2>鳄鱼公园算法分析</h2>  
        房间:&nbsp;&nbsp;<asp:DropDownList ID="m_room" runat="server" style="width:180px;height:30px"></asp:DropDownList>
        时间:&nbsp;&nbsp;<asp:TextBox ID="m_time" runat="server" style="width:400px;height:20px"></asp:TextBox>  
        <asp:Button ID="Button1" runat="server" onclick="onQuery" Text="查询" style="width:100px;height:30px" />
        <asp:Table ID="m_result" runat="server" CssClass="cTable">
        </asp:Table>
        <br />
        <span id="m_page" style="text-align:center;display:block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
    </div>
</asp:Content>
