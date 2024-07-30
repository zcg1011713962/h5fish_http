using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordFeastBasicData : System.Web.UI.Page
    {
        private static string[] s_head_1 = new string[] { "日期", "每日获得100000金币的人数", "每日获得500000金币的人数", "每日获得1000000金币的人数", "每日获得2000000金币的人数", "每日获得3000000金币的人数" };
        private static string[] s_head_2 = new string[] { "日期", "每日领取100000金币的人数", "每日领取500000金币的人数", "每日领取1000000金币的人数", "每日领取2000000金币的人数", "每日领取3000000金币的人数" };
        private static string[] m_content = new string[s_head_1.Length];
        private PageFishlordFeast m_gen = new PageFishlordFeast(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_FISHLORD_FEAST_STAT, Session, Response);
            if (!IsPostBack)
            {
                m_queryType.Items.Add("每日获得相应档位金币人数统计");
                m_queryType.Items.Add("每日领取相应档位金币人数统计");
                if (m_gen.parse(Request))
                {
                    m_queryType.SelectedIndex = m_gen.m_op;
                    m_time.Text = m_gen.m_time;
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_op = m_queryType.SelectedIndex;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            OpRes res = user.doQuery(param, QueryType.queryTypeFishlordFeastStat);
            switch(param.m_op)
            {
                case 0: genTable1(m_result, res, user, s_head_1,param); break;
                case 1: genTable1(m_result, res, user, s_head_2, param); break;
            }
        }
           
        //生成表 捕鱼盛宴活动
        private void genTable1(Table table, OpRes res, GMUser user, string[]s_head, ParamQuery query_param)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                m_page.InnerHtml = "";
                m_foot.InnerHtml = "";
                return;
            }

            List<dailyFishlordFeastItem> qresult = (List<dailyFishlordFeastItem>)user.getQueryResult(QueryType.queryTypeFishlordFeastStat);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                dailyFishlordFeastItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_prob1Count.ToString();
                m_content[f++] = item.m_prob2Count.ToString();
                m_content[f++] = item.m_prob3Count.ToString();
                m_content[f++] = item.m_prob4Count.ToString();
                m_content[f++] = item.m_prob5Count.ToString();
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatFishlordFeastBasicData.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

    }
}