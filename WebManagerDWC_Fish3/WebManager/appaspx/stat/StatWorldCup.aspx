<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatWorldCup.aspx.cs" Inherits="WebManager.appaspx.stat.StatWorldCup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .Boss{margin-top:20px;border:1px solid black;padding:10px;}
        .OpModify{width:800px; background:rgb(255,255,255);padding-bottom:20px}
    </style>
    <script src="../../Scripts/module/sea.js"></script>
    <link rel="stylesheet" type="text/css" media="all" href="../../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../../Scripts/datepicker/daterangepicker.js"></script>
    <style>
        #notes ul, #notes li {
            list-style:none;
            font-size:16px;
        }
        .PopDialogBg{
            position:fixed;
        }
    </style>
	<script type="text/javascript">
	    $(function () {
	        $('#m_date').daterangepicker();
	    });
	    seajs.use("../../Scripts/stat/StatWorldCup.js");
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth" style="width:80%">
        <h2 style="text-align:center;margin-bottom:20px;">世界杯大竞猜赛事表</h2>
        <table style="margin:0 auto;text-align:center;">
            <tr>
                <td style="min-width:140px;">
                    <input type="button" id="btn_add" class="btn btn-primary form-control"  style="width:120px;height:35px;" value="新增赛事" />
                </td>
                <td>
                    <asp:Button ID="btn_refresh" runat="server" Text="刷新服务器" 
                        class="btn btn-primary form-control" style="width:120px;height:35px;" onclick="onReFresh"/>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
                </td>
            </tr>
        </table><br/>
        <div id="notes">
            <ul style="background-color:#ccc;opacity:0.7">
                <li style="color:red">【提示】</li>
                <li>&ensp;1. 每天中午12点前，需要设置好比赛结果。</li>
                <li>&ensp;2. 12点前没有设置结果的，该场比赛将不会发奖励。会在次天12点重新检测是否设置了结果。</li>
                <li>&ensp;3. 12点发奖励期间，服务器将禁止刷新。</li>
                <li>&ensp;4. 对阵双方不能颠倒。如 俄罗斯-沙特，不能写成  沙特 - 俄罗斯。</li>
            </ul>
        </div>
        <asp:Table ID="m_setListTable" style="text-align:center;width:100%" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
    </div>
    <div id="divModifyNewParam" class="PopDialogBg">
        <div class="container form-horizontal OpModify">
            <h3 id="btn_close" style="text-align:center;padding:10px;margin-bottom:10px;background:#ccc;cursor:pointer;">关闭</h3>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">ID：</label>
                <div class="col-sm-6">
                    <asp:TextBox ID="m_matchId" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
                <div class="col-sm-4">
                    <span>ID为整数，1-64之间，不允许重复</span>
                </div> 
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">比赛时间：</label>
                <div class="col-sm-6">
                    <asp:TextBox ID="m_gameTime" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
                <div class="col-sm-4">
                    <span>格式如：2018-02-08 00:00</span>
                </div> 
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">竞猜截止时间：</label>
                <div class="col-sm-6">
                    <asp:TextBox ID="m_deadTime" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
                 <div class="col-sm-4">
                    <span>格式如：2018-02-08 00:00</span>
                </div> 
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">显示时间：</label>
                <div class="col-sm-6">
                    <asp:TextBox ID="m_showTime" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
                 <div class="col-sm-4">
                    <span>格式如：2018-02-08 00:00</span>
                </div> 
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">组别：</label>
                <div class="col-sm-6">
                    <asp:DropDownList ID="m_groupId" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">比赛类型：</label>
                <div class="col-sm-6">
                    <asp:DropDownList ID="m_typeId" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">队伍1：</label>
                <div class="col-sm-6">
                     <asp:DropDownList ID="m_team1" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">队伍2：</label>
                <div class="col-sm-6">
                    <asp:DropDownList ID="m_team2" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">队伍1得分：</label>
                <div class="col-sm-6">
                    <asp:TextBox ID="m_team1Score" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
                <div class="col-sm-4">
                    <span>请输入整数（为空时，默认-1）</span>
                </div> 
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">队伍2得分：</label>
                <div class="col-sm-6">
                    <asp:TextBox ID="m_team2Score" runat="server" CssClass="form-control"  ClientIDMode="Static"></asp:TextBox>
                </div>
                <div class="col-sm-4">
                    <span>请输入整数（为空时，默认-1）</span>
                </div> 
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">单人押注上限：</label>
                <div class="col-sm-6">
                    <asp:TextBox ID="m_betMaxCount" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
                <div class="col-sm-4">
                    <span>请输入正整数</span>
                </div> 
            </div>
            <input id="btn_submit" type="button" class="btn btn-primary form-control"  value="提交修改" />
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                     <span id="m_opRes" style="font-size:medium;color:red" runat="server"></span>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
