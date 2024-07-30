using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// FishlordMythicalRoomCheatPlayerScore 的摘要说明
    /// </summary>
    public class FishlordMythicalRoomCheatPlayerScore : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_PUMP_MYTHICAL_ROOM_RANK_CHEAT, context.Session, context.Response);
            GMUser user = (GMUser)context.Session["user"];

            string playerId = context.Request["playerId"];
            string param = context.Request["scoreParam"];
            string nickName = context.Request["nickname"];
            CMemoryBuffer buffer = CommandMgr.getMemoryBuffer(user, true);
            buffer.Writer.Write(playerId);
            buffer.Writer.Write(param);
            buffer.Writer.Write(nickName);
            CommandBase cmd = CommandMgr.processCmd(CmdName.AlterFishlordMythicalRankControl, buffer, user);
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