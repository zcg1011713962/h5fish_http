using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// PlayerBasicInfo 的摘要说明
    /// </summary>
    public class PlayerBasicInfo : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck("", context.Session, context.Response);
            ParamQuery p = new ParamQuery();

            p.m_op = Convert.ToInt32(context.Request.Form["op"]);
            p.m_playerId = context.Request.Form["playerId"];
            p.m_param = context.Request.Form["newVal"];

            GMUser user = (GMUser)context.Session["user"];
            OpRes res = user.doDyop(p, DyOpType.opTypePlayerBasicInfoChange);
            string str = OpResMgr.getInstance().getResultString(res);
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