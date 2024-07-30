using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public struct RetResult
{
    public const string KEY_RESULT = "result";

    public const int RET_SUCCESS = 0;      // 成功

    public const int RET_UNKONWN = 1;      // 未知

    public const int RET_PARAM_ERROR = 2;  // 参数错误

    public const int RET_NOT_FIND = 3;  // 没找到数据

    public const int RET_HAS_RECEIVE = 4;  // 已领取

    public const int RET_NOT_SATISFY_COND = 5;  // 不满足条件

    public const int RET_ACT_NOT_START = 6;  // 活动未开启
}

public struct CC
{
    // 救济金领取次数
    public const int BENEFIT_RECV_COUNT = 4;
}


