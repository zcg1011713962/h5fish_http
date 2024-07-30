<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OperationGetPlayerIdByOrderId.aspx.cs" Inherits="WebManager.appaspx.operation.OperationGetPlayerIdByOrderId" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">通过第三方订单号查询玩家ID</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">支付表：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_table" runat="server" CssClass="form-control" style="height:35px;"></asp:DropDownList>
            </div>
        </div>

        <div class="form-group" id="playerId">
            <label for="account" class="col-sm-2 control-label">订单ID：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_orderId" runat="server" CssClass="form-control" style="height:30px;"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                 <asp:Button ID="btnStat" runat="server" Text="查询" CssClass="btn btn-primary form-control" OnClick="onQuery"/>
            </div>
        </div>
        <div style="padding:0 1px;">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center"></asp:Table>
            <span id="m_span" runat="server" style="color:red;display:block;text-align:center"></span>
        </div>
    </div>
</asp:Content>
