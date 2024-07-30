using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Collections.Specialized;
using System.Text;

namespace HttpRecharge
{
    public partial class xunlei_pay : System.Web.UI.Page
    {
        public const string SERVER_KEY = "c43bbd8bc6ae455cafdae67255334b21";
        public static string[] SPLIT_STR = new string[] { "?-?" };

        protected void Page_Load(object sender, EventArgs e)
        {
            var pay = new PayCallbackXunlei();
            string retStr = pay.notifyPay(Request);
            Response.Write(retStr);
            return;

            byte[] byteArr = new byte[Request.InputStream.Length];
            Request.InputStream.Read(byteArr, 0, byteArr.Length);
            string byteStr = Encoding.Default.GetString(byteArr);

            string codeStr = "success";
            try
            {
                Dictionary<string, object> dic = JsonHelper.ParseFromStr<Dictionary<string, object>>(byteStr);
                OrderInfo orderInfo = JsonHelper.ParseFromStr<OrderInfo>(dic["order_info"].ToString());

                string status = orderInfo.pay_status;
                if (status != "success") // 仅success时，迅雷才会通知
                {
                    Response.Write("error");
                    return;
                }

                string sign = Convert.ToString(dic["sign"]);

                Dictionary<string, object> upData = getData(orderInfo);

                string calSign = getSignForAnyValid(upData);
                if (sign != calSign)
                {
                    CLOG.Info("check sign fail");
                    Response.Write("errorSign");
                    return;
                }

                string[] infoList = splitInfo(orderInfo.product_id);
                if (infoList.Length != 5)
                {
                    Response.Write("error");
                    return;
                }

                int rmb = (int)Convert.ToDouble(upData["total_amount"]);
                if (rmb <= 0)
                {
                    Response.Write("error");
                    return;
                }

                upData["PayTime"] = DateTime.Now;
                upData["PayCode"] = infoList[0];
                upData["PlayerId"] = Convert.ToInt32(infoList[2]);
                upData["Account"] = infoList[1];
                upData["RMB"] = rmb;
                upData["OrderID"] = infoList[4];
                upData["Process"] = false;
                upData["channel_number"] = infoList[3];

                if (!MongodbPayment.Instance.KeyExistsBykey(PayTable.XUNLEI_PAY, "OrderID", upData["OrderID"]))
                {
                    if (MongodbPayment.Instance.ExecuteInsert(PayTable.XUNLEI_PAY, upData))
                    {
                        PayCommon.savePayinfo("lobby", Convert.ToInt32(upData["RMB"]));

                        Dictionary<string, object> savelog = new Dictionary<string, object>();
                        savelog["acc"] = upData["Account"];
                        savelog["real_acc"] = upData["Account"];
                        savelog["time"] = DateTime.Now;
                        savelog["channel"] = upData["channel_number"];
                        savelog["rmb"] = upData["RMB"];
                        MongodbPayment.Instance.ExecuteInsert("PayLog", savelog);
                    }
                    else
                    {
                        codeStr = "error";
                    }
                }
            }
            catch (Exception ex)
            {
                codeStr = "error";
            }

            Response.Write(codeStr);
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

        string[] splitInfo(string info)
        {
            string[] arr = info.Split(SPLIT_STR, StringSplitOptions.RemoveEmptyEntries);
            return arr;
        }
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
