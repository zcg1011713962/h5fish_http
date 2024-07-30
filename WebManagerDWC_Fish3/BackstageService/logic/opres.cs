using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Web;

class OpResMgr
{
    // ȡ�ý����
    public static string getResultString(OpRes res)
    {
        XmlConfig xml = ResMgr.getInstance().getRes("opres.xml");
        return xml.getString(Convert.ToString((int)res), "");
    }
}

public enum OpRes
{
    opres_success,              // �ɹ�
    op_res_failed,              // ʧ��
    op_res_time_format_error,   // ʱ���ʽ��
    op_res_not_found_data,      // û���ҵ��������
    op_res_not_select_any_item, // û��ѡ���κ���Ŀ
    op_res_param_not_valid,     // �����Ƿ�
    op_res_item_not_exist,      // �����ڸõ���
    op_res_pwd_not_valid,       // �����ʽ����ȷ
    op_res_connect_failed,      // ����db������ʧ��
}

