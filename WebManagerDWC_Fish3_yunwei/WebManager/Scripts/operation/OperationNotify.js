define(function (require, exports, module)
{
    $(function () {
        var id = $("#m_noticeId");
        var title = $("#m_title");

        var start = $("#m_timeStart");
        var end = $("#m_timeEnd");
        var order = $("#m_order");
        var comment = $("#m_comment");

        var submit = $("#btn_publish");
        var resTable = $("#MainContent_m_result");
        var regTime_1 = /^(\d{4})\/([0|1]\d)\/([0|1|2]\d)$/;
        var regTime_2 = /^(\d{4})\/([0|1]\d)\/([0|1|2]\d) ([0|1|2]\d):([0-5]\d)$/;
        var regOrder = /^\d+$/;
        $('#m_res').text("");

        if (!window.btoa)
        {
            window.btoa = $.base64.btoa
        }

        resTable.find("a").click(function () {
            var _this = $(this);
            var noticeId = _this.attr("noticeId");
            $.ajax({
                type: "POST",
                url: "/ashx/OperationNotify.ashx",
                data: {
                    'op':1,'noticeId':noticeId
                },

                success: function (data) {
                    var msg = JSON.parse(data);
                    if (msg["res"] == 0)
                    {
                        var dataList = $.parseJSON(msg["dataList"]);
                        //console.log(dataList);
                        id.val(dataList.m_id);
                        title.val(dataList.m_title);
                        CKEDITOR.instances.m_content.setData(dataList.m_content);
                        start.val(dataList.m_startTime);
                        end.val(dataList.m_deadTime);
                        order.val(dataList.m_order);
                        comment.val(dataList.m_comment);
                    }
                }
            });
        });


        submit.click(function () {   
            var content = CKEDITOR.instances.m_content.getData();
            var content_data = window.btoa(unescape(encodeURIComponent(content)));
            //日期
            var time_1 = $.trim(start.val()); //开始日期
            var time_2 = $.trim(end.val());//结束日期

            //标题长度不大于6个汉字（英文占1个字符，中文汉字占2个字符）
            var titleStr = title.val(),titleLen = 0,charCode=-1;
            for (var i = 0; i < titleStr.length; i++)
            {
                charCode = titleStr.charCodeAt(i);
                if (charCode >= 0 && charCode <= 128) {
                    titleLen += 1;
                } else
                {
                    titleLen += 2;
                }
            }

            if (titleLen > 12)
            {
                $("#m_res").text("输入标题长度过长......");
                return false;
            }

            $.ajax({
                type: "POST",
                url: "/ashx/OperationNotify.ashx",
                data: {
                    'op':0,'id': id.val(), 'title': title.val(),
                    'content': content_data,
                    'start': time_1,
                    'end':time_2,'order':order.val(),'comment':comment.val()
                },

                success: function (data) {
                    $('#m_res').text(data);
                    id.val("");
                    title.val("");
                    CKEDITOR.instances.m_content.setData("");
                    start.val("");
                    end.val("");
                    order.val("");
                    comment.val("");
                }
            });
        });
    });
});