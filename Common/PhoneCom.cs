using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Text;
using System.Security.Cryptography;
using System.Net;

public struct MsgPhone
{
    // 当前要使用的知信平台
    public static int MSG_USE = Convert.ToInt32(WebConfigurationManager.AppSettings["use"]);

    // 网易云信
    public const int WANG_YI_YUN_XIN = 3;

    public static string BIND_INFO = WebConfigurationManager.AppSettings["bind_info"];
    public static string SAFE_BOX_INFO = WebConfigurationManager.AppSettings["safeBoxInfo"];
    public static string VERIFY_INFO = WebConfigurationManager.AppSettings["verifyPhone"];
    public static string ANY_INFO = WebConfigurationManager.AppSettings["anyInfo"];
}

// 组装信息内容
public class SetUPInfo : MsgSendBase
{
    private StringBuilder m_urlBuilder = new StringBuilder();
    private StringBuilder m_textBuilder = new StringBuilder();

    // 组装验证码接口
    public override string setUpMsgInfoCheckInfo(string phone, string code, int type)
    {
        string fmt = "";
        switch (type)
        {
            case 0:
                {
                    fmt = MsgPhone.BIND_INFO;
                }
                break;
            case 1:
                {
                    fmt = MsgPhone.SAFE_BOX_INFO;
                }
                break;
            case 2:
                {
                    fmt = MsgPhone.VERIFY_INFO;
                }
                break;
            case 3:
                {
                    fmt = MsgPhone.ANY_INFO;
                }
                break;
            default:
                {
                    return "";
                }
                break;
        }
        /*if (type == 0)
        {
            fmt = WebConfigurationManager.AppSettings["bind_info"];
        }
        else
        {
            fmt = WebConfigurationManager.AppSettings["safeBoxInfo"];
        }*/

        string use = WebConfigurationManager.AppSettings["use"];
        string content = "";
        if (use == "1")
        {
            content = HttpUtility.UrlDecode(string.Format(fmt, code), Encoding.UTF8);
            return setUpMsgInfo_New(phone, content);
        }

        content = string.Format(fmt, code);
        return setUpMsgInfo(phone, content);
    }

    public override string send(string msg, int sendType)
    {
        try
        {
            var ret = HttpPost.Get(new Uri(msg));
            string retstr = Encoding.UTF8.GetString(ret);
            return retstr;
        }
        catch (System.Exception ex)
        {
            CLOG.Info("SetUPInfo.send, ex, {0}", ex.ToString());
            return "-1024";
        }
    }

    // 组装账号找回接口
    public string setUpMsgInfoSearchAccount(string phone, string account)
    {
        string use = WebConfigurationManager.AppSettings["use"];
        string content = "";
        if (use == "1")
        {
            content = HttpUtility.UrlDecode(string.Format(WebConfigurationManager.AppSettings["content"], account));
            return setUpMsgInfo_New(phone, content);
        }

        content = string.Format(WebConfigurationManager.AppSettings["content"], account);
        return setUpMsgInfo(phone, content);
    }

    public string setUpMsgInfoPhoneCode(string phone, string pwdcode)
    {
        string use = WebConfigurationManager.AppSettings["use"];
        string content = "";
        if (use == "1")
        {
            content = HttpUtility.UrlDecode(string.Format(WebConfigurationManager.AppSettings["pwdcode"], pwdcode));
            return setUpMsgInfo_New(phone, content);
        }

        content = string.Format(WebConfigurationManager.AppSettings["pwdcode"], pwdcode);
        return setUpMsgInfo(phone, content);
    }

    // 组装服务器监控短信
    public string setUpServerMonitorInfo(string phone, string serverIdList, string msgInfo)
    {
        string use = WebConfigurationManager.AppSettings["use"];
        string content = "";
        if (use == "1")
        {
            content = HttpUtility.UrlDecode(string.Format(WebConfigurationManager.AppSettings[msgInfo], serverIdList));
            return setUpMsgInfo_New(phone, content);
        }

        content = string.Format(WebConfigurationManager.AppSettings[msgInfo], serverIdList);
        return setUpMsgInfo(phone, content);
    }

    // 适配返回给服务端的值
    public override string adapterRetValue(string ret)
    {
        string use = WebConfigurationManager.AppSettings["use"];
        if (use == "1")
        {
            if (ret[0] != '0') // 1号接口，返回失败
            {
                return "1";
            }
            return "0"; // 返回0成功
        }

        if (ret[0] == '-') // 2号接口，返回 - 失败
        {
            return "1";
        }
        return "0"; // 返回0成功
    }

    // 组装发送内容
    private string setUpMsgInfo_New(string phone, string content)
    {
        string url = HttpUtility.UrlDecode(WebConfigurationManager.AppSettings["url"], Encoding.UTF8);
        string zh = HttpUtility.UrlDecode(WebConfigurationManager.AppSettings["account"], Encoding.UTF8);
        string pwd = HttpUtility.UrlDecode(WebConfigurationManager.AppSettings["pwd"], Encoding.UTF8);

        m_urlBuilder.Remove(0, m_urlBuilder.Length);
        m_urlBuilder.Append(url);
        m_urlBuilder.Append("?");
        m_urlBuilder.Append("account=");
        m_urlBuilder.Append(zh);
        m_urlBuilder.Append('&');

        m_urlBuilder.Append("password=");
        m_urlBuilder.Append(pwd);
        m_urlBuilder.Append('&');

        m_urlBuilder.Append("content=");
        m_urlBuilder.Append(content);

        m_urlBuilder.Append('&');

        m_urlBuilder.Append("sendtime=");

        m_urlBuilder.Append('&');
        m_urlBuilder.Append("phonelist=");
        m_urlBuilder.Append(phone);

        m_urlBuilder.Append('&');
        m_urlBuilder.Append("taskId=");

        DateTime now = DateTime.Now;
        string ss = genIdentifyingCode(new Random((int)DateTime.Now.Ticks));
        string str = string.Format("{0}_{1:D4}{2:D2}{3:D2}{4:D2}{5:D2}{6:D2}_http_{7}", 
            WebConfigurationManager.AppSettings["account"], now.Year, now.Month, now.Day,
            now.Hour, now.Minute, now.Second, ss);
        m_urlBuilder.Append(str);

        return m_urlBuilder.ToString();
    }

