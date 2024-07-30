<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatGameRecharge.aspx.cs" Inherits="WebManager.appaspx.stat.StatGameRecharge" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
	<script type="text/javascript">
	    $(document).ready(function ()
	    {
	        $('#MainContent_m_time').daterangepicker();
	    });
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="text-align:center">
        <h2 style="padding:20px;">游戏每天收入统计</h2>
        时间区间：<asp:TextBox ID="m_time" runat="server" style="width:300px;height:30px;"></asp:TextBox>
        <asp:Button ID="Button3" runat="server" onclick="onStat" Text="统计" CssClass="btn btn-primary"/>
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"  style="margin-top:10px;">
        </asp:Table>
    </div>
</asp:Content>
