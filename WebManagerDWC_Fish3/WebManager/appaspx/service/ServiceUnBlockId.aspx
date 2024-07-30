<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ServiceUnBlockId.aspx.cs" Inherits="WebManager.appaspx.service.ServiceUnBlockId" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">已停封玩家ID列表</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">玩家ID：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_playerId" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <asp:Button ID="Button1" runat="server" Text="查询" CssClass="btn btn-primary form-control" OnClick="onQueryBlockId"/><br />
        <br />
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center;"> </asp:Table>

        <asp:Button ID="btn_unBlockId" style="display:none;" runat="server" Text="解封" CssClass="btn btn-primary form-control" OnClick="onUnBlockPlayerId"/>
        <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        <br />
        <br />
        <span id="m_page" style="text-align:center;display:block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
    </div>
</asp:Content>
