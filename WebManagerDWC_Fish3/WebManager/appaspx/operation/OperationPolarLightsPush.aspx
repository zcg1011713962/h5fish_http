<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="OperationPolarLightsPush.aspx.cs" Inherits="WebManager.appaspx.operation.OperationPolarLightsPush" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .Boss{margin-top:20px;border:1px solid black;padding:10px;}
        .OpModify{width:800px; background:rgb(255,255,255);padding-bottom:20px}
    </style>
    <script src="../../Scripts/module/sea.js"></script>
    <link rel="stylesheet" type="text/css" media="all" href="../../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../../Scripts/datepicker/daterangepicker.js"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#m_date').daterangepicker();
	    });
	    seajs.use("../../Scripts/operation/PolarLightsPush.js");
    </script>
    <style>
        #m_resetMidTime {
            margin-bottom:10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth" style="width:80%">
        <h2 style="text-align:center;margin-bottom:20px;">极光推送</h2>
        <table style="margin:0 auto;text-align:center;">
            <tr>
                <td>
                    <input type="button" id="btn_add" class="btn btn-primary form-control"  style="width:125px;height:35px;" value="新增" />
                </td>
            </tr>
            <tr>
                <td>
                    <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
                </td>
            </tr>
        </table><br/><br/><br/>
        <asp:Table ID="m_setListTable" style="text-align:center;width:100%" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
    </div>
    <div id="divModifyNewParam" class="PopDialogBg">
        <div class="container form-horizontal OpModify">
            <h3 id="btn_close" style="text-align:center;padding:10px;margin-bottom:10px;background:#ccc;cursor:pointer;">关闭</h3>
            
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">目标玩家渠道：</label>
                <div class="col-sm-6" style="height:100px;overflow:auto;border:1px solid #ccc;margin-left:10px;">
                    <asp:CheckBoxList ID="m_channel" runat="server"></asp:CheckBoxList>
                </div>
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">目标玩家VIP：</label>
                <div class="col-sm-6">
                    <asp:DropDownList ID="m_vip" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">推送日期区间：</label>
                <div class="col-sm-6">
                    <asp:TextBox ID="m_date" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">推送日期星期：</label>
                <div class="col-sm-6">
                    <asp:CheckBoxList ID="m_week" runat="server"></asp:CheckBoxList>
                </div>
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">推送时间：</label>
                <div class="col-sm-6">
                    <asp:TextBox ID="m_time1" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
                <div class="col-sm-4">
                    <span>精确到时分，格式如：08:08</span>
                </div> 
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">推送内容：</label>
                <div class="col-sm-6">
                    <asp:TextBox ID="m_content" runat="server" CssClass="form-control" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">备注：</label>
                <div class="col-sm-6">
                    <asp:TextBox ID="m_note" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div>
            <input id="btn_submit" type="button" class="btn btn-primary form-control"  value="提交修改" />
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                     <span id="m_opRes" style="font-size:medium;color:red" runat="server"></span>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
