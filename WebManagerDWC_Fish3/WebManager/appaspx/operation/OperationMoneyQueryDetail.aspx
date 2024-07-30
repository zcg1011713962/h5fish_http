<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="OperationMoneyQueryDetail.aspx.cs" Inherits="WebManager.appaspx.operation.OperationMoneyQueryDetail" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
	<script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
	<script type="text/javascript">
        $(function () {
            $('#__time__').daterangepicker();
        });
	</script>
    <style type="text/css">
        .cSafeWidth td{padding:5px;}
        .cSafeWidth input[type=submit]{margin-right:10px;}
        .form-control{height:35px;width:240px;}
        .table {text-align:center;}
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div  class="cSafeWidth">
    <h2 style="padding:20px;text-align:center">玩家金币变化详细</h2>
    <table style="margin:0 auto;">
        <tr>
            <td>
                查询方式：
            </td>
            <td>
                <asp:DropDownList ID="m_queryWay" runat="server" CssClass="form-control"></asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="m_param" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>所在游戏：</td>
            <td>
                <asp:DropDownList ID="m_whichGame" runat="server" CssClass="form-control"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>变化原因：</td>
            <td>
                <div>
                        <input id="m_filter1" name="filter1" list="MainContent_m_filter_list" runat="server" onfocus="value=''" onclick="value =''" 
                                    autocomplete="off"  style="height:35px;width:240px;padding-left:15px;" />
                        <datalist id = "m_filter_list" runat = "server" >
                        </datalist>              
                </div>
            </td>
        </tr>
        <%--<tr>
            <td>变化原因:</td>
            <td>
                <asp:DropDownList ID="m_filter" runat="server" CssClass="form-control"></asp:DropDownList>
            </td>
        </tr>--%>
        <tr>
            <td>属性：</td>
            <td>
                <asp:DropDownList ID="m_property" runat="server" CssClass="form-control"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>时间：</td>
            <td>
                <asp:TextBox ID="__time__" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="Button3" runat="server" onclick="onQuery" Text="查询"  CssClass="btn btn-primary"/>
                <asp:Button ID="Button1" runat="server" onclick="onExport" Text="导出Excel" CssClass="btn btn-primary" />
            </td>
        </tr>
        <tr>
            <td colspan="3"><span id="m_res" style="font-size:medium;color:red" runat="server"></span></td>
        </tr>
    </table>
    <br />
    </div>
    <div class="container-fluid"><asp:Table ID="m_result" runat="server" CssClass="table table-striped table-bordered">
    </asp:Table></div>
    <br />
    <span id="m_page" style="text-align:center;display:block" runat="server"></span>
    <br />
    <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
</asp:Content>
