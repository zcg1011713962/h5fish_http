using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Common;
using System.Collections.Specialized;
using System.IO;
using System.Xml;
using NS_OpenApiV3;
using NS_SnsNetWork;
using NS_SnsSigCheck;
using NS_SnsStat;
using WxPayAPI;
using ThoughtWorks;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using System.Drawing;
using System.Drawing.Imaging;

public class CVIVOGetOrderNum
{
    public static List<string> s_excludeKey = new List<string>(new string[] { "signMethod", "signature" });

    // vivo的订单推送接口
    const string URL_GET_ORDER = "https://pay.vivo.com.cn/vivoPay/getVivoOrderNum?{0}";

    public string PayNotifyURL { set; get; }

    public string VivoCpId { set; get; }

    public string VivoAppId { set; get; }

    public string VivoCpKey { set; get; }

    public string SelfOrderId { set; get; }

    public string Price { set; get; }

    public string ProductTitle { set; get; }

    public string ProductDesc { set; get; }

    protected string Version { set; get; }

    public static CVIVOGetOrderNum createVivoOrder(HttpRequest Request)
    {
        string version = Request.QueryString["ver"];

        if (string.IsNullOrEmpty(version))
        {
            version = "1";
        }

        CVIVOGetOrderNum obj = null;
        switch (version)
        {
            case "1":
                {
                    obj = new CVIVOGetOrderNum();
                }
                break;
            case "2":
                {
                    obj = new CVIVOGetOrderNumV2();
                }
                break;
            default:
                {
                    obj = new CVIVOGetOrderNum();
                }
                break;
        }
        obj.Version = version;
        return obj;
    }

    public string getOrderNum()
    {
        if (string.IsNullOrEmpty(SelfOrderId) ||
           string.IsNullOrEmpty(Price) ||
           string.IsNullOrEmpty(ProductTitle) ||
           string.IsNullOrEmpty(ProductDesc))
        {
            VivoOrderNumInfo info = new VivoOrderNumInfo();
            info.respCode = 400;
            return retMsg(info);
        }

        try
        {
            string param = genSendParam(SelfOrderId, Price, ProductTitle, ProductDesc);
            string notifyURL = getNotifyURL();
            string url = string.Format(notifyURL, param);
            byte[] retStr = HttpPost.Post(new Uri(url));
            string str = Encoding.UTF8.GetString(retStr);

            return getMessage(str);
            //VivoOrderNumInfo info = JsonHelper.ParseFromStr<VivoOrderNumInfo>(str);
            //return retMsg(info);
        }
        catch (System.Exception ex)
        {
            CLOG.Info(ex.ToString());
            VivoOrderNumInfo info = new VivoOrderNumInfo();
            info.respCode = 501;
            return retMsg(info);
        }
    }

    public virtual string genSendParam(string selfOrderId, string price, string productTitle, string productDesc)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();

        dict.Add("version", "1.0.0");
        dict.Add("signMethod", "MD5");

        dict.Add("storeId", VivoCpId);
        dict.Add("appId", VivoAppId);
        dict.Add("storeOrder", selfOrderId);
        dict.Add("notifyUrl", PayNotifyURL);
        dict.Add("orderTime", DateTime.Now.ToString("yyyyMMddHHmmss"));
        dict.Add("orderAmount", price);
        dict.Add("orderTitle", productTitle);
        dict.Add("orderDesc", productDesc);

        PayCheck check = new PayCheck();
        string waitSign = check.getVivoWaitSigned(dict, s_excludeKey, VivoCpKey);
        string sign = Helper.getMD5(waitSign);
        dict.Add("signature", sign);

        dict["notifyUrl"] = HttpUtility.UrlEncode(PayNotifyURL);
        string result = check.getWaitSignStrByAsc(dict);

        return result;
    }

    public virtual string getMessage(string str)
    {
        VivoOrderNumInfo info = JsonHelper.ParseFromStr<VivoOrderNumInfo>(str);
        return retMsg(info);
    }

    public virtual string getNotifyURL()
    {
        return URL_GET_ORDER;
    }

    string retMsg(VivoOrderNumInfo info)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("ret = {};");
        sb.AppendFormat("ret.result = {0};", info.respCode);
        sb.AppendFormat("ret.vivoOrder = \"{0}\";", info.vivoOrder);
        sb.AppendFormat("ret.vivoSignature = \"{0}\";", info.vivoSignature);
        sb.Append("return ret;");

        return sb.ToString();
    }

    class VivoOrderNumInfo
    {
        public int respCode = 0;
        public string respMsg = "";
        public string signMethod = "";
        public string signature = "";
        public string vivoSignature = "";
        public string vivoOrder = "";
        public string orderAmount = "";
    }
}

//////////////////////////////////////////////////////////////////////////
// vivo得到订单接口升级， 版本2
public class CVIVOGetOrderNumV2 : CVIVOGetOrderNum
{
    const string URL_GET_ORDER1 = "https://pay.vivo.com.cn/vcoin/trade?{0}";

    public override string genSendParam(string selfOrderId, string price, string productTitle, string productDesc)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();

        dict.Add("version", "1.0.0");
        dict.Add("signMethod", "MD5");

        dict.Add("cpId", VivoCpId);
        dict.Add("appId", VivoAppId);
        dict.Add("cpOrderNumber", selfOrderId);
        dict.Add("notifyUrl", PayNotifyURL);
        dict.Add("orderTime", DateTime.Now.ToString("yyyyMMddHHmmss"));
        dict.Add("orderAmount", 100 * (int)Convert.ToDouble(price));
        dict.Add("orderTitle", productTitle);
        dict.Add("orderDesc", productDesc);
        dict.Add("extInfo", Version);

        PayCheck check = new PayCheck();
        string waitSign = check.getVivoWaitSigned(dict, s_excludeKey, VivoCpKey);
        string sign = Helper.getMD5(waitSign);
        dict.Add("signature", sign);

        dict["notifyUrl"] = HttpUtility.UrlEncode(PayNotifyURL);
        string result = check.getWaitSignStrByAsc(dict);

        return result;
    }

    public override string getMessage(string str)
    {
        VivoOrderNumInfo1 info = JsonHelper.ParseFromStr<VivoOrderNumInfo1>(str);
        return retMsg1(info);
    }

    public override string getNotifyURL()
    {
        return URL_GET_ORDER1;
    }

    string retMsg1(VivoOrderNumInfo1 info)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("ret = {};");
        sb.AppendFormat("ret.result = {0};", info.respCode);
        sb.AppendFormat("ret.vivoOrder = \"{0}\";", info.orderNumber);
        sb.AppendFormat("ret.vivoSignature = \"{0}\";", info.accessKey);
        sb.Append("return ret;");

        return sb.ToString();
    }

    class VivoOrderNumInfo1
    {
        public int respCode = 0;
        public string respMsg = "";
        public string signMethod = "";
        public string signature = "";
        public string accessKey = "";
        public string orderNumber = "";
        public string orderAmount = "";
    }
}

//////////////////////////////////////////////////////////////////////////
public class AiBeiTransParam
{
    public int m_playerId;
    public int m_payId;
    public string m_productName;
    public string m_orderId;
    public float m_price;
}

public class AiBeiTrans
{
    const string AIBEI_ORDER_URL = "http://ipay.iapppay.com:9999/payapi/order";

    public string NotifyURL { set; get; }

    public string NotifyFailURL { set; get; }

    public string PrivateKey { set; get; }

    public string AppId { set; get; }

    // 返回爱贝的支付网关
    public virtual string transRL(AiBeiTransParam param)
    {
        string resultURL = "";
        do
        {
            try
            {
                string transid = reqTransid(param);
                if (string.IsNullOrEmpty(transid))
                {
                    CLOG.Info("aibei, 获取交易流水号失败");
                    break;
                }

                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("tid", transid);
                data.Add("app", AppId);
                data.Add("url_r", NotifyURL);
                data.Add("url_h", NotifyFailURL);
                string dataStr = JsonHelper.genJson(data);

                string strReqSign = reqData(dataStr, 1);
                resultURL = "https://web.iapppay.com/h5/gateway?" + strReqSign;
            }
            catch (System.Exception ex)
            {
                CLOG.Info("AiBeiTrans: " + ex.ToString());
            }
        } while (false);

        return resultURL;
    }

    protected string reqData(string content, int flag = 0)
    {
        PayCheck chk = new PayCheck();
        string sign = chk.signByRSAMd5(content, PrivateKey);

        if (flag == 0)
        {
            return "transdata=" + HttpUtility.UrlEncode(content) + "&sign=" + HttpUtility.UrlEncode(sign) + "&signtype=RSA";
        }
        else
        {
            return "data=" + HttpUtility.UrlEncode(content) + "&sign=" + HttpUtility.UrlEncode(sign) + "&sign_type=RSA";
        }
    }

    // 获取交易流水号 
    protected string reqTransid(AiBeiTransParam param)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("appid", AppId);
        data.Add("waresid", param.m_payId);
        data.Add("cporderid", param.m_orderId);
        data.Add("currency", "RMB");
        data.Add("waresname", param.m_productName);
        data.Add("price", param.m_price);
        data.Add("appuserid", param.m_playerId.ToString());
        data.Add("notifyurl", NotifyURL);

