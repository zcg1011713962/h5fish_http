using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace h5.api
{
    /// <summary>
    /// ActRecvBenefit 的摘要说明
    /// </summary>
    public class ActRecvBenefit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string playerId = context.Request.Form["playerId"];
            // 0 获取状态  1请求领取
            string op = context.Request.Form["op"];
            string ret = "";

            if (string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(op))
            {
                Dictionary<string, object> retdata = new Dictionary<string, object>();
                retdata.Add(RetResult.KEY_RESULT, RetResult.RET_PARAM_ERROR);
                ret = JsonHelper.genJson(retdata);
            }
            else
            {
                CMemoryBuffer buf = CommandBase.createBuf();
                BinaryWriter w = buf.Writer;
                w.Write(op);
                w.Write(playerId);
                CommandRecvBenefit cmd = new CommandRecvBenefit();
                ret = cmd.execute(buf);
            }

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