<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OperationGameDailyTotalLoseWinRank.aspx.cs" Inherits="WebManager.appaspx.operation.OperationGameDailyTotalLoseWinRank" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">小游戏日累计输赢</h2>
        
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">游戏类型：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_gameType" runat="server" CssClass="form-control" onchange="changeTimeType()"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">榜单类型：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_rankType" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>

        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">年份：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_year" placeHolder="2018" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">月份：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_month" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>

        <asp:Button ID="Button1" runat="server" Text="确定" onclick="onQuery" CssClass="btn btn-primary" style="width:90%;margin-left:10%"/>
        <br/><br/>
         <div class="table-responsive" style="margin-top:10px;text-align:center">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
         </div>
      </div>
</asp:Content>
