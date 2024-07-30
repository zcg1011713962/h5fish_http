define(function (require,exports,module) {
    $(function () {
        var commonJs = require('./common.js');
       
        $(".btn_delete").each(function () {
            $(this).click(function () {
                var id = $(this).attr("id");
                var index = $(this).attr("index");

                $.ajax({
                    type: "POST",
                    url: "/ashx/cdkeyNew.ashx",
                    data: { "id": id },
                    success: function (data) {
                        commonJs.checkAjaxScript(data);
                        console.log(data);
                        var arr = data.split("#");
                        if (arr[0] == 0) {
                            setDeleteTr(index);
                        }
                        $("#MainContent_m_opRes").text("删除记录" + arr[1]);
                        $("#MainContent_m_res").text("");
                    }
                });
            });
        });
    });

    function setDeleteTr(trIndex) {
        $("#MainContent_m_result").find("tr").eq(parseInt(trIndex)).remove();
    }

});