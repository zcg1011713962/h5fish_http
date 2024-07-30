$(function () {
    var tr = $("#MainContent_m_result").find('tr').eq(1).find('td');
    $("#btnConfirm").click(function () {
        var m_param = $("#m_param").val();
        var m_playerId = $("#MainContent_m_playerId").val();
        var m_nickName = $("#MainContent_m_nickName").val();

        if (isNaN(m_param) || isNaN(parseInt(m_param)) || isNaN(m_playerId) || isNaN(parseInt(m_playerId)))
        {
            alert("输入参数非法");
            return false;
        }

        if (parseInt(m_param) < 0)
        {
            alert("输入参数非法");
            return false;
        }

        $.ajax({
            type: "POST",
            url: "/ashx/DragonScaleControl.ashx",
            data: { 'playerId': m_playerId, 'scoreParam': m_param, 'nickname': m_nickName },
            success: function (data) {
                if (data == 0) {
                    $("#MainContent_m_res").text(" 操作成功");
                    tr.eq(2).text(m_param);
                    
                } else {
                    $("#MainContent_m_res").text(" 操作失败");
                }
            },
        });
    });
});