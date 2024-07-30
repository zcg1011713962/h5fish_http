using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{
    public class PaoPaoZhuanQueryReg : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CPaoPaoZhuanQueryReg obj = new CPaoPaoZhuanQueryReg();
            obj.AccTable = PayTable.PAOPAOZHUAN_ACC;
            obj.OutErrPrefix = "CPaoPaoZhuanQueryReg";
            obj.Secret = CC.PAOPAOZHUAN_SECRET;
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