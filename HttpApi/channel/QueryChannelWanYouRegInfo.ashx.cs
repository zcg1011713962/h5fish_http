using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.channel
{
    // 查询万游是否注册过某个ID
    public class QueryChannelWanYouRegInfo : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CQueryChannelRegInfo obj = new CQueryChannelRegInfo();
            obj.ChannelId = PayTable.CHANNEL_WANYOU;
            obj.AccTable = PayTable.WANYOU_ACC;
            obj.Id = context.Request.QueryString["id"];
            string str = obj.queryData();
            context.Response.ContentType = "text/plain";
            context.Response.Write(str);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}