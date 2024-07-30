define(function (require, exports, module) {
    $(function () {
        $(".btn-primary").each(function () {
            $(this).click(function () {
                var btn = $(this);
                var _id = btn.attr("id");
                $.ajax({
                    type: "POST",
                    url: "/ashx/StatAirdropSysCtrl.ashx",
                    data: { "id": _id },
                    success: function (data) {
                        var str_data = data.split('#');
                        var code = parseInt(str_data[0]);
                        var msg = str_data[1];
                        if (code == 0)
                            btn.parent().parent().remove();

                        $("#MainContent_m_res1").text(msg);
                        return false;
                    }
                });
            });
        });
    });
});