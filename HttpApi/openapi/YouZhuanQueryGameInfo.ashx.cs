using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{
    public class YouZhuanQueryGameInfo : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CYouZhuanQueryGame obj = new CYouZhuanQueryGame();
            obj.Secret = CC.YOUZHUAN_SECRET;
            obj.OutErrPrefix = "CYouZhuanQueryGame";
            obj.StatusCodeName = "status";
            obj.PumpTableName = "pumpChannelPlayer100011";

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