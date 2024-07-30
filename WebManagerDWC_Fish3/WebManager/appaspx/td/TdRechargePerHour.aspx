<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="TdRechargePerHour.aspx.cs" Inherits="WebManager.appaspx.td.TdRechargePerHour" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>

    <script src="../../Scripts/stat/highcharts.js" type="text/javascript"></script>
    <script src="../../Scripts/module/sea.js" type="text/javascript"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#MainContent_m_time').daterangepicker();
	    });

	    seajs.use("../../Scripts/td/TdRechargePerHour.js?ver=1");
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth">
        <h2 style="text-align:center;padding:20px;">实时付费</h2>

        <table style="margin:0 auto;">
            <tr>
                <td>时间：</td>
                <td><asp:TextBox ID="m_time" runat="server" style="width:300px;" CssClass="form-control"></asp:TextBox></td>
               <%-- <td>&ensp;渠道：</td>
                <td><asp:DropDownList ID="m_channel" runat="server" style="width:300px;" CssClass="form-control"></asp:DropDownList></td>--%>
                <td><asp:Button ID="Button3" runat="server" onclick="onQuery" Text="查询" style="width:60px;height:35px" /></td>
            </tr>
        </table>
    </div>
    <asp:Table ID="m_result" runat="server" CssClass="cTable"></asp:Table>
    
    <div id="container" style="min-width: 310px; height: 400px; margin: 0 auto"></div>
</asp:Content>
