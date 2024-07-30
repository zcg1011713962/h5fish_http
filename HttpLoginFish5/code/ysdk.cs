using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using YSDK;
using System.Net;
using System.IO;
using System.Text;

/*
    ysdk游戏币模式
 */

public class YsdkBase
{
    public string AppKey { set; get; }

    public string AppID { set; get; }

    public bool IsTest { set; get; }
}

public class YsdkCheckPay : YsdkBase
{
    public string check(HttpRequest Request)
    {
        string retStr = "";
        do
        {
            try
            {
                string[] strs = Request.Params["ysdkdata"].ToString().Trim().Split(':');
                if (strs.Length < 5)
                {
                    retStr = Helper.buildLuaReturn(-2, "param error");
                    CLOG.Info("ysdkcheck, param error");
                    break;
                }

                string platform = strs[0];
                string openid = strs[1];
                string openkey = strs[2];
                string pf = strs[3];
                string pfkey = strs[4];
                //CLOG.Info("param: platform- {0}, openid- {1}, openkey- {2}, pf- {3}, pfkey- {4}",
                //    platform, openid, openkey, pf, pfkey);

                int pay_amount = Convert.ToInt32(Request.Params["amount"]) * 10;
               // CLOG.Info("pay_amount. " + pay_amount);

                string mode = "GET";
                
                YSDKHelper ysdkHelper = new YSDKHelper(AppKey, AppID);
                ysdkHelper.setTest(IsTest);

                ysdkHelper.initKeyValue(openid, openkey, pf, pfkey);
                ysdkHelper.addKeyValue("userip", Helper.GetWebClientIp());

                string url = ysdkHelper.buildURL(YSDKMethod.Get_Balance);
               // CLOG.Info("url: " + url);
                Uri uri = new Uri(url);
                HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                request.Method = mode;
                request.Headers.Add("cookie", ysdkHelper.buildCookie(platform, YSDKMethod.Get_Balance));

                HttpWebResponse responser = request.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(responser.GetResponseStream(), Encoding.UTF8);
                string msg = reader.ReadToEnd();

                BalanceResult result = JsonHelper.ParseFromStr<BalanceResult>(msg);
                if (result.ret == 0)
                {
                    if (result.balance >= pay_amount)
                    {
                        retStr = (Helper.buildLuaReturn(0, ""));
                    }
                    else
                    {
                        retStr = (Helper.buildLuaReturn(-1, "need recharge"));
                        CLOG.Info("ysdkcheck, need recharge, 余额 {0}", result.balance);
                    }
                }
                else
                {
                    retStr = (Helper.buildLuaReturn(result.ret, result.msg));
                    CLOG.Info("ysdkcheck, error, {0}, code {1}, balance {2}", result.msg, result.ret, result.balance);
                }
            }
            catch (Exception error)
            {
                retStr = (Helper.buildLuaReturn(-1000, error.Message));
                CLOG.Info("ysdkcheck,happend ex,  {0}", error.ToString());
            }

        } while (false);
       
        return retStr;
    }
}

public class YsdkPay : YsdkBase
{
    // log表
    public string YsdkLogTable { set; get; }

    // 支付表
    public string YsdkPayTable { set; get; }

