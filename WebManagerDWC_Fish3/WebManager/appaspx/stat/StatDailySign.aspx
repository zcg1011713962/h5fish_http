<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatDailySign.aspx.cs" Inherits="WebManager.appaspx.stat.StatDailySign" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#MainContent_m_time').daterangepicker();
	    });
	</script>
    <script src="../../Scripts/stat/highcharts.js" type="text/javascript"></script>
    <script src="../../Scripts/module/sea.js" type="text/javascript"></script>
    <script type="text/javascript">
        seajs.use("../../Scripts/stat/StatSignByMonth.js");
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">每日签到</h2>
         <div class="form-group">
            <label for="account" class="col-sm-2 control-label">时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <asp:Button ID="Button1" runat="server" Text="查询" onclick="onQuery" CssClass="btn btn-primary" style="width:90%;margin-left:10%"/><br/><br/>

         <div class="container" style="margin-top:10px;text-align:center">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
         </div>
      </div>

    <h2 style="padding:20px;text-align:center">签到情况分布表（散点折线图）</h2>
    <div style="margin-left:18%;width:62%">
        <div class="form-group" style="text-align:right">
            <label for="account" class="col-sm-2 control-label">年份：</label>
            <div class="col-sm-10">
                <input type="text" id="m_year" class="form-control"/>
            </div>
        </div>
        <br /><br /> <br />
        <div class="form-group" style="text-align:right">
            <label for="account" class="col-sm-2 control-label">月份：</label>
            <div class="col-sm-10">
                <select id="m_month" class="form-control" >
                    <option value="1">1月</option>
                    <option value="2">2月</option>
                    <option value="3">3月</option>
                    <option value="4">4月</option>
                    <option value="5">5月</option>
                    <option value="6">6月</option>
                    <option value="7">7月</option>
                    <option value="8">8月</option>
                    <option value="9">9月</option>
                    <option value="10">10月</option>
                    <option value="11">11月</option>
                    <option value="12">12月</option>
                </select>
            </div>
        </div>
        <br /><br />
        <div class="form-group" style="text-align:right">
            <label for="account" class="col-sm-2 control-label"></label>
            <div class="col-sm-10">
                <input type="button" id="onQueryMonth" value="查询" class="form-control btn-primary" style="text-align:center" />
            </div>
        </div>
        <div id="divTemplate" style="display:none;margin-left:10%;">
            <div id="{0}" style="max-width:1200px;min-height:400px; margin:10px auto;border:1px solid #000;border-radius:10px;padding:10px;"></div>
        </div>
        <div id="divContent" style="padding:20px 10px;margin-left:10%;"></div>
    </div>
</asp:Content>
