using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    public class RechargePerHour : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.TD_RECHARGE_PER_HOUR, context.Session, context.Response);
            ParamQuery param = new ParamQuery();
            param.m_time = DateTime.Now.Date.AddDays(-1).ToShortDateString() + "-" + DateTime.Now.Date.ToShortDateString();
            param.m_param = context.Request.Form["time"];
            param.m_channelNo=" ";
            if (string.IsNullOrEmpty(param.m_param)) 
            {
                param.m_op = 1;
            }else
            {
                param.m_op = 2;
            }
            GMUser user = (GMUser)context.Session["user"];
            OpRes res = user.doQuery(param, QueryType.queryTypeRechargePerHour);
            Dictionary<string, object> retData = new Dictionary<string, object>();

            if (res == OpRes.opres_success)
            {
                DataEachDay qresult = (DataEachDay)user.getQueryResult(QueryType.queryTypeRechargePerHour);
                var allData = qresult.getData();
                if (param.m_op == 2)
                {
                    int[] m_total = new int[24];
                    DateTime mint = DateTime.Now, maxt = DateTime.Now;   //2017/10/01   2017/10/30
                    bool res1 = Tool.splitTimeStr(param.m_param, ref mint, ref maxt);

                    int day = 0;
                    foreach (var data in allData)
                    {
                        if (data.m_time >= mint && data.m_time <= maxt)
                        {
                            day++;
                            for (int i = 0; i < 24; i++)
                            {
                                m_total[i] += data.m_data[i];
                            }
                        }
                        var s = string.Join<int>(",", data.m_data);
                        if (data.m_time == DateTime.Now.Date || data.m_time == DateTime.Now.Date.AddDays(-1))
                        {
                            retData.Add(data.m_time.ToShortDateString(), s);
                        }
                    }

                    for (int k = 0; k < 24; k++)
                    {
                        m_total[k] = Convert.ToInt32(Math.Round(m_total[k] * 1.0 / day));
                    }

                    var t = string.Join<int>(",", m_total);
                    retData.Add("total", t);
                }
                else   
                {
                    foreach (var data in allData)
                    {
                        var s = string.Join<int>(",", data.m_data);
                        retData.Add(data.m_time.ToShortDateString(), s);
                    }
                }
            }

            string str = ItemHelp.genJsonStr(retData);
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