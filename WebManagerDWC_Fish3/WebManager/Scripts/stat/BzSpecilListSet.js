$(document).ready(function () {
    var OP_VIEW_BUFF = 2;
    var OP_REMOVE_BUFF = 1;
    var BTN_ID = 0;

    // 刷新玩家列表
    $('#btnRefresh').click(function () {

        $.ajax({
            type: "POST",
            url: "/ashx/BzSpecilList.ashx",
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
                break;
        }
    }

    function viewBuff(jsonObj) {
        $('#playerSpecilList').html('');
        var head = '<tr><td>玩家ID</td><td>状态</td><td>操作</td></tr>';
        var playerSpecilList = $('#playerSpecilList');
        $(head).appendTo(playerSpecilList);
        var bList = JSON.parse(jsonObj.buffList);

        var len = bList.length;
        for (var i = 0; i < bList.length; i++) {
            var obj = bList[i];
            var btnId = "RemovePlayerBuff" + BTN_ID;
            BTN_ID++;
            var tstr = '<tr id="' + obj.m_playerId + '">' +
                       '<td>' + obj.m_playerId + '</td>' +
                       '<td>' + obj.m_state + '</td>' +
                       '<td>' + '<input type="button" id="' + btnId + '" value="移除BUFF"/>' + '</td>' +
                       '</tr>';

            var t = $(tstr);
            playerSpecilList.append(t);

            $('#' + btnId).click(function () {
                $.ajax({
                    type: "POST",
                    url: "/ashx/BzSpecilList.ashx",
                    data: {
                        op: OP_REMOVE_BUFF, "playerId": $(this).parent().parent().find('td').eq(0).html()
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
            $('#playerSpecilList').find('tr[id=' + obj.playerId + ']').remove();
        }
    }
});

