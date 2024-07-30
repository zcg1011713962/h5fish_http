define(function (require, exports, module) {
$(function () {

    var oExpTable = $('#MainContent_m_expRateTable');
    var oModifyUI = $('#divModifyNewParam');
    var oMinEarn = $('#m_minEarn');
    var oMaxEarn = $('#m_maxEarn');
    var oStartEarn = $('#m_startEarn');
    var oExpEarn = $('#m_expEarn');
    var opResInfo = $('#opRes');
    var oMinControlEarn = $("#m_minControlEarn");
    var oMaxControlEarn = $("#m_maxControlEarn");

    var INDEX_START_EARN = 8;
    var INDEX_MIN_CONTROL_EARN = 12;
    var INDEX_MAX_CONTROL_EARN = 10;
    var INDEX_MIN_EARN = 13;
    var INDEX_MAX_EARN = 9;   //最大盈利率 9 > 控制赢利率大  10> 期望赢利率 11 > 控制赢利率小 12> 最小赢利率13
    var INDEX_EXP_EARN = 11;

    var cdataJs = require('../cdata.js');
    var g_roomId = 1;

    oExpTable.find('a').click(function () {

        var _this = $(this);
        var roomid = _this.attr('roomId');
        var index = _this.parent().parent().index();
        var param = findCurParam(_this);
        showModifyUI(roomid, param,index);
    });
    
    oModifyUI.find('h2').click(function () {
        oModifyUI.hide();
        window.location.reload();
    });
    
    $('#btnModifyParam').click(function () {

        opResInfo.html('正在操作...');
        var index = $(this).attr('index');
        reqOp(0, {
            'room': g_roomId,
            'expRate': oExpEarn.val(),
            'maxEarnValue': oMaxEarn.val(),
            'minEarnValue': oMinEarn.val(),
            'startEarnValue': oStartEarn.val(),
            'minControlEarnValue': oMinControlEarn.val(),
            'maxControlEarnValue': oMaxControlEarn.val()
        }, function (data) {
            opResInfo.html(data);
            //if (data == "操作成功") {
            //    setTimeout(function () {
            //        oModifyUI.hide();
            //    }, 3000); //3秒后执行且仅执行一次

            //    //更改数据
            //    var tr = oExpTable.find("tr").eq(parseInt(index));
            //    tr.find('td').eq(8).text(oStartEarn.val());
            //    tr.find('td').eq(9).text(oMaxEarn.val());
            //    tr.find('td').eq(10).text(oMaxControlEarn.val());
            //    tr.find('td').eq(11).text(oExpEarn.val());
            //    tr.find('td').eq(12).text(oMinControlEarn.val());
            //    tr.find('td').eq(13).text(oMinEarn.val());
            //}
        })
    });

    function showModifyUI(roomId, curParam,index)
    {
        oModifyUI.show();
        var name = cdataJs.gameRoom[roomId];
        oModifyUI.find('h3').html(name + "参数修改");
        g_roomId = roomId;
        oModifyUI.find('#btnModifyParam').attr('index',index);
        init();

        function init()
        {
            opResInfo.html('');
            oMinEarn.val(curParam.minV);
            oMaxEarn.val(curParam.maxV);
            oStartEarn.val(curParam.startV);
            oExpEarn.val(curParam.expV);
            oMinControlEarn.val(curParam.minControlV);
            oMaxControlEarn.val(curParam.maxControlV);
        }
    }

    function reqOp(op, jsonParam, callBack) {

        var jd = {};
        $.extend(jd, jsonParam);
        jd.op = op;

        $.ajax({
            type: "POST",
            url: "/ashx/FishControl.ashx",
            data: jd,

            success: function (data) {
                callBack(data);
            }
        });
    }

    function findCurParam($obj)
    {
        var result = {};

        var tds = $obj.parent().parent().find('td');
        result.startV = tds.eq(INDEX_START_EARN).text();
        result.minV = tds.eq(INDEX_MIN_EARN).text();
        result.maxV = tds.eq(INDEX_MAX_EARN).text();
        result.expV = tds.eq(INDEX_EXP_EARN).text();
        result.minControlV = tds.eq(INDEX_MIN_CONTROL_EARN).text();
        result.maxControlV = tds.eq(INDEX_MAX_CONTROL_EARN).text();
        return result;
    }
});
});