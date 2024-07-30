using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

namespace WebManager.ashx
{
    /// <summary>
    /// OperationAd100003ActivityCFG 的摘要说明
    /// </summary>
    public class OperationAd100003ActivityCFG : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            GMUser user = (GMUser)context.Session["user"];

            int itemId = Convert.ToInt32(context.Request.Form["itemId"]);
            string code = context.Request.Form["qihao"].Trim();
            string startTime = context.Request.Form["startTime"].Trim();
            string endTime = context.Request.Form["endTime"].Trim();

            Match m1 = Regex.Match(startTime, Exp.DATE_TIME5);
            Match m2 = Regex.Match(endTime, Exp.DATE_TIME5);
            OpRes res = OpRes.op_res_failed;
            if (m1.Success && m2.Success)
            {
                CMemoryBuffer buffer = CommandMgr.getMemoryBuffer(user, true);
                buffer.Writer.Write(itemId);
                buffer.Writer.Write(code);
                buffer.Writer.Write(startTime);
                buffer.Writer.Write(endTime);
                CommandBase cmd = CommandMgr.processCmd(CmdName.AlterOperationAd100003ActSet, buffer, user);
                res = cmd.getOpRes();
            }
            else
            {
                res = OpRes.op_res_param_not_valid;
            }

            string str = OpResMgr.getInstance().getResultString(res);

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