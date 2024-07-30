define(function (require, exports, module) {
$(function () {

    var OP_ADD = 0;
    var OP_MODIFY = 2;
    var OP_QUERY = 3;
    var ADD_PPARAM = ['txtNickName', 'txtVipLevel', 'txtHead', 'txtFrameId', 'txtGold',
        'txtItem24', 'txtItem25', 'txtItem26', 'txtItem27'];

    function reqOp(op, robotId, jsonParam, callBack) {

        var jd = {};
        $.extend(jd, jsonParam);
        jd.op = op;
        jd.robotId = robotId;

        $.ajax({
            type: "POST",
            url: "/ashx/ServiceRobotRole.ashx",
            data: jd,

            success: function (data) {
                callBack(data);
            }
        });
    }

    // 注册按钮事件
    function regBtnEvent() {
        
        $(".table input[type='button']").click(function () {

            var inputId = $(this).attr('data');
            var key = $('#' + inputId).attr('name');
            var value = $('#' + inputId).val();
            console.log('key------------ ' + key + ' ----------' + value);
            var robotId = $('#txtRobotId').val();
            if (!$.isNumeric(robotId)) {
                alert('机器人ID非法');
                return;
            }

            console.log('id------------ ' + robotId);

            var param = {};
            param[key] = value;            
            var transstr = JSON.stringify(param);
            console.log(transstr);
            $('#m_res').text('请求中');

            reqOp(OP_MODIFY, robotId, { data: transstr}, function (retData) {

                var rd = $.parseJSON(retData);
                $('#m_res').text(rd["resultStr"]);

            });

        });
    }

    function getAddParam() {
        var param = {};
        for (var i = 0; i < ADD_PPARAM.length; i++) {
            var id = ADD_PPARAM[i];
            var name = $('#' + id).attr('name');
            var val = $('#' + id).val();
            param[name] = val;
        }
        return param;
    }

    function onSearchResult(jd, result) {

        var r = parseInt(result);
        if (r != 0) return;

        var dstJd = $.parseJSON(jd['content']);

        $.each(dstJd, function (k, v) {

            $('#' + k).val(v);
        });

    }

    function regOpPannel() {
        $(".opPanel input[type='button']").click(function () {

            var robotId = $('#txtRobotId').val();
            if (!$.isNumeric(robotId)) {
                alert('机器人ID非法');
                return;
            }

            var param = {};
            var opIdStr = $(this).attr('data');
            var opId = parseInt(opIdStr);
            if (opId == OP_ADD) {  // 增加
                param = getAddParam();
            }
            else if (opId == OP_QUERY) {

            }

            console.log('id------------ ' + robotId);

            var transstr = JSON.stringify(param);
            console.log(transstr);
            $('#m_res').text('请求中');

            reqOp(opId, robotId, { data: transstr }, function (retData) {

                if (opId == OP_QUERY) {
                    var jd = $.parseJSON(retData);
                    var result = jd['result'];
                    onSearchResult(jd, result);

                    $('#m_res').text(jd["resultStr"]);
                }
                else {
                    var rd = $.parseJSON(retData);
                    $('#m_res').text(rd["resultStr"]);
                }

            });

        });
    }

    function initSystem() {
        regBtnEvent();
        regOpPannel();
    }

    initSystem();

});
});



