<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ServiceReplacementOrder.aspx.cs" Inherits="WebManager.appaspx.service.ServiceReplacementOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <style>
        td:nth-of-type(odd) {
            min-width: 95px;
        }

        td:nth-of-type(even) {
            min-width: 120px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
            $("#MainContent_m_item").prop("disabled", false);
        });

        function changeOp() {
            var index = $("#MainContent_m_opReason").prop("selectedIndex");
            if (index == 0) {
                $("#MainContent_m_item").prop("disabled", false);
            } else {
                $("#MainContent_m_item").prop("selectedIndex",0);
                $("#MainContent_m_item").prop("disabled", true);
            }
        }
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="background-color: #ccc; margin: 20px;">
        <h2 style="padding: 20px; text-align: center">客服补单/大户随访/换包福利-系统</h2>
        <div style="margin: 0 auto; width: 55%; line-height: 28px; font-weight: bold;">
            注：本系统仅用于对充值未到账的玩家进行补单，对客服工作中随访或换包的用户进行小额福利礼包赠送；所有赠送内容均以固定邮件格式发放，详情内容如下：<br />
            发送人：捕鱼运营团队<br />
            邮件名称：购买遗失补偿（补单）/福利（换包福利/大户福利）<br />
            邮件内容：尊敬的海上冒险者，补偿您在游戏过程中遗失的物品如下，感谢您一直以来的支持（补单）/尊敬的海上冒险者，赠送您福利如下，感谢您一直以来的支持（福利）<br />
        </div>
        <table style="margin: 0 auto; margin-top: 10px;">
            <tr style="text-align: right">
                <td>操作原因：</td>
                <td style="min-width: 150px;">
                    <asp:DropDownList ID="m_opReason" runat="server" CssClass="form-control" Style="min-width: 100px;" onchange="changeOp()"></asp:DropDownList>
                </td>
                <td>目标ID：</td>
                <td>
                    <asp:TextBox ID="m_playerIdList" runat="server" placeHolder="注：多个用；隔开" CssClass="form-control" Style="width: 400px;"></asp:TextBox></td>
                <td>补单项目：</td>
                <td>
                    <asp:DropDownList ID="m_item" runat="server" CssClass="form-control" Style="min-width: 130px;"></asp:DropDownList></td>
                <td>&ensp;&ensp;补单补贴/客服回访福利：</td>
                <td>
                    <asp:DropDownList ID="m_bonus" runat="server" CssClass="form-control" Style="min-width: 130px;"></asp:DropDownList></td>
                <td style="min-width: 60px;">&ensp;&ensp;操作备注：</td>
                <td>
                    <asp:TextBox ID="m_comment" runat="server" CssClass="form-control"></asp:TextBox></td>
            </tr>
        </table>
        <br />
        <br />
        <div style="text-align: center">
            <asp:Button ID="Button1" runat="server" Text="点击发放" OnClick="onSubmit" CssClass="btn btn-primary" />
            <span id="m_res" runat="server" style="color: red"></span>
        </div>
        <br />
    </div>
    <hr />
    <%-- ///////////////////////////////////////////////////////////////////////////////////////////////////// --%>

    <div class="container form-horizontal">
        <h2 style="margin: 20px 0 10px 0; text-align: center">系统操作历史查询</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control" Style="width: 50%; display: inline-block"></asp:TextBox>
                <asp:Button ID="Button2" runat="server" Text="查询" OnClick="onQuery" CssClass="btn btn-primary" Style="width: 15%; margin-left: 30px;" />
                <asp:Button ID="Button3" runat="server" Text="导出excel" OnClick="onExportExcel" CssClass="btn btn-primary" Style="width: 15%;" />
                <span id="m_resInfo" runat="server" style="color: red"></span>
            </div>
        </div>
        <div class="table-responsive" style="margin-top: 10px; text-align: center">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
        </div>
        <span id="m_page" style="text-align: center; display: block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size: x-large; text-align: center; display: block" runat="server"></span>
    </div>
    <br />
</asp:Content>
