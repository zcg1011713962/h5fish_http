<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ShcdCardsQuery.aspx.cs" Inherits="WebManager.appaspx.stat.shcd.ShcdCardsQuery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../../Scripts/datepicker/daterangepicker.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">黑红梅方牌局查询</h2>

        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">日期：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control" style="height:30px;"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">房间选择：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_roomId" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>

        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">牌局ID：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_cardId" runat="server" CssClass="form-control" style="height:30px;"></asp:TextBox>
            </div>
        </div>
        
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                 <asp:Button ID="btnStat" runat="server" Text="查询" CssClass="btn btn-primary form-control" OnClick="onQuery"/>
            </div>
        </div>
    </div>
    <div class="table-responsive" style="padding:0 1px;">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center"></asp:Table>
    </div>
    
    <br />
    <br />
    <span id="m_page" style="text-align:center;display:block" runat="server"></span>
    <br />
    <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
</asp:Content>
