using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatActivityNiuYearFeedBack : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "档位", "奖励", "玩家ID", "中奖码" };
        private string[] m_content = new string[s_head.Length];
        
        private PageJiuQiuNationalActPlayer m_gen = new PageJiuQiuNationalActPlayer(100);

        static StatActivityNiuYearFeedBack()
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_FEED_BACK, Session, Response);

            if (!IsPostBack)
            {
                m_optional.Items.Add(new ListItem("青龙赏", "1"));
                m_optional.Items.Add(new ListItem("白虎赏", "2"));
                m_optional.Items.Add(new ListItem("玄武赏", "3"));
                m_optional.Items.Add(new ListItem("朱雀赏", "4"));
                m_optional.Items.Add(new ListItem("玄牛赏", "5"));
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = Convert.ToInt32(m_optional.SelectedValue);
            param.m_time = m_time.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;

            OpRes res = user.doQuery(param, QueryType.queryNiuYearFeedBack);
            genTableLottery(m_result, res, user, s_head);
        }

        private void genTableLottery(Table table, OpRes res, GMUser user, string []head)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";

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

            int i = 0, k = 0, n = 0;
            for (i = 0; i < head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = head[i];
            }

            List<NiuYearFeedBackInfo> qresult = (List<NiuYearFeedBackInfo>)user.getQueryResult(QueryType.queryNiuYearFeedBack);

            for (i = 0; i < qresult.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                NiuYearFeedBackInfo item = qresult[i];
                m_content[n++] = item.m_genTime;
                m_content[n++] = m_optional.SelectedItem.Text;
                m_content[n++] = "奖励" + item.m_rewardId.ToString();
                m_content[n++] = item.m_playerId.ToString();
                m_content[n++] = item.m_rewardCode;

                for (k = 0; k < head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[k];
                }
            }
        }
    }
}