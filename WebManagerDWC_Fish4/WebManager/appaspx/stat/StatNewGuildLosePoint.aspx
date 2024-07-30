<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatNewGuildLosePoint.aspx.cs" Inherits="WebManager.appaspx.stat.StatNewGuildLosePoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
	<script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div  style="margin:0 auto;text-align:center">
        <div class="cSafeWidth">
            <h2 style="text-align:center;padding:20px;">流失点统计（百分比）</h2>

            <div class="form-group" style="width:80%">
                <label for="account" class="col-sm-2" style="width:100px;clear:both">查询时间：</label>
                <div class="col-sm-10" style="width:400px;display:inline-block">
                    <asp:TextBox ID="m_time" runat="server" CssClass="form-control" style="display:inline-block;width:380px;height:35px;"></asp:TextBox>
                </div>
                <asp:Button ID="Button1" runat="server" Text="确定" onclick="onQuery" CssClass="btn btn-primary" style="width:30%;display:inline-block"/>
            </div>
        </div>
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="width:99%;margin:0 auto"></asp:Table>
            <br /><br />  
    </div> 
</asp:Content>
