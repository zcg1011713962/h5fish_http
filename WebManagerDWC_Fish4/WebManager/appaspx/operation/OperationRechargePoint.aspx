<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="OperationRechargePoint.aspx.cs" Inherits="WebManager.appaspx.operation.OperationRechargePoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script src="../../Scripts/module/sea.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script src="../../Scripts/tool/FileSaver.min.js"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#MainContent_m_time').daterangepicker();
	    });
	    seajs.use("../../Scripts/operation/OperationRechargePoint.js");
	</script>
    <style type="text/css">
        .rechargePoint{padding-left:10px;}
        .rechargePoint table:first-child td{padding:2px;}
         .cTable tr:hover td{background:#ddd;}
         .cTable td{font-size:12px;font-weight:bold;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="text-align:center;padding:20px;">付费点统计</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">时间:</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">渠道:</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_channel" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">查看方式:</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_showWay" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
           &ensp;&ensp;
            <div class="col-sm-offset-2" style="display:inline-block;margin-top:20px;margin-right:10px;">
                <asp:Button ID="Button3" runat="server" CssClass="btn btn-primary form-control"
                    onclick="onQuery" Text="查询" style="width:200px;"></asp:Button>
            </div>
            <div class="col-sm-offset-2" style="display:inline-block;margin-left:10px;">
                <input type="button" id="exportExcel" class="btn btn-primary form-control" 
                    value="导出Excel" style="width:200px;"/>
            </div>
        </div>
    </div>
    <div class="container-fluid table-responsive">
        <asp:Table ID="m_result" runat="server" CssClass="table table-bordered table-hover cTable" >
        </asp:Table>
    </div>
    <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
</asp:Content>
