<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="fruitResult.aspx.cs" Inherits="WebManager.appaspx.stat.fruit.fruitResult" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .cSafeWidth td{padding:5px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth" style="text-align:center">
        <h2 style="padding:20px;">水果机结果控制</h2>
        <table style="margin:0 auto">
            <tr>
                <td>玩家ID：</td>
                <td><asp:TextBox ID="m_player" runat="server" CssClass="form-control"></asp:TextBox></td>
            </tr>
            <tr>
                <td>开奖结果:</td>
                <td><asp:DropDownList ID="m_result" runat="server" CssClass="form-control" class="cDropDownList"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>&ensp;</td>
                <td>
                    <asp:Button ID="Button2" runat="server" Text="设置结果" onclick="onSetResult" CssClass="btn btn-primary" style="width:180px;"/>
                </td>
            </tr>
        </table>
         <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
    </div>
</asp:Content>
