﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ShcdIndependent.aspx.cs" Inherits="WebManager.appaspx.stat.shcd.ShcdIndependent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <div class="cSafeWidth" style="text-align:center">
        <h2 style="padding:20px;">独立数据-黑红梅方</h2>    
        <asp:Button ID="Button1" runat="server" onclick="onStat" Text="统计" style="width:100px;height:30px" />
        <asp:Table ID="m_result" runat="server" CssClass="cTable">
        </asp:Table>
    </div>
</asp:Content>