    private string setUpMsgInfo(string phone, string content)
    {
        string url = WebConfigurationManager.AppSettings["url2"];
        string zh = WebConfigurationManager.AppSettings["account2"];
        string pwd = WebConfigurationManager.AppSettings["pwd2"];
        //string id = WebConfigurationManager.AppSettings["sms_type"];

        m_urlBuilder.Remove(0, m_urlBuilder.Length);
        m_urlBuilder.Append(url);
        m_urlBuilder.Append("?");
        m_urlBuilder.Append("Uid=");
        m_urlBuilder.Append(zh);
        m_urlBuilder.Append('&');

        m_urlBuilder.Append("Key=");
        m_urlBuilder.Append(pwd);
        m_urlBuilder.Append('&');

        m_urlBuilder.Append("smsMob=");
        m_urlBuilder.Append(phone);
        m_urlBuilder.Append('&');

        m_urlBuilder.Append("smsText=");
        m_urlBuilder.Append(content);
        return m_urlBuilder.ToString();
    }

    // 生成6位随机数字验证码
    private string genIdentifyingCode(Random rand)
    {
        m_textBuilder.Remove(0, m_textBuilder.Length);
        int i = 0;
        for (i = 0; i < 6; i++)
        {
            m_textBuilder.Append(rand.Next(0, 10));
        }
        return m_textBuilder.ToString();
    }
}

public class CTool
{
    public static string getMD5Hash(String input)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] res = md5.ComputeHash(Encoding.Default.GetBytes(input), 0, input.Length);
        return BitConverter.ToString(res).Replace("-", "");
    }

    // 根据符号ch对串str进行拆分
    public static string[] split(string str, char ch)
    {
        char[] sp = { ch };
        string[] arr = str.Split(sp);
        return arr;
    }

    public static string encryptToSHA1(string str, bool toLower)
    {
        SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
        byte[] str1 = Encoding.UTF8.GetBytes(str);
        byte[] str2 = sha1.ComputeHash(str1);
        sha1.Clear();
        (sha1 as IDisposable).Dispose();
        if (toLower)
            return BitConverter.ToString(str2).Replace("-", "").ToLower();
        return BitConverter.ToString(str2).Replace("-", "");
    }
}

//////////////////////////////////////////////////////////////////////////
public class MsgSendBase
{
    public static MsgSendBase createMsg()
    {
        MsgSendBase obj = null;
        switch (MsgPhone.MSG_USE)
        {
            case MsgPhone.WANG_YI_YUN_XIN:
                {
                    obj = new MsgWangYi();
                }
                break;
            default:
                {
                    obj = new SetUPInfo(); 
                }
                break;
        }

        return obj;
    }

    public virtual string setUpMsgInfoCheckInfo(string phone, string code, int type) { throw new Exception("not implement"); }

    public virtual string adapterRetValue(string ret) { throw new Exception("not implement"); }

    public virtual string send(string msg, int sendType) { throw new Exception("not implement"); }
}

//////////////////////////////////////////////////////////////////////////
public class MsgWangYi : MsgSendBase
{
    static string APP_KEY = "3c15d36e164da558d318feab80c8a18b";
    static string APP_SECRET = "04c632bfbf4d";
    static string URL = "https://api.netease.im/sms/sendtemplate.action?templateid={0}&mobiles=[{1}]&params=[{2}]";
    static string TEMPLATE_BIND = "3045026";
    static string TEMPLATE_MODIFY = "3034577";
    
    string m_phone;
    string m_code;

    public override string setUpMsgInfoCheckInfo(string phone, string code, int type)
    {
        m_phone = phone;
        m_code = code;
        return "";
    }

    public override string adapterRetValue(string ret)
    {
        if (ret == "200")
            return "0"; // 返回0成功

        return "1";
    }

    public override string send(string msg, int sendType) 
    {
        string tmpMsg = "";
        if (sendType == 0)
        {
            tmpMsg = string.Format(URL, TEMPLATE_BIND, m_phone, m_code);
        }
        else
        {
            tmpMsg = string.Format(URL, TEMPLATE_MODIFY, m_phone, m_code);
        }

        string retCode = "";
        try
        {
            var uri = new Uri(tmpMsg);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            String nonce = new Random().Next().ToString();
            DateTime now = DateTime.Now;
            TimeSpan ts = now - DateTime.Parse("1970-01-01 00:00:00");
            string curTime = ts.TotalSeconds.ToString();
            string checkSum = CTool.encryptToSHA1(APP_SECRET + nonce + curTime, true);

            request.Headers.Add("AppKey", APP_KEY);
            request.Headers.Add("Nonce", nonce);
            request.Headers.Add("CurTime", curTime);
            request.Headers.Add("CheckSum", checkSum);
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            byte[] ret = HttpPost.Post2(uri, request);
            string retstr = Encoding.UTF8.GetString(ret);
            Dictionary<string, object> retObj = JsonHelper.ParseFromStr<Dictionary<string, object>>(retstr);
            retCode = Convert.ToString(retObj["code"]);
        }
        catch (System.Exception ex)
        {
            CLOG.Info("send msg:{0}", ex.ToString());
        }

        return retCode;
    }
}


