<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatPlayerFavor.aspx.cs" Inherits="WebManager.appaspx.stat.StatPlayerFavor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#MainContent_m_time').daterangepicker();
	    });
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="text-align:center">
        <h2 style="text-align:center;padding:20px;">玩家喜好</h2>
        统计类型：<asp:DropDownList ID="m_statWay" runat="server" style="width:130px;height:35px"></asp:DropDownList>
        时间：<asp:TextBox ID="m_time" runat="server" style="width:300px;height:35px"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" onclick="onStat" Text="统计"  CssClass="btn btn-primary" />
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered">
        </asp:Table>
    </div>
</asp:Content>
