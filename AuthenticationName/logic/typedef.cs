using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public struct ConstDef
{
    public const int AUTH_CHECK = 1;  // 实名认证

    public const int AUTH_QUEYR = 2;  // 实名查询

    public const int AUTH_UPDATA = 3;  // 数据上报

    public const int FMT_CODE = 8;
}

public class HttpReturn
{
    public string m_retStr;

    public void reset()
    {
        m_retStr = "ok";
    }
}

public class AuthInfoReq
{
    // 操作码 AUTH_QUEYR
    public int m_opCode;
}

// 认证信息
public class AuthInfoVerify : AuthInfoReq
{
    // id唯一标识
    public string m_id;

    // 用户实名
    public string m_name;

    // 身份证号
    public string m_idNum;
}

public class AuthInfoUpdata : AuthInfoReq
{
    // 会话标识
    public string m_session;

    // 0-下线 1-上线
    public int m_action;

    // 用户类型  0：已认证通过用户   2：游客用户
    public int m_userType;

    // 设备号
    public string m_deviceId;

    // 已通过实名认证用户的唯一标
    // 识，已认证通过用户必填
    public string m_pi;
}

public class AuthInfoQuery : AuthInfoReq
{
    public string m_playerId;  // 玩家ID
}

public class ResultData
{
    public int status;
    public string pi;
}

/////////////////////////////////////////////////
public class AuthInfoRep
{
    public int m_opCode;

    public object m_param;
}

// 查询结果响应
public class AuthInfoQueryRep
{
    public string m_playerId;
    ///////////////////////////////////

    public int errcode = -1;
    public string errmsg = "";
    public int m_status = -1;
    public string m_pi = "";

    public void fromJsonStr(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            LogMgr.error("AuthInfoQueryRep.fromJsonStr str empty");
            return;
        }

        Dictionary<string, object> dataDic = JsonHelper.ParseFromStr<Dictionary<string, object>>(str);
        errcode = Convert.ToInt32(dataDic["errcode"]);
        errmsg = Convert.ToString(dataDic["errmsg"]);
        if (errcode != 0)
        {
            this.m_status = 2;
            LogMgr.error("AuthInfoQueryRep.fromJsonStr, ecode {0}, emsg {1}", errcode, errmsg);
        }

        if (dataDic.ContainsKey("data"))
        {
            JObject data = (JObject)dataDic["data"];
            JToken resultTmp = null;
            if (data.TryGetValue("result", out resultTmp))
            {
                JObject result = (JObject)resultTmp;
                try
                {
                    this.m_status = result.GetValue("status").ToObject<int>();
                    this.m_pi = result.GetValue("pi").ToObject<string>();
                }
                catch (Exception ex)
                {
                    LogMgr.error(ex.ToString());
                }
            }
        }
    }
}

// 数据上报结果
public class AuthInfoUpdataRep
{
    public int errcode = -1;
    public string errmsg = "";

    public void fromJsonStr(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            LogMgr.error("AuthInfoUpdataRep.fromJsonStr str empty");
            return;
        }

        Dictionary<string, object> data = JsonHelper.ParseFromStr<Dictionary<string, object>>(str);
        if(data.ContainsKey("errcode"))
        {
            errcode = Convert.ToInt32(data["errcode"]);
        }
        if (data.ContainsKey("errmsg"))
        {
            errmsg = Convert.ToString(data["errmsg"]);
        }
        if (errcode != 0)
        {
            LogMgr.error("AuthInfoUpdataRep.fromJsonStr, ecode {0}, emsg {1}", errcode, errmsg);
        }

        if (data.ContainsKey("data"))
        {
            JObject jo = (JObject)data["data"];
            JToken jt = null;
            if(jo.TryGetValue("results", out jt))
            {
                try 
                {
                    JArray arr = (JArray)jt;
                    if (arr.Count > 0)
                    {
                        JObject tmp = (JObject)arr[0];
                        int no = tmp.GetValue("no").ToObject<int>();
                        int code = tmp.GetValue("errorcode").ToObject<int>();
                        string msg = tmp.GetValue("errmsg").ToObject<string>();
                    }
                }
                catch(Exception ex)
                {
                    LogMgr.error(ex.ToString());
                }
            }
        }
    }
}

// 从认证服务器返回的信息
public class AuthInfoCheckRep
{
    public string m_playerId;
    ///////////////////////////////////
    public int errcode = -1;

    public string errmsg = "";

    public int status = -1;

    public string pi = "";

    public void fromJsonStr(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            LogMgr.error("AuthInfoCheckRep.fromJsonStr str empty");
            return;
        }

        Dictionary<string, object> dataDic = JsonHelper.ParseFromStr<Dictionary<string, object>>(str);
        if(dataDic.ContainsKey("errcode"))
        {
            errcode = Convert.ToInt32(dataDic["errcode"]);
        }
        if(dataDic.ContainsKey("errmsg"))
        {
            errmsg = Convert.ToString(dataDic["errmsg"]);
        }
        if (errcode != 0)
        {
            this.status = 2;
            LogMgr.error("AuthInfoCheckRep.fromJsonStr, ecode {0}, emsg {1}", errcode, errmsg);
        }
        if (dataDic.ContainsKey("data"))
        {
            JObject data = (JObject)dataDic["data"];
            JToken resultTmp = null;
            if (data.TryGetValue("result", out resultTmp))
            {
                JObject result = (JObject)resultTmp;
                try
                {
                    this.status = result.GetValue("status").ToObject<int>();
                    this.pi = result.GetValue("pi").ToObject<string>();
                }
                catch(Exception ex)
                {
                    LogMgr.error(ex.ToString());
                }
            }
        }
    }
}




























