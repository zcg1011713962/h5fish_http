<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatFishlordSharkRoomAct.aspx.cs" Inherits="WebManager.appaspx.stat.StatFishlordSharkRoomAct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
	<script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
            changeTimeType();
        });

        function changeTimeType()
        {
            var index = $("#MainContent_m_queryType").prop("selectedIndex");
            if (index == 3) //排行榜
            {
                $("#div_type").show();
                changeRankType();
            }
            else
            {
                $("#div_type").hide();
                $("#div_time").show();
            }
        }

        function changeRankType()
        {
            var index = $("#MainContent_m_type").prop("selectedIndex");

            // 0当前排行   1历史
            if (index == 0)
            {
                $("#div_time").hide();
            } else
            {
                $("#div_time").show();
            }
        }

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">巨鲨场功能统计</h2>

        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">查询内容：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_queryType" runat="server" CssClass="form-control" onchange="changeTimeType()"></asp:DropDownList>
            </div>
        </div>

        <div class="form-group" id="div_type">
            <label for="account" class="col-sm-2 control-label">排行类型：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_type" runat="server" CssClass="form-control" onchange="changeRankType()"></asp:DropDownList>
            </div>
        </div>

         <div class="form-group" id="div_time">
            <label for="account" class="col-sm-2 control-label">查询时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <asp:Button ID="Button1" runat="server" Text="确定" onclick="onQuery" CssClass="btn btn-primary" style="width:90%;margin-left:10%"/>
        <br/><br/>
      </div>
     <div class="table-responsive" style="margin-top:10px;text-align:center;width:70%;margin:0 auto">
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
    </div>
</asp:Content>
