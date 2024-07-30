<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatOldEarningsRate.aspx.cs" Inherits="WebManager.appaspx.stat.StatOldEarningsRate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 style="text-align:center;padding:20px;">重置前盈利率</h2>    
        <p style="text-align:center;">
            游戏：<asp:DropDownList ID="m_game" runat="server" style="width:200px;height:30px"></asp:DropDownList>
            <asp:Button ID="Button1" runat="server" onclick="onQuery" Text="查询" style="width:100px;height:30px" />
        </p>
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center">
        </asp:Table>
    </div>
</asp:Content>
