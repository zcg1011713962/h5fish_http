<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatWjlwGoldEarn.aspx.cs" Inherits="WebManager.appaspx.stat.StatWjlwGoldEarn" %>
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
    <div class="container form-horizontal" style="width:60%">
        <h2 style="padding:20px;text-align:center">围剿龙王金币玩法统计</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control" style="height:35px;width:75%;display:inline-block"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="查询" CssClass="btn btn-primary form-control" style="height:35px;width:15%;display:inline-block"  OnClick="onQuery"/>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">当前期望盈利率(千分比)：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_expRate" runat="server" CssClass="form-control" placeHolder="此处为千分比，输入值为整数" style="height:35px;width:75%;display:inline-block"></asp:TextBox>
                <asp:Button ID="btnStat" runat="server" Text="确定" CssClass="btn btn-primary form-control" style="height:35px;width:15%;display:inline-block" OnClick="onConfirm"/>
                <span id="m_res" style="font-size:medium;color:red" runat="server">操作成功</span>
            </div>
        </div>
        <br />
        <div style="padding:0 1px;">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center"></asp:Table>
        </div>
         <span id="m_page" style="text-align: center; display: block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size: x-large; text-align: center; display: block" runat="server"></span>
     </div>
</asp:Content>
