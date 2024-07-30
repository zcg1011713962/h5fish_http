<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="OperationRecharge.aspx.cs" Inherits="WebManager.appaspx.operation.OperationRecharge" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="text-align:center;padding-bottom:20px;">后台充值</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">充值金额:</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_rechargeRMB" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">玩家ID:</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_playerId" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">备注(可填操作原因):</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_comment" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                 <asp:Button ID="Button2" runat="server" Text="充值" CssClass="btn btn-primary form-control"
                                onclick="onRecharge"  OnClientClick="return confirm('是否确定给该玩家充值?')" />
            </div>
         </div>
        <div class="col-sm-offset-2 col-sm-10">
            <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        </div>
    </div>
</asp:Content>
