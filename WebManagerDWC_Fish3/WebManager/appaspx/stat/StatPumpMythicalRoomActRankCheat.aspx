<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatPumpMythicalRoomActRankCheat.aspx.cs" Inherits="WebManager.appaspx.stat.StatPumpMythicalRoomActRankCheat" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .OpModify {
            width: 550px;
            background: rgb(255,255,255);
            padding-top: 20px;
            text-align: center;
        }
    </style>
    <script src="../../Scripts/module/sea.js"></script>
    <script>
        seajs.use("../../Scripts/stat/StatPumpMythicalRankCheat.js");
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal" style="padding: 20px; text-align: center">
        <h2 style="text-align: center; margin-bottom: 20px;">修改圣兽场排行</h2>
        <h2 style="height: 40px; line-height: 40px; background-color: #ccc; padding: 0 50px;">修改积分提示：
                修改积分时，需要先下线。等待超过7分钟再上线，才能确保服务器的数据不会覆盖修改后的数据。
        </h2>
        <br />
        <br />
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">玩家ID：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_playerId" runat="server" CssClass="form-control" Style="height: 30px;width:70%;display:inline-block"></asp:TextBox>
                <span>&ensp;机器人区间：[10099001，10099200]</span>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Button ID="btnStat" runat="server" Text="查询" CssClass="btn btn-primary form-control" OnClick="onQuery" />
            </div>
        </div>
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
        <hr />
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">玩家昵称：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_nickName" runat="server" CssClass="form-control" style="width:78%;display:inline-block"></asp:TextBox>
                <span>&ensp;注：机器人首次作弊需输入标志</span>
            </div>
        </div>
        <div id="alterScore" runat="server" style="text-align: center;">
            <span style="font-weight: bold;" class="col-sm-2 control-label">修改玩家积分数：</span>
            <input id="m_param" class="col-sm-10" type="text" style="width: 60%; height: 31px;margin-left:20px;"/>
            <input id="btnConfirm" type="button" class="btn btn-primary form-control" value="确定" style="width: 225px;" />
            <span id="m_res" style="font-size: medium; color: red" runat="server"></span>
        </div>
    </div>
</asp:Content>
