<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ServiceMainQuery.aspx.cs" Inherits="WebManager.appaspx.service.ServiceMainQuery" %>
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
    <div class="container form-horizontal" style="text-align:center">
        <h2 style="padding:20px;">邮件查询</h2>
        邮件状态：<asp:DropDownList ID="m_isDel" runat="server" style="width:150px;height:35px"></asp:DropDownList>&ensp;&ensp;
        <asp:DropDownList ID="m_queryWay" runat="server" style="width:110px;height:35px"></asp:DropDownList>
        <asp:TextBox ID="m_param" runat="server" style="width:200px;height:35px;" ></asp:TextBox>&ensp;&ensp;
        时间：<asp:TextBox ID="m_time" runat="server" style="width:180px;height:35px;" ></asp:TextBox>&ensp;&ensp;
        <asp:Button ID="btn" CssClass="btn btn-primary" runat="server" OnClick="onQueryMail" Text="查询" style="width:133px;height:35px;"/>
    </div>
    <br />
    <div class="container-fluid">
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center"></asp:Table>
        <br />
        <span id="m_page" style="text-align:center;display:block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
    </div>
</asp:Content>
