using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// OperationNotify 的摘要说明
    /// </summary>
    public class OperationNotify : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_OPERATION_NOTICE, context.Session, context.Response);

            GMUser user = (GMUser)context.Session["user"];

            string str = "";
            ParamNotify p = new ParamNotify();
            p.m_op = Convert.ToInt32(context.Request.Form["op"]);
            if (p.m_op == 0) //提交
            {
                p.m_id = context.Request.Form["id"];
                p.m_title = context.Request.Form["title"];
                p.m_content = context.Request.Form["content"];
                p.m_startTime = context.Request.Form["start"];
                p.m_endTime = context.Request.Form["end"];
                p.m_order = context.Request.Form["order"];
                p.m_comment = context.Request.Form["comment"];

                OpRes res = user.doDyop(p, DyOpType.opTypeOperationNotify);
                str = OpResMgr.getInstance().getResultString(res);
            }
            else  //编辑
            {
                p.m_id = context.Request.Form["noticeId"];
                OpRes res = user.doQuery(p, QueryType.queryTypeNoticeInfo);
                if (res == OpRes.opres_success)
                {
                    ResultNoticeInfo qresult = (ResultNoticeInfo)user.getQueryResult(QueryType.queryTypeNoticeInfo);
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Add("res", res);
                    data.Add("dataList", BaseJsonSerializer.serialize(qresult));
                    str = ItemHelp.genJsonStr(data);
                }
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