<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="FishlordBulletHeadStat.aspx.cs" Inherits="WebManager.appaspx.stat.fish.FishlordBulletHeadStat" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../../Scripts/datepicker/daterangepicker.js"></script>
    <script src="../../../Scripts/module/sea.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
        });
        seajs.use("../../../Scripts/stat/FishlordBulletHead.js");
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal" style="text-align:center">
        <h2 style="padding:20px;text-align:center">鱼雷统计</h2>
        <asp:Button ID="Button1" runat="server" Text="重置" onclick="onReset" CssClass="btn btn-primary" />
        <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        <br/><br/>
        <div class="container" style="margin-top:10px;">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
         </div>
        <hr />

        <h2 style="padding:20px;text-align:center">鱼雷查询</h2>

        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">查询内容：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_queryType" runat="server" CssClass="form-control" onchange="paramStyle()"></asp:DropDownList>
            </div>
        </div>

        <div class="form-group">
            <label for="account"  class="col-sm-2 control-label">时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Button ID="btn_stat" runat="server" Text="查询" CssClass="btn btn-primary  form-control" OnClick="OnStat"/>
            </div>
        </div>
    </div>
     <asp:Table ID="m_resTable" runat="server" CssClass="table table-hover table-bordered" style="text-align:center;margin:0 auto;width:90%"></asp:Table>
</asp:Content>
