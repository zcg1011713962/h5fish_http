﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatRebateGift.aspx.cs" Inherits="WebManager.appaspx.stat.StatRebateGift" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script src="../../Scripts/module/sea.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding: 20px; text-align: center">返利礼包</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control" Style="height: 35px;"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">类型：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_type" runat="server" CssClass="form-control" onchange="selectOpType()"></asp:DropDownList>
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Button ID="btn_query" runat="server" Text="查询" CssClass="btn btn-primary form-control" OnClick="onQuery" />
            </div>
        </div>
        <br />
        <br />
    </div>
    <div style="margin-top: 10px; text-align: center; width: 90%; margin: 0 auto">
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
    </div>
</asp:Content>
