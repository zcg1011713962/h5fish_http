<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OperationActivityCfg.aspx.cs" Inherits="WebManager.appaspx.operation.OperationActivityCfg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"> 
    <style type="text/css">
        .OpModify {
            width: 800px;
            background: rgb(255,255,255);
            padding-bottom: 20px;
        }
    </style>   
    <script src="../../Scripts/module/sea.js"></script>
    <script type="text/javascript">
        seajs.use("../../Scripts/operation/OperationActivityCFG.js");
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">活动配置表</h2>
         <div class="table-responsive" style="margin-top:10px;text-align:center">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
         </div>
      </div>
    <div id="divModifyNewParam" class="PopDialogBg">
        <div class="container form-horizontal OpModify">
            <h2 style="text-align: center; padding: 10px; margin-bottom: 10px; background: #ccc; cursor: pointer;">关闭</h2>
            <h3 style="text-align: center; padding: 10px; margin-bottom: 10px;"></h3>
            <div id="roomId_3">
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">开始时间：</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="m_startTime" runat="server" CssClass="form-control" ClientIDMode="Static" style="display:inline;width:350px;"></asp:TextBox>
                        <span>&ensp;&ensp;时间格式：2020-06-07 00:00:00</span>
                    </div>
                </div>

                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">结束时间:</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="m_endTime" runat="server" CssClass="form-control" ClientIDMode="Static" style="display:inline;width:350px;"></asp:TextBox>
                        <span>&ensp;&ensp;时间格式：2020-06-07 00:00:00</span>
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label"></label>
                    <div class="col-sm-10">
                        <input type="button" value="提交修改" class="btn btn-primary form-control" id="btnModifyParam" style="width:359px;"/>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <span id="opRes" style="color: red"></span>
                    </div>
                </div>
            </div>
         </div>
     </div>
</asp:Content>
