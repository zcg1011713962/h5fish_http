using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{
    /// <summary>
    /// roleinfo 的摘要说明
    /// </summary>
    public class roleinfo : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            ApiQueryData obj = new ApiQueryData();
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