using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    public class FishControl : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_PARAM_CONTROL, context.Session, context.Response);

            GMUser user = (GMUser)context.Session["user"];
            ParamFishlordNew p = new ParamFishlordNew();
            p.m_isReset = false;
            p.m_isNewAlg = true;
            p.m_roomList = context.Request.Form["room"];

            p.m_expRate = context.Request.Form["expRate"];
            p.m_maxEarnValue = context.Request.Form["maxEarnValue"];
            p.m_minEarnValue = context.Request.Form["minEarnValue"];
            p.m_startEarnValue = context.Request.Form["startEarnValue"];
            p.m_minControlEarnValue = context.Request.Form["minControlEarnValue"];
            p.m_maxControlEarnValue = context.Request.Form["maxControlEarnValue"];
            
            p.m_rightId = RightDef.FISH_PARAM_CONTROL;
            OpRes res = user.doDyop(p, DyOpType.opTypeFishlordParamAdjust);
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