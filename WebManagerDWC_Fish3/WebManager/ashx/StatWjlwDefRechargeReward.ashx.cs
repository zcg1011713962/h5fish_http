using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// StatWjlwDefRechargeReward 的摘要说明
    /// </summary>
    public class StatWjlwDefRechargeReward : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck("", context.Session, context.Response);
            GMUser user = (GMUser)context.Session["user"];
            ParamQuery p = new ParamQuery();
            p.m_param = context.Request["id"];
            OpRes res = user.doDyop(p, DyOpType.opTypeStatWjlwDefRechargeReward);

            string str = OpResMgr.getInstance().getResultString(res);
            context.Response.ContentType = "text/plain";
            context.Response.Write((int)res + "#" + str + "#" + p.m_param);
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