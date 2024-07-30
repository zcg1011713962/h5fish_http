﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;

namespace HttpLogin.newAcc
{
    public partial class newAutoRegedit : System.Web.UI.Page
    {
        const string AES_KEY = "&@*(#kas9081fajk";

        protected void Page_Load(object sender, EventArgs e)
        {
            CAutoRegAcc obj = new CAutoRegAcc();
            obj.regAcc(Request);
            string str = obj.getRetStr();
            Response.Write(str);
            return;



            if (!LoginCheck.canRegAcc(Request.Params["deviceID"]))
            {
                Response.Write("local ret = {code = -1}; return ret;");
                return;
            }
            string platform = Request.Params["platform"];
            if (string.IsNullOrEmpty(platform))
            {
                Response.Write("local ret = {code = -1}; return ret;");
                return;
            }

            string table = ConfigurationManager.AppSettings["acc_" + platform];
            if (string.IsNullOrEmpty(table))
            {
                Response.Write("local ret = {code = -15}; return ret;");
                return;
            }

            string acc_reg = BuildAccount.getAutoAccount(table, table);
            if (string.IsNullOrEmpty(acc_reg))
            {
                Response.Write("local ret = {code = -12}; return ret;");
                return;
            }
            string encrypt = Request.Params["encrypt"];
            bool pwd_encrypt = false;
            if (!string.IsNullOrEmpty(encrypt) && encrypt == "true")
            {
                pwd_encrypt = true;
            }
            string pwd = null;
            string out_pwd = null;
            string save_pwd = null;
            if (pwd_encrypt)
            {
                pwd = BuildAccount.getAutoPassword(6);
                out_pwd = AESHelper.AESEncrypt(pwd, AES_KEY);
                save_pwd = AESHelper.MD5Encrypt(pwd);
            }
            else
            {
                out_pwd = BuildAccount.getAutoPassword(20);
                pwd = string.Format("{0}{1}{2}{3}{4}{5}", out_pwd[8], out_pwd[16], out_pwd[4], out_pwd[11], out_pwd[2], out_pwd[9]);//password
                save_pwd = AESHelper.MD5Encrypt(pwd);
            }
            Random rd = new Random();
            int randkey = rd.Next();
            Dictionary<string, object> updata = new Dictionary<string, object>();
            updata["acc"] = acc_reg;
            string endacc = Guid.NewGuid().ToString().Replace("-", "");
            updata["acc_real"] = endacc;
            updata["pwd"] = save_pwd;
            DateTime now = DateTime.Now;
            updata["randkey"] = randkey;
            updata["lasttime"] = now.Ticks;
            updata["regedittime"] = now;
            updata["regeditip"] = Request.ServerVariables.Get("Remote_Addr").ToString();
            updata["updatepwd"] = false;
            //updata["platform"] = Platform;
            updata["lastip"] = Convert.ToString(updata["regeditip"]);
            string deviceID = Request.Params["deviceID"];
            updata["deviceId"] = (deviceID == null) ? "" : deviceID;

            string strerr = MongodbAccount.Instance.ExecuteStoreBykey(table, "acc", acc_reg, updata);
            if (strerr != "")
            {
                Response.Write("local ret = {code = -11}; return ret;");
            }
            else
            {
                string channelID = Request.Params["channelID"];
                channelID = LoginCommon.channelToString(channelID);

                Dictionary<string, object> savelog = new Dictionary<string, object>();
                savelog["acc"] = acc_reg;
                savelog["acc_real"] = endacc;
                
                if (!string.IsNullOrEmpty(deviceID))
                {
                    savelog["acc_dev"] = deviceID;
                    LoginCheck.incRegAcc(deviceID);
                }
                savelog["ip"] = Request.ServerVariables.Get("Remote_Addr").ToString();
                savelog["time"] = now;
                savelog["channel"] = channelID;
                MongodbAccount.Instance.ExecuteInsert("RegisterLog", savelog);

                //渠道每日注册
                if (string.IsNullOrEmpty(channelID) == false)
                {
                    MongodbAccount.Instance.ExecuteIncBykey("day_regedit", "date", DateTime.Now.Date, channelID, 0);
                }
                string ret = string.Format("local ret = {{code = 0, acc=\"{0}\", pwd=\"{1}\", acc_real=\"{2}\"}}; return ret;", acc_reg, out_pwd, endacc);
                Response.Write(ret);
            }

            //Response.Write("local ret = {code = 0, acc=\"fish000001\", pwd=\"123456\"};"); 
        }
    }
}