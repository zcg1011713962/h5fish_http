<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatSpittorSnatchActivity.aspx.cs" Inherits="WebManager.appaspx.stat.StatSpittorSnatchActivity" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <script type="text/javascript">
         $(function () {
             ShowInput();
         });
         function ShowInput() {
             if ($("#MainContent_m_queryType").prop("selectedIndex") == 2) {
                 $("#playerId").show();
                 $("#killCount").show();
                 $("#note").show();
                 $("#MainContent_m_result").hide();
             }
             else {
                 $("#killCount").hide();
                 $("#playerId").hide();
                 $("#note").hide();
             }
         }
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">金蟾夺宝活动</h2>
        
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">查询方式：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_queryType" runat="server" CssClass="form-control" onchange="ShowInput()"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group" id="note">
            <label for="account" class="col-sm-2 control-label"></label>
            <div class="col-sm-10">
                <span style="color:red;">注意：修改时，需要先下线。等待超过7分钟再上线，才能确保服务器的数据不会覆盖修改后的数据。</span>
            </div>
        </div>
        <div class="form-group" id="playerId">
            <label for="account" class="col-sm-2 control-label">玩家ID：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_playerId" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <div class="form-group"id="killCount">
            <label for="account" class="col-sm-2 control-label">金蟾击杀数量：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_killCount" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <asp:Button ID="Button1" runat="server" Text="确定" onclick="onQuery" CssClass="btn btn-primary" style="width:90%;margin-left:10%"/><br/><br/>
        <span id="m_resNote" style="font-size:medium;color:red;margin-left:50%;" runat="server"></span>
         <div class="table-responsive" style="margin-top:10px;text-align:center">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
         </div>
      </div>
</asp:Content>
