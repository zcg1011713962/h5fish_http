<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatAirdropSys.aspx.cs" Inherits="WebManager.appaspx.stat.StatAirdropSys" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../Scripts/module/sea.js"></script>
     <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script type="text/javascript">
        seajs.use("../../Scripts/stat/StatAirdropSysCtrl.js");

        $(function () {
            $('#MainContent_m_time').daterangepicker();
            selectOpType();
            selectOpPlayer();
        });

        function selectOpType() {
            var type = $("#MainContent_m_type").prop("selectedIndex"); // 发布0 打开1

            if (type == 1) {
                $("#MainContent_div_timeState").show();
                $("#div_time").show();
            } else {
                $("#MainContent_div_timeState").hide();
                $("#div_time").hide();
            }
        }

        function selectOpPlayer() //玩家 系统
        {
            var player = $("#MainContent_m_airDropType").prop("selectedIndex");
            if (player == 1) 
            {
                $("#MainContent_m_type").prop("selectedIndex", 0);
                $("#MainContent_m_type").attr("disabled", true);
                $("#div_time").hide();
                $("#div_player").hide();
            } else
            {
                $("#MainContent_m_type").attr("disabled", false);
                $("#div_player").show();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding: 20px; text-align: center">系统空投</h2>
        
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">道具名称：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_state" runat="server" CssClass="form-control" Style="width: 60%; display: inline-block"></asp:DropDownList>
            </div>
        </div>

        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">空投类型：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_airDropType" runat="server" CssClass="form-control" Style="width: 60%; display: inline-block" onchange="selectOpPlayer()"></asp:DropDownList>
            </div>
        </div>

        <div class="form-group" id="div_player">
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

        <div class="form-group" id="div_time">
            <label for="account" class="col-sm-2 control-label">日期：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control" Style="width: 60%; display: inline-block"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Button ID="btn_query" runat="server" Text="查询" CssClass="btn btn-primary form-control" OnClick="onQuery" Style="width: 60%" />
            </div>
        </div>
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
