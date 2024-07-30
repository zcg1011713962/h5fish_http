using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat.fish
{
    public partial class FishlordBaojinRankDetail : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "玩家ID", "房间类型","参加比赛次数", "比赛1档爆机次数", "比赛2档爆机次数", "比赛3档爆机次数", "比赛4档爆机次数", "比赛5档爆机次数"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);
            string indexStr = Request.QueryString["index"];
            if (string.IsNullOrEmpty(indexStr))
                return;
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_param = Request.QueryString["playerId"];
            OpRes res = user.doQuery(param, QueryType.queryTypeFishlordBaojinRankDetail);
            genTable(m_result, res, user);
        }
        private void genTable(Table table, OpRes res, GMUser user)
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
            List<FishlordBaojinRankItem> qresult = (List<FishlordBaojinRankItem>)user.getQueryResult(QueryType.queryTypeFishlordBaojinRankDetail);
            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            int totalJoinCount = 0;
            int totalBaoji_1 = 0;
            int totalBaoji_2 = 0;
            int totalBaoji_3 = 0;
            int totalBaoji_4 = 0;
            int totalBaoji_5 = 0;

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                FishlordBaojinRankItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_room;
                m_content[f++] = item.m_joinCount.ToString();
                m_content[f++] = item.m_baoji_1.ToString();
                m_content[f++] = item.m_baoji_2.ToString();
                m_content[f++] = item.m_baoji_3.ToString();
                m_content[f++] = item.m_baoji_4.ToString();
                m_content[f++] = item.m_baoji_5.ToString();

                totalJoinCount += item.m_joinCount;
                totalBaoji_1 += item.m_baoji_1;
                totalBaoji_2 += item.m_baoji_2;
                totalBaoji_3 += item.m_baoji_3;
                totalBaoji_4 += item.m_baoji_4;
                totalBaoji_5 += item.m_baoji_5;

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            addStatFoot(m_result,totalJoinCount,totalBaoji_1,totalBaoji_2,totalBaoji_3,totalBaoji_4,totalBaoji_5);
        }

        protected void addStatFoot(Table table, int totalJoinCount, int totalBaoji_1, int totalBaoji_2, int totalBaoji_3, int totalBaoji_4, int totalBaoji_5)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            int f = 0;
            m_content[f++] = "总计";
            m_content[f++] = "";
            m_content[f++] = "";
            m_content[f++] = totalJoinCount.ToString();
            m_content[f++] = totalBaoji_1.ToString();
            m_content[f++] = totalBaoji_2.ToString();
            m_content[f++] = totalBaoji_3.ToString();
            m_content[f++] = totalBaoji_4.ToString();
            m_content[f++] = totalBaoji_5.ToString();

            for (int j = 0; j < s_head.Length; j++)
            {
                TableCell td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        }
    }
}