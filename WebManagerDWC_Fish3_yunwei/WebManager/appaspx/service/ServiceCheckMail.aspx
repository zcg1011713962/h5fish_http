<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ServiceCheckMail.aspx.cs" Inherits="WebManager.appaspx.service.ServiceCheckMail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h2 style="text-align:center;padding:20px;">邮件检测</h2>
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered">
        </asp:Table>
        <div style="text-align:center;">
            <asp:Button ID="Button2" runat="server" onclick="onDelMail" Text="删除"  CssClass="btn btn-primary btn-lg"/>
            <asp:Button ID="Button1" runat="server" onclick="onSendMail" Text="发送" CssClass="btn btn-primary btn-lg"/>
        </div>
        <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
    </div>
</asp:Content>
