<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ServiceExchangeAudit.aspx.cs" Inherits="WebManager.appaspx.service.ServiceExchangeAudit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <script src="../../Scripts/module/sea.js"></script>
    <script type="text/javascript">
        seajs.use("../../Scripts/service/ServiceExchangeAudit.js");
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="text-align:center;width:90%">
        <h2 style="text-align:center;padding:20px;">实物审核管理</h2>
        审核状态:&nbsp;&nbsp;<asp:DropDownList ID="m_filter" runat="server" style="width:180px;height:35px"></asp:DropDownList>&ensp;&ensp;
        玩家ID:&nbsp;&nbsp;<asp:TextBox ID="m_param" runat="server" style="width:180px;height:35px"></asp:TextBox>&ensp;&ensp;
        &nbsp;<asp:Button ID="Button3" runat="server" onclick="onSearch" Text="查询" CssClass="btn btn-primary" />
        <br /><br />
        <asp:Table ID="GiftTable" runat="server" CssClass="table table-hover table-bordered">
        </asp:Table>
        <span id="m_page" style="text-align:center;display:block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
    </div>
</asp:Content>
