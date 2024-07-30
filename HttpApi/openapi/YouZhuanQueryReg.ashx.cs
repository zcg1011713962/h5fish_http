using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{
    public class YouZhuanQueryReg : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CYouZhuanQueryReg obj = new CYouZhuanQueryReg();
            obj.AccTable = PayTable.YOUZHUAN_ACC;
            obj.OutErrPrefix = "CYouZhuanQueryReg";
            obj.Secret = CC.YOUZHUAN_SECRET;
            obj.StatusCodeName = "status";

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