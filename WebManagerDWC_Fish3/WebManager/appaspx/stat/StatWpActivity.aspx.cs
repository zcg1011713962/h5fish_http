using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatWpActivity : System.Web.UI.Page
    {
        private static string[] s_head_1 = new string[]{ "日期","每日达到5500的人数", "每日达到6000的人数", "每日达到6500的人数", "每日达到7000的人数", "每日达到7500的人数", 
                                               "每日达到8000的人数", "每日达到8500的人数", "每日达到9000的人数", "每日达到9500的人数", "每日达到10000的人数" };
        private string[] m_content_1 = new string[s_head_1.Length];

        private static string[] s_head_2 = new string[] { "日期", "炮数", "钻石消耗", "极光符文消耗", "浪花符文消耗", "火山符文消耗", "太阳符文消耗" };
        private string[] m_content_2 = new string[s_head_2.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_WP_ACTIVITY_STAT, Session, Response);
            if (!IsPostBack)
            {
                m_queryType.Items.Add("每日达炮数人数统计");
                m_queryType.Items.Add("升级消耗");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_op= m_queryType.SelectedIndex;
            OpRes res = OpRes.op_res_failed;
            if(Convert.ToInt32(param.m_op)==0)
            {
                res = user.doQuery(param, QueryType.queryTypeWpActivityStat);
                genTable1(m_result, res, user);
            }

            if(Convert.ToInt32(param.m_op)==1)
            {
                res = user.doQuery(param, QueryType.queryTypeWpActivityPlayerStat);
                genTable2(m_result, res, user);
            }
        }
        private void genTable1(Table table, OpRes res, GMUser user)
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

            List<dailyReachFireCountPlayerStatItem> qresult = (List<dailyReachFireCountPlayerStatItem>)user.getQueryResult(QueryType.queryTypeWpActivityStat);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head_1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head_1[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                dailyReachFireCountPlayerStatItem item = qresult[i];

                m_content_1[f++] = item.m_time;

                foreach(var d in item.m_fireCountNum)
                {
                    m_content_1[f++] = d.Value.ToString();
                }

                for (j = 0; j < s_head_1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content_1[j];
                }
            }
        }
        private void genTable2(Table table, OpRes res, GMUser user)
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

           // List<dailyFirstReachFireCountPlayerItem> qresult = (List<dailyFirstReachFireCountPlayerItem>)user.getQueryResult(QueryType.queryTypeWpActivityPlayerStat);
            WpItemConsumeResult qresult = (WpItemConsumeResult)user.getQueryResult(QueryType.queryTypeWpActivityPlayerStat);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head_2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head_2[i];
            }

            var allItems = qresult.getAllDescByTime();
            int f = 0;

            foreach (var item in allItems)
            {
                PlayerTypeData<WpConsumeItem> witem = item.Value;

                foreach (var fLevel in witem.m_items)
                {
                    f = 0;
                    tr = new TableRow();
                    m_result.Rows.Add(tr);

                    WpConsumeItem oneItem = fLevel.Value;

                    m_content_2[f++] = item.Key.ToShortDateString(); // 时间

                    Fish_LevelCFGData LevelData = Fish_LevelCFG.getInstance().getValue(fLevel.Key);
                    if (LevelData != null)
                    {
                        m_content_2[f++] = LevelData.m_openRate.ToString();   // 捕鱼等级
                    }
                    else
                    {
                        m_content_2[f++] = fLevel.Key.ToString();
                    }

                    for (int k = 0; k < WpItemConsumeResult.S_ITEM.Length; k++)
                    {
                        m_content_2[f++] = oneItem.getItemCount(WpItemConsumeResult.S_ITEM[k]).ToString();
                    }

                    for (j = 0; j < s_head_2.Length; j++)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = m_content_2[j];
                    }
                }
            }
        }
    }
}