<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OperationFishlordRobotRankCFG.aspx.cs" Inherits="WebManager.appaspx.operation.OperationFishlordRobotRankCFG" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="container form-horizontal div-robot" style="padding: 20px; text-align: center;width:50%">
        <h2 style="text-align: center; margin-bottom: 20px;">机器人积分管理</h2>
         <br />
         <div class="form-group">
            <label for="account" class="col-sm-2 control-label">排行榜：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_type" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">参数：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_param" runat="server" CssClass="form-control" Style="height: 30px;"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Button ID="btnStat" runat="server" Text="保存" CssClass="btn btn-primary form-control" OnClick="onEdit"/>
                <span id="m_res" style="font-size: medium; color: red" runat="server"></span>
            </div>
        </div>
        <div class="table-responsive" style="margin-top:10px;text-align:center">
           <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
        </div>
    </div>
</asp:Content>
