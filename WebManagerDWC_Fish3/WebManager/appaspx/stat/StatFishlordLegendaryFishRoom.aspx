<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatFishlordLegendaryFishRoom.aspx.cs" Inherits="WebManager.appaspx.stat.StatFishlordLegendaryFishRoom" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
            paramStyle();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">巨鲲场玩法</h2>

         <div class="form-group">
             <label for="account" class="col-sm-2 control-label">统计内容：</label>
             <div class="col-sm-10">
                 <asp:DropDownList ID="m_actId" runat="server" CssClass="form-control" onchange="paramStyle()"></asp:DropDownList>
             </div>
         </div>
          <div class="form-group" id="activityTime">
            <label for="account" class="col-sm-2 control-label">时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <asp:Button ID="Button1" runat="server" Text="确定" onclick="onQuery" CssClass="btn btn-primary" style="width:90%;margin-left:10%"/>
        <br/>
        <br/>
         <div class="table-responsive" style="margin-top:10px;text-align:center">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
         </div>
      </div>
</asp:Content>
