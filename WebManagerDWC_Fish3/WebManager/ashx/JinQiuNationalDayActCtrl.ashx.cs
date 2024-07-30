using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Drawing;
using System.IO;

namespace WebManager.ashx
{
    /// <summary>
    /// JinQiuNationalDayActCtrl 的摘要说明
    /// </summary>
    public class JinQiuNationalDayActCtrl : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_JINQIU_NATIONAL_ACT_CTRL, context.Session, context.Response);
            GMUser user = (GMUser)context.Session["user"];

            string playerId = context.Request["playerId"];
            string param = context.Request["scoreParam"];
            string oldParam = context.Request["oldScore"];
            CMemoryBuffer buffer = CommandMgr.getMemoryBuffer(user, true);
            buffer.Writer.Write(playerId);
            buffer.Writer.Write(param);
            buffer.Writer.Write(oldParam);
            CommandBase cmd = CommandMgr.processCmd(CmdName.AlterJinQiuNationalDayAct, buffer, user);
            OpRes res = cmd.getOpRes();

            string str = OpResMgr.getInstance().getResultString(res);
            context.Response.ContentType = "text/plain";
            context.Response.Write((int)res);
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