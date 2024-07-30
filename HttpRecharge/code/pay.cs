using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.Collections.Specialized;
using System.Web.UI;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Configuration;
using Common;
using System.IO;
using System.Net;
using System.Collections;
using System.Text.RegularExpressions;

using NS_OpenApiV3;
using NS_SnsNetWork;
using NS_SnsSigCheck;
using NS_SnsStat;
using System.Diagnostics;

//////////////////////////////////////////////////////////////////////////
public class PayCallbackQuicksdk : PayCallbackBase
{
   // public const string MD5_KEY = "57jfsrwsi6mfuayesgg3rqzyvp2jdp5j";
    //public const string CALLBACK_KEY = "52795318485433831236655560248833";

    public string Md5Key { set; get; }
    public string CallbackKey { set; get; }

    public const string OK = "SUCCESS";
    public const string FAIL = "FAILED";

    public override string notifyPay(object param)
    {
        HttpRequest Request = (HttpRequest)param;
        string result = OK;
       // CLOG.Info("quicksdk..enter");

        try
        {
            do
            {
                string nt_data = Request.Form["nt_data"];
                string sign = Request.Form["sign"];
                string md5sign = Request.Form["md5Sign"];

               // CLOG.Info("nt_data:" + nt_data);
               // CLOG.Info("sign:" + sign);
               // CLOG.Info("md5sign:" + md5sign);

                string calSign = Helper.getMD5(nt_data + sign + Md5Key);
                if (calSign != md5sign)
                {
                    CLOG.Info("quicksdk..sign error, calSign: {0}, fact: {1}", calSign, md5sign);
                    result = "error";
                    break;
                }

                string nt_data_d = decode(nt_data, CallbackKey);
               // CLOG.Info("nt_data_d:" + nt_data_d);
               
                XmlGen gen = new XmlGen();
                Dictionary<string, object> inputData = gen.fromXmlString("quicksdk_message/message", nt_data_d);
                if (Convert.ToString(inputData["status"]) != "0")
                {
                    CLOG.Info("quicksdk..status error， {0}", Convert.ToString(inputData["status"]));
                    result = FAIL;
                    break;
                }

                string selfOrderId = Convert.ToString(inputData["extras_params"]);
                PayInfoBase baseData = new PayInfoBase();

                double price = Convert.ToDouble(inputData["amount"]);
                CheckRet checkCode = checkOrder(selfOrderId, (int)price, baseData);
                if (checkCode != CheckRet.checkRetSuccess)
                {
                    if (checkCode == CheckRet.checkRetRmbError)
                    {
                        CLOG.Info("quicksdk..rmberror");
                        result = "error";
                        break;
                    }
                    else if (checkCode == CheckRet.checkRetNoOrder)
                    {
                        result = "error";
                        CLOG.Info("quicksdk..noorder:{0}", selfOrderId);
                        break;
                    }
                    else if (checkCode == CheckRet.checkRetException)
                    {
                        CLOG.Info("quicksdk..ex");
                        result = "error";
                        break;
                    }
                    else
                    {
                        result = "error";
                        CLOG.Info("quicksdk..other error");
                        break;
                    }
                }
                baseData.addBaseDataToDic(inputData);

                bool res = insertPayData(PayTable.QUICKSDK_PAY, inputData, baseData);
                if (!res)
                {
                    CLOG.Info("quickdk..db error");
                }

            } while (false);
        }
        catch (System.Exception ex)
        {
            result = ex.ToString();
            CLOG.Info("quickdk " + ex.ToString());
        }

        return result;
    }

    static public string decode(string src, string key)
    {

        if (src == null || src.Length == 0)
        {
            return src;
        }

        string pattern = "\\d+";
        MatchCollection results = Regex.Matches(src, pattern);

        ArrayList list = new ArrayList();
        for (int i = 0; i < results.Count; i++)
        {
            try
            {
                String group = results[i].ToString();
                list.Add((Object)group);
            }
            catch (Exception e)
            {
                return src;
            }
        }

        if (list.Count > 0)
        {
            try
            {
                byte[] data = new byte[list.Count];
                byte[] keys = System.Text.Encoding.Default.GetBytes(key);

                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = (byte)(Convert.ToInt32(list[i]) - (0xff & Convert.ToInt32(keys[i % keys.Length])));
                }
                return System.Text.Encoding.Default.GetString(data);
            }
            catch (Exception e)
            {
                return src;
            }
        }
        else
        {
            return src;
        }

    }
}

//////////////////////////////////////////////////////////////////////////
// 手机QQ支付回调
public class PayCallbackQQMinisdk : PayCallbackBase
{
   public static string URL_PATH="/QQMiniPay.aspx";
   static List<string> s_excludeKey = new List<string>(new string[] { "sig" });

   public string AppSecret { set; get; }

   public string PayTableName { set; get; }

   public override string notifyPay(object param)
   {
       HttpRequest Request = (HttpRequest)param;
       Dictionary<string, object> resultData = new Dictionary<string, object>();
       try
       {
           do
           {
               byte[] byteArr = new byte[Request.InputStream.Length];
               Request.InputStream.Read(byteArr, 0, byteArr.Length);
               string byteStr = Encoding.Default.GetString(byteArr);
               CLOG.Info("PayCallbackQQMinisdk:" + byteStr);

               Dictionary<string, object> inputData = JsonHelper.ParseFromStr<Dictionary<string, object>>(byteStr);

               PayCheck check = new PayCheck();
               string waitSign1 = check.getWaitSignStrByAsc(inputData, s_excludeKey);
               string waitSign = "POST&" + Helper.UrlEncode(URL_PATH) + "&" + waitSign1 + "&AppSecret=" + AppSecret;
               string sign = check.signByHMACSHA256(waitSign, AppSecret);
               if (sign != Convert.ToString(inputData["sig"]))
               {
                   CLOG.Info("PayCallbackQQMinisdk, sign error");
                   break;
               }

               string selfOrderId = Convert.ToString(inputData["bill_no"]);
               PayInfoBase baseData = new PayInfoBase();

               double price = Convert.ToDouble(inputData["amt"])/10;
               CheckRet checkCode = checkOrder(selfOrderId, (int)price, baseData);
               if (checkCode != CheckRet.checkRetSuccess)
               {
                   if (checkCode == CheckRet.checkRetRmbError)
                   {
                       CLOG.Info("PayCallbackQQMinisdk..rmberror");
                       resultData.Add("code", 4);
                       break;
                   }
                   else if (checkCode == CheckRet.checkRetNoOrder)
                   {
                       resultData.Add("code", 5);
                       CLOG.Info("PayCallbackQQMinisdk..noorder:{0}", selfOrderId);
                       break;
                   }
                   else if (checkCode == CheckRet.checkRetException)
                   {
                       CLOG.Info("PayCallbackQQMinisdk..ex");
                       resultData.Add("code", 6);
                       break;
                   }
                   else
                   {
                       resultData.Add("code", 7);
                       break;
                   }
               }
               baseData.addBaseDataToDic(inputData);

               bool res = insertPayData(PayTableName, inputData, baseData);
               if (!res)
               {
                   CLOG.Info("PayCallbackQQMinisdk..db error");
                   resultData.Add("code", 2);
                   break;
               }

               resultData.Add("code", 0);
           } while (false);
       }
       catch (System.Exception ex)
       {
           resultData.Add("code", 1);
           CLOG.Info("PayCallbackQQMinisdk " + ex.ToString());
       }

       return JsonHelper.ConvertToStr(resultData);
   }
}

// 华为支付回调
public class PayCallbackHuaWei : PayCallbackBase
{
    public static List<string> s_excludeKey = new List<string>(new string[] { "sign", "signType" });

    public string PublicKey
    {
        set;get;
    }
    public string PayTableName
    {
        set;
        get;
    }

    public override string notifyPay(object param) 
    {
        Dictionary<string, object> retData = new Dictionary<string, object>();

        try
        {
           //CLOG.Info("PayCallbackHuaWei.notifyPay enter: {0}", PayTableName);

            HttpRequest Request = (HttpRequest)param;
            Dictionary<string, object> inputData = new Dictionary<string, object>();
            foreach (string key in Request.Form)
            {
                inputData[key] = Request.Form[key];
            }

            if (inputData.Count <= 0)
            {
                var req = Request.QueryString;
                foreach (string key in req)
                {
                    inputData[key] = req[key];
                }

                if (inputData.Count <= 0)
                {
                    byte[] byteArr = new byte[Request.InputStream.Length];
                    Request.InputStream.Read(byteArr, 0, byteArr.Length);
                    
                    string byteStr1 = Encoding.Default.GetString(byteArr);
                    CLOG.Info("default---{0} $$$$ {1}", PayTableName, byteStr1);

                    string byteStr = Encoding.UTF8.GetString(byteArr);
                    CLOG.Info("utf8---{0} $$$$ {1}", PayTableName, byteStr);
                    NameValueCollection nvc = HttpUtility.ParseQueryString(byteStr);
                    string[] allKeys = nvc.AllKeys;
                    foreach (var d in allKeys)
                    {
                        inputData[d] = nvc[d];
                    }

                    if (inputData.Count <= 0)
                    {
                        retData.Add("result", 98); // 参数错误
                        CLOG.Info("[{0}]参数错", PayTableName);
                        return JsonHelper.genJson(retData);
                    }
                }
            }

            string waitStr = getWaitSignStrByAsc(inputData, s_excludeKey);
            string signType = "";
            if (inputData.ContainsKey("signType"))
            {
                signType = Convert.ToString(inputData["signType"]);
            }
            bool signRes = false;

            if (signType == "RSA256")
            {
                signRes = checkSignByRSA256PublicKey(waitStr, Convert.ToString(inputData["sign"]), PublicKey);
            }
            else
            {
                signRes = checkSignByRSAPublicKey(waitStr, Convert.ToString(inputData["sign"]), PublicKey);
            }

            if (!signRes)
            {
                retData.Add("result", 1); // 验签失败
                CLOG.Info("[{0}]验签失败", PayTableName);
                return JsonHelper.genJson(retData);
            }

            int rmb = (int)Convert.ToDouble(inputData["amount"]);
            if (rmb == 0)
            {
                retData.Add("result", 98);
                CLOG.Info("[{0}]人民币错误", PayTableName);
                return JsonHelper.genJson(retData);
            }

//            string[] arrStr = splitInfo(Convert.ToString(inputData["extReserved"]));
//             PayInfoBase baseData = new PayInfoBase();
//             baseData.m_payTime = DateTime.Now;
//             baseData.m_payCode = arrStr[0];
//             baseData.m_account = arrStr[1];
//             baseData.m_playerId = Convert.ToInt32(arrStr[2]);
//             baseData.m_rmb = rmb;
//             baseData.m_orderId = Convert.ToString(inputData["requestId"]);
//             baseData.m_channelNumber = arrStr[3];

            string selfOrderId = Convert.ToString(inputData["requestId"]);
            PayInfoBase baseData = new PayInfoBase();
            CheckRet checkCode = checkOrder(selfOrderId, rmb, baseData);
            if (checkCode != CheckRet.checkRetSuccess)
            {
                if (checkCode == CheckRet.checkRetRmbError)
                {
                    CLOG.Info("huawei..rmberror");
                    retData.Add("result", 3);
                    return JsonHelper.genJson(retData);
                }
                else if (checkCode == CheckRet.checkRetNoOrder)
                {
                    retData.Add("result", 3);
                    CLOG.Info("huawei..noorder:{0}", selfOrderId);
                    return JsonHelper.genJson(retData);
                }
                else if (checkCode == CheckRet.checkRetException)
                {
                    retData.Add("result", 94);
                    CLOG.Info("huawei..ex");
                    return JsonHelper.genJson(retData);
                }
            }

            /*if (!PayBase.existOrderInPaymentTotal(baseData, MongodbPayment.Instance))
            {
                retData.Add("result", 99); // 没有查询到订单
                CLOG.Info("[{0}]没找到OrderID[{1}]", PayTableName, baseData.m_orderId);
                return JsonHelper.genJson(retData);
            }*/

            baseData.addBaseDataToDic(inputData);
            bool res = insertPayData(PayTableName, inputData, baseData);
            if (!res)
            {
                retData.Add("result", 95); // IO 错误
                CLOG.Info("[{0}]db error", PayTableName);
                return JsonHelper.genJson(retData);
            }

            retData.Add("result", 0); // 成功
            return JsonHelper.genJson(retData);
        }
        catch (System.Exception ex)
        {
            CLOG.Info("{0}, {1}", PayTableName, ex.ToString());
        }

        retData.Add("result", 98); // 参数错误
        return JsonHelper.genJson(retData);
    }
}

///////////////////////////////////////////////////////////////////////////////
// 金立支付回调
public class PayCallbackAmigo : PayCallbackBase
{
    // 公钥
    public static string PUBLIC_KEY = "<RSAKeyValue><Modulus>rHqRCOeRFSN3XT/xEAYUrDCyPeaaQhYjb6cHgYz2zXG93tWt7a38Cdt3T+0JGb/PyI9FCfsc19l9/303udUdihxesvSRWJGZpwhdS+KML6nXQL0R0neFf+rwTwMLxxNLj3DoO8SujjlN3zjc9O/OI3ta2Kn+8XfoX1j2epEMzHc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
    static List<string> s_excludeKey = new List<string>(new string[] { "sign" });
    static string[] S_FIELDS = { "PayCode", "PlayerId", "Account", "channel_number" };

