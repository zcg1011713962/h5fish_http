using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// ServiceBindUnbindPhone 的摘要说明
    /// </summary>
    public class ServiceBindUnbindPhone : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck("", context.Session, context.Response);

            GMUser user = (GMUser)context.Session["user"];

            ParamQuery p = new ParamQuery();
            p.m_type = Convert.ToInt32(context.Request.Form["opType"]);

            p.m_playerId = context.Request.Form["playerId"];
            p.m_param = context.Request.Form["phoneNo"];

            OpRes res = OpRes.op_res_failed;
            string str = "";
            //1验证手机  2绑定 3移除 4 查询
            if (p.m_type == 1 || p.m_type == 4)  //查询
            {
                Dictionary<string, object> jd = new Dictionary<string, object>();
                if (p.m_type == 1)
                {
                    res = user.doDyop(p, DyOpType.opTypeServiceVertifyPhoneNo);
                    str = OpResMgr.getInstance().getResultString(res);

                    jd.Add("result", OpResMgr.getInstance().getResultString(res));
                }
                
                //查询
                res = user.doQuery(null, QueryType.queryTypeStatPlayerPhoneVertify);

                List<PlayerPhoneItem> itemList = (List<PlayerPhoneItem>)user.getQueryResult(QueryType.queryTypeStatPlayerPhoneVertify);

                jd.Add("playerList", BaseJsonSerializer.serialize(itemList));
                str = ItemHelp.genJsonStr(jd);

            }
            else if (p.m_type == 2)  
            {
                p.m_op = Convert.ToInt32(context.Request.Form["isBind"]);
                res = user.doDyop(p, DyOpType.opTypeBindUnbindPhone);
                str = OpResMgr.getInstance().getResultString(res);
            }
            else if (p.m_type == 3) //移除
            {
                p.m_param = context.Request.Form["id"];
                res = user.doDyop(p, DyOpType.opTypeServiceVertifyPhoneNo);
                str = OpResMgr.getInstance().getResultString(res);
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