using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx
{
    public partial class WechatRecvRewardStat : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "1日领取人次", "2日领取人次", "3日领取人次", "4日领取人次", "5日领取人次", "6日领取人次", "7日领取人次", "总计","领取微信号总计"};
        private string[] m_content = new string[s_head.Length];
        private PagePlayerPumpBw m_gen = new PagePlayerPumpBw(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";
            RightMgr.getInstance().opCheck(RightDef.OTHER_WECHAT_RECV_STAT, Session, Response);
            if (m_gen.parse(Request))
            {
                m_time.Text = m_gen.m_time;
                onQuery(null, null);
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_countEachPage = m_gen.rowEachPage;
            param.m_curPage = m_gen.curPage;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeWechatRecvRewardStat, user);
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
            List<wechatRecvRewardItem> qresult = (List<wechatRecvRewardItem>)mgr.getQueryResult(QueryType.queryTypeWechatRecvRewardStat);
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
                wechatRecvRewardItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_day1.ToString();
                m_content[f++] = item.m_day2.ToString();
                m_content[f++] = item.m_day3.ToString();
                m_content[f++] = item.m_day4.ToString();
                m_content[f++] = item.m_day5.ToString();
                m_content[f++] = item.m_day6.ToString();
                m_content[f++] = item.m_day7.ToString();
                m_content[f++] = item.m_total.ToString();
                m_content[f++] = item.m_wechatTotal.ToString();
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/WechatRecvRewardStat.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}