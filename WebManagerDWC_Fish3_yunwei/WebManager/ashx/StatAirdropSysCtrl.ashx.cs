using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// StatAirdropSysCtrl 的摘要说明
    /// </summary>
    public class StatAirdropSysCtrl : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_AIR_DROP_SYS, context.Session, context.Response);

            GMUser user = (GMUser)context.Session["user"];
            ParamAirdropSysItem p = new ParamAirdropSysItem();

            p.m_op = 1;
            p.m_uuid = context.Request.Form["id"];
            OpRes res = user.doDyop(p, DyOpType.opTypeStatAirdropSys);
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