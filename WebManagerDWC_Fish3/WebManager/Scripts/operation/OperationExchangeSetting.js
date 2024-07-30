define(function (require, exports, module) {
$(function () {

    var commonJs = require('../common.js');
    //////////////////////////////////////////////////////
    var OP_MODIFY = 2;
    var OP_GET_ALL = 3;

    var g_items = [
        { name: "30元话费", id:1 },
        { name: "50元话费", id: 2 },
        { name: "100元京东卡", id: 3 },
        { name: "青铜鱼雷x3", id: 4 },
        { name: "金币x300000", id: 5 },
        { name: "钻石x150", id: 6 },
        { name: "白银鱼雷x3", id: 7 },
        { name: "钻石鱼雷", id: 8 },
    ];
    //////////////////////////////////////////////////////

    function initBnt() {
        $('.btn').click(function () {
            var did = $(this).attr('data');
            //console.log('----------' + did);
            var v = $('#' + did).val();

            showOpResult('进行中', did);

            reqOp(OP_MODIFY, { id: did, val: v }, function (data) {
                var jdata = $.parseJSON(data);
                showOpResult(jdata["resultStr"], jdata["id"]);
            });
        });
    }

    function reqOp(op, jsonParam, callBack) {

        var jd = {};
        $.extend(jd, jsonParam);
        jd.op = op;

        $.ajax({
            type: "POST",
            url: "/ashx/OperationExchangeSetting.ashx",
            data: jd,

            success: function (data) {
                callBack(data);
            }
        });
    }

    function showOpResult(str, id) {

        $('#info_' + id).text(str);
    }

    function setData(id, val) {
        var elem = $('#' + id);
        elem.val(val);
    }

    function genContent() {
        var divContent = $('#container');
        divContent.empty();

        for (var i = 0; i < g_items.length; i++) {

            var obj = g_items[i];
            var hml = $('#divTemplate').html().format(obj.id, obj.name);
            //console.log(hml);
            divContent.append(hml);
            divContent.append("<br/>");
        }
    }

    function getAllData() {
        reqOp(OP_GET_ALL, {}, function (data) {

            var jdata = $.parseJSON(data);
            console.log(jdata);
            if (jdata["result"] == 0) {

                var dlist = jdata['data'];
                for (var d in dlist) {
                    setData(d, dlist[d]);
                }
            }
            
        });
    }

    // 初始化系统运行
    function initSystem() {
        genContent();
        initBnt();
        getAllData();
    }

    initSystem();

});
});