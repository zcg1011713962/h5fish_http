define(function(require, exports, module) {
$(function () {

    function setStartInfo(id, info) {
        $(id).text(info);
    }

    var idExpRate = '#MainContent_txtExpRate';
    var idExpTable = '#MainContent_m_expRateTable';
    var idRes = '#MainContent_m_res';
    //var expCheat = /^\s*(\d+)\s+(\d+)\s*$/;
    var expCheat = /^\s*(\d+)\s+(\d+)(;\s*(\d+)\s+(\d+))*$/;

    var commonJs = require('../common.js');

    $("#btnModifyLevel").click(function () {
        var roomList = getRoomList();
        if (!roomList)
        {
            alert('请选择房间!');
            return;
        }

        $.ajax({
            type: "POST",
            url: "/ashx/ShcdControl.ashx",
            data: { roomId: roomList, level: $('#level').val(), op: 1 },

            success: function (data) {
                commonJs.checkAjaxScript(data);

                var arr = data.split('#');
                $(idRes).text(arr[1]);

                //var tbody = $(idExpTable).find("tbody").eq(0);
                //tbody.children("tr").eq(1).children("td").eq(6).text(arr[1]);
                if(arr[0]==0)
                {
                    setSuccessData(arr[3], 6, arr[2]);
                }
            }
        });
    });

    // 修改大小王个数
    $("#btnModifyJokerCount").click(function () {
        var roomList = getRoomList();
        if (!roomList) {
            alert('请选择房间!');
            return;
        }

        var count = parseInt($(idExpRate).val());
        if (isNaN(count)) {
            alert("输入非法");
            return;
        }

        if (count < 0 || count > 8) {
            alert('请输入0-8之间的整数')
            return;
        }

        $.ajax({
            type: "POST",
            url: "/ashx/ShcdControl.ashx",
            data: { roomId: roomList, level: count, op: 2 },

            success: function (data) {
                commonJs.checkAjaxScript(data);

                var arr = data.split('#');
                if (arr[0] == 0) {
                    //var td = $(idExpTable).find("tr").eq(1).find('td').eq(7);
                    //td.text(arr[2]);
                    setSuccessData(arr[3], 6, arr[2]);
                }
                $(idRes).text(arr[1]);
            }
        });
    });

    // 修改最大作弊阀值
    $("#btnModifyMaxExpRate").click(function () {

        var roomList = getRoomList();
        if (!roomList) {
            alert('请选择房间!');
            return;
        }

        var maxExpVal = $(idExpRate).val();

        $.ajax({
            type: "POST",
            url: "/ashx/ShcdControl.ashx",
            data: { roomId: roomList, level: maxExpVal, op: 4 },

            success: function (data) {
                commonJs.checkAjaxScript(data);
                var arr = data.split('#');
                if (arr[0] == 0) {
                    setSuccessData(arr[3], 2, arr[2]);
                }
                $(idRes).text(arr[1]);
            }
        });

    });

    // 修改作弊开始结束局数
    $("#btnCheat").click(function () {

        var roomList = getRoomList();
        if (!roomList) {
            alert('请选择房间!');
            return;
        }

        var cheatVal = $(idExpRate).val();
        if (!isCheatStrValid(cheatVal))
        {
            alert('格式非法，应为 起始局数 + 空格 + 结束局数 ，多组用分号, 数字在[1,100]之间');
            return;
        }

        $.ajax({
            type: "POST",
            url: "/ashx/ShcdControl.ashx",
            data: { roomId: roomList, level: cheatVal, op: 3 },

            success: function (data) {
                commonJs.checkAjaxScript(data);

                var arr = data.split('#');
                if (arr[0] == 0) {
                    //var td = $(idExpTable).find("tr").eq(1).find('td').eq(8);
                    //td.text(arr[2]);

                    setSuccessData(arr[3], 7, arr[2]);
                }
                $(idRes).text(arr[1]);
            }
        });
        
    });

    $("#level").change(function () {
        var v = $(this).val();

        if (v === "0")
        {
            $("#MainContent_stat_common_StatCommonShcd_Content_Button2").attr("disabled", false);
        }
        else
        {
            $("#MainContent_stat_common_StatCommonShcd_Content_Button2").attr("disabled", true);
        }
    });

    function getRoomList()
    {
        var str = '';
        var first = true;
        $(idExpTable + ' input[type=checkbox]').each(function (i, elem) {

            if ($(elem).is(':checked'))
            {
                if (first)
                {
                    str += $(elem).val();
                    first = false;
                }
                else
                {
                    str += ',' + $(elem).val();
                }
            }
        });
        return str;
    }

    function setSuccessData(roomList, tdIndex, newValue)
    {
        var aroom = roomList.split(',');
        console.log(aroom + '...........' + tdIndex);
        for(var i = 0; i < aroom.length; i++)
        {
            var td = $(idExpTable).find("tr").eq(parseInt(aroom[i])).find('td').eq(tdIndex);
            td.text(newValue);
        }
    }

    function parseCheatStr(str)
    {
        var i = 0, k = 0;
        var arr = str.split(';');
        //console.log(arr);
        var result = [];
        var tmp = [];
        for(var i = 0; i < arr.length; i++)
        {
            tmp = [];
            var a = arr[i].split(' ');
            for (k = 0; k < a.length; k++)
            {
                if (a[k])
                {
                    tmp.push(a[k]);
                }
            }

            //console.log(tmp);
            var obj = { s: 0, e: 0 };
            obj.s = parseInt(tmp[0]);
            obj.e = parseInt(tmp[1]);
            result.push(obj);
        }
        return result;
    }

    function isCheatStrValid(str)
    {
        if (!expCheat.test(str))
            return false;

        var resArr = parseCheatStr(str);
        //console.log(resArr);
        for (var i = 0; i < resArr.length; i++)
        {
            var obj = resArr[i];
            if (obj.e > 100 || obj.s < 1 || obj.s > obj.e)
                return false;
        }

        return true;
    }
});


});