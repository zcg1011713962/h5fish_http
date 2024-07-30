using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpApi.openapi
{
    public class XiaoZhuoQueryGameInfo : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            CXiaoZhuoQueryGame obj = new CXiaoZhuoQueryGame();
            obj.Secret = CC.XIAOZHUO_SECRET;
            obj.OutErrPrefix = "CXiaoZhuoQueryGame";
            obj.StatusCodeName = "status";
            obj.PumpTableName = "pumpChannelPlayer100014";

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