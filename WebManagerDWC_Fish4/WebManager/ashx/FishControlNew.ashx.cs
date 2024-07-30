using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// FishControlNew 的摘要说明
    /// </summary>
    public class FishControlNew : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_PARAM_CONTROL, context.Session, context.Response);

            GMUser user = (GMUser)context.Session["user"];
            ParamFishlord5 p = new ParamFishlord5();
            p.m_isReset = false;
            p.m_isNewAlg = true;
            p.m_roomList = context.Request.Form["room"];

            p.m_jsckpotGrandPump = context.Request.Form["jsckpotGrandPump"];
            p.m_jsckpotSmallPump = context.Request.Form["jsckpotSmallPump"];
            p.m_normalFishRoomPoolPumpParam = context.Request.Form["normalFishRoomPoolPumpParam"];

            p.m_baseRate = context.Request.Form["rateCtr"];
            p.m_checkRate = context.Request.Form["checkRate"];
            p.m_trickDeviationFix = context.Request.Form["trickDeviationFix"];

            p.m_incomeThreshold = context.Request.Form["incomeThreshold"];
            p.m_earnRatemCtrMax = context.Request.Form["earnRatemCtrMax"];
            p.m_earnRatemCtrMin = context.Request.Form["earnRatemCtrMin"];

            p.m_rightId = RightDef.FISH_PARAM_CONTROL;
            OpRes res = user.doDyop(p, DyOpType.opTypeFishlordParamAdjustNew);
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