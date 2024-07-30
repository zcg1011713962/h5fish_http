<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ServiceRechargeQueryNew.aspx.cs" Inherits="WebManager.appaspx.service.ServiceRechargeQueryNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>

    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
        });
	</script>
    <style type="text/css">
        .cSafeWidth td {
            padding: 6px;
        }

        .cSafeWidth tr td:nth-of-type(1) {
            text-align: right;
        }

        .cSafeWidth input[type=text], .cSafeWidth select {
            width: 220px;
            height: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth">
        <h2 style="text-align: center; padding: 20px;">充值记录查询</h2>
        <table style="margin: 0 auto">
            <tr>
                <td>查询方式：</td>
                <td>
                    <asp:DropDownList ID="m_queryWay" runat="server"></asp:DropDownList>
                    &ensp;查询参数：
                    <asp:TextBox ID="m_param" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>渠道：</td>
                <td>
                    <asp:DropDownList ID="m_channel" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>充值结果：</td>
                <td>
                    <asp:DropDownList ID="m_rechargeResult" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>付费点：</td>
                <td>
                    <input id="m_rechargePoint" name="filter1" list="MainContent_m_rechargePoint_list" runat="server" onfocus="value=''" onclick="value = ''"
                        autocomplete="off" style="height: 35px; width: 220px; padding-left: 15px;" />
                        <datalist id = "m_rechargePoint_list" runat = "server">
                        </datalist>       
                    <%--<asp:DropDownList ID="m_rechargePoint" runat="server"></asp:DropDownList>--%>           
                </td>
            </tr>
            <tr>
                <td>充值范围：</td>
                <td>
                    <asp:TextBox ID="m_range" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>时间：</td>
                <td>
                    <asp:TextBox ID="m_time" runat="server"></asp:TextBox>
                    <asp:Button ID="Button1" runat="server" OnClick="onQueryRecharge" Text="查询" Style="width: 100px; height: 30px" />
                    <asp:Button ID="Button2" runat="server" OnClick="onStatRecharge" Text="统计" Style="width: 100px; height: 30px" />
                    <asp:Button ID="Button3" runat="server" OnClick="onExport" Text="导出Excel" Style="width: 100px; height: 30px" />
                    <asp:Button ID="btn" runat="server" OnClick="onQueryByAibei" Text="通过爱贝充值查询" Style="width: 130px; height: 30px;" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td><span id="m_res" style="font-size: medium; color: red" runat="server"></span></td>
            </tr>
        </table>
    </div>

    <%--  游戏服务器:<asp:DropDownList ID="m_gameServer" runat="server" style="width:130px;height:30px"></asp:DropDownList> --%>

    <%-- <asp:Button ID="Button4" runat="server" onclick="onSameOrder"  Text="统计相同的订单" style="width:100px;height:30px" /> --%>
    <div class="container-fluid">
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center"></asp:Table>
    </div>

    <br />
    <br />
    <span id="m_page" style="text-align: center; display: block" runat="server"></span>
    <br />
    <span id="m_foot" style="font-size: x-large; text-align: center; display: block" runat="server"></span>
</asp:Content>
