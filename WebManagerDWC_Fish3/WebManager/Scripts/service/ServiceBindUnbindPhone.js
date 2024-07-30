define(function (require, exports, module) {
    $(function () {
        var oBindType = $("#MainContent_m_type");
        var oPlayerId = $("#MainContent_m_playerId");
        var oPhoneNo = $("#MainContent_m_phone");

        var oRes = $("#MainContent_m_res");
        var oTable = $("#MainContent_m_result");

        var BTN_ID = 0;

        //点击验证手机时 弹出确认框
        $("#MainContent_btn_vertify").click(function () {
            var res = confirm("确定验证手机？");
            if (res) //确认
            {
                $.ajax({
                    type: "POST",
                    url: "/ashx/ServiceBindUnbindPhone.ashx",
                    data: {
                        'opType': 1,
                        'playerId': oPlayerId.val(),
                        'phoneNo': oPhoneNo.val(),
                    },
                    success: function (data) {
                        var obj = JSON.parse(data);
                        var res = obj.result;
                        oRes.text(res);
                        ViewTable(obj);
                        return false;
                    }
                });
                return false;
            }
        });

        //查询验证码
        $("#MainContent_btn_query").click(function () {
            $.ajax({
                type: "POST",
                url: "/ashx/ServiceBindUnbindPhone.ashx",
                data: {
                    'opType': 4
                },
                success: function (data) {
                    var obj = JSON.parse(data);
                    ViewTable(obj);
                    return false;
                }
            });
            return false;
        });

        //绑定
        $("#MainContent_btn_click").click(function ()
        {
            var isBind = oBindType.get(0).selectedIndex;
            var res = confirm("确定绑定解绑手机？");
            if (res) //确认
            {
                $.ajax({
                    type: "POST",
                    url: "/ashx/ServiceBindUnbindPhone.ashx",
                    data: {
                        'opType': 2,
                        'isBind': isBind,
                        'playerId': oPlayerId.val(),
                        'phoneNo': oPhoneNo.val(),
                    },
                    success: function (data) {
                        oRes.text(data);
                        oTable.html("");
                        return false;
                    }
                });

                return false;
            }
        });

        function ViewTable(jsonObj) {
            oTable.html("");
            var head = '<tr><td>生成时间</td><td>玩家ID</td><td>手机号</td><td>验证码</td><td>操作</td></tr>';
            $(head).appendTo(oTable);
            var pList = JSON.parse(jsonObj.playerList);

            var len = pList.length;
            for (var i = 0; i < pList.length; i++) {
                var obj = pList[i];
                var btnId = "RemovePlayerCode" + BTN_ID;
                BTN_ID++;
                var tstr = '<tr id="' + obj.m_id + '">' +
                           '<td>' + obj.m_time +'</td>' +
                           '<td>' + obj.m_playerId + '</td>' +
                           '<td>' + obj.m_phoneNo + '</td>' +
                           '<td>' + obj.m_code + '</td>' +
                           '<td>' + '<input type="button" id="' + btnId + '" value="移除"/>' + '</td>' +
                           '</tr>';

                var t = $(tstr);
                oTable.append(t);
                $('#' + btnId).click(function () {
                    var id = $(this).parent().parent().attr("id");

                    $.ajax({
                        type: "POST",
                        url: "/ashx/ServiceBindUnbindPhone.ashx",
                        data: {
                            'opType': 3, "id": id
                        },
                        success: function (data) {
                            removeBuff(id);
                        }
                    });
                });
            }
        }

        function removeBuff(obj) {
                oTable.find('tr[id=' + obj + ']').remove();
        }

    });
});