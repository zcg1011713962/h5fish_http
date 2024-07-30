using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Common;

public struct CC
{
    public const int RET_SUCCESS = 200;
    public const int RET_PLAYER_NOT_EXIST = 300;
    public const int RET_PARAM_ERROR = 1;  // 输入参数错误

    public const string SECRET = "Jiejibuyu&Duoyou666";

    public static string[] PLAYER_FIELD = { "nickname", "gold", "PlayerLevel", "VipLevel", "logout_time", "recharged" };


    public const string XIANWAN_SECRET = "Jiejibuyu&Duoyou678";

    public const string DANDANZHUAN_SECRET = "Jiejibuyu&DanDanZhuan409";

    public const string HULU_SECRET = "Jiejibuyu&Hulu908";

    public const string YOUZHUAN_SECRET = "Jiejibuyu&YouZhuan415";

    public const string MAIZIZHUAN_SECRET = "Jiejibuyu&MaiziZhuan403";

    public const string JUXIANGWAN_SECRET = "Jiejibuyu&JuXiangWan518";

    public const string XIAOZHUO_SECRET = "Jiejibuyu&XiaoZhuo124";

    public const string PAOPAOZHUAN_SECRET = "Jiejibuyu&PaoPaoZhuan209";

    public const string DDQW_SECRET = "Jiejibuyu&DDQW319";
}

public class Tool
{
    public static long getTimeStamp(DateTime t)
    {
        //DateTime start = new DateTime(1970, 1, 1);
        TimeSpan span = t - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        return (long)span.TotalSeconds;
    }
}

public struct Exp
{
    public const string DATE_TIME = @"^\s*(\d{4})(\d{2})(\d{2})\s*$";
}

public class InputData
{
    // 玩家ID
    public int m_playerId;
    public string m_os;
    // 查询开始时间
    public DateTime m_startTime; 
    // 查询结束时间
    public DateTime m_endTime;

    public string m_startTimeStr;
    public string m_endTimeStr;

    // 签名
    public string m_sign;
}

public class OutData
{
    public int m_playerId;
    public string m_nickName = "";
    public int m_fishLevel;
    public int m_vipLevel;
    public long m_gold;
    public int m_recharged;
    public DateTime m_lastOnlineTime;

    public long m_room01WinGold;
    public long m_room02WinGold;
    public long m_room03WinGold;
    public long m_roleGetDimensity;
    public int m_timePayment;
    public int m_ticket; // 所得话费
}

// 查询数据
public class ApiQueryData
{
    InputData m_inputData = new InputData();
    OutData m_outData = new OutData();

    public string doQuery(HttpRequest req)
    {
        string retstr = "";
        do
        {
            try
            {
                int code = checkInput(req);
                if (code != CC.RET_SUCCESS)
                {
                    retstr = outputData(null, code);
                    break;
                }

                code = queryBaseData(m_inputData);
                if (code != CC.RET_SUCCESS)
                {
                    retstr = outputData(null, code);
                    break;
                }

                code = queryGameData(m_inputData);
                if (code != CC.RET_SUCCESS)
                {
                  //  retstr = outputData(null, code);
                   // break;
                }

                retstr = outputData(m_outData, code);
            }
            catch (System.Exception ex)
            {
            }
        } while (false);

        return retstr;
    }

    string outputData(OutData data, int code)
    {
        Dictionary<string, object> rep = new Dictionary<string, object>();
        do 
        {
            rep.Add("state_code", code);

            if (code != CC.RET_SUCCESS)
            {
                break;
            }

            Dictionary<string, object> roleInfo = new Dictionary<string, object>();
            rep.Add("role_info", roleInfo);
            roleInfo.Add("role_id", data.m_playerId);
            roleInfo.Add("server_id", 1);
            roleInfo.Add("role_name", data.m_nickName);
            roleInfo.Add("role_level", data.m_fishLevel);
            roleInfo.Add("role_vip", data.m_vipLevel);
            //roleInfo.Add("role_payamount", 0);
            roleInfo.Add("role_gold", data.m_gold);
            roleInfo.Add("role_payamount", data.m_recharged);
            roleInfo.Add("update_time", data.m_lastOnlineTime);

            Dictionary<string, object> roleData = new Dictionary<string, object>();
            rep.Add("role_data", roleData);
            roleData.Add("room_01_wingold", data.m_room01WinGold);
            roleData.Add("room_02_wingold", data.m_room02WinGold);
            roleData.Add("room_03_wingold", data.m_room03WinGold);
            roleData.Add("role_get_dimensity", data.m_roleGetDimensity);
          //  roleData.Add("role_vip", data.m_vipLevel);
            roleData.Add("time_payamount", data.m_timePayment);
            roleData.Add("role_get_Telephone", data.m_ticket);
            roleData.Add("start_time", m_inputData.m_startTimeStr);
            roleData.Add("end_time", m_inputData.m_endTimeStr);

        } while (false);

        return JsonHelper.genJson(rep);
    }

    // 检测输入数据
    int checkInput(HttpRequest req)
    {
        Dictionary<string, object> checkData = new Dictionary<string, object>();

        string id = req["role_id"];
        if (string.IsNullOrEmpty(id))
        {
            CLOG.Info("role_id param error");
            return CC.RET_PARAM_ERROR;
        }
        string os = req["os"];
        if (string.IsNullOrEmpty(os))
        {
            CLOG.Info("os param error");
            return CC.RET_PARAM_ERROR;
        }
        string signIn = req["sign"];
        if (string.IsNullOrEmpty(signIn))
        {
            CLOG.Info("sign param error");
            return CC.RET_PARAM_ERROR;
        }
        string timeStr = req["start_time"];
        if (!paraseTime(timeStr, false, ref m_inputData.m_startTime))
        {
            CLOG.Info("start_time param error, {0}", timeStr);
            return CC.RET_PARAM_ERROR;
        }
        checkData.Add("start_time", timeStr);
        m_inputData.m_startTimeStr = timeStr;

        timeStr = req["end_time"];
        if (!paraseTime(timeStr, true, ref m_inputData.m_endTime))
        {
            CLOG.Info("end_time param error, {0}", timeStr);
            return CC.RET_PARAM_ERROR;
        }
        checkData.Add("end_time", timeStr);
        m_inputData.m_endTimeStr = timeStr;

        try
        {
            m_inputData.m_playerId = Convert.ToInt32(id);
            m_inputData.m_os = os;
            m_inputData.m_sign = signIn;
        }
        catch (System.Exception ex)
        {
            CLOG.Info("role_id param error111, {0}", id);
            return CC.RET_PARAM_ERROR;
        }

        checkData.Add("role_id", m_inputData.m_playerId);
        checkData.Add("os", m_inputData.m_os);
        checkData.Add("server_id", 1);
        checkData.Add("key", "Jiejibuyu&Duoyou666");

        PayCheck check = new PayCheck();
        string waitStr = check.getWaitSignStrByAsc(checkData);
        string sign = Common.Helper.getMD5(waitStr);
        if (sign != m_inputData.m_sign)
        {
            CLOG.Info("sing error, wait={0},  srcSign={1}, factSign={2}", waitStr, m_inputData.m_sign, sign);
            return CC.RET_PARAM_ERROR;
        }
        return CC.RET_SUCCESS;
    }

    public static bool paraseTime(string timeStr, bool isaddoneday, ref DateTime time)
    {
        Match match = Regex.Match(timeStr, Exp.DATE_TIME);
        if (!match.Success)
        {
            return false;
        }

        GroupCollection c = match.Groups;
        int y = Convert.ToInt32(c[1].Value);
        int m = Convert.ToInt32(c[2].Value);
        int d = Convert.ToInt32(c[3].Value);
        DateTime t = new DateTime(y, m, d);
        if (isaddoneday)
        {
            t = t.AddDays(1);
        }
        time = t;
        return true;
    }

    int queryBaseData(InputData data)
    {
        Dictionary<string, object> retData = MongodbPlayer.Instance.ExecuteGetOneBykey("player_info", "player_id", data.m_playerId, CC.PLAYER_FIELD);
        if (retData == null)
            return CC.RET_PLAYER_NOT_EXIST;

        m_outData.m_playerId = data.m_playerId;
        if (retData.ContainsKey("nickname"))
        {
            m_outData.m_nickName = Convert.ToString(retData["nickname"]);
        }
        if (retData.ContainsKey("gold"))
        {
            m_outData.m_gold = Convert.ToInt64(retData["gold"]);
        }
        if (retData.ContainsKey("PlayerLevel"))
        {
            m_outData.m_fishLevel = Convert.ToInt32(retData["PlayerLevel"]);
        }
        if (retData.ContainsKey("VipLevel"))
        {
            m_outData.m_vipLevel = Convert.ToInt32(retData["VipLevel"]);
        }
        if (retData.ContainsKey("logout_time"))
        {
            m_outData.m_lastOnlineTime = Convert.ToDateTime(retData["logout_time"]).ToLocalTime();
        }
        if (retData.ContainsKey("recharged"))
        {
            m_outData.m_recharged = Convert.ToInt32(retData["recharged"]);
        }
        return CC.RET_SUCCESS;
    }

