$(function () {
    var TODAY_MAX_SCORE = 2;
    var WEEK_MAX_SCORE = 3;

    var tr = $("#MainContent_m_result").find('tr').eq(1).find('td');
    var currTodayMaxScore = tr.eq(TODAY_MAX_SCORE).text();
    var currWeekMaxScore = tr.eq(WEEK_MAX_SCORE).text();

    $("#btnConfirm").click(function () {
        var m_param = $("#m_param").val();

        if (isNaN(m_param) || isNaN(parseInt(m_param)))
        {
            alert("输入参数非法");
        }
        else 
        {
            if (parseInt(m_param) >= parseInt(currWeekMaxScore))
            {
                $("#paramTips").text("新值 >= 周最高积分，确定继续修改？");
            } else if (parseInt(m_param) >= parseInt(currTodayMaxScore) && parseInt(m_param) < parseInt(currWeekMaxScore))
            {
                $("#paramTips").text("日最高积分 <= 新值 < 周最高值，确定继续修改？");
            } else
            {
                $("#paramTips").text("新值 < 日最高积分，确定继续修改？");
            }
            $('#divModifyNewParam').show();
        }
    });

    //取消
    $("#btnCancel").click(function () 
    {
        $('#divModifyNewParam').hide();
        $("#MainContent_m_res").text("");
    });
    //确定
    $("#btnModifyParam").click(function () {

        $('#divModifyNewParam').hide();

        var m_playerId = $("#MainContent_m_playerId").val();
        var m_param = $("#m_param").val();
        $.ajax({
            type: "POST",
            url: "/ashx/FishlordBaojinScoreAlter.ashx",
            data: { 'playerId':m_playerId,'scoreParam': m_param },
            success: function (data)
            {
                if (data == 0) {
                    $("#MainContent_m_res").text(" 操作成功");
                    var index = 0;

                    if (parseInt(currWeekMaxScore) < parseInt(m_param))
                    {
                        index = 2;
                    }else {
                        index = 1;
                    }

                    for (var i = 0; i < index; i++)
                    {
                        tr.eq(i+2).text(m_param);
                    }
                } else
                {
                    $("#MainContent_m_res").text(" 操作失败");
                }
            },

        });
    });
});