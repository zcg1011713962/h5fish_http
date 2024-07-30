$(document).ready(function () {
    var OP_VIEW_BUFF = 2;
    var OP_REMOVE_BUFF = 1;
    var OP_CLEAR_CURR_OP_VALUE = 3;
    var BTN_ID = 0;

    // 刷新玩家列表
    $('#btnRefresh').click(function () {

        $.ajax({
            type: "POST",
            url: "/ashx/ShuihzScorePool.ashx",
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
            case OP_CLEAR_CURR_OP_VALUE:
                {
                    removeBuff(obj);
                }
                break;
        }
    }

    function viewBuff(jsonObj) {
        $('#buffPlayerList').html('');
        var head = '<tr><td>玩家ID</td><td>玩家昵称</td><td>BUFF水池上次操作时间</td><td>BUFF水池当前操作值</td><td>BUFF水池当前值</td><td>操作</td></tr>';
        var $bufferPlayerList = $('#buffPlayerList');
        $(head).appendTo($bufferPlayerList);
        var bList = JSON.parse(jsonObj.buffList);

        var len = bList.length;
        for (var i = 0; i < bList.length; i++) {
            var obj = bList[i];

            var btnId = "RemovePlayerBuff" + BTN_ID;
            BTN_ID++;
            var tstr = '<tr id="' + obj.m_playerId + '">' +
                '<td>' + obj.m_playerId + '</td>' +
                '<td>' + obj.m_nickName + '</td>' +
                '<td>' + obj.m_lastFixTime + '</td>' +
                '<td>' + obj.m_lastPoolSetVal + '</td>' +
                '<td>' + obj.m_playerScorePool + '</td>';

            if (obj.m_playerScorePool != 0) {
                tstr += '<td>' + '<input type="button" id="' + btnId + '" flag="1"  value="移除BUFF"/>' + '</td>';
            } else {
                tstr += '<td>' + '<input type="button" id="' + btnId + '" flag="3"  value="清零当前操作值"/>' + '</td>';
            }

            tstr += '</tr>';


            var t = $(tstr);
            $bufferPlayerList.append(t);

            $('#' + btnId).click(function () {
                var op = $(this).attr("flag");
                $.ajax({
                    type: "POST",
                    url: "/ashx/ShuihzScorePool.ashx",
                    data: {
                        op: op, "playerId": $(this).parent().parent().find('td').eq(0).html()
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
            $('#buffPlayerList').find('tr[id=' + obj.playerId + ']').remove();
        }
    }
});

