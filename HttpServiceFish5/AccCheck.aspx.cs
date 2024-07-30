using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace HttpLogin.Default
{
    public partial class AccCheck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.RequestType != "GET")
                return;

            string sacc = Request.Params["acc"];
            string platform = Request.Params["platform"];

            if (string.IsNullOrEmpty(sacc) || string.IsNullOrEmpty(platform))
            {
                CLOG.Info("data error");
                ReturnMsg("data error");  
                return;
            }

            string table = "";

            if (Login.isAccLand())
            {
                AccLandCheck ack = new AccLandCheck();
                AccLandContext context = new AccLandContext();
                context.Platform = platform;
                context.Acc = sacc;
                string msg = ack.startCheck(context);
                Response.Write(msg);
                return;
            }
            else
            {
                table = ConfigurationManager.AppSettings["acc_" + platform];
                if (string.IsNullOrEmpty(table))
                {
                    CLOG.Info("error platform,{0}", platform);
                    ReturnMsg("error platform");//platform error
                    return;
                }
            }

            //List<IMongoQuery> imqs = new List<IMongoQuery>();
            //imqs.Add(Query.EQ("acc", sacc));

            string acckey = ConfigurationManager.AppSettings["acckey_" + platform];
            if (string.IsNullOrEmpty(acckey))
            {
                acckey = "acc";
            }

            Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetBykey(table, acckey, sacc, new string[] { "randkey", "lasttime", "lastip", "deviceId" });
            if (data != null && data.Count>=2)
            {
                string jsonstr = data["randkey"].ToString() + "_" + data["lasttime"].ToString();     //JsonHelper.ConvertToStr(data);
                string lastIp = "";
                string deviceId = "";
                if (data.ContainsKey("lastip"))
                {
                    lastIp = Convert.ToString(data["lastip"]);
                }
                if (data.ContainsKey("deviceId"))
                {
                    deviceId = Convert.ToString(data["deviceId"]);
                }
                ReturnMsg(jsonstr.Trim(), lastIp, deviceId, true);
                ExceptionCheckInfo.doSaveCheckInfo(Request, "login");
            }
            else
            {
                CLOG.Info("db error, {0}, platform:{1}", sacc, platform);
                ReturnMsg("db error");             
            }            
        }

        void ReturnMsg(string info, string lastIP = "", string deviceId = "", bool bret = false)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["result"] = bret;
            if (bret)
            {
                data["data"] = info;
                data["lastip"] = lastIP;
                data["deviceId"] = deviceId;
            }
            else
                data["error"] = info;

            string jsonstr = JsonHelper.ConvertToStr(data);
            Response.Write(Convert.ToBase64String(Encoding.Default.GetBytes(jsonstr)));            
        }
    }
}