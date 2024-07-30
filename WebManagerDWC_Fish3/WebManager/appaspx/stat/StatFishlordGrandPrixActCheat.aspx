<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatFishlordGrandPrixActCheat.aspx.cs" Inherits="WebManager.appaspx.stat.StatFishlordGrandPrixActCheat" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">捕鱼大奖赛排行榜作弊</h2>
        <br/>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">玩家ID：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_playerId" runat="server" CssClass="form-control" style="width:50%;display:inline-block"></asp:TextBox>
                <asp:Button ID="Button2" runat="server" Text="确定" onclick="onQuery" CssClass="btn btn-primary" style="width:20%;"/>
                <span>机器人ID区间：[10099001，10099200]</span>
            </div>
        </div>
         
        <hr />
        <h2 style="padding:20px;text-align:center">捕鱼大奖赛排行榜积分修改</h2>
        <h2 style="height:40px;line-height:40px;background-color:#ccc;padding:0 50px;">
            修改积分提示：
            修改积分时，需要先下线。等待超过7分钟再上线，才能确保服务器的数据不会覆盖修改后的数据。
        </h2>
        <br />
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">玩家昵称：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_nickName" runat="server" CssClass="form-control" style="width:60%;display:inline-block"></asp:TextBox>
                <span>&ensp;注：机器人首次作弊需输入标志</span>
            </div>
        </div>

        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">模式选择：</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_type" runat="server" CssClass="form-control" Style="width: 80%; display: inline-block"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">修改积分：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_score" runat="server" CssClass="form-control" style="width:40%;display:inline-block"></asp:TextBox>
            
                <asp:Button ID="Button1" runat="server" Text="修改" onclick="onEdit" CssClass="btn btn-primary" style="width:40%;display:inline-block"/>&ensp;&ensp;
                <br />
                <span id="m_res" style="font-size:medium;color:red;text-align:center;" runat="server"></span>
            </div>
        </div>
        <div class="table-responsive" style="margin-top:10px;text-align:center;margin-left:10%">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
         </div>
        <br/><br/>
      </div>
</asp:Content>
