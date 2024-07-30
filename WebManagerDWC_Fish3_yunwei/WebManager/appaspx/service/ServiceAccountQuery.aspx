<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ServiceAccountQuery.aspx.cs" Inherits="WebManager.appaspx.service.ServiceAccountQuery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal" style="text-align:center">
        <h2 style="padding:20px;">账号查询</h2>
        <asp:DropDownList ID="m_queryWay" runat="server" style="width:200px;height:35px"></asp:DropDownList>
        <asp:TextBox ID="m_param" runat="server" style="width:350px;height:35px"></asp:TextBox>&ensp;&ensp;
        <asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" onclick="onQueryAccount" Text="查询" style="width:133px;height:35px" />
    </div>
    <br />
    <div class="container-fluid">
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
        <br />
        <span id="m_page" style="text-align:center;display:block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
    </div>
    <%--<hr />--%>
    <div class="container form-horizontal" style="display:none">
        <h2 style="padding:20px;text-align:center">添加账号(游客快速开始)</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">玩家ID：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_playerId" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">玩家账号：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_account" runat="server" CssClass="form-control"></asp:TextBox>
                <span>请输入6-14位字符，允许字母和数字</span>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">玩家密码：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_pwd" runat="server" CssClass="form-control"></asp:TextBox>
                <span>请输入6-14位字符，允许字母和数字</span>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Button ID="btn" runat="server" Text="确定" CssClass="btn btn-primary form-control" OnClick="onConfirm"/>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
            </div>
        </div>
    </div>
</asp:Content>
