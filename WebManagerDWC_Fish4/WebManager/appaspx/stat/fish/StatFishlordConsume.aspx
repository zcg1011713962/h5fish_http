<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatFishlordConsume.aspx.cs" Inherits="WebManager.appaspx.stat.fish.StatFishlordConsume" %>
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
        <h2 style="padding:20px;text-align:center">捕鱼消耗统计</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">货币类型：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="moneyType" runat="server" CssClass="form-control" onchange="getRoom()"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control" style="height:30px;"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                 <asp:Button ID="btnStat" runat="server" Text="统计" CssClass="btn btn-primary form-control" OnClick="btnStat_Click"/>
            </div>
        </div>
        <br />
    </div>
    <div style="padding:0 10px;">
        <asp:Table ID="tabResult" runat="server" CssClass="table table-hover table-bordered" style="text-align:center"></asp:Table>
    </div>
</asp:Content>
