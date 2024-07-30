using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// ChannelOpenCloseGame 的摘要说明
    /// </summary>
    public class ChannelOpenCloseGame : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.SVR_CHANNEL_OPEN_CLOSE_GAME, context.Session, context.Response);
            ParamQuery param = new ParamQuery();
            param.m_op = Convert.ToInt32(context.Request.Form["op"]);
            param.m_channelNo = context.Request.Form["channelNo"];

            GMUser user = (GMUser)context.Session["user"];

            string str = "";
            switch (param.m_op)
            {
                case 2:  //刷新列表
                    {
                        OpRes res = user.doQuery(param, QueryType.queryTypeChannelOpenCloseGame);

                        Dictionary<string, object> data = new Dictionary<string, object>();
                        List<channelOpenCloseGameList> itemList = (List<channelOpenCloseGameList>)user.getQueryResult(QueryType.queryTypeChannelOpenCloseGame);
                        data.Add("dataList", BaseJsonSerializer.serialize(itemList));
                        data.Add("op", param.m_op);
                        str = ItemHelp.genJsonStr(data);
                    }
                    break;
                case 1:
                    {
                        CMemoryBuffer buffer = CommandMgr.getMemoryBuffer(user, true);
                        buffer.Writer.Write(param.m_channelNo);
                        buffer.Writer.Write(1);//删除
                        CommandBase cmd = CommandMgr.processCmd(CmdName.SetChannelOpenCloseGame, buffer, user);
                        OpRes res = cmd.getOpRes();
                        Dictionary<string, object> data = new Dictionary<string, object>();
                        data.Add("op", param.m_op);
                        data.Add("result", (int)res);
                        data.Add("channel", param.m_channelNo);
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