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
    public partial class CheckRecharge : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //account
            //platform
            string platform = Request.Params["platform"];
            string account = Request.Params["account"];
            if (string.IsNullOrEmpty(platform) || string.IsNullOrEmpty(account))
            {
                CLOG.Info("CheckRecharge, data error");
                ReturnMsg("data error");//data error
                return;
            }

            string table = PayTable.getPayTableName(platform); //ConfigurationManager.AppSettings["pay_" + platform];
            //string table = "anysdk_pay";
            if (string.IsNullOrEmpty(table))
            {
                CLOG.Info("CheckRecharge, error platform:{0}", platform);
                ReturnMsg("error platform");//platform error
                return;
            }

            //CLOG.Info("CheckRecharge, {0}, {1}", platform, account);

            string splayerid = Request.Params["playerid"];
            if (string.IsNullOrEmpty(splayerid))
                splayerid = "0";

            int playerid = 0;

            try
            {
                playerid = Convert.ToInt32(splayerid);
            }
            catch (Exception)
            {

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

            lmq.Add(Query.EQ("Process", false));

            TalkingGame tg = new TalkingGame();
            var list = MongodbPayment.Instance.ExecuteGetListByQuery(table, Query.And(lmq), new string[] { "OrderID", "PayCode", "RMB" });
            
            //PayInfoBase baseData = new PayInfoBase();
            //baseData.m_state = 0;

            if (list.Count > 0)
            {       
                string ret = string.Empty;                
                foreach (var it in list)
                {
                    string rmb = "0";
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["check_money"]))
                        rmb = it["RMB"].ToString();

                    ret += (" " + it["PayCode"].ToString() + "_" + rmb + "_" + it["OrderID"].ToString());
                    tg.adddata(splayerid, it["OrderID"].ToString(), it["RMB"].ToString(), it["PayCode"].ToString());

                    //baseData.m_orderId = it["OrderID"].ToString();
                    //PayBase.updateDataToPaymentTotal(baseData, MongodbPayment.Instance);
                }

               /* Dictionary<string, object> data = new Dictionary<string, object>();
                data["Process"] = true;
                data["UpdateTime"] = DateTime.Now;
                if (playerid > 0)
                    data["PlayerId"] = playerid;

                string err = MongodbPayment.Instance.ExecuteUpdateByQuery(table, Query.And(lmq), data, UpdateFlags.Multi);
                if (err == string.Empty)*/
                {
                    ReturnMsg(ret.Trim(), platform, true);
                    ExceptionCheckInfo.doSaveCheckInfo(Request, "recharge");
                   
                    tg.PostToTG();                   
                }
                //else
                 //   ReturnMsg(err);
            }
            else
            {
               // CLOG.Info("CheckRecharge, can't find orderid playerId:{0}, account:{1}, platform:{2}", playerid, account, platform);
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