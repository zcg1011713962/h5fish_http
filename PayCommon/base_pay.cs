using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

public struct PayTable
{
    //qqgame
    public const string QQGAME_PAY = "qqgame_pay";

    public const string XUNLEI_PAY = "xunlei_pay";

    // 华为支付表
    public const string HUAWEI_PAY = "huawei_pay";

    public const string HUAWEI2_PAY = "huawei2_pay";

    public const string HUAWEI3_PAY = "huawei3_pay";

    public const string HUAWEI_BAIWAN_PAY = "huawei_baiwan_pay";

    public const string HUAWEI_FISHING_JOY2_PAY = "huawei_fishingJoy2_pay";

    // anysdk支付表
    public const string ANYSDK_PAY = "anysdk_pay";

    public const string QUICKSDK_PAY = "quicksdk_pay";
    public const string DUOYOU_PAY = "duoyou_pay";

    // 金立
    public const string JINLI_PAY = "jinli_pay";

    // ysdk支付
    public const string YSDK_PAY = "ysdk_pay";

    // vivo支付表
    public const string VIVO_PAY = "vivo_pay";
    public const string VIVO2_PAY = "vivo2_pay";
    public const string VIVO3_PAY = "vivo3_pay";
    public const string VIVO_BAI_WAN_PAY = "vivo_baiwan_pay";
    public const string VIVO_FISHING_JOY2 = "vivo_fishingJoy2_pay";

    // baidu2支付表
    public const string BAIDU2_PAY = "baidu2_pay";
    public const string BUYU2_BAIDU_PAY = "buyu2_baidu_pay";
    public const string BAIDU_FISHINGJOY2_PAY = "baidu_fishingJoy2_pay";

    public const string LOG_ANYSDK = "anysdk_log";

    // 支付总表
    public const string PAYMENT_TOTAL = "player_pay";

    // ysdk直购道具模式
    public const string YSDK_DIRECT_PAY = "ysdk_direct_pay";

    public const string YSDK_DIRECT_PAY2 = "ysdk_direct_pay2";

    public const string YSDK_DIRECT_PAY_CAIJIN_JINCHAN = "ysdk_direct_jinchan_pay";

    // 乐视支付表
    public const string LETV_PAY = "letv_pay";

    // 微信支付表
    public const string WEIXIN_PAY = "weixin_pay";

    public const string ALI_PAY = "ali_pay";

    public const string MEIZU_PAY = "meizu_pay";

    public const string XIAOMI_PAY = "xiaomi_pay";

    public const string XIAOMI_FISHING_JOY2_PAY = "xiaomi_fishingJoy2_pay";

    public const string XIAOMI_FISHING_JOY2_2_PAY = "xiaomi_fishingJoy2_2_pay";

    public const string FAN_PAY = "fan_pay";

    public const string AIBEI_PAY = "aibei_pay";

    public const string QINGYUAN_PAY = "qingyuan_pay";
    public const string TTHY_PAY = "tthy_pay";
    public const string TTHY_OPPO_PAY = "tthy_oppo_pay";

    public const string TTHY_YSDK_PAY = "tthy_ysdk_pay";
    public const string TTHY_YSDK_PAY_LOG = "tthy_ysdk_log";

    public const string OPPO_PAY = "oppo_pay";
    public const string YIJIE_PAY = "yijie_pay";

    public const string HOSPITAL_PAY = "hospital_pay";
    public const string UC_PAY = "uc_pay";
    public const string APPLE_PAY = "appstore_pay";

    //////////////////////////////////////////////////////////////////////////
    public const string VIVO_APP_ID = "86b605c0cdfda9c96963f5f5f789e5fd";
    public const string VIVO_CP_KEY = "02b0846d906e7303b84521d08a30f263";
    public const string VIVO_CP_ID = "20160125113929378044";

    public const string VIVO_CP_KEY2 = "83525dd6cad0b89ee67300cdc9d87900";

    public const string VIVO_CP_KEY3 = "a59f4610b846445c7d8b8bb5d13ec26d";

    public const string VIVO_CP_KEY_BAI_WAN = "4d9c89b7cd31f17fda2647c4ebdd959c";
    public const string VIVO_CP_KEY_FISHING_JOY2 = "12863755F2D5FF3673D15C74663C77EA";

    // 微信密钥
    public const string WEIXIN_API_SECRET = "9c2a89d907dc4416b33011293e8a24fa";
    // appid
    public const string WEIXIN_APPID = "wxecdeb723dda84bfe";

    // 正式
    public const string TTHY_YSDK_APPKEY = "puGm2f98AV0sJQOqtal8gSUYFpQSgajb";
    // 测试
    public const string TTHY_YSDK_APPKEY_TEST = "L3Tf4vHIkgKXBFtE";
    public const string TTHY_YSDK_APPID = "1106638202";

    public const string ALI_WEB_PAY = "ali_web_pay";

    //////////////////////////////////////////////////////////////////////////
    // 百度2，订单中转
    public const string BAIDU2_TRANSITION = "baidu2_transition";

    // 魅族单机secret
    public const string MEIZU_APP_SECRET = "hMfPE2a1nbvv6mDnRfUxXD2Ixv6HO6GJ";

    //////////////////////////////////////////////////////////////////////////
    public const string YSDK_APATH_KEY = "__apath";

    //////////////////////////////////////////////////////////////////////////
    // 账号总表
    public static string ACC_DEFAULT = "AccountTable";

