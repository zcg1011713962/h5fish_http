<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatWjlwDefRechargeReward.aspx.cs" Inherits="WebManager.appaspx.stat.StatWjlwDefRechargeReward" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../Scripts/module/sea.js"></script>
	<script type="text/javascript">
	    seajs.use("../../Scripts/stat/StatWjlwDefRechargeReward.js");
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="container form-horizontal">
        <h2 style="text-align:center;padding-bottom:20px;">围剿龙王自定义当天付费玩家最终的排名</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">昵称：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_nickname" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">奖项：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_rewardId" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                 <asp:Button ID="Button2" runat="server" Text="确定" CssClass="btn btn-primary form-control" onclick="onConfirm"/>
            </div>
            <br />
            <br />
            <div class="col-sm-offset-2 col-sm-10" style="text-align:center">
                <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
            </div>
         </div>
         <hr />
         <div style="margin-top:10px;text-align:center">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>  
        </div>
      </div>
</asp:Content>