    // 查询游戏数据
    int queryGameData(InputData data)
    {
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(data.m_endTime));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(data.m_startTime));
        IMongoQuery imq = Query.And(Query.EQ("playerId", data.m_playerId), imq1, imq2);
        List<Dictionary<string, object>> retData = null;
        if (data.m_os == "1") // os
        {
            retData = MongodbLog.Instance.ExecuteGetListByQuery("pumpChannelPlayer100006", imq);
        }
        else
        {
            retData = MongodbLog.Instance.ExecuteGetListByQuery("pumpChannelPlayer000971", imq);
        }

        if (retData == null)
            return CC.RET_SUCCESS;

        if (retData.Count == 0)
            return CC.RET_SUCCESS;

        for (int i = 0; i < retData.Count; ++i)
        {
            Dictionary<string, object> d = retData[i];
            if (d.ContainsKey("room_01_wingold"))
            {
                m_outData.m_room01WinGold += Convert.ToInt64(d["room_01_wingold"]);
            }
            if (d.ContainsKey("room_02_wingold"))
            {
                m_outData.m_room02WinGold += Convert.ToInt64(d["room_02_wingold"]);
            }
            if (d.ContainsKey("room_03_wingold"))
            {
                m_outData.m_room03WinGold += Convert.ToInt64(d["room_03_wingold"]);
            }
            if (d.ContainsKey("role_get_dimensity"))
            {
                m_outData.m_roleGetDimensity += Convert.ToInt64(d["role_get_dimensity"]);
            }
            if (d.ContainsKey("time_payamount"))
            {
                m_outData.m_timePayment += Convert.ToInt32(d["time_payamount"]);
            }
            if (d.ContainsKey("role_get_Telephone"))
            {
                m_outData.m_ticket += Convert.ToInt32(d["role_get_Telephone"]);
            }
        }

        return CC.RET_SUCCESS;
    }
}

//////////////////////////////////////////////////////////////////////////
public class InfoParam
{
    public string m_userid;  // 玩家ID
    public string m_channel;
    public string m_sign;
    public string m_startTimeStr;
    public string m_endTimeStr;

    public DateTime m_startTime;
    public DateTime m_endTime;
}

public class PlayerInfo
{
    public int m_playerId;
    public string m_nickName;
    public int m_playerLevel;
    public int m_recharged;
    public long m_regTime;
    public long m_winGold;
}

// 广告主查询玩家信息
public class QueryPlayerInfo
{
    public const int RET_SUCCESS = 1;
    public const int RET_PARAM_ERROR = 2;
    public const int RET_SIGN_ERROR = 3;
    public const int RET_NO_PLAYER = 4;

    static string[] S_ACC_FIELD = { "acc_real" };
    static string[] S_PLAYER_INFO_FIELD = { "nickname", "player_id", "PlayerLevel", "recharged", "create_time" };

    Dictionary<string, object> m_retData = new Dictionary<string, object>();

    PlayerInfo m_outData = new PlayerInfo();

    public string doQuery(object param)
    {
        HttpRequest req = (HttpRequest)param;

        do
        {
            try
            {
                InfoParam dataInfo = null;
                int code = getParam(req, ref dataInfo);
                if (code != RET_SUCCESS)
                {
                    m_retData.Add("success", code);
                    break;
                }
                if (!checkSign(dataInfo))
                {
                    m_retData.Add("success", RET_SIGN_ERROR);
                    break;
                }

                if (queryPlayerInfo(dataInfo) == null)
                {
                    m_retData.Add("success", RET_NO_PLAYER);
                    break;
                }
                if (queryGameData(dataInfo) != RET_SUCCESS)
                {
                    m_retData.Add("success", 0);
                    break;
                }

                m_retData.Add("success", RET_SUCCESS);
                Dictionary<string, object> d = new Dictionary<string, object>();
                d.Add("userid", Convert.ToInt32(dataInfo.m_userid));
                d.Add("nickname", m_outData.m_nickName);
                d.Add("win_gold", m_outData.m_winGold);
                d.Add("level", m_outData.m_playerLevel);
                d.Add("payamount", m_outData.m_recharged);
                d.Add("regTime", m_outData.m_regTime);
                m_retData.Add("data", d);
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }
        } while (false);

        return JsonHelper.genJson(m_retData);
    }

    int getParam(HttpRequest req, ref InfoParam data)
    {
        string userid = req.QueryString["userid"];
        if (string.IsNullOrEmpty(userid))
        {
            CLOG.Info("QueryPlayerInfo.getParam, userid param lost");
            return RET_PARAM_ERROR;
        }
       /* string channel = req.QueryString["channel"];
        if (string.IsNullOrEmpty(channel))
        {
            CLOG.Info("QueryPlayerInfo.getParam, channel param lost");
            return RET_PARAM_ERROR;
        }*/
        string keycode = req.QueryString["keycode"];
        if (string.IsNullOrEmpty(keycode))
        {
            CLOG.Info("QueryPlayerInfo.getParam, keycode param lost");
            return RET_PARAM_ERROR;
        }
        string timeStr1 = req["startTime"];
        DateTime startTime = new DateTime();
        if (!ApiQueryData.paraseTime(timeStr1, false, ref startTime))
        {
            CLOG.Info("QueryPlayerInfo.getParam error, {0}", timeStr1);
            return RET_PARAM_ERROR;
        }

        DateTime endTime = new DateTime();
        string timeStr2 = req["endTime"];
        if (!ApiQueryData.paraseTime(timeStr2, true, ref endTime))
        {
            CLOG.Info("QueryPlayerInfo.getParam param error, {0}", timeStr2);
            return RET_PARAM_ERROR;
        }

        data = new InfoParam();
       // data.m_channel = channel;
        data.m_sign = keycode.Trim();
        data.m_userid = userid.Trim();
        data.m_startTimeStr = timeStr1.Trim();
        data.m_endTimeStr = timeStr2.Trim();
        data.m_startTime = startTime;
        data.m_endTime = endTime;

        return RET_SUCCESS;
    }

    bool checkSign(InfoParam data)
    {
        string wait = data.m_userid + data.m_startTimeStr + data.m_endTimeStr + CC.XIANWAN_SECRET;
        string sign = Common.Helper.getMD5(wait);
        return sign == data.m_sign;
    }

    PlayerInfo queryPlayerInfo(InfoParam data)
    {
/*
        string tableName = PayTable.getAccountTableByPlatform("guanggao");
        Dictionary<string, object> dic = MongodbAccount.Instance.ExecuteGetBykey(tableName, "acc", data.m_userid, S_ACC_FIELD);
        if (dic == null) return null;

        if(!dic.ContainsKey("acc_real"))
            return null;

        string accReal=Convert.ToString(dic["acc_real"]);
*/

        Dictionary<string, object> player = MongodbPlayer.Instance.ExecuteGetBykey("player_info", "player_id", Convert.ToInt32(data.m_userid), S_PLAYER_INFO_FIELD);
        if (player == null)
            return null;

       /* if (player.ContainsKey("player_id"))
        {
            info.m_playerId = Convert.ToInt32(player["player_id"]);
        }*/
        if (player.ContainsKey("nickname"))
        {
            m_outData.m_nickName = Convert.ToString(player["nickname"]);
        }
        if (player.ContainsKey("PlayerLevel"))
        {
            m_outData.m_playerLevel = Convert.ToInt32(player["PlayerLevel"]);
        }
//         if (player.ContainsKey("recharged"))
//         {
//             m_outData.m_recharged = Convert.ToInt32(player["recharged"]);
//         }
        if (player.ContainsKey("create_time"))
        {
            m_outData.m_regTime = Tool.getTimeStamp(Convert.ToDateTime(player["create_time"]).ToLocalTime());
        }
        return m_outData;
    }

