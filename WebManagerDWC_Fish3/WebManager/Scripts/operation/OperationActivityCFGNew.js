define(function (require, exports, module) {
    $(function () {
        var oExpTable = $('#MainContent_m_result');
        var oModifyUI = $('#divModifyNewParam');
        var oMonday = $('#m_monday');
        var oCode = $('#MainContent_m_code');
        var opResInfo = $('#opRes');

        var cdataJs = require('../cdata.js');

        //删除
        oExpTable.find('.btn_delete').click(function ()
        {
            if (confirm("确定删除？")) {
                var _this = $(this);
                var _id = _this.attr("id");

                reqOp(2, {
                    'id': _id
                }, function (data) {
                        alert(data);
                        window.location.reload();
                })
            }
        })

        //修改参数
        oExpTable.find('.btn_edit').click(function () {
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
            var _id = $(this).attr('id');
            var actId = $(this).attr('actId');


            reqOp(1, {
                'id': _id,
                'actId': actId,
                'monday': oMonday.val()
            }, function (data) {
                opResInfo.html(data);
            })
        });

        function showModifyUI(curParam) {
            oModifyUI.show();
            oModifyUI.find('#btnModifyParam').attr("id", curParam.id).attr("actId", curParam.actId);
            init();

            function init() {
                oMonday.val(curParam.monday);
            }
        }

        function reqOp(op, jsonParam, callBack) {

            var jd = {};
            $.extend(jd, jsonParam);
            jd.op = op;

            $.ajax({
                type: "POST",
                url: "/ashx/OperationActivityCFGNew.ashx",
                data: jd,

                success: function (data) {
                    callBack(data);
                }
            });
        }

        function findCurParam($obj) {
            var result = {};

            var tds = $obj.parent().parent().find('td');

            result.id = $obj.attr("id");
            result.actId = $obj.attr("actId");
            result.monday = $obj.attr("monday");
            return result;
        }
    });
});