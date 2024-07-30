using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// FishlordLegendaryCheatScore 的摘要说明
    /// </summary>
    public class FishlordLegendaryCheatScore : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_FISHLORD_LEGENDARY_RANK_CHEAT, context.Session, context.Response);
            GMUser user = (GMUser)context.Session["user"];

            string playerId = context.Request["playerId"];
            string param = context.Request["scoreParam"];
            int op = Convert.ToInt32(context.Request["op"]);
            string nickName = context.Request["nickname"];
            CMemoryBuffer buffer = CommandMgr.getMemoryBuffer(user, true);
            buffer.Writer.Write(playerId);
            buffer.Writer.Write(op);
            buffer.Writer.Write(param);
            buffer.Writer.Write(nickName);
            CommandBase cmd = CommandMgr.processCmd(CmdName.AlterFishlordLegendaryRankControl, buffer, user);
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