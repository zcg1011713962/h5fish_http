<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatFishlordBaojinControl.aspx.cs" Inherits="WebManager.appaspx.stat.StatFishlordBaojinControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal" style="padding:20px;text-align:center">
        <h2 style="text-align:center;margin-bottom:20px;">爆金比赛场参数调整</h2>
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered">
        </asp:Table>
        <%--<div style="text-align:center">
            抽水比：<asp:TextBox ID="m_param" runat="server" CssClass="cTextBox"></asp:TextBox>
                    <asp:Button ID="Button1" runat="server" Text="确定" onclick="onConfirm" style="width:125px;height:25px"/>
                    <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        </div>--%>
    </div>
</asp:Content>
