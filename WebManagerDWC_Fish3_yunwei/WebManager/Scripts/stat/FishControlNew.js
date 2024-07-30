define(function (require, exports, module) {
    $(function () {

        var oExpTable = $('#MainContent_m_expRateTable');
        var oModifyUI = $('#divModifyNewParam');

        //高级场
        var oJsckpotGrandPump = $("#m_jsckpotGrandPump"); //大奖
        var oJsckpotSmallPump = $("#m_jsckpotSmallPump");//小奖
        var oPoolPumpParam = $("#m_normalFishRoomPoolPumpParam"); //小鱼

        var oRateCtr = $("#m_rateCtr");
        var oCheckRate = $("#m_checkRate");
        var oTrickDeviationFix = $("#m_trickDeviationFix");

        var oIncomeThreshold = $("#m_incomeThreshold");
        var oEarnRatemCtrMax = $("#m_earnRatemCtrMax");
        var oEarnRatemCtrMin = $("#m_earnRatemCtrMin");

        var oLegendaryFishRate = $("#m_legendaryFishRate"); //鲲币转换系数

        var oMythicalScoreTurnRate = $("#m_mythicalScoreTurnRate"); //朱雀转化玄武系数
        var oMythicalFishRate = $("#m_mythicalFishRate"); // 玄武累分系数

        var opResInfo = $('#opRes');

        var cdataJs = require('../cdata.js');
        var g_roomId = 1;

        //修改参数
        oExpTable.find('a').not(".btn-detail").click(function () {

            var _this = $(this);
            var roomid = _this.attr('roomId');
            var index = _this.parent().parent().index();
            var param = findCurParam(_this);
            showModifyUI(roomid, param, index);


            $("#roomId_2").hide();
            $("#roomId_3").hide();
            $("#roomId_8").hide();
            $("#roomId_9").hide();
            if (roomid == 2 || roomid == 5 || roomid == 6 || roomid == 7)
            {
                $("#roomId_2").show();
            } else if (roomid == 3) {
                $("#roomId_2").show();
                $("#roomId_3").show();
            } else if(roomid == 8){
                $("#roomId_8").show();
            } else if (roomid == 9) {
                $("#roomId_9").show();
            }

        });

        oModifyUI.find('h2').click(function () {
            oModifyUI.hide();
            window.location.reload();
        });

        $('#btnModifyParam').click(function () {

            opResInfo.html('正在操作...');
            var index = $(this).attr('index');

            reqOp(0, {
                'room': g_roomId,

                'jsckpotGrandPump': oJsckpotGrandPump.val(),
                'jsckpotSmallPump': oJsckpotSmallPump.val(),
                'normalFishRoomPoolPumpParam': oPoolPumpParam.val(),

                'rateCtr': oRateCtr.val(),
                'checkRate': oCheckRate.val(),
                'trickDeviationFix': oTrickDeviationFix.val(),

                'incomeThreshold': oIncomeThreshold.val(),
                'earnRatemCtrMax': oEarnRatemCtrMax.val(),
                'earnRatemCtrMin': oEarnRatemCtrMin.val(),
                'legendaryFishRate': oLegendaryFishRate.val(),

                'mythicalScoreTurnRate': oMythicalScoreTurnRate.val(),
                'mythicalFishRate':oMythicalFishRate.val(),

            }, function (data) {
                opResInfo.html(data);
            })
        });

        function showModifyUI(roomId, curParam, index) {
            oModifyUI.show();
            var name = cdataJs.gameRoom[roomId];
            oModifyUI.find('h3').html(name + "参数修改");
            g_roomId = roomId;
            oModifyUI.find('#btnModifyParam').attr('index', index);
            init();

            function init() {
                opResInfo.html('');

                oJsckpotGrandPump.val(curParam.jsckpotGrandPump);
                oJsckpotSmallPump.val(curParam.jsckpotSmallPump);
                oPoolPumpParam.val(curParam.normalFishRoomPoolPumpParam);

                oRateCtr.val(curParam.rateCtr);
                oCheckRate.val(curParam.checkRate);
                oTrickDeviationFix.val(curParam.trickDeviationFix);

                oEarnRatemCtrMax.val(curParam.earnRatemCtrMax);
                oEarnRatemCtrMin.val(curParam.earnRatemCtrMin);
                oIncomeThreshold.val(curParam.incomeThreshold);

                oLegendaryFishRate.val(curParam.legendaryFishRate);

                oMythicalScoreTurnRate.val(curParam.mythicalScoreTurnRate);
                oMythicalFishRate.val(curParam.mythicalFishRate);
            }
        }

        function reqOp(op, jsonParam, callBack) {

            var jd = {};
            $.extend(jd, jsonParam);
            jd.op = op;

            $.ajax({
                type: "POST",
                url: "/ashx/FishControlNew.ashx",
                data: jd,

                success: function (data) {
                    callBack(data);
                }
            });
        }

        function findCurParam($obj) {
            var result = {};

            var tds = $obj.parent().parent().find('td');

            result.jsckpotGrandPump = $obj.attr("jsckpotGrandPump");
            result.jsckpotSmallPump = $obj.attr("jsckpotSmallPump");
            result.normalFishRoomPoolPumpParam = $obj.attr("normalfishroompoolpumpparam");

            result.rateCtr = $obj.attr("rateCtr");
            result.checkRate = $obj.attr("checkrate");
            result.trickDeviationFix = $obj.attr("trickDeviationFix");

            result.legendaryFishRate = $obj.attr("legendaryFishRate");

            result.mythicalScoreTurnRate = $obj.attr("mythicalScoreTurnRate");
            result.mythicalFishRate = $obj.attr("mythicalFishRate");

            result.earnRatemCtrMax = tds.eq(8).text();
            result.earnRatemCtrMin = tds.eq(9).text();
            result.incomeThreshold = tds.eq(10).text();
            return result;
        }
    });
});