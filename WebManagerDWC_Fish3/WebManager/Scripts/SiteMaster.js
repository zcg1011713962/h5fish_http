define(function (require, exports, module) {
    $(function () {
        var op = 1;
        //双击时修改菜单栏
        $("#btn_changeMenu").dblclick(function () {
            $.ajax({
                type: "POST",
                url: "/ashx/SiteMaster.ashx",
                data: {"op":op},
                success: function (data){
                    console.log(data);
                }
            });
        });
    });
});