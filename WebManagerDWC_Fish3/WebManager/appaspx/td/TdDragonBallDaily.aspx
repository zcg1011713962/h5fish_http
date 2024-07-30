<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="TdDragonBallDaily.aspx.cs" Inherits="WebManager.appaspx.td.TdDragonBallDaily" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
         .cTable tr:hover td{background:#ddd;}
    </style>
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
    <div class="container form-horizontal">
        <h2 style="text-align:center;padding:20px;">每日龙珠</h2>
      <%--  <table>
            <tr>
                <td>时间:</td>
                <td><asp:TextBox ID="m_time" runat="server" style="width:400px;height:20px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>每龙珠价值:</td>
                <td><asp:TextBox ID="m_eachValue" runat="server" style="width:400px;height:20px"></asp:TextBox></td>
                <td>小数，默认值1</td>
            </tr>
            <tr>
                <td>渠道折价:</td>
                <td><asp:TextBox ID="m_discount" runat="server" style="width:400px;height:20px"></asp:TextBox></td>
                <td>小数，默认值1</td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="Button3" runat="server" onclick="onQuery" Text="查询" style="width:60px;height:30px" />
                    <asp:Button ID="Button1" runat="server" onclick="onExport" Text="导出Excel" style="width:100px;height:30px" />
                </td>
            </tr>
            <tr>
                <td colspan="3"><span id="m_res" style="font-size:medium;color:red" runat="server"></span></td>
            </tr>
        </table> --%>
         <div class="form-group">
            <label for="account" class="col-sm-2 control-label">时间:</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">每龙珠价值:</label>
            <div class="col-sm-8">
                <asp:TextBox ID="m_eachValue" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-sm-2">
                <label for="account" class="control-label">小数，默认值0.12</label>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">渠道折价:</label>
            <div class="col-sm-8">
                <asp:TextBox ID="m_discount" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-sm-2">
                <label class="control-label">小数，默认值0.45</label>
            </div>
        </div>
        <div style="text-align:center;">
            <asp:Button ID="Button3" runat="server" onclick="onQuery" Text="查询" CssClass="btn btn-primary" />
            <asp:Button ID="Button1" runat="server" onclick="onExport" Text="导出Excel" CssClass="btn btn-primary" />
        </div>
        <div class="col-sm-offset-2 col-sm-10">
            <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        </div>
    </div>
    <div class="container-fluid"><asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table></div>
    <br />
    <span id="m_page" style="text-align:center;display:block" runat="server"></span>
    <br />
    <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
</asp:Content>
