define(function (require, exports, module) {
    $(function (){
        var oTime = $('#time');
        var oRankSel = $('#rankSel');
        var resultDiv = $('#resultDiv');

        $('#btnQuery').click(function () {
            reqData(oTime.val(), oRankSel.val(), function (data) {
                resultDiv.empty();
                var jd = JSON.parse(data);
                for(var d in jd)
                {
                    $('<h2/>').appendTo(resultDiv).html(d).css({
                        "text-align": "center", "background": "#ccc",
                        "padding": "6px", "font-size": "30px"
                    });
                    $(jd[d]).attr('class', 'table table-hover table-bordered').appendTo(resultDiv);
                }
            });
        });

        function reqData(time, rankId, callBack)
        {
            $.ajax({
                type: "POST",
                url: "/ashx/OperationPlayerRichesRank.ashx",
                data: { 'time': time,'rankId': rankId},

                success: function (data) {
                    callBack(data);
                }
            });
        }
    });
});