    public static string ACC_ANYSDK = "anysdk_login";

    public static string PLATFORM_DEFAULT = "default";

    // 根据平台名称，获取表名
    public static string getPayTableName(string platform)
    {
        return platform + "_pay";
    }

    public static string getAccountTableByPlatform(string platform)
    {
        if (platform == PLATFORM_DEFAULT)
            return ACC_DEFAULT;

        return platform + "_acc";
    }

    public static string XIANWAN_ACC = "xianwan_acc";
    public static string DUOYOU_ACC = "duoyou_acc";
    public static string JUYAN_ACC = "juyan_acc";
    public static string WANYOU_ACC = "wanyou_acc";

    public static string DANDANZHUAN_ACC = "dandanzhuan_acc";
    public static string HULU_ACC = "hulu_acc";
    public static string YOUZHUAN_ACC = "youzhuan_acc";
    public static string MAIZIZHUAN_ACC = "maizizhuan_acc";
    public static string JUXIANGWAN_ACC = "juxiangwan_acc";
    public static string XIAOZHUO_ACC = "xiaozhuo_acc";
    public static string PAOPAOZHUAN_ACC = "paopaozhuan_acc";
    public static string DDQW_ACC = "ddqw_acc";

    public static string HUAWEI_ACC = "huawei_acc";
    public static string OPPO_ACC = "oppo_acc";
    public static string XIAOMI_ACC = "xiaomi_acc";

    //////////////////////////////////////////////////////////////////////////
    // 万游渠道ID
    public static string CHANNEL_WANYOU = "100300";

    public static string CHANNEL_HUAWEI = "100500";
}

public enum PayState
{
    paystate_success = 0,	    // 已发货， 支付成功
    paystate_has_req = 1,		// 请求支付
    paystate_has_pay = 2,		// 已支付(后台有回调)
};

public class PayInfoBase
{
    // 支付时间
    public DateTime m_payTime = DateTime.MinValue;

    // 支付code
    public string m_payCode;

    // 玩家ID
    public int m_playerId;

    // 账号
    public string m_account;

    public int m_rmb;

    public string m_orderId = "";

    public string m_channelNumber;

    // 订单状态
    public int m_state = (int)PayState.paystate_has_pay;

    // 获取基础保存数据
    public Dictionary<string, object> getBaseData()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        addBaseDataToDic(dic);
        return dic;
    }

    // 获取要更新的数据
    public Dictionary<string, object> getUpdateBaseData()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["status"] = m_state;
        if (m_payTime != DateTime.MinValue)
        {
            dic["PayTime"] = m_payTime;
        }
        return dic;
    }

    // 兼容之前的数据
    public void addBaseDataToDic(Dictionary<string, object> target)
    {
        if (target == null)
            return;

        target["PayTime"] = m_payTime;
        target["PayCode"] = m_payCode;
        target["PlayerId"] = m_playerId;
        target["Account"] = m_account;
        target["RMB"] = m_rmb;
        target["OrderID"] = m_orderId;
        target["Process"] = false;
        target["channel_number"] = m_channelNumber;
        // target["status"] = m_state;
    }
}

//////////////////////////////////////////////////////////////////////////

public class PayBase
{
    public static bool existOrderInPaymentTotal<T>(PayInfoBase baseData,
                                                   MongodbHelper<T> pdb) where T : new()
    {
        if (baseData == null)
            return false;

        return pdb.KeyExistsBykey(PayTable.PAYMENT_TOTAL, "OrderID", baseData.m_orderId);
    }

    // 更新付费总表中的数据
    public static void updateDataToPaymentTotal<T>(PayInfoBase baseData, MongodbHelper<T> pdb) where T : new()
    {
        Dictionary<string, object> data = baseData.getUpdateBaseData();
        string ret = pdb.ExecuteUpdate(PayTable.PAYMENT_TOTAL, "OrderID", baseData.m_orderId, data, UpdateFlags.None);
        if (ret != string.Empty)
        {
            CLOG.Info("PayBase, updateDataToPaymentTotal, ex:{0}", ret);
        }
    }

    public static Dictionary<string, object> queryBaseData<T>(string orderId,
                                                              MongodbHelper<T> pdb,
                                                              string[] fields = null
                                                              ) where T : new()
    {
        return pdb.ExecuteGetBykey(PayTable.PAYMENT_TOTAL, "OrderID", orderId, fields);
    }

    public static long getTS()
    {
        TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        return Convert.ToInt64(ts.TotalSeconds);
    }

    // 毫秒
    public static long getTSMill()
    {
        TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        return Convert.ToInt64(ts.TotalMilliseconds);
    }
}

//////////////////////////////////////////////////////////////////////////
public struct PayConstDef
{
    public const string URL_QQ_OPEN_API_TEST = "https://openapi.sparta.html5.qq.com/v3/user/get_info";

    public const string URL_QQ_OPEN_API = "http://openapi.tencentyun.com/v3/user/get_info";

    public static string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss"; 
}

public struct NotifyCC
{
    public const string NOTIFY_QQ = "1";
}


public struct QQgameCFG
{
    //appid
    public const int appid = 1110413989;
    public const string appkey = "8mwiDCdgKLs7TkDb";
    public const string SERVER_NAME_TEST = "openapi.sparta.html5.qq.com";
    public const string SERVER_NAME = "openapi.tencentyun.com";

    public const string QQ_GAME_CONFIRM_TABLE = "qqgame_pay_tmp";

    public const string qqgame_report_login = "";

}
