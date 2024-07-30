<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="TdPlayerDragonBall.aspx.cs" Inherits="WebManager.appaspx.td.TdPlayerDragonBall" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
         .cTable tr:hover td{background:#ddd;}
    </style>
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
     <div class="cSafeWidth" style="text-align:center">
        <h2 style="padding:20px;">玩家龙珠监控</h2>
        <table style="margin:0 auto">
            <tr>
                <td>时间：</td>
                <td><asp:TextBox ID="m_time" runat="server" style="width:400px;height:35px"></asp:TextBox></td>
                <td><asp:Button ID="Button3" runat="server" onclick="onQuery" Text="查询"  CssClass="btn btn-primary" style="margin-left:10px;"/></td>
                <td><asp:Button ID="Button1" runat="server" onclick="onExport" Text="导出Excel" CssClass="btn btn-primary"  style="margin-left:10px;"/></td>
            </tr>
            <tr>
                <td colspan="3"><span id="m_res" style="font-size:medium;color:red" runat="server"></span></td>
            </tr>
        </table>
    </div>
    <br />
    <div class="container-fluid" style="text-align:center"><asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table></div>
    <br />
    <span id="m_page" style="text-align:center;display:block" runat="server"></span>
    <br />
    <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
</asp:Content>