    public override string notifyPay(object param)
    {
        string resultCode = "success";
        HttpRequest Request = (HttpRequest)param;
        Dictionary<string, object> inputData = new Dictionary<string, object>();
        foreach (string key in Request.Form)
        {
            inputData[key] = Request.Form[key];
        }

        Dictionary<string, object> retData = new Dictionary<string, object>();

        try
        {
            string waitStr = getWaitSignStrByAsc(inputData, s_excludeKey);
            bool signRes = checkSignByRSAPublicKey(waitStr, Convert.ToString(inputData["sign"]), PUBLIC_KEY);
            if (!signRes)
            {
                resultCode = "sign error";
                CLOG.Info("jinli 验签错误");
                return resultCode;
            }

            string selfOrderId = Convert.ToString(inputData["out_order_no"]);
            Dictionary<string, object> qd = PayBase.queryBaseData(selfOrderId, MongodbPayment.Instance);
            if (qd == null)
            {
                resultCode = "noOrderId";
                CLOG.Info("jinli noOrderId:{0}", selfOrderId);
                return resultCode;
            }

            int srcRMB = Convert.ToInt32(qd["RMB"]);
            int factRMB = (int)(double)Convert.ToDouble(inputData["deal_price"]);
            if (srcRMB != factRMB)
            {
                resultCode = "rmb error";
                CLOG.Info("jinli rmb error:{0}, need:{1}, fact:{2}", selfOrderId, srcRMB, factRMB);
                return resultCode;
            }
            PayInfoBase baseData = new PayInfoBase();
            baseData.m_payTime = DateTime.Now;
            baseData.m_payCode = Convert.ToString(qd["PayCode"]);
            baseData.m_playerId = Convert.ToInt32(qd["PlayerId"]);
            baseData.m_account = Convert.ToString(qd["Account"]);
            baseData.m_rmb = srcRMB;//(int)(double)Convert.ToDouble(inputData["deal_price"]);
            baseData.m_orderId = selfOrderId;
            baseData.m_channelNumber = Convert.ToString(qd["channel_number"]);

            baseData.addBaseDataToDic(inputData);
            bool res = insertPayData(PayTable.JINLI_PAY, inputData, baseData);
            if (!res)
            {
                resultCode = "db error";
            }
        }
        catch (System.Exception ex)
        {
            resultCode = "exception";
            CLOG.Info("jinli:{0}", ex.ToString());
        }
        
        return resultCode;
    }
}

//////////////////////////////////////////////////////////////////////////
// anysdk回调
public class PayCallbackAnysdk : PayCallbackBase
{
    //const string ANYSDK_KEY = "70E70442152F7D7E24FC04C6DF7E3115";

    static string ANYSDK_KEY = ConfigurationManager.AppSettings["anysdk_key"];

    static List<string> s_excludeKey = new List<string>(new string[] { "sign", "signType" });

    public override string notifyPay(object param)
    {
        Page page = (Page)param;
        HttpRequest Request = page.Request;
        string result = "ok";
        NameValueCollection req = Request.Form;

        if (req.Count <= 0)
        {
            result = "req.Count <= 0";
            CLOG.Info("anysdk req.Count <= 0");
            return result;
        }

        Dictionary<string, object> data = new Dictionary<string, object>();
        foreach (string key in req)
        {
            data[key] = req[key];
        }

        PayInfoBase baseData = new PayInfoBase();
        baseData.m_payTime = DateTime.Now;
        data["PayTime"] = baseData.m_payTime;
        MongodbPayment.Instance.ExecuteInsert(PayTable.LOG_ANYSDK, data);

        if (data["sign"].ToString() != getSignForAnyValid(Request) ||
            data["pay_status"].ToString() != "1")
        {
            CLOG.Info("anysdk:验签或pay_status:{0}", data["pay_status"].ToString());
            return result;
        }

        try
        {
            int payCode = check_paycode(data["product_id"].ToString(), page);
            if (payCode < 0)
            {
                data["amount"] = "0";//异常订单不算充值
            }

            try
            {
                 baseData.m_rmb = (int)Convert.ToDouble(data["amount"]);
            }
            catch (Exception)
            {
                baseData.m_rmb = 0;
            }

            if (baseData.m_rmb == 0)
            {
                result = "rmb error";
                CLOG.Info("anysdk..rmb is 0");
                return result;
            }

            //data["ServerId"] = Convert.ToInt32(data["server_id"]);

            baseData.m_account = data["channel_number"].ToString() + "_" + data["user_id"].ToString();
            baseData.m_payCode = payCode.ToString();
            baseData.m_playerId = Convert.ToInt32(data["game_user_id"]);
            baseData.m_channelNumber = data["channel_number"].ToString();

            if (data.ContainsKey("private_data"))
            {
                string[] strs = data["private_data"].ToString().Trim().Split('#');
                if (strs.Length > 1)
                {
                    baseData.m_orderId = strs[0]; // 订单号

                    data["shoppage"] = strs[1];

                    //爱贝渠道特殊处理
                    if (data["channel_number"].ToString() == "800053")
                    {
                        //data["Account"] = strs[2];
                        baseData.m_account = strs[2];
                    }
                    //真实账号KEY
                    data["real_Account"] = strs[2];
                    //设备号
                    if (strs.Length > 3)
                        data["acc_dev"] = strs[3];
                }
                else
                {
                    data["shoppage"] = data["private_data"];
                }
            }
            else
            {
                result = "noPrivateData";
                CLOG.Info("anysdk..noPrivateData");
                return result;
            }

            Dictionary<string, object> qd = PayBase.queryBaseData(baseData.m_orderId, MongodbPayment.Instance, S_TOTAL_PAY_FIELDS);
            if (qd == null)
            {
                result = "noOrderId";
                CLOG.Info("anysdk..noorderId:{0}", baseData.m_orderId);
                return result;
            }
            else
            {
                int rmb = Convert.ToInt32(qd["RMB"]);
                if (baseData.m_rmb != rmb)
                {
                    result = "rmb error";
                    CLOG.Info("anysdk..rmb error:应为{0}, 实为{1}", rmb, baseData.m_rmb);
                    return result;
                }
            }

            baseData.addBaseDataToDic(data);

            bool res = insertPayData(PayTable.ANYSDK_PAY, data, baseData);
            if (!res)
            {
                CLOG.Info("anysdk..db error");
            }
        }
        catch (Exception ex)
        {
            result = ex.ToString();
            CLOG.Info("andsdk " + ex.ToString());
        }

        return result;
    }

    //获得anysdk支付通知 sign,将该函数返回的值与any传过来的sign进行比较验证
    public String getSignForAnyValid(HttpRequest request)
    {
        NameValueCollection requestParams = request.Form;//获得所有的参数名
        List<String> ps = new List<String>();
        foreach (string key in requestParams)
        {
            ps.Add(key);
        }

        sortParamNames(ps);// 将参数名从小到大排序，结果如：adfd,bcdr,bff,zx

        String paramValues = "";
        foreach (string param in ps)
        {//拼接参数值
            if (param == "sign")
            {
                continue;
            }
            String paramValue = requestParams[param];
            if (paramValue != null)
            {
                paramValues += paramValue;
            }
        }
        String md5Values = Helper.getMD5(paramValues);
        md5Values = Helper.getMD5(md5Values.ToLower() + ANYSDK_KEY).ToLower();
        return md5Values;
    }

    //将参数名从小到大排序，结果如：adfd,bcdr,bff,zx
    public static void sortParamNames(List<String> paramNames)
    {
        paramNames.Sort((String str1, String str2) => { return str1.CompareTo(str2); });
    }

    public int check_paycode(string productid, Page page)
    {
        int returnid = -1;
        string isThPay = ConfigurationManager.AppSettings["th_pay"];
        if (string.IsNullOrEmpty(isThPay))
        {
            isThPay = "false";
        }
        if (isThPay.Equals("true"))
        {
            Dictionary<string, int> payCodes = GetTHPayCode(page);
            if (payCodes.ContainsKey(productid))
            {
                returnid = payCodes[productid];
            }
            else
            {
                returnid = -1;
            }
        }
        else
        {
            string pid = productid;
            int index = pid.LastIndexOf('_');
            if (index > 0)
                pid = pid.Remove(0, index + 1);

            try
            {
                returnid = Convert.ToInt32(pid);
            }
            catch (Exception ed)
            {
                returnid = -1;
            }
        }
        return returnid;
    }

