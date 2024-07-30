using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    public class OperationRank : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_MONEY_RANK, context.Session, context.Response);

            GMUser user = (GMUser)context.Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = context.Request.Form["time"];
            param.m_param = context.Request.Form["rankId"];
            param.m_way = (QueryWay)Convert.ToInt32(context.Request.Form["rankWay"]);
            OpRes res = user.doQuery(param, QueryType.queryTypeCoinGrowthRank);
            string str = "";
            if (res == OpRes.opres_success)
            {
                if ((int)param.m_way >= 2)
                {
                    List<PlayerRankItem> qresult = (List<PlayerRankItem>)user.getQueryResult(null, QueryType.queryTypeCoinGrowthRank);

                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Add("qresult", BaseJsonSerializer.serialize(qresult));
                    str = ItemHelp.genJsonStr(data);
                }
                else
                {
                    ResultRank qresult = (ResultRank)user.getQueryResult(QueryType.queryTypeCoinGrowthRank);
                    str = qresult.getJson(user);
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