<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatPumpNp7DayRechargeAct.aspx.cs" Inherits="WebManager.appaspx.stat.StatPumpNp7DayRechargeAct" %>
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
      <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">七日连充</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label" >统计项目：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_item" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label" >渠道：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_channel" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label" >时间：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Button ID="btn_stat" runat="server" Text="统计" CssClass="btn btn-primary  form-control" OnClick="OnStat"/>
            </div>
        </div>
    </div>
    
    <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center;width:90%;margin:0 auto"></asp:Table>
  
</asp:Content>
