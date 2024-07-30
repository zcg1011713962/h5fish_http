﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatFishlordDeskEarningsRate.aspx.cs" Inherits="WebManager.appaspx.stat.StatFishlordDeskEarningsRate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="text-align:center">
        <h2 style="padding:20px;">经典捕鱼桌子盈利率查询</h2>
        <asp:DropDownList ID="m_room" runat="server"  CssClass="cDropDownList"></asp:DropDownList>&ensp;&ensp;
        <asp:Button ID="btnQuery" runat="server" Text="查询" CssClass="cButton" OnClick="btnQuery_Click" />

        <asp:Table ID="m_result" runat="server" CssClass="cTable"></asp:Table>

        <div class="cDivPageFoot">
            <br />
            <div id="m_page" style="text-align:center;display:block" runat="server"></div>
            <div id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></div>
            <div class="clear"></div>
        </div>
    </div>
</asp:Content>