        string content = JsonHelper.genJson(data);
        string strReqData = reqData(content, 0);
        string result = HttpPost.Post3(AIBEI_ORDER_URL, strReqData);
        string respData = HttpUtility.UrlDecode(result);
        var dataList = explainDataList(respData);
        if (dataList["res"] == "fail")
        {
            CLOG.Info("aibei,reqtransid: " + respData);
            return "";
        }

        string transid = dataList["transid"];
        return transid;
    }

    //分解下单结果transdata
    protected Dictionary<string, string> explainDataList(string respData)
    {
        //成功示例：transdata={"transid":"11111"}&sign=xxxxxx&signtype=RSA
        //失败示例：transdata={"code":"1001","errmsg":"签名验证失败"}
        Dictionary<string, string> dList = new Dictionary<string, string>();
        //开始分割参数
        string transdata = "";
        string sign = "";
        string signtype = "";
        string[] dataArray = respData.Replace("\"", "").Split('&');

        foreach (string s in dataArray)
        {
            if (s.StartsWith("transdata"))
            {
                transdata = s.Substring(s.IndexOf("=") + 1);
            }
            else if (s.StartsWith("sign="))
            {
                sign = s.Substring(s.IndexOf("=") + 1);
            }
            else if (s.StartsWith("signtype"))
            {
                signtype = s.Substring(s.IndexOf("=") + 1);
            }
        }

        var resData = transdata.Replace("{", "").Replace("}", "");
        if (dataArray.Length == 1) //下单失败
        {
            var data = resData.Split(',')[1];
            var msg = data.Split(':')[1];
            dList.Add("res", "fail");
            dList.Add("msg", msg);
        }
        else //下单成功
        {
            var data = resData.Split(':')[1];
            dList.Add("res", "success");
            dList.Add("transid", data);
        }
        return dList;
    }
}

public class AiBeiTransSwitch : AiBeiTrans
{
    public override string transRL(AiBeiTransParam param)
    {
        string result = "";
        do
        {
            try
            {
                string transid = reqTransid(param);
                if (string.IsNullOrEmpty(transid))
                {
                    CLOG.Info("AiBeiTransSwitch, 获取交易流水号失败");
                    break;
                }

                // result = string.Format("transid={0}&appid={1}&rmb={2}", transid, AppId, Convert.ToInt32(param.m_price));
                result = string.Format("transid={0}&appid={1}", transid, AppId);
            }
            catch (System.Exception ex)
            {
                CLOG.Info("AiBeiTransSwitch: " + ex.ToString());
            }
        } while (false);

        return result;
    }
}

//////////////////////////////////////////////////////////////////////////
// 魅族(联网)游戏，订单获取签名
public partial class MeizuOnlineTrans
{
    static List<string> s_excludeKey = new List<string>(new string[] { "sign", "sign_type" });

    public string AppId { set; get; }

    public string AppSecret { set; get; }

    public string trans(object param)
    {
        HttpRequest Request = (HttpRequest)param;
        Dictionary<string, object> ret = new Dictionary<string, object>();
        ret["_result"] = "error";

        do
        {
            try
            {
                string totalPrice = Request.QueryString["totalPrice"];
                string selfOrderId = Request.QueryString["selfOrderId"];
                string productId = Request.QueryString["paycode"];
                string productSubject = Request.QueryString["productname"];
                string uid = Request.QueryString["uid"];
                string productBody = productSubject;

                if (string.IsNullOrEmpty(totalPrice) ||
                    string.IsNullOrEmpty(selfOrderId) ||
                    string.IsNullOrEmpty(productId) ||
                    string.IsNullOrEmpty(productSubject))
                {
                    CLOG.Info("MeizuTrans, param error, {0}, {1}, {2}, {3} ", totalPrice, selfOrderId, productId, productSubject);
                    break;
                }

                TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
                long time = Convert.ToInt64(ts.TotalMilliseconds);

                Dictionary<string, object> indata = new Dictionary<string, object>();
                indata.Add("app_id", AppId);
                indata.Add("cp_order_id", selfOrderId);

                indata.Add("uid", uid);

                indata.Add("product_id", productId);
                indata.Add("product_subject", productSubject);
                indata.Add("product_body", productBody);

                indata.Add("product_unit", "ge");
                indata.Add("buy_amount", 1);

                indata.Add("product_per_price", totalPrice);
                indata.Add("total_price", totalPrice);

                indata.Add("create_time", time);

                indata.Add("pay_type", 0);

                indata.Add("user_info", "test");

                PayCheck chk = new PayCheck();
                string wait = chk.getMeizuSingleWaitSignStr(indata, s_excludeKey, AppSecret);
                string sign = Helper.getMD5(wait);

                ret["_result"] = "ok";
                ret["sign"] = sign;
                ret["createTime"] = time;
            }
            catch (System.Exception ex)
            {
                CLOG.Info("MeizuTrans, " + ex.ToString());
            }
        } while (false);

        string str = LoginCommon.genLuaRetString(ret);
        return str;
    }
}

//////////////////////////////////////////////////////////////////////////
public class CRechargeEx
{
    private Dictionary<int, int> m_dic = new Dictionary<int, int>();

    public int getPayCode(int rechargeId)
    {
        if (m_dic.ContainsKey(rechargeId))
        {
            return m_dic[rechargeId];
        }
        return -1;
    }

