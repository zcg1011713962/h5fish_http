using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace HttpLogin.Default
{
    public partial class newOpVersionCtrl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.RequestType != "GET")
                return;

            //返回参数
            string channel = Request.Params["channel"];
            if (string.IsNullOrEmpty(channel))
            {
                ReturnMsg("-1", false, "参数非法");//data is null
                return;
            }

            try
            {
                int channelNo = 0;
                if (!int.TryParse(channel, out channelNo))
                {
                    ReturnMsg("-3", false, "参数非法");
                    return;
                }

                getDataList(channelNo);
            }
            catch (Exception)
            {
                ReturnMsg("-3", false, "未知错误");
            }
        }

        //根据渠道号获取版本号
        void getDataList(int channel)
        {
            List<Dictionary<string, object>> dataList = MongodbAccount.Instance.ExecuteGetListBykey("channelVerControl", "channel", channel);
            if (dataList == null || dataList.Count == 0)
            {
                ReturnMsg("-2", false, "该渠道号不存在");
                return;
            }

            List<Dictionary<string, object>> channelInfo = new List<Dictionary<string, object>>();
            Dictionary<string, object> data = new Dictionary<string, object>();
            for (int k = 0; k < dataList.Count; k++)
            {
                data.Clear();

                string time = "";
                if (dataList[k].ContainsKey("genTime"))
                    time = Convert.ToDateTime(dataList[k]["genTime"]).ToLocalTime().ToString();

                int channelNo = -1;
                if (dataList[k].ContainsKey("channel"))
                    channelNo = Convert.ToInt32(dataList[k]["channel"]);

                int version = 0;
                if (dataList[k].ContainsKey("version"))
                    version = Convert.ToInt32(dataList[k]["version"]);

                int turretLv = -1;
                if (dataList[k].ContainsKey("turretLv"))
                    turretLv = Convert.ToInt32(dataList[k]["turretLv"]);

                int vipLv = -1;
                if (dataList[k].ContainsKey("vipLv"))
                    vipLv = Convert.ToInt32(dataList[k]["vipLv"]);
                
                object[] onOff = new Object[]{0,0,0,0,0};
                if (dataList[k].ContainsKey("onOff"))
                    onOff = (object[])dataList[k]["onOff"];

                data.Add("time", time);
                data.Add("channel", channelNo);
                data.Add("version", version);
                data.Add("turretLv", turretLv);
                data.Add("vipLv", vipLv);
                data.Add("onOff", onOff);

                channelInfo.Add(data);
            }

            ReturnMsg1(dataList, true, "操作成功");
            return;
        }

        void ReturnMsg1(object param, bool ret = false, string acc = "")
        {
            List<Dictionary<string, object>> dataList = (List<Dictionary<string, object>>)param;

            Dictionary<string, object> data = new Dictionary<string, object>();
            data["res"] = ret;
            if (ret)
                data["data"] = dataList;

            data["msg"] = acc;

            string jsondata = JsonHelper.ConvertToStr(data);
            Response.Write(jsondata);
            return;
        }

        void ReturnMsg(string info, bool ret = false, string acc = "")
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["res"] = ret;
            if (ret)
                data["data"] = info;
            //if (ret)
            //{
            //    data["channel"] = info;
            //}
            //else
            //{
            //    data["error"] = info;
            //}

            data["msg"] = acc;
            
            string jsondata = JsonHelper.ConvertToStr(data);
            //Response.Write(Convert.ToBase64String(Encoding.Default.GetBytes(jsondata)));
            Response.Write(jsondata);
            return;
        }
    }
}