    protected Dictionary<string, int> GetTHPayCode(Page page)
    {
        Dictionary<string, int> ResList = (Dictionary<string, int>)page.Cache.Get("THPayCode");

        if (ResList == null)
        {
            try
            {
                XmlDocument docServer = new XmlDocument();
                string path = HttpContext.Current.Server.MapPath("./");
                docServer.Load(path + "M_RechangeCFG.xml");

                XmlNode configServer = docServer.SelectSingleNode("Root");
                ResList = new Dictionary<string, int>();
                foreach (XmlNode node in configServer.ChildNodes)
                {
                    if (node.Name == "Data")
                    {
                        int paycode = Convert.ToInt32(node.Attributes["ID"].Value);
                        string appstoreid = node.Attributes["AppStoreID"].Value;
                        if (!string.IsNullOrEmpty(appstoreid))
                        {
                            ResList.Add(appstoreid, paycode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CLOG.Info(ex.Message);
            }
            page.Cache["THPayCode"] = ResList;
        }
        return ResList;
    }
}

// 迅雷支付回调
public class PayCallbackXunlei : PayCallbackBase
{
    public const string SERVER_KEY = "c43bbd8bc6ae455cafdae67255334b21";
    static string[] S_FIELDS = { "PlayerId", "PayCode", "Account", "channel_number" };

    public override string notifyPay(object param)
    {
        HttpRequest Request = (HttpRequest)param;

        byte[] byteArr = new byte[Request.InputStream.Length];
        Request.InputStream.Read(byteArr, 0, byteArr.Length);
        string byteStr = Encoding.Default.GetString(byteArr);
       // CLOG.Info("xunlei..enter");

        string codeStr = "success";
        try
        {
            Dictionary<string, object> dic = JsonHelper.ParseFromStr<Dictionary<string, object>>(byteStr);
            OrderInfo orderInfo = JsonHelper.ParseFromStr<OrderInfo>(dic["order_info"].ToString());

            string status = orderInfo.pay_status;
            if (status != "success") // 仅success时，迅雷才会通知
            {
                codeStr = "error";
                return codeStr;
            }

            string sign = Convert.ToString(dic["sign"]);

            Dictionary<string, object> upData = getData(orderInfo);

            string calSign = getSignForAnyValid(upData);
            if (sign != calSign)
            {
                codeStr = "errorSign";
                CLOG.Info("xunlei..errorSign");
                return codeStr;
            }

            int rmb = (int)Convert.ToDouble(upData["total_amount"]);
            if (rmb <= 0)
            {
                codeStr = "error";
                CLOG.Info("xunlei..error amount");
                return codeStr;
            }

            PayInfoBase baseData = new PayInfoBase();
            baseData.m_orderId = orderInfo.product_id;
            
            Dictionary<string, object> qd = PayBase.queryBaseData(baseData.m_orderId, MongodbPayment.Instance, S_FIELDS);
            //if (!existOrderInPaymentTotal(baseData))
            if (qd == null)
            {
                codeStr = "noOrderId";
                CLOG.Info("xunlei..noOrderId:{0}, fullstr:{1}", baseData.m_orderId, orderInfo.product_id);
                return codeStr;
            }
            int srcRMB = Convert.ToInt32(qd["RMB"]);
            if (rmb != srcRMB)
            {
                codeStr = "rmb error";
                CLOG.Info("xunlei rmb error:{0}, need:{1}, fact:{2}", baseData.m_orderId, srcRMB, rmb);
                return codeStr;
            }
            baseData.m_payTime = DateTime.Now;
            baseData.m_payCode = Convert.ToString(qd["PayCode"]);
            baseData.m_playerId = Convert.ToInt32(qd["PlayerId"]);
            baseData.m_account = Convert.ToString(qd["Account"]);
            baseData.m_rmb = rmb;
            baseData.m_channelNumber = Convert.ToString(qd["channel_number"]);

            baseData.addBaseDataToDic(upData);
            bool res = insertPayData(PayTable.XUNLEI_PAY, upData, baseData);
            if (!res)
            {
                codeStr = "dberror";
                CLOG.Info("xunlei..dberror");
            }
        }
        catch (Exception ex)
        {
            codeStr = "error";
            CLOG.Info("xunlei..{0}", ex.ToString());
        }

        return codeStr;
    }

    public String getSignForAnyValid(Dictionary<string, object> data)
    {
        var descData = from s in data
                       orderby s.Key ascending
                       select s;

        StringBuilder sbuilder = new StringBuilder();
        bool first = true;
        foreach (var d in descData)
        {
            if (d.Value == null)
                continue;

            if (Convert.ToString(d.Value) == "")
                continue;

            if (first)
            {
                first = false;
                sbuilder.AppendFormat("{0}={1}", d.Key, d.Value);
            }
            else
            {
                sbuilder.AppendFormat("&{0}={1}", d.Key, d.Value);
            }
        }

        sbuilder.AppendFormat("&key={0}", SERVER_KEY);
        String md5Values = Helper.getMD5Upper(sbuilder.ToString());
        return md5Values;
    }

    Dictionary<string, object> getData(OrderInfo orderInfo)
    {
        Dictionary<string, object> upData = new Dictionary<string, object>();
        upData["body"] = orderInfo.body;
        upData["channel_pay_uid"] = orderInfo.channel_pay_uid;
        upData["order_id"] = orderInfo.order_id;
        upData["channel_trade_no"] = orderInfo.channel_trade_no;
        upData["server_id"] = orderInfo.server_id;
        upData["create_time"] = orderInfo.create_time;
        upData["pay_channel"] = orderInfo.pay_channel;
        upData["pay_time"] = orderInfo.pay_time;
        upData["game_id"] = orderInfo.game_id;
        upData["subject"] = orderInfo.subject;
        upData["pay_uid"] = orderInfo.pay_uid;
        upData["total_amount"] = orderInfo.total_amount;
        upData["notify_status"] = orderInfo.notify_status;
        upData["product_id"] = orderInfo.product_id;
        upData["pay_status"] = orderInfo.pay_status;
        return upData;
    }

    // 订单信息
    public class OrderInfo
    {
        public string body;
        public string channel_pay_uid;
        public string order_id;
        public string channel_trade_no;
        public string server_id;
        public string create_time;
        public string pay_channel;
        public string pay_time;
        public string game_id;
        public string subject;
        public string pay_uid;
        public string total_amount;
        public string notify_status;
        public string product_id;
        public string pay_status;
    }
}

/////////////////////////////////////////////////////////////////////////
// vivo支付回调
public class PayCallbackVIVO : PayCallbackBase
{
    protected static List<string> s_excludeKey = new List<string>(new string[] { "signMethod", "signature" });

    public string PayTableName { set; get; }

    public string VivoCpKey { set; get; }

    public static PayCallbackVIVO createVivoPay(HttpRequest Request)
    {
        // extInfo 透传参数传的是版本号(自定义的版本号)
        string version = Request.Form["extInfo"];
        if (string.IsNullOrEmpty(version))
        {
            version = "1";
        }

        switch (version)
        {
            case "1":
                {
                    return new PayCallbackVIVO();
                }
                break;
            case "2":
                {
                    return new PayCallbackVIVO_V2();
                }
                break;
            default:
                {
                    return new PayCallbackVIVO();
                }
                break;
        }
    }

    public override string notifyPay(object param)
    {
        HttpRequest Request = (HttpRequest)param;
        Dictionary<string, object> inputData = new Dictionary<string, object>();
        foreach (string key in Request.Form)
        {
            inputData[key] = Request.Form[key];
        }
        string codeStr = "success";
        try
        {
            if (!isPaySuccess(inputData))
            {
                codeStr = "error";
                return codeStr;
            }

            PayCheck check = new PayCheck();
            string wait = check.getVivoWaitSigned(inputData, s_excludeKey, VivoCpKey);
           // CLOG.Info(wait);
            string calSign = Helper.getMD5(wait);
            if (Convert.ToString(inputData["signature"]) != calSign)
            {
                codeStr = "errorSign";
                CLOG.Info("vivo..errorSign");
                return codeStr;
            }

            PayCallbackVIVO_Info payInfo = getPayInfo(inputData);
            //int rmb = getFactRmb(inputData);//(int)Convert.ToDouble(inputData["orderAmount"]);
            if (payInfo.m_rmb <= 0)
            {
                codeStr = "error";
                CLOG.Info("vivo..error amount");
                return codeStr;
            }

            PayInfoBase baseData = new PayInfoBase();
            baseData.m_orderId = payInfo.m_selfOrderId; //Convert.ToString(inputData["storeOrder"]);

            Dictionary<string, object> qd = PayBase.queryBaseData(baseData.m_orderId, MongodbPayment.Instance, PayCallbackBase.S_TOTAL_PAY_FIELDS);
            if (qd == null)
            {
                codeStr = "noOrderId";
                CLOG.Info("vivo..noOrderId:{0}", baseData.m_orderId);
                return codeStr;
            }

            int srcRMB = Convert.ToInt32(qd["RMB"]);
            if (payInfo.m_rmb != srcRMB)
            {
                codeStr = "rmberror";
                CLOG.Info("vivo..error rmb, 应为 {0}, 实为 {1}", srcRMB, payInfo.m_rmb);
                return codeStr;
            }

            baseData.m_payTime = DateTime.Now;
            baseData.m_payCode = Convert.ToString(qd["PayCode"]);
            baseData.m_playerId = Convert.ToInt32(qd["PlayerId"]);
            baseData.m_account = Convert.ToString(qd["Account"]);
            baseData.m_rmb = payInfo.m_rmb;
            baseData.m_channelNumber = Convert.ToString(qd["channel_number"]);

            changeFieldName(inputData);
            baseData.addBaseDataToDic(inputData);
            bool res = insertPayData(PayTableName, inputData, baseData);
            if (!res)
            {
                codeStr = "dberror";
                CLOG.Info("vivo..dberror");
            }
        }
        catch (Exception ex)
        {
            codeStr = "error";
            CLOG.Info("vivo..{0}", ex.ToString());
        }

        return codeStr;
    }

    // 是否支付成功了
    public virtual bool isPaySuccess(Dictionary<string, object> inputData)
    {
        if (Convert.ToString(inputData["respCode"]) != "0000")
        {
            return false;
        }
        return true;
    }

    public virtual PayCallbackVIVO_Info getPayInfo(Dictionary<string, object> inputData)
    {
        PayCallbackVIVO_Info info = new PayCallbackVIVO_Info();
        info.m_rmb = (int)Convert.ToDouble(inputData["orderAmount"]);
        info.m_selfOrderId = Convert.ToString(inputData["storeOrder"]);
        return info;
    }

    // 为了和版本1中的字段名称保持一致， 需要改字段名
    // vivo校报版支付回调中返回的字段名部分发生了改变
    public virtual void changeFieldName(Dictionary<string, object> inputData)
    {

    }
}

public class PayCallbackVIVO_Info
{
    public int m_rmb;
    public string m_selfOrderId;
}

public class PayCallbackVIVO_V2 : PayCallbackVIVO
{
    public override bool isPaySuccess(Dictionary<string, object> inputData)
    {
        if (Convert.ToString(inputData["tradeStatus"]) != "0000")
        {
            return false;
        }
        return true;
    }

    public override PayCallbackVIVO_Info getPayInfo(Dictionary<string, object> inputData)
    {
        PayCallbackVIVO_Info info = new PayCallbackVIVO_Info();
        int rmb = (int)Convert.ToDouble(inputData["orderAmount"]);
        info.m_rmb = rmb / 100; // 新版单位是分，转成元
        info.m_selfOrderId = Convert.ToString(inputData["cpOrderNumber"]);
        return info;
    }

    // 转发v1版本的字段名
    public override void changeFieldName(Dictionary<string, object> inputData)
    {
        if (inputData.ContainsKey("orderNumber"))
        {
            inputData.Add("vivoOrder", Convert.ToString(inputData["orderNumber"]));
            inputData.Remove("orderNumber");
        }
        if (inputData.ContainsKey("cpOrderNumber"))
        {
            inputData.Add("storeOrder", Convert.ToString(inputData["cpOrderNumber"]));
            inputData.Remove("cpOrderNumber");
        }
        if (inputData.ContainsKey("orderAmount"))
        {
            inputData["orderAmount"] = "" + Convert.ToInt32(inputData["orderAmount"]) / 100;
        }
        if (inputData.ContainsKey("cpId"))
        {
            inputData.Add("storeId", Convert.ToString(inputData["cpId"]));
            inputData.Remove("cpId");
        }
    }
}

/////////////////////////////////////////////////////////////////////////
// 百度移动游戏sdk(单机版)
public class PayCallbackBaidu2 : PayCallbackBase
{
    //public const string BAIDU2_SECRET_KEY = "5hT1aBW6g1knmhgSkp7d2PzvkS2qhMpG";
    static string[] s_waitKey = { "appid", "orderid", "amount", "unit", "status", "paychannel" };

    public string SecretKey { set; get; }

    public string PayTableName { set; get; }

    public override string notifyPay(object param)
    {
        HttpRequest Request = (HttpRequest)param;
        var req = Request.QueryString;
        Dictionary<string, object> inputData = new Dictionary<string, object>();

        foreach (string key in req)
        {
            inputData[key] = req[key];
        }

        string codeStr = "success";
        try
        {
            if (Convert.ToString(inputData["status"]) != "success")
            {
                codeStr = "error_status";
                CLOG.Info("baidu2...error_status");
                return codeStr;
            }

            string wait = getWaitSignStr(inputData);
            string calSign = Helper.getMD5(wait);
            if (Convert.ToString(inputData["sign"]) != calSign)
            {
                codeStr = "errorSign";
                CLOG.Info("baidu2...errorSign");
                return codeStr;
            }

            int rmb = 0;
            double amount = Convert.ToDouble(inputData["amount"]);
            string unit = Convert.ToString(inputData["unit"]);
            if (unit == "fen")
            {
                rmb = (int)(amount / 100);
            }
            else if (unit == "yuan")
            {
                rmb = (int)amount;
            }
            else
            {
                codeStr = "unit error";
                CLOG.Info("baidu2..unit error:{0}", unit);
                return codeStr;
            }
            if (rmb <= 0)
            {
                codeStr = "rmbError";
                CLOG.Info("baidu2..error rmb");
                return codeStr;
            }

            PayInfoBase baseData = new PayInfoBase();
            string playerId = Convert.ToString(inputData["cpdefinepart"]);
            bool res = getOrderId(playerId, ref baseData.m_orderId);
            if (!res)
            {
                codeStr = "noOrderId";
                CLOG.Info("baidu2..noPlayerId:{0}", playerId);
                return codeStr;
            }

            CheckRet checkCode = checkOrder(baseData.m_orderId, rmb, baseData);
            if (checkCode != CheckRet.checkRetSuccess)
            {
                if (checkCode == CheckRet.checkRetRmbError)
                {
                    CLOG.Info("baidu2..rmberror, need:{0}, fact:{1}", baseData.m_rmb, rmb);
                }

                codeStr = "checkOrder error";
                return codeStr;
            }
            /*Dictionary<string, object> qd = PayBase.queryBaseData(baseData.m_orderId, MongodbPayment.Instance, PayCallbackBase.S_TOTAL_PAY_FIELDS);
            if (qd == null)
            {
                codeStr = "noOrderId";
                CLOG.Info("baidu2..noOrderId:{0}", baseData.m_orderId);
                return codeStr;
            }

            baseData.m_payTime = DateTime.Now;
            baseData.m_payCode = Convert.ToString(qd["PayCode"]);
            baseData.m_playerId = Convert.ToInt32(qd["PlayerId"]);
            baseData.m_account = Convert.ToString(qd["Account"]);
            baseData.m_rmb = rmb;
            baseData.m_channelNumber = Convert.ToString(qd["channel_number"]);
            */
            baseData.addBaseDataToDic(inputData);
            res = insertPayData(PayTableName, inputData, baseData);
            if (!res)
            {
                codeStr = "dberror";
                CLOG.Info("baidu2..dberror");
            }
        }
        catch (Exception ex)
        {
            codeStr = "error";
            CLOG.Info("baidu2..{0}", ex.ToString());
        }

        return codeStr;
    }

    string getWaitSignStr(Dictionary<string, object> inputData)
    {
        StringBuilder sbuilder = new StringBuilder();
        for (int i = 0; i < s_waitKey.Length; i++)
        {
            if (inputData.ContainsKey(s_waitKey[i]))
            {
                sbuilder.Append(Convert.ToString(inputData[s_waitKey[i]]));
            }
        }

        sbuilder.Append(SecretKey);
        return sbuilder.ToString();
    }

    // 返回订单ID
    bool getOrderId(string playerId, ref string orderId)
    {
        Dictionary<string, object> ret =
            MongodbPayment.Instance.ExecuteGetOneBykey(PayTable.BAIDU2_TRANSITION, "playerId", playerId);
        if (ret == null)
            return false;

        if (!ret.ContainsKey("orderId"))
            return false;

        orderId = Convert.ToString(ret["orderId"]);
        MongodbPayment.Instance.ExecuteRemoveBykey(PayTable.BAIDU2_TRANSITION, "playerId", playerId);
        return true;
    }
}

/////////////////////////////////////////////////////////////////////////
// ysdk(直购模式)
public class PayCallbackYsdkDirect : PayCallbackBase
{
    // 沙箱
    public const string APP_KEY = "AbLpBtdKMUBPtqQ3unnDLuIfu8moR2Th";

    // 现网
    //public const string APP_KEY = "RJJJRZDq1co4EWuVRNNlQKZ3UQPCsaAJ";

    static List<string> s_excludeKey = new List<string>(new string[] { "sig", PayTable.YSDK_APATH_KEY });
    static char[] s_split = { '*' };
    public static string[] S_TOTAL_PAY_FIELDS_YSDK = { "PlayerId", "PayCode", "Account", "channel_number", "RMB" };

    public override string notifyPay(object param)
    {
        HttpRequest Request = (HttpRequest)param;
        var req = Request.QueryString;
        Dictionary<string, object> inputData = new Dictionary<string, object>();

        foreach (string key in req)
        {
            inputData[key] = req[key];
        }

        Dictionary<string, object> ret = new Dictionary<string, object>();
        try
        {
            do 
            {
                bool valid = isDataValid(inputData, Request);
                if (!valid)
                {
                    doRet(ret, 4, "sign error");
                    CLOG.Info("ysdk_direct sign error");
                    break;
                }

                if (inputData.ContainsKey(PayTable.YSDK_APATH_KEY))
                {
                    inputData.Remove(PayTable.YSDK_APATH_KEY);
                }

                PayInfoBase baseData = new PayInfoBase();
                baseData.m_orderId = getSelfOrderId(inputData);

                Dictionary<string, object> qd = PayBase.queryBaseData(baseData.m_orderId,
                    MongodbPayment.Instance, PayCallbackYsdkDirect.S_TOTAL_PAY_FIELDS_YSDK);
                if (qd == null)
                {
                    doRet(ret, 4, "no orderid");
                    CLOG.Info("ysdk_direct noOrderId:{0}", baseData.m_orderId);
                    break;
                }

                baseData.m_payTime = DateTime.Now;
                baseData.m_payCode = Convert.ToString(qd["PayCode"]);
                baseData.m_playerId = Convert.ToInt32(qd["PlayerId"]);
                baseData.m_account = Convert.ToString(qd["Account"]);
                baseData.m_rmb = Convert.ToInt32(qd["RMB"]);
                baseData.m_channelNumber = Convert.ToString(qd["channel_number"]);

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTable.YSDK_DIRECT_PAY, inputData, baseData);
                if (!res)
                {
                    doRet(ret, 4, "dberror");
                    CLOG.Info("ysdk_direct dberror");
                    break;
                }

                doRet(ret, 0, "OK");

            } while (false);
        }
        catch (Exception ex)
        {
            doRet(ret, 4, "ex");
            CLOG.Info("ysdk_direct {0}", ex.ToString());
        }

        return JsonHelper.genJson(ret);
    }

    bool isDataValid(Dictionary<string, object> inputData, HttpRequest Request)
    {
        string wait = getWaitSignStr(inputData, Request);
        return isSignValid(wait, inputData);
    }

    string getWaitSignStr(Dictionary<string, object> inputData, HttpRequest Request)
    {
        string url = "";
        // 相对路径
        if (inputData.ContainsKey(PayTable.YSDK_APATH_KEY))
        {
            url = Convert.ToString(inputData[PayTable.YSDK_APATH_KEY]);
        }
        else
        {
            url = Request.Url.AbsolutePath;
        }
        string eurl = urlEncode(url);

        string wasc = getWaitSignStrByAsc(inputData, s_excludeKey);
        string ewasc = urlEncode(wasc);

        string wait = string.Format("{0}&{1}&{2}", Request.HttpMethod, eurl, ewasc);
        return wait;
    }

    bool isSignValid(string wait, Dictionary<string, object> inputData)
    {
        string skey = APP_KEY + "&";
        string sign = Helper.SHA1Encrypt(wait, skey);
        string outsign = Convert.ToString(inputData["sig"]);
        return sign == outsign;
    }

    void doRet(Dictionary<string, object> ret, int code, string msg)
    {
        ret["ret"] = code;
        ret["msg"] = msg;
    }

    string getSelfOrderId(Dictionary<string, object> inputData)
    {
        string str = Convert.ToString(inputData["appmeta"]);
        string[] arr = str.Split(s_split, StringSplitOptions.RemoveEmptyEntries);
        return arr[0];
    }

    public string urlEncode(string str)
    {
        StringBuilder builder = new StringBuilder();
        foreach (char c in str)
        {
            if (HttpUtility.UrlEncode(c.ToString()).Length > 1)
            {
                builder.Append(HttpUtility.UrlEncode(c.ToString()).ToUpper());
            }
            else
            {
                builder.Append(c);
            }
        }
        return builder.ToString();
    }
}

/////////////////////////////////////////////////////////////////////////
// 乐视支付回调
public class PayCallbackLetv : PayCallbackBase
{
    const string SECRET_KEY = "0ad5cd142c1045beadcdfb59edb56ae6";
    static List<string> s_excludeKey = new List<string>(new string[] { "sign" });

    public override string notifyPay(object param)
    {
        HttpRequest Request = (HttpRequest)param;
        Dictionary<string, object> inputData = new Dictionary<string, object>();
        foreach (string key in Request.QueryString)
        {
            inputData[key] = Request.QueryString[key];
        }
        string codeStr = "success";
        try
        {
            string wait = getWaitSignStr(inputData);
            string calSign = Helper.getMD5(wait);
            if (Convert.ToString(inputData["sign"]) != calSign)
            {
                codeStr = "errorSign";
                CLOG.Info("letv..errorSign");
                return codeStr;
            }

            int rmb = (int)Convert.ToDouble(inputData["original_price"]);
            if (rmb <= 0)
            {
                codeStr = "error";
                CLOG.Info("letv..error amount");
                return codeStr;
            }

            PayInfoBase baseData = new PayInfoBase();
            baseData.m_orderId = Convert.ToString(inputData["cooperator_order_no"]);

            Dictionary<string, object> qd = PayBase.queryBaseData(baseData.m_orderId, MongodbPayment.Instance, PayCallbackBase.S_TOTAL_PAY_FIELDS);
            if (qd == null)
            {
                codeStr = "noOrderId";
                CLOG.Info("letv..noOrderId:{0}", baseData.m_orderId);
                return codeStr;
            }

            int srcRMB = Convert.ToInt32(qd["RMB"]);
            if (srcRMB != rmb)
            {
                codeStr = "rmb error";
                CLOG.Info("letv rmb error:{0}, need:{1}, fact:{2}", baseData.m_orderId, srcRMB, rmb);
                return codeStr;
            }
            baseData.m_payTime = DateTime.Now;
            baseData.m_payCode = Convert.ToString(qd["PayCode"]);
            baseData.m_playerId = Convert.ToInt32(qd["PlayerId"]);
            baseData.m_account = Convert.ToString(qd["Account"]);
            baseData.m_rmb = rmb;
            baseData.m_channelNumber = Convert.ToString(qd["channel_number"]);

            baseData.addBaseDataToDic(inputData);
            bool res = insertPayData(PayTable.LETV_PAY, inputData, baseData);
            if (!res)
            {
                codeStr = "dberror";
                CLOG.Info("letv..dberror");
            }
        }
        catch (Exception ex)
        {
            codeStr = "error";
            CLOG.Info("letv..{0}", ex.ToString());
        }

        return codeStr;
    }

    string getWaitSignStr(Dictionary<string, object> inputData)
    {
        string str = "";
        str = getWaitSignStrByAsc(inputData, s_excludeKey);
        str += "&key=" + SECRET_KEY;
        return str;
    }
}

/////////////////////////////////////////////////////////////////////////
// 微信小程序支付回调
public class PayCallbackWeixinMini : PayCallbackBase
{
    public string ApiSecret { set; get; }

    public string PayTableName { set; get; }

    static List<string> s_excludeKey = new List<string>(new string[] { "sign" });

    public override string notifyPay(object param)
    {
        HttpRequest Request = (HttpRequest)param;
        byte[] byteArr = new byte[Request.InputStream.Length];
        Request.InputStream.Read(byteArr, 0, byteArr.Length);
        string byteStr = Encoding.UTF8.GetString(byteArr);
        XmlGen xml = new XmlGen();
        Dictionary<string, object> inputData = xml.fromXmlString("xml", byteStr);

        Dictionary<string, object> ret = new Dictionary<string, object>();
        string retCode = "SUCCESS";
        do 
        {
            try
            {
                string resstr = Convert.ToString(inputData["return_code"]);
                if (resstr != "SUCCESS")
                {
                    CLOG.Info("weixinMini result is {0}", resstr);
                    retCode = "error";
                    break;
                }

                PayCheck chk = new PayCheck();
                string wait = chk.getWeiXinWaitSigned(inputData, s_excludeKey, ApiSecret);
                string calSign = Helper.getMD5Upper(wait);
                if (Convert.ToString(inputData["sign"]) != calSign)
                {
                    retCode = "errorSign";
                    CLOG.Info("weixinMini..errorSign");
                    break;
                }

                int rmb = (int)Convert.ToDouble(inputData["total_fee"]);
                
                PayInfoBase baseData = new PayInfoBase();
                baseData.m_orderId = Convert.ToString(inputData["out_trade_no"]);

                Dictionary<string, object> qd = PayBase.queryBaseData(baseData.m_orderId, MongodbPayment.Instance, PayCallbackBase.S_TOTAL_PAY_FIELDS);
                if (qd == null)
                {
                    retCode = "noOrderId";
                    CLOG.Info("weixinMini..noOrderId:{0}", baseData.m_orderId);
                    break;
                }

                int srcrmb = Convert.ToInt32(qd["RMB"]);
                if (rmb != srcrmb * 100)
                {
                    retCode = "rmb error";
                    string err = string.Format("weixin..rmb error:应为{0},实为{1}", (srcrmb * 100), rmb);
                    CLOG.Info(err);
                    break;
                }

                baseData.m_payTime = DateTime.Now;
                baseData.m_payCode = Convert.ToString(qd["PayCode"]);
                baseData.m_playerId = Convert.ToInt32(qd["PlayerId"]);
                baseData.m_account = Convert.ToString(qd["Account"]);
                baseData.m_rmb = rmb / 100; // 单位分，需转成元
                baseData.m_channelNumber = Convert.ToString(qd["channel_number"]);

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTableName, inputData, baseData);
                if (!res)
                {
                    retCode = "dberror";
                    CLOG.Info("weixinMini..dberror");
                }
            }
            catch (Exception ex)
            {
                retCode = "error";
                CLOG.Info("weixinMini..{0}", ex.ToString());
            }

        } while (false);

        ret.Add("return_code", retCode);
        if (retCode == "SUCCESS")
        {
            ret.Add("return_msg", "OK");
        }
        else
        {
            ret.Add("return_msg", retCode);
        }
        return xml.genXML("xml", ret);
    }
}

/////////////////////////////////////////////////////////////////////////
// 支付付回调
public class PayCallbackALi : PayCallbackBase
{
    public static string PUBLIC_KEY = "<RSAKeyValue><Modulus>wyOnd9OkPHyHwjmk18olHiR5KyL2CEkXHO97N7xdWU+1zfETT8CcaPKVmiTJr8k5/TeB0Ot9qiQVR3/LMR4v3xLK2qa8Arc780BE6zUcFcSzuY1ZkRoutI/FHtXnyJcz11fY3vZpI8zpnbLMpmZIdkIVKwpmWCPxQhGt/3DTFtE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
    static List<string> s_excludeKey = new List<string>(new string[] { "sign", "sign_type" });

    public override string notifyPay(object param)
    {
        HttpRequest Request = (HttpRequest)param;
        Dictionary<string, object> inputData = new Dictionary<string, object>();
        foreach (string key in Request.Form)
        {
            inputData[key] = Request.Form[key];
        }

//         foreach (string key in Request.QueryString)
//         {
//             inputData[key] = Request.QueryString[key];
//         }
//         PayCheck chk = new PayCheck();
//         string tmp = chk.getWaitSignStrByAsc(inputData);
//         CLOG.Info(tmp);

        string codeStr = "success";
        do 
        {
            try
            {
                string status = Convert.ToString(inputData["trade_status"]);
                if (status != "TRADE_SUCCESS")
                {
                    codeStr = "stateError";
                    break;
                }

                string wait = getWaitSignStr(inputData);
                bool checkRes = checkSignByRSAPublicKey(wait, Convert.ToString(inputData["sign"]), PUBLIC_KEY);
                if (!checkRes)
                {
                    codeStr = "errorSign";
                    CLOG.Info("ali..errorSign");
                    break;
                }

                PayInfoBase baseData = new PayInfoBase();
                baseData.m_orderId = Convert.ToString(inputData["out_trade_no"]);

                Dictionary<string, object> qd = PayBase.queryBaseData(baseData.m_orderId, MongodbPayment.Instance, PayCallbackBase.S_TOTAL_PAY_FIELDS);
                if (qd == null)
                {
                    codeStr = "noOrderId";
                    CLOG.Info("ali..noOrderId:{0}", baseData.m_orderId);
                    break;
                }

                int inrmb = (int)Convert.ToDouble(inputData["total_amount"]);
                int srcrmb = Convert.ToInt32(qd["RMB"]);
                if (inrmb != srcrmb)
                {
                    codeStr = "rmb error";
                    CLOG.Info("ali..rmb error:应为{0}, 实为{1}", srcrmb, inrmb);
                    break;
                }

                baseData.m_payTime = DateTime.Now;
                baseData.m_payCode = Convert.ToString(qd["PayCode"]);
                baseData.m_playerId = Convert.ToInt32(qd["PlayerId"]);
                baseData.m_account = Convert.ToString(qd["Account"]);
                baseData.m_rmb = srcrmb;
                baseData.m_channelNumber = Convert.ToString(qd["channel_number"]);

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTable.ALI_PAY, inputData, baseData);
                if (!res)
                {
                    codeStr = "dberror";
                    CLOG.Info("ali..dberror");
                }
            }
            catch (Exception ex)
            {
                codeStr = "error";
                CLOG.Info("ali..{0}", ex.ToString());
            }
        } while (false);
       
        return codeStr;
    }

    string getWaitSignStr(Dictionary<string, object> inputData)
    {
        string str = "";
        str = getWaitSignStrByAsc(inputData, s_excludeKey);
        return str;
    }
}

/////////////////////////////////////////////////////////////////////////
// 魅族回调(单机版)
public class PayCallbacMeizu : PayCallbackBase
{
    const string RET_EX = "900000";

