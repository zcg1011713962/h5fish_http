<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatIncomeExpensesError.aspx.cs" Inherits="WebManager.appaspx.stat.StatIncomeExpensesError" %>
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
    <h2 style="text-align:center;padding:20px;">收支错误查询</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">时间:</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group" id="room">
            <label for="account" class="col-sm-2 control-label">错误类型：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_showWay" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                 <asp:Button ID="Button1" runat="server" Text="查询" CssClass="btn btn-primary form-control" onclick="onQueryError"  ClientIDMode="Static"/>
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center;"></asp:Table>
            </div>
        </div>
    
    </div>
        <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
    <br />
    <br />
    <span id="m_page" style="text-align:center;display:block" runat="server"></span>
    <br />
    <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
</asp:Content>

