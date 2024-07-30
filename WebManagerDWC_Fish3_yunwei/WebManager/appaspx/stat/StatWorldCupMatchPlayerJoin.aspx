<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatWorldCupMatchPlayerJoin.aspx.cs" Inherits="WebManager.appaspx.stat.StatWorldCupMatchPlayerJoin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth" style="width: 90%">
        <h2 style="text-align: center; margin-bottom: 20px;">世界杯大竞猜玩家押注统计</h2>
        <table style="margin: 0 auto; text-align: center;">
            <tr>
                <td style="min-width: 140px;">
                    <asp:Button ID="btn_query" runat="server" class="btn btn-primary form-control" Style="width: 120px; height: 35px;" Text="查询" OnClick="onQuery" />
                </td>
            </tr>
        </table>
        <br />
        <br />
        <br />
        <div class="table-responsive" style="margin-top: 10px; text-align: center">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
        </div>
    </div>
</asp:Content>
