using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WebManager
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);

            GMUser user = (GMUser)Session["user"];
            if (user != null)
                content.InnerText = user.m_user;

            //查询收入、DAU、ARPU、ARPPU等
            ParamQuery param = new ParamQuery();
            OpRes res = user.doQuery(param, QueryType.queryTypeStatTodayInfo);
            genTable(res, user, param.m_param);
        }

        //生成表
        private void genTable( OpRes res, GMUser user, string playerList)
        {
            List<TodayInfoItem> qresult = (List<TodayInfoItem>)user.getQueryResult(QueryType.queryTypeStatTodayInfo);
            for (int i = 0; i < qresult.Count; i++)
            {
                TodayInfoItem item = qresult[i];
                m_totalIncome.InnerHtml = "今日收入：" + item.m_totalIncome + "元";
                m_dau.InnerHtml = "今日DAU：" + item.m_activeCount + "人";
                m_arpu.InnerHtml = "今日ARPU：" + item.getARPU();
                m_arppu.InnerHtml = "今日ARPPU：" + item.getARPPU();
                m_rechargeRate.InnerHtml = "今日付费率：" + item.getRechargeRate();
                m_register.InnerHtml = "新增注册：" + item.m_rechargeCount + "人";
            }

            //实时在线
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            res = mgr.doQuery(null, QueryType.queryTypeOnlinePlayerCount, user);
            int c = (int)mgr.getQueryResult(QueryType.queryTypeOnlinePlayerCount);
            m_online.InnerHtml = "实时在线：" + c + "人";
        }
    }
}
