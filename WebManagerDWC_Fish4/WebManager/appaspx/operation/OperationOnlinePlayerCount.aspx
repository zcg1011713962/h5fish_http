<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="OperationOnlinePlayerCount.aspx.cs" Inherits="WebManager.appaspx.operation.OperationOnlinePlayerCount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="text-align:center">
        <h2 style="padding:20px;">当前在线人数</h2>
        <asp:Label ID="m_count" runat="server" Enabled="false" style="width:300px;height:30px;font-size:25px" ></asp:Label>
    </div>
</asp:Content>
