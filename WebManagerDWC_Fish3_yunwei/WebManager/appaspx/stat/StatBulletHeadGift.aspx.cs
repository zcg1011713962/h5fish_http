using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatBulletHeadGift : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "弹头礼包3元","弹头礼包30元","弹头礼包198元","弹头礼包648元"};
        private static string[] s_head2 = new string[]{"购买人次", "卖出数量", "送出道具类型","送出道具总数量", "详情" };
        private string[] m_content = new string[s_head.Length];
        private PagePanicBoxPlayer m_gen = new PagePanicBoxPlayer(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_BULLET_HEAD_GIFT, Session, Response);
            if (!IsPostBack)
            {
                if (m_gen.parse(Request))
                {
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
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            OpRes res = user.doQuery(param, QueryType.queryTypeStatBulletHeadGift);
            genTable(m_result, res, user, param);
        }

        private void genTable(Table table, OpRes res, GMUser user, ParamQuery param)
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

            List<StatBulletHeadGiftItem> qresult = (List<StatBulletHeadGiftItem>)user.getQueryResult(QueryType.queryTypeStatBulletHeadGift);
            int i = 0, j = 0;
            // 表头 第一行
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                if (i == 0)
                {
                    td.RowSpan = 2;
                    td.ColumnSpan = 1;
                }
                else {
                    td.RowSpan = 1;
                    td.ColumnSpan = 5;
                }
                td.Attributes.CssStyle.Value = "vertical-align:middle";
            }

            //表头 第二行
            tr = new TableRow();
            table.Rows.Add(tr);
            for (j = 0; j < 4 ; j++ )
            {
                for (i = 0; i < s_head2.Length; i++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = s_head2[i];
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                }
            }

            int[] giftIds = new int[] { 104, 105, 106, 107};
            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);

                StatBulletHeadGiftItem item = qresult[i];

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time;

                foreach (int giftId in giftIds) 
                {
                    if (item.m_data.ContainsKey(giftId))
                    {
                        StatBulletHeadGiftItemList tmp = item.m_data[giftId];

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = tmp.m_rechargeCount.ToString();

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = tmp.m_saleCount.ToString();

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = tmp.getSendItemName();
                        
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = tmp.m_sendItemCount.ToString();

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = tmp.getDetail();
                    }
                    else 
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = "";

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = "";

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = "";

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = "";

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = "";
                    }
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatBulletHeadGift.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

    }
}