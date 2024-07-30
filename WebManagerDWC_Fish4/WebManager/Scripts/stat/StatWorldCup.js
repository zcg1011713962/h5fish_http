define(function (require, exports, module) {
    $(function () {
        //op   0添加 1编辑 2删除 3查询 4刷新
        var oModifyUI = $("#divModifyNewParam");
        var oAdd = $("#btn_add");
        var oClose = $("#btn_close");
        var BTN_ID = 0;
        var regString = /^\s*(\d+)\s*$/;
        var regTime = /^\s*(\d{4})-(\d{1,2})-(\d{1,2})\s+(0\d{1}|1\d{1}|2[0-3]):([0-5]\d{1})$/;

        var idList = [];

        //ID大于等于49之后，需与数据库已有信息比较，该数组存储数据库信息
        var matchInfoFormDb = [];  //group,type,team1,team2

        //读取xml文件
        var matchXmlInfo = [];  //group,type,team1,team2
        getXmlInfo();

        //初始化
        init();

        //新增
        oAdd.click(function () {
            showModifyUI();
            $("#btn_submit").attr("data", "");
            //关闭
            oClose.click(function () {
                hideModifyUI();
            });
        });

        //当点击提交时  //区分是新增还是编辑  看btn的data属性是否为空 空 op=0新增 不空op=1编辑
        $('#btn_submit').click(function () {
            var id = $(this).attr('data');
            btnSubmit(id);
        });

        function btnSubmit(param) {
            var op = 2;
            if (param == "") { //_id为空 则添加
                op = 0;   
            } else {
                op = 1;
            }

            //赛事ID
            var matchId = $("#m_matchId").val();
            if (matchId == "") {
                $('#MainContent_m_opRes').text("ID不允许为空......");
                return false;
            }
            if (!regString.test(matchId)) {
                $("#MainContent_m_opRes").text("请输入正整数......");
                return false;
            }
            if (parseInt(matchId) < 0 || parseInt(matchId) >64) {
                $("#MainContent_m_opRes").text("请输入[1-64]间的整数......");
                return false;
            }
            //验证填入的ID是否已经存在
            if (op == 0 && idList[matchId] != "undefined") {
                $('#MainContent_m_opRes').text("ID已存在，请重新输入......");
                return false;
            }
            if (op == 1 && idList[matchId] != "undefined" && (param != idList[matchId])) {
                $('#MainContent_m_opRes').text("ID已存在，请重新输入......");
                return false;
            }
            ///////////////////////////////////////////////////////////////////
            //比赛时间  竞猜截止时间 显示时间
            var gameTime = $.trim($('#m_gameTime').val());
            var deadTime = $.trim($('#m_deadTime').val());
            var showTime = $.trim($('#m_showTime').val());
            if (!regTime.test(gameTime) || !regTime.test(deadTime) || !regTime.test(showTime)) {
                $('#MainContent_m_opRes').text("请按正确格式输入时间......");
                return false;
            }
            ////////////////////////////////////////////////////////////////////
            //组别 比赛类型
            var matchName = $("#m_groupId").val();
            var typeId = $("#m_typeId").val();
            ////////////////////////////////////////////////////////////////////////////////////
            //队伍1  队伍2
            var team1Id = $("#m_team1").val();
            var team2Id = $("#m_team2").val();
            if (team1Id == team2Id && team1Id != 99){
                $('#MainContent_m_opRes').text("比赛队伍不能相同......");
                return false;
            }
           ////////////////////////////////////////////////////////////////////////////////////
            if (matchId >= 49) { //ID与组别、比赛类型、队伍1、队伍2 对应数据表已有信息
                if (matchName != matchInfoFormDb[parseInt(matchId)][0]) {
                    if (!confirm("确定要修改组别信息？"))
                        return false;
                }
                if (typeId != matchInfoFormDb[parseInt(matchId)][1]) {
                    if (!confirm("确定要修改比赛类型信息？"))
                        return false;
                }
                if (team1Id != matchInfoFormDb[parseInt(matchId)][2]) {
                    if (!confirm("确定要修改队伍1信息？"))
                        return false;
                }
                if (team2Id != matchInfoFormDb[parseInt(matchId)][3]) {
                    if (!confirm("确定要修改队伍2信息？"))
                        return false;
                }
                if (team1Id != team2Id && (team1Id == 99 || team2Id == 99)) {
                    $('#MainContent_m_opRes').text("比赛队伍选择出错......");
                    return false;
                }
            } else {//ID与组别、比赛类型、队伍1、队伍2对应xml配置验证
                if (matchXmlInfo[parseInt(matchId)][0] != parseInt(matchName)) {
                    $('#MainContent_m_opRes').text("选择 组别 跟配置预设信息不相符......");
                        return false;
                }
                if (matchXmlInfo[parseInt(matchId)][1] != parseInt(typeId)) {
                    $('#MainContent_m_opRes').text("选择 比赛类型 跟配置预设信息不相符......")
                        return false;
                }
                if (matchXmlInfo[parseInt(matchId)][2] != parseInt(team1Id)) {
                    $('#MainContent_m_opRes').text("选择 队伍1 跟配置预设信息不相符......")
                        return false;
                }
                if (matchXmlInfo[parseInt(matchId)][3] != parseInt(team2Id)) {
                    $('#MainContent_m_opRes').text("选择 队伍2 跟配置预设信息不相符......")
                        return false;
                }
            }
            //////////////////////////////////////////////////////////////////////////////////////
            //队伍得分  为空时默认-1
            var team1Score = $("#m_team1Score").val();
            var team2Score = $("#m_team2Score").val();
            if ((team1Score!="" && isNaN(team1Score)) || (team2Score!="" && isNaN(team2Score))) {
                $('#MainContent_m_opRes').text("请输入整数得分......");
                return false;
            }
            if (team1Score == "") team1Score = -1;
            if (team2Score == "") team2Score = -1;

            var score1 = parseInt(team1Score);
            var score2 = parseInt(team2Score);
            if ( (score1 ^ score2) < 0)
            {
                $('#MainContent_m_opRes').text("得分不能一正一负......");
                return false;
            }
            //////////////////////////////////////////////////////////////////////////////////////
            //单人押注上限 10000000
            var betMaxCount = $("#m_betMaxCount").val();
            if (betMaxCount == "") {
                $("#MainContent_m_opRes").text("单人押注上限不允许为空......");
                return false;
            } else 
            {
                if (!regString.test(betMaxCount)){
                    $("#MainContent_m_opRes").text("请输入正整数......");
                    return false;
                }
                if (parseInt(betMaxCount) < 0){
                    $("#MainContent_m_opRes").text("请输入大于零的整数......");
                    return false;
                }
                if (parseInt(betMaxCount) > 10000000){
                    $("#MainContent_m_opRes").text("押注数量超出上限......");
                    return false;
                }
            }

            reqOp(op, {
                'id':param,
                'matchId': matchId,
                'matchStartTime': gameTime,
                'betEndTime':deadTime,
                'showTime': showTime,
                'matchName': matchName,
                'matchType': typeId,
                'homeTeamId': team1Id,
                'visitTeamId': team2Id,
                'homeScore': team1Score,
                'visitScore': team2Score,
                'betMaxCount':betMaxCount
            }, function (data) {
                hideModifyUI();
                $("#MainContent_m_res").text(data);
                if (data = "操作成功") {
                    init();
                }
            });
        }

        function init() {
            reqOp(3, {}, function (data) {
                var obj = JSON.parse(data);
                viewList(obj);
            });
        }

        //显示 删除 编辑
        function viewList(jsonObj) {
            var dataList = [];
            $('#MainContent_m_setListTable').html('');
            var $matchList = $('#MainContent_m_setListTable');
            var bList = JSON.parse(jsonObj.queryList);
            var len = bList.length;

            //如果查询没有，则显示表头
            if (len > 0) {
                var head = '<tr>' +
                               '<td>ID</td>' +
                               '<td>比赛时间</td>' +
                               '<td>竞猜截止时间</td>' +
                               '<td>显示时间</td>' +
                               '<td>组别</td>' +
                               '<td>比赛类型</td>' +
                               '<td>队伍1</td>' +
                               '<td>队伍2</td>' +
                               '<td>队伍1得分</td>' +
                               '<td>队伍2得分</td>' +
                               '<td>单人押注上限</td>' +
                               '<td>操作</td>' +
                           '</tr>';
                $(head).appendTo($matchList);
            }

            idList = {};
            matchInfoFormDb = [];
            for (var i = 0; i < bList.length; i++) {
                var obj = bList[i];
                dataList[obj.m_id] = obj;
                idList[obj.m_matchId] = obj.m_id;

                if (obj.m_matchId >= 49)
                    matchInfoFormDb[obj.m_matchId] = [obj.m_matchNameId, obj.m_matchType, obj.m_homeTeamId, obj.m_visitTeamId];

                var btnId1 = "EditMatch" + BTN_ID;
                var btnId2 = "RemoveMatch" + BTN_ID;
                var btnId3 = "RefreshMatch" + BTN_ID;
                var btnId4 = "RefreshMatch_res" + BTN_ID;
                BTN_ID++;

                var matchType = obj.m_matchType == 0 ? "小组赛" : "决赛";

                var tstr = '<tr id="' + obj.m_id + '">' +
                    '<td>' + obj.m_matchId + '</td>' +
                    '<td>' + obj.m_matchStartTime + '</td>' +
                    '<td>' + obj.m_betEndTime + '</td>' +
                    '<td>' + obj.m_showTime + '</td>' +
                    '<td>' + obj.m_matchName + '</td>' +
                    '<td>' + matchType + '</td>' +
                    '<td>' + obj.m_homeTeam + '</td>' +
                    '<td>' + obj.m_visitTeam + '</td>' +
                    '<td>' + obj.m_homeScore + '</td>' +
                    '<td>' + obj.m_visitScore + '</td>' +
                    '<td>' + obj.m_betMaxCount + '</td>';
                tstr += '<td>' + '<input type="button" id="' + btnId1 + '" value="编辑" />&ensp;' +
                                 '<input type="button" id="' + btnId2 + '" index="' + obj.m_matchId + '"value="删除" />&ensp;' +
                                 '<input type="button" id="' + btnId3 + '" index="' + obj.m_matchId + '" value="刷新服务器" />&ensp;' +
                                 '<span id="'+obj.m_matchId+'" style="color:red"></span>'
                                 '</td>';
                tstr += '</tr>';
                var t = $(tstr);
                $matchList.append(t);

                //编辑
                $('#' + btnId1).click(function () {
                    showModifyUI();
                    var id = $(this).parent().parent().attr("id");
                    $('#btn_submit').attr('data', id);
                    var obj_edit = dataList[id];

                    //ID
                    $("#m_matchId").val(obj_edit.m_matchId);

                    //比赛时间  竞猜截止时间 显示时间
                    $('#m_gameTime').val(obj_edit.m_matchStartTime);
                    $('#m_deadTime').val(obj_edit.m_betEndTime);
                    $('#m_showTime').val(obj_edit.m_showTime);
                   
                    //组别
                    $("#m_groupId option[value='" + obj_edit.m_matchNameId + "']").prop("selected", 'selected');

                    //比赛类型
                    $("#m_typeId option[value='" + obj_edit.m_matchType + "']").prop("selected", 'seleceted');

                    //队伍1  队伍2
                    $("#m_team1 option[value='" + obj_edit.m_homeTeamId + "']").prop("selected", 'selected');
                    $("#m_team2 option[value='" + obj_edit.m_visitTeamId + "']").prop("selected", 'selected');

                    //队伍得分
                    $("#m_team1Score").val(obj_edit.m_homeScore);
                    $("#m_team2Score").val(obj_edit.m_visitScore);

                    //单人押注上限
                    $("#m_betMaxCount").val(obj_edit.m_betMaxCount);

                    //关闭
                    oClose.click(function () {
                        hideModifyUI();
                    });
                });
                //删除
                $('#' + btnId2).click(function () {
                    //确认是否删除
                    var matchId = $(this).attr("index");
                    if (!confirm("确认删除ID为 " + matchId + " 赛事设置？"))
                        return false;

                    var id = $(this).parent().parent().attr("id");
                    reqOp(2, {
                        "id": id
                    }, function (data) {
                        $("#MainContent_m_res").text(data);
                        if (data == "操作成功") {
                            init();
                        }
                    });
                });
                //刷新服务器
                $('#' + btnId3).click(function () {
                    var matchId = $(this).attr("index");
                    reqOp(4, {
                        "matchId": matchId
                    }, function (data) {
                        $('#'+matchId).text("刷新服务器： " + data);
                        //if (data == "操作成功") {
                        //    init();
                        //}
                    });
                });
            }
        }

        function reqOp(op, jsonParam, callBack) {
            var jd = {};
            $.extend(jd, jsonParam);
            jd.op = op;

            $.ajax({
                type: "POST",
                url: "/ashx/WorldCupMatch.ashx",
                data: jd,
                success: function (data) {
                    callBack(data)
                }
            });
        }

        function hideModifyUI() {
            oModifyUI.hide();
            $("#m_matchId").val("");

            //比赛时间  竞猜截止时间 显示时间
            $('#m_gameTime').val("");
            $('#m_deadTime').val("");
            $('#m_showTime').val("");

            //组别
            $("#m_groupId option:first").prop("selected", 'selected');
            //比赛类型
            $("#m_typeId option:first").prop("selected", 'selected');

            //队伍1  队伍2
            $("#m_team1 option:first").prop("selected", 'selected');
            $("#m_team2 option:first").prop("selected", 'selected');

            //队伍得分
            $("#m_team1Score").val("");
            $("#m_team2Score").val("");

            //单人押注上限
            $("#m_betMaxCount").val("");
        }

        function showModifyUI() {
            oModifyUI.show();
            $('#MainContent_m_opRes').text("");
        }

        function getXmlInfo() {
            matchXmlInfo = [];
            $.get("/data/M_WorldCupSchedule.xml", function (xml) {
                $(xml).find("Data").each(function (i) {
                    var data = $(this);
                    var id = parseInt(data.attr("ID"));
                    var group = parseInt(data.attr("Group"));
                    var type = parseInt(data.attr("RaceType"));
                    var team1 = parseInt(data.attr("Team_1"));
                    var team2 = parseInt(data.attr("Team_2"));

                    matchXmlInfo[id] = [group,type,team1,team2];
                });
            });
        }
    });
});