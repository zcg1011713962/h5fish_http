<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OperationActivityCFGNew.aspx.cs" Inherits="WebManager.appaspx.operation.OperationActivityCFGNew" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .OpModify {
            width: 550px;
            background: rgb(255,255,255);
            padding-bottom: 20px;
        }
    </style>  
    
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script src="../../Scripts/module/sea.js"></script>

    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
	<script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
        });
        seajs.use("../../Scripts/operation/OperationActivityCFGNew.js");
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="container form-horizontal" style="text-align:center; width:40%">
        <h2 style="padding:30px;text-align:center">活动时间设置</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">活动：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_actList" runat="server" CssClass="form-control" style="display:inline-block;"></asp:DropDownList>
            </div>
        </div>
         <div class="form-group">
            <label for="account" class="col-sm-2 control-label">日期：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_monthday" runat="server" CssClass="form-control" style="display:inline-block;"></asp:TextBox>
                <span style="display:inline-block;">【开始日期，结束日期】格式：2020-06-07, 2020-08-07</span>
            </div>
        </div>
         <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Button ID="btnStat" runat="server" Text="保存" CssClass="btn btn-primary form-control" OnClick="onEdit" style="display:inline;width:40%"/>
                <span id="m_res" style="font-size: medium; color: red; display:inline-block; width:59%;" runat="server"></span>
            </div>
        </div>
         <hr />
         <h2 style="padding:20px;text-align:center">活动时间查询</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">活动：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_actList1" runat="server" CssClass="form-control" style="display:inline-block;"></asp:DropDownList>
            </div>
        </div>
         <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Button ID="Button1" runat="server" Text="查询" CssClass="btn btn-primary form-control" OnClick="onQuery" style="display:inline;"/>
            </div>
        </div>

        <br />
         <div style="margin-top:10px;text-align:center">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="margin-left:5%"></asp:Table>
         </div>
         <span id="m_page" style="text-align:center;display:block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
      </div>
    <div id="divModifyNewParam" class="PopDialogBg">
        <div class="container form-horizontal OpModify">
            <h2 style="text-align: center; padding: 10px; margin-bottom: 10px; background: #ccc; cursor: pointer;">关闭</h2>
            <h3 style="text-align: center; padding: 10px; margin-bottom: 10px;">修改活动日期</h3>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">日期：</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="m_monday" runat="server" CssClass="form-control" ClientIDMode="Static" style="width:250px;display:inline"></asp:TextBox>
                        <span>日期格式：2020-06-07</span>
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label"></label>
                    <div class="col-sm-10">
                        <input type="button" value="提交修改" class="btn btn-primary form-control" id="btnModifyParam" style="width:250px;"/>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <span id="opRes" style="color: red"></span>
                    </div>
                </div>
            </div>
     </div>
</asp:Content>
