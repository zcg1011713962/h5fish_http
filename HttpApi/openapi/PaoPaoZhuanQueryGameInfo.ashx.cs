using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{
    public class PaoPaoZhuanQueryGameInfo : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CPaoPaoZhuanQueryGame obj = new CPaoPaoZhuanQueryGame();
            obj.Secret = CC.PAOPAOZHUAN_SECRET;
            obj.OutErrPrefix = "CPaoPaoZhuanQueryGame";
            obj.StatusCodeName = "status";
            obj.PumpTableName = "pumpChannelPlayer100015";

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