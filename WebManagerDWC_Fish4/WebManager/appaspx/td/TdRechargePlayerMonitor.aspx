<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="TdRechargePlayerMonitor.aspx.cs" Inherits="WebManager.appaspx.td.TdRechargePlayerMonitor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
         .cTable tr:hover td{background:#ddd;}
         .cSafeWidth td{padding:6px;}
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
    <div class="cSafeWidth" style="text-align:center;">
        <h2 style="padding:20px;">新进玩家付费监控</h2>
        <table style="margin:0 auto">
            <tr>
                <td>渠道:</td>
                <td><asp:DropDownList ID="m_channel" runat="server" CssClass="form-control"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>注册时间:</td>
                <td><asp:TextBox ID="m_time" runat="server" style="width:400px;height:35px"></asp:TextBox></td>
                 <td style="padding-left:10px;">
                    <asp:Button ID="Button3" runat="server" onclick="onQuery" Text="查询" CssClass="btn btn-primary"/>
                 </td>
                <td style="padding-left:10px;">
                    <asp:Button ID="Button1" runat="server" onclick="onExport" Text="导出Excel" CssClass="btn btn-primary" />
                </td>
            </tr>
            <tr>
                <td colspan="3"><span id="m_res" style="font-size:medium;color:red" runat="server"></span></td>
            </tr>
        </table>
    </div>
    <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center"></asp:Table>
    <br />
    <span id="m_page" style="text-align:center;display:block" runat="server"></span>
    <br />
    <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
</asp:Content>
