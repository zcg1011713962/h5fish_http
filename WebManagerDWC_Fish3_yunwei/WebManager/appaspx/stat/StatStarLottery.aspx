﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatStarLottery.aspx.cs" Inherits="WebManager.appaspx.stat.StatStarLottery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
     <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">星星抽奖统计</h2>
         <div class="form-group">
            <label for="account" class="col-sm-2 control-label">时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">等级：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_level" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>

        <asp:Button ID="Button1" runat="server" Text="查询" onclick="onQuery" CssClass="btn btn-primary" Width="100%"/><br/><br/>

         <div class="container" style="margin-top:10px;">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
         </div>
      </div>
    <br />
    <br />
    <span id="m_page" style="text-align:center;display:block" runat="server"></span>
    <br />
    <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
</asp:Content>
