$(function ()
{
    var oTarget = $('#MainContent_m_target');
    
    var sel = oTarget.get(0).selectedIndex;
    showFullSendCond(sel);

    oTarget.change(function ()
    {
        var v = $(this).get(0).selectedIndex;
        showFullSendCond(v);
    });
});

function showFullSendCond(show)
{
    if(show == 1)
    {
        $("#divFullCond").show();
    }
    else
    {
        $("#divFullCond").hide();
    }
}
