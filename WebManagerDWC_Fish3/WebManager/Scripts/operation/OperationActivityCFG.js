define(function (require, exports, module) {
    $(function () {
        var oExpTable = $('#MainContent_m_result');
        var oModifyUI = $('#divModifyNewParam');
        var oStartTime = $('#m_startTime');
        var oEndTime = $('#m_endTime');

        var opResInfo = $('#opRes');

        var cdataJs = require('../cdata.js');

        //修改参数
        oExpTable.find('a').click(function () {

            var _this = $(this);
            var index = _this.parent().parent().index();
            var param = findCurParam(_this);
            showModifyUI(param);
        });

        oModifyUI.find('h2').click(function () {
            oModifyUI.hide();
            window.location.reload();
        });

        $('#btnModifyParam').click(function () {
            opResInfo.html('正在操作...');
            var itemId = $(this).attr('itemId');

            reqOp(0, {
                'itemId': itemId,
                'startTime': oStartTime.val(),
                'endTime': oEndTime.val()
            }, function (data) {
                opResInfo.html(data);
            })
        });

        function showModifyUI(curParam) {
            oModifyUI.show();
            oModifyUI.find('#btnModifyParam').attr("itemId", curParam.itemId);
            init();

            function init() {
                oStartTime.val(curParam.startTime);
                oEndTime.val(curParam.endTime);
            }
        }

        function reqOp(op, jsonParam, callBack) {

            var jd = {};
            $.extend(jd, jsonParam);
            jd.op = op;

            $.ajax({
                type: "POST",
                url: "/ashx/OperationActivityCFG.ashx",
                data: jd,

                success: function (data) {
                    callBack(data);
                }
            });
        }

        function findCurParam($obj) {
            var result = {};

            var tds = $obj.parent().parent().find('td');

            result.itemId = $obj.attr("itemId");
            result.startTime = $obj.attr("startTime");
            result.endTime = $obj.attr("endTime");
            return result;
        }
    });
});