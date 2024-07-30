using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public struct RetResult
{
    public const string KEY_RESULT = "result";
    public const int RET_SUCCESS = 0;      // 成功
    public const int RET_FAIL = 1;     //失败
    public const int RET_HAS_EXIST = 2;//已经存在
    public const int RET_PARAM_ERROR = 3;  // 参数错误
}

