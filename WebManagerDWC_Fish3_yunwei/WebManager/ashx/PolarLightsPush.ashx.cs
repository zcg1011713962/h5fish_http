using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// PolarLightsPush 的摘要说明
    /// </summary>
    public class PolarLightsPush : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_POLAR_LIGHTS_PUSH, context.Session, context.Response);

            GMUser user = (GMUser)context.Session["user"];

            PolarLightsParam p = new PolarLightsParam();
            p.m_op=Convert.ToInt32(context.Request.Form["op"]);
            p.m_id=context.Request.Form["id"];
            p.m_channelList=context.Request.Form["channelList"];
            p.m_vipList = context.Request.Form["vip"];
            p.m_date=context.Request.Form["date"];
            p.m_weekList=context.Request.Form["weekList"];
            p.m_time=context.Request.Form["time"];
            p.m_content=context.Request.Form["content"];
            p.m_note=context.Request.Form["note"];

            string str = ""; 
            if (p.m_op == 3) //查看
            {
                QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
                OpRes res = mgr.doQuery(p, QueryType.queryTypePolarLightsPush, user);
                Dictionary<string, object> data = new Dictionary<string, object>();
                List<PolarLightsItems> qresult = (List<PolarLightsItems>)mgr.getQueryResult(QueryType.queryTypePolarLightsPush);
                data.Add("queryList", BaseJsonSerializer.serialize(qresult));
                str = ItemHelp.genJsonStr(data);
            }
            else  //删除 编辑 新增
            {
                OpRes res = user.doDyop(p, DyOpType.opTypePolarLightsPush);
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