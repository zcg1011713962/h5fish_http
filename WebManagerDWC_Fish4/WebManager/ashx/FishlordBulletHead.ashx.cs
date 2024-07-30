using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// FishlordBulletHead 的摘要说明
    /// </summary>
    public class FishlordBulletHead : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_BULLET_HEAD_HEAD, context.Session, context.Response);

            GMUser user = (GMUser)context.Session["user"];

            ParamQuery p = new ParamQuery();
            p.m_op = Convert.ToInt32(context.Request.Form["id"]);
            //4使用范围  5 杀鱼范围
            p.m_playerId = context.Request.Form["type"];
            p.m_param = context.Request.Form["goldKillMin"];
            p.m_channelNo = context.Request.Form["goldKillMax"];

            string str = "";
            OpRes res = user.doDyop(p, DyOpType.opTypeFishlordBulletHeadRcParam);
                str = OpResMgr.getInstance().getResultString(res);
            context.Response.ContentType = "text/plain";
            context.Response.Write((int)res + "#" + str + "#" + p.m_param +","+ p.m_channelNo);
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