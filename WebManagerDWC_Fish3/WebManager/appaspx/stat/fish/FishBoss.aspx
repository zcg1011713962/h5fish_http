<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="FishBoss.aspx.cs" Inherits="WebManager.appaspx.stat.fish.FishBoss" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../../../Scripts/datepicker/daterangepicker.js"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#MainContent_m_time').daterangepicker();
	    });
	</script>
    <style type="text/css">
        .cSafeWidth td{padding:5px;}
        table.Report td{font-size:16px;padding:10px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="text-align:center;margin-bottom:20px;">BOSS消耗</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">房间:</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_room" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">BOSS:</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_bossList" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">时间:</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                 <asp:Button ID="btnQuery" runat="server" Text="查询" CssClass="btn btn-primary form-control" onclick="onQuery" ClientIDMode="Static"/>
            </div>
         </div>
    </div>
    <div class="container-fluid" style="text-align:center">
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
    </div>
</asp:Content>
