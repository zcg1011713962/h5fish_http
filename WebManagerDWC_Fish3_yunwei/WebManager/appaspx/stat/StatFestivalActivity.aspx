<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatFestivalActivity.aspx.cs" Inherits="WebManager.appaspx.stat.StatFestivalActivity" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="cSafeWidth">
        <h2 style="text-align:center;padding:20px;">节日活动</h2>
        <asp:Button ID="Button1" runat="server" Text="查询" onclick="onQuery" CssClass="btn btn-primary" Width="100%"/><br/><br/>
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center;">
        </asp:Table>
    </div>
</asp:Content>
