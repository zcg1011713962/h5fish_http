<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ServiceSelectTheLossOfPlayers.aspx.cs" Inherits="WebManager.appaspx.service.ServiceSelectTheLossOfPlayers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script src="../../Scripts/module/sea.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script src="../../Scripts/tool/FileSaver.min.js"></script>
    <style>
        td:nth-of-type(odd) {
            min-width: 95px;
        }
        td:nth-of-type(even) {
            min-width: 120px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
        });
        seajs.use("../../Scripts/service/ServiceSelectTheLossOfPlayers.js");
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal" style="width:50%">
        <h2 style="margin: 20px 0 10px 0; text-align: center">流失大户筛选</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">VIP等级：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_vip" runat="server" CssClass="form-control" placeHolder="格式：3" style="width:300px;display:inline-block"></asp:TextBox> 
                <span>（筛选大于所填写的VIP等级用户）</span>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">流失大于：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_days" runat="server" CssClass="form-control" placeHolder="格式：30" style="width:300px;display:inline-block"></asp:TextBox>
                <span>(筛选最后一次登录时间到查询当天累计时长超过所填写天数的用户)</span>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">充值行为：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control" style="width:300px;display:inline-block"></asp:TextBox>
                <span>（筛选在查询时间内有充值行为的用户）</span>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">绑定手机：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_isBindPhone" runat="server" CssClass="form-control" style="width:300px;display:inline-block"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group" style="text-align:center">
                <asp:Button ID="Button2" runat="server" Text="查询" OnClick="onQuery" CssClass="btn btn-primary" Style="width: 15%; margin-left: 30px;" />
                <input type="button" id="exportExcel" class="btn btn-primary form-control" value="导出Excel" style="width:15%;"/>    
                <span id="m_resInfo" runat="server" style="color: red"></span>
        </div>
    </div>
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align: center;"></asp:Table>
        <span id="m_page" style="text-align: center; display: block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size: x-large; text-align: center; display: block" runat="server"></span>
    
</asp:Content>
