<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatMiddleRoomExchange.aspx.cs" Inherits="WebManager.appaspx.stat.StatMiddleRoomExchange" %>
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
        <h2 style="text-align:center;padding:20px;">中级场礼包统计</h2>

       <%-- <div class="form-group">
            <label for="account" class="col-sm-2 control-label">礼包类型：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_giftId" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>--%> 
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">查询时间：</label>
            <div class="col-sm-10" style="display:inline-block">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control" style="display:inline-block;height:35px;"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label for="account" class="col-sm-2 control-label"></label>
            <div class="col-sm-10">
                <asp:Button ID="Button1" runat="server" Text="确定" onclick="onQuery" CssClass="btn btn-primary" style="width:100%;display:inline-block"/>
            </div>
        </div> 
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center"></asp:Table>
        <br /><br />  
    </div> 
</asp:Content>
