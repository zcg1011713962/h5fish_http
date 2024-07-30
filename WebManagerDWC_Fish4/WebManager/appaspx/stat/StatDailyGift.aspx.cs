using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatDailyGift : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] { "日期", "每日6元特惠", "每日30元特惠","每日98元特惠"};
        private static string[] s_head2 = new string[] { "购买人数","额外奖励人数","详情"};
        private string[] m_content = new string[s_head1.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_STAT_DAILY_GIFT, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            OpRes res = user.doQuery(param, QueryType.queryTypeStatDailyGift);
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

            List<StatDailyGiftItem> qresult = (List<StatDailyGiftItem>)user.getQueryResult(QueryType.queryTypeStatDailyGift);

            int i = 0, j = 0, index = 0;
            // 表头 第一行
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head1[i];
                if (i == 0)
                {
                    td.ColumnSpan = 1;
                    td.RowSpan = 2;
                }
                else
                {
                    td.ColumnSpan = 3;
                    td.RowSpan = 1;
                }
                td.Attributes.CssStyle.Value = "vertical-align:middle";
            }

            // 表头 第二行
            tr = new TableRow();
            table.Rows.Add(tr);
            for (j = 0; j < s_head1.Length - 1; j++)
            {
                for (i = 0; i < s_head2.Length; i++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = s_head2[i];
                    td.ColumnSpan = 1;
                    td.RowSpan = 1;
                }
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);

                StatDailyGiftItem item = qresult[i];
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time.ToString();

                foreach (var da in item.m_giftList) 
                {
                    index = da.Key + 29;

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.Value[0].ToString();

                    td = new TableCell();
                    td.Text = da.Value[1].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = item.getDetail(index).ToString();
                }
            }
        }
    }
}