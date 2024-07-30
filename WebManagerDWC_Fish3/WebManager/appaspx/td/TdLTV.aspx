<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="TdLTV.aspx.cs" Inherits="WebManager.appaspx.td.TdLTV" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#MainContent_m_time').daterangepicker();
	    });
	</script>

    <style type="text/css">
        .cSafeWidth td{padding:5px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth">
        <h2 style="text-align:center;padding:20px;">LTV统计</h2>
        <table style="margin:0 auto">
            <tr>
                <td>时间：</td>
                <td colspan="2"><asp:TextBox ID="m_time" runat="server" style="width:400px;height:35px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>渠道：</td>
                <td><asp:DropDownList ID="m_channel" runat="server" style="width:240px;height:35px"></asp:DropDownList></td>
                <td><asp:Button ID="Button3" runat="server" onclick="onQuery" Text="查询" style="width:150px;"  CssClass="btn btn-primary" /></td>
            </tr>
        </table>
    </div>
    <br />
    <div class="container-fluid" style="text-align:center">
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered">
        </asp:Table>
    </div>
</asp:Content>
