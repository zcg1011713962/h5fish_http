define(function (require, exports, module) {
    $(function () {
        $(".btn_remove").each(function () {
            $(this).click(function () {
                var btn = $(this);
                var _id = btn.attr("_id");
                $.ajax({
                    type: "POST",
                    url: "/ashx/StatWjlwDefRechargeReward.ashx",
                    data: { "id": _id },
                    success: function (data) {
                        var str_data = data.split('#');
                        var code = parseInt(str_data[0]);
                        var msg = str_data[1];
                        var str_id = str_data[2];
                        if (code == 0)
                            btn.parent().parent().remove();
                        
                        $("#MainContent_m_res").text(msg);
                        return false;
                    }
                });
            });
        });
    });
});