    // 查询游戏数据
    int queryGameData(InfoParam data)
    {
        int playerId = Convert.ToInt32(data.m_userid);
        TDataInfo winGold = null;
        bool res = HttpApi.Global.getTCacheData().getWinGold(playerId, ref winGold);
        if (res)
        {
            TPlayerInfo t = (TPlayerInfo)winGold.m_data;
            m_outData.m_winGold = t.m_winGold;
            m_outData.m_recharged = (int)t.m_recharge;
            return RET_SUCCESS;
        }

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(data.m_endTime));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(data.m_startTime));
        IMongoQuery imq = Query.And(Query.EQ("playerId", playerId), imq1, imq2);

        List<Dictionary<string, object>> retData = MongodbLog.Instance.ExecuteGetListByQuery("pumpChannelPlayer100003", imq);
        if (retData == null)
            return RET_SUCCESS;

        if (retData.Count == 0)
            return RET_SUCCESS;

        for (int i = 0; i < retData.Count; ++i)
        {
            Dictionary<string, object> d = retData[i];
            if (d.ContainsKey("win_gold"))
            {
                m_outData.m_winGold += Convert.ToInt64(d["win_gold"]);
            }
            if (d.ContainsKey("payamount"))
            {
                m_outData.m_recharged += Convert.ToInt32(d["payamount"]);
            }
        }

        TPlayerInfo ti = new TPlayerInfo();
        ti.m_recharge = m_outData.m_recharged;
        ti.m_winGold = m_outData.m_winGold;
        HttpApi.Global.getTCacheData().addWinGold(playerId, ti);
        return RET_SUCCESS;
    }
}

//////////////////////////////////////////////////////////////////////////
public class ReqInfoParam
{
    public string m_deviceId;
    public string m_sign;
}

public class ReqAccInfo
{
    public int m_playerId;
    public string m_account;
    public string m_nickName;
    public string m_deviceId;
    public string m_createTime;
}

// 闲玩,查询注册信息
public class CXWQueryReg
{
    static string[] S_FIELDS = { "acc_real" };
    static string[] PLAYER_FIELDS = { "player_id", "nickname", "create_time" };

    public string queryReg(object param)
    {
        Dictionary<string, object> ret = new Dictionary<string, object>();
        ReqAccInfo info = null;

        HttpRequest request = (HttpRequest)param;
        do
        {
            try
            {
                ReqInfoParam inParam = null;
                if (getParam(request, ref inParam) != QueryPlayerInfo.RET_SUCCESS)
                {
                    ret.Add("success", QueryPlayerInfo.RET_PARAM_ERROR);
                    break;
                }

                info = queryInfo(inParam);
                if (info == null)
                {
                    ret.Add("success", QueryPlayerInfo.RET_NO_PLAYER);
                    break;
                }

                return getRegAccInfo(ret, info);
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }
        } while (false);

        return JsonHelper.genJson(ret);
    }

    int getParam(HttpRequest req, ref ReqInfoParam data)
    {
        string deviceid = req.QueryString["deviceid"];
        if (string.IsNullOrEmpty(deviceid))
        {
            CLOG.Info("CXWQueryReg.getParam, deviceid param lost");
            return QueryPlayerInfo.RET_PARAM_ERROR;
        }
        string sign = req.QueryString["keycode"];
        if (string.IsNullOrEmpty(deviceid))
        {
            CLOG.Info("CXWQueryReg.getParam, keycode param lost");
            return QueryPlayerInfo.RET_PARAM_ERROR;
        }

        data = new ReqInfoParam();
        data.m_deviceId = deviceid.Trim();
        data.m_sign = sign.Trim();

        return QueryPlayerInfo.RET_SUCCESS;
    }

    string getRegAccInfo(Dictionary<string, object> ret, ReqAccInfo info)
    {
        if (info != null)
        {
            ret.Add("success", 1);
            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("userid", info.m_playerId);
           // d.Add("account", info.m_account);
            d.Add("nickname", info.m_nickName);
            d.Add("deviceid", info.m_deviceId);
            //d.Add("regTime", info.m_createTime);
            ret.Add("data", d);
        }
        else
        {
            ret.Add("success", 0);
        }

        return JsonHelper.genJson(ret);
    }

    ReqAccInfo queryInfo(ReqInfoParam param)
    {
        Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetBykey(PayTable.XIANWAN_ACC, "deviceId", param.m_deviceId, S_FIELDS);
        if (data == null)
            return null;

        if(!data.ContainsKey("acc_real"))
            return null;

        string accReal = Convert.ToString(data["acc_real"]);
        Dictionary<string, object> pd = MongodbPlayer.Instance.ExecuteGetBykey("player_info", "account", accReal, PLAYER_FIELDS);
        if (pd == null)
            return null;

        ReqAccInfo info = new ReqAccInfo();
        info.m_deviceId = param.m_deviceId;
        info.m_nickName = Convert.ToString(pd["nickname"]);
        info.m_playerId = Convert.ToInt32(pd["player_id"]);
       /* if (pd.ContainsKey("create_time"))
        {
            info.m_createTime = Convert.ToDateTime(pd["create_time"]).ToLocalTime().ToString(PayConstDef.DATE_TIME_FORMAT);
        }*/
        return info;
    }

    bool checkSign(ReqInfoParam inParam)
    {
        string wait = inParam.m_deviceId + CC.XIANWAN_SECRET;
        string sign = Helper.getMD5(wait);
        return sign == inParam.m_sign;
    }
}

//////////////////////////////////////////////////////////////////////////
public class DDZReg : ReqAccInfo
{
    public string m_rtime;
}

// 蛋蛋赚，查询注册信息
public class CDanDanZhuanQueryReg
{
    static string[] S_FIELDS = { "acc_real"/*, "regedittime"*/ };
    static string[] PLAYER_FIELDS = { "player_id", "nickname", "create_time" };

    public string queryReg(object param)
    {
        Dictionary<string, object> ret = new Dictionary<string, object>();
        DDZReg info = null;

        HttpRequest request = (HttpRequest)param;
        do
        {
            try
            {
                ReqInfoParam inParam = null;
                if (getParam(request, ref inParam) != QueryPlayerInfo.RET_SUCCESS)
                {
                    ret.Add("status", QueryPlayerInfo.RET_PARAM_ERROR);
                    break;
                }
                if(!checkSign(inParam))
                {
                    ret.Add("status", QueryPlayerInfo.RET_SIGN_ERROR);
                    break;
                }
                info = queryInfo(inParam);
                if (info == null)
                {
                    ret.Add("status", QueryPlayerInfo.RET_NO_PLAYER);
                    break;
                }

                return getRegAccInfo(ret, info);
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }
        } while (false);

        return JsonHelper.genJson(ret);
    }

    int getParam(HttpRequest req, ref ReqInfoParam data)
    {
        string deviceid = req.QueryString["deviceid"];
        if (string.IsNullOrEmpty(deviceid))
        {
            CLOG.Info("CDanDanZhuanQueryReg.getParam, deviceid param lost");
            return QueryPlayerInfo.RET_PARAM_ERROR;
        }
        string sign = req.QueryString["keycode"];
        if (string.IsNullOrEmpty(deviceid))
        {
            CLOG.Info("CDanDanZhuanQueryReg.getParam, keycode param lost");
            return QueryPlayerInfo.RET_PARAM_ERROR;
        }

        data = new ReqInfoParam();
        data.m_deviceId = deviceid.Trim();
        data.m_sign = sign.Trim();

        return QueryPlayerInfo.RET_SUCCESS;
    }

    string getRegAccInfo(Dictionary<string, object> ret, DDZReg info)
    {
        if (info != null)
        {
            ret.Add("status", 0);
            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("Userid", info.m_playerId);
            d.Add("GameName", info.m_nickName);
            d.Add("Deviceid", info.m_deviceId);
            d.Add("Channel", "100009");
            d.Add("rtime", Convert.ToInt64(info.m_rtime));
            ret.Add("data", d);
        }
        else
        {
            ret.Add("success", 0);
        }

        return JsonHelper.genJson(ret);
    }

    DDZReg queryInfo(ReqInfoParam param)
    {
        Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetBykey(PayTable.DANDANZHUAN_ACC, "deviceId", param.m_deviceId, S_FIELDS);
        if (data == null)
            return null;

        if (!data.ContainsKey("acc_real"))
            return null;

        string accReal = Convert.ToString(data["acc_real"]);
        Dictionary<string, object> pd = MongodbPlayer.Instance.ExecuteGetBykey("player_info", "account", accReal, PLAYER_FIELDS);
        if (pd == null)
            return null;

        DDZReg info = new DDZReg();
        info.m_deviceId = param.m_deviceId;
        info.m_nickName = Convert.ToString(pd["nickname"]);
        info.m_playerId = Convert.ToInt32(pd["player_id"]);
        info.m_rtime = Tool.getTimeStamp( Convert.ToDateTime(pd["create_time"]).ToLocalTime()).ToString(); //.ToString(PayConstDef.DATE_TIME_FORMAT);
        return info;
    }

