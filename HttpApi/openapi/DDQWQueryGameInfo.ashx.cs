using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{
    // 豆豆趣玩
    public class DDQWQueryGameInfo : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CDDQWQueryGame obj = new CDDQWQueryGame();
            obj.Secret = CC.DDQW_SECRET;
            obj.OutErrPrefix = "CDDQWQueryGame";
            obj.StatusCodeName = "status";
            obj.PumpTableName = "pumpChannelPlayer100016";

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