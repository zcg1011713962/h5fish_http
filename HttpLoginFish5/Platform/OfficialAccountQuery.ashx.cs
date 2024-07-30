using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpLogin.Platform
{
    /// <summary>
    /// OfficialAccountQuery 的摘要说明
    /// </summary>
    public class OfficialAccountQuery : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            COfficialAccountsQuery obj = new COfficialAccountsQuery();
            obj.AppId = "wx865f2be166ff5c91";
            obj.AppSecret = "6e93d5a0a268139ae30ea3053be7246c";
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