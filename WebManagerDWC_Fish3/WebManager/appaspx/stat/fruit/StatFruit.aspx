<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatFruit.aspx.cs" Inherits="WebManager.appaspx.stat.fruit.StatFruit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal" style="text-align:center;">
        <h2 style="padding:20px;">水果机参数调整</h2>
        <br />
        <asp:Table ID="m_expRateTable" runat="server" CssClass="cTable"></asp:Table>
        <br />
        <table style="margin:0 auto">
            <tr>
                <td>期望盈利率：</td>
                <td><asp:TextBox ID="m_expRate" runat="server" CssClass="form-control" style="width:150px;"></asp:TextBox></td>
                <td>
                    &ensp;<asp:Button ID="Button3" CssClass="btn btn-primary" runat="server" Text="修改" onclick="onModifyExpRate" style="width:150px;"/>
                </td>
                <td>
                    &ensp;<asp:Button ID="Button1" CssClass="btn btn-danger" runat="server" Text="重置" onclick="onReset" style="width:150px;"/>
                </td>
            </tr>
        </table>
        <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
    </div>
</asp:Content>
