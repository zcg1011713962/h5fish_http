<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RobotCheat.aspx.cs" Inherits="WebManager.appaspx.RobotCheat" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="../Scripts/module/sea.js"></script>
    <script type="text/javascript">
        seajs.use("../Scripts/RobotCheat.js?ver=1.3");
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 style="padding:20px;text-align:center">积分修改作弊</h2>
        <div class="row" style="padding-bottom:20px">
            <div class="col-sm-8 col-sm-offset-2">
                <span class="label label-warning">修改积分时，需要先下线。等待超过7分钟再上线，才能确保服务器的数据不会覆盖修改后的数据。</span>
            </div>
        </div>

        <div class="row">
            <%-- <div class="col-sm-12">
                <ul class="SelCard">
                    <li class="Active" data="RobotArena">竞技场作弊</li><li data="RobotIntScoreSend">积分送大奖作弊</li>
                </ul>
            </div> --%>

            <ul class="nav nav-tabs">
              <li role="presentation" class="active" data="RobotArena"><a href="#" >竞技场作弊</a></li>
              <li role="presentation" data="RobotIntScoreSend"> <a href="#" >积分送大奖作弊</a></li>
              <li role="presentation" data="RobotBreakEgg"> <a href="#" >砸蛋作弊</a></li>
              <li role="presentation" data="RobotSummerDay"> <a href="#" >夏日狂欢作弊</a></li>
              <li role="presentation" data="RobotDanGrading"> <a href="#" >段位赛作弊</a></li>
              <li role="presentation" data="RobotArenaFree"> <a href="#" >自由赛作弊</a></li>
              <li role="presentation" data="RobotDaSheng"> <a href="#" >大圣场作弊</a></li>
              <li role="presentation" data="RobotKillMonster"> <a href="#" >猎妖塔作弊</a></li>
              <li role="presentation" data="RobotKillFireDragon"> <a href="#" >追击火龙作弊</a></li>
              <li role="presentation" data="RobotD11"> <a href="#" >双11狂欢</a></li>
            </ul>
        </div>
        <br />

        <div class="row">
            <div id="OpPanelRobotArena" class="container form-horizontal" style="width:80%;display:none">
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">玩家Id：</label>
                    <div class="col-sm-4">
                        <input type="text" id="PlayerIdRobotArena" class="form-control" />
                    </div>    
                    <div class="col-sm-4">
                         机器人ID区间：[10099001，10099200]
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">昵称：</label>
                    <div class="col-sm-4">
                        <input type="text" id="NickNameRobotArena" class="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">当日积分：</label>
                    <div class="col-sm-4">
                        <input type="text" id="ScoreDayRobotArena" class="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">本周积分：</label>
                    <div class="col-sm-4">
                        <input type="text" id="ScoreWeekRobotArena" class="form-control" />
                    </div>
                </div>
            </div>

            <div id="OpPanelRobotIntScoreSend" class="container form-horizontal" style="width:80%;display:none">
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">玩家Id：</label>
                    <div class="col-sm-4">
                        <input type="text" id="PlayerIdRobotIntScoreSend" class="form-control" />
                    </div>         
                    <div class="col-sm-4">
                         机器人ID区间：[10099001，10099200]
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">昵称：</label>
                    <div class="col-sm-4">
                        <input type="text" id="NickNameRobotIntScoreSend" class="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">当日积分：</label>
                    <div class="col-sm-4">
                        <input type="text" id="ScoreDayRobotIntScoreSend" class="form-control" />
                    </div>
                </div>
            </div>

            <div id="OpPanelRobotBreakEgg" class="container form-horizontal" style="width:80%;display:none">
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">玩家Id：</label>
                    <div class="col-sm-4">
                        <input type="text" id="PlayerIdRobotBreakEgg" class="form-control" />
                    </div>         
                    <div class="col-sm-4">
                         机器人ID区间：[10099001，10099200]
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">昵称：</label>
                    <div class="col-sm-4">
                        <input type="text" id="NickNameRobotBreakEgg" class="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">积分：</label>
                    <div class="col-sm-4">
                        <input type="text" id="ScoreDayRobotBreakEgg" class="form-control" />
                    </div>
                </div>
            </div>

            <div id="OpPanelRobotSummerDay" class="container form-horizontal" style="width:80%;display:none">
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">玩家Id：</label>
                    <div class="col-sm-4">
                        <input type="text" id="PlayerIdRobotSummerDay" class="form-control" />
                    </div>         
                    <div class="col-sm-4">
                         机器人ID区间：[10099001，10099200]
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">昵称：</label>
                    <div class="col-sm-4">
                        <input type="text" id="NickNameRobotSummerDay" class="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">积分：</label>
                    <div class="col-sm-4">
                        <input type="text" id="ScoreDayRobotSummerDay" class="form-control" />
                    </div>
                </div>
            </div>

            <div id="OpPanelRobotDanGrading" class="container form-horizontal" style="width:80%;display:none">
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">玩家Id：</label>
                    <div class="col-sm-4">
                        <input type="text" id="PlayerIdRobotDanGrading" class="form-control" />
                    </div>         
                    <div class="col-sm-4">
                         机器人ID区间：[10099001，10099200]
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">昵称：</label>
                    <div class="col-sm-4">
                        <input type="text" id="NickNameRobotDanGrading" class="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">积分：</label>
                    <div class="col-sm-4">
                        <input type="text" id="ScoreDayRobotDanGrading" class="form-control" />
                    </div>
                </div>
            </div>

            <div id="OpPanelRobotArenaFree" class="container form-horizontal" style="width:80%;display:none">
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">比赛类型：</label>
                    <div class="col-sm-4" id="RoomTypeRobotArenaFree">
                        <select class="form-control">
                            <option value="12">初级赛</option>
                            <option value="13">中级赛</option>
                        </select>
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">玩家Id：</label>
                    <div class="col-sm-4">
                        <input type="text" id="PlayerIdRobotArenaFree" class="form-control" />
                    </div>         
                    <div class="col-sm-4">
                         机器人ID区间：[10099001，10099200]
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">昵称：</label>
                    <div class="col-sm-4">
                        <input type="text" id="NickNameRobotArenaFree" class="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">积分：</label>
                    <div class="col-sm-4">
                        <input type="text" id="ScoreDayRobotArenaFree" class="form-control" />
                    </div>
                </div>
            </div>

            <div id="OpPanelRobotDaSheng" class="container form-horizontal" style="width:80%;display:none">
               <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">积分类型：</label>
                    <div class="col-sm-4" id="RoomTypeRobotDaSheng">
                        <select class="form-control">
                            <option value="0">日积分</option>
                            <option value="1">周积分</option>
                        </select>
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">玩家Id：</label>
                    <div class="col-sm-4">
                        <input type="text" id="PlayerIdRobotDaSheng" class="form-control" />
                    </div>    
                    <div class="col-sm-4">
                         机器人ID区间：[10099001，10099200]
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">昵称：</label>
                    <div class="col-sm-4">
                        <input type="text" id="NickNameRobotDaSheng" class="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">积分：</label>
                    <div class="col-sm-4">
                        <input type="text" id="ScoreDayRobotDaSheng" class="form-control" />
                    </div>
                </div>
            </div>

            <div id="OpPanelRobotKillMonster" class="container form-horizontal" style="width:80%;display:none">
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">玩家Id：</label>
                    <div class="col-sm-4">
                        <input type="text" id="PlayerIdRobotKillMonster" class="form-control" />
                    </div>         
                    <div class="col-sm-4">
                         机器人ID区间：[10099001，10099200]
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">昵称：</label>
                    <div class="col-sm-4">
                        <input type="text" id="NickNameRobotKillMonster" class="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">积分：</label>
                    <div class="col-sm-4">
                        <input type="text" id="ScoreDayRobotKillMonster" class="form-control" />
                    </div>
                </div>
            </div>

            <div id="OpPanelRobotKillFireDragon" class="container form-horizontal" style="width:80%;display:none">
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">玩家Id：</label>
                    <div class="col-sm-4">
                        <input type="text" id="PlayerIdRobotKillFireDragon" class="form-control" />
                    </div>         
                    <div class="col-sm-4">
                         机器人ID区间：[10099001，10099200]
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">昵称：</label>
                    <div class="col-sm-4">
                        <input type="text" id="NickNameRobotKillFireDragon" class="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">积分：</label>
                    <div class="col-sm-4">
                        <input type="text" id="ScoreDayRobotKillFireDragon" class="form-control" />
                    </div>
                </div>
            </div>

            <div id="OpPanelRobotD11" class="container form-horizontal" style="width:80%;display:none">
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">玩家Id：</label>
                    <div class="col-sm-4">
                        <input type="text" id="PlayerIdRobotD11" class="form-control" />
                    </div>         
                    <div class="col-sm-4">
                         机器人ID区间：[10099001，10099200]
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">昵称：</label>
                    <div class="col-sm-4">
                        <input type="text" id="NickNameRobotD11" class="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">积分：</label>
                    <div class="col-sm-4">
                        <input type="text" id="ScoreDayRobotD11" class="form-control" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-group">
                <div class="col-sm-2 col-sm-offset-2">
                    <input type="button" value="查询" id="ScoreSearch" class="ScoreSearch form-control btn btn-primary" />
                </div>
                <div class="col-sm-2">
                    <input type="button" value="提交修改" id="ScoreModify" class="ScoreModify form-control btn btn-primary" />
                </div>
                <div class="col-sm-4">
                    <span class="label label-default" id="ResultCode"></span>
                </div>
            </div>
        </div>
        <br />

        <div class="row">
            <table class="table table-hover" id="SearchResult"></table>
        </div>
    </div>
</asp:Content>
