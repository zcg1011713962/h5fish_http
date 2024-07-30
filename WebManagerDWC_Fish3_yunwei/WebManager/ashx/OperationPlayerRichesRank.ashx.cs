using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// OperationPlayerRichesRank 的摘要说明
    /// </summary>
    public class OperationPlayerRichesRank : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_PLAYER_RICHES_RANK, context.Session, context.Response);

            GMUser user = (GMUser)context.Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = context.Request.Form["time"];
            param.m_param = context.Request.Form["rankId"];
            OpRes res = user.doQuery(param, QueryType.queryTypePlayerRichesRank);
            string str = "";
            if (res == OpRes.opres_success)
            {
                PlayerRichesRank qresult = (PlayerRichesRank)user.getQueryResult(QueryType.queryTypePlayerRichesRank);
                str = qresult.getJson(user);
            }

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