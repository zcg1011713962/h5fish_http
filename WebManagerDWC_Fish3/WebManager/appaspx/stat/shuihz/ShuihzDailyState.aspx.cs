using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat.shuihz
{
    public partial class ShuihzDailyState : System.Web.UI.Page
    {
        private static string[] s_head = { "日期","今日游戏轮数","出现小玛丽的轮到次数","今日小玛丽总计奖励金币","今日小玛丽平均每轮奖励倍率"};
        private string[] m_content = new string[s_head.Length];
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.SHUIHZ_DAILY_STATE, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            OpRes res = user.doQuery(param, QueryType.queryTypeShuihzDailyState);
            genTable(m_result, res, user);
        }

        //生成表
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

            List<ResultShuihzEarningItem> qresult = (List<ResultShuihzEarningItem>)user.getQueryResult(QueryType.queryTypeShuihzDailyState);

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

                ResultShuihzEarningItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_playCount.ToString();
                m_content[f++] = item.m_bonusTurnCount.ToString();
                m_content[f++] = item.m_rewardGold.ToString();
                m_content[f++]=Convert.ToString(item.m_bonusTurnCount!=0?(Math.Round(item.m_rewardGold*1.0/item.m_bonusTurnCount,3)):0);

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