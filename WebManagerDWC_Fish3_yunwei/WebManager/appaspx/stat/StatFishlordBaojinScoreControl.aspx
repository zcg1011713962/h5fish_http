<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatFishlordBaojinScoreControl.aspx.cs" Inherits="WebManager.appaspx.stat.StatFishlordBaojinScoreControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .OpModify{width:550px; background:rgb(255,255,255);padding-top:20px;text-align:center}
    </style>
    <script src="../../Scripts/module/sea.js"></script>
    <script>
        seajs.use("../../Scripts/stat/FishlordBaojinScoreAlter.js");
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal" style="padding:20px;text-align:center">
        <h2 style="text-align:center;margin-bottom:20px;">修改竞技场得分</h2>
            <h2 style="height:40px;line-height:40px;background-color:#ccc;padding:0 50px;">
                修改竞技场得分提示：
                修改得分时，需要先下线。等待超过7分钟再上线，才能确保服务器的数据不会覆盖修改后的数据。
            </h2>
        <br />
        <br />
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">玩家ID：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_playerId" runat="server" CssClass="form-control" style="height:30px;"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                 <asp:Button ID="btnStat" runat="server" Text="查询" CssClass="btn btn-primary form-control" OnClick="onQuery"/>
            </div>
        </div>
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
        <br />
        <div id="alterScore" runat="server" style="text-align:center;">
            <span style="font-weight:bold;">修改玩家竞技场得分：</span>
            <input id="m_param" type="text" style="width:200px;height:31px;"/>
            <input id="btnConfirm" type="button" class="btn btn-primary form-control" value="确定" style="width:125px;"/>
            <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        </div>
        <br /><br /><hr /><br />
        <h2 style="text-align:center;margin-bottom:20px;">
            机器人最高积分：&ensp;
            <asp:Label ID="m_count" runat="server" Enabled="false" style="width:300px;height:30px;font-size:20px;color:red;" ></asp:Label>
        </h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">修改积分：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_value" runat="server" CssClass="form-control" style="height:30px;"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                 <asp:Button ID="Button1" runat="server" Text="修改" CssClass="btn btn-primary form-control" OnClick="onEdit"/>
            </div>
            <span id="m_resEdit" style="font-size:medium;color:red" runat="server"></span>
        </div>
   </div>

    <div id="divModifyNewParam" class="PopDialogBg">
        <div class="container form-horizontal OpModify">
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                    <h2 id="paramTips"></h2>
                </div>
            </div>
            <br />
            <div class="form-group" style="display:inline-block">
                <div class="col-sm-offset-2 col-sm-10">
                     <input type="button" value="取消" style="width:100px;" class="btn btn-default form-control" id="btnCancel" />
                </div>
            </div>
            <div class="form-group" style="display:inline-block">
                <div class="col-sm-offset-2 col-sm-10">
                     <input type="button" value="确定" style="width:100px;" class="btn btn-primary form-control" id="btnModifyParam" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