    static List<string> s_excludeKey = new List<string>(new string[] { "sign", "sign_type" });

    public string AppSecret { set; get; }
    public string PayTableName { set; get; }

    public override string notifyPay(object param)
    {
        HttpRequest Request = (HttpRequest)param;
        Dictionary<string, object> inputData = new Dictionary<string, object>();
        foreach (string key in Request.Form)
        {
            inputData[key] = Request.Form[key];
        }

        PayCheck check = new PayCheck();

//         string ss = check.getMeizuSingleWaitSignStr(inputData, null);
//         CLOG.Info("原:" +ss);

        Dictionary<string, object> retData = new Dictionary<string, object>();

        do
        {
            try
            {
                if (Convert.ToString(inputData["trade_status"]) != "3")
                {
                    retData.Add("code", RET_EX);
                    break;
                }

                string wait = check.getMeizuSingleWaitSignStr(inputData, s_excludeKey, AppSecret);
               // CLOG.Info("待" + wait);
                string calSign = Helper.getMD5(wait);
                if (Convert.ToString(inputData["sign"]) != calSign)
                {
                    retData.Add("code", RET_EX);
                    CLOG.Info("meizu..errorSign");
                    break;
                }

                int rmb = (int)Convert.ToDouble(inputData["total_price"]);

                PayInfoBase baseData = new PayInfoBase();
                baseData.m_orderId = Convert.ToString(inputData["cp_order_id"]);

                Dictionary<string, object> qd = PayBase.queryBaseData(baseData.m_orderId, MongodbPayment.Instance, PayCallbackBase.S_TOTAL_PAY_FIELDS);
                if (qd == null)
                {
                    retData.Add("code", RET_EX);
                    CLOG.Info("meizu..noOrderId:{0}", baseData.m_orderId);
                    break;
                }

                int srcRMB = Convert.ToInt32(qd["RMB"]);
                if (srcRMB != rmb)
                {
                    retData.Add("code", RET_EX);
                    CLOG.Info("meizu..error amount, {0}", rmb.ToString());
                    break;
                }

                baseData.m_payTime = DateTime.Now;
                baseData.m_payCode = Convert.ToString(qd["PayCode"]);
                baseData.m_playerId = Convert.ToInt32(qd["PlayerId"]);
                baseData.m_account = Convert.ToString(qd["Account"]);
                baseData.m_rmb = rmb;
                baseData.m_channelNumber = Convert.ToString(qd["channel_number"]);

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTableName, inputData, baseData);
                if (!res)
                {
                    retData.Add("code", RET_EX);
                    CLOG.Info("meizu..dberror");
                    break;
                }

                retData.Add("code", "200");
            }
            catch (Exception ex)
            {
                retData.Add("code", RET_EX);
                CLOG.Info("meizu..{0}", ex.ToString());
            }

        } while (false);
        
        return JsonHelper.genJson(retData);
    }
}


/////////////////////////////////////////////////////////////////////////
// 小米回调(游戏名：电玩城捕鱼)
public class PayCallbacXiaoMi : PayCallbackBase
{
    const string RET_SIGN_ERROR = "1525";  // 签名错误
    const string RET_SUCCESS = "200";
    //const string APPSECRET = "6/Yfvi0OMhIE0sWB50X5rQ==";

    static List<string> s_excludeKey = new List<string>(new string[] { "signature" });

    public string AppSecret { set; get; }
    public string PayTableName { set; get; }

