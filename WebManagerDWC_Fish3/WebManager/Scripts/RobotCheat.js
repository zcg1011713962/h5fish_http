define(function (require, exports, module) {
$(function () {

    //////////////// 常量定义/////////////////////////////////////////////

    // 竞技场机器人
    var ROBOT_ARENA = "RobotArena";   
    // 积分送大奖
    var ROBOT_INT_SCORE = "RobotIntScoreSend";   
    // 砸蛋
    var ROBOT_BREAK_EGG = "RobotBreakEgg";   
    // 夏日狂欢
    var ROBOT_SUMMER_DAY = "RobotSummerDay";   
    // 段位赛
    var ROBOT_DAN_GRADING = "RobotDanGrading";   
    // 自由赛
    var ROBOT_ARENA_FREE = "RobotArenaFree";   
    // 大圣场作弊
    var ROBOT_DASHENG = "RobotDaSheng";   
    // 猎妖塔作弊
    var ROBOT_KILL_MONSTER = "RobotKillMonster";   
    // 追击火龙
    var ROBOT_KILL_FIRE_DRAGON = "RobotKillFireDragon";   
    // d11狂欢
    var ROBOT_KILL_FIRE_DRAGON = "RobotD11";   

    var OP_SEARCH = 1;
    var OP_MODIFY = 2;
    ////////////////////////作弊实例/////////////////////////////////////

    var objRobotArena = {
        elemPlayerId: null,
        elemNickName: null,
        elemScoreDay: null,
        elemScoreWeek: null,

        // 切换页签后，初始化
        init: function () {
            this.elemPlayerId = $(getIdForPlayer(true));
            this.elemNickName = $(getIdForName(true));
            this.elemScoreDay = $(getIdForScore(true));
            this.elemScoreWeek = $(getIdForScoreWeek(true));
        },

        // 查询出数据后，显示
        showData: function (data) {
           // console.log(data);
            if (data.result == 0) {

                var sd = $.parseJSON(data.resultStr);
                console.log(sd);

                this.elemNickName.val(sd.m_nickName);
                this.elemScoreDay.val(sd.m_dayScore);
                this.elemScoreWeek.val(sd.m_weekScore);

                //showOpResult('操作成功');    
            }
            else
            {
                this.elemNickName.val('');
                this.elemScoreDay.val('');
                this.elemScoreWeek.val('');

                showOpResult(data.resultStr);            
            }
        },

        modify: function () {

        },

        isValidData: function (data) {

            if (data.playerId == '')
                return false;

            if (data.scoreDay < 0) {
                return false;
            }
            if (data.scoreWeek < 0)
                return false;
            if (data.nickName == '')
                return false;

            return true;
        },

        // 以json形式返回数据, 提供给服务器修改
        getData: function () {

            var objd = {
                "playerId": this.elemPlayerId.val(),
                "nickName": this.elemNickName.val(),
                "scoreDay": this.elemScoreDay.val(),
                "scoreWeek": this.elemScoreWeek.val(),
            };

            return objd;
        },

    };

    var objRobotIntScoreSend = {
        elemPlayerId: null,
        elemNickName: null,
        elemScoreDay: null,
        //elemScoreWeek: null,

        // 切换页签后，初始化
        init: function () {
            this.elemPlayerId = $(getIdForPlayer(true));
            this.elemNickName = $(getIdForName(true));
            this.elemScoreDay = $(getIdForScore(true));
            //this.elemScoreWeek = $(getIdForScoreWeek(true));
        },

        // 查询出数据后，显示
        showData: function (data) {
            // console.log(data);
            if (data.result == 0) {

                var sd = $.parseJSON(data.resultStr);
                //console.log(sd);

                this.elemNickName.val(sd.m_nickName);
                this.elemScoreDay.val(sd.m_dayScore);
                //this.elemScoreWeek.val(sd.m_weekScore);

                //showOpResult('操作成功');    
            }
            else {
                this.elemNickName.val('');
                this.elemScoreDay.val('');

                showOpResult(data.resultStr);
            }
        },

        modify: function () {

        },

        isValidData: function (data) {

            if (data.playerId == '')
                return false;

            if (data.scoreDay < 0) {
                return false;
            }
         //  if (data.scoreWeek < 0)
         //      return false;
            if (data.nickName == '')
                return false;

            return true;
        },

        // 以json形式返回数据, 提供给服务器修改
        getData: function () {

            var objd = {
                "playerId": this.elemPlayerId.val(),
                "nickName": this.elemNickName.val(),
                "scoreDay": this.elemScoreDay.val(),
                //"scoreWeek": this.elemScoreWeek.val(),
            };

            return objd;
        },

    };

    var objRobotArenaFree = {
        elemPlayerId: null,
        elemNickName: null,
        elemScoreDay: null,
        elemRoomType: null,

        // 切换页签后，初始化
        init: function () {
            this.elemPlayerId = $(getIdForPlayer(true));
            this.elemNickName = $(getIdForName(true));
            this.elemScoreDay = $(getIdForScore(true));
            this.elemRoomType = $(getIdForRoomType(true));
        },

        // 查询出数据后，显示
        showData: function (data) {
            // console.log(data);
            if (data.result == 0) {

                var sd = $.parseJSON(data.resultStr);
                console.log(sd);

                this.elemNickName.val(sd.m_nickName);
                this.elemScoreDay.val(sd.m_dayScore);
                //this.elemRoomType.val(sd.m_weekScore);
               // this.cmbSelect(sd.m_weekScore);
                //showOpResult('操作成功');    
            }
            else {
                this.elemNickName.val('');
                this.elemScoreDay.val('');
               // this.elemScoreWeek.val('');

                showOpResult(data.resultStr);
            }
        },

        modify: function () {

        },

        isValidData: function (data) {

            if (data.playerId == '')
                return false;

            if (data.scoreDay < 0) {
                return false;
            }
            if (data.roomType < 0)
                return false;
            if (data.nickName == '')
                return false;

            return true;
        },

        // 以json形式返回数据, 提供给服务器修改
        getData: function () {

            var v = $(getIdForRoomType(true) + ' option:selected').val();
            var objd = {
                "playerId": this.elemPlayerId.val(),
                "nickName": this.elemNickName.val(),
                "scoreDay": this.elemScoreDay.val(),
                "scoreWeek": v,
            };

            return objd;
        },

        getCmbValue: function () {
            var v = $(getIdForRoomType(true) + ' option:selected').val();
            return v;
        },

        cmbSelect: function (curVal) {

            var id = getIdForRoomType(true);
            //var count = $(id + " option").length;
            var options = $(id + " option");
            var count = options.length;
            for (var i = 0; i < count; i++) {
                if (options[i].value == curVal+'') {
                    options[i].selected = true;
                    break;
                }
            }
        },

    };
    //////////////////////全局变量/////////////////////////////////////

    // 当前操作的面板
    var curOpPanel = null;

    // 当前哪个机器人作弊(key)
    var curRobotKey = '';

    /////////////////////////////////////////////////////////////

    // 机器人作弊集合
    var robotCollection = {
        //[ROBOT_ARENA]: objRobotArena,
        //[ROBOT_INT_SCORE]: objRobotIntScoreSend,

        // 当前操作的机器人实例
        curRobotObj: null,

        // 注册作弊实例
        regRobtoCheat: function (robotKey, robotObj) {
            this[robotKey] = robotObj;
        },

        init: function (robot) {
         
            if (this[robot] != undefined) {

                this.curRobotObj = this[robot];
                curRobotKey = robot;
                this[robot].init();

                if (curOpPanel != null) {
                    curOpPanel.hide();
                }
                console.log(curOpPanel);

                // 获取面板id
                curOpPanel = $('#OpPanel' + robot);

                if (curOpPanel != null) {
                    curOpPanel.show();  // 显示出面板
                }

                showOpResult("");
            }
            else
            {
                curRobotObj = null;
                console.log("not found " + robot);
            }
        },

        showData: function (robot, data) {
            if (this[robot] != undefined) {
                this[robot].showData(data);
            }
        },

        modify: function (robot) {
            if (this[robot] != undefined) {
                this[robot].modify();
            }
        },

        getCurRobotObj: function () {
            return this.curRobotObj;
        }
    };

    /////////////////////////////////////////////////////////////
    function getIdForPlayer(addPrefix) {
        var s = 'PlayerId' + curRobotKey;
        if (addPrefix) {
            s = '#' + s;
        }
        return s;
    }
    function getIdForName(addPrefix) {
        var s = 'NickName' + curRobotKey;
        if (addPrefix) {
            s = '#' + s;
        }
        return s;
    }
    function getIdForScore(addPrefix) {
        var s = 'ScoreDay' + curRobotKey;
        if (addPrefix) {
            s = '#' + s;
        }
        return s;
    }
    function getIdForScoreWeek(addPrefix) {
        var s = 'ScoreWeek' + curRobotKey;
        if (addPrefix) {
            s = '#' + s;
        }
        return s;
    }
    function getIdForRoomType(addPrefix) {
        var s = 'RoomType' + curRobotKey;
        if (addPrefix) {
            s = '#' + s;
        }
        return s;
    }
    /////////////////////////////////////////////////////////////
    function initRobotFun() {
        $('.nav-tabs li').click(function () {
            var did = $(this).attr('data');
           // console.log('-------- ' + did);
            $(this).attr('class', 'active').siblings().attr('class', '');
            robotCollection.init(did);

        });

        robotCollection.init(ROBOT_ARENA);
    }

    // 点击查询玩家积分
    $('#ScoreSearch').click(function () {

        var elemId = getIdForPlayer(true);
        var playerId = $(elemId).val();
        if (playerId == '') {
            console.log("ScoreSearch click playerId null");
            return;
        }

        var other = 0;
        if (curRobotKey == ROBOT_ARENA_FREE ||
            curRobotKey == ROBOT_DASHENG) {
            other = objRobotArenaFree.getCmbValue();
        }

        reqOp(OP_SEARCH, { op: OP_SEARCH, "robot": curRobotKey, "playerId": playerId, "other": other }, function (data) {

            var jdata = $.parseJSON(data);
            if (jdata.robot == curRobotKey) { // 没有切换到别的机器人作弊
                robotCollection.showData(curRobotKey, jdata);
            }
            
        });
    });

    // 点击修改玩家积分
    $('#ScoreModify').click(function () {

        var obj = robotCollection.getCurRobotObj();
        if (obj == null)
        {
            console.log("ScoreModify click not found cur obj");
            return;
        }

        var reqObj = { "robot": curRobotKey };
        var reqData = obj.getData();
        if (!obj.isValidData(reqData)) {
            showOpResult("参数非法");
            return;
        }

        showOpResult('进行中');

        $.extend(reqObj, reqData);

        reqOp(OP_MODIFY, reqObj, function (data) {

            var jdata = $.parseJSON(data);
            showOpResult(jdata.resultStr);

        });

    });

    function reqOp(op, jsonParam, callBack) {

        var jd = {};
        $.extend(jd, jsonParam);
        jd.op = op;

        $.ajax({
            type: "POST",
            url: "/ashx/RobotCheatHandle.ashx",
            data: jd,

            success: function (data) {
                callBack(data);
            }
        });
    }

    function showOpResult(str) {

        $('#ResultCode').text(str);
    }

    // 初始化系统运行
    function initSystem() {

        // 注册作弊实例。 添加新的作弊功能时，只需定义 key + 作弊实例，并且实现接口
        robotCollection.regRobtoCheat(ROBOT_ARENA, objRobotArena);
        robotCollection.regRobtoCheat(ROBOT_INT_SCORE, objRobotIntScoreSend);
        // 砸蛋
        robotCollection.regRobtoCheat(ROBOT_BREAK_EGG, objRobotIntScoreSend);
        // 夏日狂欢
        robotCollection.regRobtoCheat(ROBOT_SUMMER_DAY, objRobotIntScoreSend);
        // 段位作弊
        robotCollection.regRobtoCheat(ROBOT_DAN_GRADING, objRobotIntScoreSend);
        // 自由赛作弊
        robotCollection.regRobtoCheat(ROBOT_ARENA_FREE, objRobotArenaFree);
        robotCollection.regRobtoCheat(ROBOT_DASHENG, objRobotArenaFree);
        robotCollection.regRobtoCheat(ROBOT_KILL_MONSTER, objRobotIntScoreSend);
        robotCollection.regRobtoCheat(ROBOT_KILL_FIRE_DRAGON, objRobotIntScoreSend);
        robotCollection.regRobtoCheat(ROBOT_KILL_FIRE_DRAGON, objRobotIntScoreSend);

        initRobotFun();
    }

    initSystem();

});
});