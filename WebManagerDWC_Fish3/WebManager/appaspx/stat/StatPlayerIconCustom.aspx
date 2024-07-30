<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatPlayerIconCustom.aspx.cs" Inherits="WebManager.appaspx.stat.StatPlayerIconCustom" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="text-align:center;padding:20px;">VIP2及以上玩家自定义头像统计</h2>
        <asp:Button ID="Button1" runat="server" Text="查询" onclick="onQuery" Width="100%"  CssClass="btn btn-primary" /><br/><br/>
    
        <div class="table-responsive">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center;"> </asp:Table>
        </div>
        <br />
        <br />
        <span id="m_page" style="text-align:center;display:block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
    </div>
</asp:Content>
