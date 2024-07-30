define(function (require, exports, module) {
    $(function () {

        var oExpTable = $('#MainContent_m_expRateTable');
        var oModifyUI = $('#divModifyNewParam');

        //高级场
        var oJsckpotGrandPump = $("#m_jsckpotGrandPump"); //大奖抽水系数
        var oJsckpotSmallPump = $("#m_jsckpotSmallPump");//小奖抽水系数
        var oPoolPumpParam = $("#m_normalFishRoomPoolPumpParam"); //小鱼抽水系数

        var oRateCtr = $("#m_rateCtr");   // 机率控制
        var oCheckRate = $("#m_checkRate"); // 刷新周期
        var oTrickDeviationFix = $("#m_trickDeviationFix");  // 玩家误差值

        var oIncomeThreshold = $("#m_incomeThreshold");  // 码量控制值
        //var oEarnRatemCtrMax = $("#m_earnRatemCtrMax");  // 控制范围上限
        //var oEarnRatemCtrMin = $("#m_earnRatemCtrMin");  // 控制范围下限
        var oBigPoolPumpRate = $("#m_bigPoolPumpRate");  // 抽水率
        var oBigPoolExpectedEarnRate = $("#m_bigPoolExpectedEarnRate");  // 期望盈利率

        //var oLegendaryFishRate = $("#m_legendaryFishRate"); //鲲币转换系数

        var oMythicalScoreTurnRate = $("#m_mythicalScoreTurnRate"); //朱雀转化玄武系数
        var oMythicalFishRate = $("#m_mythicalFishRate"); // 玄武累分系数

        var oColorFishCtrCount1 = $("#m_colorFishCtrCount1"); // 捕获次数一档
        var oColorFishCtrCount2 = $("#m_colorFishCtrCount2");  // 捕获次数二档

        var oColorFishCtrRate1 = $("#m_colorFishCtrRate1"); // 花色鱼命中系数（一档）
        var oColorFishCtrRate2 = $("#m_colorFishCtrRate2"); // 花色鱼命中系数（二档）

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
            //$("#roomId_8").hide();
            $("#roomId_9").hide();
            $("#roomId_2_add").hide();
            if (roomid == 2 || roomid == 5 || roomid == 6 || roomid == 7 || roomid == 10)
            {
                $("#roomId_2").show();
                if (roomid == 2)
                    $("#roomId_2_add").show();

            } else if (roomid == 3) {
                $("#roomId_2").show();
                $("#roomId_3").show();
            }
            //else if (roomid == 8) {
            //    $("#roomId_8").show();
            //}
            else if (roomid == 9) {
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
                //'earnRatemCtrMax': oEarnRatemCtrMax.val(),
                //'earnRatemCtrMin': oEarnRatemCtrMin.val(),

                'bigPoolPumpRate': oBigPoolPumpRate.val(),
                'bigPoolExpectedEarnRate': oBigPoolExpectedEarnRate.val(),

                //'legendaryFishRate': oLegendaryFishRate.val(),

                'mythicalScoreTurnRate': oMythicalScoreTurnRate.val(),
                'mythicalFishRate': oMythicalFishRate.val(),

                'colorFishCtrCount1': oColorFishCtrCount1.val(),
                'colorFishCtrCount2': oColorFishCtrCount2.val(),
                'colorFishCtrRate1': oColorFishCtrRate1.val(),
                'colorFishCtrRate2': oColorFishCtrRate2.val(),

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

                //oEarnRatemCtrMax.val(curParam.earnRatemCtrMax);
                //oEarnRatemCtrMin.val(curParam.earnRatemCtrMin);
                oBigPoolPumpRate.val(curParam.bigPoolPumpRate);
                oBigPoolExpectedEarnRate.val(curParam.bigPoolExpectedEarnRate);

                oIncomeThreshold.val(curParam.incomeThreshold);

                //oLegendaryFishRate.val(curParam.legendaryFishRate);

                oMythicalScoreTurnRate.val(curParam.mythicalScoreTurnRate);
                oMythicalFishRate.val(curParam.mythicalFishRate);

                oColorFishCtrCount1.val(curParam.colorFishCtrCount1);
                oColorFishCtrCount2.val(curParam.colorFishCtrCount2);
                oColorFishCtrRate1.val(curParam.colorFishCtrRate1);
                oColorFishCtrRate2.val(curParam.colorFishCtrRate2);
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

            //result.legendaryFishRate = $obj.attr("legendaryFishRate");

            result.mythicalScoreTurnRate = $obj.attr("mythicalScoreTurnRate");
            result.mythicalFishRate = $obj.attr("mythicalFishRate");

            result.colorFishCtrCount1 = $obj.attr("colorFishCtrCount1");
            result.colorFishCtrCount2 = $obj.attr("colorFishCtrCount2");
            result.colorFishCtrRate1 = $obj.attr("colorFishCtrRate1");
            result.colorFishCtrRate2 = $obj.attr("colorFishCtrRate2");

            //result.earnRatemCtrMax = tds.eq(8).text();
            //result.earnRatemCtrMin = tds.eq(9).text();
            result.bigPoolExpectedEarnRate = tds.eq(8).text();
            result.bigPoolPumpRate = tds.eq(9).text();

            result.incomeThreshold = tds.eq(10).text();
            return result;
        }
    });
});