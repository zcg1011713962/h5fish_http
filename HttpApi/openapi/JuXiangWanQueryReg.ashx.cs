using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{
    public class JuXiangWanQueryReg : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CJuXiangWanQueryReg obj = new CJuXiangWanQueryReg();
            obj.AccTable = PayTable.JUXIANGWAN_ACC;
            obj.OutErrPrefix = "CJuXiangWanQueryReg";
            obj.Secret = CC.JUXIANGWAN_SECRET;
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