<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatSdLotteryActDetail.aspx.cs" Inherits="WebManager.appaspx.stat.StatSdLotteryActDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div  style="background-color:#ccc;width:90%;margin:0 auto">
        <div class="cSafeWidth" style ="width:70%">
            <h2 style="text-align:center;padding:20px;">开服抽奖统计详情</h2>
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center"></asp:Table>
        </div>
        <span id="m_page" style="text-align:center;display:block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
    </div>
</asp:Content>
