<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatServiceDumpTime.aspx.cs" Inherits="WebManager.appaspx.stat.StatServiceDumpTime" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <div class="container form-horizontal">
            <h2 style="padding:20px;text-align:center">服务器宕机记录</h2>
      </div>
    <div class="table-responsive" style="margin:0 20px;">
         <span id="m_res" style="font-size:medium;color:red;text-align:center;" runat="server"></span>
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
    </div>
</asp:Content>
