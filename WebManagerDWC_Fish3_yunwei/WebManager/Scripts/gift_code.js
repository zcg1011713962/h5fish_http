/*
        ��������
*/

// �ύ���������
function submitGiftCodeParam()
{
    // ��������������Ϣ
    var code = document.getElementById("MainContent_operation_common_m_codeInfo");
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
        var txt = document.getElementById("CodeNum" + i);
        var retCode = isValidCount(txt.value);
        if (retCode == 2)
        {
            addErrorInfo("param invalid");
            return;
        }
        
        if (retCode == 0)
        {
            var plat = document.getElementById("MainContent_operation_common_Plat" + i);
            // ���ID��ƽ̨,����
            code.value = code.value + intArr[i + 1] + "," + plat.selectedIndex + "," + txt.value + ";";
        }
    }

    if (code.value == "") // ���ύ
        return;

    var f = document.getElementById("Form1");
    f.submit();
}

// ����0�ɹ�, 1�����,2�Ƿ�
function isValidCount(val)
{
    if (isNaN(val))
        return 2;

    var res = parseInt(val);
    if (res <= 0)
        return 1;

    return 0;
}

function addErrorInfo(errInfo)
{
    var sp = document.getElementById("MainContent_operation_common_m_res");
    sp.innerText = errInfo;
}
