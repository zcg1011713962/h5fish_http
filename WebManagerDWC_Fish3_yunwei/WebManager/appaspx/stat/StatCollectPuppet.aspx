﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatCollectPuppet.aspx.cs" Inherits="WebManager.appaspx.stat.StatCollectPuppet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
            var index = $("#MainContent_m_queryType").prop("selectedIndex");
            if (index == 4 || index == 5) {
                $("#setTime").hide();
            } else
            {
                $("#activityType").hide();
            }
        });
        function hiddenTime() {
            var index=$("#MainContent_m_queryType").prop("selectedIndex");
            if ( index==4||index==5) {
                $("#setTime").hide();
                $("#activityType").show();
            }
            else {
                $("#setTime").show();
                $("#activityType").hide();
            }
        }
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">集玩偶活动基础数据</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">查询内容：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_queryType" runat="server" CssClass="form-control" onchange="hiddenTime()"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group" id="setTime">
            <label for="account" class="col-sm-2 control-label">时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control" style="height:30px;"></asp:TextBox>
            </div>
        </div>
        <div class="form-group" id="activityType">
            <label for="account" class="col-sm-2 control-label">活动属性：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_type" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                 <asp:Button ID="btnStat" runat="server" Text="统计" CssClass="btn btn-primary form-control" OnClick="onQuery"/>
            </div>
        </div>
        <br />
        <div style="padding:0 1px;">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center"></asp:Table>
        </div>
        <br />
        <br />
        <div id="footPage">
            <span id="m_page" style="text-align:center;display:block" runat="server"></span>
            <br />
            <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
        </div>
    </div>
</asp:Content>