    public override string notifyPay(object param)
    {
        HttpRequest Request = (HttpRequest)param;
        Dictionary<string, object> inputData = new Dictionary<string, object>();
        foreach (string key in Request.QueryString)
        {
            inputData[key] = Request.QueryString[key];
        }

        Dictionary<string, object> retData = new Dictionary<string, object>();

        do
        {
            try
            {
                if (Convert.ToString(inputData["orderStatus"]) != "TRADE_SUCCESS")
                {
                    retData.Add("errcode", RET_SIGN_ERROR);
                    break;
                }

                PayCheck chk = new PayCheck();
                string wait = chk.getWaitSignStrByAsc2(inputData, s_excludeKey);
                string calSign = Helper.SHA1EncryptBy16Hex(wait, AppSecret);
                if (Convert.ToString(inputData["signature"]) != calSign)
                {
                    retData.Add("errcode", RET_SIGN_ERROR);
                    CLOG.Info("xiaomi..errorSign");
                    break;
                }

                int rmbfen = (int)Convert.ToDouble(inputData["payFee"]);
                int rmbyuan = rmbfen / 100;

                string selfOrderId = Convert.ToString(inputData["cpOrderId"]);

                PayInfoBase baseData = new PayInfoBase();
                CheckRet checkCode = checkOrder(selfOrderId, rmbyuan, baseData);
                if (checkCode != CheckRet.checkRetSuccess)
                {
                    if (checkCode == CheckRet.checkRetRmbError)
                    {
                        CLOG.Info("xiaomi, rmberror, need {0}, fact {1}", baseData.m_rmb, rmbyuan);
                    }

                    retData.Add("errcode", RET_SIGN_ERROR);
                    break;
                }

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTableName, inputData, baseData);
                if (!res)
                {
                    retData.Add("errcode", RET_SIGN_ERROR);
                    CLOG.Info("xiaomi..dberror");
                    break;
                }

                retData.Add("errcode", RET_SUCCESS);
            }
            catch (Exception ex)
            {
                retData.Add("errcode", RET_SIGN_ERROR);
                CLOG.Info("xiaomi..{0}", ex.ToString());
            }

        } while (false);

        return JsonHelper.genJson(retData);
    }
}

/////////////////////////////////////////////////////////////////////////
// 游戏Fan
public class PayCallbacGameFan : PayCallbackBase
{
    static string[] s_signfield = { "orderid", "username", "gameid", "roleid", "serverid", "paytype", "amount", "paytime", "attach" };
    const string APP_KEY = "bd3bb71204f976c413d7e038b39cd0df";

    const string RET_ERROR_SIGN = "errorSign";
    const string RET_SUCCESS = "success";

    public override string notifyPay(object param)
    {
        HttpRequest Request = (HttpRequest)param;
        Dictionary<string, object> inputData = new Dictionary<string, object>();
        foreach (string key in Request.Form)
        {
            inputData[key] = Request.Form[key];
        }

        string retStr = "";

        do
        {
            try
            {
                string wait = getWaitSign(inputData);
                string calSign = Helper.getMD5(wait);
                if (Convert.ToString(inputData["sign"]) != calSign)
                {
                    retStr = RET_ERROR_SIGN;
                    CLOG.Info("gamefan..errorSign");
                    break;
                }

                int rmb = (int)Convert.ToDouble(inputData["amount"]);

                string selfOrderId = Convert.ToString(inputData["attach"]);

                PayInfoBase baseData = new PayInfoBase();
                CheckRet checkCode = checkOrder(selfOrderId, rmb, baseData);
                if (checkCode != CheckRet.checkRetSuccess)
                {
                    if (checkCode == CheckRet.checkRetRmbError) // 调试用
                    {
                        CLOG.Info("gamefan..rmberror");
                        retStr = RET_ERROR_SIGN;
                        break;
                    }
                    else
                    {
                        retStr = RET_ERROR_SIGN;
                        break;
                    }
                }

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTable.FAN_PAY, inputData, baseData);
                if (!res)
                {
                    retStr = RET_ERROR_SIGN;
                    CLOG.Info("gamefan..dberror");
                    break;
                }

                retStr = RET_SUCCESS;
            }
            catch (Exception ex)
            {
                retStr = RET_ERROR_SIGN;
                CLOG.Info("gamefan..{0}", ex.ToString());
            }

        } while (false);

        return retStr;
    }

    string getWaitSign(Dictionary<string, object> inputData)
    {
        StringBuilder builder = new StringBuilder();

        bool first = true;
        object value = null;

        for (int i = 0; i < s_signfield.Length; i++)
        {
            if (inputData.ContainsKey(s_signfield[i]))
            {
                value = inputData[s_signfield[i]];
            }
            else
            {
                value = "";
            }

            if (first)
            {
                first = false;
                builder.AppendFormat("{0}={1}", s_signfield[i], value);
            }
            else
            {
                builder.AppendFormat("&{0}={1}", s_signfield[i], value);
            }
        }

        builder.AppendFormat("&appkey={0}", APP_KEY);
        return builder.ToString();
    }
}

/////////////////////////////////////////////////////////////////////////
public class AibeiParam
{
    public int transtype;
    public string cporderid;
    public string transid;
    public string appuserid;
    public string appid;
    public int waresid;
    public int feetype;
    public float money;
    public string currency;
    public int result;
    public string transtime;
    public string cpprivate;
    public int paytype;
}

// 爱贝支付回调
public class PayCallbacAiBei : PayCallbackBase
{
    public string PublicKey { set; get; }

    public override string notifyPay(object param)
    {
        string retStr = "SUCCESS";

        do
        {
            try
            {
                HttpRequest Request = (HttpRequest)param;

                byte[] byteArr = new byte[Request.InputStream.Length];
                Request.InputStream.Read(byteArr, 0, byteArr.Length);
                string byteStr = Encoding.Default.GetString(byteArr);
                CLOG.Info(byteStr);
                if (string.IsNullOrEmpty(byteStr))
                {
                    string s1 = "";
                    if (Request.InputStream.Length > 0)
                    {
                        s1 = Encoding.UTF8.GetString(byteArr);
                    }

                    CLOG.Info("输入串空, length:{0}, str: {1}", Request.InputStream.Length, s1);
                }

                NameValueCollection nvc = HttpUtility.ParseQueryString(byteStr);
                Dictionary<string, object> inputData = new Dictionary<string, object>();
                inputData.Add("transdata", nvc["transdata"]);
                inputData.Add("sign", nvc["sign"]);
                inputData.Add("signtype", nvc["signtype"]);

                string transdata = Convert.ToString(inputData["transdata"]);
                AibeiParam aibeiParam = JsonHelper.ParseFromStr<AibeiParam>(transdata);
                string sign = Convert.ToString(inputData["sign"]);
                bool checkRes = checkSignByRSAMd5PublicKey(transdata, sign, PublicKey);
                if (!checkRes)
                {
                    retStr = "sign_error";
                    CLOG.Info("aibei, sign error");
                    break;
                }

                int rmb = (int)aibeiParam.money;

                PayInfoBase baseData = new PayInfoBase();
                CheckRet checkCode = checkOrder(aibeiParam.cporderid, rmb, baseData);
                if (checkCode != CheckRet.checkRetSuccess)
                {
                    if (checkCode == CheckRet.checkRetRmbError)
                    {
                        CLOG.Info("aibei..rmberror");
                        retStr = "sign_error";
                    }
                    else if (checkCode == CheckRet.checkRetNoOrder)
                    {
                        retStr = "noorder";
                        CLOG.Info("aibei..noorder:{0}", aibeiParam.cporderid);
                    }
                    break;
                }

                Dictionary<string, object> otherData = JsonHelper.ParseFromStr<Dictionary<string, object>>(transdata);
                baseData.addBaseDataToDic(otherData);
                bool res = insertPayData(PayTable.AIBEI_PAY, otherData, baseData);
                if (!res)
                {
                    retStr = "sign_error";
                    CLOG.Info("aibei..dberror");
                    break;
                }
            }
            catch (Exception ex)
            {
                retStr = "sign_error";
                CLOG.Info("aibei..{0}", ex.ToString());
            }

        } while (false);

        return retStr;
    }
}

//////////////////////////////////////////////////////////////////////////
// 清源(IOS)支付回调
public class PayCallbackQingYuan : PayCallbackBase
{
    static List<string> s_excludeKey = new List<string>(new string[] { "sign" });

    public string PublicKey { set; get; }

    public override string notifyPay(object param)
    {
        string retStr = "SUCCESS";

        do
        {
            try
            {
                HttpRequest Request = (HttpRequest)param;
                Dictionary<string, object> inputData = new Dictionary<string, object>();
                foreach (string key in Request.Form)
                {
                    inputData[key] = Request.Form[key];
                }

                //////////////////////////test////////////////////////////////////////////////
               // string sss = getWaitSignStrByAsc(inputData);
               // CLOG.Info(sss);
                //////////////////////////////////////////////////////////////////////////

                if (Convert.ToInt32(inputData["status"]) != 5) // 不为5为错误订单
                {
                    break;
                }

                string waitStr = getWaitSignStrByAsc(inputData, s_excludeKey);
                string sign = Convert.ToString(inputData["sign"]);
                bool checkRes = checkSignByRSAPublicKey(waitStr, sign, PublicKey);
                if (!checkRes)
                {
                    retStr = "sign_error";
                    CLOG.Info("QingYuan, sign error");
                    break;
                }

                int rmb = (int)Convert.ToDouble(inputData["price"]);
                string selfOrderId = Convert.ToString(inputData["orderid"]);
                PayInfoBase baseData = new PayInfoBase();
                CheckRet checkCode = checkOrder(selfOrderId, rmb, baseData);
                if (checkCode != CheckRet.checkRetSuccess)
                {
                    if (checkCode == CheckRet.checkRetRmbError)
                    {
                        CLOG.Info("QingYuan..rmberror");
                        retStr = "sign_error";
                        break;
                    }
                    else if (checkCode == CheckRet.checkRetNoOrder)
                    {
                        retStr = "noorder";
                        CLOG.Info("QingYuan..noorder:{0}", selfOrderId);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTable.QINGYUAN_PAY, inputData, baseData);
                if (!res)
                {
                    retStr = "sign_error";
                    CLOG.Info("QingYuan..dberror");
                    break;
                }
            }
            catch (Exception ex)
            {
                retStr = "sign_error";
                CLOG.Info("QingYuan..{0}", ex.ToString());
            }

        } while (false);

        return retStr;
    }
}

//////////////////////////////////////////////////////////////////////////
// 天天互娱支付回调
public class PayCallbackTianTianhy : PayCallbackBase
{
    public string AppKey { set; get; }
    public string PayTableName { set; get; }

    public override string notifyPay(object param)
    {
        string retStr = "1";

        do
        {
            try
            {
                HttpRequest Request = (HttpRequest)param;
                Dictionary<string, object> inputData = new Dictionary<string, object>();
                foreach (string key in Request.Form)
                {
                    inputData[key] = Request.Form[key];
                }

                //////////////////////////test////////////////////////////////////////////////
                //string sss = getWaitSignStrByAsc(inputData);
                //CLOG.Info(sss);
                //////////////////////////////////////////////////////////////////////////

                string waitStr = getWaitSign(inputData);
                string sign = Convert.ToString(inputData["sign"]);
               // CLOG.Info(waitStr + "----------" + sign);
                bool checkRes = Helper.getMD5(waitStr) == sign;
                if (!checkRes)
                {
                    retStr = "0";
                    CLOG.Info("tthy, sign error");
                    break;
                }

                int rmb = (int)Convert.ToDouble(inputData["price"]);
                string selfOrderId = Convert.ToString(inputData["extend"]);
                PayInfoBase baseData = new PayInfoBase();
                CheckRet checkCode = checkOrder(selfOrderId, rmb, baseData);
                if (checkCode != CheckRet.checkRetSuccess)
                {
                    if (checkCode == CheckRet.checkRetRmbError)
                    {
                        CLOG.Info("tthy..rmberror");
                        retStr = "0";
                        break;
                    }
                    else if (checkCode == CheckRet.checkRetNoOrder)
                    {
                        retStr = "0";
                        CLOG.Info("tthy..noorder:{0}", selfOrderId);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTableName, inputData, baseData);
                if (!res)
                {
                    retStr = "0";
                    CLOG.Info("tthy..dberror");
                    break;
                }
            }
            catch (Exception ex)
            {
                retStr = "0";
                CLOG.Info("tthy..{0}", ex.ToString());
            }

        } while (false);

        return retStr;
    }

    string getWaitSign(Dictionary<string, object> inputData)
    {
        StringBuilder build = new StringBuilder();
        build.Append(Convert.ToString(inputData["game_id"]));
        build.Append(Convert.ToString(inputData["out_trade_no"]));
        build.Append(Convert.ToString(inputData["price"]));
        build.Append(Convert.ToString(inputData["extend"]));
        build.Append(AppKey);
        return build.ToString();
    }
}

//////////////////////////////////////////////////////////////////////////
// oppo支付回调
public class PayCallbackOppo : PayCallbackBase
{
    static List<string> s_excludeKey = new List<string>(new string[] { "sign", "userId" });

    const string OK = "OK";
    const string FAIL = "FAIL";
    string m_fmt = "result={0}&resultMsg={1}";

    public string PublicKey { set; get; }
    public string PayTableName { set; get; }

    public override string notifyPay(object param)
    {
        string retStr = string.Format(m_fmt, OK, "ok");

        do
        {
            try
            {
                HttpRequest Request = (HttpRequest)param;
                Dictionary<string, object> inputData = new Dictionary<string, object>();
                foreach (string key in Request.Form)
                {
                    inputData[key] = Request.Form[key];
                }
               /* if(!inputData.ContainsKey("paymentWay"))
                {
                    inputData.Add("paymentWay", "");
                }
                if(!inputData.ContainsKey("attach"))
                {
                    inputData.Add("attach", "");
                }*/

                PayCheck chk = new PayCheck();
                string waitStr = chk.getWaitSignStrByAsc2(inputData, s_excludeKey, (k,v)=> { return true; });

                //////////////////////////test////////////////////////////////////////////////
                CLOG.Info(waitStr);
                //////////////////////////////////////////////////////////////////////////

                string sign = Convert.ToString(inputData["sign"]);
                bool checkRes = checkSignByRSA256PublicKey(waitStr, sign, PublicKey);
                if (!checkRes)
                {
                   // checkRes= checkSignByRSAPublicKey(waitStr, sign, PublicKey);
                   // checkRes = checkSignByRSAMd5PublicKey(waitStr, sign, PublicKey);
                    retStr = string.Format(m_fmt, FAIL, "sign_error");
                    CLOG.Info("oppo, sign error, need {0}", sign);
                    break;
                }

                int rmb = (int)Convert.ToDouble(inputData["price"]) / 100;
                string selfOrderId = Convert.ToString(inputData["partnerOrder"]);
                PayInfoBase baseData = new PayInfoBase();
                CheckRet checkCode = checkOrder(selfOrderId, rmb, baseData);
                if (checkCode != CheckRet.checkRetSuccess)
                {
                    if (checkCode == CheckRet.checkRetRmbError)
                    {
                        CLOG.Info("oppo..rmberror");
                        retStr = string.Format(m_fmt, FAIL, "rmberror");
                        break;
                    }
                    else if (checkCode == CheckRet.checkRetNoOrder)
                    {
                        retStr = string.Format(m_fmt, FAIL, "noorder");
                        CLOG.Info("oppo..noorder:{0}", selfOrderId);
                        break;
                    }
                    else
                    {
                        retStr = string.Format(m_fmt, FAIL, "other error");
                        CLOG.Info("oppo..other error");
                        break;
                    }
                }

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTableName, inputData, baseData);
                if (!res)
                {
                    retStr = string.Format(m_fmt, FAIL, "dberror");
                    CLOG.Info("oppo..dberror");
                    break;
                }
            }
            catch (Exception ex)
            {
                retStr = string.Format(m_fmt, FAIL, ex.ToString());
                CLOG.Info("oppo..{0}", ex.ToString());
            }

        } while (false);

