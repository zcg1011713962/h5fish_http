﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

public enum RetCode
{
    ret_success,
    ret_busy,
    ret_error,
}

public enum ServiceType
{
    serviceTypeExportExcel,
    serviceTypeSendMail,
    serviceTypeGiftCode,
    serviceTypeUpdatePlayerTask,
}

[Serializable]
public class ServiceParam
{
    // 服务器IP
    public string m_toServerIP;
}

[Serializable]
public class ExportParam : ServiceParam
{
    // 账号
    public string m_account = "";

    // 服务器IP
    public string m_dbServerIP;

    // 表名
    public string m_tableName = "";

    // 导出条件
    public Dictionary<string, object> m_condition;
}

// 全局福利
[Serializable]
public class ParamSendMailFullSvr : ServiceParam
{
    // 往哪个服务器发
    public string m_dbServerIP = "";
    public string m_title = "";
    public string m_sender = "";
    public string m_content = "";
    public string m_itemList = "";
    public int m_validDay;

    // 发放条件
    public Dictionary<string, object> m_condition;
}

[Serializable]
public class GiftCodeInfo
{
    // 礼包ID
    public long m_giftId;
    // 平台
    public string m_plat = "";
    // 生成个数
    public int m_count;
}

// 生成礼包码
[Serializable]
public class ParamGenGiftCode : ServiceParam
{
    // 写到哪个db服务器
    public string m_dbServerIP = "";
    public int m_count;
    public int m_pici;
}

// 针对全体玩家的操作
[Serializable]
public class ParamFullPlayerOp : ServiceParam
{
    public const int ADD_NEW_TASK = 0;
    public const int RESET_GIFT_GUIDE_FLAG = 1;

    // 操作代码
    public int m_opCode;
}

public delegate int CallBackService(ServiceParam param, ServiceType st);

[Serializable]
public class ServersEngine : System.MarshalByRefObject
{
    public static CallBackService s_callService = null;

    // 测试远程服务的运行状态
    public int testRemoteServer()
    {
        return 1;
    }

    public int reqService(ServiceParam param, ServiceType st)
    {
        if (s_callService == null)
            return (int)RetCode.ret_error;

        return s_callService(param, st);
    }
}

