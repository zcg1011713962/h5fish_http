using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// SignByMonth 的摘要说明
    /// </summary>
    public class SignByMonth : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            RightMgr.getInstance().opCheck(RightDef.DATA_PUMP_DAILY_SIGN, context.Session, context.Response);
            GMUser user = (GMUser)context.Session["user"];
            ParamSignByMonth p = new ParamSignByMonth();
            p.m_op=Convert.ToInt32(context.Request["op"]);
            p.m_year = Convert.ToInt32(context.Request["year"]);
            p.m_month = Convert.ToInt32(context.Request["month"]);
            OpRes res = user.doQuery(p, QueryType.queryTypeSignByMonth);

            string str = "";
            Dictionary<string, object> data = new Dictionary<string, object>();

            paramSignByMonthItem qresult = (paramSignByMonthItem)user.getQueryResult(QueryType.queryTypeSignByMonth);
            data.Add("query", qresult.m_queryData);
            data.Add("cur", qresult.m_curData);
            data.Add("last", qresult.m_lastData);
            str = ItemHelp.genJsonStr(data);

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