        return retStr;
    }

    string getWaitSign(Dictionary<string, object> inputData)
    {
        StringBuilder build = new StringBuilder();
        build.AppendFormat("notifyId={0}", Convert.ToString(inputData["notifyId"]));
        build.AppendFormat("&partnerOrder={0}", Convert.ToString(inputData["partnerOrder"]));
        build.AppendFormat("&productName={0}", Convert.ToString(inputData["productName"]));
        build.AppendFormat("&productDesc={0}", Convert.ToString(inputData["productDesc"]));
        build.AppendFormat("&price={0}", Convert.ToString(inputData["price"]));
        build.AppendFormat("&count={0}", Convert.ToString(inputData["count"]));
        build.AppendFormat("&attach={0}", Convert.ToString(inputData["attach"]));
        return build.ToString();
    }
}

//////////////////////////////////////////////////////////////////////////
// 易接支付回调
public class PayCallbackYiJie : PayCallbackBase
{
    static List<string> s_excludeKey = new List<string>(new string[] { "sign" });

    public string ShareKey { set; get; }

    public override string notifyPay(object param)
    {
        string retStr = "SUCCESS";

        do
        {
            try
            {
                HttpRequest Request = (HttpRequest)param;
                Dictionary<string, object> inputData = new Dictionary<string, object>();
                foreach (string key in Request.QueryString)
                {
                    inputData[key] = Request.QueryString[key];
                }

                if (inputData.Count <= 0)
                {
                    foreach (string key in Request.Form)
                    {
                        inputData[key] = Request.Form[key];
                    }
                }

                string waitStr = getWaitSignStrByAsc(inputData, s_excludeKey);

                //////////////////////////test////////////////////////////////////////////////
                // string sss = getWaitSignStrByAsc(inputData);
                //CLOG.Info(waitStr);
                //////////////////////////////////////////////////////////////////////////

                string sign = Convert.ToString(inputData["sign"]);
                string factSign = Helper.getMD5(waitStr + ShareKey);
                bool checkRes = (sign == factSign);
                if (!checkRes)
                {
                    retStr = "sign_error";
                    CLOG.Info("yijie, sign error");
                    break;
                }

                int rmb = (int)Convert.ToDouble(inputData["fee"]) / 100;
                string selfOrderId = Convert.ToString(inputData["cbi"]);
                PayInfoBase baseData = new PayInfoBase();
                CheckRet checkCode = checkOrder(selfOrderId, rmb, baseData);
                if (checkCode != CheckRet.checkRetSuccess)
                {
                    if (checkCode == CheckRet.checkRetRmbError)
                    {
                        retStr = "rmberror";
                        CLOG.Info("yijie..rmberror");
                        break;
                    }
                    else if (checkCode == CheckRet.checkRetNoOrder)
                    {
                        retStr = "noorder";
                        CLOG.Info("yijie..noorder:{0}", selfOrderId);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTable.YIJIE_PAY, inputData, baseData);
                if (!res)
                {
                    retStr = "dberror";
                    CLOG.Info("yijie..dberror");
                    break;
                }
            }
            catch (Exception ex)
            {
                retStr = "happend ex";
                CLOG.Info("yijie..{0}", ex.ToString());
            }

        } while (false);

        return retStr;
    }
}

//////////////////////////////////////////////////////////////////////////
public class PayCallbackHospital : PayCallbackBase
{
    const int OK = 1;
    const int FAIL = 0;

    public string AppSecret { set; get; }
    public string PayTableName { set; get; }

    public override string notifyPay(object param)
    {
        Dictionary<string, object> ret = new Dictionary<string, object>();
        
        do
        {
            try
            {
                HttpRequest Request = (HttpRequest)param;
                Dictionary<string, object> inputData = new Dictionary<string, object>();

                byte[] byteArr = new byte[Request.InputStream.Length];
                Request.InputStream.Read(byteArr, 0, byteArr.Length);
                string byteStr = Encoding.Default.GetString(byteArr);
                inputData = JsonHelper.ParseFromStr<Dictionary<string, object>>(byteStr);

                if (inputData.Count <= 0)
                {
                    foreach (string key in Request.QueryString)
                    {
                        inputData[key] = Request.QueryString[key];
                    }
                    if (inputData.Count <= 0)
                    {
                        foreach (string key in Request.Form)
                        {
                            inputData[key] = Request.Form[key];
                        }
                    }
                }

                if (Convert.ToInt32(inputData["payState"]) != 91)
                {
                    addResult(ret, FAIL);
                    CLOG.Info("hospital, pay fail");
                    break;
                }

                string waitStr = getWaitSign(inputData);
                string selfSign = Helper.getMD5Upper(waitStr);
                string midsign = selfSign.Substring(8, 16);
                string sign = Convert.ToString(inputData["sign"]);

                //////////////////////////test////////////////////////////////////////////////
                //CLOG.Info(waitStr + "  sign=" + sign + "  selfSign=" + selfSign + " midsign=" + midsign);
                //////////////////////////////////////////////////////////////////////////

                if (midsign != sign)
                {
                    addResult(ret, FAIL);
                    CLOG.Info("hospital, sign error");
                    break;
                }

                int rmb = 100; // 由于回调未传金额，不判断rmb
                string selfOrderId = Convert.ToString(inputData["orderOuterNo"]);
                PayInfoBase baseData = new PayInfoBase();
                CheckRet checkCode = checkOrder(selfOrderId, rmb, baseData);
                if (checkCode != CheckRet.checkRetSuccess)
                {
                    if (checkCode == CheckRet.checkRetRmbError)
                    {
                    }
                    else if (checkCode == CheckRet.checkRetNoOrder)
                    {
                        addResult(ret, FAIL);
                        CLOG.Info("hospital..noorder:{0}", selfOrderId);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTableName, inputData, baseData);
                if (!res)
                {
                    addResult(ret, FAIL);
                    CLOG.Info("hospital..dberror");
                    break;
                }
                else
                {
                    addResult(ret, OK);
                }
            }
            catch (Exception ex)
            {
                addResult(ret, FAIL);
                CLOG.Info("hospital..{0}", ex.ToString());
            }

        } while (false);

        return JsonHelper.genJson(ret);
    }

    string getWaitSign(Dictionary<string, object> inputData)
    {
        StringBuilder build = new StringBuilder();
        build.AppendFormat("orderNo={0}", Convert.ToString(inputData["orderNo"]));
        build.AppendFormat("&orderOuterNo={0}", Convert.ToString(inputData["orderOuterNo"]));
        build.AppendFormat("&payState={0}", Convert.ToString(inputData["payState"]));
        build.AppendFormat("&payTime={0}", Convert.ToString(inputData["payTime"]));
        build.AppendFormat("&payType={0}", Convert.ToString(inputData["payType"]));
        build.AppendFormat("&key={0}", AppSecret);
        
        return build.ToString();
    }

    void addResult(Dictionary<string, object> ret, int result)
    {
        ret.Add("result", result);
        ret.Add("msg", "");
        ret.Add("data", "");
    }
}

//////////////////////////////////////////////////////////////////////////
// uc支付回调
public class PayCallbackUC : PayCallbackBase
{
    public string ApiKey { set; get; }

    public const string RET_SUCCESS = "SUCCESS";
    public const string RET_FAIL = "FAILURE";

    public override string notifyPay(object param)
    {
        string retStr = RET_SUCCESS;

        do
        {
            try
            {
                HttpRequest Request = (HttpRequest)param;
                Dictionary<string, object> rechargeData = new Dictionary<string, object>();
                
                byte[] byteArr = new byte[Request.InputStream.Length];
                Request.InputStream.Read(byteArr, 0, byteArr.Length);
                string byteStr = Encoding.UTF8.GetString(byteArr);
               // CLOG.Info(byteStr);
                rechargeData = JsonHelper.ParseFromStr<Dictionary<string, object>>(byteStr);

                string strData = Convert.ToString(rechargeData["data"]);
                //CLOG.Info(strData);
                Dictionary<string, object> inputData = JsonHelper.ParseFromStr<Dictionary<string, object>>(strData);
                if (Convert.ToString(inputData["orderStatus"]) != "S")
                {
                    break;
                }

                string waitStr = getWaitSignStrByAsc2ExceptAnd(inputData, null);
                
                //////////////////////////test////////////////////////////////////////////////
                //CLOG.Info(waitStr);
                //////////////////////////////////////////////////////////////////////////

                string sign = Convert.ToString(rechargeData["sign"]);
                string factSign = Helper.getMD5(waitStr + ApiKey);
                bool checkRes = (sign == factSign);
                if (!checkRes)
                {
                    retStr = RET_FAIL;
                    CLOG.Info("uc, sign error");
                    break;
                }

                int rmb = (int)Convert.ToDouble(inputData["amount"]);
                string selfOrderId = Convert.ToString(inputData["orderId"]);
                PayInfoBase baseData = new PayInfoBase();
                CheckRet checkCode = checkOrder(selfOrderId, rmb, baseData);
                if (checkCode != CheckRet.checkRetSuccess)
                {
                    if (checkCode == CheckRet.checkRetRmbError)
                    {
                        retStr = RET_FAIL;
                        CLOG.Info("uc..rmberror");
                        break;
                    }
                    else if (checkCode == CheckRet.checkRetNoOrder)
                    {
                        retStr = RET_FAIL;
                        CLOG.Info("uc..noorder:{0}", selfOrderId);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTable.UC_PAY, inputData, baseData);
                if (!res)
                {
                    retStr = RET_FAIL;
                    CLOG.Info("uc..dberror");
                    break;
                }
            }
            catch (Exception ex)
            {
                retStr = RET_FAIL;
                CLOG.Info("uc..{0}", ex.ToString());
            }

        } while (false);

        return retStr;
    }
}

//////////////////////////////////////////////////////////////////////////
// 苹果回调，由客户端来调用
public class PayCallbacApple : PayCallbackBase
{
    public string TestVerifyURL; // 验证URL
    public string ProductVerifyURL;

    public const string RET_SUCCESS = "ok";
    public const string RET_FAIL = "fail";

    // 存储苹果productid与paycode的对应关系
    static Dictionary<string, string> m_productId2PayCode = new Dictionary<string, string>();

    static PayCallbacApple()
    {
        string path = HttpRuntime.BinDirectory + "..\\data";
        string file = Path.Combine(path, "M_RechangeCFG.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNode node = doc.SelectSingleNode("/Root");

        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            try
            {
                string payCode = node.Attributes["ID"].Value;
                string productId = node.Attributes["AppStoreID2"].Value.Trim();

                if (!string.IsNullOrEmpty(productId))
                {
                    m_productId2PayCode.Add(productId, payCode);
                }

                string productId3 = node.Attributes["AppStoreID3"].Value.Trim();
                if (!string.IsNullOrEmpty(productId3))
                {
                    m_productId2PayCode.Add(productId3, payCode);
                }
                string productId4 = node.Attributes["AppStoreID4"].Value.Trim();
                if (!string.IsNullOrEmpty(productId4))
                {
                    m_productId2PayCode.Add(productId4, payCode);
                }
            }
            catch (System.Exception ex)
            {
               // CLOG.Info(ex.ToString());
            }
        }
    }

    public override string notifyPay(object param)
    {
        string retStr = RET_SUCCESS;
       // CLOG.Info("apple..start callback");
        do
        {
            try
            {
                HttpRequest Request = (HttpRequest)param;
                Dictionary<string, object> verifyData = genVerityData(Request);

                string receiptData = Convert.ToString(verifyData["receiptData"]);

                /*try
                {
                    byte[] arr = Convert.FromBase64String(receiptData);
                    // 里面已包含 receipt-data 信息
                    receiptData = Encoding.UTF8.GetString(arr);
                }
                catch (System.Exception ex)
                {
                    retStr = RET_FAIL;
                    CLOG.Info("apple..base64 data fail");
                    break;
                }*/

                List<Dictionary<string, object>> retData = null;
                string statusCode = null;
                bool res = verity(ProductVerifyURL, receiptData, ref retData, ref statusCode);
                if (statusCode == "21007") // 需要切到测试环境
                {
                    res = verity(TestVerifyURL, receiptData, ref retData, ref statusCode);
                }
                if (!res)
                {
                    retStr = RET_FAIL;
                    CLOG.Info("apple..veriy fail");
                    break;
                }

                PayInfoBase baseData = new PayInfoBase();
                CheckRet checkCode = checkOrder(Convert.ToString(verifyData["selfOrderId"]), baseData);
                if (checkCode != CheckRet.checkRetSuccess)
                {
                    retStr = RET_FAIL;
                    CLOG.Info("apple..check order fail, orderid:{0}", Convert.ToString(verifyData["selfOrderId"]));
                    break;
                }

                Dictionary<string, object> inputData = fillInputData(retData, baseData);
                
                if (inputData.Count <= 0)
                {
                    retStr = RET_FAIL;
                    CLOG.Info("apple..input data empty");
                    break;
                }

                baseData.addBaseDataToDic(inputData);
                string transid = Convert.ToString(inputData["transaction_id"]);
                if (!MongodbPayment.Instance.KeyExistsBykey(PayTable.APPLE_PAY, "transaction_id", transid))
                {
                    res = insertPayData(PayTable.APPLE_PAY, inputData, baseData);
                    if (!res)
                    {
                        retStr = RET_FAIL;
                        CLOG.Info("apple..dberror");
                        break;
                    }
                }
                else
                {
                    retStr = RET_FAIL;
                    CLOG.Info("apple..transionid duplicate: {0}, playerId: {1}", transid, baseData.m_playerId);
                    break;
                }
            }
            catch (Exception ex)
            {
                retStr = RET_FAIL;
                CLOG.Info("apple..{0}", ex.ToString());
            }

        } while (false);

        return retStr;
    }

