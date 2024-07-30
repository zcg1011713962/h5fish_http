using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct PlatformName
{
    public const string PLAT_BAIDU2 = "baidu2";
    public const string PLAT_BAIDU_FISHING_JOY2 = "baidu_fishingJoy2";

    public const string PLAT_FAN = "fan";
    public const string PLAT_HUAWEI = "huawei";
    public const string PLAT_HUAWEI2 = "huawei2";
    public const string PLAT_HUAWEI3 = "huawei3";
    public const string PLAT_HUAWEI_BAIWAN = "huaweibaiwan";
    public const string PLAT_HUAWEI_FISHING_JOY2 = "huawei_fishingJoy2";

    public const string PLAT_LETV = "letv";
    public const string PLAT_WEIXIN = "weixin";
    public const string PLAT_XIAOMI = "xiaomi";
    public const string PLAT_XIAOMI_FISHING_JOY2 = "xiaomi_fishingJoy2";
    public const string PLAT_XIAOMI_FISHING_JOY2_2 = "xiaomi_fishingJoy2_2";

    public const string PLAT_XUNLEI = "xunlei";
    public const string PLAT_YSDK = "ysdk";
    public const string PLAT_TTHY_YSDK = "tthy_ysdk";

    public const string PLAT_QINGYUAN = "qingyuan";
    public const string PLAT_TTHY = "tthy";
    public const string PLAT_TTHY_OPPO = "tthy_oppo";
    public const string PLAT_OPPO = "oppo";
    public const string PLAT_YIJIE = "yijie";

    public const string PLAT_MEIZU_ONLINE = "meizu_online";

    public const string PLAT_HOSPITAL = "hospital";

    public const string PLAT_VIVO_FUHAO = "vivo_fuhao";
    public const string PLAT_VIVO_JIEJI = "vivo_jieji";
    public const string PLAT_VIVO_BAIWAN = "vivo_baiwan";
}

public class PlatformSdk
{
    public const string SDK_BAIDU = "baidu";
    public const string SDK_FAN = "fan";
    public const string SDK_HUAWEI = "huawei";
    public const string SDK_LETV = "letv";
    public const string SDK_WEIXIN = "weixin";
    public const string SDK_XIAOMI = "xiaomi";
    public const string SDK_XUNLEI = "xunlei";
    public const string SDK_YSDK = "txysdk";
    public const string SDK_QINGYUAN = "qingyuan";
    public const string SDK_TTHY = "tthy";
    public const string SDK_OPPO = "oppo";
    public const string SDK_YIJIE = "yijie";
    public const string SDK_MEIZU = "meizu";
    public const string SDK_AH = "ah";
    public const string SDK_VIVO = "vivo";

    public const int RESULT_SUCCESS = 0;
    public const int RESULT_ANYSDK = 1;
    public const int RESULT_NOT_FOUND_SDK = 2;

    // 平台名->sdk(单接sdk)
    static Dictionary<string, string> m_dic = new Dictionary<string, string>();

    static PlatformSdk()
    {
        m_dic.Add("baidu2", SDK_BAIDU);
        m_dic.Add(PlatformName.PLAT_BAIDU_FISHING_JOY2, SDK_BAIDU);
        m_dic.Add("fan", SDK_FAN);
        m_dic.Add("huawei", SDK_HUAWEI);
        m_dic.Add("huawei2", SDK_HUAWEI);
        m_dic.Add("huawei3", SDK_HUAWEI);
        m_dic.Add(PlatformName.PLAT_HUAWEI_BAIWAN, SDK_HUAWEI);
        m_dic.Add(PlatformName.PLAT_HUAWEI_FISHING_JOY2, SDK_HUAWEI);

        m_dic.Add("letv", SDK_LETV);
        m_dic.Add("weixin", SDK_WEIXIN);
        m_dic.Add("xiaomi", SDK_XIAOMI);
        m_dic.Add(PlatformName.PLAT_XIAOMI_FISHING_JOY2, SDK_XIAOMI);
        m_dic.Add(PlatformName.PLAT_XIAOMI_FISHING_JOY2_2, SDK_XIAOMI);

        m_dic.Add("xunlei", SDK_XUNLEI);
        m_dic.Add("ysdk", SDK_YSDK);
        m_dic.Add(PlatformName.PLAT_TTHY_YSDK, SDK_YSDK);

        m_dic.Add(PlatformName.PLAT_QINGYUAN, SDK_QINGYUAN);
        m_dic.Add(PlatformName.PLAT_TTHY, SDK_TTHY);
        m_dic.Add(PlatformName.PLAT_TTHY_OPPO, SDK_OPPO);

        m_dic.Add(PlatformName.PLAT_OPPO, SDK_OPPO);
        m_dic.Add(PlatformName.PLAT_YIJIE, SDK_YIJIE);

        m_dic.Add(PlatformName.PLAT_MEIZU_ONLINE, SDK_MEIZU);

        m_dic.Add(PlatformName.PLAT_HOSPITAL, SDK_AH);

        m_dic.Add(PlatformName.PLAT_VIVO_FUHAO, SDK_VIVO);
        m_dic.Add(PlatformName.PLAT_VIVO_JIEJI, SDK_VIVO);
        m_dic.Add(PlatformName.PLAT_VIVO_BAIWAN, SDK_VIVO);
    }

    public static int getSdkByPlatform(string platform, ref string sdk)
    {
        if (platform == "anysdk")
            return RESULT_ANYSDK;

        if (m_dic.ContainsKey(platform))
        {
            sdk = m_dic[platform];
            return RESULT_SUCCESS;
        }

        CLOG.Info("PlatformSdk.getSdkByPlatform, not found sdk for plat:{0}", platform);
        return RESULT_NOT_FOUND_SDK;
    }
}




