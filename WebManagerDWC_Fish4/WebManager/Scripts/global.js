$(function () {

    var topNavMenu = $('#topNavMenu');
    var oBody = $('body');

    $(window).resize(function () {

        onResize();
    });

    function onResize()
    {
        var h = topNavMenu.outerHeight();
        oBody.css('padding-top', h);
    }

    onResize();
});