    public void init(string channel)
    {
        string path = HttpRuntime.BinDirectory + "..\\download";
        string file = Path.Combine(path, "M_RechangeExCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            string sid = node.Attributes["ID"].Value;
            if (channel == "800094")
            {
                string ids = node.Attributes["RechargeIDs"].Value;
                string paycodes = node.Attributes["PayCodes"].Value;

                string[] aids = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] apaycodes = paycodes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < aids.Length; i++)
                {
                    int key = Convert.ToInt32(aids[i]);
                    if (!m_dic.ContainsKey(key))
                    {
                        m_dic.Add(key, Convert.ToInt32(apaycodes[i]));
                    }
                }
                break;
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 微信小程序获取id
public partial class WeiXinMiniGetId
{
    // 回调通知URL
    public string PayNotifyURL { set; get; }

    public string AppId { set; get; }

    // 商户号
    public string MchId { set; get; }

    public string Body { set; get; }

    public string SelfOrderId { set; get; }

    // 以分为单位
    public string TotalFee { set; get; }

    public string ApiSecret { set; get; }

    // 微信统一下单API接口
    public const string WINXIN_UNIFY_API = "https://api.mch.weixin.qq.com/pay/unifiedorder";

    public const string RAND_STR = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public string trans()
    {
        Dictionary<string, object> ret = new Dictionary<string, object>();
        do
        {
            if (string.IsNullOrEmpty(Body) || string.IsNullOrEmpty(SelfOrderId) || string.IsNullOrEmpty(TotalFee))
            {
                ret.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
                break;
            }

            try
            {
                string msgXML = genTranslateXMLString(Body, SelfOrderId, TotalFee);
                byte[] retByte = HttpPost.PostXML(new Uri(WINXIN_UNIFY_API), msgXML);

                XmlGen xml = new XmlGen();
                Dictionary<string, object> retObj = xml.fromXmlString("xml", Encoding.UTF8.GetString(retByte));
                if (Convert.ToString(retObj["return_code"]) != "SUCCESS")
                {
                    break;
                }

                ret = genReSignData(retObj);
                ret.Add(CC.KEY_CODE, CC.SUCCESS);
            }
            catch (System.Exception ex)
            {
                CLOG.Info("WeiXinGetId:" + ex.ToString());
            }

        } while (false);

        return JsonHelper.genJson(ret);
    }

    Dictionary<string, object> genTranData(string body, string selfOrderId, string totalFee)
    {
        Dictionary<string, object> ret = new Dictionary<string, object>();
        ret.Add("appid", AppId);
        ret.Add("mch_id", MchId);
        StringBuilder builder = new StringBuilder();
        Random r = new Random();
        for (int i = 0; i < 16; i++)
        {
            builder.Append(RAND_STR[r.Next(RAND_STR.Length)]);
        }

        ret.Add("nonce_str", builder.ToString());//随机字符串
        ret.Add("body", body);
        ret.Add("out_trade_no", selfOrderId);
        ret.Add("total_fee", totalFee);
        ret.Add("spbill_create_ip", Helper.GetWebClientIp());
        ret.Add("notify_url", PayNotifyURL);
        ret.Add("trade_type", "JSAPI");
        return ret;
    }

    public string genTranslateXMLString(string body, string selfOrderId, string totalFee)
    {
        PayCheck chk = new PayCheck();
        Dictionary<string, object> transData = genTranData(body, selfOrderId, totalFee);
        string waitSign = chk.getWeiXinWaitSigned(transData, null, ApiSecret);
        string sign = Helper.getMD5Upper(waitSign);
        transData.Add("sign", sign);

        XmlGen xml = new XmlGen();
        string str = xml.genXML("xml", transData);
        return str;
    }

    Dictionary<string, object> genReSignData(Dictionary<string, object> retObj)
    {
        Dictionary<string, object> ret = new Dictionary<string, object>();
        ret.Add("appid", Convert.ToString(retObj["appid"]));
        ret.Add("partnerid", Convert.ToString(retObj["mch_id"]));
        //ret.Add("prepayid", Convert.ToString(retObj["prepay_id"]));
        ret.Add("package", Convert.ToString(retObj["prepay_id"]));
        ret.Add("noncestr", Convert.ToString(retObj["nonce_str"]));
        TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
        ret.Add("timestamp", Convert.ToInt64(ts.TotalSeconds).ToString());
        ret.Add("signType", "MD5");

        PayCheck chk = new PayCheck();
        string waitSign = chk.getWeiXinWaitSigned(ret, null, ApiSecret);
        string sign = Helper.getMD5Upper(waitSign);
        ret.Add("paySign", sign);

        return ret;
    }
}


//////////////////////////////////////////////////////////////////////////
// 微信小程序米大师获取id
public class WeiXinMiniMidasGetId
{
    // 回调通知URL
    public string PayNotifyURL { set; get; }

    public string AppId { set; get; }

    // 商户号
    public string MchId { set; get; }

    public string Body { set; get; }

    public string SelfOrderId { set; get; }

    // 以分为单位
    public string TotalFee { set; get; }

    public string ApiSecret { set; get; }

    // 微信统一下单API接口
    public const string WINXIN_UNIFY_API = "https://api.mch.weixin.qq.com/pay/unifiedorder";

    public const string RAND_STR = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public string trans()
    {
        Dictionary<string, object> ret = new Dictionary<string, object>();
        do
        {
            if (string.IsNullOrEmpty(Body) || string.IsNullOrEmpty(SelfOrderId) || string.IsNullOrEmpty(TotalFee))
            {
                ret.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
                break;
            }

            try
            {
                string msgXML = genTranslateXMLString(Body, SelfOrderId, TotalFee);
                byte[] retByte = HttpPost.PostXML(new Uri(WINXIN_UNIFY_API), msgXML);

                XmlGen xml = new XmlGen();
                Dictionary<string, object> retObj = xml.fromXmlString("xml", Encoding.UTF8.GetString(retByte));
                if (Convert.ToString(retObj["return_code"]) != "SUCCESS")
                {
                    break;
                }

                ret = genReSignData(retObj);
                ret.Add(CC.KEY_CODE, CC.SUCCESS);
            }
            catch (System.Exception ex)
            {
                CLOG.Info("WeiXinGetId:" + ex.ToString());
            }

        } while (false);

        return JsonHelper.genJson(ret);
    }

    Dictionary<string, object> genTranData(string body, string selfOrderId, string totalFee)
    {
        Dictionary<string, object> ret = new Dictionary<string, object>();
        ret.Add("appid", AppId);
        ret.Add("mch_id", MchId);
        StringBuilder builder = new StringBuilder();
        Random r = new Random();
        for (int i = 0; i < 16; i++)
        {
            builder.Append(RAND_STR[r.Next(RAND_STR.Length)]);
        }

        ret.Add("nonce_str", builder.ToString());//随机字符串
        ret.Add("body", body);
        ret.Add("out_trade_no", selfOrderId);
        ret.Add("total_fee", totalFee);
        ret.Add("spbill_create_ip", Helper.GetWebClientIp());
        ret.Add("notify_url", PayNotifyURL);
        ret.Add("trade_type", "JSAPI");
        return ret;
    }

    public string genTranslateXMLString(string body, string selfOrderId, string totalFee)
    {
        PayCheck chk = new PayCheck();
        Dictionary<string, object> transData = genTranData(body, selfOrderId, totalFee);
        string waitSign = chk.getWeiXinWaitSigned(transData, null, ApiSecret);
        string sign = Helper.getMD5Upper(waitSign);
        transData.Add("sign", sign);

        XmlGen xml = new XmlGen();
        string str = xml.genXML("xml", transData);
        return str;
    }

    Dictionary<string, object> genReSignData(Dictionary<string, object> retObj)
    {
        Dictionary<string, object> ret = new Dictionary<string, object>();
        ret.Add("appid", Convert.ToString(retObj["appid"]));
        ret.Add("partnerid", Convert.ToString(retObj["mch_id"]));
        //ret.Add("prepayid", Convert.ToString(retObj["prepay_id"]));
        ret.Add("package", Convert.ToString(retObj["prepay_id"]));
        ret.Add("noncestr", Convert.ToString(retObj["nonce_str"]));
        TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
        ret.Add("timestamp", Convert.ToInt64(ts.TotalSeconds).ToString());
        ret.Add("signType", "MD5");

        PayCheck chk = new PayCheck();
        string waitSign = chk.getWeiXinWaitSigned(ret, null, ApiSecret);
        string sign = Helper.getMD5Upper(waitSign);
        ret.Add("paySign", sign);

        return ret;
    }
}

public class MidasTokenInfo
{
    public DateTime m_endTime;
    public string m_token;
}

//////////////////////////////////////////////////////////////////////////
// 米大师的token管理
public class WeiXinMiniMidasTokenMgr
{
    public static string URL = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

    private Dictionary<string, MidasTokenInfo> m_data = new Dictionary<string, MidasTokenInfo>();
    static object s_object = new object();

    public string getToken(string appId, string appSecret)
    {
        MidasTokenInfo info = null;
        lock (s_object)
        {
            try
            {
                if (m_data.ContainsKey(appId))
                {
                    info = m_data[appId];
                    if (DateTime.Now > info.m_endTime.AddMinutes(-10))
                    {
                        info = refreshToken(appId, appSecret);
                    }
                }
                else
                {
                    info = refreshToken(appId, appSecret);
                }
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }
        }

        if (info == null)
            return "";

        return info.m_token;
    }

    // 刷新token
    public MidasTokenInfo refreshToken(string appId, string appSecret)
    {
        MidasTokenInfo info = null;
        try
        {
            string fmt = string.Format(URL, appId, appSecret);
            byte[] ret = HttpPost.Get(new Uri(fmt));
            string retstr = Encoding.UTF8.GetString(ret);
            CLOG.Info(retstr);
            Dictionary<string, object> data = JsonHelper.ParseFromStr<Dictionary<string, object>>(retstr);
            if (data.ContainsKey("errcode") && Convert.ToInt32(data["errcode"]) != 0)
            {
                CLOG.Info("WeiXinMiniMidasTokenMgr,refreshToken, token error, {0}", Convert.ToInt32(data["errcode"]));
                return info;
            }

            info = new MidasTokenInfo();
            info.m_token = Convert.ToString(data["access_token"]);
            info.m_endTime = DateTime.Now.AddSeconds(Convert.ToInt32(data["expires_in"]));
            m_data[appId] = info;
        }
        catch (System.Exception ex)
        {
            CLOG.Info(ex.ToString());
        }

        return info;
    }
}

////////////////////////////////////////////////////////////////////////
public class MidasUriInfo
{
    public string m_uri;
    public string m_fullURL;

    public MidasUriInfo(string uri)
    {
        m_uri = uri;
        m_fullURL = "https://api.weixin.qq.com" + uri + "?access_token={0}";
    }
}

public class WeiXinMiniMidasRecharge
{
    // 0正式，1测试
    public int IsTest { set; get; }

    public string OpenId { set; get; }
    public string AppId { set; get; }
    public string OfferId { set; get; }

    // 由登录返回的key,由客户端保存，提交上来
    public string SessionKey { set; get; }

    public string AppSecret { set; get; }

    // 米大师秘钥
    public string MidasSecret
    {
        set;
        get;
    }

    // 扣除游戏币个数, 这个计算出来
    public int Amt { set; get; }

    // 充值金额
    public string Rmb { set; get; }

    public string SelfOrderId { set; get; }

    public string PayTableName { set; get; }

    static MidasUriInfo[] s_balance =
    {
        new MidasUriInfo("/cgi-bin/midas/getbalance"),
        new MidasUriInfo("/cgi-bin/midas/sandbox/getbalance"),
    };

    static MidasUriInfo[] s_pay =
    {
        new MidasUriInfo("/cgi-bin/midas/pay"),
        new MidasUriInfo("/cgi-bin/midas/sandbox/pay"),
    };

    // 1人民币转成多少虚拟币
    public const int RMB_TO_VIRTUAL_MONEY = 1;

    long getTS()
    {
        TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        return Convert.ToInt64(ts.TotalSeconds);
    }

    // 获取余额
    public int getBalance(string token)
    {
        string fmt = string.Format(s_balance[IsTest].m_fullURL, token);

        int balanceResult = 0;

        do
        {
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("openid", OpenId);
                data.Add("appid", AppId);
                data.Add("offer_id", OfferId);
                data.Add("ts", getTS());
                data.Add("zone_id", "1");
                data.Add("pf", "android");

                PayCheck check = new PayCheck();
                string waitSign1 = check.getWaitSignStrByAsc(data, null, null);
                string waitSign2 = waitSign1 + "&org_loc=" + s_balance[IsTest].m_uri + "&method=POST&secret=" + MidasSecret;
                string sig = check.signByHMACSHA256(waitSign2, MidasSecret);
                data.Add("sig", sig);

                data.Add("access_token", token);
                string waitSign3 = check.getWaitSignStrByAsc(data, null, null);
                string waitSign4 = waitSign3 + "&org_loc=" + s_balance[IsTest].m_uri + "&method=POST&session_key=" + SessionKey;
                string mpSign = check.signByHMACSHA256(waitSign4, SessionKey);
                data.Add("mp_sig", mpSign);

                string json = JsonHelper.ConvertToStr(data);
                byte[] retbyte = HttpPost.PostJson(new Uri(fmt), json);
                string retstr = Encoding.UTF8.GetString(retbyte);
                Dictionary<string, object> retData = JsonHelper.ParseFromStr<Dictionary<string, object>>(retstr);
                if (retData.ContainsKey("errcode") && Convert.ToInt32(retData["errcode"]) != 0)
                {
                    CLOG.Info("WeiXinMiniMidasRecharge.getBalance, error code = {0}", Convert.ToInt32(retData["errcode"]));
                    break;
                }

                balanceResult = Convert.ToInt32(retData["balance"]);
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }
        } while (false);

        return balanceResult;
    }

    public string doRecharge()
    {
        string token = HttpLogin.Global.getTokenMgr().getToken(AppId, AppSecret);
        Dictionary<string, object> resultData = new Dictionary<string, object>();

        CLOG.Info("WeiXinMiniMidasRecharge, input, SessionKey={0}, SelfOrderId={1}, OpenId={2}", SessionKey, SelfOrderId, OpenId);
        do
        {
            if (!checkValid(resultData, token))
            {
                break;
            }

            try
            {
                Dictionary<string, object> reqData = new Dictionary<string, object>();
                reqData.Add("openid", OpenId);
                reqData.Add("appid", AppId);
                reqData.Add("offer_id", OfferId);
                reqData.Add("ts", getTS());
                reqData.Add("zone_id", "1");
                reqData.Add("pf", "android");
                reqData.Add("amt", Amt);
                reqData.Add("bill_no", SelfOrderId);

                PayCheck check = new PayCheck();
                string waitSign1 = check.getWaitSignStrByAsc(reqData, null, null);
                string waitSign2 = waitSign1 + "&org_loc=" + s_pay[IsTest].m_uri + "&method=POST&secret=" + MidasSecret;
                string sig = check.signByHMACSHA256(waitSign2, MidasSecret);
                reqData.Add("sig", sig);

                reqData.Add("access_token", token);
                string waitSign3 = check.getWaitSignStrByAsc(reqData, null, null);
                string waitSign4 = waitSign3 + "&org_loc=" + s_pay[IsTest].m_uri + "&method=POST&session_key=" + SessionKey;
                string mp_sig = check.signByHMACSHA256(waitSign4, SessionKey);
                reqData.Add("mp_sig", mp_sig);

                string fmt = string.Format(s_pay[IsTest].m_fullURL, token);
                string json = JsonHelper.ConvertToStr(reqData);
                byte[] retbyte = HttpPost.PostJson(new Uri(fmt), json);
                string retstr = Encoding.UTF8.GetString(retbyte);
                Dictionary<string, object> retData = JsonHelper.ParseFromStr<Dictionary<string, object>>(retstr);
                if (retData.ContainsKey("errcode") && Convert.ToInt32(retData["errcode"]) != 0)
                {
                    CLOG.Info("WeiXinMiniMidasRecharge.doRecharge, error={0}", Convert.ToInt32(retData["errcode"]));
                    resultData.Add(CC.KEY_CODE, CC.ERR_PAY_FAILED);
                    break;
                }

                if (addToTable())
                {
                    resultData.Add(CC.KEY_CODE, CC.SUCCESS);
                    break;
                }

                resultData.Add(CC.KEY_CODE, CC.ERR_DB);
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }
        } while (false);

        return JsonHelper.ConvertToStr(resultData);
    }

    protected bool checkValid(Dictionary<string, object> resultData, string token)
    {
        if (string.IsNullOrEmpty(OpenId))
        {
            resultData.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
            CLOG.Info("WeiXinMiniMidasRecharge,checkValid, OpenId param error");
            return false;
        }
        if (string.IsNullOrEmpty(Rmb))
        {
            resultData.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
            CLOG.Info("WeiXinMiniMidasRecharge,checkValid, rmb param error");
            return false;
        }

        Amt = Convert.ToInt32(Rmb) * RMB_TO_VIRTUAL_MONEY;
        int balance = getBalance(token);
        if (balance < Amt)
        {
            resultData.Add(CC.KEY_CODE, CC.ERR_BALANCE_NOT_ENOUGH);
            CLOG.Info("WeiXinMiniMidasRecharge,checkValid, balance not enough");
            return false;
        }

        if (string.IsNullOrEmpty(SelfOrderId))
        {
            resultData.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
            CLOG.Info("WeiXinMiniMidasRecharge,checkValid, selforderid param error");
            return false;
        }
        if (string.IsNullOrEmpty(SessionKey))
        {
            resultData.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
            CLOG.Info("WeiXinMiniMidasRecharge,checkValid, sessionkey param error");
            return false;
        }

        return true;
    }

    protected bool addToTable()
    {
        Dictionary<string, object> qd = PayBase.queryBaseData(SelfOrderId, MongodbPayment.Instance);
        if (qd == null)
        {
            CLOG.Info("WeiXinMiniMidasRecharge.addToTable, not tind orderid={0}", SelfOrderId);
            return false;
        }

        int srcRMB = Convert.ToInt32(qd["RMB"]);
        if (Convert.ToInt32(Rmb) != srcRMB)
        {
            CLOG.Info("WeiXinMiniMidasRecharge.addToTable, rmb not match, src {0}, upload {1}, openid={2}",
                srcRMB, Convert.ToInt32(Rmb), OpenId);
            return false;
        }

        PayInfoBase baseData = new PayInfoBase();
        baseData.m_payTime = DateTime.Now;
        baseData.m_payCode = Convert.ToString(qd["PayCode"]);
        baseData.m_playerId = Convert.ToInt32(qd["PlayerId"]);
        baseData.m_account = Convert.ToString(qd["Account"]);
        baseData.m_rmb = Convert.ToInt32(Rmb);
        baseData.m_channelNumber = Convert.ToString(qd["channel_number"]);
        baseData.m_orderId = SelfOrderId;

        Dictionary<string, object> d = new Dictionary<string, object>();
        baseData.addBaseDataToDic(d);

        Dictionary<string, object> savelog = new Dictionary<string, object>();
        savelog["acc"] = baseData.m_account;
        savelog["real_acc"] = baseData.m_account;
        savelog["time"] = DateTime.Now;
        savelog["channel"] = baseData.m_channelNumber;
        savelog["rmb"] = baseData.m_rmb;
        MongodbPayment.Instance.ExecuteInsert("PayLog", savelog);

        return LoginPayBase.insertPayData(PayTableName, d, baseData);
    }
}

public class LoginPayBase
{
    public static bool insertPayData(string tableName, Dictionary<string, object> allData, PayInfoBase baseData)
    {
        bool res = false;
        if (!MongodbPayment.Instance.KeyExistsBykey(tableName, "OrderID", allData["OrderID"]))
        {
            if (MongodbPayment.Instance.ExecuteInsert(tableName, allData))
            {
                // 渠道统计log
                /*Dictionary<string, object> savelog = new Dictionary<string, object>();
                savelog["acc"] = allData["Account"];
                if (allData.ContainsKey("real_Account"))
                {
                    savelog["real_acc"] = allData["real_Account"];
                }
                else
                {
                    savelog["real_acc"] = allData["Account"];
                }
                //记录设备号
                if (allData.ContainsKey("acc_dev"))
                {
                    savelog["acc_dev"] = allData["acc_dev"];
                }

                savelog["time"] = DateTime.Now;
                savelog["channel"] = allData["channel_number"];
                savelog["rmb"] = allData["RMB"];
                MongodbPayment.Instance.ExecuteInsert("PayLog", savelog);*/
                res = true;
            }
        }
        else
        {
            res = true; // 订单已经存在了
        }
        if (baseData != null)
        {
            PayBase.updateDataToPaymentTotal(baseData, MongodbPayment.Instance);
        }
        return res;
    }
}

//////////////////////////////////////////////////////////////////////////
// 米大师的token管理
public class QQMiniMidasTokenMgr
{
    public static string URL = "https://api.q.qq.com/api/getToken?grant_type=client_credential&appid={0}&secret={1}";

