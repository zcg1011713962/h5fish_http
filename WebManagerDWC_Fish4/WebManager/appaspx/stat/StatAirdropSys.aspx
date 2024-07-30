<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatAirdropSys.aspx.cs" Inherits="WebManager.appaspx.stat.StatAirdropSys" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../Scripts/module/sea.js"></script>
    <script type="text/javascript">
        seajs.use("../../Scripts/stat/StatAirdropSysCtrl.js");
        $(function () {
            selectOpType();
        });

        function selectOpType()
        {
            var type = $("#MainContent_m_type").prop("selectedIndex");
            if (type == 1)
            {
                $("#MainContent_div_timeState").show();
            } else {
                $("#MainContent_div_timeState").hide();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding: 20px; text-align: center">系统空投</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">空投ID:</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_uuid" runat="server" CssClass="form-control" Style="width: 60%; display: inline-block"></asp:TextBox>
                <span>&ensp;（注：空投ID大于900000，且不可重复）</span>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">空投道具：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_itemId" runat="server" CssClass="form-control" Style="width: 55%; display: inline-block"></asp:TextBox>
                <span>&ensp;（注：只能放置一种道具，道具数量上限100，格式： 道具ID : 数量）</span>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">空投密码：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_pwd" runat="server" CssClass="form-control" Style="width: 60%; display: inline-block"></asp:TextBox>
                <span>&ensp;（注：6-10个位长数字字母）</span>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Button ID="btn_confirm" runat="server" Text="发布" CssClass="btn btn-primary form-control" OnClick="onPublish" Style="width: 60%; display: inline-block" />
                <span id="m_res" style="font-size: medium; color: red" runat="server"></span>
            </div>
        </div>
        <hr />
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">道具状态：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_state" runat="server" CssClass="form-control" Style="width: 60%; display: inline-block"></asp:DropDownList>
            </div>
        </div>

        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">空投类型：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_airDropType" runat="server" CssClass="form-control" Style="width: 60%; display: inline-block"></asp:DropDownList>
            </div>
        </div>

        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">玩家ID:</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_playerId" runat="server" CssClass="form-control" Style="width: 60%; display: inline-block"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">查询内容：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_type" runat="server" CssClass="form-control" Style="width: 60%; display: inline-block" onchange="selectOpType()" ></asp:DropDownList>
            </div>
        </div>
        <div class="form-group" id="div_timeState" runat="server" style="display:none">
            <label for="account" class="col-sm-2 control-label">过期状态：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_timeState" runat="server" CssClass="form-control" Style="width: 60%; display: inline-block"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Button ID="btn_query" runat="server" Text="查询" CssClass="btn btn-primary form-control" OnClick="onQuery" Style="width: 60%" />
                <span id="m_res1" style="font-size: medium; color: red" runat="server"></span>
            </div>
        </div>
        <br />
        <br />
        <div class="table-responsive" style="margin-top: 10px; text-align: center">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
        </div>
        <br />
        <span id="m_page" style="text-align: center; display: block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size: x-large; text-align: center; display: block" runat="server"></span>
    </div>
</asp:Content>
