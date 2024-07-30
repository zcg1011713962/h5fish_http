<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatKdActivityRank.aspx.cs" Inherits="WebManager.appaspx.stat.StatKdActivityRank" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            changeTimeType();
        });

        function changeTimeType() {
            var index = $("#MainContent_m_queryType").prop("selectedIndex");
            if (index == 0) {
                $(".time_type_div").show();
                changeDayType();
            }else
            {
                $(".time_type_div").hide();
                $(".time_div").hide();
            }
        }
        function changeDayType() {
            var index_time = $("#MainContent_m_timeType").prop("selectedIndex");
            if (index_time == 0) {
                $(".time_div").hide();
            } else {
                $(".time_div").show();
            }
        }
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">屠龙榜</h2>
        
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">查询内容：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_queryType" runat="server" CssClass="form-control" onchange="changeTimeType()"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group time_type_div">
            <label for="account" class="col-sm-2 control-label">查询时间：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_timeType" runat="server" CssClass="form-control" onchange="changeDayType()"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group time_div">
            <label for="account" class="col-sm-2 control-label"></label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control" placeHolder ="日期格式：2019/06/06"></asp:TextBox>
            </div>
        </div>
        <asp:Button ID="Button1" runat="server" Text="确定" onclick="onQuery" CssClass="btn btn-primary" style="width:90%;margin-left:10%"/>
        <br/><br/>
         <div class="table-responsive" style="margin-top:10px;text-align:center">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
         </div>
      </div>
</asp:Content>
