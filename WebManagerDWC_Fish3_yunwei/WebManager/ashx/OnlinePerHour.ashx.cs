using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace WebManager.ashx
{
    public class OnlinePerHour : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.TD_ONLINE_PER_HOUR, context.Session, context.Response);
            int op = Convert.ToInt32(context.Request.Form["op"]);
            string time = context.Request.Form["time"];
            string str = "";
            GMUser user = (GMUser)context.Session["user"];
            if (op == 0)
            {
                ParamQuery param = new ParamQuery();
                param.m_way = QueryWay.by_way0 + op;
                param.m_time = time;
                param.m_param = context.Request.Form["param"];
                OpRes res = user.doQuery(param, QueryType.queryTypeOnlinePlayerNumPerHour);
                if (res == OpRes.opres_success)
                {
                    Table table = new Table();
                    TableTdOnlinePlayerNumPerHour view = new TableTdOnlinePlayerNumPerHour();
                    view.genTable(user, table, res);

                    StringWriter sw = new StringWriter();
                    HtmlTextWriter w = new HtmlTextWriter(sw);
                    table.RenderControl(w);
                    str = sw.GetStringBuilder().ToString();
                }
            }
            else
            {
                ParamOnlinePerHour param = new ParamOnlinePerHour();
                param.m_gameId = Convert.ToInt32(context.Request.Form["gameId"]);
                if (string.IsNullOrEmpty(time)) //默认显示今天的在线
                {
                    param.m_time =  DateTime.Now.Date.ToShortDateString();
                }
                else
                {
                    param.m_time = time;
                }
                OpRes res = user.doQuery(param, QueryType.queryTypeOnlinePlayerNumPerHourNew);
                if (res == OpRes.opres_success)
                {
                    List<OutPngItem> qresult =
                       (List<OutPngItem>)user.getQueryResult(QueryType.queryTypeOnlinePlayerNumPerHourNew);
                    str = BaseJsonSerializer.serialize(qresult);
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