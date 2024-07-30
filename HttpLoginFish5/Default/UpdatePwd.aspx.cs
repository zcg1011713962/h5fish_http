using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Specialized;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace HttpLogin.Default
{
    // 通过手机找回密码时，验证发给手机的码。
    public partial class UpdatePwd : System.Web.UI.Page
    {
        const string AES_KEY = "&@*(#kas9081fajk";

        private Dictionary<string, object> m_dic = new Dictionary<string, object>();
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.RequestType != "POST")
                return;

            do 
            {
                string strdata = Request.Params["data"];
                string strmd5 = Request.Params["sign"];
                if (string.IsNullOrEmpty(strdata) || string.IsNullOrEmpty(strmd5))            
                    return;            

                strdata = Encoding.Default.GetString(Convert.FromBase64String(strdata));   

                if (strmd5 != AESHelper.MD5Encrypt(strdata + AES_KEY))
                {
                    m_dic[CC.KEY_CODE] = CC.ERR_SIGN;
                    //Response.Write("err_sign_error");//sign error
                    break;
                }

                Dictionary<string, object> data = JsonHelper.ParseFromStr<Dictionary<string, object>>(strdata);
                if (data == null || data.Count < 3)
                {
                    m_dic[CC.KEY_CODE] = CC.ERR_DATA;
                    break;
                   // Response.Write("err_data_error");//json error
                    //return;
                }
                try
                {
                    UpdateAccount(data);
                }
                catch (Exception ex)
                {
                   // Response.Write(ex.ToString());
                    m_dic[CC.KEY_CODE] = CC.ERR_UNKNOWN;
                }

            } while (false);
            Response.Write(JsonHelper.genJson(m_dic));
        }

        void UpdateAccount(Dictionary<string, object> cinfo)
        {
            string strphone = cinfo["n1"].ToString();
            if (strphone.Length != 11 || !Regex.IsMatch(strphone, @"^\d{11}$"))
            {
               // Response.Write("err_not_phone");//号码错误
                m_dic.Add(CC.KEY_CODE, CC.ERR_PHONE);
                return;
            }

            string[] field = { "acc", "pwdcode", "searchTime" };
            Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetBykey("AccountTable", CC.LOGIN_FIELD_MOBILE, strphone, field);
            if (data == null)
            {
               // Response.Write("err_not_bind");//未绑定  
                m_dic.Add(CC.KEY_CODE, CC.ERR_NOT_BINDPHONE);
            }
            else
            {
                DateTime last = DateTime.MinValue;
                if (data.ContainsKey("searchTime"))
                {
                    last = Convert.ToDateTime(data["searchTime"]).ToLocalTime();
                }

                TimeSpan span = DateTime.Now - last;
                if (span.TotalSeconds >= 3600)
                {
                   // Response.Write("err_code_error"); // 验证码过期了
                    m_dic.Add(CC.KEY_CODE, CC.ERR_CODE);
                    return;
                }

                if (cinfo["n2"].ToString() != data["pwdcode"].ToString())
                {
                    //Response.Write("err_code_error");//验证码错误
                    m_dic.Add(CC.KEY_CODE, CC.ERR_CODE);
                    return;
                }

                string spwd = cinfo["n3"].ToString();//AESHelper.AESDecrypt(cinfo["n3"].ToString(), AES_KEY);//password
                if (spwd.Length != 32)//md5
                {
                    //Response.Write("err_pwd_error");//密码错误
                    m_dic.Add(CC.KEY_CODE, CC.ERR_PWD);
                    return;
                }

                Dictionary<string, object> updata = new Dictionary<string,object>();
                updata["pwd"] = spwd;
                updata["pwdcode"] = "";
                string ret = MongodbAccount.Instance.ExecuteUpdate("AccountTable", CC.LOGIN_FIELD_MOBILE, strphone, updata, UpdateFlags.None);
                if (ret == "")
                {
                    //sendMsgToPhone(strphone, strphone);
                    // Response.Write("err_success");
                    m_dic.Add(CC.KEY_CODE, CC.SUCCESS);
                    if (data.ContainsKey("acc"))
                    {
                        m_dic.Add("acc", Convert.ToString(data["acc"]));
                    }
                }
                else
                {
                    m_dic.Add(CC.KEY_CODE, CC.ERR_DB);
                    //   Response.Write("err_system_error");
                }
            }
        }

        // 向手机phone发送验证码code
        private string sendMsgToPhone(string phone, string account)
        {
            SetUPInfo info = new SetUPInfo();
            string url = info.setUpMsgInfoSearchAccount(phone, account);
            var ret = HttpPost.Get(new Uri(url));
            if (ret != null)
            {
                string retstr = Encoding.UTF8.GetString(ret);
                retstr = info.adapterRetValue(retstr);
                if (retstr != "0")
                {
                    return "resFailed";  // 发送短信失败
                }
                return "resSuccess";
            }
            return "resFailed";  // 发送短信失败
        }    
    }
}