<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatTotalConsume.aspx.cs" Inherits="WebManager.appaspx.stat.StatTotalConsume" %>
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
    <div class="container form-horizontal" style="text-align:center">
        <h2 style="padding:20px;">消耗总计</h2>
        货币类型：<asp:DropDownList ID="m_currencyType" runat="server" style="width:200px;height:30px">
            <asp:ListItem Value="1">金币</asp:ListItem>
            <asp:ListItem Value="2">钻石</asp:ListItem>
            <asp:ListItem Value="3">话费</asp:ListItem>
            <asp:ListItem Value="24">鱼雷</asp:ListItem>
            <asp:ListItem Value="28">魔石</asp:ListItem>
            <asp:ListItem Value="30">碎片</asp:ListItem>
        </asp:DropDownList>
    
        变化类型：<asp:DropDownList ID="m_changeType" runat="server" style="width:200px;height:30px"></asp:DropDownList>

        时间区间：<asp:TextBox ID="m_time" runat="server" style="width:300px;height:30px;"></asp:TextBox>
        <asp:Button ID="Button3" runat="server" onclick="onStat" Text="统计" style="width:60px;height:30px" />
    </div>
    <asp:Table ID="m_result" runat="server" CssClass="cTable">
    </asp:Table>
</asp:Content>
