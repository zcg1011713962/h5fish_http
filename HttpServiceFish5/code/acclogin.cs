using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Configuration;
using Common;
using System.Text;

public struct DEF
{
    public static string LOGIN_FOR = WebConfigurationManager.AppSettings["loginfor"];
}

public class Login
{
    public static bool isAccLand()
    {
        return DEF.LOGIN_FOR == "fish3";
    }
}

public class AccLandContext
{
    // 账号创建时间
    public long CreateTime { set; get; }

    // 所绑定的手机号
    public string MobilePhone { set; get; }

    public string Acc { set; get; }

    public string Platform { set; get; }

    // 输出
    public string m_accName;

    // 输出
    public string m_userId;

    // 输出
    public string m_channel;
}

// 玩家id已绑定手机的前提下，从渠道表移入总表
public class AccLandBase
{
    public static string[] s_field = new string[] { "user_sdk", "uid", "deviceId", "ip", "lastip", 
        "channel", "randkey", "lasttime", "pwd", "platform", "regedittime", "regeditip" };

    public virtual void startLand(AccLandContext context)
    {
        throw new Exception("not implement");
    }

    public Dictionary<string, object> createUpData(Dictionary<string, object> data, AccLandContext context)
    {
        Dictionary<string, object> updata = new Dictionary<string, object>();

        if (data.ContainsKey("randkey"))
        {
            updata.Add("randkey", Convert.ToInt32(data["randkey"]));
        }
        if (data.ContainsKey("lasttime"))
        {
            updata.Add("lasttime", Convert.ToInt64(data["lasttime"]));
        }
        if (data.ContainsKey("deviceId"))
        {
            updata.Add("deviceId", Convert.ToString(data["deviceId"]));
        }
        if (data.ContainsKey("lastip"))
        {
            updata.Add("lastip", Convert.ToString(data["lastip"]));
        }

        updata.Add("acc", context.m_accName);
        updata.Add("acc_real", context.m_accName);
        updata.Add("updatepwd", false);
        updata.Add("bindPhone", context.MobilePhone);

        return updata;
    }

    public DateTime calRededitTime(AccLandContext context)
    {
        DateTime s = new DateTime(1970, 1, 1);
        DateTime res = s.AddSeconds(context.CreateTime).ToLocalTime();
        return res;
    }
}

//////////////////////////////////////////////////////////////////////////
public class AccLandAnysdk : AccLandBase
{
    public override void startLand(AccLandContext context)
    {
        if (string.IsNullOrEmpty(context.MobilePhone))
            return;

        Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetBykey(PayTable.ACC_ANYSDK, "acc", context.Acc, s_field);
        if (data == null)
            return;

        if (!data.ContainsKey("user_sdk") ||
            !data.ContainsKey("uid"))
            return;

        context.m_accName = context.Acc; // Convert.ToString(data["user_sdk"]).ToLower() + "_" + Convert.ToString(data["uid"]);

        if (MongodbAccount.Instance.KeyExistsBykey(PayTable.ACC_DEFAULT, "acc_real", context.m_accName))
        {
            return;
        }

        Dictionary<string, object> updata = createUpData(data, context);
        updata.Add("pwd", Helper.getMD5Upper("123456"));
        updata.Add("platform", "anysdk");

        if (data.ContainsKey("ip"))
        {
            updata.Add("regeditip", Convert.ToString(data["ip"]));
        }
        if (data.ContainsKey("channel"))
        {
            updata.Add("channel", Convert.ToString(data["channel"]));
        }

        updata.Add("regedittime", calRededitTime(context));

       // CLOG.Info("moveacc anysdk, req acc:{0}, newacc:{1} plat:{2}", context.Acc, context.m_accName, context.Platform);
        MongodbAccount.Instance.ExecuteInsert(PayTable.ACC_DEFAULT, updata);
    }
}

//////////////////////////////////////////////////////////////////////////
public class AccLandThirdParty : AccLandBase
{
    public override void startLand(AccLandContext context)
    {
        if (string.IsNullOrEmpty(context.MobilePhone))
            return;

        string table = ConfigurationManager.AppSettings["acc_" + context.Platform];
        if (string.IsNullOrEmpty(table))
        {
            return;
        }
        Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetBykey(table, "acc", context.Acc, s_field);
        if (data == null)
            return;

        string sdk = "";
        int code = PlatformSdk.getSdkByPlatform(context.Platform, ref sdk);
        if (code != PlatformSdk.RESULT_SUCCESS)
            return;

        bool res = buildContext(context);
        if (!res) return;

        context.m_accName = context.Acc; // sdk + "_" + context.m_userId;

        if (MongodbAccount.Instance.KeyExistsBykey(PayTable.ACC_DEFAULT, "acc_real", context.m_accName))
        {
            return;
        }

        Dictionary<string, object> updata = createUpData(data, context);
        if (data.ContainsKey("pwd"))
        {
            updata.Add("pwd", Convert.ToString(data["pwd"]).ToUpper());
        }
        else
        {
            updata.Add("pwd", Helper.getMD5Upper("123456"));
        }

        updata.Add("platform", context.Platform);

        if (data.ContainsKey("regeditip"))
        {
            updata.Add("regeditip", Convert.ToString(data["regeditip"]));
        }

        updata.Add("channel", context.m_channel);

        if (data.ContainsKey("regedittime"))
        {
            updata.Add("regedittime", Convert.ToDateTime(data["regedittime"]).ToLocalTime());
        }
        else
        {
            updata.Add("regedittime", calRededitTime(context));
        }

        //CLOG.Info("moveacc thirdparty, req acc:{0}, newacc:{1} plat:{2}", context.Acc, context.m_accName, context.Platform);

        MongodbAccount.Instance.ExecuteInsert(PayTable.ACC_DEFAULT, updata);
    }

