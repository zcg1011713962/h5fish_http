using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPlayerIconCustom : System.Web.UI.Page
    {
        private static string[] s_head = new string[] {"玩家头像", "玩家ID", "注册时间","VIP等级","VIP经验"};
        private string[] m_content=new string[s_head.Length];
        private PageGen m_gen = new PageGen(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_PLAYER_ICON_CUSTOM, Session, Response);
            if (!IsPostBack)   //客户端回发而加载
            {
                if (m_gen.parse(Request))
                {
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            OpRes res = user.doQuery(param, QueryType.queryTypePlayerIconCustomStat);
            genTable(m_result, res, user, param);
        }

        //生成表
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
                return;
            }

            List<PlayerIconCustomItem> qresult = (List<PlayerIconCustomItem>)user.getQueryResult(QueryType.queryTypePlayerIconCustomStat);

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
                tr = new TableRow();
                m_result.Rows.Add(tr);

                PlayerIconCustomItem item = qresult[i];
                td = new TableCell();
                tr.Cells.Add(td);
                Image img = new Image();
                img.Attributes.CssStyle.Value = "width:72px;height:72px";
                img.ImageUrl = item.m_iconCustom;
                td.Controls.Add(img);

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_playerId.ToString();
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_createTime;
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_vipLevel.ToString();
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_vipExp.ToString();
                td.Attributes.CssStyle.Value = "vertical-align:middle";

            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatPlayerIconCustom.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}