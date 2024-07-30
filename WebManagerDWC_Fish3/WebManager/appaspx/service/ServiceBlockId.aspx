<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ServiceBlockId.aspx.cs" Inherits="WebManager.appaspx.service.ServiceBlockId" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">停封玩家ID</h2>
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
                <asp:Button ID="btn" runat="server" Text="停封" CssClass="btn btn-primary form-control" OnClick="onBlockPlayerId"/>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
            </div>
        </div>
    </div>
</asp:Content>