    Dictionary<string, object> fillInputData(List<Dictionary<string, object>> retData, PayInfoBase baseData)
    {
        Dictionary<string, object> inputData = new Dictionary<string, object>();
        foreach (var dic in retData)
        {
            string product_id = dic["product_id"].ToString();

            if (!m_productId2PayCode.ContainsKey(product_id))
            {
                continue;
            }
            string payId = m_productId2PayCode[product_id];
            if (payId == baseData.m_payCode)
            {
                inputData.Add("product_id", product_id);
                inputData.Add("transaction_id", dic["transaction_id"].ToString());
                inputData.Add("original_transaction_id", dic["original_transaction_id"].ToString());
                try
                {
                    string purdate = Convert.ToString(dic["original_purchase_date"]);
                    int index = purdate.IndexOf("Etc/GMT");
                    if (index >= 0)
                    {
                        purdate = purdate.Substring(0, index);
                    }
                    DateTime dt = Convert.ToDateTime(purdate);
                    inputData.Add("original_purchase_date", dt);
                }
                catch (System.Exception ex)
                {
                }

                break;
            }
        }

        return inputData;
    }

    Dictionary<string, object> genVerityData(HttpRequest Request)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("selfOrderId", Request.QueryString["selfOrderId"]);

        byte[] byteArr = new byte[Request.InputStream.Length];
        Request.InputStream.Read(byteArr, 0, byteArr.Length);
        string byteStr = Encoding.UTF8.GetString(byteArr);
        //CLOG.Info("genVerityData, receiptData--: " + byteStr);
        data.Add("receiptData", byteStr);
        return data;
    }

    bool verity(string verifyURL, string receiptData, ref List<Dictionary<string, object>> retData, ref string statusCode)
    {
        try
        {
            MemoryStream stream = new MemoryStream();
            byte[] data = Encoding.UTF8.GetBytes(receiptData);
            stream.Write(data, 0, data.Length);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(verifyURL);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = stream.Length;
            Stream requestStream = request.GetRequestStream();
            stream.Position = 0L;
            stream.CopyTo(requestStream);
            stream.Close();
            requestStream.Close();

            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            {
                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
               // CLOG.Info("apple accept: " + retString);
                Dictionary<string, object> ret1 = JsonHelper.ParseFromStr<Dictionary<string, object>>(retString);
                statusCode = ret1["status"].ToString();
                if (ret1["status"].ToString() == "0")
                {
                    Dictionary<string, object> ret2 = JsonHelper.ParseFromStr<Dictionary<string, object>>(ret1["receipt"].ToString());
                    List<Dictionary<string, object>> ret3 = JsonHelper.ParseFromStr<List<Dictionary<string, object>>>(ret2["in_app"].ToString());
                    retData = ret3;
                    return true;
                }
            }
        }
        catch (System.Exception ex)
        {
            CLOG.Info(ex.ToString());
        }
        return false;
    }
}

//////////////////////////////////////////////////////////////////////////
/*
public class PayCallbackQQLobby : PayCallbackBase
{
    // 发货URL, 不包括IP部分
    public string SEND_URL = "";

    const int OK = 0;
    const int FAIL = 1;
    const string OK_STR = "OK";
    const string FAIL_STR = "FAIL";

    public string AppSecret { set; get; }
    public string PayTableName { set; get; }

    public override string notifyPay(object param)
    {
        Dictionary<string, object> ret = new Dictionary<string, object>();

        do
        {
            try
            {
                HttpRequest Request = (HttpRequest)param;
                Dictionary<string, object> inputData = getInputData(Request);

                QQCommon check = new QQCommon();
                string waitStr = check.getWaitSignStr(inputData, SEND_URL, Request.HttpMethod);
                string selfSign = check.signStr(waitStr);
                string sign = Convert.ToString(inputData["sig"]);

                CLOG.Info(waitStr);

                if (selfSign != sign)
                {
                    addResult(ret, FAIL, FAIL_STR);
                    CLOG.Info("QQLobby, sign error");
                    break;
                }

                int rmb = 100; // 由于回调未传金额，不判断rmb
                string selfOrderId = Convert.ToString(inputData["cee_extend"]);
                PayInfoBase baseData = new PayInfoBase();
                CheckRet checkCode = checkOrder(selfOrderId, rmb, baseData);
                if (checkCode != CheckRet.checkRetSuccess)
                {
                    if (checkCode == CheckRet.checkRetRmbError)
                    {
                    }
                    else if (checkCode == CheckRet.checkRetNoOrder)
                    {
                        addResult(ret, FAIL, FAIL_STR);
                        CLOG.Info("QQLobby..noorder:{0}", selfOrderId);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTableName, inputData, baseData);
                if (!res)
                {
                    addResult(ret, FAIL, FAIL_STR);
                    CLOG.Info("QQLobby..dberror");
                    break;
                }
                else
                {
                    addResult(ret, OK, OK_STR);
                }
            }
            catch (Exception ex)
            {
                addResult(ret, FAIL, FAIL_STR);
                CLOG.Info("QQLobby..{0}", ex.ToString());
            }

        } while (false);

        return JsonHelper.genJson(ret);
    }

    Dictionary<string, object> getInputData(HttpRequest Request)
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        return result;
    }

    void addResult(Dictionary<string, object> ret, int result, string msg)
    {
        ret.Add("ret", result);
        ret.Add("msg", msg);
    }
}
*/

/////////////////////////////////////////////////////////////////////////
// 支付付页面支付回调
public class PayCallbackALiWeb : PayCallbackBase
{
    public string PublicKey { set; get; }
    public string PayTableName { set; get; }
    public string AppId { set; get; }
    public string ErrorPrefix { set; get; }
    
    static List<string> s_excludeKey = new List<string>(new string[] { "sign", "sign_type" });

    public PayCallbackALiWeb()
    {
        ErrorPrefix = "PayCallbackALiWeb";
    }

    public override string notifyPay(object param)
    {
        HttpRequest Request = (HttpRequest)param;
        Dictionary<string, object> inputData = new Dictionary<string, object>();
        foreach (string key in Request.Form)
        {
            inputData[key] = Request.Form[key];
        }

       // CLOG.Info("PayCallbackALiWeb enter");

        string codeStr = "success";
        do
        {
            try
            {
                string status = Convert.ToString(inputData["trade_status"]);
                if (status != "TRADE_SUCCESS")
                {
                    codeStr = "stateError";
                    CLOG.Info("{0}..status error", ErrorPrefix);
                    break;
                }

                if (Convert.ToString(inputData["app_id"]) != AppId)
                {
                    codeStr = "appid error";
                    CLOG.Info("{0}..appid error", ErrorPrefix);
                    break;
                }

                string wait = getWaitSignStr(inputData);
               // CLOG.Info(wait);

                bool checkRes = checkSignByRSA256PublicKey(wait, Convert.ToString(inputData["sign"]), PublicKey);
                if (!checkRes)
                {
                    codeStr = "errorSign";
                    CLOG.Info("{0}..errorSign", ErrorPrefix);
                    break;
                }

                PayInfoBase baseData = new PayInfoBase();
                baseData.m_orderId = Convert.ToString(inputData["out_trade_no"]);

                Dictionary<string, object> qd = PayBase.queryBaseData(baseData.m_orderId, MongodbPayment.Instance, PayCallbackBase.S_TOTAL_PAY_FIELDS);
                if (qd == null)
                {
                    codeStr = "noOrderId";
                    CLOG.Info("{1}..noOrderId:{0}", baseData.m_orderId, ErrorPrefix);
                    break;
                }

                int inrmb = (int)Convert.ToDouble(inputData["total_amount"]);
                int srcrmb = Convert.ToInt32(qd["RMB"]);
                if (inrmb != srcrmb)
                {
                    codeStr = "rmb error";
                    CLOG.Info("{2}..rmb error, need{0}, fact{1}", srcrmb, inrmb, ErrorPrefix);
                    break;
                }

                baseData.m_payTime = DateTime.Now;
                baseData.m_payCode = Convert.ToString(qd["PayCode"]);
                baseData.m_playerId = Convert.ToInt32(qd["PlayerId"]);
                baseData.m_account = Convert.ToString(qd["Account"]);
                baseData.m_rmb = srcrmb;
                baseData.m_channelNumber = Convert.ToString(qd["channel_number"]);

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTableName, inputData, baseData);
                if (!res)
                {
                    codeStr = "dberror";
                    CLOG.Info("{0}..dberror", ErrorPrefix);
                }
            }
            catch (Exception ex)
            {
                codeStr = "error";
                CLOG.Info("{1}..{0}", ex.ToString(), ErrorPrefix);
            }
        } while (false);

        return codeStr;
    }

    string getWaitSignStr(Dictionary<string, object> inputData)
    {
        string str = "";
        str = getWaitSignStrByAsc(inputData, s_excludeKey);
        return str;
    }
}

/////////////////////////////////////////////////////////////////////////
// 微信web支付回调
public class PayCallbackWeixinWeb : PayCallbackBase
{
    public string ApiSecret { set; get; }

    public string PayTableName { set; get; }

    public string ErrorPrefix { set; get; }

    static List<string> s_excludeKey = new List<string>(new string[] { "sign" });

    public PayCallbackWeixinWeb()
    {
        ErrorPrefix = "weixin_web";
    }
    
    public override string notifyPay(object param)
    {
        HttpRequest Request = (HttpRequest)param;
        byte[] byteArr = new byte[Request.InputStream.Length];
        Request.InputStream.Read(byteArr, 0, byteArr.Length);
        string byteStr = Encoding.UTF8.GetString(byteArr);
        XmlGen xml = new XmlGen();
        Dictionary<string, object> inputData = xml.fromXmlString("xml", byteStr);
        // CLOG.Info("enter PayCallbackWeixinWeb");
        Dictionary<string, object> ret = new Dictionary<string, object>();
        string retCode = "SUCCESS";
        do
        {
            try
            {
                string resstr = Convert.ToString(inputData["return_code"]);
                if (resstr != "SUCCESS")
                {
                    CLOG.Info("{0} result is {1}", ErrorPrefix, resstr);
                    retCode = "error";
                    break;
                }

                PayCheck chk = new PayCheck();
                string wait = chk.getWeiXinWaitSigned(inputData, s_excludeKey, ApiSecret);
                //CLOG.Info(wait);

                string calSign = Helper.getMD5Upper(wait);
                if (Convert.ToString(inputData["sign"]) != calSign)
                {
                    retCode = "errorSign";
                    CLOG.Info("{0}..errorSign", ErrorPrefix);
                    break;
                }

                int rmb = (int)Convert.ToDouble(inputData["total_fee"]);

                PayInfoBase baseData = new PayInfoBase();
                baseData.m_orderId = Convert.ToString(inputData["out_trade_no"]);

                Dictionary<string, object> qd = PayBase.queryBaseData(baseData.m_orderId, MongodbPayment.Instance, PayCallbackBase.S_TOTAL_PAY_FIELDS);
                if (qd == null)
                {
                    retCode = "noOrderId";
                    CLOG.Info("{1}..noOrderId:{0}", baseData.m_orderId, ErrorPrefix);
                    break;
                }

                int srcrmb = Convert.ToInt32(qd["RMB"]);
                if (rmb != srcrmb * 100)
                {
                    retCode = "rmb error";
                    string err = string.Format("{2}..rmb error,need{0},fact{1}", (srcrmb * 100), rmb, ErrorPrefix);
                    CLOG.Info(err);
                    break;
                }

                baseData.m_payTime = DateTime.Now;
                baseData.m_payCode = Convert.ToString(qd["PayCode"]);
                baseData.m_playerId = Convert.ToInt32(qd["PlayerId"]);
                baseData.m_account = Convert.ToString(qd["Account"]);
                baseData.m_rmb = rmb / 100; // 单位分，需转成元
                baseData.m_channelNumber = Convert.ToString(qd["channel_number"]);

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTableName, inputData, baseData);
                if (!res)
                {
                    retCode = "dberror";
                    CLOG.Info("{0}..dberror", ErrorPrefix);
                }
            }
            catch (Exception ex)
            {
                retCode = "error";
                CLOG.Info("{1}..{0}", ex.ToString(), ErrorPrefix);
            }

        } while (false);

        ret.Add("return_code", retCode);
        if (retCode == "SUCCESS")
        {
            ret.Add("return_msg", "OK");
        }
        else
        {
            ret.Add("return_msg", retCode);
        }
        return xml.genXML("xml", ret);
    }
}

/////////////////////////////////////////////////////////////////////////
// 多游苹果越狱支付回调
public class PayCallbackDuoyouApple : PayCallbackBase
{
    public string AppId { set; get; }

    public string AppKey { set; get; }

    public string PayTableName { set; get; }

    static List<string> s_excludeKey = new List<string>(new string[] { "sign", "sign_type" });

