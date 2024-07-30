﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ServiceGuideTheLossOfPlayers.aspx.cs" Inherits="WebManager.appaspx.service.ServiceGuideTheLossOfPlayers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <style>
        td:nth-of-type(odd) {
            min-width: 95px;
        }

        td:nth-of-type(even) {
            min-width: 120px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="background-color: #ccc; margin: 20px;">
        <h2 style="padding: 20px; text-align: center">流失大户引导完成添加记录</h2>
        <div style="margin: 0 auto; width: 30%; line-height: 28px; font-weight: bold;">
            注：该记录不可重复添加，仅可对每个ID设置一次引导记录；确认引导后记录引导时间；<br />
        </div>
        <table style="margin: 0 auto; margin-top: 10px;">
            <tr style="text-align: right">
                <td>引导ID：</td>
                <td><asp:TextBox ID="m_playerId" runat="server" CssClass="form-control" Style="width: 200px;"></asp:TextBox></td>
                <td style="min-width: 60px;">&ensp;&ensp;引导备注：</td>
                <td><asp:TextBox ID="m_comment" runat="server" CssClass="form-control" Style="width: 200px;"></asp:TextBox></td>
                <td><asp:Button ID="Button1" runat="server" Text="确认完成" OnClick="onSubmit" CssClass="btn btn-primary"/></td>
                <td><span id="m_res" runat="server" style="color: red;"></span></td>
            </tr>
        </table>
        <br />
        <br />
    </div>
    <hr />
    <%-- ///////////////////////////////////////////////////////////////////////////////////////////////////// --%>

    <div class="container form-horizontal">
        <h2 style="margin: 20px 0 10px 0; text-align: center">引导记录效果查询</h2>
        <div style="margin: 0 auto; width: 50%; line-height: 28px; font-weight: bold;">
            注：本列表只查询在此页面被添加过引导记录的玩家，展示被引导玩家后续的行为动态；<br />
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">时间：</label>
            <div class="col-sm-10" style="margin:0 auto">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control" Style="width: 50%; display: inline-block"></asp:TextBox>
                <asp:Button ID="Button2" runat="server" Text="查询" OnClick="onQuery" CssClass="btn btn-primary" Style="width: 30%; margin-left: 30px;" />
                <%--<asp:Button ID="Button3" runat="server" Text="导出excel" OnClick="onExportExcel" CssClass="btn btn-primary" Style="width: 15%;" />--%>
                <span id="m_resInfo" runat="server" style="color: red">操作成功</span>
            </div>
        </div>
        <div class="table-responsive" style="margin-top: 10px; text-align: center">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
        </div>
        <span id="m_page" style="text-align: center; display: block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size: x-large; text-align: center; display: block" runat="server"></span>
    </div>
    <br />
</asp:Content>
