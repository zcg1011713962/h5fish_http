using System;
using System.Collections.Generic;
using System.Text;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System.IO;

public class CheckBase
{
    protected static URLInfo m_urlInfo;
    protected static string m_testCode;

    public string ContentType { private set; get; } = "application/json; charset=utf-8";

    protected static SecretInfo m_secretInfo;

    public static volatile bool m_isEnableLog = false;

    public static void init()
    {
        string itTestStr = ResMgr.getInstance().getString(JsonCfg.JSON_CONFIG, "isTest");
        bool isTest = Convert.ToBoolean(itTestStr);
        m_urlInfo = new URLInfo(isTest);

        m_testCode = ResMgr.getInstance().getString(JsonCfg.JSON_CONFIG, "testCode");
        m_secretInfo = new SecretInfo(isTest);
    }

    public static CheckBase create(AuthInfoReq req)
    {
        CheckBase obj = null;
        switch (req.m_opCode)
        {
            case ConstDef.AUTH_CHECK:
                {
                    obj = new CheckRealName();
                }
                break;
            case ConstDef.AUTH_QUEYR:
                {
                    obj = new CheckQuery();
                }
                break;
            case ConstDef.AUTH_UPDATA:
                {
                    obj = new CheckUpdata();
                }
                break;
            default:
                {
                    LogMgr.error("CheckBase.create not find type {0}", req.m_opCode);
                }
                break;
        }


        return obj;
    }

    public virtual void check(AuthInfoReq data) { }

    public static byte[] parseHexStr2Byte(string hexStr)
    {
        if (hexStr.Length < 1)
            return null;

        byte[] result = new byte[hexStr.Length / 2];
        for (int i = 0; i < hexStr.Length / 2; i++)
        {
            string subHigh = hexStr.Substring(i * 2, 1);
            int high = Convert.ToInt32(subHigh, 16);

            string subLow = hexStr.Substring(i * 2 + 1, 1);
            int low = Convert.ToInt32(subLow, 16);

            result[i] = (byte)(high * 16 + low);
        }
        return result;
    }

    protected static byte[] getNonce()
    {
       // string testsss = "ng7s/4p2XFsO7FBhp45q43OqYxG85zRADJ1J7bxAXdVd1uxu2So6h2w4yaM4DqgVM6QeGihN+Ks78Qn0PedDx0xdKYfnSfdxFOerLQ4R93pOnpUSdvPI2ej+pGB8zhNnNeuCurw=";
       // byte[] testarr = Convert.FromBase64String(testsss);

        byte[] nonce = new byte[12];
        //Array.Copy(testarr, 0, nonce, 0, nonce.Length);
        new Random().NextBytes(nonce);

        return nonce;
    }

    protected static string aesGcmEncrypt(string str, string secKey)
    {
        byte[] wait = Encoding.UTF8.GetBytes(str);
        var aesEngine = new Org.BouncyCastle.Crypto.Engines.AesEngine();
        //var fast = new Org.BouncyCastle.Crypto.Engines.AesFastEngine();
       // var padding = new Pkcs7Padding();
       
        GcmBlockCipher chiper = new GcmBlockCipher(aesEngine);
        //BufferedAeadBlockCipher pbb = new BufferedAeadBlockCipher(chiper); 

       // int sizee = chiper.GetBlockSize();

        byte[] key = CheckBase.parseHexStr2Byte(secKey); 
        KeyParameter kparam = new KeyParameter(key);
        byte[] nonce = getNonce();

       // byte[] associatedText = new byte[1];
        chiper.Init(true, new AeadParameters(kparam, 128, nonce, null));
        byte[] rv = new byte[chiper.GetOutputSize(wait.Length)];
        int oLen = chiper.ProcessBytes(wait, 0, wait.Length, rv, 0);
        try
        {
            chiper.DoFinal(rv, oLen);
        }
        catch (Exception ex)
        {
            return "";
        }

        MemoryStream ms = new MemoryStream(nonce.Length + rv.Length);
        ms.Write(nonce, 0, nonce.Length);
        ms.Write(rv, 0, rv.Length);
        byte[] dst = ms.ToArray();

        return Convert.ToBase64String(dst);
    }

