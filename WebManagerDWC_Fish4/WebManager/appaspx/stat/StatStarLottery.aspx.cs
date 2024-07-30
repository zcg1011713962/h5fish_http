using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace WebManager.appaspx.stat
{
    public partial class StatStarLottery : System.Web.UI.Page
    {
        private static string[] s_head_1 = new string[] { "日期", "等级", "总投注"};
        private static string[] s_head_2 = new string[] { "抽奖次数", "抽奖人数", "对应金币","盈利率" };
        private PageStarLottery m_gen = new PageStarLottery(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAR_LOTTERY, Session, Response);
            if (!IsPostBack)   //客户端回发而加载
            {
                m_level.Items.Add(new ListItem("全部", ""));
                for (int i = 0; i<StrName.s_starLotteryName.Count();i++ )
                {
                    m_level.Items.Add(new ListItem(StrName.s_starLotteryName[i], (i+1).ToString()));
                }
                
                if (m_gen.parse(Request))
                {
                    m_level.SelectedValue = m_gen.m_param;
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
            param.m_param = (m_level.SelectedValue).ToString();
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            OpRes res = user.doQuery(param, QueryType.queryTypeStarLottery);
            genTable(m_result, res, user,param);
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

            ResultStarLottery qresult = (ResultStarLottery)user.getQueryResult(QueryType.queryTypeStarLottery);

            var fields = from f in qresult.m_fields orderby f ascending select f;

            // 生成行标题  表头1 (日期、等级、总投注) +表头2+表头3（抽奖次数、抽奖人次、对应金币、盈利率）
            for (int i = 0; i< s_head_1.Length;i++ )
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head_1[i];
            }
            foreach (var reason in fields)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = qresult.getTitleName(reason);
            }
            for (int i = 0; i < s_head_2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head_2[i];
            }

            //内容
            for (int i = 0; i < qresult.m_result.Count(); i++)
            {
                StarLotteryItem item = qresult.m_result[i];
                tr = new TableRow();
                m_result.Rows.Add(tr);

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time.ToShortDateString();

                td = new TableCell();
                tr.Cells.Add(td);
                string name = StrName.s_starLotteryName[item.m_level-1];
                td.Text = name;

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_totalOutlay.ToString();

                // 生成这个结果
                foreach (var reason in fields)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    if (item.m_gift.ContainsKey(reason))
                    {
                        td.Text = item.m_gift[reason].ToString();
                    }
                    else
                    {
                        td.Text = "0";
                    }
                }

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_lotteryCount.ToString();

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_lotteryNum.ToString();

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_correspondingGold.ToString();

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_earningRate.ToString();
                setColor(td, td.Text);
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatStarLottery.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
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