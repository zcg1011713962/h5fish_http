<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ShcdResult.aspx.cs" Inherits="WebManager.appaspx.stat.shcd.ShcdResult" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        td.left{width:100px;}
        table.tableForm td{padding-top:5px; padding-bottom:5px;}
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth" style="text-align:center">
        <h2 style="padding:20px;">黑红梅方结果控制</h2>
        <table style="margin:0 auto" class="tableForm">
            <tr>
                <td>房间：</td>
                <td><asp:DropDownList ID="m_roomList" runat="server" class="cDropDownList"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>开奖结果：</td>
                <td><asp:DropDownList ID="m_result" runat="server" class="cDropDownList"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>&ensp;</td>
                <td>
                    <asp:Button ID="Button2" runat="server" Text="设置结果" onclick="onSetResult" CssClass="cButton" style="width:180px;"/>
                </td>
            </tr>
        </table>
        <span id="m_res" style="font-size:medium;color:red" runat="server"></span>

        <div style="border:1px solid black;margin-top:10px;padding:5px;">
            <h2>结果列表</h2>
            <asp:Table ID="m_allResult" runat="server" CssClass="cTable">
            </asp:Table>
        </div>
    </div>
</asp:Content>
