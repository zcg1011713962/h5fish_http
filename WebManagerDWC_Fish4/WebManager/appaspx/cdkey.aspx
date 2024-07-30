<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="cdkey.aspx.cs" Inherits="WebManager.appaspx.cdkey" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="../Scripts/module/sea.js"></script>
    <script type="text/javascript">
        seajs.use("../Scripts/cdkey.js");
	</script>
    <style type="text/css">
        .OP{margin-top:50px;}
        .OP>div{display:none;}
        #ModifyPici{display:none;position:absolute;width:100%;height:100%;background:rgba(0,0,0,0.7);left:0;top:0;
                    padding-top:200px;
        }
        #ModifyPici a{display:block;background:#ddd;font-size:18px;text-align:center;margin-top:20px;}
        .OpModifyPici{width:800px; background:rgb(255,255,255);padding-bottom:20px}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="clearfix" style="width:420px;margin:10px auto;">
        <ul class="SelCard">
            <li class="Active" data="op1">生成批次</li><li data="op2">批次查询</li><li data="op3">cdkey查询</li>
        </ul>
    </div>

    <div class="OP">
        <div id="op1" role="form" class="container" style="display:block;">
            <div class="form-group">
                <label for="pici">批次号:</label>
                <input type="text" class="form-control" id="pici" placeholder="" name="pici" disabled>
            </div>
            <div class="form-group">
                <label for="validDays">失效日期(格式如 2016/12/12):</label>
                <input type="text" class="form-control" id="validDays" placeholder="" name="validDays">
            </div>
            <div class="form-group">
                <label for="cdkeyCount">生成cdkey数量:</label>
                <input type="text" class="form-control" id="cdkeyCount" placeholder="" name="cdkeyCount"/>
            </div>
            <div class="form-group">
                <label for="giftList">对应礼包:</label>
                <select id="giftList" name="giftList" class="form-control">
                </select>
            </div>
            <button type="button" class="btn btn-default" id="btnSubmit">提交</button>
            <label class="text-info" id="infoForGenPici"></label>
        </div>
        
        <div id="op2" class="container-fluid">
            <table class="table table-hover table-bordered" id="tablePici"></table>
        </div>

        <div id="op3" class="container">
            <div class="form-group">
                <label for="cdkeyNum">cdkey:</label>
                <input type="text" class="form-control" id="cdkeyNum" placeholder="" name="cdkeyNum"/>
            </div>
            <button type="button" class="btn btn-primary" id="btnQueryCDKEY">查询</button>
            <table class="table table-hover table-bordered" id="tableCDKEY" style="margin-top:10px;"></table>
        </div>
    </div>

    <div id="ModifyPici" style="z-index:2000;">
        <div class="container OpModifyPici">
            <a href="javascript:;">关闭</a>
            <h2 style="text-align:center;padding:10px;">修改批次信息</h2>
            <div class="form-group">
                <label for="piciM">批次号:</label>
                <input type="text" class="form-control" id="piciM" placeholder="" name="piciM" disabled/>
            </div>
            <div class="form-group">
                <label for="validDaysM">失效日期(格式如 2016/12/12):</label>
                <input type="text" class="form-control" id="validDaysM" placeholder="" name="validDaysM">
            </div>
            <div class="form-group">
                <label for="giftListM">对应礼包:</label>
                <select id="giftListM" name="giftListM" class="form-control">
                </select>
            </div>
            <button type="button" class="btn btn-primary form-control" id="btnModify">提交修改</button>
            <label class="text-info" id="txtInfoModify"></label>
        </div>
    </div>
</asp:Content>
