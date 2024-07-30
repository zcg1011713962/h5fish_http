define(function (require, exports, module) {
$(function () {

    var OP_GEN_PICI = 1;
    var OP_SEARCH_PICI = 2;
    var OP_EXPORT_PICI = 3;
    var OP_GET_GIFTS = 4;
    var OP_EXPORT_CDKEY = 5;  //导出cdkey
    var OP_CHANGE_PICI_STATE = 6;
    var OP_MODIFY_PICI = 7;
    var OP_RELOAD_PICI = 8;
    var OP_QUERY_CDKEY = 9;

    var g_curGenPiciId = 0;

    var g_dataGift = {};
    var g_fun = {};

    function init()
    {
        reqOp(OP_GET_GIFTS, {}, function (data) {
            var jd = $.parseJSON(data);
            g_dataGift = jd['gift'];
            g_curGenPiciId = jd['curPici']['key'];
            console.log(g_curGenPiciId);
            afterInit();
        });
    }

    function afterInit()
    {
        (function () {

            var oPici = $('#pici');
            var oValidDays = $('#validDays');
            var oGiftList = $('#giftList');
            var oCount = $('#cdkeyCount');
            var oInfoFoGenPici = $('#infoForGenPici');

            $('#btnSubmit').click(function () {

                oInfoFoGenPici.html('');

                reqOp(OP_GEN_PICI, {

                    'pici': oPici.val(),
                    'validDays': oValidDays.val(),
                    'count': oCount.val(),
                    'giftId': oGiftList.val()
                },

                function (data) {

                    var jd = $.parseJSON(data);
                    g_curGenPiciId = jd['curPici'];
                    oInfoFoGenPici.html(jd['result']);
                    resetGenInfo();
                });
            });

            function fillGiftList()
            {
                oGiftList.empty();
                var str = '';
                var isSel = true;
                for(var d in g_dataGift)
                {
                    str += option(d, g_dataGift[d], isSel);
                    isSel = false;
                }

                oGiftList.html(str);
            }

            function resetGenInfo()
            {
                oPici.val(g_curGenPiciId);
                oValidDays.val('');
                oCount.val('');
            }

            fillGiftList();
            resetGenInfo();
        })();

        (function () {

            var oTablePic = $('#tablePici');
            var oModifyPici = $('#ModifyPici');
            var oGiftListM = $('#giftListM');
            var oTableCDKEY = $('#tableCDKEY');
            var aPiciData = {};

            function getPiciData(pici)
            {
                for (var i = 0; i < aPiciData.length; i++)
                {
                    if (pici == aPiciData[i].pici)
                        return aPiciData[i];
                }
                return null;
            }

            function queryPici()
            {
                reqOp(OP_SEARCH_PICI,{}, function (data) {

                    oTablePic.empty();
                    var str = genHead();
                    var jd = $.parseJSON(data);
                    aPiciData = jd['piciList'];

                    for (var i = 0; i < aPiciData.length; i++)
                    {
                        var obj = aPiciData[i];
                        str += '<tr>';
                        str += cell(obj.pici);
                        str += cell(g_dataGift[obj.giftId]);
                        str += cell(obj.count);
                        //str += cell(obj.validDays);
                        str += cell(obj.genTime);
                        str += cell(obj.deadTime);
                        str += cell(obj.enable ? '是' : '否');
                        str += cell(alink('修改', {'pici':obj.pici, 'op':'modify'}));
                        str += cell(alink('启用', { 'pici': obj.pici, 'op': 'start' }));
                        str += cell(alink('停用', { 'pici': obj.pici, 'op': 'stop' }));
                        str += cell(alink('导出cdkey', { 'pici': obj.pici, 'op': 'export' }));
                        str += cell(alink('通知服务器重载数据', { 'pici': obj.pici, 'op': 'reload' }));
                        str += '</tr>';
                    }
                    
                    oTablePic.html(str);
                    addOpEvent();
                });
            }

            function addOpEvent()
            {
                oTablePic.find('a').click(function () {

                    var op = $(this).attr('op');
                    var pici = $(this).attr('pici');

                    switch(op)
                    {
                        case 'modify':
                            {
                                showModifyUI(pici);
                            }
                            break;
                        case 'start':
                            {
                                startOrStop(pici, true);
                            }
                            break;
                        case 'stop':
                            {
                                startOrStop(pici, false);
                            }
                            break;
                        case 'export':
                            {
                                exportExcel(pici);
                            }
                            break;
                        case 'reload':
                            {
                                reload(pici);
                            }
                            break;
                    }
                });
            }

            function genHead()
            {
                var str = '<tr>';
                str += cell('批次号');
                str += cell('对应礼包');
                str += cell('cdkey生成数量');
                //str += cell('有效天数');
                str += cell('生成日期');
                str += cell('失效日期');
                str += cell('是否启用');
                str += cell('');
                str += cell('');
                str += cell('');
                str += cell('');
                str += '</tr>';
                return str;
            }

            function showModifyUI(pici)
            {
                var opiciM = $('#piciM');
                var oValidDaysM = $('#validDaysM');
                var oBtnModify = $('#btnModify');
                var oTxtInfoModify = $('#txtInfoModify');
                oTxtInfoModify.text('');

                oModifyPici.show();
                oModifyPici.css('width', $(window).width()).css('height', $(window).height()).show();

                var data = getPiciData(pici);
                if(data != null)
                {
                    opiciM.val(data.pici);
                    oValidDaysM.val(data.validDays);
                    fillDropGiftList(data.giftId);
                }

                oBtnModify.off().click(function(){
                    
                    reqOp(OP_MODIFY_PICI, {

                        'pici': opiciM.val(),
                        'validDays': oValidDaysM.val(),
                        'giftId': oGiftListM.val()
                    }, function (data) {
                        oTxtInfoModify.text(data);
                    });
                });
            }

            function fillDropGiftList(curGiftId) {
                oGiftListM.empty();
                var str = '';
                var isSel = true;
                for (var d in g_dataGift) {

                    if (d == curGiftId)
                    {
                        isSel = true;
                    }
                    else
                    {
                        isSel = false;
                    }
                    str += option(d, g_dataGift[d], isSel);
                }

                oGiftListM.html(str);
            }

            function exportExcel(pici)
            {
                reqOp(OP_EXPORT_CDKEY, { 'pici': pici }, function (data) {

                    alert(data);

                });
            }

            function startOrStop(pici, enable)
            {
                reqOp(OP_CHANGE_PICI_STATE, { 'pici': pici, 'enable':enable }, function (data) {

                    alert(data);

                });
            }

            function reload(pici)
            {
                reqOp(OP_RELOAD_PICI, { 'pici': pici}, function (data) {

                    alert(data);

                });
            }

            function initSearch()
            {
                oModifyPici.find('a').click(function () {

                    oModifyPici.hide();
                });

                $(window).resize(function () {
                    oModifyPici.css('width', $(window).width()).css('height', $(window).height());
                });

                $('#btnQueryCDKEY').click(function () {
                    queryCDKEY();
                });
            }

            function queryCDKEY() {
                reqOp(OP_QUERY_CDKEY, { 'cdkey': $('#cdkeyNum').val() }, function (data) {

                    oTableCDKEY.empty();
                    var jd = $.parseJSON(data);
                    if (jd.result == 0)
                    {
                        var str = genCDKEYTableHead();
                        str += '<tr>';
                        str += cell(jd.cdkey);
                        str += cell(jd.genTime);
                        if (jd.playerId > 0) {
                            str += cell(jd.useTime);
                            str += cell(jd.playerId);
                        }
                        else {
                            str += cell('');
                            str += cell('');
                        }
                        oTableCDKEY.html(str);
                    }
                    else
                    {
                        oTableCDKEY.html(jd.resultStr);
                    }
                });

                function genCDKEYTableHead()
                {
                    var str = '<tr>';
                    str += cell('cdkey');
                    str += cell('生成时间');
                    str += cell('使用时间');
                    str += cell('玩家id');
                    str += '</tr>';
                    return str;
                }
            }

            g_fun['op2'] = queryPici;

            initSearch();

        })();

        $('.SelCard li').click(function () {
            var did = $(this).attr('data');
            $('#' + did).show().siblings().hide();
            $(this).attr('class', 'Active').siblings().attr('class', '');

            if (g_fun[did]) {
                g_fun[did]();
            }
        });
    }

    function option(val, txt, isSel)
    {
        var str = '';
        if(isSel)
        {
            str = '<option selected value="' + val + '">';
        }
        else
        {
            str = '<option value="' + val + '">';
        }
        str += txt;
        str += '</option>';
        return str;
    }

    function cell(txt)
    {
        var str = '<td>';
        str += txt;
        str += '</td>'
        return str;
    }

    function alink(txt, prop)
    {
        var str = '<a href="javascript:;" ';
        for (var p in prop)
        {
            str += ' ' + p + '=';
            str += prop[p];
        }

        str += '>';
        
        str += txt;
        str += '</a>';
        return str;
    }

    function reqOp(op, jsonParam, callBack) {

        var jd = {};
        $.extend(jd, jsonParam);
        jd.op = op;

        $.ajax({
            type: "POST",
            url: "/ashx/CDKEY.ashx",
            data: jd,

            success: function (data) {
                callBack(data);
            }
        });
    }

    init();

});
});