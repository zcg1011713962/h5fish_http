<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatFishlordLegendaryRank.aspx.cs" Inherits="WebManager.appaspx.stat.StatFishlordLegendaryRank" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
            paramStyle();
        });
        function paramStyle() {
            var indexAct = $("#MainContent_m_actId").prop("selectedIndex");
            if (indexAct == 1) {
                //定海神针 
                rank2Style();
            } else {
                //巨鲲降世

                $("#MainContent_m_rankType1").show();
                $("#MainContent_m_rankType2").hide();
                rank1Style();
            }
        }
        function rank1Style() {
            var indexRank = $("#MainContent_m_rankType1").prop("selectedIndex");
            if (indexRank == 1) {
                $("#activityTime").show();
            } else {
                $("#activityTime").hide();
            }
        }
        function rank2Style() {
            $("#MainContent_m_rankType1").hide();
            $("#MainContent_m_rankType2").show();
            $("#activityTime").hide();
        }
       
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">巨鲲降世/定海神针排行榜</h2>

         <div class="form-group">
             <label for="account" class="col-sm-2 control-label">排行内容：</label>
             <div class="col-sm-10">
                 <asp:DropDownList ID="m_actId" runat="server" CssClass="form-control" onchange="paramStyle()"></asp:DropDownList>
             </div>
         </div>
         <div class="form-group">
             <label for="account" class="col-sm-2 control-label">排行类型：</label>
             <div class="col-sm-10">
                 <asp:DropDownList ID="m_rankType1" runat="server" CssClass="form-control" onchange="rank1Style()"></asp:DropDownList>
                 <asp:DropDownList ID="m_rankType2" runat="server" CssClass="form-control" onchange="rank2Style()"></asp:DropDownList>
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
