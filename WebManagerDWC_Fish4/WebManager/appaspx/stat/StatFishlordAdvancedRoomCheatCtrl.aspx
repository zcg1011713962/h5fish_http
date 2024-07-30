<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatFishlordAdvancedRoomCheatCtrl.aspx.cs" Inherits="WebManager.appaspx.stat.StatFishlordAdvancedRoomCheatCtrl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">高级场作弊</h2>
        <br/>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">玩家ID：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_playerId" runat="server" CssClass="form-control" style="width:60%;display:inline-block"></asp:TextBox>
                <asp:Button ID="Button2" runat="server" Text="确定" onclick="onQuery" CssClass="btn btn-primary" style="width:20%;"/>
            </div>
        </div>
         <div class="table-responsive" style="margin-top:10px;text-align:center;margin-left:10%">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
         </div>
        <hr />
        <h2 style="padding:20px;text-align:center">高级场玩家积分修改</h2>
        <h2 style="height:40px;line-height:40px;background-color:#ccc;padding:0 50px;">
            修改积分提示：
            修改积分时，需要先下线。等待超过7分钟再上线，才能确保服务器的数据不会覆盖修改后的数据。
        </h2>
        <br />
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">修改积分：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_score" runat="server" CssClass="form-control" style="width:40%;display:inline-block"></asp:TextBox>
            
                <asp:Button ID="Button1" runat="server" Text="修改" onclick="onEdit" CssClass="btn btn-primary" style="width:40%;display:inline-block"/>&ensp;&ensp;
                <br />
                <span id="m_res" style="font-size:medium;color:red;text-align:center;" runat="server"></span>
            </div>
        </div>
      </div>
</asp:Content>
