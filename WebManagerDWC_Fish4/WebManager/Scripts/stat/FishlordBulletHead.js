define(function (require, exports, module) {
    $(function () {
        var commonJs = require('../common.js');
        
        var reg = /^([0-9]|([1-9][0-9]*))(,[1-9][0-9]*)$/;
        $(".btn_edit").each(function () {
            $(this).click(function () {
                $("#MainContent_m_res").text("");
                var index = $(this) + 1;
                var id = $(this).attr("id");
                var editValue = $(this).parent().find(".input_data").val();
                var type = $(this).attr("op");
                if (!reg.test(editValue))
                {
                    alert("请以 a,b 格式填写（a,b为正整数且a<b）");
                    return false;
                }

                var goldKill_arr = editValue.split(',');
                var goldKillMin = goldKill_arr[0];
                var goldKillMax = goldKill_arr[1];
                if (goldKillMin >= goldKillMax) {
                    alert("请输入符合要求的数据格式");
                    return false;
                }
                $.ajax({
                    type: "POST",
                    url: "/ashx/FishlordBulletHead.ashx",
                    data: { "id": id, "goldKillMin": goldKillMin, "goldKillMax": goldKillMax,"type":type},
                    success: function (data) {
                        commonJs.checkAjaxScript(data);
                        console.log(data);
                        var arr = data.split('#');
                        if (arr[0] == 0) {
                            setSuccessData(index,type, arr[2]);
                        }

                        $("#MainContent_m_res").text(arr[1]);
                        return false;
                    }
                });
            });
        });
    });

    function setSuccessData(trIndex,tdIndex, newValue) {
        var td = $("#MainContent_m_result").find("tr").eq(parseInt(trIndex)).find('td').eq(tdIndex);
        td.text(newValue);
    }
});