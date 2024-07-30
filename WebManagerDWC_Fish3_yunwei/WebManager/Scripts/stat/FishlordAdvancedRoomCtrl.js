define(function (require, exports, module) {
    $(function () {
        var commonJs = require('../common.js');

        var reg = /^\s*(\d+)\s*$/;

        $(".btn_edit").each(function () {
            $(this).click(function () {
                $("#MainContent_m_res").text("");

                var rewardRatio = 0;
                var maxWinCount = 0;


                var type = $(this).attr("op");
                
                if (type == -1 || type == -2 || type == -3 || type == -4 || type == -5) 
                {
                    rewardRatio = $(this).parent().find(".input_data").val();
                    if (!reg.test(rewardRatio) || parseInt(rewardRatio) < 0) 
                    {
                        alert("参数输入错误");
                        return false;
                    }

                    if ((type == -3 || type == -5) && rewardRatio > 100)
                    {
                        alert("参数输入错误");
                        return false;
                    }

                } else if(type == 3) //奖池期望值
                {
                    rewardRatio = $(this).parent().parent().find("td").eq(1).find(".rewardRatio").val();
                    if (!reg.test(rewardRatio) || parseInt(rewardRatio) < 0)
                    {
                        alert("参数输入错误");
                        return false;
                    }
                }
                else if (type == 4 || type == 5) //控制系数
                {
                    rewardRatio = $(this).parent().parent().find("td").eq(1).find(".rewardRatio").val();
                    if (!reg.test(rewardRatio) || parseInt(rewardRatio) < 0 || parseInt(rewardRatio) > 100) {
                        alert("参数输入错误");
                        return false;
                    }
                }
                else
                {
                    maxWinCount = $(this).parent().parent().find("td").eq(1).find(".maxWinCount").val();
                    rewardRatio = $(this).parent().parent().find("td").eq(2).find(".rewardRatio").val();

                    if (!reg.test(maxWinCount) || parseInt(maxWinCount) < 0 || 
                        !reg.test(rewardRatio) || parseInt(rewardRatio) < 0 || parseInt(rewardRatio) > 100)
                    {
                        alert("参数输入错误");
                        return false;
                    }
                }

                $.ajax({
                    type: "POST",
                    url: "/ashx/StatFishlordAdvancedRoomCtrl.ashx",
                    data: {
                        "op": type, "maxWinCount": maxWinCount, "rewardRatio": rewardRatio
                    },
                    success: function (data) {
                        commonJs.checkAjaxScript(data);
                        console.log(data);
                        var arr = data.split('#');

                        $("#MainContent_m_res").text(arr[1]);
                        return false;
                    }
                });
            });
        });
    });
});