    private Dictionary<string, MidasTokenInfo> m_data = new Dictionary<string, MidasTokenInfo>();
    static object s_object = new object();

    public string getToken(string appId, string appSecret)
    {
        MidasTokenInfo info = null;
        lock (s_object)
        {
            try
            {
                if (m_data.ContainsKey(appId))
                {
                    info = m_data[appId];
                    if (DateTime.Now > info.m_endTime.AddMinutes(-10))
                    {
                        info = refreshToken(appId, appSecret);
                    }
                }
                else
                {
                    info = refreshToken(appId, appSecret);
                }
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }
        }

        if (info == null)
            return "";

        return info.m_token;
    }

    // 刷新token
    public MidasTokenInfo refreshToken(string appId, string appSecret)
    {
        MidasTokenInfo info = null;
        try
        {
            string fmt = string.Format(URL, appId, appSecret);
            byte[] ret = HttpPost.Get(new Uri(fmt));
            string retstr = Encoding.UTF8.GetString(ret);
            CLOG.Info(retstr);
            Dictionary<string, object> data = JsonHelper.ParseFromStr<Dictionary<string, object>>(retstr);
            if (data.ContainsKey("errcode") && Convert.ToInt32(data["errcode"]) != 0)
            {
                CLOG.Info("QQMiniMidasTokenMgr,refreshToken, token error, {0}", Convert.ToInt32(data["errcode"]));
                return info;
            }

            info = new MidasTokenInfo();
            info.m_token = Convert.ToString(data["access_token"]);
            info.m_endTime = DateTime.Now.AddSeconds(Convert.ToInt32(data["expires_in"]));
            m_data[appId] = info;
        }
        catch (System.Exception ex)
        {
            CLOG.Info(ex.ToString());
        }

        return info;
    }
}

public class QQMiniMidasRecharge
{
    // 0正式，1测试
    public int IsTest { set; get; }

