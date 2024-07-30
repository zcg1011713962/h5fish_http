using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// ActivityPanicBuyingCfg 的摘要说明
    /// </summary>
    public class ActivityPanicBuyingCfg : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_ACTIVITY_PANIC_BUYING_CFG, context.Session, context.Response);
            GMUser user = (GMUser)context.Session["user"];
            paramActivityPanicBuyingCfgParamAdjust p =new paramActivityPanicBuyingCfgParamAdjust();
            p.m_activityList=context.Request["activityId"];
            p.m_maxCount=context.Request["maxCount"];
            p.m_rightId = RightDef.DATA_ACTIVITY_PANIC_BUYING_CFG;
            OpRes res = user.doDyop(p, DyOpType.opTypeActivityPanicBuyingCfgParamAdjust);

            string str = OpResMgr.getInstance().getResultString(res);
            context.Response.ContentType = "text/plain";
            context.Response.Write((int)res + "#" + str + "#" + p.m_maxCount.ToString() + "#" + p.m_activityList);
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