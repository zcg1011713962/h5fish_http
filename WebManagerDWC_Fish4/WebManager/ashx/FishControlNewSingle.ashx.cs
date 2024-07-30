using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// FishControlNewSingle 的摘要说明
    /// </summary>
    public class FishControlNewSingle : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_PARAM_CONTROL, context.Session, context.Response);

            GMUser user = (GMUser)context.Session["user"];
            ParamFishlord5 p = new ParamFishlord5();
            p.m_isReset = false;
            p.m_isNewAlg = true;

            p.m_roomList = "1";

            p.m_baseRate = context.Request.Form["baseRate"];
            p.m_deviationFix = context.Request.Form["deviationFix"];
            p.m_checkRate = context.Request.Form["noValuePlayerRate"];

            p.m_rightId = RightDef.FISH_PARAM_CONTROL;
            OpRes res = user.doDyop(p, DyOpType.opTypeFishlordSingleParamAdjustNew);
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