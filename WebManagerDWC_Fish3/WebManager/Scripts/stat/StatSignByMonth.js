define(function (require, exports, module) {

    $(function () {
    var commonJs = require('../common.js');
    var g_jsonData = null;
    var g_id = 1;
	var g_signData = [];
	var g_monthData = ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'];
	var g_serial = [{ key: 'query', name: '查询月' }, { key: 'cur', name: '当月' }, { key: 'last', name: '上月' }];
	var g_year = $("#m_year");
	var g_month = $("#m_month");
	var YEAR = /^20[0-9]{2}$/;

	function init() {
	    for (var i = 1; i <= 31; i++) {
	        g_signData[i - 1] = '签' + i + '次';
	    }

        //默认写出当前年月
	    var myDate = new Date();
	    var myYear=myDate.getFullYear();
	    var myMonth = myDate.getMonth() + 1;

	    g_year.val(myYear);
	    $("#m_month option[value="+myMonth+"]").attr("selected",true);
	}
	init();

	function reqMonthSign( thisYear, thisMonth )
	{ 
		 $.ajax({
			type: "POST",
			url: "/ashx/SignByMonth.ashx",
			data: { op: 1, year:thisYear, month:thisMonth },
			success: function (data) {
			    console.log(data);
			    g_jsonData = JSON.parse(data);
				processData(thisMonth);
			}
		});
	}
	
	$('#onQueryMonth').click(function () {
	    var year = g_year.val();
        if (!YEAR.test(parseInt(year)))
        {
            alert("请输入正确的年份");
            return false;
        }
        var month = g_month.val();
		reqMonthSign( parseInt(year), parseInt(month) );
    });

    function processData( month ) 
	{
        if ( g_jsonData == null )
            return;

        var divContent = $( '#divContent' );
        divContent.empty();

        var chart = $( '#divTemplate' ).html().format( 'chart' + g_id );
        divContent.append( chart );
        drawCurve('chart' + g_id, g_monthData[month - 1]);
        g_id++;
    }

    function drawCurve( charId, chartTitle )
    {
        var dataArr = [];

		for (var s = 0; s < g_serial.length; s++)
		{
		    var jd = g_jsonData[g_serial[s].key];
		    var t2 = [];
		    for (var i = 0; i < g_signData.length; i++)
		    {
		        if (jd[i + 1] != undefined)
		        {
		            t2[i] = jd[i + 1];
		        }
		    }
		    dataArr.push({ name: g_serial[s].name, "data": t2 });
		}
	
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
                categories: g_signData
            },
            yAxis: {
                title: {
                    text: '签到人数'
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
	
});
});