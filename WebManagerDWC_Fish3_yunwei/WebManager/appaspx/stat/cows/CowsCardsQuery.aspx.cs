using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace WebManager.appaspx.stat.cows
{
    public partial class CowsCardsQuery : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "牌局ID","输赢位置一览(红赢绿输黑平)","抽水总和","庄家抽水总和","系统支出", "系统收入","玩家支出", "玩家收入",
          "庄家输赢总计","坐庄者","牌局详情","下注玩家列表"};
        private string[] m_content = new string[s_head.Length+4];

        private PageCowsCard m_gen = new PageCowsCard(100);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.COW_CARDS_QUERY, Session, Response);
            if(!IsPostBack)
            {
                m_roomId.Items.Add("金币场");
                m_roomId.Items.Add("龙珠场");

                if (m_gen.parse(Request))
                {
                    m_time.Text = m_gen.m_time;
                    m_cardId.Text = m_gen.m_param;
                    m_playerId.Text = m_gen.m_playerId;
                    m_roomId.SelectedIndex = (m_gen.m_op - 1);
                    onQuery(null, null);
                }
            }
        }
        
        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_param = m_cardId.Text;
            param.m_playerId = m_playerId.Text;
            param.m_op = (m_roomId.SelectedIndex + 1);
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            
            OpRes res = user.doQuery(param, QueryType.queryTypeCowsCardsQuery);
            genTable(m_result, res, user, param);
        }

        //牛牛牌局调整
        private void genTable(Table table, OpRes res, GMUser user, ParamQuery query_param)
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

            List<CowsCardsItem> qresult = (List<CowsCardsItem>)user.getQueryResult(QueryType.queryTypeCowsCardsQuery);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                if (i == 2)
                {
                    td.ColumnSpan = 5;
                }
                else 
                {
                    td.ColumnSpan = 1;
                }
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                CowsCardsItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_cardId.ToString();
                m_content[f++] = "庄";
                m_content[f++] = "东";
                m_content[f++] = "南";
                m_content[f++] = "西";
                m_content[f++] = "北";
                m_content[f++] = item.m_pumpTotal.ToString();
                m_content[f++] = item.m_pumpBankerTotal.ToString();
                m_content[f++] = item.m_sysOutlay.ToString();
                m_content[f++] = item.m_sysIncome.ToString();
                m_content[f++] = item.m_playerOutlay.ToString();
                m_content[f++] = item.m_playerIncome.ToString();
                m_content[f++] = item.m_bankerWinLose.ToString();
                m_content[f++] = item.m_bankerId.ToString();
                m_content[f++] = item.getExParam(i);
                m_content[f++] = item.getExParam1(i);
                for (j = 0; j < s_head.Length+4; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];

                    switch(j)
                    {
                        case 2: setColor(td, item.m_bankerWinLose); break;
                        case 3: setColor(td, item.m_playerWinLose0); break;
                        case 4: setColor(td, item.m_playerWinLose1); break;
                        case 5: setColor(td, item.m_playerWinLose2); break;
                        case 6: setColor(td, item.m_playerWinLose3); break;
                    }
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/cows/CowsCardsQuery.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        protected void setColor(TableCell td, long num)
        {
            if (num >0)
            {
                td.ForeColor = Color.Red;
            }
            else if (num < 0)
            {
                td.ForeColor = Color.Green;
            }
            else 
            {
                td.ForeColor = Color.Black;
            }
        }
    }
}