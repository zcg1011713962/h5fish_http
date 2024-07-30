<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatJinQiuNationalDayActTask.aspx.cs" Inherits="WebManager.appaspx.stat.StatJinQiuNationalDayActTask" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .cSafeWidth td{padding:6px;}
        .cSafeWidth tr td:nth-of-type(1){text-align:center;}
        .cSafeWidth input[type=text], .cSafeWidth  select {width:220px;height:30px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="table-responsive cSafeWidth"  style="background-color:#ccc;width:99%;margin:0 auto;text-align:center">
        <h2 style="text-align:center;padding:20px;">双十一活动任务统计详情</h2>
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
    </div>
</asp:Content>
