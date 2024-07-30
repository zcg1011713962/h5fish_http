using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    public class ServiceRobotRole : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_BULLET_HEAD_HEAD, context.Session, context.Response);
            GMUser user = (GMUser)context.Session["user"];

            Dictionary<string, object> retVal = new Dictionary<string, object>();

            do
            {
                int op = Convert.ToInt32(context.Request.Form["op"]);
                int robotId = Convert.ToInt32(context.Request.Form["robotId"]);
                string robot = Convert.ToString(context.Request.Form["data"]);
                CommandRobotRole cmd = new CommandRobotRole();
                CMemoryBuffer buf = new CMemoryBuffer();
                buf.begin();
                buf.Writer.Write(op);
                buf.Writer.Write(robotId);
                buf.Param1 = robot;
                OpRes res = cmd.execute(buf, user);
                retVal.Add("result", (int)res);

                if (op == DefCC.OP_VIEW)
                {
                    RobotInfoResult r = (RobotInfoResult)cmd.getResult(user);
                    string content = JsonHelper.ConvertToStr(r);
                    retVal.Add("content", content);

                    string str = OpResMgr.getInstance().getResultString(res);
                    retVal.Add("resultStr", str);
                }
                else
                {
                    string str = OpResMgr.getInstance().getResultString(res);
                    retVal.Add("resultStr", str);
                }
                
            } while (false);

            context.Response.ContentType = "text/plain";
            context.Response.Write(JsonHelper.ConvertToStr(retVal));
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