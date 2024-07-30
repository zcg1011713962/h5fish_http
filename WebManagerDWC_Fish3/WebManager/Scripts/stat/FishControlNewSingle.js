define(function (require, exports, module) {
    $(function () {

        var oExpTable = $('#MainContent_m_expRateTable');
        var oModifyUI = $('#divModifyNewParam');

        var oBaseRate = $("#m_baseRate");
        var oDeviationFix = $("#m_deviationFix");
        var oNoValuePlayerRate = $("#m_noValuePlayerRate");

        var opResInfo = $('#opRes');

        var cdataJs = require('../cdata.js');

        //修改参数
        oExpTable.find('a').click(function () {

            var _this = $(this);
            var index = _this.parent().parent().index();
            var param = findCurParam(_this);
            showModifyUI(param, index);
        });

        oModifyUI.find('h2').click(function () {
            oModifyUI.hide();
            window.location.reload();
        });

        $('#btnModifyParam').click(function () {

            opResInfo.html('正在操作...');
            var index = $(this).attr('index');

            reqOp(0, {
                'baseRate': oBaseRate.val(),
                'deviationFix': oDeviationFix.val(),
                'noValuePlayerRate': oNoValuePlayerRate.val()
            }, function (data) {
                opResInfo.html(data);
            })
        });

        function showModifyUI(curParam, index) {
            oModifyUI.show();
            oModifyUI.find('#btnModifyParam').attr('index', index);
            init();

            function init() {
                opResInfo.html('');
                oDeviationFix.val(curParam.deviationFix);
                oBaseRate.val(curParam.baseRate);
                oNoValuePlayerRate.val(curParam.noValuePlayerRate);
            }
        }

        function reqOp(op, jsonParam, callBack) {

            var jd = {};
            $.extend(jd, jsonParam);
            jd.op = op;

            $.ajax({
                type: "POST",
                url: "/ashx/FishControlNewSingle.ashx",
                data: jd,

                success: function (data) {
                    callBack(data);
                }
            });
        }

        function findCurParam($obj) {
            var result = {};

            var tds = $obj.parent().parent().find('td');
            result.deviationFix = $obj.attr("deviationfix");
            result.baseRate = $obj.attr("baserate");
            result.noValuePlayerRate = $obj.attr("novalueplayerrate");
            return result;
        }
    });
});