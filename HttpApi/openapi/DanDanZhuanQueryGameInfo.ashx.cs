using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{
    public class DanDanZhuanQueryGameInfo : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CDanDanZhuanQueryPlayerInfo obj = new CDanDanZhuanQueryPlayerInfo();
            string str = obj.doQuery(context.Request);
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