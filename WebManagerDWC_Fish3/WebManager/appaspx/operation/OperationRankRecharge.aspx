<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OperationRankRecharge.aspx.cs" Inherits="WebManager.ashx.OperationRankRecharge" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="text-align:center;padding-bottom:20px;">充值排行榜</h2>
        <div class="table-responsive" style="margin-top: 10px; text-align: center">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center;width:95%;margin:0 auto"></asp:Table>
        </div>
    </div>
</asp:Content>