    public string OpenId { set; get; }
    public string AppId { set; get; }

    // 由登录返回的key,由客户端保存，提交上来
    public string SessionKey { set; get; }

    public string AppSecret { set; get; }

    // 扣除游戏币个数, 这个计算出来
    protected int Amt { set; get; }

    // 充值金额
    public string Rmb { set; get; }

    public string SelfOrderId { set; get; }
    public string GoodId { set; get; }

    public static string URL_PATH = "/api/json/openApiPay/GamePrePay";
    public static string URL_FULL = "https://api.q.qq.com" + URL_PATH + "?access_token={0}";

    // 1人民币转成多少虚拟币
    public const int RMB_TO_VIRTUAL_MONEY = 10;

    long getTS()
    {
        TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        return Convert.ToInt64(ts.TotalSeconds);
    }

    public string doRecharge()
    {
        string token = HttpLogin.Global.getQQTokenMgr().getToken(AppId, AppSecret);
        Dictionary<string, object> resultData = new Dictionary<string, object>();

        CLOG.Info("QQMiniMidasRecharge, input, SessionKey={0}, SelfOrderId={1}, OpenId={2}", SessionKey, SelfOrderId, OpenId);
        do
        {
            if (!checkValid(resultData, token))
            {
                break;
            }

            try
            {
                Dictionary<string, object> reqData = new Dictionary<string, object>();
                reqData.Add("openid", OpenId);
                reqData.Add("appid", AppId);
                reqData.Add("ts", getTS());
                reqData.Add("zone_id", "1");
                reqData.Add("pf", "qq_m_qq-2001-android-2011");
                reqData.Add("amt", Amt);
                reqData.Add("bill_no", SelfOrderId);
                reqData.Add("goodid", GoodId);
                reqData.Add("good_num", 1);

                PayCheck check = new PayCheck();
                string waitSign1 = check.getWaitSignStrByAsc(reqData, null, null);
                string waitSign2 = "POST&" + Helper.UrlEncode(URL_PATH) + "&" + waitSign1
                    + "&session_key=" + SessionKey;
                string sig = check.signByHMACSHA256(waitSign2, SessionKey);
                reqData.Add("sig", sig);
                reqData.Add("access_token", token);

                string fmt = string.Format(URL_FULL, token);
                string json = JsonHelper.ConvertToStr(reqData);
                byte[] retbyte = HttpPost.PostJson1(new Uri(fmt), json);
                string retstr = Encoding.UTF8.GetString(retbyte);
                Dictionary<string, object> retData = JsonHelper.ParseFromStr<Dictionary<string, object>>(retstr);
                if (retData.ContainsKey("errcode") && Convert.ToInt32(retData["errcode"]) != 0)
                {
                    CLOG.Info("QQMiniMidasRecharge.doRecharge, error={0}", Convert.ToInt32(retData["errcode"]));
                    resultData.Add(CC.KEY_CODE, CC.ERR_PAY_FAILED);
                    break;
                }

                resultData.Add(CC.KEY_CODE, CC.SUCCESS);
                resultData.Add("prepayId", Convert.ToString(retData["prepayId"]));
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }
        } while (false);

        return JsonHelper.ConvertToStr(resultData);
    }

    protected bool checkValid(Dictionary<string, object> resultData, string token)
    {
        if (string.IsNullOrEmpty(OpenId))
        {
            resultData.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
            CLOG.Info("QQMiniMidasRecharge,checkValid, OpenId param error");
            return false;
        }
        if (string.IsNullOrEmpty(Rmb))
        {
            resultData.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
            CLOG.Info("QQMiniMidasRecharge,checkValid, rmb param error");
            return false;
        }

        Amt = Convert.ToInt32(Rmb) * RMB_TO_VIRTUAL_MONEY;

        if (string.IsNullOrEmpty(SelfOrderId))
        {
            resultData.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
            CLOG.Info("QQMiniMidasRecharge,checkValid, selforderid param error");
            return false;
        }
        if (string.IsNullOrEmpty(SessionKey))
        {
            resultData.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
            CLOG.Info("QQMiniMidasRecharge,checkValid, sessionkey param error");
            return false;
        }
        if (string.IsNullOrEmpty(GoodId))
        {
            resultData.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
            CLOG.Info("QQMiniMidasRecharge,checkValid, GoodId param error");
            return false;
        }

        return true;
    }
}

//////////////////////////////////////////////////////////////////////////
// 微信公众号token管理
public class OfficialAccountsTokenMgr
{
    public static string URL = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

    private Dictionary<string, MidasTokenInfo> m_data = new Dictionary<string, MidasTokenInfo>();
    static object s_object = new object();

    public string getToken(string appId, string appSecret)
    {
        MidasTokenInfo info = null;
        lock (s_object)
        {
            try
            {
                if (m_data.ContainsKey(appId))
                {
                    info = m_data[appId];
                    if (DateTime.Now > info.m_endTime.AddMinutes(-10))
                    {
                        info = refreshToken(appId, appSecret);
                    }
                }
                else
                {
                    info = refreshToken(appId, appSecret);
                }
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
            }
        }

        if (info == null)
            return "";

        return info.m_token;
    }

    // 刷新token
    public MidasTokenInfo refreshToken(string appId, string appSecret)
    {
        MidasTokenInfo info = null;
        try
        {
            string fmt = string.Format(URL, appId, appSecret);
            byte[] ret = HttpPost.Get(new Uri(fmt));
            string retstr = Encoding.UTF8.GetString(ret);
            CLOG.Info(retstr);
            Dictionary<string, object> data = JsonHelper.ParseFromStr<Dictionary<string, object>>(retstr);
            if (data.ContainsKey("errcode") && Convert.ToInt32(data["errcode"]) != 0)
            {
                CLOG.Info("OfficialAccountsTokenMgr,refreshToken, token error, {0}", Convert.ToInt32(data["errcode"]));
                return info;
            }

            info = new MidasTokenInfo();
            info.m_token = Convert.ToString(data["access_token"]);
            info.m_endTime = DateTime.Now.AddSeconds(Convert.ToInt32(data["expires_in"]));
            m_data[appId] = info;
        }
        catch (System.Exception ex)
        {
            CLOG.Info(ex.ToString());
        }

        return info;
    }
}

// 是否关注过公众号
public class COfficialAccountsQuery
{
    public static string URL = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";
    const string RET_YES = "1";
    const string RET_NO = "0";

    public string AppId { set; get; }

    public string AppSecret { set; get; }

    public string doQuery(object param)
    {
        HttpRequest req = (HttpRequest)param;
        string openId = req["openId"];
        if (string.IsNullOrEmpty(openId))
        {
            CLOG.Info("OfficialAccountsQuery.doQuery, openId param error");
            return RET_NO;
        }

        Dictionary<string, object> data = queryData(openId);
        if (data == null)
        {
            return RET_NO;
        }

        if (data.ContainsKey("subscribe"))
        {
            if (Convert.ToInt32(data["subscribe"]) == 1)
                return RET_YES;
        }

        return RET_NO;
    }

    Dictionary<string, object> queryData(string openId)
    {
        Dictionary<string, object> retData = null;
        try
        {
            string fmt = string.Format(URL, HttpLogin.Global.getOfficialAccountTokenMgr().getToken(AppId, AppSecret), openId);
            byte[] ret = HttpPost.Get(new Uri(fmt));
            string retstr = Encoding.UTF8.GetString(ret);
            CLOG.Info(retstr);
            retData = JsonHelper.ParseFromStr<Dictionary<string, object>>(retstr);
            if (retData.ContainsKey("errcode") && Convert.ToInt32(retData["errcode"]) != 0)
            {
                CLOG.Info("COfficialAccountsQuery,queryData, error, {0}, openId={1}", Convert.ToInt32(retData["errcode"]), openId);
                return null;
            }
        }
        catch (System.Exception ex)
        {
            CLOG.Info(ex.ToString());
        }

        return retData;
    }
}

//////////////////////////////////////////////////////////////////////////
// 支付定web支付返回html
public class CAliTransitionWeb
{
    public string AppId { set; get; }
    public string AppPrivateKey { set; get; }
    public string AppPublicKey { set; get; }
    public string NotifyUrl { set; get; }
    public string ReturnUrl { set; get; }
    public string OrderId { set; get; }
    public string TotalAmount { set; get; }
    public string Subject { set; get; }
    public string PayCode { set; get; }

    public const string ALI_SERVER_URL = "https://openapi.alipay.com/gateway.do";

    public string getHtml()
    {
        string retstr = "";
        try
        {
            Aop.Api.DefaultAopClient client = new Aop.Api.DefaultAopClient(ALI_SERVER_URL,
                AppId, AppPrivateKey, "json", "utf-8", "RSA2", AppPublicKey);
            Aop.Api.Request.AlipayTradeWapPayRequest request = new Aop.Api.Request.AlipayTradeWapPayRequest();
            request.SetReturnUrl(ReturnUrl);
            request.SetNotifyUrl(NotifyUrl);

            var c = genContent();
            request.BizContent = JsonHelper.genJson(c);

            retstr = client.pageExecute<Aop.Api.Response.AlipayTradeWapPayResponse>(request).Body;
        }
        catch (System.Exception ex)
        {
            CLOG.Info(ex.ToString());
        }
        return retstr;
    }

