using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.channel
{
    public class QueryChannelWanYou : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            QueryChannel obj = new QueryChannel();
            obj.ChannelId = "100300";
            obj.TimeStr = context.Request.QueryString["time"];
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