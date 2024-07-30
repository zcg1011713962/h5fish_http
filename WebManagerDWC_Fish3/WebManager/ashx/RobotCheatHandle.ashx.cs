using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    public class RobotCheatHandle : IHttpHandler, IRequiresSessionState
    {
        public const int OP_SEARCH = 1;
        public const int OP_MODIFY = 2;
        public const int ROBOT_MIN_ID = 10099001;
        public const int ROBOT_MAX_ID = 10099200;

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            GMUser user = (GMUser)context.Session["user"];

            Dictionary<string, object> retVal = new Dictionary<string, object>();

            do
            {
                int op = Convert.ToInt32(context.Request.Form["op"]);
                string robot = Convert.ToString(context.Request.Form["robot"]);

                retVal.Add("op", op);
                retVal.Add("robot", robot);

                var obj = RobotCheatBase.create(robot);
                if (obj == null || user == null)
                {
                    retVal.Add("resultStr", "unknown");
                    retVal.Add("result", 1);
                    break;
                }

                if (op == OP_SEARCH)
                {
                    search(user, retVal, obj, context);
                }
                else if (op == OP_MODIFY)
                {
                    modify(user, retVal, obj, context);
                }
            } while (false);

            context.Response.ContentType = "text/plain";
            context.Response.Write(JsonHelper.ConvertToStr(retVal));
        }

        void search(GMUser user, Dictionary<string, object> retVal, RobotCheatBase obj, HttpContext context)
        {
            int playerId = Convert.ToInt32(context.Request.Form["playerId"]);
            string other = Convert.ToString(context.Request.Form["other"]);
            CMemoryBuffer buf = new CMemoryBuffer();
            buf.Param1 = context.Request.Form;
            buf.Writer.Write(playerId);
            if (!string.IsNullOrEmpty(other))
            {
                buf.Writer.Write(other);
            }
            OpRes code = obj.search(user, buf);
            if (code == OpRes.opres_success)
            {
                RobotCheatArenaRes d = (RobotCheatArenaRes)obj.getSearchResult();
                if (d != null)
                {
                    retVal.Add("resultStr", d.getJsonStr());
                }
                else
                {
                    retVal.Add("resultStr", "unknown");
                }
            }
            else
            {
                retVal.Add("resultStr", OpResMgr.getInstance().getResultString(code));
            }

            retVal.Add("result", (int)code);
        }

        void modify(GMUser user, Dictionary<string, object> retVal, RobotCheatBase obj, HttpContext context)
        {
            int playerId = Convert.ToInt32(context.Request.Form["playerId"]);
            string nickName = Convert.ToString(context.Request.Form["nickName"]);
            int scoreDay = Convert.ToInt32(context.Request.Form["scoreDay"]);
            int scoreWeek = Convert.ToInt32(context.Request.Form["scoreWeek"]);

            CMemoryBuffer buf = new CMemoryBuffer();
            buf.Param1 = context.Request.Form;
            buf.Writer.Write(playerId);
            buf.Writer.Write(nickName);
            buf.Writer.Write(scoreDay);
            buf.Writer.Write(scoreWeek);
            buf.Writer.Write(isRobot(playerId));
            OpRes code = obj.modify(user, buf);
            retVal.Add("resultStr", OpResMgr.getInstance().getResultString(code));
            retVal.Add("result", (int)code);
        }

        bool isRobot(int id)
        {
            return id >= ROBOT_MIN_ID && id <= ROBOT_MAX_ID;
        }
    }
}