    public override string notifyPay(object param)
    {
       // CLOG.Info("PayCallbackDuoyouApple enter");

        HttpRequest Request = (HttpRequest)param;
        string retCode = "success";
        Dictionary<string, object> inputData = getInputData(Request);

        do
        {
            try
            {
                PayCheck chk = new PayCheck();
               // string all = chk.getWaitSignStrByAsc(inputData, null);
               // CLOG.Info(all);

                string wait = chk.getWaitSignStrByAsc(inputData, s_excludeKey);
                string key = genKey();
                wait = wait + "&key=" + key;
                string sign = chk.signByHMACSHA256(wait, key);

                if (Convert.ToString(inputData["sign"]).ToLower() != sign)
                {
                    retCode = "errorSign";
                    CLOG.Info("duoyou..errorSign");
                    break;
                }

                int rmb = (int)Convert.ToDouble(inputData["total_fee"]); // 分

                PayInfoBase baseData = new PayInfoBase();
                baseData.m_orderId = Convert.ToString(inputData["out_trade_no"]);

                Dictionary<string, object> qd = PayBase.queryBaseData(baseData.m_orderId, MongodbPayment.Instance, PayCallbackBase.S_TOTAL_PAY_FIELDS);
                if (qd == null)
                {
                    retCode = "noOrderId";
                    CLOG.Info("duoyou..noOrderId:{0}", baseData.m_orderId);
                    break;
                }

                int srcrmb = Convert.ToInt32(qd["RMB"]);
                if (rmb != srcrmb * 100)
                {
                    retCode = "rmb error";
                    string err = string.Format("duoyou..rmb error: need {0},fact {1}", (srcrmb * 100), rmb);
                    CLOG.Info(err);
                    break;
                }

                baseData.m_payTime = DateTime.Now;
                baseData.m_payCode = Convert.ToString(qd["PayCode"]);
                baseData.m_playerId = Convert.ToInt32(qd["PlayerId"]);
                baseData.m_account = Convert.ToString(qd["Account"]);
                baseData.m_rmb = rmb / 100; // 单位分，需转成元
                baseData.m_channelNumber = Convert.ToString(qd["channel_number"]);

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTableName, inputData, baseData);
                if (!res)
                {
                    retCode = "dberror";
                    CLOG.Info("duoyou..dberror");
                }
            }
            catch (Exception ex)
            {
                retCode = "error";
                CLOG.Info("duoyou..{0}", ex.ToString());
            }

        } while (false);

        return retCode;
    }

    // 生成key
    string genKey()
    {
        string w = AppId + "GAME_MAKERS" + AppKey;
        return Helper.getMD5(w);
    }

    Dictionary<string, object> getInputData(HttpRequest Request)
    {
        byte[] byteArr = new byte[Request.InputStream.Length];
        Request.InputStream.Read(byteArr, 0, byteArr.Length);
        string byteStr = Encoding.UTF8.GetString(byteArr);

        NameValueCollection nvc = HttpUtility.ParseQueryString(byteStr);
        Dictionary<string, object> inputData = new Dictionary<string, object>();
        string[] allKeys = nvc.AllKeys;
        foreach (var k in allKeys)
        {
            inputData.Add(k, nvc[k]);
        }
        return inputData;
    }

    Dictionary<string, object> getInputData1(HttpRequest Request)
    {
        Dictionary<string, object> inputData = new Dictionary<string, object>();
        string[] allKeys = Request.Form.AllKeys;
        foreach (var k in allKeys)
        {
            inputData.Add(k, Request.Form[k]);
        }
        return inputData;
    }
}

////////////////////////////////////////////////////////////////////////
//QQgame发货
public class SendgoodsQQgame : PayCallbackBase
{
    static List<string> s_excludeKey = new List<string>(new string[] { "sig" , "cee_extend" });

    // 1人民币转成多少虚拟币     amt这里以0.1Q点为单位。即如果总金额为18Q点，则这里显示的数字是180
    public const int RMB_TO_VIRTUAL_MONEY = 10;

    public static string URL_PATH = "/QQgameSendgoods.ashx";

    public  string sendGoods(object param)
    {
        Dictionary<string, object> res_back = new Dictionary<string, object>();
        do
        {
            try
            {
                HttpRequest Request = (HttpRequest)param;
                Dictionary<string, object> inputData = new Dictionary<string, object>();
                Dictionary<string, string> inputDataOri = new Dictionary<string, string>();

                foreach (string key in Request.QueryString)
                {
                    if (key == "sig")
                    {
                        inputData[key] = Request.QueryString[key];
                    }
                    else
                    {
                        inputData[key] = SnsSigCheck.EncodeValue(Request.QueryString[key]);
                    }
                    inputDataOri[key] = Request.QueryString[key];
                }

                //超时
             // if (PayBase.getTS() - Convert.ToInt64(inputData["ts"]) > 60*15) {
             //     res_back.Add("ret", 2);
             //     res_back.Add("msg", "token expire");
             //     break;
             // }

                //验签
                PayCheck check = new PayCheck();
                string oriStr = "";// check.getWaitSignStrByAsc(inputDataOri);
                CLOG.Info("SendgoodsQQgame oriStr:  " + oriStr);
                string waitSign1 = check.getWaitSignStrByAsc(inputData, s_excludeKey);
                CLOG.Info("SendgoodsQQgame:  " + waitSign1);

                //string waitSign = "GET&" + SnsSigCheck.UrlEncode(URL_PATH, Encoding.UTF8) + "&" + SnsSigCheck.UrlEncode(waitSign1, Encoding.UTF8);
                string waitSign = "GET&" + SnsSigCheck.UrlEncode(URL_PATH, Encoding.UTF8) + "&" + SnsSigCheck.UrlEncode(waitSign1, Encoding.UTF8);
                //waitSign = waitSign.Replace("*", "%2A");
                string AppSecret = QQgameCFG.appkey + "&";
                string sign = Helper.SHA1Encrypt(waitSign, AppSecret);

                bool res1 = SnsSigCheck.VerifySig("GET", URL_PATH, inputDataOri, AppSecret, Convert.ToString(inputDataOri["sig"]));
                if (sign != Convert.ToString(inputData["sig"]))
                {
                    res_back.Add("ret", 4);
                    res_back.Add("msg", "sig error");
                    CLOG.Info("qqgame:" + "sign error");
                    break;
                }

                /**
                 * 验签成功  必须在2秒内发货  
                */
                PayInfoBase baseData = new PayInfoBase();
                string payItems = Convert.ToString(inputData["payitem"]);
                string[] arr = payItems.Split('*');
                baseData.m_orderId = arr[0];

                Dictionary<string, object> qd = PayBase.queryBaseData(baseData.m_orderId, MongodbPayment.Instance, PayCallbackBase.S_TOTAL_PAY_FIELDS);
                if (qd == null)
                {
                    CLOG.Info("qqgame..noOrderId:{0}", baseData.m_orderId);
                    res_back.Add("ret", 4);
                    res_back.Add("msg", "noOrderId error");
                    break;
                }

                int srcrmb = Convert.ToInt32(qd["RMB"]);  //元
                int amt_price = Convert.ToInt32(inputData["amt"]);  // 0.1Q点单位   也就是分
                if (amt_price != srcrmb * 100)
                {
                    string err = string.Format("qqgame.rmb error,need{0},fact{1}", (srcrmb * 100), amt_price);
                    CLOG.Info(err);

                    res_back.Add("ret", 4);
                    res_back.Add("msg", "amt error");
                    break;
                }

                baseData.m_payTime = DateTime.Now;
                baseData.m_payCode = Convert.ToString(qd["PayCode"]);
                baseData.m_playerId = Convert.ToInt32(qd["PlayerId"]);
                baseData.m_account = Convert.ToString(qd["Account"]);
                baseData.m_rmb = amt_price / 100; // 单位分，需转成元
                baseData.m_channelNumber = Convert.ToString(qd["channel_number"]);

                baseData.addBaseDataToDic(inputData);
                insertConfirmData(inputData, baseData);  
                bool res = insertPayData(PayTable.QQGAME_PAY, inputData, baseData);
                if (!res)
                {
                    CLOG.Info("qqgame..dberror");
                    res_back.Add("ret", 4);
                    res_back.Add("msg", "db error");
                    break;
                }

                res_back.Add("ret", 0);
                res_back.Add("msg", "OK");
            }
            catch (System.Exception ex)
            {
                res_back["ret"] = 4;
                res_back["msg"] = "db error";
                CLOG.Info(ex.ToString());
            }
        } while (false);

        return JsonHelper.ConvertToStr(res_back);
    }

   void insertConfirmData(Dictionary<string, object> inputData, PayInfoBase baseData)
    {
        try
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("orderid", baseData.m_orderId);
            data.Add("openid", Convert.ToString(inputData["openid"]));
            //data.Add("openkey", Convert.ToString(inputData["openkey"]));
            data.Add("appid", Convert.ToString(inputData["appid"]));
            data.Add("pf", inputData.ContainsKey("pf") ? Convert.ToString(inputData["pf"]) : "qqgame");

            data.Add("ts", PayBase.getTS().ToString());
            data.Add("payitem", Convert.ToString(inputData["payitem"]));
            data.Add("token_id", Convert.ToString(inputData["token"]));
            data.Add("billno", Convert.ToString(inputData["billno"]));
            data.Add("zoneid", Convert.ToString(inputData["zoneid"]));
            data.Add("provide_errno", "0"); //发货成功时填入0
            data.Add("amt", inputData.ContainsKey("amt") ? Convert.ToString("amt") : "0");
            data.Add("payamt_coins", inputData.ContainsKey("payamt_coins") ? Convert.ToString("payamt_coins") : "0");
            data.Add("create_time", DateTime.Now);
            bool res1 = insertPayConfirmData(QQgameCFG.QQ_GAME_CONFIRM_TABLE, data);
        }
        catch(Exception ex)
        {
            CLOG.Info(ex.ToString());
        }
    }
}

////////////////////////////////////////////////////////////////////////
// 华为支付回调h5端
public class PayCallbackHuaWeiH5 : PayCallbackHuaWei
{
    public override string notifyPay(object param)
    {
        Dictionary<string, object> retData = new Dictionary<string, object>();

        try
        {
            do
            {
                HttpRequest Request = (HttpRequest)param;
                Dictionary<string, object> inputData = new Dictionary<string, object>();
                foreach (string key in Request.Form)
                {
                    inputData[key] = Request.Form[key];
                }

                string waitStr = getWaitSignStrByAsc(inputData, s_excludeKey);
               // CLOG.Info("huawei wait:" + waitStr);
                string signType = "";
                if (inputData.ContainsKey("signType"))
                {
                    signType = Convert.ToString(inputData["signType"]);
                }
                bool signRes = false;

                if (signType == "RSA256")
                {
                    signRes = checkSignByRSA256PublicKey(waitStr, Convert.ToString(inputData["sign"]), PublicKey);
                }
                else
                {
                    signRes = checkSignByRSAPublicKey(waitStr, Convert.ToString(inputData["sign"]), PublicKey);
                }

                if (!signRes)
                {
                    retData.Add("result", 1); // 验签失败
                    CLOG.Info("PayCallbackHuaWeiH5 sign error");
                    break;
                }

                int rmb = (int)Convert.ToDouble(inputData["amount"]);
                string selfOrderId = Convert.ToString(inputData["requestId"]);
                PayInfoBase baseData = new PayInfoBase();
                CheckRet checkCode = checkOrder(selfOrderId, rmb, baseData);
                if (checkCode != CheckRet.checkRetSuccess)
                {
                    if (checkCode == CheckRet.checkRetRmbError)
                    {
                        CLOG.Info("PayCallbackHuaWeiH5,rmberror, need {0}, fact {1}", baseData.m_rmb, rmb);
                        retData.Add("result", 3);
                        break;
                    }
                    else if (checkCode == CheckRet.checkRetNoOrder)
                    {
                        retData.Add("result", 3);
                        CLOG.Info("PayCallbackHuaWeiH5..noorder:{0}", selfOrderId);
                        break;
                    }
                    else
                    {
                        retData.Add("result", 94);
                        CLOG.Info("PayCallbackHuaWeiH5..ex");
                        break;
                    }
                }

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTableName, inputData, baseData);
                if (!res)
                {
                    retData.Add("result", 95); // IO 错误
                    CLOG.Info("PayCallbackHuaWeiH5 db error");
                    break;
                }

                retData.Add("result", 0);              
            } while (false);
        }
        catch (System.Exception ex)
        {
            CLOG.Info("PayCallbackHuaWeiH5 {0}", ex.ToString());
            retData.Add("result", 94);
        }
        
        return JsonHelper.genJson(retData);
    }
}


////////////////////////////////////////////////////////////////////////
// tianwangyule 回调
public class PayCallbackTianWangYuLe : PayCallbackBase
{
    private string m_retStr;

    public string Secret { set; get; }

    public string PayTableName { set; get; }

    public override string notifyPay(object param)
    {
        setRetStr("success");

        try
        {
            do
            {
                HttpRequest Request = (HttpRequest)param;
                Dictionary<string, object> inputData = new Dictionary<string, object>();
                foreach (string key in Request.Form)
                {
                    inputData[key] = Request.Form[key];
                }

                if ("success" != Convert.ToString(inputData["status"]))
                {
                    setRetStr("status error");
                    CLOG.Info("PayCallbackTianWangYuLe status error");
                    break;
                }

                string waitStr = getWaitSign(inputData);
                CLOG.Info(waitStr);
                CLOG.Info("ori sign " + Convert.ToString(inputData["sign"]));

                string sign = Helper.getMD5Upper(waitStr);
                if (sign != Convert.ToString(inputData["sign"]))
                {
                    setRetStr("sign error");
                    CLOG.Info("PayCallbackTianWangYuLe sign error");
                    break;
                }

                string selfOrderId = Convert.ToString(inputData["orderid"]);
                PayInfoBase baseData = new PayInfoBase();
                CheckRet checkCode = checkOrder(selfOrderId, baseData);
                if (checkCode != CheckRet.checkRetSuccess)
                {
                    if (checkCode == CheckRet.checkRetNoOrder)
                    {
                        setRetStr("no order");
                        CLOG.Info("PayCallbackTianWangYuLe..noorder:{0}", selfOrderId);
                        break;
                    }
                    else
                    {
                        setRetStr("exception");
                        CLOG.Info("PayCallbackTianWangYuLe..ex");
                        break;
                    }
                }

                baseData.addBaseDataToDic(inputData);
                bool res = insertPayData(PayTableName, inputData, baseData);
                if (!res)
                {
                    setRetStr("db error");
                    CLOG.Info("PayCallbackTianWangYuLe db error");
                    break;
                }
            } while (false);
        }
        catch (System.Exception ex)
        {
            CLOG.Info("PayCallbackTianWangYuLe {0}", ex.ToString());
            setRetStr("exception 1");
        }

        return m_retStr;
    }

    private void setRetStr(string str)
    {
        m_retStr = str;
    }

    private string getWaitSign(Dictionary<string, object> inputData)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("merchantid={0}", Convert.ToString(inputData["merchantid"]));
        builder.Append("&");

        builder.AppendFormat("orderid={0}", Convert.ToString(inputData["orderid"]));
        builder.Append("&");

        builder.AppendFormat("status={0}", Convert.ToString(inputData["status"]));
        builder.Append("&");

        builder.AppendFormat("secret={0}", Secret);

        return builder.ToString();
    }
}