    Dictionary<string, object> genContent()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("out_trade_no", OrderId);
        dic.Add("total_amount", TotalAmount);
        dic.Add("subject", Subject);
        dic.Add("product_code", PayCode);

        return dic;
    }
}

// 微信web支付
public partial class WeiXinWebTransition
{
    // 支付成功回调通知
    public string PayNotifyURL { set; get; }

    public string ApiSecret { set; get; }

    public string AppId { set; get; }

    // 商户号
    public string MchId { set; get; }

    // 商口描述
    public string Body { set; get; }

    // 订单ID
    public string SelfOrderId { set; get; }

    public string TotalFee { set; get; }

    // 微信统一下单API接口
    public const string WINXIN_UNIFY_API = "https://api.mch.weixin.qq.com/pay/unifiedorder";

    public const string RAND_STR = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public string getHtml()
    {
        string retstr = "";
        Dictionary<string, object> ret = new Dictionary<string, object>();
        do
        {
            if (string.IsNullOrEmpty(Body) || string.IsNullOrEmpty(SelfOrderId) || string.IsNullOrEmpty(TotalFee))
            {
                break;
            }

            try
            {
                string msgXML = genTranslateXMLString(Body, SelfOrderId, TotalFee);
                byte[] retByte = HttpPost.PostXML(new Uri(WINXIN_UNIFY_API), msgXML);

                XmlGen xml = new XmlGen();
                Dictionary<string, object> retObj = xml.fromXmlString("xml", Encoding.UTF8.GetString(retByte));
                if (Convert.ToString(retObj["return_code"]) != "SUCCESS")
                {
                    break;
                }

                //ret = genReSignData(retObj);
                retstr = Convert.ToString(retObj["mweb_url"]);
                byte[] getstr = HttpPost.Get(new Uri(retstr), "gameyy.com");
                string str = Encoding.UTF8.GetString(getstr);
                return str;
            }
            catch (System.Exception ex)
            {
                CLOG.Info("WeiXinWebTransition:" + ex.ToString());
            }

        } while (false);

        return retstr;
    }

    Dictionary<string, object> genTranData(string body, string selfOrderId, string totalFee)
    {
        Dictionary<string, object> ret = new Dictionary<string, object>();
        ret.Add("appid", AppId);
        ret.Add("mch_id", MchId);
        StringBuilder builder = new StringBuilder();
        Random r = new Random();
        for (int i = 0; i < 16; i++)
        {
            builder.Append(RAND_STR[r.Next(RAND_STR.Length)]);
        }

        ret.Add("nonce_str", builder.ToString());//随机字符串
        ret.Add("body", body);
        ret.Add("out_trade_no", selfOrderId);
        ret.Add("total_fee", totalFee);
        ret.Add("spbill_create_ip", Common.Helper.GetWebClientIp());
        ret.Add("notify_url", PayNotifyURL);
        ret.Add("trade_type", "MWEB");

        Dictionary<string, object> tmp = new Dictionary<string, object>();
        tmp.Add("type", "Android");
        tmp.Add("app_name", "捕鱼支付台");
        tmp.Add("package_name", "com.tencent.tmgp.yqpbydr");
        Dictionary<string, object> top = new Dictionary<string, object>();
        top.Add("h5_info", tmp);
        string scene = JsonHelper.genJson(top);
        ret.Add("scene_info", scene);

        return ret;
    }

    public string genTranslateXMLString(string body, string selfOrderId, string totalFee)
    {
        PayCheck chk = new PayCheck();
        Dictionary<string, object> transData = genTranData(body, selfOrderId, totalFee);
        string waitSign = chk.getWeiXinWaitSigned(transData, null, ApiSecret);
        string sign = Helper.getMD5Upper(waitSign);
        transData.Add("sign", sign);

        XmlGen xml = new XmlGen();
        string str = xml.genXML("xml", transData);
        return str;
    }

    Dictionary<string, object> genReSignData(Dictionary<string, object> retObj)
    {
        Dictionary<string, object> ret = new Dictionary<string, object>();
        ret.Add("appid", Convert.ToString(retObj["appid"]));
        // ret.Add("partnerid", Convert.ToString(retObj["mch_id"]));
        ret.Add("mch_id", Convert.ToString(retObj["mch_id"]));
        // ret.Add("prepayid", Convert.ToString(retObj["prepay_id"]));
        //  ret.Add("body")
        ret.Add("package", "Sign=WXPay");
        ret.Add("noncestr", Convert.ToString(retObj["nonce_str"]));
        TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
        ret.Add("timestamp", Convert.ToInt64(ts.TotalSeconds).ToString());

        PayCheck chk = new PayCheck();
        string waitSign = chk.getWeiXinWaitSigned(ret, null, ApiSecret);
        string sign = Helper.getMD5Upper(waitSign);
        ret.Add("sign", sign);

        return ret;
    }
}

///////////////////////////////////////////////////////////////////////////
//QQgame 支付
public class QQgameRachrage
{
    public string Channel { set; get; }
    public string OpenId { set; get; }
    public string OpenKey { set; get; }
    public string Pf { set; get; }
    public string Pfkey { set; get; }

    public string Zoneid { set; get; }
    public string Payitem { set; get; }  // 物品信息   ID*price*num的格式
    public string Goodsmeta { set; get; }   //name*des
    public string Goodsurl { set; get; }  // 物品图片url

    //1Q币 = 1元    以Q点为单位，1Q币=10Q点   

    public string Rmb { set; get; }
    public int Amt { set; get; }  //Q点
    public string IsBluevip { set; get; } //是否是蓝钻用于蓝钻折扣8折

    public string SelfOrderId { set; get; }
    public string GoodsId { set; get; }

    public string PayTableName { set; get; }

    public bool IsTest { set; get; }

    public string doRecharge()
    {
        Dictionary<string, object> resultData = new Dictionary<string, object>();

        // CLOG.Info("QQgameRecharge, input, OpenId={0}, OpenKey={1}, Pf={2}, Pfkey={3}, Rmb={4},  SelfOrderId={5}, GoodsId={6}, Goodsmeta={7}, Goodsurl={8}",
        //     OpenId, OpenKey, Pf, Pfkey, Rmb, SelfOrderId, GoodsId, Goodsmeta, Goodsurl);
        do
        {
            if (!checkValid(resultData))
                break;

            try
            {
                Dictionary<string, string> reqData = new Dictionary<string, string>();

                reqData.Add("openid", OpenId);
                reqData.Add("openkey", OpenKey);
                reqData.Add("appid", QQgameCFG.appid.ToString());
                reqData.Add("pf", Pf);

                reqData.Add("pfkey", Pfkey);
                reqData.Add("amt", Amt.ToString());

                long ts = PayBase.getTS();
                reqData.Add("ts", ts.ToString());

                //构造 Payitem
                //  Payitem = GoodsId + "*" + Amt + "*" + 1;
                Payitem = SelfOrderId + "*" + Amt + "*" + 1;

                reqData.Add("payitem", Payitem);
                reqData.Add("appmode", "1");
                reqData.Add("goodsmeta", Goodsmeta);  //goodsmeta的值必须是是UTF8格式的字符
                reqData.Add("goodsurl", Goodsurl);
                reqData.Add("zoneid", Zoneid);
                reqData.Add("cee_extend", SelfOrderId);
                OpenApiV3 sdk = new OpenApiV3(QQgameCFG.appid, QQgameCFG.appkey);
                if (IsTest)
                {
                    sdk.SetServerName(QQgameCFG.SERVER_NAME_TEST);
                }
                else
                {
                    sdk.SetServerName(QQgameCFG.SERVER_NAME);
                }

                //支付接口   获取token 和 url_params
                Dictionary<string, object> result = buyGoods(sdk, reqData);
                if (result == null)
                {
                    resultData.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
                }
                else
                {
                    resultData = result;
                }
            }
            catch (System.Exception ex)
            {
                CLOG.Info(ex.ToString());
                resultData.Add("error", ex.ToString());
            }
        } while (false);

        return JsonHelper.ConvertToStr(resultData);
    }

    Dictionary<string, object> buyGoods(OpenApiV3 sdk, Dictionary<string, string> reqData)
    {
        // Q点直购
        RstArray goods_info = sdk.Api("/v3/pay/buy_goods", reqData, "https");  //本接口需要使用https协议访问
        if (goods_info.Ret == 0)
        {
            Dictionary<string, object> tmpRet = JsonHelper.ParseFromStr<Dictionary<string, object>>(goods_info.Msg);
            //return tmpRet;
            if (Convert.ToInt32(tmpRet["ret"]) != 0)
            {
                CLOG.Info("QQgameRecharge error info {0}", goods_info.Msg);
                return null;
            }
            return tmpRet;
        }
        CLOG.Info("QQgameRecharge error info {0}", goods_info.Ret);
        return null;
    }

