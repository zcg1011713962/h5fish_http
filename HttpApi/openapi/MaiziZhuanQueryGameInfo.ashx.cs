using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{
    public class MaiziZhuanQueryGameInfo : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CMaiziZhuanQueryGame obj = new CMaiziZhuanQueryGame();
            obj.Secret = CC.MAIZIZHUAN_SECRET;
            obj.OutErrPrefix = "CMaiziZhuanQueryGame";
            obj.StatusCodeName = "status";
            obj.PumpTableName = "pumpChannelPlayer100012";

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