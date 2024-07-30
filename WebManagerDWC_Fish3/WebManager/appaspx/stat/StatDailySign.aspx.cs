using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatDailySign : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","当日活跃人数","活跃老玩家人数（注册超过7天）","当日总签到数",
           "二档奖励人数","VIP签到人数","获得话费人数","获得VIP经验人数","签到百分比：活跃老玩家/总签到","分享人数","额外金币"};
        private string[] m_content=new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_PUMP_DAILY_SIGN, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_channelNo = "";
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeDailySign, user);
            genTable(m_result, res, param, user, mgr);
        }

        //生成查询表
        private void genTable(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            m_result.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
            TableCell td = null;
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }
            List<dailySignItem> qresult = (List<dailySignItem>)mgr.getQueryResult(QueryType.queryTypeDailySign);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;
                dailySignItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_actCount.ToString();
                m_content[f++] = item.m_oldPlayerLogin.ToString();
                m_content[f++] = item.m_signCount.ToString();

                m_content[f++] = item.m_uplevelSignCount.ToString();
                m_content[f++] = item.m_vipSignCount.ToString();
                m_content[f++] = item.m_getTicketCount.ToString();
                m_content[f++] = item.m_getVipExpCount.ToString();

                m_content[f++] = item.getPercent(item.m_oldPlayerLogin, item.m_signCount);

                m_content[f++] = item.m_shareCount.ToString();
                m_content[f++] = item.m_shareGold.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }
    }
}