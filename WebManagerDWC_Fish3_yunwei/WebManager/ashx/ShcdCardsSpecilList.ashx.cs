using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// ShcdCardsSpecilList 的摘要说明
    /// </summary>
    public class ShcdCardsSpecilList : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.SHCD_CARDS_SPECIL_LIST, context.Session, context.Response);
            ParamQuery param = new ParamQuery();

            param.m_op = Convert.ToInt32(context.Request.Form["op"]);
            param.m_playerId = context.Request.Form["playerId"];

            GMUser user = (GMUser)context.Session["user"];
            
            string str = "";
            switch (param.m_op)
            {
                case 2:
                    {
                        OpRes res = user.doQuery(param, QueryType.queryTypeShcdCardsSpecilList);

                        Dictionary<string, object> data = new Dictionary<string, object>();
                        List<specilListItem> itemList = (List<specilListItem>)user.getQueryResult(QueryType.queryTypeShcdCardsSpecilList);
                        data.Add("buffList", BaseJsonSerializer.serialize(itemList));
                        data.Add("op", param.m_op);
                        str = ItemHelp.genJsonStr(data);
                    }
                    break;
                case 1:
                    {
                        CMemoryBuffer buffer = CommandMgr.getMemoryBuffer(user, true);
                        buffer.Writer.Write(param.m_playerId);
                        buffer.Writer.Write(0);
                        buffer.Writer.Write(1);//删除
                        CommandBase cmd = CommandMgr.processCmd(CmdName.SetShcdCardsSpecilList, buffer, user);
                        OpRes res = cmd.getOpRes();
                        Dictionary<string, object> data = new Dictionary<string, object>();
                        data.Add("op", param.m_op);
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