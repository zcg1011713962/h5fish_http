<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OperationPlayerRichesRank.aspx.cs" Inherits="WebManager.appaspx.operation.OperationPlayerRichesRank" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script src="../../Scripts/module/sea.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#time').daterangepicker();
        });
        seajs.use("../../Scripts/operation/OperationPlayerRichesRank.js");
	</script>
    <style type="text/css">
        .SearchCondition .form-control {
            height: 35px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 style="text-align: center; padding: 20px;">玩家财富榜</h2>
        <table class="SearchCondition" style="margin: 0 auto;">
            <tr>
                <td>榜单类型：</td>
                <td>
                    <select id="rankSel" class="form-control">
                        <option value="0">金币榜</option>
                        <option value="1">龙珠榜</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>时间：</td>
                <td>
                    <input type="text" id="time" class="form-control" /></td>
                <td>
                    <input type="button" id="btnQuery" value="查询" class="btn btn-primary" /></td>
            </tr>
        </table>
    </div>
    <div id="resultDiv" class="container"></div>
    <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered">
    </asp:Table>
</asp:Content>
