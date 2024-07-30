define(function (require, exports, module) {
    $(function () {
        $("#MainContent_m_result").find('a').click(function () {
            var _this = $(this);
            var id = _this.attr('id');
            $.ajax({
                type: "POST",
                url: "/ashx/Word2LogicItemError.ashx",
                data: {"id":id},
                success: function (data) {
                    if (data == "opres_success")
                    {
                        _this.parent().parent().find("td").eq(8).html("是");
                        _this.parent().parent().find("td").eq(4).children().eq(0).css("color", "red");
                        //_this.parent().parent().find("td").eq(8).html("");
                    }
                }
            });
        });
    });
});