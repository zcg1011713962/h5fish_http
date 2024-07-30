define(function (require, exports, module) {
$(function () {

    var GROWTH = 0;
    var NET_GROWTH = 1;
    var oTime = $('#time');
    var oRankSel = $('#rankSel');
    var oRankWay = $('#rankWay');
    var resultDiv = $('#resultDiv');
    var channel = $('#MainContent_m_channel');
    $('#btnQuery').click(function () {
        reqData(oTime.val(), oRankWay.val(), oRankSel.val(), channel.val(), function (data) {
            
            resultDiv.empty();

            var jd = JSON.parse(data);

            var way = parseInt(oRankWay.val());
            if (way >= 2)
            {
                //console.log(jd.qresult);

                var str_table = '<table class="table table-hover table-bordered">';

                str_table += '<tr><td>日期</td><td>排行</td><td>玩家ID</td><td>数量</td></tr>';
                var jda = JSON.parse(jd.qresult);
                for (var d in jda)
                {
                    str_table += '<tr>';

                    str_table += '<td>';
                    str_table += jda[d].m_time;
                    str_table += '</td>';

                    str_table += '<td>';
                    str_table += jda[d].m_rankId;
                    str_table += '</td>';

                    str_table += '<td>';
                    str_table += jda[d].m_playerId;
                    str_table += '</td>';

                    str_table += '<td>';
                    str_table += jda[d].m_value;
                    str_table += '</td>';

                    str_table += '</tr>';
                }

                str_table += '</table>';

                resultDiv.html(str_table);

            } else
            {
                for (var d in jd)
                {
                    $('<h2/>').appendTo(resultDiv).html(d).css({
                        "text-align": "center", "background": "#ccc",
                        "padding": "6px", "font-size": "30px"
                    });
                    $(jd[d]).attr('class', 'table table-hover table-bordered').appendTo(resultDiv);
                }
            }
        });

    });

    function reqData(time, rankWay, rankId, channel,callBack)
    {
        $.ajax({
            type: "POST",
            url: "/ashx/OperationRank.ashx",
            data: { 'time': time, 'rankWay': rankWay, 'rankId': rankId},

            success: function (data) {
                callBack(data);
            }
        });
    }
});
});
