using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// ShuihzScorePool 的摘要说明
    /// </summary>
    public class ShuihzScorePool : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.SHUIHZ_PLAYER_SCORE_POOL, context.Session, context.Response);
            ParamQuery param = new ParamQuery();

            param.m_op = Convert.ToInt32(context.Request.Form["op"]);
            param.m_playerId = context.Request.Form["playerId"];

            GMUser user = (GMUser)context.Session["user"];
            OpRes res = user.doDyop(param, DyOpType.opTypeShuihzPlayerScorePool);

            string str = "";

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("op", param.m_op);

            switch (param.m_op)
            {
                case 2:
                    {
                        List<ViewAddScorePoolItem> itemList =
                            (List<ViewAddScorePoolItem>)user.getSys<DyOpMgr>(SysType.sysTypeDyOp).getResult(DyOpType.opTypeShuihzPlayerScorePool);
                        data.Add("buffList", BaseJsonSerializer.serialize(itemList));
                        str = ItemHelp.genJsonStr(data);
                    }
                    break;
                case 1:
                case 3:
                    {
                        data.Add("result", (int)res);
                        data.Add("playerId", param.m_playerId);
                        str = ItemHelp.genJsonStr(data);
                    }
                    break;
            }

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