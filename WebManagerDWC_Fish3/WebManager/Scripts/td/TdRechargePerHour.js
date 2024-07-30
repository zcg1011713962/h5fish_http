define(function (require, exports, module) {
    $(function () {
        var cdata = require('../cdata.js');
        var queryTime = $("#MainContent_m_time").val();
        
        init();

        $("#MainContent_Button3").click(function () {
            init();
        });

        function init()
        {
            $.ajax({
                type: "POST",
                url: "/ashx/RechargePerHour.ashx",
                data: {"time": queryTime},
                success: function (data) {
                    drawCurve(data);
                }
            });
        }
        function drawCurve(data)
        {
            var jd = JSON.parse(data);
            var dataArr = [];
            var field = 0;
            var nowDate = new Date();//今天
            var date_1 = getFormatDate(nowDate);

            var date2 = new Date(nowDate.getTime() - 24 * 3600 * 1000);//昨天
            var date_2 = getFormatDate(date2);

            for (var d in jd)
            {
                if (d != date_1 && d != date_2 && d != 'total')
                {
                    continue;
                }
                    

                if (d == date_1)
                    t1 = "今天";

                if (d == date_2)
                    t1 = "昨天";

                if (d == 'total')
                    t1 = "查询平均";
                
                var t2 = jd[d].split(',');
                var t3 = [];
                for (var k = 0; k < t2.length; k++)
                {
                    t3[k] = parseInt(t2[k]);
                }
                dataArr.push({ name: t1, "data": t3 });
            }

            var credits = Highcharts.getOptions().credits;
            credits.enabled = false;

            $('#container').highcharts({
                title: {
                    text: '实时付费',
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
                        text: '付费金额'
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                tooltip: {
                    valueSuffix: '元'
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                series: dataArr
            });
        }

        function getFormatDate(setDate) {
            var year = setDate.getFullYear();
            var month = setDate.getMonth() + 1;
            var date = setDate.getDate();
            return year + "/" + month + "/" + date;
        }
    });
});