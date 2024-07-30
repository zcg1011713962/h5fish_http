<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatFishlordControlNewDetail.aspx.cs" Inherits="WebManager.appaspx.stat.StatFishlordControlNewDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .OpModify{width:800px; background:rgb(255,255,255);padding-bottom:20px}
    </style>
    <script src="../../Scripts/module/sea.js"></script>
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
    <div class="container-fluid container form-horizontal">
        <h2 style="text-align:center;margin-bottom:20px;"><span id="roomName" runat="server"></span>场次后台管理</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">查询时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <asp:Button ID="Button1" runat="server" Text="查询" onclick="onQuery" CssClass="btn btn-primary" style="width:90%;margin-left:10%"/>
        <br />
        <br />
        <asp:Table ID="m_expRateTable" runat="server" CssClass="table table-hover table-bordered" style="text-align:center">
        </asp:Table>
    </div>
</asp:Content>
