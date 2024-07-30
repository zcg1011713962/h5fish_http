<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatCrocodile.aspx.cs" Inherits="WebManager.appaspx.stat.StatCrocodile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal" style="text-align:center;">
        <h2 style="padding:20px;">鳄鱼大亨参数调整</h2>
        <asp:Table ID="m_expRateTable" runat="server" CssClass="cTable">
        </asp:Table>
        <br />
        <table style="margin:0 auto">
            <tr>
                <td>修改参数：</td>
                <td style="min-width:200px;">
                    <asp:DropDownList ID="m_paramItem" runat="server" CssClass="form-control"></asp:DropDownList>
                </td>
                <td>&ensp;&ensp;具体值：</td>
                <td><asp:TextBox ID="m_expRate" runat="server" style="width:100px;height:25px"></asp:TextBox></td>
                <td>
                    &ensp;<asp:Button ID="Button3" runat="server" Text="修改参数" onclick="onModifyExpRate" style="width:125px;height:25px"/>
                </td>
                <td>
                    &ensp;<asp:Button ID="Button1" runat="server" Text="重置" onclick="onReset" style="width:125px;height:25px"/>
                </td>
            </tr>
        </table>
        <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
    </div>
</asp:Content>
