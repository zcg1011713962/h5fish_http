using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// FishScorePool 的摘要说明
    /// </summary>
    public class FishScorePool : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_PLAYER_SCORE_POOL, context.Session, context.Response);
            ParamGameBItem param = new ParamGameBItem();

            param.m_op = Convert.ToInt32(context.Request.Form["op"]);
            param.m_playerId = context.Request.Form["playerId"];
            param.m_type = Convert.ToInt32(context.Request.Form["moneyType"]);
            param.m_param = context.Request.Form["id"];

            GMUser user = (GMUser)context.Session["user"];
            OpRes res = user.doDyop(param, DyOpType.opTypePlayerScorePool);

            string str = "";

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("op", param.m_op);

            switch (param.m_op)
            {
                case 2:
                    {
                        List<ViewAddScorePoolItem> itemList =
                            (List<ViewAddScorePoolItem>)user.getSys<DyOpMgr>(SysType.sysTypeDyOp).getResult(DyOpType.opTypePlayerScorePool);
                        data.Add("buffList", BaseJsonSerializer.serialize(itemList));
                        str = ItemHelp.genJsonStr(data);
                    }
                    break;
                case 1:
                case 3:
                    {
                        data.Add("result", (int)res);
                        data.Add("playerId", param.m_playerId);
                        data.Add("m_id", param.m_param);
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