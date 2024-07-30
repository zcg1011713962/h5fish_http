$(function () {

    var oTableResult = $('#MainContent_m_result');

    $.jqPaginator('#pagination0', {
        totalPages: 100,
        visiblePages: 10,
        currentPage: 1,
        first: '<li class="first"><a href="javascript:;">首页</a></li>',
        prev: '<li class="prev"><a href="javascript:;">上一页</a></li>',
        next: '<li class="next"><a href="javascript:;">下一页</a></li>',
        page: '<li class="page"><a href="javascript:;">{{page}}</a></li>',
        last: '<li class="last"><a href="javascript:;">尾页</a></li>',
        
        onPageChange: function (page, type) {
           
            reqOp(0, {'page':page}, function (data) {

                oTableResult.html(data);

            });
        }
    });

    function reqOp(op, jsonParam, callBack) {

        var jd = {};
        $.extend(jd, jsonParam);
        jd.op = op;

        $.ajax({
            type: "POST",
            url: "/ashx/ServiceBlockId.ashx",
            data: jd,

            success: function (data) {
                callBack(data);
            }
        });
    }

});