    bool checkSign(ReqInfoParam inParam)
    {
        string wait = inParam.m_deviceId + CC.DANDANZHUAN_SECRET;
        string sign = Helper.getMD5(wait);
        return sign == inParam.m_sign;
    }
}

// 蛋蛋赚查询玩家信息
public class CDanDanZhuanQueryPlayerInfo
{
    static string[] S_ACC_FIELD = { "acc_real" };
    static string[] S_PLAYER_INFO_FIELD = { "nickname", "player_id", "PlayerLevel", "recharged" };

    Dictionary<string, object> m_retData = new Dictionary<string, object>();

    PlayerInfo m_outData = new PlayerInfo();

    public string doQuery(object param)
    {
        HttpRequest req = (HttpRequest)param;

        do
        {
            try
            {
                InfoParam dataInfo = null;
                int code = getParam(req, ref dataInfo);
                if (code != QueryPlayerInfo.RET_SUCCESS)
                {
                    m_retData.Add("status", code);
                    break;
                }
                if (!checkSign(dataInfo))
                {
                    m_retData.Add("status", QueryPlayerInfo.RET_SIGN_ERROR);
                    break;
                }

                if (queryPlayerInfo(dataInfo) == null)
                {
                    m_retData.Add("status", QueryPlayerInfo.RET_NO_PLAYER);
                    break;
                }
                if (queryGameData(dataInfo) != QueryPlayerInfo.RET_SUCCESS)
                {
                    m_retData.Add("status", QueryPlayerInfo.RET_NO_PLAYER);
                    break;
                }

                m_retData.Add("status", 0);
                Dictionary<string, object> d = new Dictionary<string, object>();
                d.Add("userid", Convert.ToInt32(dataInfo.m_userid));
                d.Add("GameName", m_outData.m_nickName);
                d.Add("WinGold", m_outData.m_winGold);
                d.Add("PlayLevel", m_outData.m_playerLevel);
                d.Add("payamount", m_outData.m_recharged);
                m_retData.Add("data", d);
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }
        } while (false);

        return JsonHelper.genJson(m_retData);
    }

    int getParam(HttpRequest req, ref InfoParam data)
    {
        string userid = req.QueryString["userid"];
        if (string.IsNullOrEmpty(userid))
        {
            CLOG.Info("CDanDanZhuanQueryPlayerInfo.getParam, userid param lost");
            return QueryPlayerInfo.RET_PARAM_ERROR;
        }
        string keycode = req.QueryString["keycode"];
        if (string.IsNullOrEmpty(keycode))
        {
            CLOG.Info("CDanDanZhuanQueryPlayerInfo.getParam, keycode param lost");
            return QueryPlayerInfo.RET_PARAM_ERROR;
        }
        string timeStr1 = req["startTime"];
        DateTime startTime = new DateTime();
        if (!ApiQueryData.paraseTime(timeStr1, false, ref startTime))
        {
            CLOG.Info("CDanDanZhuanQueryPlayerInfo.getParam error, {0}", timeStr1);
            return QueryPlayerInfo.RET_PARAM_ERROR;
        }

        DateTime endTime = new DateTime();
        string timeStr2 = req["endTime"];
        if (!ApiQueryData.paraseTime(timeStr2, true, ref endTime))
        {
            CLOG.Info("QueryPlayerInfo.getParam param error, {0}", timeStr2);
            return QueryPlayerInfo.RET_PARAM_ERROR;
        }

        data = new InfoParam();
        // data.m_channel = channel;
        data.m_sign = keycode.Trim();
        data.m_userid = userid.Trim();
        data.m_startTimeStr = timeStr1.Trim();
        data.m_endTimeStr = timeStr2.Trim();
        data.m_startTime = startTime;
        data.m_endTime = endTime;

        return QueryPlayerInfo.RET_SUCCESS;
    }

    bool checkSign(InfoParam data)
    {
        string wait = data.m_userid + data.m_startTimeStr + data.m_endTimeStr + CC.DANDANZHUAN_SECRET;
        string sign = Common.Helper.getMD5(wait);
        return sign == data.m_sign;
    }

    PlayerInfo queryPlayerInfo(InfoParam data)
    {
        /*
                string tableName = PayTable.getAccountTableByPlatform("guanggao");
                Dictionary<string, object> dic = MongodbAccount.Instance.ExecuteGetBykey(tableName, "acc", data.m_userid, S_ACC_FIELD);
                if (dic == null) return null;

                if(!dic.ContainsKey("acc_real"))
                    return null;

                string accReal=Convert.ToString(dic["acc_real"]);
        */

        Dictionary<string, object> player = MongodbPlayer.Instance.ExecuteGetBykey("player_info", "player_id", Convert.ToInt32(data.m_userid), S_PLAYER_INFO_FIELD);
        if (player == null)
            return null;

        /* if (player.ContainsKey("player_id"))
         {
             info.m_playerId = Convert.ToInt32(player["player_id"]);
         }*/
        if (player.ContainsKey("nickname"))
        {
            m_outData.m_nickName = Convert.ToString(player["nickname"]);
        }
        if (player.ContainsKey("PlayerLevel"))
        {
            m_outData.m_playerLevel = Convert.ToInt32(player["PlayerLevel"]);
        }
        /*if (player.ContainsKey("recharged"))
        {
            m_outData.m_recharged = Convert.ToInt32(player["recharged"]);
        }*/
        return m_outData;
    }

    // 查询游戏数据
    int queryGameData(InfoParam data)
    {
        int playerId = Convert.ToInt32(data.m_userid);
        //long winGold = 0;
        TDataInfo tdata = null;
        bool res = HttpApi.Global.getTCacheData().getWinGold(playerId, ref tdata);
        if (res)
        {
            TPlayerInfo tinfo = (TPlayerInfo)tdata.m_data;
            m_outData.m_winGold = tinfo.m_winGold;
            m_outData.m_recharged = (int)tinfo.m_recharge;
            return QueryPlayerInfo.RET_SUCCESS;
        }

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(data.m_endTime));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(data.m_startTime));
        IMongoQuery imq = Query.And(Query.EQ("playerId", playerId), imq1, imq2);

        List<Dictionary<string, object>> retData = MongodbLog.Instance.ExecuteGetListByQuery("pumpChannelPlayer100009", imq);
        if (retData == null)
            return QueryPlayerInfo.RET_SUCCESS;

        if (retData.Count == 0)
            return QueryPlayerInfo.RET_SUCCESS;

        for (int i = 0; i < retData.Count; ++i)
        {
            Dictionary<string, object> d = retData[i];
            if (d.ContainsKey("win_gold"))
            {
                m_outData.m_winGold += Convert.ToInt64(d["win_gold"]);
            }
            if (d.ContainsKey("payamount"))
            {
                m_outData.m_recharged += Convert.ToInt32(d["payamount"]);
            }
        }

        TPlayerInfo tinfo2 = new TPlayerInfo();
        tinfo2.m_recharge = m_outData.m_recharged;
        tinfo2.m_winGold = m_outData.m_winGold;
        HttpApi.Global.getTCacheData().addWinGold(playerId, tinfo2);
        return QueryPlayerInfo.RET_SUCCESS;
    }
}

//////////////////////////////////////////////////////////////////////////
public class CQueryRegBase
{
    // 加密密钥
    public string Secret { set; get; }

    // 错误输出的前缀串
    public string OutErrPrefix { set; get; }

    // 账号所在的表名
    public string AccTable { set; get; }

    // code status  success 等名称
    public string StatusCodeName { set; get; }

    protected static string[] S_FIELDS = { "acc_real" };
    protected static string[] PLAYER_FIELDS = { "player_id", "nickname", "create_time" };

    public const int RET_SUCCESS = 1001;
    public const int RET_PARAM_ERROR = 1002;
    public const int RET_SIGN_ERROR = 1003;
    public const int RET_NO_PLAYER = 1004;

