using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// StatFishlordAdvancedRoomCtrl 的摘要说明
    /// </summary>
    public class StatFishlordAdvancedRoomCtrl : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_ADVANCED_ROOM_CTRL, context.Session, context.Response);

            GMUser user = (GMUser)context.Session["user"];

            ParamFishlordAdvancedRoomItem p = new ParamFishlordAdvancedRoomItem();
            p.m_op = Convert.ToInt32(context.Request.Form["op"]);
            p.m_maxWinCount = Convert.ToInt32(context.Request.Form["maxWinCount"]);
            p.m_ratio = Convert.ToInt32(context.Request.Form["rewardRatio"]);

            string str = "";
            OpRes res = user.doDyop(p, DyOpType.opTypeFishlordAdvancedRoomCtrl);
            str = OpResMgr.getInstance().getResultString(res);
            context.Response.ContentType = "text/plain";
            context.Response.Write((int)res + "#" + str + "#" + p.m_maxWinCount + "," + p.m_ratio);
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