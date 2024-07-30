<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatIndependentDice.aspx.cs" Inherits="WebManager.appaspx.stat.StatIndependentDice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>独立数据-骰宝</h2>    
    <asp:Button ID="Button1" runat="server" onclick="onStat" Text="统计" style="width:100px;height:30px" />
    <asp:Button ID="Button2" runat="server" onclick="onDelData" Text="清空数据" style="width:100px;height:30px" 
        OnClientClick="return confirm('确认清空数据?')"/>
    <asp:Table ID="m_result" runat="server" CssClass="result">
    </asp:Table>
</asp:Content>
