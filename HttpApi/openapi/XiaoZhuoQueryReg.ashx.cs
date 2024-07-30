using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{
    public class XiaoZhuoQueryReg : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CXiaoZhuoQueryReg obj = new CXiaoZhuoQueryReg();
            obj.AccTable = PayTable.XIAOZHUO_ACC;
            obj.OutErrPrefix = "CXiaoZhuoQueryReg";
            obj.Secret = CC.XIAOZHUO_SECRET;
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