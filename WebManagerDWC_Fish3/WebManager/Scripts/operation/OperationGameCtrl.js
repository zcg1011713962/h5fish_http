$(document).ready(function () {

    var OP_VIEW_BUFF = 2;
    var OP_REMOVE_BUFF = 1;
    var BTN_ID = 0;

    // 刷新玩家列表
    $('#btnRefresh').click(function () {
        $.ajax({
            type: "POST",
            url: "/ashx/OPerationGameCtrl.ashx",
            data: { op: OP_VIEW_BUFF },
            success: function (data)
            {
                processRet(data);
            }
        });
    });

    function processRet(data)
    {
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
        $('#channelVersionList').html('');
        var head = '<tr><td>时间</td><td>渠道号</td><td>版本号</td><td>炮倍率</td><td>VIP等级</td><td>功能模块</td><td>操作</td></tr>';
        var channelVersionList = $('#channelVersionList');
        $(head).appendTo(channelVersionList);
        console.log(jsonObj);
        var bList = JSON.parse(jsonObj.buffList);

        var len = bList.length;
        for (var i = 0; i < bList.length; i++) {
            var obj = bList[i];
            var btnId = "RemoveVersionCtrl" + BTN_ID;
            BTN_ID++;
            var tstr = '<tr id="' + obj.m_id + '">' +
                '<td>' + obj.m_time + '</td>' +
                '<td>' + obj.m_channelName + '</td>' +
                '<td>' + obj.m_version + '</td>'   +
                '<td>' + obj.m_turretName + '</td>'   +
                '<td>' + obj.m_vipLv + '</td>'   +
                '<td>' + obj.m_onOff + '</td>';

            //删除
            tstr += '<td>' + '<input type="button" id="' + btnId + '" flag="1"  value="移除"/>' + '</td>';
            tstr += '</tr>';

            var t = $(tstr);
            channelVersionList.append(t);

            $('#' + btnId).click(function () {
                var op = $(this).attr("flag");

                $.ajax({
                    type: "POST",
                    url: "/ashx/OPerationGameCtrl.ashx",
                    data: { op: op, "id": $(this).parent().parent().attr("id") },
                    success: function (data) {
                        processRet(data);
                    }
                });
            });
        }
    }

    function removeBuff(obj) {
        if (obj.result == 0) {
            $('#channelVersionList').find('tr[id=' + obj.m_id + ']').remove();
            $("#MainContent_m_res").html("删除成功");
        }
    }
});

