<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatServerEarnings.aspx.cs" Inherits="WebManager.appaspx.stat.StatServerEarnings" %>
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
        //seajs.use("../../Scripts/stat/StatServerEarnings.js?ver=2");
	</script>
    <style>
        td {
            min-width:100px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth" style="text-align:center">
        <h2 style="padding:20px;">游戏金币流动统计</h2>
        时间&ensp;:&ensp;<asp:TextBox ID="m_time" runat="server" style="width:300px;height:35px"></asp:TextBox>&ensp;
         <asp:Button ID="Button1" runat="server" Text="确定" onclick="onStat" CssClass="btn btn-primary" style="width:15%;"/>
    </div>

    <div id="chartActive" style="max-width:1200px;min-height:400px; margin:10px auto;border:1px solid #000;border-radius:10px;padding:10px;display:none"></div>
    <div id="chartEarnValue" style="max-width:1200px;min-height:400px; margin:10px auto;border:1px solid #000;border-radius:10px;padding:10px;display:none"></div>
    <br />
    <div class="table-responsive" style="margin:0 20px;">
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center;">
        </asp:Table>
    </div>
</asp:Content>
