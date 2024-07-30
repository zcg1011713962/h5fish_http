using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// ServiceExchangeAudit 的摘要说明
    /// </summary>
    public class ServiceExchangeAudit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.SVR_EXCHANGE_AUDIT, context.Session, context.Response);

            GMUser user = (GMUser)context.Session["user"];
            ParamQuery p = new ParamQuery();

            p.m_op = Convert.ToInt32(context.Request.Form["op"]);
            p.m_param = context.Request.Form["id"];

            OpRes res = user.doDyop(p, DyOpType.opTypeOperationExchangeAudit);
            string str = OpResMgr.getInstance().getResultString(res);

            context.Response.ContentType = "text/plain";
            context.Response.Write((int)res + "#" + str);
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