using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text;

namespace WebManager.ashx
{
    /// <summary>
    /// WorldCupMatch 的摘要说明
    /// </summary>
    public class WorldCupMatch : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_WORLD_CUP_MATCH, context.Session, context.Response);

            GMUser user = (GMUser)context.Session["user"];

            WorldCupMatchParam p = new WorldCupMatchParam();
            p.m_op = Convert.ToInt32(context.Request.Form["op"]);
            p.m_id = context.Request.Form["id"];
            p.m_matchId = context.Request.Form["matchId"];
            p.m_matchStartTime = context.Request.Form["matchStartTime"];
            p.m_betEndTime = context.Request.Form["betEndTime"];
            p.m_showTime = context.Request.Form["showTime"];
            p.m_matchNameId = Convert.ToInt32(context.Request.Form["matchName"]);
            p.m_matchType = Convert.ToInt32(context.Request.Form["matchType"]);
            p.m_homeTeamId = Convert.ToInt32(context.Request.Form["homeTeamId"]);
            p.m_visitTeamId = Convert.ToInt32(context.Request.Form["visitTeamId"]);
            p.m_homeScore = Convert.ToInt32(context.Request.Form["homeScore"]);
            p.m_visitScore = Convert.ToInt32(context.Request.Form["visitScore"]);
            p.m_betMaxCount = Convert.ToInt32(context.Request.Form["betMaxCount"]);
            string str = "";
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            if (p.m_op == 3) //查看
            {
                OpRes res = mgr.doQuery(p, QueryType.queryTypeWorldCupMatch, user);
                Dictionary<string, object> data = new Dictionary<string, object>();
                List<WorldCupMatchParam> qresult = (List<WorldCupMatchParam>)mgr.getQueryResult(QueryType.queryTypeWorldCupMatch);
                data.Add("queryList", BaseJsonSerializer.serialize(qresult));
                str = ItemHelp.genJsonStr(data);
            }
            else if (p.m_op == 4)//刷新服务器
            {
                //记录条数>0,服务器禁止刷新
                DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
                user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.WORLD_CUP_MATCH_REWARD, null, dip);

                //时间
                DateTime time1 = Convert.ToDateTime("11:59"), time2 = Convert.ToDateTime("12:03");
                DateTime now = Convert.ToDateTime(DateTime.Now.ToString("hh:mm:ss"));

                if (user.totalRecord > 0)
                {
                    str = "服务器正在结算奖励，暂不能刷新";
                }
                else if (DateTime.Compare(now, time1) >= 0 && DateTime.Compare(time2, now) >= 0)
                {
                    str = "服务器正在结算奖励，暂不能刷新";
                }
                else {
                    OpRes res = flushToGameServer(2, p.m_matchId, user);
                    str = OpResMgr.getInstance().getResultString(res);
                }
            }else  //删除 编辑 新增
            {
                OpRes res = user.doDyop(p, DyOpType.opTypeWorldCupMatch);
                str = OpResMgr.getInstance().getResultString(res);
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(str);
        }

        // 刷新到游戏服务器
        public OpRes flushToGameServer(int p, string id,GMUser user)
        {
            string fmt = string.Format("cmd=6&op={0}&ID={1}", p, id);
            string urlIp = DefCC.HTTP_MONITOR;

            DbServerInfo dbInfo = ResMgr.getInstance().getDbInfo(user.m_dbIP);
            if(dbInfo!=null)
            {
                if (!string.IsNullOrEmpty(dbInfo.m_monitor))
                    urlIp = dbInfo.m_monitor;
            }

            string url = string.Format(urlIp, fmt);
            var ret = HttpPost.Get(new Uri(url));
            if (ret != null)
            {
                string retStr = Encoding.UTF8.GetString(ret);
                if (retStr == "ok")
                    return OpRes.opres_success;
            }
            return OpRes.op_res_failed;
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