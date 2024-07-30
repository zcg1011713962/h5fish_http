define(function (require, exports, module) {
$(function () {

    //修改昵称
    $('input[id^=btn]').click(function () {

        var op = $(this).attr("op");   //1修改昵称  2修改贡献值
        var title = "";
        if (op == 1) {
            title = "修改昵称：";
        } else {
            title = "修改贡献值：";
        }

        var res = prompt(title);
        if (!res) //点击取消 该值为null
            return;

        var playerId = $(this).attr('playerId');

        req(op, playerId, res, function (data) {
            if (data == "操作成功")
                window.location.reload();
        })
    });

    function req(op, playerId, newVal, callBack)
    {
        $.ajax({
            type: "POST",
            url: "/ashx/PlayerBasicInfo.ashx",
            data: { 'playerId': playerId, 'newVal': newVal, 'op': op },

            success: function (data) {
                callBack(data);
            }
        });
    }
});

});
