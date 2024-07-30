using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace PaymentCheck
{
    public partial class PaymentOnce : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //account
            //orderid
            //platform
            string platform = Request.Params["platform"];
            string orderid = Request.Params["orderid"];
            string account = Request.Params["account"];
            if (string.IsNullOrEmpty(platform) || string.IsNullOrEmpty(account)
                || string.IsNullOrEmpty(orderid))
            {
                CLOG.Info("PaymentOnce, data error");
                ReturnMsg("data error");//data error
                return;
            }

            string table = PayTable.getPayTableName(platform);
            if (string.IsNullOrEmpty(table))
            {
                CLOG.Info("PaymentOnce, error platform:{0}", platform);
                ReturnMsg("error platform");//platform error
                return;
            }

            string splayerid = Request.Params["playerid"];
            if (string.IsNullOrEmpty(splayerid))
                splayerid = "0";

            int playerid = 0;

            try
            {
                playerid = Convert.ToInt32(splayerid);
            }
            catch (Exception ex)
            {
                CLOG.Info("PaymentOnce " + ex.ToString());
            }

            if (splayerid == "0")
            {
                splayerid = account;
            }

            List<IMongoQuery> lmq = new List<IMongoQuery>();
            if (playerid > 0)
                lmq.Add(Query.EQ("PlayerId", playerid));
            else
                lmq.Add(Query.EQ("Account", account));

            lmq.Add(Query.EQ("OrderID", orderid));
            lmq.Add(Query.EQ("Process", false));

            var one = MongodbPayment.Instance.ExecuteGetByQuery(table, Query.And(lmq), new string[] { "PayCode", "RMB" });
            if (one != null)
            {
                /*Dictionary<string, object> data = new Dictionary<string, object>();
                data["Process"] = true;
                data["UpdateTime"] = DateTime.Now;
                if (playerid > 0)
                    data["PlayerId"] = playerid;
                string err = MongodbPayment.Instance.ExecuteUpdate(table, "OrderID", orderid, data);

                try
                {
                    PayInfoBase baseData = new PayInfoBase();
                    baseData.m_state = 0;
                    baseData.m_orderId = orderid;
                    PayBase.updateDataToPaymentTotal(baseData, MongodbPayment.Instance);
                }
                catch (Exception ex)
                {
                    CLOG.Info("PaymentOnce(1), {0}", ex.ToString());
                }*/
                
                string rmb = "0";
                try
                {
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["check_money"]))
                        rmb = one["RMB"].ToString();
                }
                catch (Exception ex)
                {
                    CLOG.Info("PaymentOnce(2), {0}", ex.ToString());
                }
             
                //if (err == string.Empty)
                {
                    ReturnMsg(one["PayCode"].ToString() + " " + rmb + " " + orderid, platform, true);
                    ExceptionCheckInfo.doSaveCheckInfo(Request, "recharge");

                    TalkingGame tg = new TalkingGame();
                    tg.adddata(splayerid, orderid, one["RMB"].ToString(), one["PayCode"].ToString());
                    tg.PostToTG();
                }
                //else
                {
                   // CLOG.Info("PaymentOnce {0} " + err);
                  //  ReturnMsg(err);
                }
            }
            else
            {
                CLOG.Info("PaymentOnce, can't find orderid:{0}, playerId:{1}, account:{2}, platform:{3}", orderid, playerid, account, platform);
                ReturnMsg("can't find orderid");//需要返回payid
            }         
        }        


        void ReturnMsg(string info, string plm = "", bool bret = false)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["result"] = bret;
            
            if (bret)
                data["platform"] = plm;

            if (bret)
                data["data"] = info;
            else
                data["error"] = info;

            string jsonstr = JsonHelper.ConvertToStr(data);
            Response.Write(Convert.ToBase64String(Encoding.Default.GetBytes(jsonstr)));
        }
    }
}