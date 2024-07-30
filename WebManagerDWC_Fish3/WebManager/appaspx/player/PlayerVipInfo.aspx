<%@ Page Title=""  Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PlayerVipInfo.aspx.cs" Inherits="WebManager.appaspx.player.PlayerVipInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal" style="width:50%">
        <h2 style="padding:20px;text-align:center">VIP玩家信息</h2>
        <div class="form-group">
            <label for="account"  class="col-sm-2 control-label">VIP等级：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_vip" runat="server" CssClass="form-control" placeHolder="格式：3" style="width:300px;display:inline-block"></asp:TextBox> 
                <span>（筛选大于等于所填写的VIP等级用户）</span>
            </div>
        </div>
        <div class="form-group">
            <label  class="col-sm-2 control-label">绑定手机：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_isBindPhone" runat="server" CssClass="form-control" style="width:300px;display:inline-block"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">选择排序：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_sort" runat="server" CssClass="form-control" style="width:300px;display:inline-block"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label"></label>
            <div class="col-sm-10">
                <asp:Button ID="Button2" runat="server" Text="查询" OnClick="onQuery" CssClass="btn btn-primary" Style="width: 19%; " />
                <asp:Button ID="Button1" runat="server" OnClick="onExport" Text="导出Excel" CssClass="btn btn-primary"  Style="width: 19%; "/>
                <span id="m_resInfo" runat="server" style="color: red"></span>
                </div>
        </div>
    </div>
     <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align: center;"></asp:Table>
        <span id="m_page" style="text-align: center; display: block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size: x-large; text-align: center; display: block" runat="server"></span>
</asp:Content>
