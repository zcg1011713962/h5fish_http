$(document).ready(function () {
    var OP_VIEW_BUFF = 2;
    var OP_REMOVE_BUFF = 1;
    var BTN_ID = 0;

    // 刷新玩家列表
    $('#btnRefresh').click(function () {

        $.ajax({
            type: "POST",
            url: "/ashx/ChannelOpenCloseGame.ashx",
            data: { op: OP_VIEW_BUFF },

            success: function (data) {
                processRet(data);
            }
        });

    });

    function processRet(data) {
        var obj = JSON.parse(data);
        var op = obj.op;

        switch (op) {
            case OP_VIEW_BUFF:
                {
                    viewBuff(obj);
                }
                break;
            case OP_REMOVE_BUFF:
                {
                    removeBuff(obj);
                }
        }
    }

    function viewBuff(jsonObj) {
        $('#channelOpenCloseGameList').html('');
        var head = '<tr><td>渠道</td><td>游戏开关</td><td>炮倍率</td><td>VIP等级</td><td>操作</td></tr>';
        var channelOpenCloseGameList = $('#channelOpenCloseGameList');
        $(head).appendTo(channelOpenCloseGameList);
        var bList = JSON.parse(jsonObj.dataList);

        var len = bList.length;
        for (var i = 0; i < bList.length; i++) {
            var obj = bList[i];

            var btnId = "RemoveOpenClose" + BTN_ID;
            BTN_ID++;
            var tstr = '<tr id="' + obj.m_channelNo + '">' +
                '<td>' + obj.m_channelName + '</td>' +
                '<td>' + obj.m_isCloseAll + '</td>' +
                '<td>' + obj.m_condGameLevel + '</td>' +
                '<td>' + obj.m_condVipLevel + '</td>' +
                '<td>' + '<input type="button" id="' + btnId + '" value="删除设置"/>' + '</td>' +
                '</tr>';

            var t = $(tstr);
            channelOpenCloseGameList.append(t);

            $('#' + btnId).click(function () {

                $.ajax({
                    type: "POST",
                    url: "/ashx/ChannelOpenCloseGame.ashx",
                    data: {
                        op: OP_REMOVE_BUFF, "channelNo": $(this).parent().parent().attr("id")
                    },
                    success: function (data) {
                        processRet(data);
                    }
                });
            });
        }
    }

    function removeBuff(obj) {
        if (obj.result == 0) {
            $('#channelOpenCloseGameList').find('tr[id=' + obj.channel + ']').remove();
        }
    }
});

