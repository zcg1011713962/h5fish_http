using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{
    public class HuluQueryGameInfo : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CHuluQueryGame obj = new CHuluQueryGame();
            obj.Secret = CC.HULU_SECRET;
            obj.OutErrPrefix = "CHuluQueryGame";
            obj.StatusCodeName = "code";
            obj.PumpTableName = "pumpChannelPlayer100010";

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