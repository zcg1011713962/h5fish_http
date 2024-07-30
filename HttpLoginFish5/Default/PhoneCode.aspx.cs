using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;

namespace HttpLogin.Default
{
    // 账号找回时，发送验证码， 需要传入手机号
    public partial class PhoneCode : System.Web.UI.Page
    {
        Random m_rd = new Random();
        private Dictionary<string, object> m_dic = new Dictionary<string, object>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.RequestType != "POST")
                return;

            try
            {
                do
                {
                    string strphone = Request.Params["phone"];
                    if (string.IsNullOrEmpty(strphone))
                    {
                        m_dic.Add(CC.KEY_CODE, CC.ERR_PHONE);
                        break;
                        //   return;
                    }

                    if (strphone.Length != 11 || !Regex.IsMatch(strphone, @"^\d{11}$"))
                    {
                        m_dic.Add(CC.KEY_CODE, CC.ERR_PHONE);
                        break;
                        //  Response.Write("err_not_phone");//号码错误
                        //  return;
                    }

                    string[] field = { "searchTime", "searchCount" };
                    Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetBykey("AccountTable", CC.LOGIN_FIELD_MOBILE, strphone, field);
                    if (data == null)
                    {
                        //  Response.Write("err_not_bind");//未绑定  
                        m_dic.Add(CC.KEY_CODE, CC.ERR_NOT_BINDPHONE);
                        break;
                    }
                    else
                    {
                        DateTime last = DateTime.MinValue; // 账号找回时间
                        if (data.ContainsKey("searchTime"))
                        {
                            last = Convert.ToDateTime(data["searchTime"]).ToLocalTime();
                        }

                        DateTime now = DateTime.Now;
                        TimeSpan span = now - last;
                        int interval = Convert.ToInt32(ConfigurationManager.AppSettings["search_interval"]);
                        if (span.TotalSeconds < interval)
                        {
                            m_dic.Add(CC.KEY_CODE, CC.ERR_CODE_TIME_CD);
                            break;
                            // Response.Write("err_timecd");//验证码cd时间
                            //return;
                        }

                        int searchCount = 0;
                        if (data.ContainsKey("searchCount"))
                        {
                            searchCount = Convert.ToInt32(data["searchCount"]);
                        }

                        if (last.Date != now.Date)
                            searchCount = 0;

                        int limitCount = Convert.ToInt32(ConfigurationManager.AppSettings["search_count"]);
                        if (searchCount >= limitCount)
                        {
                            m_dic.Add(CC.KEY_CODE, CC.ERR_CODE_RUN_OUT_COUNT);
                            break;
                            // Response.Write("err_maxcount");//当日次数已满
                            // return;
                        }

                        string pwdcode = m_rd.Next(100000, 999999).ToString();

                        data["searchCount"] = ++searchCount;
                        data["searchTime"] = now;
                        data["pwdcode"] = pwdcode;
                        MongodbAccount.Instance.ExecuteUpdate("AccountTable", CC.LOGIN_FIELD_MOBILE, strphone, data);

                      //  CLOG.Info("phone={0}, code={1}", strphone, pwdcode);

                        int ret = sendMsgToPhone(strphone, pwdcode);
                        m_dic.Add(CC.KEY_CODE, ret);
                        break;
                        //Response.Write(ret);
                    }

                } while (false);
            }
            catch (Exception ex)
            {
                m_dic.Add(CC.KEY_CODE, CC.ERR_UNKNOWN);
            }

            Response.Write(JsonHelper.genJson(m_dic));
        }

        int sendMsgToPhone(string phone, string pwdcode)
        {
            SetUPInfo info = new SetUPInfo();
            string url = info.setUpMsgInfoPhoneCode(phone, pwdcode);
            var ret = HttpPost.Get(new Uri(url));
            if (ret != null)
            {
                string retstr = Encoding.UTF8.GetString(ret);
                retstr = info.adapterRetValue(retstr);
                if (retstr != "0")
                {
                    //return "err_sendfailed";  // 发送短信失败
                    return CC.ERR_CODE_SEND_FAILED;
                }
                //return "err_success";
                return CC.SUCCESS;
            }
            //return "err_sendfailed";  // 发送短信失败
            return CC.ERR_CODE_SEND_FAILED;
        }    
    }
}