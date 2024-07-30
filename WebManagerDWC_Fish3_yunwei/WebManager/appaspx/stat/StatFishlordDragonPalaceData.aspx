<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatFishlordDragonPalaceData.aspx.cs" Inherits="WebManager.appaspx.stat.StatFishlordDragonPalaceData" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
            changeItemType();
        });
        function changeItemType()
        {
            var index = $("#MainContent_m_item").prop("selectedIndex");
            if (index != 0) {
                delOptionValue("MainContent_m_roomId", "0");
            } else
            {
                addOptionValue("MainContent_m_roomId", "0");
            }
        }

        //增加select项
        function addOptionValue(id, value)
        {
            if (!isExistOption(id, value)) {
                $('#' + id).append("<option value=" + value + "> 捕鱼大厅</option");
            }
        }

        //删除select项
        function delOptionValue(id, value)
        {
            if (isExistOption(id, value)) {
                $("#" + id + "option[value=" + value + "]").remove();
            }
        }

        function isExistOption(id, value) {
            var isExist = false;
            var count = $('#' + id).find('option').length;

            for (var i = 0; i < count; i++) {
                if ($('#' + id).get(0).options[i].value == value) {
                    isExist = true;
                    break;
                }
            }
            return isExist;
        }
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">龙宫场数据统计</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label" >统计项目：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_item" runat="server" CssClass="form-control" onchange="changeItemType()"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label" >场次选择：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_roomId" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label" >时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Button ID="btn_stat" runat="server" Text="统计" CssClass="btn btn-primary  form-control" OnClick="OnStat"/>
            </div>
        </div>
    </div>
    <div class="table-responsive" style="width:99%;margin:0 auto;text-align:center;">
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="margin:0 auto"></asp:Table>
    </div>
</asp:Content>