    public string notifyPay(object param)
    {
        string retStr = "";
        HttpRequest Request = (HttpRequest)param;

        do
        {
            try
            {
                Dictionary<string, object> saveData = new Dictionary<string, object>();
                int amount = 0;
                try
                {
                    amount = (int)Convert.ToDouble(Request.Params["amount"]);
                }
                catch (Exception)
                {
                    amount = Convert.ToInt32(Request.Params["amount"]);
                }
                saveData["RMB"] = amount;
                if (amount == 0)
                {
                    retStr = (Helper.buildLuaReturn(-2, "param error"));
                    CLOG.Info("ysdkpay, 1. {0} param error", YsdkPayTable);
                    break;
                }
                int pay_amount = amount * 10;
                saveData["OrderID"] = Request.Params["orderid"];
                saveData["Account"] = Request.Params["acount"];
                saveData["PayCode"] = Request.Params["paycode"];
                saveData["Process"] = false;
                saveData["IsPay"] = false;
                saveData["PayTime"] = DateTime.Now;
                saveData["PlayerId"] = Convert.ToInt32(Request.Params["playerid"]);
                saveData["Channel"] = LoginCommon.channelToString(Request.Params["channel"]);

                ////////////////////////// 新增代码--流程修改 ///////////////////////////////
                PayInfoBase baseData = new PayInfoBase();
                baseData.m_payCode = Request.Params["paycode"];
                baseData.m_playerId = Convert.ToInt32(Request.Params["playerid"]);
                baseData.m_account = Request.Params["acount"];
                baseData.m_rmb = amount;
                baseData.m_orderId = Request.Params["orderid"];
                baseData.m_channelNumber = LoginCommon.channelToString(Request.Params["channel"]);
                baseData.m_payTime = Convert.ToDateTime(saveData["PayTime"]);

                if (!PayBase.existOrderInPaymentTotal(baseData, MongodbPayment.Instance))
                {
                    retStr = Helper.buildLuaReturn(-2, "param error");
                    CLOG.Info("ysdkpay, 2. {0} param error", YsdkPayTable);
                    break;
                }
                ////////////////////////////////////////////////////////////

                string[] strs = Request.Params["ysdkdata"].ToString().Trim().Split(':');
                if (strs.Length < 5)
                {
                    retStr = (Helper.buildLuaReturn(-2, "param error"));
                    CLOG.Info("ysdkpay, 3. {0} param error", YsdkPayTable);
                    break;
                }

                Dictionary<string, object> payData = MongodbPayment.Instance.ExecuteGetBykey(YsdkLogTable, "OrderID", saveData["OrderID"]);
                if (payData == null)
                {
                    MongodbPayment.Instance.ExecuteInsert(YsdkLogTable, saveData);
                }
                else
                {
                    bool isPay = (bool)payData["IsPay"];
                    if (isPay == true)
                    {
                        retStr = (Helper.buildLuaReturn(-1, "is pay"));
                        CLOG.Info("ysdkpay, {0} is pay", YsdkPayTable);
                        break;
                    }
                }

                string platform = strs[0];
                string openid = strs[1];
                string openkey = strs[2];
                string pf = strs[3];
                string pfkey = strs[4];

                string mode = "GET";
                YSDKHelper ysdkHelper = new YSDKHelper(AppKey, AppID);
                ysdkHelper.setTest(IsTest);

                ysdkHelper.initKeyValue(openid, openkey, pf, pfkey);
                ysdkHelper.addKeyValue("userip", Helper.GetWebClientIp());
                ysdkHelper.addKeyValue("amt", pay_amount.ToString());
                ysdkHelper.addKeyValue("billno", saveData["OrderID"].ToString());

                string url = ysdkHelper.buildURL(YSDKMethod.Pay);
                Uri uri = new Uri(url);
                HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                request.Method = mode;
                request.Headers.Add("cookie", ysdkHelper.buildCookie(platform, YSDKMethod.Pay));

                HttpWebResponse responser = request.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(responser.GetResponseStream(), Encoding.UTF8);
                string msg = reader.ReadToEnd();

                PayResult result = JsonHelper.ParseFromStr<PayResult>(msg);
                if (result.ret == 0)
                {
                    saveData["IsPay"] = true;

                    MongodbPayment.Instance.ExecuteUpdate(YsdkLogTable, "OrderID", saveData["OrderID"], saveData);
                    MongodbPayment.Instance.ExecuteInsert(YsdkPayTable, saveData);

                    PayBase.updateDataToPaymentTotal(baseData, MongodbPayment.Instance);

                    Dictionary<string, object> savelog = new Dictionary<string, object>();
                    savelog["acc"] = saveData["Account"];
                    savelog["real_acc"] = saveData["Account"];
                    savelog["acc_dev"] = Request.Params["device"];
                    savelog["time"] = DateTime.Now;
                    savelog["channel"] = LoginCommon.channelToString(saveData["Channel"].ToString());
                    savelog["rmb"] = saveData["RMB"];
                    MongodbPayment.Instance.ExecuteInsert("PayLog", savelog);

                    retStr = (Helper.buildLuaReturn(0, ""));
                }
                else
                {
                    retStr = (Helper.buildLuaReturn(result.ret, result.msg));
                    CLOG.Info("ysdkpay, {0}  error, {1}", YsdkPayTable, result.msg);
                }
            }
            catch (Exception error)
            {
                retStr = (Helper.buildLuaReturn(-1000, error.Message));
                CLOG.Info("ysdkpay, {0}  ex, {1}", YsdkPayTable, error.ToString());
            }

        } while (false);

        return retStr;
    }
}

public class YsdkCommon
{
    public static void getTthyAppKeyId(YsdkBase ysdk)
    {
        if (ysdk.IsTest)
        {
            ysdk.AppKey = PayTable.TTHY_YSDK_APPKEY_TEST;
        }
        else
        {
            ysdk.AppKey = PayTable.TTHY_YSDK_APPKEY;
        }

        ysdk.AppID = PayTable.TTHY_YSDK_APPID;
    }
}





