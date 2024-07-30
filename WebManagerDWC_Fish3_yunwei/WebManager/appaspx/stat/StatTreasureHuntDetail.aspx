<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatTreasureHuntDetail.aspx.cs" Inherits="WebManager.appaspx.stat.StatTreasureHuntDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="background-color: #ccc; width: 95%; margin: 0 auto;text-align:center">
        <div class="cSafeWidth" style="width:80%">
            <h2 style="padding: 20px;">南海寻宝玩家详情</h2>
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
        </div>
        <span id="m_page" style="text-align: center; display: block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size: x-large; text-align: center; display: block" runat="server"></span>
    </div>
</asp:Content>