    // 加密
    protected string encrypt(string body, string secKey)
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        string secdata = aesGcmEncrypt(body, secKey);
        result.Add("data", secdata);
        return JsonHelper.ConvertToStr(result);
    }

    protected Dictionary<string, object> genSysData()
    {
        Dictionary<string, object> sysData = new Dictionary<string, object>();
        sysData.Add("appId", m_secretInfo.AppId);
        sysData.Add("bizId", m_secretInfo.BizId);
        sysData.Add("timestamps", PayBase.getTSMill().ToString());

        return sysData;
    }

    // 取得待签名数据
    protected string getWaitSign(Dictionary<string, object> sysData, string body)
    {
        PayCheck ch = new PayCheck();
        string sd = ch.getWaitSignStrByAsc3(sysData, "");
        StringBuilder builder = new StringBuilder();
        builder.Append(m_secretInfo.SecKey);
        builder.Append(sd);
        builder.Append(body);
        string wait = builder.ToString();
        return wait;
    }

    // 签名
    protected string calSign(Dictionary<string, object> sysData, string body)
    {
        PayCheck ch = new PayCheck();
        string wait = getWaitSign(sysData, body);
        string sign = ch.toHex(ch.SHA256Hash(wait));
        return sign;
    }

    protected void addRep(AuthInfoRep rep)
    {
        WorkThreadConfirm sys = ServiceMgr.getInstance().getSys<WorkThreadConfirm>((int)SType.ServerType_Confirm);
        if (sys != null)
        {
            sys.addResult(rep);
        }
    }
}

/////////////////////////////////////////////////////
// 实名检测
public class CheckRealName : CheckBase
{
    void test()
    {
        PayCheck ch = new PayCheck();
        string str = "2836e95fcd10e04b0069bb1ee659955bappIdtest-appIdbizIdtest-bizIdidtest-idnametest-nametimestamps1584949895758{\"data\":\"CqT/33f3jyoiYqT8MtxEFk3x2rlfhmgzhxpHqWosSj4d3hq2EbrtVyx2aLj565ZQNTcPrcDipnvpq/D/vQDaLKW70O83Q42zvR0//OfnYLcIjTPMnqa+SOhsjQrSdu66ySSORCAo\"}";
        byte[] arr =ch.SHA256Hash(str);
        string h = ch.toHex(arr);
        int i = 0;
        i++;

        string ss = "aaa\"\"";
    }

    public override void check(AuthInfoReq data)
    {
        Dictionary<string, object> sysData = genSysData();
        Dictionary<string, object> busData = genBody(data);
        string reqBody = JsonHelper.ConvertToStr(busData);
        string reqBodySec = encrypt(reqBody, m_secretInfo.SecKey);
        string sign = calSign(sysData, reqBodySec);

        string checkURL = m_urlInfo.getCheckURL(m_testCode);

        string retstr = WxPayAPI.HttpService.PostData((webReq, sendData) =>
        {
            sendData.m_contentType = ContentType;
            sendData.m_str = reqBodySec;
            foreach (var item in sysData)
            {
                webReq.Headers.Add(item.Key, Convert.ToString(item.Value));
            }

            webReq.Headers.Add("sign", sign);

            if(CheckBase.m_isEnableLog)
            {
                LogMgr.info("CheckRealName start ------------------------------");
                LogMgr.info("请求体明文:{0}", reqBody);
                LogMgr.info("请求体密文:{0}", reqBodySec);
                string waitSign = getWaitSign(sysData, reqBodySec);
                LogMgr.info("待签名:{0}", waitSign);

                LogMgr.info("请求头 ------------------------begin");
                foreach (var item in sysData)
                {
                    LogMgr.info("{0}:{1}", item.Key, Convert.ToString(item.Value));
                }
                LogMgr.info("sign:{0}", sign);
                LogMgr.info("请求头 ------------------------end");

                LogMgr.info("CheckRealName end ------------------------------");
            }
        }, checkURL, false, 90);

        if (CheckBase.m_isEnableLog)
        {
            LogMgr.info("CheckRealName.check ret:{0}", retstr);
        }
            
        try
        {
            AuthInfoVerify oriData = (AuthInfoVerify)data;

            AuthInfoCheckRep checkReult = new AuthInfoCheckRep();
            checkReult.fromJsonStr(retstr);
            checkReult.m_playerId = oriData.m_id;

            AuthInfoRep rep = new AuthInfoRep();
            rep.m_opCode = ConstDef.AUTH_CHECK;
            rep.m_param = checkReult;
            addRep(rep);
        }
        catch (Exception ex)
        {
            LogMgr.error(ex.ToString());
        }
    }

    Dictionary<string, object> genBody(AuthInfoReq reqt)
    {
        AuthInfoVerify req = (AuthInfoVerify)reqt;
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("ai", req.m_id);
        data.Add("idNum", req.m_idNum);
        data.Add("name", req.m_name);
        return data;
    }
}

