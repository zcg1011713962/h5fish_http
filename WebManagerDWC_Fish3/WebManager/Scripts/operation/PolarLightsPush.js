define(function (require, exports, module){
    $(function () {
        //op   0添加 1编辑 2删除 3查询
        var oModifyUI = $('#divModifyNewParam');
        var oAdd = $('#btn_add');
        var oClose = $('#btn_close');
        var oAllChannel = $('#MainContent_m_channel_0');
        var oAllDay = $('#MainContent_m_week_0');
        var BTN_ID = 0;
        init();
        ///////////////////////////////////////////////////////////////////////////////////////////
        //渠道   如果全选，其他全不选
        oAllChannel.click(function () {
            if ($(this).prop("checked")) {
                $("#MainContent_m_channel input[type='checkbox']:checked").not($(this)).attr("checked", false);
            }
        });
        //如果有全选 ，再选中其他时，全选禁用
        $("#MainContent_m_channel input[type='checkbox']").not(oAllChannel).each(function () {
            $(this).click(function () {
                if ($(this).prop("checked") && oAllChannel.prop("checked")) {
                    oAllChannel.attr("checked", false);
                }
            });
        });

        //日期  如果每天，其他全不选
        oAllDay.click(function () {
            if ($(this).prop("checked")) {
                $("#MainContent_m_week input[type='checkbox']:checked").not($(this)).attr("checked", false);
            }
        });
        //如果有每天 ，再选中其他时，全选禁用
        $("#MainContent_m_week input[type='checkbox']").not(oAllDay).each(function () {
            $(this).click(function () {
                if ($(this).prop("checked") && oAllDay.prop("checked")) {
                    oAllDay.attr("checked", false);
                }
            });
        });
        ///////////////////////////////////////////////////////////////////////////////////////////
        //新增
        oAdd.click(function () {
            showModifyUI();
            $("#btn_submit").attr("data","");
            //关闭
            oClose.click(function () {
                hideModifyUI();
            });
        });
        
        //当点击提交时  //区分是新增还是编辑  看btn的data属性是否为空 空 op=0新增 不空op=1编辑
        $('#btn_submit').click(function () {
            var id = $(this).attr('data');
            btnSubmit(id);
        });

        ////////////////////////////////////////////////////////////////////////
        function init()
        {
            reqOp(3, {}, function (data) {
                    var obj = JSON.parse(data);
                    viewList(obj);
            });
        }

        //显示 删除 编辑
        function viewList(jsonObj) {
            var dataList = [];
            $('#MainContent_m_setListTable').html('');
            
            var $pushList = $('#MainContent_m_setListTable');
            
            var bList = JSON.parse(jsonObj.queryList);
            var len = bList.length;

            //如果查询没有，则显示表头
            if (len > 0)
            {
                var head = '<tr>' +
                               '<td>编号</td>' +
                               '<td>目标玩家渠道</td>' +
                               '<td>目标玩家VIP</td>' +
                               '<td>推送日期区间</td>' +
                               '<td>推送日期星期</td>' +
                               '<td>推送时间</td>' +
                               '<td>推送内容</td>' +
                               '<td>备注</td>' +
                               '<td>操作</td>' +
                           '</tr>';
                $(head).appendTo($pushList);
            }

            for (var i = 0; i < bList.length; i++) {
                var obj = bList[i];
                dataList[obj.m_id] = obj;
                var btnId1 = "EditPush" + BTN_ID;
                var btnId2 = "RemovePush" + BTN_ID;
                BTN_ID++;
                var tstr = '<tr id="' + obj.m_id + '">' +
                    '<td>' + obj.m_id + '</td>' +
                    '<td>' + obj.m_channelList + '</td>' +
                    '<td>' + obj.m_vipList + '</td>' +
                    '<td>' + obj.m_date + '</td>' +
                    '<td>' + obj.m_weekList + '</td>' +
                    '<td>' + obj.m_time + '</td>'+
                    '<td>' + obj.m_content + '</td>' +
                    '<td>' + obj.m_note + '</td>';
                tstr += '<td>' + '<input type="button" id="' + btnId1 + '" value="编辑" />&ensp;'+
                                 '<input type="button" id="' + btnId2 + '" value="删除" />' + '</td>';
                tstr += '</tr>';
                var t = $(tstr);
                $pushList.append(t);
                
                //编辑
                $('#' + btnId1).click(function () {
                    showModifyUI();
                    var id = $(this).parent().parent().attr("id");
                    $('#btn_submit').attr('data',id);
                    var obj_edit = dataList[id];
                    //渠道
                    if (obj_edit.channelList == -1) {
                        $("#MainContent_m_channel_0").prop("checked", "checked");
                    } else {
                        var channel_arr = obj_edit.channelList.split(',');
                        $('#MainContent_m_channel input[type=checkbox]').each(function () {
                            var res = $.inArray($(this).val(), channel_arr);
                            if (res != -1) {
                                $(this).prop("checked", "checked");
                            }
                        });
                    }
                    //VIP暂时不设置
                    //日期区间
                    $('#m_date').val(obj_edit.date);
                    //星期
                    if (obj_edit.weekList == 0) {
                        $("#MainContent_m_week_0").prop("checked", "checked");
                    } else {
                        var week_arr = obj_edit.weekList.split(',');
                        $('#MainContent_m_week input[type=checkbox]').each(function () {
                            var res = $.inArray($(this).val(), week_arr);
                            if (res != -1) {
                                $(this).prop("checked", "checked");
                            }
                        });
                    }
                    //时间
                    $('#m_time1').val(obj_edit.m_time);
                    //内容
                    $('#m_content').val(obj_edit.m_content);
                    //备注
                    $('#m_note').val(obj_edit.m_note);
                  
                    //关闭
                    oClose.click(function () {
                        hideModifyUI();
                    });
                });
                //删除
                $('#' + btnId2).click(function () {
                    var id = $(this).parent().parent().attr("id");
                    reqOp(2, {
                        "id": id
                    }, function (data) {
                        $("#MainContent_m_res").text(data);
                        if (data = "操作成功") {
                            init();
                        }
                    });
                });
            }
        }
        function btnSubmit(param) {
            var op = 0;
            if (param != "")
            {
                op = 1;
            }
            //渠道
            var channelList = "";
            $('#MainContent_m_channel input[type=checkbox]:checked').each(function () {
                if (channelList == "") {
                    channelList = $(this).val();
                } else {
                    channelList = channelList + ',' + $(this).val();
                }
            });
            if (channelList == "") {
                $('#MainContent_m_opRes').text("请选择渠道......");
                return false;
            }
            //VIP
            var vip = $("#m_vip").prop('selectedIndex');
            //日期
            var date = $("#m_date").val();
            if (date == "") {
                $('#MainContent_m_opRes').text("请选择日期区间......");
                return false;
            }
            //星期
            var weekList = "";
            $("#MainContent_m_week input[type=checkbox]:checked").each(function () {
                if (weekList == "") {
                    weekList = $(this).val();
                } else {
                    weekList = weekList + ',' + $(this).val();
                }
            });
            if (weekList == "") {
                $('#MainContent_m_opRes').text("请选择星期......");
                return false;
            }
            //时间
            var regTime = /^(0\d{1}|1\d{1}|2[0-3]):([0-5]\d{1})$/;
            var time = $.trim($('#m_time1').val());
            if (!regTime.test(time)) {
                $('#MainContent_m_opRes').text("请按正确格式输入时间......");
                return false;
            }
            //推送内容
            var content = $.trim($('#m_content').val());
            if (content == "") {
                $('#MainContent_m_opRes').text("请输入推送内容......");
                return false;
            }
            //备注
            var note = $.trim($('#m_note').val());
            reqOp(op, {
                'id':param,
                'channelList': channelList,
                'vip': vip,
                'date': date,
                'weekList': weekList,
                'time': time,
                'content': content,
                'note': note
            }, function (data) {
                hideModifyUI();
                $("#MainContent_m_res").text(data);
                if (data = "操作成功") {
                    init();
                }
            });
        }

        function reqOp(op, jsonParam, callBack)
        {
            var jd = {};
            $.extend(jd, jsonParam);
            jd.op = op;

            $.ajax({
                type: "POST",
                url: "/ashx/PolarLightsPush.ashx",
                data: jd,
                success: function (data) {
                    callBack(data)
                }
            });
        }

        function hideModifyUI()
        {
            oModifyUI.hide();
            $('input[type=checkbox]').attr('checked', false);
            $('#m_vip').selectedIndex = 0;
            $('#m_date').val("");
            $('#m_time1').val("");
            $('#m_content').val("");
            $('#m_note').val("");
        }

        function showModifyUI()
        {
            oModifyUI.show();
            $('#MainContent_m_opRes').text("");
        }
    });
});