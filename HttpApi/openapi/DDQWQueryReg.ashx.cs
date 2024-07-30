using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{
    // 豆豆趣玩
    public class DDQWQueryReg : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CDDQWQueryReg obj = new CDDQWQueryReg();
            obj.AccTable = PayTable.DDQW_ACC;
            obj.OutErrPrefix = "CDDQWQueryReg";
            obj.Secret = CC.DDQW_SECRET;
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