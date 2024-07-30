define(function (require, exports, module) {
    $(function () {
        var idMaxCount = '#MainContent_txtMaxCount';
        var idExpTable = '#MainContent_m_result';
        var idRes = '#MainContent_m_res';
        var commonJs = require('../common.js');
        // 修改最大次数
        $("#btnModify").click(function () {
            var activityList = getActivityList();
            if (!activityList) {
                alert('请选择活动!');
                return;
            }

            var count = parseInt($(idMaxCount).val());
            if (isNaN(count)) {
                alert("输入非法");
                return;
            }

            if (count < 0) {
                alert('请输入正整数')
                return;
            }

            $.ajax({
                type: "POST",
                url: "/ashx/ActivityPanicBuyingCfg.ashx",
                data: { activityId: activityList, maxCount: count},
                success: function (data) {
                    commonJs.checkAjaxScript(data);
                    console.log(data);
                    var arr = data.split('#');
                    if (arr[0] == 0) {
                        setSuccessData(arr[3], 3, arr[2]);
                    }
                    $(idRes).text(arr[1]);
                }
            });
        });

        function getActivityList() {
            var str = '';
            var first = true;
            $(idExpTable + ' input[type=checkbox]').each(function (i, elem) {

                if ($(elem).is(':checked')) {
                    if (first) {
                        str += $(elem).val();
                        first = false;
                    }
                    else {
                        str += ',' + $(elem).val();
                    }
                }
            });
            return str;
        }

        function setSuccessData(activityList, tdIndex, newValue) {
            var aactivity = activityList.split(',');
            console.log(aactivity + '...........' + tdIndex);
            for (var i = 0; i < aactivity.length; i++) {
                var td = $(idExpTable).find("tr").eq(parseInt(aactivity[i])).find('td').eq(tdIndex);
                td.text(newValue);
            }
        }
    });
});