    protected bool checkValid(Dictionary<string, object> resultData)
    {
        if (string.IsNullOrEmpty(OpenId) || string.IsNullOrEmpty(OpenKey) || string.IsNullOrEmpty(Pf) || string.IsNullOrEmpty(Pfkey))
        {
            resultData.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
            CLOG.Info("QQgameRecharge,checkValid, OpenId, OpenKey, Pf, Pfkey param error");
            return false;
        }

        if (string.IsNullOrEmpty(Rmb))
        {
            resultData.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
            CLOG.Info("QQgameRecharge,checkValid,  Rmb param error");
            return false;
        }
        Amt = Convert.ToInt32(Rmb) / 10;

        if (string.IsNullOrEmpty(SelfOrderId))
        {
            resultData.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
            CLOG.Info("QQgameRecharge,checkValid,  SelfOrderId param error");
            return false;
        }

        if (string.IsNullOrEmpty(GoodsId))
        {
            resultData.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
            CLOG.Info("QQgameRecharge,checkValid, GoodsId param error");
            return false;
        }

        if (string.IsNullOrEmpty(Goodsmeta))
        {
            resultData.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
            CLOG.Info("QQgameRecharge,checkValid, Goodsmeta param error");
            return false;
        }

        if (string.IsNullOrEmpty(Goodsurl))
        {
            resultData.Add(CC.KEY_CODE, CC.ERR_PARAM_ERROR);
            CLOG.Info("QQgameRecharge,checkValid, Goodsurl param error");
            return false;
        }
        return true;
    }
}

///////////////////////////////////////////////////////////////////////////
// 微信二维码支付，生成支付二维码
public class WeiXinQRCode
{
    // 自定义订单ID
    public string SelfOrderId { set; get; }

    // 商品描述
    public string Body { set; get; }

    // 以分为单位，订单总金额
    public int TotalFee { set; get; }

    ////////////////////////////////////////////////////
    // 微信支付分配的公众账号ID
    public string AppId { set; get; }

    // 商户号
    public string MchId { set; get; }

    public string ClientIp { set; get; }

    // 支付成功后的回调通知
    public string PayNotifyURL { set; get; }

    // api 密钥
    public string ApiSecret { set; get; }

    // 统一下单地址
    public const string URI_ORDER = "https://api.mch.weixin.qq.com/pay/unifiedorder";

    ////////////////////////////////////////////////////
    public int OpcodeResult { set; get; }

    // 返回支付url, 采用模式2
    public string getCodeUrl()
    {
        string ulrResult = "";
        try
        {
            do
            {
                WxPayData data = new WxPayData();
                data.ApiKey = ApiSecret;
                data.SetValue("body", Body);
                data.SetValue("out_trade_no", SelfOrderId);
                data.SetValue("total_fee", TotalFee);
                data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss")); // 交易起始时间
                data.SetValue("time_expire", DateTime.Now.AddMinutes(60).ToString("yyyyMMddHHmmss")); // 交易结束时间
                data.SetValue("trade_type", "NATIVE");//交易类型
                data.SetValue("product_id", SelfOrderId);//商品ID

                data.SetValue("notify_url", PayNotifyURL);
                data.SetValue("appid", AppId);//公众账号ID
                data.SetValue("mch_id", MchId);//商户号
                data.SetValue("spbill_create_ip", ClientIp);//终端ip	  	    
                data.SetValue("nonce_str", getRandStr());//随机字符串
                data.SetValue("sign_type", WxPayData.SIGN_TYPE_MD5);//签名类型
                data.SetValue("sign", data.MakeSign(WxPayData.SIGN_TYPE_MD5));

                string xml = data.ToXml();
                string response = HttpService.Post(xml, URI_ORDER, false, 30);
                WxPayData result = new WxPayData();
                result.FromXml(response);
                ulrResult = result.GetValue("code_url").ToString();
            }
            while (false);
        }
        catch (Exception ex)
        {
            CLOG.Info(ex.ToString());
        }

        return ulrResult;
    }

    // 生成二维码
    public MemoryStream genQR(string url)
    {
        QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
        qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
        qrCodeEncoder.QRCodeVersion = 0;
        qrCodeEncoder.QRCodeScale = 8;

        Bitmap image = qrCodeEncoder.Encode(url, Encoding.Default);
        MemoryStream ms = new MemoryStream();
        image.Save(ms, ImageFormat.Png);
        return ms;
    }

    // 装配输入参数
    public string setUpParam(object param)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        OpcodeResult = 1;

        try
        {
            do
            {
                HttpRequest req = (HttpRequest)param;
                SelfOrderId = req.QueryString["orderId"];
                Body = req.QueryString["productName"];
                TotalFee = Convert.ToInt32(req["amount"]);
                if (string.IsNullOrEmpty(SelfOrderId))
                {
                    data.Add("code", 2);
                    CLOG.Info("WeiXinQRCode, orderid null");
                    break;
                }
                if (string.IsNullOrEmpty(Body))
                {
                    data.Add("code", 2);
                    CLOG.Info("WeiXinQRCode, Body null");
                    break;
                }
                if (TotalFee <= 0)
                {
                    data.Add("code", 2);
                    CLOG.Info("WeiXinQRCode, totalfee <= 0");
                    break;
                }

                data.Add("code", 0);
                OpcodeResult = 0;
            } while (false);
        }
        catch (Exception ex)
        {
            data.Add("code", 1);
            CLOG.Info(ex.ToString());
        }

        return JsonHelper.ConvertToStr(data);
    }

    // 生成随机字符串
    string getRandStr()
    {
        WxPayAPI.lib.RandomGenerator randomGenerator = new WxPayAPI.lib.RandomGenerator();
        return randomGenerator.GetRandomUInt().ToString();
    }
}

///////////////////////////////////////////////////////////////////////////
// 华为支付，客户端得到orderInfo后，请求服务器签名
public class HuaWeiOrderInfoSign
{
    public string PrivateKey { set; get; }

    public string getSign(object param)
    {
        try
        {
            HttpRequest Request = (HttpRequest)param;
            Dictionary<string, object> inputData = new Dictionary<string, object>();

            foreach (string key in Request.Form)
            {
                inputData[key] = Request.Form[key];
            }
            PayCheck chk = new PayCheck();
            string wait = chk.getWaitSignStrByAsc(inputData);
            //string sign = chk.signByRSA2(wait, PrivateKey);
            string sign = chk.signBySHA256WithRSA(wait, PrivateKey);
            return sign;
        }
        catch (Exception ex)
        {
            CLOG.Info(ex.ToString());
        }
        return "";
    }
}

///////////////////////////////////////////////////////////////////////////
// 支付宝二维码支付，生成支付二维码
public class AliQRCode
{
    // 自定义订单ID
    public string SelfOrderId { set; get; }

    // 商品描述
    public string Body { set; get; }

    // 总金额， 13.01
    public string TotalAmount { set; get; }

    ////////////////////////////////////////////////////
    // 微信支付分配的公众账号ID
    public string AppId { set; get; }

    // 商户号
    public string MchId { set; get; }

    // api 密钥
    public string AppPrivateKey { set; get; }
    public string AppPublicKey { set; get; }

    public string ReturnURL { set; get; }
    public string NotifyURL { set; get; }

    ////////////////////////////////////////////////////
    public int OpcodeResult { set; get; }

    public string getCodeUrl()
    {
        string retstr = "";
        try
        {
            Aop.Api.DefaultAopClient client = new Aop.Api.DefaultAopClient(CAliTransitionWeb.ALI_SERVER_URL,
                AppId, AppPrivateKey, "json", "utf-8", "RSA2", AppPublicKey);
            Aop.Api.Request.AlipayTradePrecreateRequest request = new Aop.Api.Request.AlipayTradePrecreateRequest();
            //request.SetReturnUrl(ReturnURL);
            request.SetNotifyUrl(NotifyURL);

            var c = genContent();
            request.BizContent = JsonHelper.genJson(c);

            Aop.Api.Response.AlipayTradePrecreateResponse response = client.Execute(request);
            if (response.Code == "10000")
            {
                retstr = response.QrCode;
            }
        }
        catch (System.Exception ex)
        {
            CLOG.Info(ex.ToString());
        }
        return retstr;
    }

    // 生成二维码
    public MemoryStream genQR(string url)
    {
        QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
        qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
        qrCodeEncoder.QRCodeVersion = 0;
        qrCodeEncoder.QRCodeScale = 8;

        Bitmap image = qrCodeEncoder.Encode(url, Encoding.Default);
        MemoryStream ms = new MemoryStream();
        image.Save(ms, ImageFormat.Png);
        return ms;
    }

    // 装配输入参数
    public string setUpParam(object param)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        OpcodeResult = 1;

        try
        {
            do
            {
                HttpRequest req = (HttpRequest)param;
                SelfOrderId = req.QueryString["orderId"];
                Body = req.QueryString["productName"];
                TotalAmount = Convert.ToString(req["amount"]);
                if (string.IsNullOrEmpty(SelfOrderId))
                {
                    data.Add("code", 2);
                    CLOG.Info("AliQRCode, orderid null");
                    break;
                }
                if (string.IsNullOrEmpty(Body))
                {
                    data.Add("code", 2);
                    CLOG.Info("AliQRCode, Body null");
                    break;
                }

                data.Add("code", 0);
                OpcodeResult = 0;
            } while (false);
        }
        catch (Exception ex)
        {
            data.Add("code", 1);
            CLOG.Info(ex.ToString());
        }

