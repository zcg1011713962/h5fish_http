using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatJinQiuRechargeLottery : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "礼包名称", "该礼包金额（元）", "购买次数","系统总支出金币","平均金币（总支出/购买次数）"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_JIN_QIU_RECHARGE_LOTTERY, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();

            param.m_time = m_time.Text;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeJinQiuRechargeLottery, user);
            genTable(m_result, res, param, user, mgr);
        }

        //活动排行榜生成查询表
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
            List<JinQiuRechargeLotteryItem> qresult = (List<JinQiuRechargeLotteryItem>)mgr.getQueryResult(QueryType.queryTypeJinQiuRechargeLottery);
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
                JinQiuRechargeLotteryItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.getGiftName();
                m_content[f++] = item.m_price.ToString();
                m_content[f++] = item.m_buyCount.ToString();
                m_content[f++] = item.m_outlayGold.ToString();
                m_content[f++] = item.getAverage(item.m_outlayGold,item.m_buyCount);

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