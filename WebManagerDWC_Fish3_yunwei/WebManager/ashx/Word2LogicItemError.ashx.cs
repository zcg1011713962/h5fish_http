using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// Word2LogicItemError 的摘要说明
    /// </summary>
    public class Word2LogicItemError : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_WORD2_LOGIC_ITEM_ERROR, context.Session, context.Response);

            GMUser user = (GMUser)context.Session["user"];
            ParamQuery p = new ParamQuery();
            p.m_param = context.Request.Form["id"];

            OpRes res = user.doDyop(p, DyOpType.opTypeOperationIsDeal);
            context.Response.ContentType = "text/plain";
            context.Response.Write(res);
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