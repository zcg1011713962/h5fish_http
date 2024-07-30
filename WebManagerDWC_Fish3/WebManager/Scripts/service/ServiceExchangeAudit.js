define(function (require, exports, module) {
    $(function () {
        $(".btn-op").each(function () {
            $(this).click(function () {
                var res = confirm("确认次操作？");
                if (res) //确认
                {
                    var btn = $(this);
                    var _id = btn.attr("id");
                    var op = btn.attr("op");

                    $.ajax({
                        type: "POST",
                        url: "/ashx/ServiceExchangeAudit.ashx",
                        data: { "id": _id, "op": op },
                        success: function (data) {
                            var str_data = data.split('#');
                            var code = parseInt(str_data[0]);
                            var msg = str_data[1];
                            if (code == 0)
                                window.location.reload();
                            //btn.parent().parent().remove();

                            $("#MainContent_m_res1").text(msg);
                            return false;
                        }
                    });
                }

            });
        });
    });
});