<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatGeneralPlayerLost.aspx.cs" Inherits="WebManager.appaspx.stat.StatGeneralPlayerLost" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#MainContent_m_time').daterangepicker();
	    });
	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth" style="text-align:center">
        <h2 style="padding:20px;">玩家流失查询</h2>
        <div style="margin:0 auto">
            账号创建时间：
            <asp:TextBox ID="m_time" runat="server" CssClass="cTextBox" style="width:300px;height:30px;"></asp:TextBox>
            &ensp;&ensp;小于金币：
            <asp:TextBox ID="m_gold" runat="server" CssClass="cTextBox" style="width:300px;height:30px;"></asp:TextBox>
        </div>
        <br />
        <asp:Button ID="btnQuery" runat="server" Text="查询" CssClass="cButton" OnClick="btnQuery_Click" style="width:100px;" />
        <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        
        <div runat="Server" id="divResult"></div>
    </div>
</asp:Content>
