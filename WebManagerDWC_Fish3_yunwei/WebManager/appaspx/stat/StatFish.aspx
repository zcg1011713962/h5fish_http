<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatFish.aspx.cs" Inherits="WebManager.appaspx.stat.StatFish" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#MainContent_m_time').daterangepicker();
	    });

	    function onClearFishTable() {
	        alert("ok");
	    }

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h2 style="text-align:center;margin-bottom:20px;">鱼的统计</h2>
        <div class="col-sm-offset-4" style="margin-bottom:20px;">
            房间:&nbsp;&nbsp;<asp:DropDownList ID="m_room" runat="server" class="cDropDownList"></asp:DropDownList>
            &nbsp;&nbsp;&nbsp;
            时间：<asp:TextBox ID="m_time" runat="server"  style="height:35px;width:250px;"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button2" runat="server" Text="查询" onclick="onQuery" class="btn btn-primary"/>
            <asp:Button ID="Button1" runat="server" Text="清空鱼表" onclick="onClearFishTable" 
                OnClientClick="return confirm('是否确定清空鱼表?')" class="btn btn-primary"/>
        </div>
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"  style="text-align:center;"></asp:Table>
        <span id="m_page" style="text-align: center; display: block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size: x-large; text-align: center; display: block" runat="server"></span>
    </div>
</asp:Content>
