<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatReloadTable.aspx.cs" Inherits="WebManager.appaspx.stat.StatReloadTable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 style="text-align:center;padding:20px;">重新加载表格</h2>    
        表格：<br />
        <asp:DropDownList ID="m_table" runat="server" CssClass="form-control"></asp:DropDownList>
        <asp:Button ID="Button1" runat="server" onclick="onLoad" Text="加载" CssClass="btn btn-primary btn-lg" style="margin-top:10px;"  />
        <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        <br />
        <br />
        <hr />
        <br />
        <br />
        <h3 style="display:inline">性能测试日志： </h3>
        <asp:Button ID="btn_open" runat="server" onclick="onOpen" Text="开" CssClass="btn btn-primary btn-lg" style="margin-top:10px;"  />
        &ensp;&ensp;
        <asp:Button ID="btn_close" runat="server" onclick="onClose" Text="关" CssClass="btn btn-primary btn-lg" style="margin-top:10px;"  />
        <span id="m_res1" style="font-size:medium;color:red" runat="server"></span>
    </div>
</asp:Content>
