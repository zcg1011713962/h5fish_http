<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="TdLose.aspx.cs" Inherits="WebManager.appaspx.td.TdLose" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth">
        <h2 style="text-align:center;padding:20px;">大R流失</h2>

        <table style="margin:0 auto">
            <tr>
                <td colspan="2">
                    <asp:Button ID="Button3" runat="server" onclick="onQuery" Text="查询" style="width:120px;height:30px" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Table ID="m_result" runat="server" CssClass="cTable">
        </asp:Table>
    </div>
</asp:Content>
