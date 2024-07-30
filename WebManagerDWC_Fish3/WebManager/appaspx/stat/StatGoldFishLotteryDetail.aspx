<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatGoldFishLotteryDetail.aspx.cs" Inherits="WebManager.appaspx.stat.StatGoldFishLotteryDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .cSafeWidth td{padding:6px;}
        .cSafeWidth tr td:nth-of-type(1){text-align:center;}
        .cSafeWidth input[type=text], .cSafeWidth  select {width:220px;height:30px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div  style="background-color:#ccc;width:90%;margin:0 auto">
        <div class="cSafeWidth">
            <h2 style="text-align:center;padding:20px;">幸运抽奖玩家详情</h2>
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center"></asp:Table>
        </div>
        <span id="m_page" style="text-align:center;display:block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
    </div>
</asp:Content>
