﻿define(function (require, exports, module) {
$(function () {
    var cdata = require('../cdata.js');
    var commonJs = require('../common.js');
    var g_jsonData = null;
    var g_gameId = 0;
    var g_id = 1;
    var fishRoom = [1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 15, 20];

    init(1,0);

    $('#optionGame li').click(function () {
        g_gameId = $(this).attr('gameId');
        $(this).addClass('Active').siblings().removeClass('Active');
        init(1,g_gameId);
    });

    $('#onQuery').click(function () {
        $.ajax({
            type: "POST",
            url: "/ashx/OnlinePerHour.ashx",
            data: { op: 0, time: $('#m_time').val(), 'param': g_gameId },
            success: function (data) {
                $('#m_result').html(data);
            }
        })
    });

    function init(op,gameId)
    {
        $("#divContent").empty();
        $.ajax({
            type: "POST",
            url: "/ashx/OnlinePerHour.ashx",
            data: { op: 1, "gameId": gameId, time: $('#m_time').val() },
            success: function (data) {
                var resList = $.parseJSON(data);
                $.each(resList, function (index, item) {
                    //var img = '<img style="display:block;margin:0 auto;" src=" + item.m_fileName" + "/>';
                    var img = $('<img>');
                    img.attr("src", item.m_fileName);
                    img.attr("style", "display:block;margin:0 auto;");
                    $("#divContent").append(img);
                });
            }
        });
    }

    function processData() {
        if (g_jsonData == null)
            return;

        var divContent = $('#divContent');
        divContent.empty();

        var data = g_jsonData[g_gameId];
        if (data == null)
            return;

        var chart = $('#divTemplate').html().format('chart' + g_id);
        divContent.append(chart);
        drawCurve(data, 'chart' + g_id, getChartTitle(g_gameId, 0));
        g_id++;

        if (g_gameId == 1) {

            for(var i = 0; i < fishRoom.length; i++)
            {
                data = g_jsonData[g_gameId * 1000 + fishRoom[i]];
                if (data == null)
                    continue;

                chart = $('#divTemplate').html().format('chart' + g_id);
                divContent.append(chart);
                drawCurve(data, 'chart' + g_id, getChartTitle(g_gameId, fishRoom[i]));
                g_id++;
            }
        }
    }

    function drawCurve(data, charId, chartTitle)
    {
        var dataArr = [];
        var field = 0;
        for (var i = 0; i < data.length; i++)
        {
            var t1 = cdata.dateName[i];
            
            var t2 = data[i]["onlineList"].split(',');
            console.log(t1);
            console.log(t2);

            var t3 = [];
            for (var k = 0; k < t2.length; k++)
            {
                t3[k] = parseFloat(t2[k]);
            }
            dataArr.push({ name: t1, "data": t3 });
        }
        //console.log(arr);

        $('#' + charId).highcharts({
            title: {
                text: chartTitle,
                x: -20 //center
            },
            subtitle: {
                text: '',
                x: -20
            },
            xAxis: {
                categories: cdata.timePointName
            },
            yAxis: {
                title: {
                    text: '在线人数'
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            tooltip: {
                valueSuffix: '人'
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
            credits: {
                enabled: false
            },
            series: dataArr
        });
    }

    function getChartTitle(gameId, roomId)
    {
        var title = '';
        if(gameId == 1)
        {
            title = cdata.games[g_gameId] + cdata.gameRoom[roomId] + '-实时在线';
        }
        else
        {
            title = gameId > 0 ? cdata.games[g_gameId] + '-' : '';
            title += '实时在线';
        }
        return title;
    }
});
});