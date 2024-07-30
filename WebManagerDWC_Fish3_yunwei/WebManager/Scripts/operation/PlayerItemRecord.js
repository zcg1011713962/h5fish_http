define(function (require, exports, module) {
    $(function () {
        var oResultTable = $('#MainContent_m_result');
        var oTime = $('#MainContent_m_time');
        var oPlayerId = $('#MainContent_m_playerId');
        var oItem = $('#MainContent_m_itemId');
        var oSyncKey = $('#MainContent_m_syncKey');

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
            oResultTable.find('tr').each(function (i, elem) {

                first = true;

                $(elem).find('td').each(function (i, tdElem) {

                    var colspan = $(tdElem).attr('colspan');

                    if(first)
                    {
                        result += $(this).text();
                        first = false;
                    }
                    else
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
            var time = oTime.val().replace(/-/g, '~').replace(/\//g, '-');
            var playerId = '玩家' + oPlayerId.val();
            var item = '道具' + oItem.val();
            var syncKey = oSyncKey.val();
            return time + '-' + playerId + '-' + item + '-' + syncKey + '.csv';
        }
    });
});