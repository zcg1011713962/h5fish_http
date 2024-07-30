<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PlayerItemRecord.aspx.cs" Inherits="WebManager.appaspx.operation.PlayerItemRecord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
	<script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>

    <script src="../../Scripts/module/sea.js"></script>
    <script src="../../Scripts/tool/FileSaver.min.js"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#MainContent_m_time').daterangepicker();
	    });
	    seajs.use("../../Scripts/operation/PlayerItemRecord.js");
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">玩家道具获取详情</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control" style="height:30px;"></asp:TextBox>
            </div>
        </div>
        <div class="form-group" id="playerId">
            <label for="account" class="col-sm-2 control-label">玩家ID：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_playerId" runat="server" CssClass="form-control" style="height:30px;"></asp:TextBox>
            </div>
        </div>
         <div class="form-group" id="Div1">
            <label for="account" class="col-sm-2 control-label">道具ID：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_itemId" runat="server" CssClass="form-control" style="height:30px;"></asp:TextBox>
            </div>
        </div>
         <div class="form-group" id="Div2">
            <label for="account" class="col-sm-2 control-label">同步字段：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_syncKey" runat="server" CssClass="form-control" style="height:30px;"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                 <asp:Button ID="btnStat" runat="server" Text="查询" CssClass="btn btn-primary form-control" OnClick="onQuery" style="width:300px;"/>

                <div class="col-sm-offset-2" style="display:inline-block;margin-left:10px;">
                    <input type="button" id="exportExcel" class="btn btn-primary form-control" value="导出Excel" style="width:300px;"/>
                </div>
            </div>
        </div>

        <br />
        <div style="padding:0 1px;">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center"></asp:Table>
        </div>
        <br />
        <br />
        <span id="m_page" style="text-align:center;display:block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
    </div>
</asp:Content>
