<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatFishlordAdvancedRoomAct.aspx.cs" Inherits="WebManager.appaspx.stat.StatFishlordAdvancedRoomAct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
            changeTimeType();
        });
        function changeTimeType() {
            var index = $("#MainContent_m_queryType").prop("selectedIndex");
            if (index == 0) //排行榜
            {
                $("#div_rankType").show();

                var index_rank = $("#MainContent_m_rankType").prop("selectedIndex");
                if (index_rank == 0) { //当前
                    $("#div_time").hide();
                } else {
                    $("#div_time").show();
                }
            }
            else {
                $("#div_rankType").hide();
                $("#div_time").show();
            }
        }
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding: 20px; text-align: center">高级场奖池统计</h2>

        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">查询内容：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_queryType" runat="server" CssClass="form-control" onchange="changeTimeType()"></asp:DropDownList>
            </div>
        </div>

        <div class="form-group" id="div_rankType">
            <label for="account" class="col-sm-2 control-label">排行类型：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_rankType" runat="server" CssClass="form-control" onchange="changeTimeType()"></asp:DropDownList>
            </div>
        </div>

        <div class="form-group" id="div_time">
            <label for="account" class="col-sm-2 control-label">查询时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <asp:Button ID="Button1" runat="server" Text="确定" OnClick="onQuery" CssClass="btn btn-primary" Style="width: 90%; margin-left: 10%" />
        <br />
        <br />
        <div class="table-responsive" style="margin-top: 10px; text-align: center">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
        </div>
    </div>
</asp:Content>
