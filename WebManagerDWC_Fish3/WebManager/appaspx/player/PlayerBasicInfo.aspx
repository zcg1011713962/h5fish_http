<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PlayerBasicInfo.aspx.cs" Inherits="WebManager.appaspx.playerinfo.PlayerBasicInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="../../Scripts/module/sea.js"></script>
    <script type="text/javascript">
        seajs.use("../../Scripts/player/PlayerBasicInfo.js");
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal" style="text-align:center">
        <h2 style="padding:20px;text-align:center">玩家基本信息</h2>
        <div class="form-group">
            <label for="account"  class="col-sm-2 control-label">ID或昵称：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_playerIds" runat="server" CssClass="form-control" placeholder="以玩家ID或昵称查询"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Button ID="btn_stat" runat="server" Text="查询" CssClass="btn btn-primary  form-control" OnClick="OnStat"/>
            </div>
        </div>
    </div>
     <asp:Table ID="m_resTable" runat="server" CssClass="table table-hover table-bordered" style="text-align:center;margin:0 auto;width:98%"></asp:Table>
</asp:Content>