    bool buildContext(AccLandContext context)
    {
        string[] arr = context.Acc.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
        if (arr.Length == 2)
        {
            context.m_channel = arr[0];
            context.m_userId = arr[1];

            return true;
        }

        //CLOG.Info("AccLandThirdParty.buildContext, cannot split channel,userid, plat:{0}, acc:{1}", context.Platform, context.Acc);

        return false;
    }
}

//////////////////////////////////////////////////////////////////////////
public class AccLandCheck
{
    static string[] s_fields = { "randkey", "lasttime", "lastip", "deviceId" };

    public string startCheck(AccLandContext context)
    {
        string retstr = "";
        Dictionary<string, object> data = getDataFromTotalAcc(context);
        retstr = buildMsg(data, true);

        if (!string.IsNullOrEmpty(retstr))
        {
            //CLOG.Info("acccheck, 总表中存在, acc:{0}, plat:{1}, island:true", context.Acc, context.Platform);
            return retstr;
        }

        string table = ConfigurationManager.AppSettings["acc_" + context.Platform];
        if (string.IsNullOrEmpty(table))
        {
            //CLOG.Info("AccLandCheck.startCheck, error platform,{0}", context.Platform);
            return buildMsg("error platform");
        }

        data = getDataFromPlatformAcc(table, context);
        retstr = buildMsg(data, false);
        if (string.IsNullOrEmpty(retstr))
        {
            //CLOG.Info("AccLandCheck.startCheck, db error, acc:{0}, platform:{1}", context.Acc, context.Platform);
            retstr = buildMsg("db error");
        }

        //CLOG.Info("acccheck, 渠道表中存在, acc:{0}, plat:{1}, island:false", context.Acc, context.Platform);

        return retstr;
    }

    Dictionary<string, object> getDataFromTotalAcc(AccLandContext context)
    {
        Dictionary<string, object> data =
            MongodbAccount.Instance.ExecuteGetBykey(PayTable.ACC_DEFAULT, "acc_real", context.Acc, s_fields);
        return data;
    }

    Dictionary<string, object> getDataFromPlatformAcc(string talbe, AccLandContext context)
    {
        Dictionary<string, object> data =
            MongodbAccount.Instance.ExecuteGetBykey(talbe, "acc", context.Acc, s_fields);
        return data;
    }

    string buildMsg(Dictionary<string, object> data, bool isLand)
    {
        string msg = "";
        if (data != null && data.Count >= 2)
        {
            string jsonstr = data["randkey"].ToString() + "_" + data["lasttime"].ToString();
            string lastIp = "";
            string deviceId = "";
            if (data.ContainsKey("lastip"))
            {
                lastIp = Convert.ToString(data["lastip"]);
            }
            if (data.ContainsKey("deviceId"))
            {
                deviceId = Convert.ToString(data["deviceId"]);
            }
            
            msg = buildMsg(jsonstr.Trim(), lastIp, deviceId, isLand, true);
        }

        return msg;
    }

    string buildMsg(string info, string lastIP = "", string deviceId = "", bool isLand = false, bool bret = false)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["result"] = bret;
        if (bret)
        {
            data["data"] = info;
            data["lastip"] = lastIP;
            data["deviceId"] = deviceId;
            data["isLand"] = isLand;
        }
        else
        {
            data["error"] = info;
        }

        string jsonstr = JsonHelper.ConvertToStr(data);
        return Convert.ToBase64String(Encoding.Default.GetBytes(jsonstr));
    }
}

//////////////////////////////////////////////////////////////////////////
public class MoveAccFromThirdParty
{
    public void startMove(AccLandContext context)
    {
        string sdk = "";
        int code = PlatformSdk.getSdkByPlatform(context.Platform, ref sdk);
        AccLandBase acc = null;
        if (code == PlatformSdk.RESULT_ANYSDK)
        {
            acc = new AccLandAnysdk();
        }
        else if (code == PlatformSdk.RESULT_SUCCESS)
        {
            acc = new AccLandThirdParty();
        }

        if (acc != null)
        {
            acc.startLand(context);
        }
    }
}





