using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace WebManager.appaspx.stat.shuihz
{
    public partial class ShuihzTotalEarning : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] { "系统总收入", "系统总支出", "系统盈利率","游戏（648元充值）总计（RMB）" };

        private static string[] s_head2 = new string[] { "时间","系统总收入","系统总支出","系统盈利率","游戏（648元充值）总计（RMB）"};

        private string[] m_content1 = new string[s_head1.Length];
        private string[] m_content2 = new string[s_head2.Length];

        private long m_totalTodayIncome = 0;
        private long m_totalTodayOutlay = 0;
        //private double m_totalTodayRate = 0;
        private long m_totalTodayBuf = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.SHUIHZ_TOTAL_EARNING, Session, Response);
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            OpRes res = user.doQuery(param, QueryType.queryTypeShuihzTotalEarning);
            genTable1(m_result, res, user);
        }

        //查询
        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            OpRes res = user.doQuery(param, QueryType.queryTypeShuihzDailyEarning);
            genTable2(m_resultTotal, res, user);
        }

        //生成表
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

            List<ResultShuihzEarningItem> qresult = (List<ResultShuihzEarningItem>)user.getQueryResult(QueryType.queryTypeShuihzTotalEarning);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head1[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                
                ResultShuihzEarningItem item = qresult[i];
                
                m_content1[f++] = item.m_totalIncome.ToString();
                m_content1[f++] = item.m_totalOutlay.ToString();
                m_content1[f++] = item.getFactExpRate().ToString();
                m_content1[f++] = item.m_gameTotal.ToString();
                
                for (j = 0; j < s_head1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content1[j];
                    if(j==2)
                    {
                        setColor(td, m_content1[j]);
                    }
                }
            }
        }

        //生成表 每日
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

            List<ResultShuihzEarningItem> qresult = (List<ResultShuihzEarningItem>)user.getQueryResult(QueryType.queryTypeShuihzDailyEarning);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head2[i];
            }

            //总计
            TableRow tr1 = new TableRow();
            m_resultTotal.Rows.Add(tr1);

            //日期
            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_resultTotal.Rows.Add(tr);
                
                ResultShuihzEarningItem item = qresult[i];
                //每日盈利率
                m_totalTodayIncome += item.m_totalIncome;
                m_totalTodayOutlay += item.m_totalOutlay;
                m_totalTodayBuf += item.m_gameTotal;

                m_content2[f++] = item.m_time;
                m_content2[f++] = item.m_totalIncome.ToString();
                m_content2[f++] = item.m_totalOutlay.ToString();
                m_content2[f++] = item.getFactExpRate().ToString();
                m_content2[f++] = item.m_gameTotal.ToString();
                
                for (j = 0; j < s_head2.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content2[j];
                    if (j == 3)
                    {
                        setColor(td, m_content2[j]);
                    }
                }
            }

            string[] m_content3 = new string[s_head2.Length];
            int k = 0;
            m_content3[k++] = "总计";
            m_content3[k++] = m_totalTodayIncome.ToString();
            m_content3[k++] = m_totalTodayOutlay.ToString();
            m_content3[k++] = m_totalTodayIncome != 0 ? Math.Round((m_totalTodayIncome - m_totalTodayOutlay) * 1.0 / m_totalTodayIncome, 3).ToString() : "0";
            m_content3[k++] = m_totalTodayBuf.ToString();

            if (qresult.Count != 0)
            {
                for (j = 0; j < s_head2.Length; j++)
                {
                    td = new TableCell();
                    tr1.Cells.Add(td);
                    td.Text = m_content3[j];
                    td.ColumnSpan = 1;
                    if (j == 3)
                    {
                        setColor(td, m_content3[j]);
                    }
                }
            }
            else
            {
                td = new TableCell();
                tr1.Cells.Add(td);
                td.Text = "暂无数据";
                td.ColumnSpan = 6;
            }
       }
       
        protected void setColor(TableCell td, string num)
        {
            if (num[0] == '-')
            {
                td.ForeColor = Color.Red;
            }
            else
            {
                td.ForeColor = Color.Green;
            }
        }

    }
}