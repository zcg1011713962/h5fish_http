define(function (require, exports, module) {
    $(function () {
        var oResultTable = $('#MainContent_m_result');
        var oVip = $('#MainContent_m_vip');
        var oDays = $('#MainContent_m_days');
        var oTime = $('#MainContent_m_time');
        var oBindPhone = $('#MainContent_m_isBindPhone');
      
        $('#exportExcel').click(function () {
            exportExcel();
        });

        function exportExcel()
        {
            var str = "\uFEFF";
            var content = getContent();
            var fileName = getFileName();

            var blob = new Blob([str + content],{type: "text/plain;charset=utf-8"});
            saveAs(blob, fileName);
        }

        function getContent()
        {
            var result = '';
            var first = true;
            oResultTable.find('tr').each(function (i, elem)
            {
                first = true;
                $(elem).find('td').each(function (i, tdElem)
                {
                    var colspan = $(tdElem).attr('colspan');
                    if(first)
                    {
                        result += $(this).text();
                        first = false;
                    }else
                    {
                        result += ',' + $(this).text();
                    }

                    if (colspan == 2) {
                        result += ',' + $(this).text();
                    }
                });

                result += '\n';
            });

            return result;
        }

        function getFileName()
        {
            var vip = "vip等级大于" + oVip.val();
            var days = "流失大于" + oDays.val();
            var time = "时间段" + oTime.val().replace(/-/g, '~').replace(/\//g, '-') + "内有充值行为";
            var bindPhone = oBindPhone.get(0).selectedIndex == 0 ? "绑定手机":"未绑定手机";
            return vip + '-' + days + '-' + time + '-' + bindPhone + "用户信息.csv";
        }
    });
});