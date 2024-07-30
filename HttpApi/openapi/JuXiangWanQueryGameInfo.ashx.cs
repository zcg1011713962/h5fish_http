using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{
    public class JuXiangWanQueryGameInfo : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CJuXiangWanQueryGame obj = new CJuXiangWanQueryGame();
            obj.Secret = CC.JUXIANGWAN_SECRET;
            obj.OutErrPrefix = "CJuXiangWanQueryGame";
            obj.StatusCodeName = "code";
            obj.PumpTableName = "pumpChannelPlayer100013";

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