/////////////////////////////////////////////////////
// 实名查询，对于无法一次性无法完成检测的。
public class CheckQuery : CheckBase
{
    public override void check(AuthInfoReq data)
    {
        AuthInfoQuery oriData = (AuthInfoQuery)data;
        Dictionary<string, object> sysData = genSysData();
        sysData.Add("ai", oriData.m_playerId);
        string waitSign = getWaitSign(sysData, "");
        string sign = calSign(sysData, "");
        sysData.Remove("ai");

        string queryURL = m_urlInfo.getQueryURL(oriData.m_playerId, m_testCode);

        byte[] retarr = HttpPost.Get(new Uri(queryURL), (webReq) => {

            foreach (var item in sysData)
            {
                webReq.Headers.Add(item.Key, Convert.ToString(item.Value));
            }

            webReq.Headers.Add("sign", sign);

            if (CheckBase.m_isEnableLog)
            {
                LogMgr.info("CheckQuery start ------------------------------");
                //string waitSign = getWaitSign(sysData,"");
                LogMgr.info("待签名:{0}", waitSign);

                LogMgr.info("请求头 ------------------------begin");
                foreach (var item in sysData)
                {
                    LogMgr.info("{0}:{1}", item.Key, Convert.ToString(item.Value));
                }
                LogMgr.info("sign:{0}", sign);
                LogMgr.info("请求头 ------------------------end");

                LogMgr.info("CheckQuery end ------------------------------");
            }
        });

        string retstr = "";
        if (retarr != null)
        {
            retstr = Encoding.UTF8.GetString(retarr);
        }

        if (CheckBase.m_isEnableLog)
        {
            LogMgr.info("CheckQuery.check ret:{0}", retstr);
        }
            
        try
        {
            AuthInfoQueryRep tmp = new AuthInfoQueryRep();
            tmp.fromJsonStr(retstr);
            tmp.m_playerId = oriData.m_playerId;

            AuthInfoRep rep = new AuthInfoRep();
            rep.m_opCode = ConstDef.AUTH_QUEYR;
            rep.m_param = tmp;

            addRep(rep);
        }
        catch (Exception ex)
        {
            LogMgr.error(ex.ToString());
        }
    }
}

/////////////////////////////////////////////////////
// 上报数据
public class CheckUpdata : CheckBase
{
    // {"collections":[{"n":0,"si":"1111","bt":0,"ot":1234,"ct":0,"di":"1234","pi":"1234"}]}
    public override void check(AuthInfoReq data)
    {
        Dictionary<string, object> sysData = genSysData();
        string reqBody = genBody(data);
        string reqBodySec = encrypt(reqBody, m_secretInfo.SecKey);
        string sign = calSign(sysData, reqBodySec);
        string updataURL = m_urlInfo.getUpDataURL(m_testCode);

        string retstr = WxPayAPI.HttpService.PostData((webReq, sendData) =>
        {
            sendData.m_contentType = ContentType;
            sendData.m_str = reqBodySec;

            foreach (var item in sysData)
            {
                webReq.Headers.Add(item.Key, Convert.ToString(item.Value));
            }

            webReq.Headers.Add("sign", sign);

        }, updataURL, false, 90);

        LogMgr.info("CheckUpdata.check ret:{0}", retstr);

        try
        {
            AuthInfoUpdataRep tmp = new AuthInfoUpdataRep();
            tmp.fromJsonStr(retstr);

            AuthInfoRep rep = new AuthInfoRep();
            rep.m_opCode = ConstDef.AUTH_UPDATA;
            rep.m_param = tmp;
            addRep(rep);
        }
        catch (Exception ex)
        {
            LogMgr.error(ex.ToString());
        }
    }

    string genBody(AuthInfoReq reqt)
    {
        string body = "";
        AuthInfoUpdata req = (AuthInfoUpdata)reqt;

        Dictionary<string, object> dList = new Dictionary<string, object>();
        List<Dictionary<string, object>> L = new List<Dictionary<string, object>>();
        
        Dictionary<string, object> id = new Dictionary<string, object>();
        id.Add("no", 0);
        id.Add("si", req.m_session);
        id.Add("bt", req.m_action);
        id.Add("ot", PayBase.getTS());
        id.Add("ct", req.m_userType);
        id.Add("di", req.m_deviceId);
        id.Add("pi", req.m_pi);
        L.Add(id);

        dList.Add("collections", L);
        body = JsonHelper.genJson(dList);

        return body;
    }
}

