using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPumpChipFish : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "鱼ID", "系统收入","系统支出","奖励1","奖励2","奖励3","奖励4","奖励5" };
        private string[] m_content=new string[s_head.Length];
        private PagePumpChipFish m_gen = new PagePumpChipFish(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_STAT_PUMP_CHIP_FISH, Session, Response);
            if (!IsPostBack)   //客户端回发而加载
            {
                if (m_gen.parse(Request))
                {
                    m_fishId.Text = m_gen.m_param;
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
            param.m_param = m_fishId.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            OpRes res = user.doQuery(param, QueryType.queryTypePumpChipFishStat);
            genTable(m_result, res, user, param);
        }

        //查询表
        protected void genTable(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }

            List<PumpChipFishStatItem> qresult = (List<PumpChipFishStatItem>)user.getQueryResult(QueryType.queryTypePumpChipFishStat);

            // 生成行标题
            for (int i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            //内容
            for (int i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                PumpChipFishStatItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_fishId.ToString();
                m_content[f++] = item.m_sysIncome.ToString();
                m_content[f++] = item.m_sysOutlay.ToString();
                m_content[f++] = item.m_reward0.ToString();
                m_content[f++] = item.m_reward1.ToString();
                m_content[f++] = item.m_reward2.ToString();
                m_content[f++] = item.m_reward3.ToString();
                m_content[f++] = item.m_reward4.ToString();

                for (int j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatPumpChipFish.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

    }
}