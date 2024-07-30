using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace h5.api
{
    /// <summary>
    /// NoticeList 的摘要说明
    /// </summary>
    public class NoticeList : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string ret = "";
            CMemoryBuffer buf = CommandBase.createBuf();
            CommandNoticeList cmd = new CommandNoticeList();
            ret = cmd.execute(buf);

            context.Response.ContentType = "text/plain";
            context.Response.Write(ret);
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