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
    <div class="container form-horizontal">
        <h2 style="text-align:center;padding:20px;">流失点统计（百分比）</h2>
        <div class="form-group">
           <label for="account" class="col-sm-2 control-label">渠道：</label>
             <div class="col-sm-10">
                    <asp:DropDownList ID="m_channel" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">查询时间：</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <asp:Button ID="Button1" runat="server" Text="确定" onclick="onQuery" CssClass="btn btn-primary" style="width:83%;margin-left: 17%"/>
    </div> 
    <br />
    <br />
    <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="width:98%;margin:0 auto"></asp:Table>
</asp:Content>