    public string doQuery(object param)
    {
        Dictionary<string, object> ret = new Dictionary<string, object>();
        object info = null;

        HttpRequest request = (HttpRequest)param;
        do
        {
            try
            {
                object inParam = null; // ReqInfoParam
                if (getParam(request, ref inParam) != CQueryRegBase.RET_SUCCESS)
                {
                    ret.Add(StatusCodeName, CQueryRegBase.RET_PARAM_ERROR);
                    break;
                }
                if (!checkSign(inParam))
                {
                    ret.Add(StatusCodeName, CQueryRegBase.RET_SIGN_ERROR);
                    break;
                }
                info = queryInfo(inParam);
                if (info == null)
                {
                    ret.Add(StatusCodeName, CQueryRegBase.RET_NO_PLAYER);
                    break;
                }

                return getRegAccInfo(ret, info);
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }
        } while (false);

        return JsonHelper.genJson(ret);
    }

    // 获取输入参数
    protected virtual int getParam(HttpRequest req, ref object dataOut)
    {
        string deviceid = req.QueryString["deviceid"];
        if (string.IsNullOrEmpty(deviceid))
        {
            CLOG.Info("{0}.getParam, deviceid param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string sign = req.QueryString["keycode"];
        if (string.IsNullOrEmpty(deviceid))
        {
            CLOG.Info("{0}.getParam, keycode param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        var data = new ReqInfoParam();
        data.m_deviceId = deviceid.Trim();
        data.m_sign = sign.Trim();
        dataOut = data;
        return CQueryRegBase.RET_SUCCESS;
    }

    protected virtual object queryInfo(object _param)
    {
        ReqInfoParam param = (ReqInfoParam)_param;
        Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetBykey(AccTable, "deviceId", param.m_deviceId, S_FIELDS);
        if (data == null)
            return null;

        if (!data.ContainsKey("acc_real"))
            return null;

        string accReal = Convert.ToString(data["acc_real"]);
        Dictionary<string, object> pd = MongodbPlayer.Instance.ExecuteGetBykey("player_info", "account", accReal, PLAYER_FIELDS);
        if (pd == null)
            return null;

        DDZReg info = new DDZReg();
        info.m_deviceId = param.m_deviceId;
        info.m_nickName = Convert.ToString(pd["nickname"]);
        info.m_playerId = Convert.ToInt32(pd["player_id"]);
        info.m_rtime = Convert.ToDateTime(pd["create_time"]).ToLocalTime().ToString(PayConstDef.DATE_TIME_FORMAT);
        return info;
    }

    protected virtual bool checkSign(object _inParam)
    {
        ReqInfoParam inParam = (ReqInfoParam)_inParam;
        string wait = inParam.m_deviceId + Secret;
        string sign = Helper.getMD5(wait);
        return sign == inParam.m_sign;
    }

    protected virtual string getRegAccInfo(Dictionary<string, object> ret, object infoIn)
    {
        return "";
       /* DDZReg info = (DDZReg)_info;
        if (info != null)
        {
            ret.Add("status", 0);
            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("Userid", info.m_playerId);
            d.Add("GameName", info.m_nickName);
            d.Add("Deviceid", info.m_deviceId);
            d.Add("Channel", "100009");
            d.Add("rtime", info.m_rtime);
            ret.Add("data", d);
        }
        else
        {
            ret.Add("success", 0);
        }

        return JsonHelper.genJson(ret);*/
    }
}

// 查询具体的玩家信息
public class CQueryPlayerInfoBase
{
    protected static string[] S_ACC_FIELD = { "acc_real" };
    protected static string[] S_PLAYER_INFO_FIELD = { "nickname", "player_id", "PlayerLevel", "recharged", "create_time" };

    // 加密密钥
    public string Secret { set; get; }

    // 错误输出的前缀串
    public string OutErrPrefix { set; get; }

    // 统计数据所在的表
    public string PumpTableName { set; get; }

    // code status  success 等名称
    public string StatusCodeName { set; get; }

    Dictionary<string, object> m_retData = new Dictionary<string, object>();

    // 输入数据
    public object m_outData = new PlayerInfo();

    public string doQuery(object param)
    {
        HttpRequest req = (HttpRequest)param;

        do
        {
            try
            {
                object dataInfoIn = null;
                int code = getParam(req, ref dataInfoIn);
                if (code != CQueryRegBase.RET_SUCCESS)
                {
                    m_retData.Add(StatusCodeName, code);
                    break;
                }
                if (!checkSign(dataInfoIn))
                {
                    m_retData.Add(StatusCodeName, CQueryRegBase.RET_SIGN_ERROR);
                    break;
                }
                if (queryPlayerInfo(dataInfoIn) == null)
                {
                    m_retData.Add(StatusCodeName, CQueryRegBase.RET_NO_PLAYER);
                    break;
                }
                if (queryGameData(dataInfoIn) != CQueryRegBase.RET_SUCCESS)
                {
                    m_retData.Add(StatusCodeName, CQueryRegBase.RET_NO_PLAYER);
                    break;
                }

                fillDataResult(m_retData, dataInfoIn, m_outData);
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }
        } while (false);

        return JsonHelper.genJson(m_retData);
    }

    protected virtual int getParam(HttpRequest req, ref object dataOut)
    {
        string userid = req.QueryString["userid"];
        if (string.IsNullOrEmpty(userid))
        {
            CLOG.Info("{0}.getParam, userid param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string keycode = req.QueryString["keycode"];
        if (string.IsNullOrEmpty(keycode))
        {
            CLOG.Info("{0}.getParam, keycode param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string timeStr1 = req["startTime"];
        DateTime startTime = new DateTime();
        if (!ApiQueryData.paraseTime(timeStr1, false, ref startTime))
        {
            CLOG.Info("{0}.getParam error, {1}", OutErrPrefix,timeStr1);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        DateTime endTime = new DateTime();
        string timeStr2 = req["endTime"];
        if (!ApiQueryData.paraseTime(timeStr2, true, ref endTime))
        {
            CLOG.Info("{0}.getParam param error, {1}", OutErrPrefix, timeStr2);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        var data = new InfoParam();
        data.m_sign = keycode.Trim();
        data.m_userid = userid.Trim();
        data.m_startTimeStr = timeStr1.Trim();
        data.m_endTimeStr = timeStr2.Trim();
        data.m_startTime = startTime;
        data.m_endTime = endTime;
        dataOut = data;
        return CQueryRegBase.RET_SUCCESS;
    }

    protected virtual bool checkSign(object dataIn)
    {
        InfoParam data = (InfoParam)dataIn;
        string wait = data.m_userid + data.m_startTimeStr + data.m_endTimeStr + Secret;
        string sign = Common.Helper.getMD5(wait);
        return sign == data.m_sign;
    }

    protected virtual object queryPlayerInfo(object dataIn)
    {
        InfoParam data = (InfoParam)dataIn;
        Dictionary<string, object> player = MongodbPlayer.Instance.ExecuteGetBykey("player_info", "player_id", Convert.ToInt32(data.m_userid), S_PLAYER_INFO_FIELD);
        if (player == null)
            return null;

        PlayerInfo outData = (PlayerInfo)m_outData;
        outData.m_playerId = Convert.ToInt32(data.m_userid);

        if (player.ContainsKey("nickname"))
        {
            outData.m_nickName = Convert.ToString(player["nickname"]);
        }
        if (player.ContainsKey("PlayerLevel"))
        {
            outData.m_playerLevel = Convert.ToInt32(player["PlayerLevel"]);
        }
        if (player.ContainsKey("create_time"))
        {
            outData.m_regTime = Tool.getTimeStamp(Convert.ToDateTime(player["create_time"]).ToLocalTime());
        }
//         if (player.ContainsKey("recharged"))
//         {
//             outData.m_recharged = Convert.ToInt32(player["recharged"]);
//         }
        return outData;
    }

    // 查询游戏数据
    protected virtual int queryGameData(object dataIn)
    {
        InfoParam data = (InfoParam)dataIn;
        int playerId = Convert.ToInt32(data.m_userid);
        TDataInfo winGold = null;
        bool res = HttpApi.Global.getTCacheData().getWinGold(playerId, ref winGold);
        var outData = (PlayerInfo)m_outData;
        if (res)
        {
            TPlayerInfo ti = (TPlayerInfo)winGold.m_data;
            outData.m_winGold = ti.m_winGold;
            outData.m_recharged = (int)ti.m_recharge;
            return CQueryRegBase.RET_SUCCESS;
        }

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(data.m_endTime));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(data.m_startTime));
        IMongoQuery imq = Query.And(Query.EQ("playerId", playerId), imq1, imq2);

        List<Dictionary<string, object>> retData = MongodbLog.Instance.ExecuteGetListByQuery(PumpTableName, imq);
        if (retData == null)
            return CQueryRegBase.RET_SUCCESS;

        if (retData.Count == 0)
            return CQueryRegBase.RET_SUCCESS;

        for (int i = 0; i < retData.Count; ++i)
        {
            Dictionary<string, object> d = retData[i];
            if (d.ContainsKey("win_gold"))
            {
                outData.m_winGold += Convert.ToInt64(d["win_gold"]);
            }
            else if (d.ContainsKey("room_01_wingold"))
            {
                outData.m_winGold += Convert.ToInt64(d["room_01_wingold"]);
            }
            if (d.ContainsKey("payamount"))
            {
                outData.m_recharged += Convert.ToInt32(d["payamount"]);
            }
            else if (d.ContainsKey("time_payamount"))
            {
                outData.m_recharged += Convert.ToInt32(d["time_payamount"]);
            }
        }

        TPlayerInfo tpi = new TPlayerInfo();
        tpi.m_recharge = outData.m_recharged;
        tpi.m_winGold = outData.m_winGold;
        HttpApi.Global.getTCacheData().addWinGold(playerId, tpi);
        return CQueryRegBase.RET_SUCCESS;
    }

    protected virtual void fillDataResult(Dictionary<string, object> result, object dataIn, object dataOut)
    {
    }
}

//////////////////////////////////////////////////////////////////////////
public class CHuluQueryReg : CQueryRegBase
{
    protected override string getRegAccInfo(Dictionary<string, object> ret, object infoRegIn)
    {
        DDZReg info = (DDZReg)infoRegIn;
        if (info != null)
        {
            ret.Add(StatusCodeName, 1);
            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("userid", info.m_playerId);
            d.Add("nickname", info.m_nickName);
            ret.Add("data", d);
        }
        else
        {
            ret.Add(StatusCodeName, 0);
        }

        return JsonHelper.genJson(ret);
    }
}

public class CHuluQueryGame : CQueryPlayerInfoBase
{
    protected override int getParam(HttpRequest req, ref object dataOut)
    {
        string userid = req.QueryString["userid"];
        if (string.IsNullOrEmpty(userid))
        {
            CLOG.Info("{0}.getParam, userid param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string keycode = req.QueryString["keycode"];
        if (string.IsNullOrEmpty(keycode))
        {
            CLOG.Info("{0}.getParam, keycode param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string timeStr1 = DEF.START_TIME_HULU;//req["startTime"];
        DateTime startTime = new DateTime();
        if (!ApiQueryData.paraseTime(timeStr1, false, ref startTime))
        {
            CLOG.Info("{0}.getParam error, {1}", OutErrPrefix, timeStr1);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        DateTime endTime = new DateTime();
        string timeStr2 = DEF.END_TIME_HULU;//req["endTime"];
        if (!ApiQueryData.paraseTime(timeStr2, true, ref endTime))
        {
            CLOG.Info("{0}.getParam param error, {1}", OutErrPrefix, timeStr2);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        var data = new InfoParam();
        data.m_sign = keycode.Trim();
        data.m_userid = userid.Trim();
        data.m_startTimeStr = timeStr1.Trim();
        data.m_endTimeStr = timeStr2.Trim();
        data.m_startTime = startTime;
        data.m_endTime = endTime;
        dataOut = data;
        return CQueryRegBase.RET_SUCCESS;
    }

    protected override bool checkSign(object dataIn)
    {
        InfoParam data = (InfoParam)dataIn;
        string wait = data.m_userid + Secret;
        string sign = Common.Helper.getMD5(wait);
        return sign == data.m_sign;
    }

    protected override void fillDataResult(Dictionary<string, object> result, object dataIn, object dataOut)
    {
        InfoParam inParam = (InfoParam)dataIn;
        PlayerInfo outParam = (PlayerInfo)dataOut;

        result.Add(StatusCodeName, 1);
        Dictionary<string, object> d = new Dictionary<string, object>();
        d.Add("win_gold", outParam.m_winGold);
        d.Add("payamount", outParam.m_recharged);
        d.Add("username", outParam.m_nickName);
        d.Add("regTime", outParam.m_regTime);
        result.Add("data", d);
    }
}

//////////////////////////////////////////////////////////////////////////
public class CYouZhuanQueryReg : CQueryRegBase
{
    protected override string getRegAccInfo(Dictionary<string, object> ret, object infoRegIn)
    {
        DDZReg info = (DDZReg)infoRegIn;
        if (info != null)
        {
            ret.Add(StatusCodeName, 1); // 1成功
            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("userid", info.m_playerId);
            d.Add("nickname", info.m_nickName);
            ret.Add("data", d);
        }
        else
        {
            ret.Add(StatusCodeName, 0);
        }

        return JsonHelper.genJson(ret);
    }
}

public class CYouZhuanQueryGame : CQueryPlayerInfoBase
{
    protected override int getParam(HttpRequest req, ref object dataOut)
    {
        string userid = req.QueryString["userid"];
        if (string.IsNullOrEmpty(userid))
        {
            CLOG.Info("{0}.getParam, userid param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string keycode = req.QueryString["keycode"];
        if (string.IsNullOrEmpty(keycode))
        {
            CLOG.Info("{0}.getParam, keycode param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string timeStr1 = DEF.START_TIME_YOUZHUAN;//req["startTime"];
        DateTime startTime = new DateTime();
        if (!ApiQueryData.paraseTime(timeStr1, false, ref startTime))
        {
            CLOG.Info("{0}.getParam error, {1}", OutErrPrefix, timeStr1);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        DateTime endTime = new DateTime();
        string timeStr2 = DEF.END_TIME_YOUZHUAN;//req["endTime"];
        if (!ApiQueryData.paraseTime(timeStr2, true, ref endTime))
        {
            CLOG.Info("{0}.getParam param error, {1}", OutErrPrefix, timeStr2);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        var data = new InfoParam();
        data.m_sign = keycode.Trim();
        data.m_userid = userid.Trim();
        data.m_startTimeStr = timeStr1.Trim();
        data.m_endTimeStr = timeStr2.Trim();
        data.m_startTime = startTime;
        data.m_endTime = endTime;
        dataOut = data;
        return CQueryRegBase.RET_SUCCESS;
    }

    protected override bool checkSign(object dataIn)
    {
        InfoParam data = (InfoParam)dataIn;
        string wait = data.m_userid + Secret;
        string sign = Common.Helper.getMD5(wait);
        return sign == data.m_sign;
    }

    protected override void fillDataResult(Dictionary<string, object> result, object dataIn, object dataOut)
    {
        InfoParam inParam = (InfoParam)dataIn;
        PlayerInfo outParam = (PlayerInfo)dataOut;

        result.Add(StatusCodeName, 1);
        Dictionary<string, object> d = new Dictionary<string, object>();
        d.Add("win_gold", outParam.m_winGold);
        d.Add("payamount", outParam.m_recharged);
        d.Add("username", outParam.m_nickName);
        d.Add("regTime", outParam.m_regTime);
        result.Add("data", d);
    }
}

//////////////////////////////////////////////////////////////////////////
public class CMaiziZhuanQueryReg : CQueryRegBase
{
    protected override string getRegAccInfo(Dictionary<string, object> ret, object infoRegIn)
    {
        DDZReg info = (DDZReg)infoRegIn;
        if (info != null)
        {
            ret.Add(StatusCodeName, 1); // 1成功
            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("userid", info.m_playerId);
            d.Add("nickname", info.m_nickName);
            d.Add("createTime", info.m_rtime);
            ret.Add("data", d);
        }
        else
        {
            ret.Add(StatusCodeName, 0);
        }

        return JsonHelper.genJson(ret);
    }
}

public class CMaiziZhuanQueryGame : CQueryPlayerInfoBase
{
    protected override int getParam(HttpRequest req, ref object dataOut)
    {
        string userid = req.QueryString["userid"];
        if (string.IsNullOrEmpty(userid))
        {
            CLOG.Info("{0}.getParam, userid param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string keycode = req.QueryString["keycode"];
        if (string.IsNullOrEmpty(keycode))
        {
            CLOG.Info("{0}.getParam, keycode param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string timeStr1 = DEF.START_TIME_MAIZIZHUAN;//req["startTime"];
        DateTime startTime = new DateTime();
        if (!ApiQueryData.paraseTime(timeStr1, false, ref startTime))
        {
            CLOG.Info("{0}.getParam error, {1}", OutErrPrefix, timeStr1);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        DateTime endTime = new DateTime();
        string timeStr2 = DEF.END_TIME_MAIZIZHUAN;//req["endTime"];
        if (!ApiQueryData.paraseTime(timeStr2, true, ref endTime))
        {
            CLOG.Info("{0}.getParam param error, {1}", OutErrPrefix, timeStr2);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        var data = new InfoParam();
        data.m_sign = keycode.Trim();
        data.m_userid = userid.Trim();
        data.m_startTimeStr = timeStr1.Trim();
        data.m_endTimeStr = timeStr2.Trim();
        data.m_startTime = startTime;
        data.m_endTime = endTime;
        dataOut = data;
        return CQueryRegBase.RET_SUCCESS;
    }

    protected override bool checkSign(object dataIn)
    {
        InfoParam data = (InfoParam)dataIn;
        string wait = data.m_userid + Secret;
        string sign = Common.Helper.getMD5(wait);
        return sign == data.m_sign;
    }

    protected override void fillDataResult(Dictionary<string, object> result, object dataIn, object dataOut)
    {
        InfoParam inParam = (InfoParam)dataIn;
        PlayerInfo outParam = (PlayerInfo)dataOut;

        result.Add(StatusCodeName, 1);
        Dictionary<string, object> d = new Dictionary<string, object>();
        d.Add("win_gold", outParam.m_winGold);
        d.Add("payamount", outParam.m_recharged);
        d.Add("username", outParam.m_nickName);
        result.Add("data", d);
    }
}

//////////////////////////////////////////////////////////////////////////
public class JuxiangwanRegInfo : ReqInfoParam
{
    public string m_adId;  // 广告ID
}

// 聚享玩
public class CJuXiangWanQueryReg : CQueryRegBase
{
    // 获取输入参数
    protected override int getParam(HttpRequest req, ref object dataOut)
    {
        string deviceid = req.QueryString["device_code"];
        if (string.IsNullOrEmpty(deviceid))
        {
            CLOG.Info("{0}.getParam, device_code param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string adid = req.QueryString["bx_ad_id"];
        if (string.IsNullOrEmpty(adid))
        {
            CLOG.Info("{0}.getParam, bx_ad_id param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        string sign = req.QueryString["sign"];
        if (string.IsNullOrEmpty(sign))
        {
            CLOG.Info("{0}.getParam, sign param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        var data = new JuxiangwanRegInfo();
        data.m_deviceId = deviceid.Trim();
        data.m_adId = adid.Trim();
        data.m_sign = sign.Trim();
        dataOut = data;
        return CQueryRegBase.RET_SUCCESS;
    }

    protected override bool checkSign(object _inParam)
    {
        JuxiangwanRegInfo inParam = (JuxiangwanRegInfo)_inParam;
        string wait = inParam.m_adId + inParam.m_deviceId + Secret;
        string sign = Helper.getMD5(wait);
        return sign == inParam.m_sign;
    }

    protected override string getRegAccInfo(Dictionary<string, object> ret, object infoRegIn)
    {
        DDZReg info = (DDZReg)infoRegIn;
        if (info != null)
        {
            ret.Add(StatusCodeName, 1);             // 1成功
            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("play_account", info.m_playerId);  // 玩家ID
            d.Add("rolename", info.m_nickName);
            d.Add("regTime", info.m_rtime);          // 注册时间
            ret.Add("data", d);
        }
        else
        {
            ret.Add(StatusCodeName, 0);
        }

        return JsonHelper.genJson(ret);
    }
}

public class JuxiangwanInfo : InfoParam
{
    public string bx_ad_id;
    public string bx_utoken;
}

// 聚享玩游戏信息查询
public class CJuXiangWanQueryGame : CQueryPlayerInfoBase
{
    protected override int getParam(HttpRequest req, ref object dataOut)
    {
        string userid = req.QueryString["play_account"];
        if (string.IsNullOrEmpty(userid))
        {
            CLOG.Info("{0}.getParam, play_account param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string bx_utoken = req.QueryString["bx_utoken"];
        if (string.IsNullOrEmpty(userid))
        {
            CLOG.Info("{0}.getParam, bx_utoken param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string bx_ad_id = req.QueryString["bx_ad_id"];
        if (string.IsNullOrEmpty(userid))
        {
            CLOG.Info("{0}.getParam, bx_ad_id param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string sign = req.QueryString["sign"];
        if (string.IsNullOrEmpty(sign))
        {
            CLOG.Info("{0}.getParam, sign param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string timeStr1 = DEF.START_TIME_JUXIANGYOU;//req["startTime"];
        DateTime startTime = new DateTime();
        if (!ApiQueryData.paraseTime(timeStr1, false, ref startTime))
        {
            CLOG.Info("{0}.getParam error, {1}", OutErrPrefix, timeStr1);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        DateTime endTime = new DateTime();
        string timeStr2 = DEF.END_TIME_JUXIANGYOU;//req["endTime"];
        if (!ApiQueryData.paraseTime(timeStr2, true, ref endTime))
        {
            CLOG.Info("{0}.getParam param error, {1}", OutErrPrefix, timeStr2);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        var data = new JuxiangwanInfo();
        data.m_sign = sign.Trim();
        data.m_userid = userid.Trim();
        data.m_startTimeStr = timeStr1.Trim();
        data.m_endTimeStr = timeStr2.Trim();
        data.m_startTime = startTime;
        data.m_endTime = endTime;
        data.bx_ad_id = bx_ad_id;
        data.bx_utoken = bx_utoken;
        dataOut = data;
        return CQueryRegBase.RET_SUCCESS;
    }

    protected override bool checkSign(object dataIn)
    {
        JuxiangwanInfo data = (JuxiangwanInfo)dataIn;
        string wait = data.bx_ad_id + data.bx_utoken + data.m_userid + Secret;
        string sign = Common.Helper.getMD5(wait);
        return sign == data.m_sign;
    }

    protected override void fillDataResult(Dictionary<string, object> result, object dataIn, object dataOut)
    {
        PlayerInfo outParam = (PlayerInfo)dataOut;

        result.Add(StatusCodeName, 1);
        Dictionary<string, object> d = new Dictionary<string, object>();
        d.Add("win_gold", outParam.m_winGold);
        d.Add("payamount", outParam.m_recharged);
        d.Add("username", outParam.m_nickName);
        result.Add("data", d);
    }
}

//////////////////////////////////////////////////////////////////////////
// 小啄
public class CXiaoZhuoQueryReg : CQueryRegBase
{
    // 获取输入参数
    protected override int getParam(HttpRequest req, ref object dataOut)
    {
        string deviceid = req.QueryString["imei"];
        if (string.IsNullOrEmpty(deviceid))
        {
            CLOG.Info("{0}.getParam, imei param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string channel = req.QueryString["channel"];
        if (string.IsNullOrEmpty(channel))
        {
            CLOG.Info("{0}.getParam, channel param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        string sign = req.QueryString["keycode"];
        if (string.IsNullOrEmpty(sign))
        {
            CLOG.Info("{0}.getParam, keycode param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        var data = new JuxiangwanRegInfo();
        data.m_deviceId = deviceid.Trim();
        data.m_adId = channel.Trim();
        data.m_sign = sign.Trim();
        dataOut = data;
        return CQueryRegBase.RET_SUCCESS;
    }

    protected override bool checkSign(object _inParam)
    {
        JuxiangwanRegInfo inParam = (JuxiangwanRegInfo)_inParam;
        string wait = inParam.m_adId + inParam.m_deviceId + Secret;
        string sign = Helper.getMD5(wait);
        return sign == inParam.m_sign;
    }

    protected override string getRegAccInfo(Dictionary<string, object> ret, object infoRegIn)
    {
        DDZReg info = (DDZReg)infoRegIn;
        if (info != null)
        {
            // 1成功
            ret.Add(StatusCodeName, 1);            
            Dictionary<string, object> d = new Dictionary<string, object>();
            // 玩家ID
            d.Add("userid", info.m_playerId); 
            d.Add("nickname", info.m_nickName);
            d.Add("imei", info.m_deviceId);
            ret.Add("data", d);
        }
        else
        {
            ret.Add(StatusCodeName, 0);
        }

        return JsonHelper.genJson(ret);
    }
}

// 小啄游戏信息查询
public class CXiaoZhuoQueryGame : CQueryPlayerInfoBase
{
    protected override int getParam(HttpRequest req, ref object dataOut)
    {
        string userid = req.QueryString["userid"];
        if (string.IsNullOrEmpty(userid))
        {
            CLOG.Info("{0}.getParam, userid param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string channel = req.QueryString["channel"];
        if (string.IsNullOrEmpty(channel))
        {
            CLOG.Info("{0}.getParam, channel param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string sign = req.QueryString["keycode"];
        if (string.IsNullOrEmpty(sign))
        {
            CLOG.Info("{0}.getParam, keycode param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string timeStr1 = DEF.START_TIME_XIAOZHUO;
        DateTime startTime = new DateTime();
        if (!ApiQueryData.paraseTime(timeStr1, false, ref startTime))
        {
            CLOG.Info("{0}.getParam error, {1}", OutErrPrefix, timeStr1);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        DateTime endTime = new DateTime();
        string timeStr2 = DEF.END_TIME_XIAOZHUO;//req["endTime"];
        if (!ApiQueryData.paraseTime(timeStr2, true, ref endTime))
        {
            CLOG.Info("{0}.getParam param error, {1}", OutErrPrefix, timeStr2);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        var data = new JuxiangwanInfo();
        data.m_sign = sign.Trim();
        data.m_userid = userid.Trim();
        data.m_startTimeStr = timeStr1.Trim();
        data.m_endTimeStr = timeStr2.Trim();
        data.m_startTime = startTime;
        data.m_endTime = endTime;
        data.bx_utoken = channel;
        dataOut = data;
        return CQueryRegBase.RET_SUCCESS;
    }

    protected override bool checkSign(object dataIn)
    {
        JuxiangwanInfo data = (JuxiangwanInfo)dataIn;
        string wait = data.m_userid + data.bx_utoken + Secret;
        string sign = Common.Helper.getMD5(wait);
        return sign == data.m_sign;
    }

    protected override void fillDataResult(Dictionary<string, object> result, object dataIn, object dataOut)
    {
        PlayerInfo outParam = (PlayerInfo)dataOut;

        result.Add(StatusCodeName, 1);
        Dictionary<string, object> d = new Dictionary<string, object>();
        d.Add("userid", Convert.ToString(outParam.m_playerId));
        d.Add("payamount", outParam.m_recharged);
        d.Add("nickname", outParam.m_nickName);
        d.Add("level", outParam.m_playerLevel);
        d.Add("win_gold", outParam.m_winGold);
        d.Add("regTime", outParam.m_regTime);

        result.Add("data", d);
    }
}

//////////////////////////////////////////////////////////////////////////
// 泡泡赚
public class CPaoPaoZhuanQueryReg : CQueryRegBase
{
    protected override string getRegAccInfo(Dictionary<string, object> ret, object infoRegIn)
    {
        DDZReg info = (DDZReg)infoRegIn;
        if (info != null)
        {
            // 1成功
            ret.Add(StatusCodeName, 1);
            Dictionary<string, object> d = new Dictionary<string, object>();
            // 玩家ID
            d.Add("userid", info.m_playerId);
            d.Add("nickname", info.m_nickName);
            d.Add("deviceId", info.m_deviceId);
            ret.Add("data", d);
        }
        else
        {
            ret.Add(StatusCodeName, 0);
        }

        return JsonHelper.genJson(ret);
    }
}

// 泡泡赚游戏信息查询
public class CPaoPaoZhuanQueryGame : CQueryPlayerInfoBase
{
    protected override int getParam(HttpRequest req, ref object dataOut)
    {
        string userid = req.QueryString["userid"];
        if (string.IsNullOrEmpty(userid))
        {
            CLOG.Info("{0}.getParam, userid param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string sign = req.QueryString["keycode"];
        if (string.IsNullOrEmpty(sign))
        {
            CLOG.Info("{0}.getParam, keycode param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string timeStr1 = DEF.START_TIME_PAOPAOZHUAN;
        DateTime startTime = new DateTime();
        if (!ApiQueryData.paraseTime(timeStr1, false, ref startTime))
        {
            CLOG.Info("{0}.getParam error, {1}", OutErrPrefix, timeStr1);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        DateTime endTime = new DateTime();
        string timeStr2 = DEF.END_TIME_PAOPAOZHUAN;//req["endTime"];
        if (!ApiQueryData.paraseTime(timeStr2, true, ref endTime))
        {
            CLOG.Info("{0}.getParam param error, {1}", OutErrPrefix, timeStr2);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        var data = new JuxiangwanInfo();
        data.m_sign = sign.Trim();
        data.m_userid = userid.Trim();
        data.m_startTimeStr = timeStr1.Trim();
        data.m_endTimeStr = timeStr2.Trim();
        data.m_startTime = startTime;
        data.m_endTime = endTime;
        dataOut = data;
        return CQueryRegBase.RET_SUCCESS;
    }

    protected override bool checkSign(object dataIn)
    {
        JuxiangwanInfo data = (JuxiangwanInfo)dataIn;
        string wait = data.m_userid + Secret;
        string sign = Common.Helper.getMD5(wait);
        return sign == data.m_sign;
    }

    protected override void fillDataResult(Dictionary<string, object> result, object dataIn, object dataOut)
    {
        PlayerInfo outParam = (PlayerInfo)dataOut;

        result.Add(StatusCodeName, 1);
        Dictionary<string, object> d = new Dictionary<string, object>();
        d.Add("userid", Convert.ToString(outParam.m_playerId));
        d.Add("payamount", outParam.m_recharged);
        d.Add("nickname", outParam.m_nickName);
        //d.Add("level", outParam.m_playerLevel);
        d.Add("win_gold", outParam.m_winGold);
        d.Add("regTime", outParam.m_regTime);

        result.Add("data", d);
    }
}

////////////////////////////////////////////////////////
public class CDDQWQueryReg : CQueryRegBase
{
    protected override string getRegAccInfo(Dictionary<string, object> ret, object infoRegIn)
    {
        DDZReg info = (DDZReg)infoRegIn;
        if (info != null)
        {
            ret.Add(StatusCodeName, 1);
            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("userid", info.m_playerId);
            d.Add("nickname", info.m_nickName);
            ret.Add("data", d);
        }
        else
        {
            ret.Add(StatusCodeName, 0);
        }

        return JsonHelper.genJson(ret);
    }
}

public class CDDQWQueryGame : CQueryPlayerInfoBase
{
    protected override int getParam(HttpRequest req, ref object dataOut)
    {
        string userid = req.QueryString["userid"];
        if (string.IsNullOrEmpty(userid))
        {
            CLOG.Info("{0}.getParam, userid param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string keycode = req.QueryString["keycode"];
        if (string.IsNullOrEmpty(keycode))
        {
            CLOG.Info("{0}.getParam, keycode param lost", OutErrPrefix);
            return CQueryRegBase.RET_PARAM_ERROR;
        }
        string timeStr1 = DEF.START_TIME_DDQW;//req["startTime"];
        DateTime startTime = new DateTime();
        if (!ApiQueryData.paraseTime(timeStr1, false, ref startTime))
        {
            CLOG.Info("{0}.getParam error, {1}", OutErrPrefix, timeStr1);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        DateTime endTime = new DateTime();
        string timeStr2 = DEF.END_TIME_DDQW;//req["endTime"];
        if (!ApiQueryData.paraseTime(timeStr2, true, ref endTime))
        {
            CLOG.Info("{0}.getParam param error, {1}", OutErrPrefix, timeStr2);
            return CQueryRegBase.RET_PARAM_ERROR;
        }

        var data = new InfoParam();
        data.m_sign = keycode.Trim();
        data.m_userid = userid.Trim();
        data.m_startTimeStr = timeStr1.Trim();
        data.m_endTimeStr = timeStr2.Trim();
        data.m_startTime = startTime;
        data.m_endTime = endTime;
        dataOut = data;
        return CQueryRegBase.RET_SUCCESS;
    }

    protected override bool checkSign(object dataIn)
    {
        InfoParam data = (InfoParam)dataIn;
        string wait = data.m_userid + Secret;
        string sign = Common.Helper.getMD5(wait);
        return sign == data.m_sign;
    }

    protected override void fillDataResult(Dictionary<string, object> result, object dataIn, object dataOut)
    {
        InfoParam inParam = (InfoParam)dataIn;
        PlayerInfo outParam = (PlayerInfo)dataOut;

        result.Add(StatusCodeName, 1);
        Dictionary<string, object> d = new Dictionary<string, object>();
        d.Add("win_gold", outParam.m_winGold);
        d.Add("payamount", outParam.m_recharged);
        d.Add("username", outParam.m_nickName);
        d.Add("regTime", outParam.m_regTime);
        result.Add("data", d);
    }
}

