        return JsonHelper.ConvertToStr(data);
    }

    Dictionary<string, object> genContent()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("out_trade_no", SelfOrderId);
        dic.Add("total_amount", TotalAmount);
        dic.Add("subject", Body);
        dic.Add("store_id", MchId);
        dic.Add("timeout_express", "30m");
        return dic;
    }
}

///////////////////////////////////////////////////////////////////////////
// oppo 预下单
public class OppoPreOrder
{
    public string AppId { set; get; }

    public string PrivateKey { set; get; }

    public string AppKey { set; get; }

    // 回调通知
    public string NotifyUrl { set; get; }

    ////////////////////由客户提交的//////////////////////////
    public string OpenId { set; get; }

    public string ProductName { set; get; }

    public string ProductDesc { set; get; }

    // 单位分
    public long Price { set; get; }

    public string SelfOrderId { set; get; }

    public string AppVersion { set; get; }

    public string EngineVersion { set; get; }

    public const string URL = "https://jits.open.oppomobile.com/jitsopen/api/pay/v1.0/preOrder";

    public bool loadData(object param, out string outstr)
    {
        bool res = false;

        Dictionary<string, object> ret = new Dictionary<string, object>();
        HttpRequest req = (HttpRequest)param;
        do
        {
            OpenId = req.Form["openId"];
            if (string.IsNullOrEmpty(OpenId))
            {
                ret.Add("code", 1);
                CLOG.Info("OppoPreOrder.loadData miss openId");
                break;
            }
            ProductName = req.Form["productName"];
            if (string.IsNullOrEmpty(ProductName))
            {
                ret.Add("code", 1);
                CLOG.Info("OppoPreOrder.loadData miss productName");
                break;
            }
            ProductDesc = req.Form["productDesc"];
            if (string.IsNullOrEmpty(ProductDesc))
            {
                ret.Add("code", 1);
                CLOG.Info("OppoPreOrder.loadData miss productDesc");
                break;
            }
            long price = 0;
            if (!long.TryParse(req.Form["price"], out price))
            {
                ret.Add("code", 1);
                CLOG.Info("OppoPreOrder.loadData miss price");
                break;
            }
            Price = price;

            SelfOrderId = req.Form["selfOrderId"];
            if (string.IsNullOrEmpty(SelfOrderId))
            {
                ret.Add("code", 1);
                CLOG.Info("OppoPreOrder.loadData miss selfOrderId");
                break;
            }
            AppVersion = req.Form["appVersion"];
            if (string.IsNullOrEmpty(AppVersion))
            {
                ret.Add("code", 1);
                CLOG.Info("OppoPreOrder.loadData miss appVersion");
                break;
            }
            EngineVersion = req.Form["engineVersion"];
            if (string.IsNullOrEmpty(EngineVersion))
            {
                ret.Add("code", 1);
                CLOG.Info("OppoPreOrder.loadData miss engineVersion");
                break;
            }

            ret.Add("code", 0);
            res = true;
        } while (false);

        outstr = JsonHelper.ConvertToStr(ret);
        return res;
    }

    public string getOrder()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("appId", AppId);
        data.Add("openId", OpenId);
        long timestamp = PayBase.getTSMill();
        data.Add("timestamp", timestamp);
        data.Add("productName", ProductName);
        data.Add("productDesc", ProductDesc);
        data.Add("count", 1);
        data.Add("price", Price);
        data.Add("currency", "CNY");
        data.Add("cpOrderId", SelfOrderId);
        data.Add("appVersion", AppVersion);
        data.Add("engineVersion", EngineVersion);
        data.Add("callBackUrl", NotifyUrl);

        PayCheck chk = new PayCheck();
        string wait = chk.getWaitSignStrByAsc(data);
        string sign = chk.signBySHA256WithRSA(wait, PrivateKey);
        data.Add("sign", sign);

        string str = JsonHelper.ConvertToStr(data);

        Dictionary<string, object> ret = new Dictionary<string, object>();

        try
        {
            do
            {
                string retstr = HttpService.PostData((webReq, sendData) =>
                {
                    sendData.m_contentType = "application/json";
                    sendData.m_str = str;
                }, URL, false, 90);

                Dictionary<string, object> od = JsonHelper.ParseFromStr<Dictionary<string, object>>(retstr);
                if (Convert.ToInt32(od["code"]) != 200)
                {
                    ret.Add("code", 1);
                    CLOG.Info("OppoPreOrder.getOrder code {0}", Convert.ToString(od["msg"]));
                    break;
                }

                Newtonsoft.Json.Linq.JObject d = (Newtonsoft.Json.Linq.JObject)od["data"];
                Dictionary<string, object> exdata = d.ToObject<Dictionary<string, object>>();
                //Dictionary<string, object> exdata = (Dictionary<string, object>)od["data"];
                string orderNo = Convert.ToString(exdata["orderNo"]);
                string paySign = calRetSign(timestamp, orderNo, chk);
                ret.Add("code", 0);
                ret.Add("orderNo", orderNo);
                ret.Add("timestamp", timestamp);
                ret.Add("paySign", paySign);

            } while (false);

        }
        catch (Exception ex)
        {
            ret["code"] = 1;
            CLOG.Info(ex.ToString());
        }

        return JsonHelper.ConvertToStr(ret);
    }

     string calRetSign(long timestamp,  string orderNo, PayCheck chk)
    {
    string wait = $"appKey={AppKey}&orderNo={orderNo}&timestamp={timestamp}";
    return chk.signBySHA256WithRSA(wait, PrivateKey);
    }
    }

    ///////////////////////////////////////////////////////////////////////////
    // 天网娱乐 tianwangyule
    public class TianWangYuLe
    {
        public const string URL_ORDER = "http://api.tianwangyule.cn/order/v2/create?{0}";

        // 商户号
        public string Merchantid { set; get; }

        // 订单标题
        public string Subject { set; get; }

        // 订单ID
        public string OrderId { set; get; }

        // 金额，保留2位小数
        public string Amount { set; get; }

        // 到账回调通知url
        public string NotifyURL { set; get; }

        // 支付类型(ali.h5  ali.wap  ali.qr  wx.app)
        public string TypePay { set; get; }

        public string Secret { set; get; }

        public RetInfo m_ret;

        class KV
        {
            public string key;
            public string value;

            public override string ToString()
            {
                return key + "=" + value;
            }
        }

        public class RetInfo
        {
            public string result;
            public string data;
            public string code;
            public string type;
            public string message;
        }

        // 创建订单
        public string createOrder()
        {
            List<KV> vlist = new List<KV>();
            addkv(vlist, "merchantid", Merchantid);
            addkv(vlist, "subject", Subject);
            addkv(vlist, "orderid", OrderId);
            addkv(vlist, "timeoutexpress", "10m");
            addkv(vlist, "amount", Amount);
            addkv(vlist, "notifyurl", NotifyURL);

            if (TypePay == "ali")
            {
                addkv(vlist, "type", "ali.h5");
            }
            else if (TypePay == "wx")
            {
                addkv(vlist, "type", "wx.h5");
            }
            else
            {
                return "pay type error";
            }

            string sign = calSign(vlist);

            try
            {
                NameValueCollection param = genPostParam(vlist, sign);
                string debug = getDebugStr(param);

                string urlPram = getURLParam(vlist, sign);
                string fmt = string.Format(URL_ORDER, urlPram);
                byte[] arr = HttpPost.Get(new Uri(fmt));
                string retStr = Encoding.UTF8.GetString(arr);
                m_ret = JsonHelper.ParseFromStr<RetInfo>(retStr);
                if (m_ret.code != "400")
                {
                    CLOG.Info("TianWangYuLe " + m_ret.message);
                }
                else
                {
                    return "ok";
                }
            }
            catch (Exception ex)
            {
                CLOG.Info(ex.ToString());
            }

            return "";
        }

        string getDebugStr(NameValueCollection nv)
        {
            StringBuilder builder = new StringBuilder();

            string[] allKeys = nv.AllKeys;
            for (int i = 0; i < allKeys.Length; i++)
            {
                string k = allKeys[i];
                builder.Append(k);
                builder.Append("=");
                builder.Append(nv[k]);
                builder.Append("&");
            }

            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        NameValueCollection genPostParam(List<KV> vlist, string sign)
        {
            NameValueCollection v = new NameValueCollection();
            foreach (var item in vlist)
            {
                v.Add(item.key, System.Web.HttpUtility.UrlEncode(item.value));
            }
            v.Add("sign", sign);
            return v;
        }

        private void addkv(List<KV> vlist, string k, string v)
        {
            KV t = new KV();
            t.key = k;
            t.value = v;
            vlist.Add(t);
        }

        private string calSign(List<KV> vlist)
        {
            string wait = getWaitSign(vlist);
            return Helper.getMD5Upper(wait);
        }

        private string getWaitSign(List<KV> vlist)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < vlist.Count; i++)
            {
                builder.Append(vlist[i].ToString());
                builder.Append("&");
            }

            builder.Append("secret");
            builder.Append("=");
            builder.Append(Secret);
            return builder.ToString();
        }

        private string getURLParam(List<KV> vlist, string sign)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < vlist.Count; i++)
            {
                builder.AppendFormat("{0}={1}", vlist[i].key, System.Web.HttpUtility.UrlEncode(vlist[i].value));
                builder.Append("&");
            }

            builder.Append("sign");
            builder.Append("=");
            builder.Append(sign);
            return builder.ToString();

        }
    }
//        }
//    }