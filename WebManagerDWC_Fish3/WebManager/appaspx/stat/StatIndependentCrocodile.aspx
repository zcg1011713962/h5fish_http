<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatIndependentCrocodile.aspx.cs" Inherits="WebManager.appaspx.stat.StatIndependentCrocodile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth" style="text-align:center">
        <h2 style="padding:20px;">独立数据-鳄鱼</h2>    
        <asp:Button ID="Button1" runat="server" onclick="onStat" Text="统计" style="width:100px;height:30px" />
        <br />
        <br />
        <asp:Table ID="m_result" runat="server" CssClass="result">
        </asp:Table>
    </div>
</asp:Content>
