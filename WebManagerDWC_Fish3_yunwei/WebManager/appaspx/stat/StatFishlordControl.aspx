<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatFishlordControl.aspx.cs" Inherits="WebManager.appaspx.stat.StatFishlordControl" %>
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
	        $('#m_resetMidTime').daterangepicker();
	        $('#m_resetHighTime').daterangepicker();
	    });
	    seajs.use("../../Scripts/stat/FishControl.js?ver=5");
    </script>
    <style>
        #m_resetMidTime {
            margin-bottom:10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h2 style="text-align:center;margin-bottom:20px;">经典捕鱼参数调整</h2>
        <asp:Table ID="m_expRateTable" runat="server" CssClass="table table-hover table-bordered">
        </asp:Table>
        <asp:Button ID="Button1" runat="server" Text="重置" onclick="onReset" style="width:125px;height:25px"/>
        <span id="m_res" style="font-size:medium;color:red" runat="server"></span>

        <div class="Boss">
            输入中级场重置日期:&ensp;<asp:TextBox ID="m_resetMidTime" runat="server" style="width:300px;height:25px" ClientIDMode="Static"></asp:TextBox>
            <br />
            输入高级场重置日期:&ensp;<asp:TextBox ID="m_resetHighTime" runat="server" style="width:300px;height:25px" ClientIDMode="Static"></asp:TextBox>
            <asp:Button ID="Button2" runat="server" Text="查询" onclick="onBoss" style="width:125px;height:25px" />
            <asp:Table ID="m_bossTable1" runat="server" CssClass="cTable"></asp:Table>
        </div>
    </div>

    <div id="divModifyNewParam" class="PopDialogBg" >
        <div class="container form-horizontal OpModify">
            <h2 style="text-align:center;padding:10px;margin-bottom:10px;background:#ccc;cursor:pointer;">关闭</h2>
            <h3 style="text-align:center;padding:10px;margin-bottom:10px;"></h3>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">码量起始控制值:</label>
                <div class="col-sm-6">
                    <asp:TextBox ID="m_startEarn" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
                <div class="col-sm-4">
                    <label class="col-sm-offset-2 col-sm-10">码量起始控制值为非负值</label>
                </div> 
            </div>
            <div class="col-sm-offset-2 col-sm-10">
                <p>
                    以下值为千分值，只能填整数<br/>
                    <span style="color:red">且必须满足条件：最大盈利率 > 控制赢利率大 > 期望赢利率 > 控制赢利率小 > 最小赢利率</span>
                </p>
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">最大盈利率:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_maxEarn" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div> 
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">控制盈利率大:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_maxControlEarn" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div> 
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">期望盈利率:</label>
                <div class="col-sm-6">
                    <asp:TextBox ID="m_expEarn" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
                <div class="col-sm-4">
                    <label class="col-sm-offset-2 col-sm-10">介于最小盈利率与最大盈利率之间</label>
                </div> 
            </div>  
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">控制盈利率小:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_minControlEarn" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div> 
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">最小盈利率:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_minEarn" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div> 

            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                     <input type="button" value="提交修改"  class="btn btn-primary form-control" id="btnModifyParam" />
            </div>
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                     <span id="opRes"></span>
            </div>
        </div>
        </div>
        </div>
    </div>
</asp:Content>
