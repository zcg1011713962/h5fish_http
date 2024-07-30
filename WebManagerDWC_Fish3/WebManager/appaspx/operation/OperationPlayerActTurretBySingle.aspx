<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OperationPlayerActTurretBySingle.aspx.cs" Inherits="WebManager.appaspx.operation.OperationPlayerActTurretBySingle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
	<script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    
    <script src="../../Scripts/module/sea.js"></script>
    <script src="../../Scripts/tool/FileSaver.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
            $('.Page').css('overflow-x', 'scroll');
        });
        seajs.use("../../Scripts/operation/OperationPlayerActTurretBySingle.js");
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">玩家炮数成长分布</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">查询时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">注册第x天：</label>
            <div class="col-sm-10">
                <asp:DropdownList ID="m_days" runat="server" CssClass="form-control"></asp:DropdownList>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">炮倍(默认全部)：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_turret" runat="server" CssClass="form-control" placeHolder="默认全部炮倍，1~10000 ，格式： 单个整数表示对应等级，两个以空格隔开整数表示区间"></asp:TextBox>
            </div>
        </div>
        <asp:Button ID="Button1" runat="server" Text="确定" onclick="onQuery" CssClass="btn btn-primary" style="width:300px;margin-left:17%"/>
        <asp:Button ID="Button2" runat="server" Text="导出Excel" onclick="onExport" CssClass="btn btn-primary" style="width:300px;margin-left:1%"/>
        <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        <br/><br/>
      </div>
    <div style="margin-top:10px;text-align:center;width:80%;margin:0 auto">
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
    </div>
    <span id="m_page" style="text-align: center; display: block" runat="server"></span>
    <br />
    <span id="m_foot" style="font-size: x-large; text-align: center; display: block" runat="server"></span>
</asp:Content>
