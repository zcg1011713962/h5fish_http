<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="OperationMoneyWarn.aspx.cs" Inherits="WebManager.appaspx.operation.OperationMoneyWarn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="text-align:center">
    <h2 style="padding:20px;">金币预警</h2>
    货币类型&nbsp;：&ensp;<asp:DropDownList ID="m_currency" runat="server" style="width:200px;height:36px;" ></asp:DropDownList>
    <asp:Button ID="Button3" runat="server" onclick="onQuery" Text="查询"  CssClass="btn btn-primary" />
    <asp:Button ID="Button1" runat="server" onclick="onExport" Text="导出Excel" CssClass="btn btn-primary"  />
    <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
    <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="margin-top:10px;">
    </asp:Table>
    </div>
</asp:Content>
