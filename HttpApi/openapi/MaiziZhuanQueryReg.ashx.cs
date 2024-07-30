using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{
    public class MaiziZhuanQueryReg : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CMaiziZhuanQueryReg obj = new CMaiziZhuanQueryReg();
            obj.AccTable = PayTable.MAIZIZHUAN_ACC;
            obj.OutErrPrefix = "CMaiziZhuanQueryReg";
            obj.Secret = CC.MAIZIZHUAN_SECRET;
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