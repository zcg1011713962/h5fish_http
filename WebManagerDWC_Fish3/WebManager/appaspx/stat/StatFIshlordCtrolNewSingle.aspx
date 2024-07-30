<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatFIshlordCtrolNewSingle.aspx.cs" Inherits="WebManager.appaspx.stat.StatFIshlordCtrolNewSingle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .OpModify{width:800px; background:rgb(255,255,255);padding-bottom:20px}
    </style>
    <script src="../../Scripts/module/sea.js"></script>
    <link rel="stylesheet" type="text/css" media="all" href="../../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../../Scripts/datepicker/daterangepicker.js"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#m_resetMidTime').daterangepicker();
	        $('#m_resetHighTime').daterangepicker();
	    });
	    seajs.use("../../Scripts/stat/FishControlNewSingle.js?ver=5");
    </script>
    <style>
        #m_resetMidTime {
            margin-bottom:10px;
        }
        .col-sm-2 {
            width:200px;
        }
        .col-sm-10 {
            width:450px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h2 style="text-align:center;margin-bottom:20px;">个人后台管理</h2>
        <asp:Table ID="m_expRateTable" runat="server" CssClass="table table-hover table-bordered" style="text-align:center">
        </asp:Table>
    </div>
        
    <div id="divModifyNewParam" class="PopDialogBg" >
        <div class="container form-horizontal OpModify">
            <h2 style="text-align:center;padding:10px;margin-bottom:10px;background:#ccc;cursor:pointer;">关闭</h2>
            <h3 style="text-align:center;padding:10px;margin-bottom:10px;"></h3>
            
            <%--///////////////////////////////////////////////////////////////////--%>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">基数:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_baseRate" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div> 

            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">误差值:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_deviationFix" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div> 

            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">系数:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_noValuePlayerRate" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div> 

            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10" style="width:520px">
                     <input type="button" value="提交修改"  class="btn btn-primary form-control" id="btnModifyParam" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                     <span id="opRes"></span>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
