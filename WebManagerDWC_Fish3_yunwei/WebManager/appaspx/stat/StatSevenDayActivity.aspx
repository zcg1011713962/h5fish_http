﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatSevenDayActivity.aspx.cs" Inherits="WebManager.appaspx.stat.StatSevenDayActivity" %>
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
        <h2 style="text-align:center;padding:20px;">新进用户七日活动</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">渠道：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_channel" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <asp:Button ID="Button1" runat="server" Text="查询" onclick="onQuery" Width="100%"  CssClass="btn btn-primary" /><br/><br/>
    </div>
    <div class="table-responsive">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center;"></asp:Table>
    </div>
    
</asp:Content>
