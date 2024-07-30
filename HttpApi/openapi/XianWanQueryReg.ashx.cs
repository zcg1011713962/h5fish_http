using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{
    // 闲玩查询注册信息
    public class XianWanQueryReg : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CXWQueryReg obj = new CXWQueryReg();
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