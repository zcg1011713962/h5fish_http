using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{
    public class HuluQueryReg : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CHuluQueryReg obj = new CHuluQueryReg();
            obj.AccTable = PayTable.HULU_ACC;
            obj.OutErrPrefix = "CHuluQueryReg";
            obj.Secret = CC.HULU_SECRET;
            obj.StatusCodeName = "code";

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