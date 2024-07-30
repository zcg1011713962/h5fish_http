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
    /// OperationActivityCFGNew 的摘要说明
    /// </summary>
    public class OperationActivityCFGNew : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            GMUser user = (GMUser)context.Session["user"];

            int op = Convert.ToInt32(context.Request.Form["op"]);
            int actId = Convert.ToInt32(context.Request.Form["actId"]);
            string id = context.Request.Form["id"];
            string monday = context.Request.Form["monday"];

            OpRes res = OpRes.op_res_failed;

            ParamQuery p = new ParamQuery();

            p.m_op = op; //0新增   1编辑  2删除
            p.m_type = actId;
            p.m_param = monday;
            p.m_playerId = id;

            res = user.doDyop(p, DyOpType.opTypeOperationActivityCFGNew);

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