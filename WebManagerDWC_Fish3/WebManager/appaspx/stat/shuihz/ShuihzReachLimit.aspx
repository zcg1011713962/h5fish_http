<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ShuihzReachLimit.aspx.cs" Inherits="WebManager.appaspx.stat.shuihz.ShuihzReachLimit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../../Scripts/datepicker/daterangepicker.js"></script>
    <script>
        $(function () {
            $('#MainContent_m_time').daterangepicker();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="text-align:center;padding:20px;">水浒传每日达上下限人数统计</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">查询时间&ensp;:</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
                <br />
            </div>
            <asp:Button ID="Button1" runat="server" Text="查询"  onclick="onQuery"  CssClass="btn btn-primary" Width="100%"/><br/><br/>
        </div>
        
        <div class="form-group">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center;">
        </asp:Table>
        </div>
    </div>
</asp:Content>
