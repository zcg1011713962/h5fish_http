<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="LogViewer.aspx.cs" Inherits="WebManager.appaspx.LogViewer" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="stylesheet" type="text/css" media="all" href="../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../Scripts/datepicker/daterangepicker.js"></script>
    <script src="../Scripts/module/sea.js" type="text/javascript"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#MainContent_m_time').daterangepicker();
	    });

	    seajs.use("../Scripts/LogViewer.js");
	</script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2 style="text-align:center;padding:20px;">查看日志</h2>
    <div class="container">
        操作类型：<asp:DropDownList ID="opType" runat="server" style="width:250px;height:32px;"></asp:DropDownList>
        操作时间：<asp:TextBox ID="m_time" runat="server" style="width:250px;height:32px;"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="查询" OnClick="onSearchLog" CssClass="btn btn-default" />

        <input type="button" class="btn btn-primary" value="全选" id="btnSelAll"/>
        <input type="button" class="btn btn-primary" value="取消选择" id="btnCancelSelAll"/>
        <input type="button" class="btn btn-success" value="删除所选日志" id="btnDelLog"/>
    </div>
  <%--  <asp:Button ID="Button2" runat="server" Text="删除所有日志" style="width:100px;height:30px" OnClick="onDelAllLog"
         OnClientClick="return confirm('确认删除所有日志?')" />--%> 

    <div class="container-fluid" style="margin-top:30px;">
        <asp:Table ID="LogTable" runat="server" CssClass="table table-hover table-bordered">
        </asp:Table>
    </div>
    <br />
    <br />
    <span id="m_page" style="text-align:center;display:block" runat="server"></span>
    <br />
    <br />
    <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
</asp:Content>
