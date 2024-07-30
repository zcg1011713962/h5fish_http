$(document).ready(function ()
{
    var sel = $("#MainContent_m_optional").get(0).selectedIndex;
    showTime(sel);

    $("#MainContent_m_optional").change(function ()
    {
        var v = $(this).get(0).selectedIndex;
        showTime(v);
    });
});

function showTime(show)
{
    if(show == 3) // 京东卡持有
    {
        $("#groupTime").hide();
        $("#groupId").show();
    }
    else if (show == 2)
    {
        $("#groupTime").hide();
        $("#groupId").hide();
    }
    else
    {
        $("#groupTime").show();
        $("#groupId").hide();
    }
}
