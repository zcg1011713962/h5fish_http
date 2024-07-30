using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{   
    public class DanDanZhuanQueryReg : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CDanDanZhuanQueryReg obj = new CDanDanZhuanQueryReg();
            string str = obj.queryReg(context.Request);

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