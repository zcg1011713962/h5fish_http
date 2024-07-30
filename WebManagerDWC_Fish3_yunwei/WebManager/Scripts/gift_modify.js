/*
        ����޸����
*/

var DATE_TIME = new RegExp("^\\s*(\\d{4})/(\\d{1,2})/(\\d{1,2})\\s*$");

// �Կո��������������
var TWO_NUM_BY_SPACE = new RegExp("^\\s*(\\d+)\\s+(\\d+)\\s*$");

// �Կո��������������
var TWO_NUM_BY_SPACE_SEQ = new RegExp("^(\\s*(\\d+)\\s+(\\d+)\\s*)(;\\s*(\\d+)\\s+(\\d+)\\s*)+$");

// �ύ����޸Ĳ���
function submitGiftModifyParam()
{
    // ��������������Ϣ
    var code = document.getElementById("MainContent_operation_common_m_modifyInfo");
    if (code == null)
    {
        addErrorInfo("unknown error");
        return;
    }
    
    var clientInfo = document.getElementById("MainContent_operation_common_m_clientInfo").value;
    // ���ȣ�ID�б�
    var arr = clientInfo.split(",");
    if(arr.length == 0)
        return;

    var intArr = new Array();
    for (i = 0; i < arr.length; i++)
    {
        intArr[i] = parseInt(arr[i], 10);
    }

    code.value = "";
    for (i = 0; i < intArr[0]; i++)
    {
        // �������
        var content = document.getElementById("MainContent_operation_common_Content" + i);
        if (!isContentValid(content.value))
        {
            addErrorInfo("param invalid");
            return;
        }

        // ��ֹ����
        var deadTime = document.getElementById("DeadTime" + i);
        if (!isDateTimeValid(deadTime.value))
        {
            addErrorInfo("param invalid");
            return;
        }

        // ���id, ���ݣ���ֹ����
        code.value = code.value + intArr[i + 1] + "@" + content.value + "@" + deadTime.value + "#";
    }

    if (code.value == "") // ���ύ
        return;

    var f = document.getElementById("Form1");
    f.submit();
}

// ����true�Ϸ���false�Ƿ�
function isContentValid(val)
{
    if (TWO_NUM_BY_SPACE.test(val))
        return true;

    if (TWO_NUM_BY_SPACE_SEQ.test(val))
        return true;

    return false;
}

// ����true�Ϸ���false�Ƿ�
function isDateTimeValid(val)
{
    return DATE_TIME.test(val);
}

function addErrorInfo(errInfo)
{
    var sp = document.getElementById("MainContent_operation_common_m_res");
    sp.innerText = errInfo;
}
