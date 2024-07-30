<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="TdActivation.aspx.cs" Inherits="WebManager.appaspx.td.TdActivation" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
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

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth">
        <h2 style="padding:20px;text-align:center">留存相关</h2>
        <table style="margin:0 auto">
            <tr>
                <td>时间：</td>
                <td colspan="2"><asp:TextBox ID="m_time" runat="server" style="width:400px;height:30px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>渠道：</td>
                <td><asp:DropDownList ID="m_channel" runat="server" style="width:250px;height:30px"></asp:DropDownList></td>
                <td><asp:Button ID="Button3" runat="server" onclick="onQuery" Text="查询" style="width:140px;height:30px" /></td>
            </tr>
        </table>
    </div>
    <div class="container-fluid" style="text-align:center;">
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
    </div>
</asp:Content>
