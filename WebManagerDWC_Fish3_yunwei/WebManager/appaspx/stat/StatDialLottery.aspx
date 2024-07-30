<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatDialLottery.aspx.cs" Inherits="WebManager.appaspx.stat.StatDialLottery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../../../Scripts/datepicker/daterangepicker.js"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#MainContent_m_time').daterangepicker();
	    });
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="text-align:center;padding:20px;">转盘抽奖统计</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <asp:Button ID="Button1" runat="server" Text="查询" onclick="onQuery" Width="100%"  CssClass="btn btn-primary" /><br/><br/>
    </div>
    <div class="table-responsive">
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center;"> </asp:Table>
    </div>
    <br />
    <br />
    <span id="m_page" style="text-align:center;display:block" runat="server"></span>
    <br />
    <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
</asp:Content>
