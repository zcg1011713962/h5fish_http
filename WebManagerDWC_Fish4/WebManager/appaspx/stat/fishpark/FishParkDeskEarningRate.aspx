﻿<%@ Page Title="" Language="C#" MasterPageFile="~/appaspx/stat/StatCommonFishPark.master" AutoEventWireup="true" CodeBehind="FishParkDeskEarningRate.aspx.cs" Inherits="WebManager.appaspx.stat.fishpark.FishParkDeskEarningRate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="StatCommonFishPark_HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="StatCommonFishPark_Content" runat="server">
    <div class="cSafeWidth">
        <h2>鳄鱼公园桌子盈利率</h2>
        <asp:DropDownList ID="m_room" runat="server"  CssClass="cDropDownList"></asp:DropDownList>
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
