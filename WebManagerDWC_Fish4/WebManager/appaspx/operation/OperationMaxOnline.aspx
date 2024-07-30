<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="OperationMaxOnline.aspx.cs" Inherits="WebManager.appaspx.operation.OperationMaxOnline" %>
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
    <div class="cSafeWidth" style="text-align:center;">
        <h2 style="padding:20px;">最高在线</h2>
        日期：&nbsp;&nbsp;<asp:TextBox ID="m_time" runat="server" style="width:400px;height:30px"></asp:TextBox>
        &nbsp;
        <asp:Button ID="Button3" runat="server" onclick="onQuery" Text="查询" style="width:60px;height:30px" />

        <asp:Table ID="m_result" runat="server" CssClass="cTable"></asp:Table>
    </div>
</asp:Content>
