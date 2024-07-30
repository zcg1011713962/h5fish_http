using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// cdkeyNew 的摘要说明
    /// </summary>
    public class cdkeyNew : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.OTHER_CD_KEY, context.Session, context.Response);
            GMUser user = (GMUser)context.Session["user"];
            ParamCDKEY p = new ParamCDKEY();
            p.m_giftId = context.Request.Form["id"];
            p.m_op = 0;//删除
            string str = "";
            OpRes res = user.doDyop(p, DyOpType.opTypeGiftCodeNew);
            str = OpResMgr.getInstance().getResultString(res);
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