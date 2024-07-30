<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="OperationRank.aspx.cs" Inherits="WebManager.appaspx.operation.OperationRank" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script src="../../Scripts/module/sea.js" type="text/javascript"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#time').daterangepicker();
	    });
        seajs.use("../../Scripts/operation/OperationRank.js");
	</script>
    <style type="text/css">
        .SearchCondition .form-control
        {
            height: 35px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 style="text-align:center;padding:20px;">排行榜</h2>
        <table class="SearchCondition" style="margin:0 auto;">
            <tr>
                <td>排行方式：</td>
                <td>
                    <select id="rankWay" class="form-control">
                        <option value="0">增长</option>
                        <option value="1">净增长</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                     <select id="rankSel" class="form-control">
                        <option value="0">金币榜</option>
                        <option value="1">钻石榜</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>渠道：</td>
                <td><asp:DropDownList ID="m_channel" runat="server" class="form-control"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>时间：</td>
                <td><input type="text" id="time" class="form-control"/></td>
                <td><input type="button"  id="btnQuery" value="查询" class="btn btn-primary"/></td>
            </tr>
        </table>
    </div>
    <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
    <div id="resultDiv" class="container"></div>
    <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" >
    </asp:Table>
</asp:Content>
